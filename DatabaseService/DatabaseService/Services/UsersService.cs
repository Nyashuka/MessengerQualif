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
        private readonly DatabaseContext _databaseContext;

        public UsersService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<User>> CreateUser(UserDto userDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<UserDto>>> GetOtherUsersForUser(int userId)
        {
            var users = new ServiceResponse<List<UserDto>>()
            {
                Data = _databaseContext.Users.Where(x => x.Id != userId).Select(user => new UserDto
                {
                    DisplayName = user.DisplayName,
                    Id = user.Id,
                    Username = user.Username
                }).ToList()
            };

            return Task.FromResult(users);
        }

        public async Task<ServiceResponse<UserDto>> GetUserByUserId(int userId)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
            {
                return new ServiceResponse<UserDto>()
                {
                    Success = false,
                    Message = $"User with user id={userId} does not exists!"
                };
            }

            return new ServiceResponse<UserDto>
            {
                Data = new UserDto() { DisplayName = user.DisplayName, Username = user.Username, Id = user.Id },
            };
        }

        public async Task<ServiceResponse<UserDto>> GetUserByAccountId(int accountId)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(user => user.AccountId == accountId);

            if (user == null)
            {
                return new ServiceResponse<UserDto>()
                {
                    Success = false,
                    Message = $"User with account id={accountId} does not exists!"
                };
            }

            return new ServiceResponse<UserDto>
            {
                Data = new UserDto() { DisplayName = user.DisplayName, Username = user.Username, Id = user.Id },
            };
        }

        public async Task<ServiceResponse<User>> UpdateUser(UserDto newUserData)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == newUserData.Id);

            if (user == null) 
            {
                return new ServiceResponse<User> { Success = false, Message = "User to update not found" };
            }

            if(await _databaseContext.Users
                .AnyAsync(x => x.Username.ToLower().Equals(newUserData.Username.ToLower()) && 
                x.Id != newUserData.Id))
            {
                return new ServiceResponse<User> { Success = false, Message = "Username already taken" };
            }

            user.Username = newUserData.Username;
            user.DisplayName = newUserData.DisplayName;

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<User> { Data = user };
        }
    }
}
