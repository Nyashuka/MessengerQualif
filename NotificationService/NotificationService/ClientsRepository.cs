using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace NotificationService
{
    public static class ClientsRepository
    {
        public static ConcurrentDictionary<int, WebSocket> Clients { get; set; } = new ConcurrentDictionary<int, WebSocket>();
    }
}
