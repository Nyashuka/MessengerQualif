using System.Net.WebSockets;

namespace NotificationService.Services.Interfaces
{
    public interface IClientsService
    {
        void AddClient(int userId, WebSocket client);
        Task SendMessageForUserAsync(string message, int userId);
        Task HandleWebSocket(int userId, WebSocket webSocket);
    }
}
