using System.Net.WebSockets;

namespace NotificationService.Services.Interfaces
{
    public interface IConnectionsService
    {
        Task AddClientAndStartReceiving(int userId, Guid clientGuid, WebSocket client);
        Task SendMessageForUserAsync(string message, int userId);
        Task HandleWebSocket(int userId, Guid clientGuid, WebSocket webSocket);
        List<WebSocket> GetActiveConnections(List<int> dataUsers, int senderId, Guid clientGuid);
    }
}
