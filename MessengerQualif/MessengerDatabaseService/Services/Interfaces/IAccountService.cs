using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<CreatedAccountDTO>> CreateAccount(AccountDTO accountDTO);
    }
}
