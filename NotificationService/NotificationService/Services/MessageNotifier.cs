using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using NotificationService.Data;
using NotificationService.DTOs;
using NotificationService.Models;

namespace NotificationService.Services
{
    public class MessageNotifier
    {
        public async Task NotifySendMessage(List<WebSocket> users, ChatMessage message)
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

        public async Task NotifyDeletedMessage(List<WebSocket> users, DeletedMessageDto deletedMessageDto)
        {
            string jsonMessage = JsonSerializer.Serialize(deletedMessageDto);
            var responseObject = new SocketResponse()
            {
                ResponseType = SocketResponseType.DeletedMessage,
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
