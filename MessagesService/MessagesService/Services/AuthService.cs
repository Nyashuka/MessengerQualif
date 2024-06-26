﻿
using MessagesService;
using MessagesService.DTOs.Auth;
using MessagesService.Models.Requests;
using MessagesService.Services.Interfaces;

public class AuthService : IAuthService
{
    private HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Return -1, if user not authorized
    /// </summary>
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

