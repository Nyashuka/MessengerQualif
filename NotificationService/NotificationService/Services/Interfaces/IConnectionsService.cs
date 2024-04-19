using System.Net.WebSockets;

namespace NotificationService.Services.Interfaces
{
    public interface IConnectionsService
    {
        Task AddClientAndStartReceiving(int userId, WebSocket client);
        Task SendMessageForUserAsync(string message, int userId);
        Task HandleWebSocket(int userId, WebSocket webSocket);
        List<WebSocket> GetActiveConnections(List<int> dataUsers);
    }
}
