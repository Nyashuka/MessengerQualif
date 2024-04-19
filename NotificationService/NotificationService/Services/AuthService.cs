using NotificationService.DTOs;
using NotificationService.Models;
using NotificationService.Services.Interfaces;

namespace NotificationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<int> TryGetAuthenticatedUser(string accessToken)
        {
            var authenticatedUserResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAuthenticatedUserGET}?accessToken={accessToken}");
            var authenticatedUserData = await authenticatedUserResponse.Content.ReadFromJsonAsync<ServiceResponse<AuthUserDataDTO>>();

            if (authenticatedUserData == null)
            {
                return -1;
            }

            if (authenticatedUserData.Data == null)
            {
                return -1;
            }

            return authenticatedUserData.Data.Data.UserId;
        }
    }
}
