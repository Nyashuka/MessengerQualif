using ChatsService.Authorization;
using ChatsService.Chats.Services;
using ChatsService.Groups.Dto;
using ChatsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatsService.Chats
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

        [HttpGet("get-by-id")]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> GetChatById(string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            var response = await _chatsService.GetChatById(chatId);

            return Ok(response);
        }
    }
}
