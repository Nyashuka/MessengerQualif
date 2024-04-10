using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> IsAccountExists(string email);
        Task<ServiceResponse<int>> CreateAccount(CreationAccountDTO accountDTO);
        Task<ServiceResponse<AccountDTO>> GetAccount(string email);
        Task<ServiceResponse<bool>> SaveAccessToken(AccessToken accessToken);
    }
}
