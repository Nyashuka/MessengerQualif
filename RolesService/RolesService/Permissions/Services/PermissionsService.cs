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
            var databaseResponse = await _httpClient.GetAsync(APIEndpoints.GetAllPermissionsGET);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<Permission>>>();

            return data;
        }
    }
}
