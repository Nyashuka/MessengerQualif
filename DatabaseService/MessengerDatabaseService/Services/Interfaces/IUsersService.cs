using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<List<UserDTO>>> GetAllUsers();
        Task<ServiceResponse<User>> CreateUser(UserDTO userDTO);
        Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData);
    }
}
