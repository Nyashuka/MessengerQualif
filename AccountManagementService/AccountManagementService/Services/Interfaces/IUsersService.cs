using AccountManagementService.DTOs;
using AccountManagementService.Models;

namespace AccountManagementService.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<List<User>>> GetOtherUsersForUser(int userId);

        Task<ServiceResponse<UserDto>> GetUserData(int userId);
        Task<ServiceResponse<UserDto>> UpdateUser(User user);
    }
}
