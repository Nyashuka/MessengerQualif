using System.Net.WebSockets;
using System.Text;

namespace NotificationService.Services
{
    public static class NotificationsService
    {
        public static Dictionary<int, WebSocket> Clients { get; set; } = new Dictionary<int, WebSocket>();
    }
}
