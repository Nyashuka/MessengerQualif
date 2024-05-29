using ChatsService.ActionAccess.Actions;
using ChatsService.ActionAccess.Data;
using ChatsService.ActionAccess.Services;
using ChatsService.Authorization;
using ChatsService.Groups.Dto;
using ChatsService.Groups.Services;
using ChatsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatsService.Groups
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsInfoController : ControllerBase
    {
        private readonly IGroupsInfoService _groupsInfoService;
        private readonly IAuthService _authService;
        private readonly IActionAccessService _actionAccessService;


        public GroupsInfoController(IGroupsInfoService groupsInfoService, IAuthService authService, IActionAccessService actionAccessService)
        {
            _groupsInfoService = groupsInfoService;
            _authService = authService;
            _actionAccessService = actionAccessService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GroupChatInfoDto>>> UpdateInfo([FromQuery] string accessToken, GroupChatInfoDto groupChatInfoDto)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            var chatActionData = new ChangeGroupInfoChatActionData()
            {
                ChatId = groupChatInfoDto.ChatId,
                RequesterId = authenticatedUser.Data.UserId,
            };
            var chatAction = new ChangeGroupInfoChatAction(chatActionData);

            if (!await _actionAccessService.HasAccess(chatAction, groupChatInfoDto.ChatId, authenticatedUser.Data.UserId))
            {
                return Ok(new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "You don't have access to change chat info"
                });
            }

            var response = await _groupsInfoService.UpdateGroupInfo(groupChatInfoDto);

            return Ok(response);
        }

        [HttpPost("picture")]
        public async Task<ActionResult<ServiceResponse<string>>> UploadImage([FromQuery] string accessToken, [FromQuery] int chatId, IFormFile file)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            var chatActionData = new ChangeGroupInfoChatActionData()
            {
                ChatId = chatId,
                RequesterId = authenticatedUser.Data.UserId,
            };
            var chatAction = new ChangeGroupInfoChatAction(chatActionData);

            if (!await _actionAccessService.HasAccess(chatAction, chatId, authenticatedUser.Data.UserId))
            {
                return Ok(new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "You don't have access to change chat info"
                });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new ServiceResponse<bool>()
                {
                    Success = true,
                    Message = "No file uploaded."
                });
            }

            var filePath = await SavePicture(chatId, file);

            var response = await _groupsInfoService.UpdateGroupPicture(chatId, filePath);

            return Ok(response);
        }

        private async Task<string> SavePicture(int chatId, IFormFile file)
        {
            var directoryPath = Path.Combine("uploads", "profile", chatId.ToString());
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = Path.Combine(directoryPath, "avatar.jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }
    }
}
