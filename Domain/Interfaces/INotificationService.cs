using HAC_Pharma.Domain.Entities;

namespace HAC_Pharma.Domain.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetNotificationsAsync(string? userId = null, int count = 50);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string? userId = null);
    Task<Notification> CreateNotificationAsync(Notification notification);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllAsReadAsync(string? userId = null);
    Task BroadcastNotificationAsync(string title, string message, string type = "system", string? link = null);
    Task SendNotificationToUserAsync(string userId, string title, string message, string type = "system", string? link = null);
}
