using RolesService.ActionAccess.Data;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess.Actions
{
    public class AddChatMemberChatAction : IChatAction
    {
        private readonly AddChatMemberChatActionData _data;
        private readonly IRolesService _rolesService;

        public AddChatMemberChatAction(IRolesService rolesService, AddChatMemberChatActionData data)
        {
            _data = data;
            _rolesService = rolesService;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var requesterPermissionsResponse = await _rolesService.GetAllUserPermissions(_data.ChatId, _data.RequesterId);

            if (requesterPermissionsResponse == null || requesterPermissionsResponse.Data == null || requesterPermissionsResponse.Data.Count == 0)
                return false;

            int addMemberCode = Convert.ToInt32(PermissionEnum.AddMembers);
            int adminCode = Convert.ToInt32(PermissionEnum.Administrator);

            if (requesterPermissionsResponse.Data.Any(x => x.Id == addMemberCode || x.Id == adminCode))
                return true;

            return false;
        }
    }
}