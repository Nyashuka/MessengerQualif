using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;
using NotificationService.DTOs;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConnectionsService _connectionsService;
        private readonly MessageNotifier _messageNotifier;

        public NotificationController(IAuthService authService, IConnectionsService connectionsService)
        {
            _authService = authService;
            _connectionsService = connectionsService;
            _messageNotifier = new MessageNotifier();
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
            
            await _connectionsService.AddClientAndStartReceiving(userId, webSocket);

            //var buffer = Encoding.UTF8.GetBytes($"Connected: {userId}");
            //var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);

            //await client.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            
        }

        [HttpGet("notify")]
        public async Task<ActionResult> NotifyUsers(MessageSendingData data)
        {
            var activeConnections = _connectionsService.GetActiveConnections(data.Users);

            if (data.Message == null)
            {
                return BadRequest();
            }

            await _messageNotifier.NotifyUsers(activeConnections, data.Message);

            return Ok();
        }
    }
}

