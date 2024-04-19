using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<List<UserDTO>>> GetOtherUsersForUser(int userId);
        Task<ServiceResponse<UserDTO>> GetUserByAccountId(int accountId);
        Task<ServiceResponse<User>> CreateUser(UserDTO userDTO);
        Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData);
    }
}
