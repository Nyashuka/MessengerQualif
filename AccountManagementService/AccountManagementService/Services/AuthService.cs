using AccountManagementService.DTOs.Auth;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;

namespace AccountManagementService.Services
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthUserDataDTO> TryGetAuthenticatedUser(string accessToken)
        {
            var authenticatedUserResponse = await _httpClient.GetAsync($"{APIEndpoints.IsUserAuthenticatedGET}?accessToken={accessToken}");
            var authenticatedUserData = await authenticatedUserResponse.Content.ReadFromJsonAsync<ServiceResponse<AuthUserDataDTO>>();

            if (authenticatedUserData == null || authenticatedUserData.Data == null)
            {
                return new AuthUserDataDTO() { Data = null, HasAccess = false};
            }

            return new AuthUserDataDTO()
            {
                Data = authenticatedUserData.Data.Data,
                HasAccess = true
            };
        }
    }
}
