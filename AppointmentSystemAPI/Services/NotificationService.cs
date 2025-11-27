using AppointmentSystemAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AppointmentSystemAPI.Services
{
    public class NotificationService:Hub
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToWorker(string workerId, string message)
        {
            await _hubContext.Clients.Group(workerId)
                .SendAsync("ReceiveNotification", message);
        }
    }
}
