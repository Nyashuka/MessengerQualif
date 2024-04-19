using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> IsAccountExists(string email);
        Task<ServiceResponse<int>> CreateAccount(CreationAccountDTO accountDTO);
        Task<ServiceResponse<AccountDTO>> GetAccount(string email);
        Task<ServiceResponse<bool>> SaveAccessToken(AccessTokenDTO accessTokenDTO);
        Task<ServiceResponse<AccessToken>> GetAccessToken(int accountId);
        Task<ServiceResponse<UserDataByAccessTokenDTO>> GetAccountByAccessToken(string accessToken);
    }
}
