using AccountManagementService.DTOs.Auth;
using AccountManagementService.Models;

namespace AccountManagementService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse<AuthUserDataDTO>> TryGetAuthenticatedUser(string acessToken);
    }
}
