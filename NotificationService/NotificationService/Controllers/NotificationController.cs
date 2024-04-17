using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IAuthService _authService;
        private IClientsService _clientsService;

        public NotificationController(IAuthService authService, IClientsService clientsService)
        {
            _authService = authService;
            _clientsService = clientsService;
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
            
            _clientsService.AddClient(userId, webSocket);

            //var buffer = Encoding.UTF8.GetBytes($"Connected: {userId}");
            //var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            //await client.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            await _clientsService.HandleWebSocket(userId, webSocket);
            
        }

        [HttpGet("send")]
        public async Task SendMessage(string message, int userId)
        {
            await _clientsService.SendMessageForUserAsync(message, userId);
        }

        //public async Task HandleWebSocket(int userId, WebSocket webSocket)
        //{
        //    var buffer = new byte[1024];
        //    var segment = new ArraySegment<byte>(buffer);

        //    try
        //    {
        //        while (webSocket.State == WebSocketState.Open)
        //        {
        //            var result = await webSocket.ReceiveAsync(segment, CancellationToken.None);

        //            if (result.CloseStatus.HasValue)
        //            {
        //                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        //            }
        //            else
        //            {
        //                string receivedMessage = Encoding.UTF8.GetString(segment.Array, 0, result.Count);

        //                var responseMessage = "Received!";
        //                var responseBuffer = Encoding.UTF8.GetBytes(responseMessage);
        //                var responseSegment = new ArraySegment<byte>(responseBuffer);
        //                await webSocket.SendAsync(responseSegment, WebSocketMessageType.Text, true, CancellationToken.None);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {userId}: {ex.Message}");
        //    }
        //    finally
        //    {
        //        if (webSocket.State == WebSocketState.Open)
        //        {
        //            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing WebSocket connection", CancellationToken.None);
        //        }

        //        if (ClientsRepository.Clients.ContainsKey(userId))
        //        {
        //            ClientsRepository.Clients.TryRemove(userId, out var removedClient);
        //        }
        //    }
        //}
    }
}

