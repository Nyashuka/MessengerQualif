using AccountManagementService.Data;
using System.Security;

namespace AccountManagementService.Services.GroupsManagement
{
    public class GroupActionsAccess
    {
        private readonly HttpClient _httpClient;

        public GroupActionsAccess(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> HasAccessToAction(int userId, Permission permission)
        {
            return true;
        }
    }
}
