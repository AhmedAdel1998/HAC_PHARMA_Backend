using HAC_Pharma.Domain.Entities;
using HAC_Pharma.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HAC_Pharma.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications([FromQuery] int count = 50)
    {
        var userId = User.Identity?.Name; // Or use ClaimTypes.NameIdentifier
        var notifications = await _notificationService.GetNotificationsAsync(userId, count);
        return Ok(notifications);
    }

    [HttpGet("unread")]
    public async Task<ActionResult<IEnumerable<Notification>>> GetUnreadNotifications()
    {
        var userId = User.Identity?.Name;
        var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
        return Ok(notifications);
    }

    [HttpPost("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpPost("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.Identity?.Name;
        await _notificationService.MarkAllAsReadAsync(userId);
        return Ok();
    }

    // Temporary endpoint for testing
    [HttpPost("test-send")]
    public async Task<IActionResult> TestSendNotification([FromBody] TestNotificationRequest request)
    {
        await _notificationService.BroadcastNotificationAsync(request.Title, request.Message, request.Type, request.Link);
        return Ok();
    }
}

public class TestNotificationRequest
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "system";
    public string? Link { get; set; }
}
