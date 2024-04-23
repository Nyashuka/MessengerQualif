using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using NotificationService.Data;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class MessageNotifier
    {
        public async Task NotifyUsers(List<WebSocket> users, ChatMessage message)
        {
            string jsonMessage = JsonSerializer.Serialize(message);
            var responseObject = new SocketResponse()
            {
                ResponseType = SocketResponseType.TextMessage,
                JsonData = jsonMessage
            };

            string response = JsonSerializer.Serialize(responseObject);

            var buffer = Encoding.UTF8.GetBytes(response);
            var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            foreach (var user in users)
            {
                await user.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
