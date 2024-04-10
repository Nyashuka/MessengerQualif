using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> CreateUser(UserDTO userDTO);
        Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData);
    }
}
