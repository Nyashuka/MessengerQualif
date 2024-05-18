using RolesService.Models;
using RolesService.Roles.Dto;
using RolesService.Roles.Models;

namespace RolesService.Roles.Services
{
    public class RolesService : IRolesService
    {
        private readonly HttpClient _httpClient;

        public RolesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<Role>> CreateRole(RoleDto role)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreateRolePOST, role);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<Role>>();

            return data;
        }

        public async Task<ServiceResponse<List<Role>>> GetAllChatRoles(int chatId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllGroupRolesGET}?chatId={chatId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<Role>>>();

            return data;
        }
    }
}
