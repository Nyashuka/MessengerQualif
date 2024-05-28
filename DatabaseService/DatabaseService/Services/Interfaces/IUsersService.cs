using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<List<UserDto>>> GetOtherUsersForUser(int userId);
        Task<ServiceResponse<UserDto>> GetUserByUserId(int userId);
        Task<ServiceResponse<UserDto>> GetUserByAccountId(int accountId);
        Task<ServiceResponse<User>> CreateUser(UserDto userDto);
        Task<ServiceResponse<User>> UpdateUser(UserDto newUserData);
        Task<ServiceResponse<string>> UpdateProfilePicture(int userId, string avatarURL);
    }
}
