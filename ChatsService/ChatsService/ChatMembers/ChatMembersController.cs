using ChatsService.ActionAccess.Actions;
using ChatsService.ActionAccess.Services;
using ChatsService.Authorization;
using ChatsService.ChatMembers.Dto;
using ChatsService.ChatMembers.Services;
using ChatsService.Groups.Services;
using ChatsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatsService.ChatMembers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMembersController : ControllerBase
    {
        private readonly IChatMembersService _chatMembersService;
        private readonly IAuthService _authService;
        private readonly IActionAccessService _actionAccessService;

        public ChatMembersController(IChatMembersService chatMembersService, IAuthService authService, IActionAccessService actionAccessService)
        {
            _chatMembersService = chatMembersService;
            _authService = authService;
            _actionAccessService = actionAccessService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetAllChatUsers(string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if(authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            if(!(await _actionAccessService.HasAccess(new GetChatMembersActionAccess(), chatId, authData.Data.UserId)))
            {
                return Unauthorized();
            }

            var response = _chatMembersService.GetChatMembersByChatId(chatId);

            return Ok(response);
        }


    }
}
