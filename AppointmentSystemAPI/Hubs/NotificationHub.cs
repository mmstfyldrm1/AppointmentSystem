using Microsoft.AspNetCore.SignalR;

namespace AppointmentSystemAPI.Hubs
{
    public class NotificationHub:Hub
    {
        public async Task JoinGroup(string workerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, workerId);
        }
    }
}
