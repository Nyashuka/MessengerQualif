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

            if (!authData.Success)
                return Unauthorized();

            if (chatDto.Members.Count != 2 || !chatDto.Members.Any(m => m.Id == authData.Data.Data.UserId))
                return Unauthorized();

            var response = await _chatsService.CreatePersonalChatIfNotExists(chatDto);

            return Ok(response);    
        }

        [HttpGet("get-personal-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetPersonalChatById([FromQuery] string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.Success || !authData.Data.HasAccess)
                return Unauthorized();

            ServiceResponse<ChatDto> response = await _chatsService.GetPersonalChatById(chatId);

            if(!response.Data.Members.Any(m => m.Id == authData.Data.Data.UserId))
                return Unauthorized();

            return Ok(response);
        }

        [HttpGet("get-personal")]
        public async Task<ActionResult<ServiceResponse<List<ChatDto>>>> GetAllPersonalChats([FromQuery] string accessToken)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.Success)
                return Unauthorized();

            var response = await _chatsService.GetAllPersonalChats(authData.Data.Data.UserId);

            return Ok(response);
        }

        [HttpPost("get-personal")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreatePersonalChat([FromQuery] string accessToken, List<UserDto> users)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.Success)
                return Unauthorized();

            if (!users.Any(m => m.Id == authData.Data.Data.UserId))
                return Unauthorized();

            var response = await _chatsService.GetPersonalChat(users);

            return Ok(response);
        }

    }
}
