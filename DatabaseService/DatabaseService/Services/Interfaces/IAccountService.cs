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
    }
}
