using ChatsService.Authorization;
using ChatsService.ChatMembers.Dto;
using ChatsService.Groups.Dto;
using ChatsService.Groups.Models;
using ChatsService.Models;
using ChatsService.PersonalChats.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatsService.PersonalChats
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalChatsController : ControllerBase
    {
        private readonly IPersonalChatsService _personalChatsService;
        private readonly IAuthService _authService;

        public PersonalChatsController(IPersonalChatsService personalChatsService, IAuthService authService)
        {
            _personalChatsService = personalChatsService;
            _authService = authService;
        }

        [HttpPost("create-personal")]
        public async Task<ActionResult<ServiceResponse<Chat>>> CreatePersonalChat([FromQuery] string accessToken, ChatDto chatDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            if (chatDto.Members.Count != 2 || !chatDto.Members.Any(m => m.Id == authData.Data.UserId))
                return Unauthorized();

            var response = await _personalChatsService.CreatePersonalChatIfNotExists(chatDto);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ChatDto>>>> GetAllPersonalChats([FromQuery] string accessToken)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            var response = await _personalChatsService.GetAllPersonalChats(authData.Data.UserId);

            return Ok(response);
        }

        [HttpPost("get-personal-by-members")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetPersonalChatByMembers([FromQuery] string accessToken, List<UserDto> users)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            if (!users.Any(m => m.Id == authData.Data.UserId))
                return Unauthorized();

            var response = await _personalChatsService.GetPersonalChatByMembers(users);

            return Ok(response);
        }


    }
}