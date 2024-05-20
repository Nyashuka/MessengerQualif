using MessagesService.ActionAccess.Actions;
using MessagesService.ActionAccess.Data;
using MessagesService.ActionAccess.Services;
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
        private readonly IActionAccessService _actionAccessService;

        public MessagesController(IAuthService authService, IChatService chatService, IMessageService messageService, IActionAccessService actionAccessService)
        {
            _authService = authService;
            _chatService = chatService;
            _messageService = messageService;
            _actionAccessService = actionAccessService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<MessageDto>>> SendMessage([FromQuery] string accessToken, ClientMessageDTO clientMessageDTO)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            var chat = await _chatService.GetChat(clientMessageDTO.ChatId);
            
            if(chat.Data.ChatTypeId == 1)
            {
                var data = new SendMessageChatActionData()
                {
                    RequesterId = senderId,
                    ChatId = clientMessageDTO.ChatId,
                };
                if(!await _actionAccessService.HasAccess(new SendTextMessageChatAction(data), clientMessageDTO.ChatId, senderId))
                {
                    return Ok(new ServiceResponse<MessageDto>()
                    {
                        Success = false,
                        Message = "You do not have acces to send text messages"
                    });
                }
            }

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
