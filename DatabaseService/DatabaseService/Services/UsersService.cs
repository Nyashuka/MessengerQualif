using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
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

        public Task<ServiceResponse<List<UserDTO>>> GetOtherUsersForUser(int userId)
        {
            var users = new ServiceResponse<List<UserDTO>>()
            {
                Data = _databaseContext.Users.Where(x => x.Id != userId).Select(user => new UserDTO
                {
                    DisplayName = user.DisplayName,
                    Id = user.Id,
                    Username = user.Username
                }).ToList()
            };

            return Task.FromResult(users);
        }

        public async Task<ServiceResponse<UserDTO>> GetUserByAccountId(int accountId)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(user => user.AccountId == accountId);

            if (user == null)
            {
                return new ServiceResponse<UserDTO>()
                {
                    Success = false,
                    ErrorMessage = $"User with account id={accountId} does not exists!"
                };
            }

            return new ServiceResponse<UserDTO>
            {
                Data = new UserDTO() { DisplayName = user.DisplayName, Username = user.Username, Id = user.Id },
            };
        }

        public Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData)
        {
            throw new NotImplementedException();
        }
    }
}
