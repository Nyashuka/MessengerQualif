using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using System.Text;

namespace NotificationService.Controllers
{
    public class WebSocketTestController : ControllerBase
    {
        [HttpGet("/ws")]
        public async Task Get([FromHeader]string username)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                //HttpContext.Request.Headers["username"];

                using (var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync())
                {
                    await Spam(webSocket, username);
                }

            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Spam(WebSocket webSocket, string name)
        {
            var buffer = Encoding.UTF8.GetBytes(name);
            var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            while (webSocket.State == WebSocketState.Open)
            {
                await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);

                await Task.Delay(2000);
            }
        }
    }
}
