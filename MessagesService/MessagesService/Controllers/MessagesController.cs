using MessagesService.DTOs;
using MessagesService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessagesService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;

        public MessagesController(IAuthService authService, IChatService chatService, IMessageService messageService) 
        {
            _authService = authService;
            _chatService = chatService;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult> SendMessage([FromQuery] string accessToken, ClientMessageDTO clientMessageDTO)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            //if(!await _chatService.IsUserChatMember(senderId)) return Unauthorized();

            await _messageService.HandleMessage(senderId, accessToken, clientMessageDTO);

            return Ok();
        }
    }
}
