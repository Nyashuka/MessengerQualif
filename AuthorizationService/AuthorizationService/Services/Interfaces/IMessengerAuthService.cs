using AuthorizationService.DTOs;
using AuthorizationService.Models;

namespace AuthorizationService.Services.Interfaces
{
    public interface IMessengerAuthService
    {
        Task<ServiceResponse<int>> CreateAccount(ClientCreationAccountDTO creationAccountDTO);
    }
}
