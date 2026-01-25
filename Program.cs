using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HAC_Pharma.Infrastructure.Data;
using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Application.Hubs;


using System.IdentityModel.Tokens.Jwt; // Added for JwtSecurityTokenHandler

// Disable automatic claim mapping (keeps "role" as "role" instead of mapping to long URI)
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Check for Railway environment variables
var dbHost = Environment.GetEnvironmentVariable("MYSQLHOST");
if (!string.IsNullOrEmpty(dbHost))
{
    var dbUser = Environment.GetEnvironmentVariable("MYSQLUSER") ?? "root";
    var dbPassword = Environment.GetEnvironmentVariable("MYSQLPASSWORD") ?? "";
    var dbPort = Environment.GetEnvironmentVariable("MYSQLPORT") ?? "3306";
    var dbName = Environment.GetEnvironmentVariable("MYSQLDATABASE") ?? "railway"; // Use provided database name or fallback

    connectionString = $"server={dbHost};port={dbPort};user={dbUser};password={dbPassword};database={dbName}";
}


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    try 
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            b => b.MigrationsAssembly("HAC_Pharma").EnableRetryOnFailure());
    }
    catch (System.Exception)
    {
        // Fallback when DB is not reachable
        options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2)),
            b => b.MigrationsAssembly("HAC_Pharma").EnableRetryOnFailure());
    }
});

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
        ClockSkew = TimeSpan.Zero
    };
});

// ... (Rest of configuration)

var app = builder.Build();

// Apply pending migrations and Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try 
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }
        
        // Seed database (create roles and admin user)
        await HAC_Pharma.Infrastructure.Data.DbSeeder.SeedAsync(app.Services);

        // Seed translations from JSON files
        await HAC_Pharma.Infrastructure.Data.TranslationSeeder.SeedAsync(app.Services, app.Environment);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while initializing the database. Ensure MySQL is running and accessible.");
    }
}

// Configure the HTTP request pipeline.
    // app.MapOpenApi(); // Optional: Keep or remove depending on preference, identifying mostly with .NET 9 features
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HAC Pharma API V1");
        c.RoutePrefix = string.Empty; // Swagger at root
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

// Use CORS
app.UseCors("AllowAll");

// Authentication & Authorization
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
