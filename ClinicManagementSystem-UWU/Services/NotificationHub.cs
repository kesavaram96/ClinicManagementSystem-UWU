using Microsoft.AspNetCore.SignalR;

namespace ClinicManagementSystem_UWU.Services
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", new { Message = message });
        }
    
    }
}
