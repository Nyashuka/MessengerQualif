using AccountManagementService.DTOs;
using AccountManagementService.Models;

namespace AccountManagementService.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ServiceResponse<User>> UpdateProfileInfo(UserDto userDto);
        Task<ServiceResponse<string>> UpdateProfilePicture(int userId, string avatarURL);
    }
}
