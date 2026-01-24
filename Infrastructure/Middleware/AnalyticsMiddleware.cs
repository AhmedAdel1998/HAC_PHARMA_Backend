using System.Security.Claims;
using HAC_Pharma.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HAC_Pharma.Infrastructure.Middleware;

public class AnalyticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;

    public AnalyticsMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture details before processing request to avoid context disposal issues
        var path = context.Request.Path.Value ?? "";
        
        // Exclude API calls, static files, and admin pages from tracking
        if (!string.IsNullOrEmpty(path) && 
            !path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) && 
            !path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) &&
            !path.Contains("."))
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var referrer = context.Request.Headers["Referer"].ToString();

            // Fire and forget tracking SAFELY using a new scope
            _ = Task.Run(async () => 
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var analyticsService = scope.ServiceProvider.GetRequiredService<IAnalyticsService>();
                    try
                    {
                        await analyticsService.TrackPageViewAsync(path, null, userAgent, ipAddress, referrer, null, userId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Analytics Background Error]: {ex.Message}");
                    }
                }
            });
        }

        await _next(context);
    }
}
