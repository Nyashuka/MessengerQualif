using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IAuthService _authService;

        public NotificationController(IAuthService authService)
        {
            _authService = authService;
            //Clients = new Dictionary<int, WebSocket>();
            //_ = Spam();
        }

        [HttpGet("connect")]
        public async Task Connect([FromHeader] string accessToken)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
                return;

            int userId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (userId == -1)
                return;

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            
            var buffer = Encoding.UTF8.GetBytes($"Connected: {userId}");
            var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

            NotificationsService.Clients[userId] = webSocket;

            await HandleWebSocket(webSocket, userId);
        }

        private async Task HandleWebSocket(WebSocket webSocket, int userId)
        {
            var buffer = new byte[1024 * 4]; // Розмір буфера
            var segment = new ArraySegment<byte>(buffer);

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    // Читаємо повідомлення з WebSocket
                    var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

                    if (result.CloseStatus.HasValue)
                    {
                        // Клієнт закриває WebSocket
                        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    }
                    else
                    {
                        // Обробляємо отримані дані тут
                        string receivedMessage = Encoding.UTF8.GetString(segment.Array, 0, result.Count);
                        // Можливо, ви хочете відправити відповідь або інші дії

                        // Якщо ви хочете надіслати відповідь, використовуйте SendAsync
                        // Наприклад:
                        var responseMessage = "Ваше повідомлення отримано!";
                        var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
                        var responseSegment = new ArraySegment<byte>(responseBuffer);
                        await webSocket.SendAsync(responseSegment, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок, якщо необхідно
                Console.WriteLine($"Помилка в обробці WebSocket для користувача {userId}: {ex.Message}");
            }
            finally
            {
                // Закриваємо WebSocket після закриття з'єднання
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Закриття WebSocket з'єднання", CancellationToken.None);
                }

                // Видаляємо клієнта з сервісу клієнтів
                if (NotificationsService.Clients.ContainsKey(userId))
                {
                    NotificationsService.Clients.Remove(userId);
                }
            }
        }
    }
}

