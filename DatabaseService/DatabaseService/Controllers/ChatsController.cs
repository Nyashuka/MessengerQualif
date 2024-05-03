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
        private readonly IChatMembersService _chatMembersService;

        public ChatsController(IChatService chatService, IChatMembersService chatMembersService)
        {
            _chatService = chatService;
            _chatMembersService = chatMembersService;
        }

        [HttpGet("get-chat-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatById(int chatId)
        {
            var response = await _chatService.GetChatById(chatId);

            return Ok(response);
        }

        [HttpGet("get-chat-members")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatMembers(int chatId)
        {
            var response = await _chatMembersService.GetChatMembersByChatId(chatId);

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

        [HttpPost("create-group")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreateGroup(ChatDto chatDto)
        {
            var response = await _chatService.CreateGroupChat(chatDto);

            return Ok(response);
        }

        [HttpGet("delete")]
        public async Task<ActionResult> DeleteChat(int chatId)
        {
            var response = await _chatService.DeleteChat(chatId);

            return Ok(response);
        }

        [HttpPost("add-member")]
        public async Task<ActionResult> AddChatMember(ChatMemberDTO chatMemberDto)
        {
            var response = await _chatMembersService.AddMember(chatMemberDto);

            return Ok(response);
        }


        [HttpPost("delete-member")]
        public async Task<ActionResult> DeleteChatMember(ChatMemberDTO chatMemberDto)
        {
            var response = await _chatMembersService.DeleteMember(chatMemberDto);

            return Ok(response);
        }
    }
}
