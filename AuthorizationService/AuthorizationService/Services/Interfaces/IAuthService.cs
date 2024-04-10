﻿using AuthorizationService.DTOs;
using AuthorizationService.Models;

namespace AuthorizationService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> CreateAccount(ClientCreationAccountDTO creationAccountDTO);
        Task<bool> IsUserExist(string email);
        Task<ServiceResponse<string>> Login(string email, string password);
    }
}