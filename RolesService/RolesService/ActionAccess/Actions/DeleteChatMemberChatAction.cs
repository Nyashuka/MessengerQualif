using RolesService.ActionAccess.Data;
using RolesService.Roles.Services;

namespace RolesService.ActionAccess.Actions
{
    public class DeleteChatMemberChatAction : IChatAction
    {
        private readonly IRolesService _rolesService;
        private readonly DeleteChatMemberChatActionData _data;

        public DeleteChatMemberChatAction(IRolesService rolesService, DeleteChatMemberChatActionData data)
        {
            _rolesService = rolesService;
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var requesterPermissionsResponse = await _rolesService.GetAllUserPermissions(_data.ChatId, _data.RequesterId);
            var deletUserPermissionsResponse = await _rolesService.GetAllUserPermissions(_data.ChatId, _data.UserIdToDelete);

            if (requesterPermissionsResponse == null || requesterPermissionsResponse.Data == null || requesterPermissionsResponse.Data.Count == 0)
                return false;

            int deleteMemberCode = Convert.ToInt32(PermissionEnum.DeleteMembers);
            int adminMemberCode = Convert.ToInt32(PermissionEnum.Administrator);

            bool isDeletedAdmin = deletUserPermissionsResponse.Data.Any(x => x.Id == adminMemberCode);
            
            if(isDeletedAdmin)
                return false;

            bool isRequesterAdmin = requesterPermissionsResponse.Data.Any(x => x.Id == adminMemberCode);
            bool hasRequesterDeleteAccess = requesterPermissionsResponse.Data.Any(x => x.Id == deleteMemberCode);
            bool hasDeletedDeleteAccess = deletUserPermissionsResponse.Data.Any(x => x.Id == deleteMemberCode);

            if (isRequesterAdmin)
                return true;

            if (!hasDeletedDeleteAccess && hasRequesterDeleteAccess)
                return true;

            return false;
        }
    }
}