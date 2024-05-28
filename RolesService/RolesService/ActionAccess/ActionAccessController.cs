using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolesService.ActionAccess.Actions;
using RolesService.ActionAccess.Data;
using RolesService.ActionAccess.Services;
using RolesService.Models;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionAccessController : ControllerBase
    {
        private readonly IActionAccessService _actionAccessService;
        private readonly IRolesService _roleService;

        public ActionAccessController(IActionAccessService actionAccessService, IRolesService roleService)
        {
            _actionAccessService = actionAccessService;
            _roleService = roleService;
        }

        [HttpPost("can-delete-member")]
        public async Task<ActionResult<ServiceResponse<bool>>> CanDeleteMember(DeleteChatMemberChatActionData data)
        {
            var result = await _actionAccessService.HasAccess(new DeleteChatMemberChatAction(_roleService, data), data.ChatId, data.RequesterId);

            return Ok(new ServiceResponse<bool> { Data = result });
        }

        [HttpPost("can-add-member")]
        public async Task<ActionResult<ServiceResponse<bool>>> CanAddMember(AddChatMemberChatActionData data)
        {
            var result = await _actionAccessService.HasAccess(new AddChatMemberChatAction(_roleService, data), data.ChatId, data.RequesterId);

            return Ok(new ServiceResponse<bool> { Data = result });
        }

        [HttpPost("can-send-text-message")]
        public async Task<ActionResult<ServiceResponse<bool>>> CanSendTextMessage(SendMessageChatActionData data)
        {
            var result = await _actionAccessService.HasAccess(new SendMessageChatAction(_roleService, data), data.ChatId, data.RequesterId);

            return Ok(new ServiceResponse<bool> { Data = result });
        }

        [HttpPost("can-change-chat-info")]
        public async Task<ActionResult<ServiceResponse<bool>>> CanSendTextMessage(ChangeGroupInfoChatActionData data)
        {
            var result = await _actionAccessService.HasAccess(new ChangeGroupInfoChatAction(_roleService, data), data.ChatId, data.RequesterId);

            return Ok(new ServiceResponse<bool> { Data = result });
        }
    }
}
