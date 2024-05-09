using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatsService _chatsService;
        private readonly IAuthService _authService;

        public ChatsController(IChatsService chatsService, IAuthService authService)
        {
            _chatsService = chatsService;
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

            var response = await _chatsService.CreatePersonalChatIfNotExists(chatDto);

            return Ok(response);    
        }

        [HttpGet("get-personal-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetPersonalChatById([FromQuery] string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            ServiceResponse<ChatDto> response = await _chatsService.GetPersonalChatById(chatId);

            if(response.Data == null || response.Data.Members == null ||
                !response.Data.Members.Any(m => m.Id == authData.Data.UserId))
                return Unauthorized();

            return Ok(response);
        }

        [HttpGet("get-personal")]
        public async Task<ActionResult<ServiceResponse<List<ChatDto>>>> GetAllPersonalChats([FromQuery] string accessToken)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            var response = await _chatsService.GetAllPersonalChats(authData.Data.UserId);

            return Ok(response);
        }

        [HttpPost("get-personal")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreatePersonalChat([FromQuery] string accessToken, List<UserDto> users)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            if (!users.Any(m => m.Id == authData.Data.UserId))
                return Unauthorized();

            var response = await _chatsService.GetPersonalChat(users);

            return Ok(response);
        }

        [HttpPost("create-group")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreateGroupChat([FromQuery] string accessToken, ChatDto chatDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            if (chatDto.ChatInfo == null)
                return Unauthorized();

            if(chatDto.ChatInfo.OwnerUser == null || chatDto.ChatInfo.OwnerUser.Id != authData.Data.UserId)
                return Unauthorized();

            var response = await _chatsService.CreateGroupChatIfNotExists(chatDto);

            return Ok(response);
        }
    }
}
