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
    public class ChatMembersController : ControllerBase
    {
        private readonly IChatMembersService _chatMembersService;

        public ChatMembersController(IChatMembersService chatMembersService)
        {
            _chatMembersService = chatMembersService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ChatMember>>> AddChatMember(ChatMemberDTO chatMemberDto)
        {
            var response = await _chatMembersService.AddMember(chatMemberDto);

            return Ok(response);
        }


        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteChatMember(ChatMemberDTO chatMemberDto)
        {
            var response = await _chatMembersService.DeleteMember(chatMemberDto);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetChatMembers(int chatId)
        {
            var response = await _chatMembersService.GetChatMembersByChatId(chatId);

            return Ok(response);
        }
    }
}
