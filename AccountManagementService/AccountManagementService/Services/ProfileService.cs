using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;

namespace AccountManagementService.Services
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient _httpClient;

        public ProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<User>> UpdateProfileInfo(UserDto userDto)
        {
            var response = await _httpClient.PatchAsJsonAsync($"{APIEndpoints.UpdateProfilePATCH}", userDto);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<User>>();

            return data;
        }

        public async Task<ServiceResponse<string>> UpdateProfilePicture(int userId, string avatarURL)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.UpdateProfilePicturePUT}?userId={userId}&avatarURL={avatarURL}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            return data;
        }
    }
}
