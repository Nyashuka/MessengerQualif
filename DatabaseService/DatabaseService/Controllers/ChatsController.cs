using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatsController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("get-chat-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatById(int chatId)
        {
            var response = await _chatService.GetChatById(chatId);

            return Ok(response);
        }

        [HttpGet("get-chat-by-message-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatByMessageId(int messageId)
        {
            var response = await _chatService.GetChatByMessageId(messageId);

            return Ok(response);
        }

        [HttpGet("get-chat-by-role-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatByRoleId(int roleId)
        {
            var response = await _chatService.GetChatByRoleId(roleId);

            return Ok(response);
        }

        [HttpGet("get-personal")]
        public async Task<ActionResult<ServiceResponse<List<ChatDto>>>> GetAllPersonalChats(int userId)
        {
            var response = await _chatService.GetAllPersonalChats(userId);

            return Ok(response);
        }

        [HttpPost("get-personal")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetPersonalChat(List<UserDto> users)
        {
            var response = await _chatService.GetPersonalChatIfExists(users);

            return Ok(response);
        }

        [HttpPost("create-personal")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreateChat(ChatDto chatDto)
        {
            var response = await _chatService.CreatePersonalChatIfNotExists(chatDto);

            return Ok(response);
        }

        [HttpGet("delete")]
        public async Task<ActionResult> DeleteChat(int chatId)
        {
            var response = await _chatService.DeleteChat(chatId);

            return Ok(response);
        }

        
    }
}
