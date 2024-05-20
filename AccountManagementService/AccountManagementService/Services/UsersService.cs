using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;

namespace AccountManagementService.Services
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;

        public UsersService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<List<User>>> GetOtherUsersForUser(int userId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetUsersGET}?userId={userId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data;
        }

        public async Task<ServiceResponse<UserDto>> GetUserData(int userId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetUserDataGET}?userId={userId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<UserDto>>();

            return data;
        }

        public async Task<ServiceResponse<UserDto>> UpdateUser(User user)
        {
            var databaseResponse = await _httpClient.PatchAsJsonAsync($"{APIEndpoints.GetUserDataGET}", user);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<UserDto>>();

            return data;
        }
    }
}
