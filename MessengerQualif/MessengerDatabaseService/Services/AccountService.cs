using MessengerDatabaseService.DataContexts;
using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MessengerDatabaseService.Services
{
    public class AccountService : IAccountService
    {
        private readonly DatabaseContext _databaseContext;

        public AccountService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> IsAccountExists(string email)
        {
            return await _databaseContext.Accounts.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public async Task<bool> IsUserExists(string username)
        {
            return await _databaseContext.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower()));
        }

        public async Task<ServiceResponse<int>> CreateAccount(CreationAccountDTO accountDTO)
        {
            if (await IsAccountExists(accountDTO.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    ErrorMessage = $"Account with email '{accountDTO.Email}' already exists!"
                };
            }

            Account account = new Account()
            {
                Email = accountDTO.Email,
                PasswordHash = Convert.FromBase64String(accountDTO.PasswordHash),
                PasswordSalt = Convert.FromBase64String(accountDTO.PasswordSalt)
            };

            _databaseContext.Accounts.Add(account);
            await _databaseContext.SaveChangesAsync();

            User user = new User()
            {
                AccountId = account.Id,
                Username = accountDTO.Username,
                DisplayName = accountDTO.DisplayName
            };

            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<int>
            {
                Data = account.Id
            };
        }
    }
}
