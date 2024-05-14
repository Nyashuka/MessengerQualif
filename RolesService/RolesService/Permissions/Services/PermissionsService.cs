using RolesService.Models;
using RolesService.Permissions.Models;

namespace RolesService.Permissions.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly HttpClient _httpClient;

        public PermissionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<Permission>>> GetAllPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
