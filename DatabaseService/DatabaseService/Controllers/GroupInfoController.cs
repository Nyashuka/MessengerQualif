using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupInfoController : ControllerBase
    {
        private readonly IGroupsInfoService _groupsInfoService;

        public GroupInfoController(IGroupsInfoService groupsInfoService)
        {
            _groupsInfoService = groupsInfoService;
        }

        [HttpGet("picture")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateProfilePicture(int chatId, string avatarURL)
        {
            var response = await _groupsInfoService.UpdateProfilePicture(chatId, avatarURL);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult<ServiceResponse<GroupChatInfoDto>>> UpdateInfo(GroupChatInfoDto groupChatInfoDto)
        {
            var response = await _groupsInfoService.UpdateInfo(groupChatInfoDto);

            return Ok(response);
        }
    }
}
