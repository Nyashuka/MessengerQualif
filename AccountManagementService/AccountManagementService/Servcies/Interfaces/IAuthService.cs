using AccountManagementService.DTOs;
using AccountManagementService.Models;

namespace AccountManagementService.Servcies.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthUserDataDTO>> TryGetAuthenticatedUser(string acessToken);
    }
}
