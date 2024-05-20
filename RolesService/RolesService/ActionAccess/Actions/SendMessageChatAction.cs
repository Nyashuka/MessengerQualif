
using RolesService.ActionAccess.Data;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess.Actions
{
    public class SendMessageChatAction : IChatAction
    {
        private readonly IRolesService _rolesService;
        private readonly SendMessageChatActionData _data;

        public SendMessageChatAction(IRolesService rolesService, SendMessageChatActionData data)
        {
            _data = data;
            _rolesService = rolesService;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var requesterPermissionsResponse = await _rolesService.GetAllUserPermissions(_data.ChatId, _data.RequesterId);

            int sendMessageAccessId = Convert.ToInt32(PermissionEnum.SendTextMessages);

            if(requesterPermissionsResponse.Data.Any(x => x.Id == sendMessageAccessId))
            {
                return true;
            }

            return false;
        }
    }
}
