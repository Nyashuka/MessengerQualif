
using RolesService.ActionAccess.Data;

namespace RolesService.ActionAccess.Actions
{
    public class CanManageRolesChatAction : IChatAction
    {
        public CanManageRolesChatActionData _data;

        public CanManageRolesChatAction(CanManageRolesChatActionData data)
        {
            _data = data;
        }

        public Task<bool> HasAccess(HttpClient httpClient)
        {
            return Task.FromResult(false);
        }
    }
}
