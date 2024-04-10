using MessengerDatabaseService.DataContexts;
using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MessengerDatabaseService.Services
{
    public class UserService : IUserService
    {
        private DatabaseContext _databaseContext;

        public UserService(DatabaseContext databaseContext)
        { 
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<User>> CreateUser(UserDTO userDTO)
        {
            var user = new User()
            {
                AccountId = userDTO.AccountId,
                Username = userDTO.Username,
                DisplayName = userDTO.ProfileName,
                Account = await _databaseContext.Accounts.FirstAsync(a => a.Id == userDTO.AccountId)
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<User>() { Data = user };
        }

        public async Task<ServiceResponse<User>> UpdateUser(int userId, UserDTO newUserData)
        {
            var user = await _databaseContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new ServiceResponse<User>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"User with id '{userId}' does not exists!"
                };

            user.Username = newUserData.Username;
            user.DisplayName = newUserData.ProfileName;
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<User>() { Data = user };
        }
    }
}
