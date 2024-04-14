using AccountManagementService.Models;
using AccountManagementService.Servcies.Interfaces;

namespace AccountManagementService.Servcies
{
    public class UsersServcie : IUsersService
    {
        private HttpClient _httpClient;

        public UsersServcie(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<User>>> GetAllUsers()
        {
            var response = await _httpClient.GetAsync(APIEndpoints.GetUsersGET);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data;
        }
    }
}
