using MessengerDatabaseService.DataContexts;
using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;

namespace MessengerDatabaseService.Services
{
    public class UsersService : IUsersService
    {
        private DatabaseContext _databaseContext;

        public UsersService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<User>> CreateUser(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<UserDTO>>> GetAllUsers()
        {
            var users = new ServiceResponse<List<UserDTO>>()
            {
                Data = _databaseContext.Users.Select(user => new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Id = user.Id,
                    Username = user.Username
                }).ToList()
            };

            return Task.FromResult(users);
        }

        public Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData)
        {
            throw new NotImplementedException();
        }
    }
}
