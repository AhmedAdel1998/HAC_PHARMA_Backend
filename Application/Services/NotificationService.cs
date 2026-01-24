using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Domain.Interfaces;
using HAC_Pharma.Infrastructure.Data;
using HAC_Pharma.Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HAC_Pharma.Application.Services;

public class NotificationService : INotificationService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public async Task<IEnumerable<Notification>> GetNotificationsAsync(string? userId = null, int count = 50)
    {
        var query = _context.Notifications.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(n => n.UserId == userId || n.UserId == null);
        }
        else
        {
            query = query.Where(n => n.UserId == null);
        }

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string? userId = null)
    {
        var query = _context.Notifications.Where(n => !n.IsRead);

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(n => n.UserId == userId || n.UserId == null);
        }
        else
        {
            query = query.Where(n => n.UserId == null);
        }

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        // Send to SignalR
        if (string.IsNullOrEmpty(notification.UserId))
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);
        }
        else
        {
            await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification);
        }

        return notification;
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _context.Notifications.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(string? userId = null)
    {
        var query = _context.Notifications.Where(n => !n.IsRead);

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(n => n.UserId == userId || n.UserId == null);
        }
        else
        {
            query = query.Where(n => n.UserId == null);
        }

        var notifications = await query.ToListAsync();
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task BroadcastNotificationAsync(string title, string message, string type = "system", string? link = null)
    {
        var notification = new Notification
        {
            Title = title,
            Message = message,
            Type = type,
            Link = link,
            CreatedAt = DateTime.UtcNow
        };

        await CreateNotificationAsync(notification);
    }

    public async Task SendNotificationToUserAsync(string userId, string title, string message, string type = "system", string? link = null)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            Link = link,
            CreatedAt = DateTime.UtcNow
        };

        await CreateNotificationAsync(notification);
    }
}
