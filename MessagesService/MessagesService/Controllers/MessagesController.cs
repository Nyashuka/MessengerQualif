using MessagesService.DTOs;
using MessagesService.Models.Requests;
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
        public async Task<ActionResult<ServiceResponse<MessageDto>>> SendMessage([FromQuery] string accessToken, ClientMessageDTO clientMessageDTO)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            //if(!await _chatService.IsUserChatMember(senderId)) return Unauthorized();

            var response = await _messageService.HandleMessage(senderId, accessToken, clientMessageDTO);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<MessageDto>>>> GetChatMessagesByChatId([FromQuery] string accessToken, int chatId)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            //if(!await _chatService.IsUserChatMember(senderId)) return Unauthorized();

            var messagesResponse = await _messageService.GetAllChatMessages(chatId);

            return Ok(messagesResponse);
        }
    }
}
