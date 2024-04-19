using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("create")]
        public async Task<ActionResult> CreateChat(ChatDTO chatDto)
        {
            var response = await _chatService.CreateChat(chatDto);

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
