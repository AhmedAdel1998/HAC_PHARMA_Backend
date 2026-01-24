using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HAC_Pharma.Infrastructure.Data;
using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Application.Hubs;
using System.IdentityModel.Tokens.Jwt;

// Disable automatic claim mapping (keeps "role" as "role" instead of mapping to long URI)
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("HAC_Pharma")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        NameClaimType = "name"
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated successfully for user: " + context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

// Configure SignalR
builder.Services.AddSignalR();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200", 
            "https://localhost:4200", 
            "http://localhost:3000",
            "https://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Register CMS Services
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IAuthService, HAC_Pharma.Application.Services.AuthService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IContentService, HAC_Pharma.Application.Services.ContentService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IProductService, HAC_Pharma.Application.Services.ProductService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IMediaService, HAC_Pharma.Application.Services.MediaService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IUserService, HAC_Pharma.Application.Services.UserService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IContactService, HAC_Pharma.Application.Services.ContactService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IJobService, HAC_Pharma.Application.Services.JobService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IEventService, HAC_Pharma.Application.Services.EventService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.IAnalyticsService, HAC_Pharma.Application.Services.AnalyticsService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.ISettingsService, HAC_Pharma.Application.Services.SettingsService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.ITranslationService, HAC_Pharma.Application.Services.TranslationService>();
builder.Services.AddScoped<HAC_Pharma.Domain.Interfaces.INotificationService, HAC_Pharma.Application.Services.NotificationService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "HAC Pharma API",
        Version = "v1",
        Description = "CMS API for HAC Pharma pharmaceutical management system",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "HAC Pharma",
            Email = "support@hacpharma.com"
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Apply pending migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

// Seed database (create roles and admin user)
await HAC_Pharma.Infrastructure.Data.DbSeeder.SeedAsync(app.Services);

// Seed translations from JSON files
await HAC_Pharma.Infrastructure.Data.TranslationSeeder.SeedAsync(app.Services, app.Environment);

// Configure the HTTP request pipeline - Enable Swagger for ALL environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HAC Pharma API V1");
    c.RoutePrefix = "swagger"; // Access Swagger at /swagger
});

app.UseHttpsRedirection();

// Serve static files (for uploads)
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
    Directory.CreateDirectory(uploadsPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Use CORS - Must be before Authentication/Authorization
app.UseCors("AllowAll");

// Debugging Middleware: Log headers
app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
    Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");
    if (!string.IsNullOrEmpty(authHeader))
    {
        Console.WriteLine($"   Auth Header found: {authHeader.Substring(0, Math.Min(20, authHeader.Length))}...");
    }
    else
    {
        Console.WriteLine("   ⚠️ No Authorization Header found!");
    }
    await next();
});

// Authentication & Authorization
app.UseAuthentication();

// Debugging Middleware: Log User Claims after Authentication
app.Use(async (context, next) =>
{
    Console.WriteLine($"[AuthDebug] User Authenticated: {context.User.Identity?.IsAuthenticated}");
    if (context.User.Identity?.IsAuthenticated == true)
    {
        Console.WriteLine($"[AuthDebug] Name: {context.User.Identity.Name}");
        foreach (var claim in context.User.Claims)
        {
            Console.WriteLine($"[AuthDebug] Claim: {claim.Type} = {claim.Value}");
        }
    }
    await next();
});

app.UseAuthorization();

// Track page views
app.UseMiddleware<HAC_Pharma.Infrastructure.Middleware.AnalyticsMiddleware>();

app.MapControllers();

// Map SignalR Hubs
app.MapHub<NotificationHub>("/hubs/notifications");
app.MapHub<DataHub>("/hubs/data");
app.MapHub<MonitoringHub>("/hubs/monitoring");

app.Run();
