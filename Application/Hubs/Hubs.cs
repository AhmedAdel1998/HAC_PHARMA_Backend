using Microsoft.AspNetCore.SignalR;

namespace HAC_Pharma.Application.Hubs;

/// <summary>
/// SignalR hub for notification broadcasting
/// </summary>
public class NotificationHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task SendToUser(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
    }
}

/// <summary>
/// SignalR hub for real-time data updates
/// </summary>
public class DataHub : Hub
{
    public async Task SubscribeToEntity(string entityType, int entityId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"{entityType}-{entityId}");
    }

    public async Task UnsubscribeFromEntity(string entityType, int entityId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{entityType}-{entityId}");
    }

    public async Task BroadcastUpdate(string entityType, object data)
    {
        await Clients.All.SendAsync($"{entityType}Updated", data);
    }
}

/// <summary>
/// SignalR hub for monitoring and alerts
/// </summary>
public class MonitoringHub : Hub
{
    public async Task SubscribeToWarehouse(int warehouseId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Warehouse-{warehouseId}");
    }

    public async Task UnsubscribeFromWarehouse(int warehouseId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Warehouse-{warehouseId}");
    }

    public async Task SendTemperatureAlert(int warehouseId, string message, object data)
    {
        await Clients.Group($"Warehouse-{warehouseId}").SendAsync("TemperatureAlert", message, data);
    }

    public async Task SendSystemAlert(string alertType, object data)
    {
        await Clients.All.SendAsync("SystemAlert", alertType, data);
    }
}
