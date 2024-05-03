using AccountManagementService.DTOs.Auth;
using AccountManagementService.Models;

namespace AccountManagementService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthUserDataDTO> TryGetAuthenticatedUser(string acessToken);
    }
}
