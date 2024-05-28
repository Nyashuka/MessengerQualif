using RolesService.ActionAccess.Data;
using RolesService.Permissions.Models;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess.Actions
{
    public class ChangeGroupInfoChatAction : IChatAction
    {
        private readonly IRolesService _rolesService;
        private readonly ChangeGroupInfoChatActionData _data;

        public ChangeGroupInfoChatAction(IRolesService rolesService, ChangeGroupInfoChatActionData data)
        {
            _rolesService = rolesService;
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var requesterPermissionsResponse = await _rolesService.GetAllUserPermissions(_data.ChatId, _data.RequesterId);

            if (requesterPermissionsResponse.Data == null || requesterPermissionsResponse.Data.Count == 0)
                return false;

            if (IsAdministrator(requesterPermissionsResponse.Data))
                return true;

            if (HasChangeInfoAccess(requesterPermissionsResponse.Data))
                return true;

            return false;
        }

        private bool HasChangeInfoAccess(List<Permission> permissions)
        {
            return permissions.Any(x => x.Id == Convert.ToInt32(PermissionEnum.ChangeChatInfo));
        }

        private bool IsAdministrator(List<Permission> permissions)
        {
            return permissions.Any(x => x.Id == Convert.ToInt32(PermissionEnum.Administrator));
        }
    }
}