﻿using MessagesService.ActionAccess.Actions;
using MessagesService.ActionAccess.Data;
using MessagesService.ActionAccess.Services;
using MessagesService.DTOs;
using MessagesService.Models.Requests;
using MessagesService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        [HttpGet("get-by-id")]
        public async Task<ActionResult<ServiceResponse<MessageDto>>> GetMessageById(string accessToken, int messageId)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            var response = await _messageService.GetMessageById(messageId);

            if (!await _chatService.IsUserChatMember(response.Data.ChatId, senderId))
                return new ServiceResponse<MessageDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "You don't have access to this chat"
                };

            return response;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<MessageDto>>>> GetChatMessagesByChatId([FromQuery] string accessToken, int chatId)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            if (!await _chatService.IsUserChatMember(chatId, senderId))
                return new ServiceResponse<List<MessageDto>>()
                {
                    Data = null,
                    Success = false,
                    Message = "You don't have access to this chat"
                };

            var messagesResponse = await _messageService.GetAllChatMessages(chatId);

            return Ok(messagesResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<MessageDto>>> SendMessage([FromQuery] string accessToken, [FromQuery] string clientGuid, ClientMessageDTO clientMessageDTO)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            var chat = await _chatService.GetChatById(clientMessageDTO.ChatId);

            if (chat.Data.ChatTypeId == 1)
            {
                var data = new SendMessageChatActionData()
                {
                    RequesterId = senderId,
                    ChatId = clientMessageDTO.ChatId,
                };
                if (!await _actionAccessService.HasAccess(new SendTextMessageChatAction(data), clientMessageDTO.ChatId, senderId))
                {
                    return Ok(new ServiceResponse<MessageDto>()
                    {
                        Success = false,
                        Message = "You do not have acces to send text messages"
                    });
                }
            }

            if (!await _chatService.IsUserChatMember(chat.Data.Id, senderId))
                return new ServiceResponse<MessageDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "You don't have access to this chat"
                };

            var response = await _messageService.SendMessage(senderId, accessToken, clientGuid, clientMessageDTO);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteChatMessage([FromQuery] string accessToken, [FromQuery] string clientGuid, int messageId)
        {
            int senderId = await _authService.TryGetAuthenticatedUser(accessToken);

            if (senderId == -1) return Unauthorized();

            var messagesResponse = await _messageService.DeleteMessage(senderId, accessToken, clientGuid, messageId);

            return Ok(messagesResponse);
        }
    }
}
