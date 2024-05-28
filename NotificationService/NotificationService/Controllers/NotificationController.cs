using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;
using NotificationService.DTOs;
using NotificationService.Services;
using NotificationService.Models;

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
        public async Task Connect([FromHeader] string accessToken, [FromHeader] string clientGuid)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
                return;

            int userId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (userId == -1)
                return;

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            
            await _connectionsService.AddClientAndStartReceiving(userId, new Guid(clientGuid), webSocket);            
        }

        [HttpPost("notify")]
        public async Task<ActionResult<ServiceResponse<bool>>> NotifyUsers([FromQuery]string accessToken, [FromQuery] string clientGuid, NotifyDataDto data)
        {
            int userId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (userId == -1)
                return Unauthorized();

            if (data.Message == null)
            {
                return BadRequest();
            }

            var activeConnections = _connectionsService.GetActiveConnections(data.Recipients, userId, new Guid(clientGuid));

            await _messageNotifier.NotifySendMessage(activeConnections, data.Message);

            return Ok(new ServiceResponse<bool>() { Data = true });
        }

        [HttpPost("notify-delete-message")]
        public async Task<ActionResult<ServiceResponse<bool>>> NotifyDeleteMessage([FromQuery] string accessToken, [FromQuery] string clientGuid, NotifyDeleteMessageDto data)
        {
            int userId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (userId == -1)
                return Unauthorized();

            var activeConnections = _connectionsService.GetActiveConnections(data.Recipients, userId, new Guid(clientGuid));

            var deletedMessageData = new DeletedMessageDto()
            {
                ChatId = data.ChatId,
                MessageId = data.MessageId,
            };

            await _messageNotifier.NotifyDeletedMessage(activeConnections, deletedMessageData);

            return Ok(new ServiceResponse<bool>() { Data = true });
        }
    }
}

