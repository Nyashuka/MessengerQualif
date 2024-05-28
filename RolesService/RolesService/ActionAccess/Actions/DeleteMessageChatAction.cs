
using RolesService.ActionAccess.Data;
using RolesService.Permissions.Models;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess.Actions
{
    public class DeleteMessageChatAction : IChatAction
    {
        private readonly IRolesService _rolesService;
        private readonly DeleteMessageChatActionData _data;

        public DeleteMessageChatAction(IRolesService rolesService, DeleteMessageChatActionData data)
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

            if (HasDeleteMessagesAccess(requesterPermissionsResponse.Data))
                return true;

            return false;
        }

        private bool HasDeleteMessagesAccess(List<Permission> permissions)
        {
            return permissions.Any(x => x.Id == Convert.ToInt32(PermissionEnum.DeleteMessages));
        }

        private bool IsAdministrator(List<Permission> permissions)
        {
            return permissions.Any(x => x.Id == Convert.ToInt32(PermissionEnum.Administrator));
        }
    }
}
