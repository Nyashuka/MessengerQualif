using ChatsService.Authorization;
using ChatsService.Groups.Dto;
using ChatsService.Groups.Interfaces;
using ChatsService.Groups.Models;
using ChatsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatsService.Groups
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService _groupsService;
        private readonly IAuthService _authService;

        public GroupsController(IGroupsService groupsService, IAuthService authService)
        {
            _groupsService = groupsService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ChatDto>>> CreateGroupChat([FromQuery] string accessToken, ChatDto chatDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            if (chatDto.ChatInfo == null)
                return Unauthorized();

            if (chatDto.ChatInfo.Owner == null || chatDto.ChatInfo.Owner.Id != authData.Data.UserId)
                return Unauthorized();

            var response = await _groupsService.CreateGroup(chatDto);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Chat>>>> GetAllGroupsByUserId(string accessToken)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authData.HasAccess || authData.Data == null)
                return Unauthorized();

            var response = await _groupsService.GetAllGroupsByUserId(authData.Data.UserId);

            return Ok(response);
        }
    }
}
