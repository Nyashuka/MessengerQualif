﻿using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Servcies.Interfaces;

namespace AccountManagementService.Servcies
{
    public class AuthService : IAuthService
    {
        private HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<AuthUserDataDTO>> TryGetAuthenticatedUser(string accessToken)
        {
            var authenticatedUserResponse = await _httpClient.GetAsync($"{APIEndpoints.IsUserAuthenticatedGET}?accessToken={accessToken}");
            var authenticatedUserData = await authenticatedUserResponse.Content.ReadFromJsonAsync<ServiceResponse<AuthUserDataDTO>>();

            if (authenticatedUserData == null || authenticatedUserData.Success == false ||
               authenticatedUserData.Data == null)
            {
                return new ServiceResponse<AuthUserDataDTO>
                {
                    Success = false,
                    ErrorMessage = authenticatedUserResponse.ReasonPhrase
                };
            }

            return new ServiceResponse<AuthUserDataDTO> 
            {
                Data = authenticatedUserData.Data
            };
        }
    }
}