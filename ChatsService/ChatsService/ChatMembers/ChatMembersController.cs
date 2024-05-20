using ChatsService.ActionAccess.Actions;
using ChatsService.ActionAccess.Data;
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

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            if (!(await _actionAccessService.HasAccess(new GetChatMembersChatAction(), chatId, authData.Data.UserId)))
                return Unauthorized();

            var response = await _chatMembersService.GetChatMembersByChatId(chatId);

            return Ok(response);
        }

        [HttpPost("username")]
        public async Task<ActionResult<ServiceResponse<UserDto>>> AddMemberByUsername(string accessToken, ChatMemberByUsernameDto chatMemberDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var dataToValidateAccess = new AddChatMemberChatActionData()
            {
                ChatId = chatMemberDto.ChatId,
                RequesterId = authData.Data.UserId,
                UsernameToAdd = chatMemberDto.Username,
            };

            if (!(await _actionAccessService.HasAccess(new AddChatMemberChatAction(dataToValidateAccess), chatMemberDto.ChatId, authData.Data.UserId)))
                return Ok(new ServiceResponse<UserDto>() { Success = false, Message = "You dont have access to add users" });

            var response = await _chatMembersService.AddMemberByUsername(chatMemberDto);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteMember(string accessToken, int chatId, int userId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var dataToValidateAccess = new DeleteChatMemberChatActionData()
            {
                ChatId = chatId,
                RequesterId = authData.Data.UserId,
                UserIdToDelete = userId,
            };

            if (!(await _actionAccessService.HasAccess(new DeleteChatMemberChatAction(dataToValidateAccess), chatId, authData.Data.UserId)))
                return Ok(new ServiceResponse<bool>() { Data = false, Success = false, Message = "You do not have access to delete this user" });

            var response = await _chatMembersService.DeleteMember(chatId, userId);

            return Ok(response);
        }
    }
}
