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

        private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<CreatedAccountDTO>> CreateAccount(AccountDTO accountDTO)
        {
            if (await IsAccountExists(accountDTO.Email))
            {
                return new ServiceResponse<CreatedAccountDTO>
                {
                    Success = false,
                    ErrorMessage = $"Account with email '{accountDTO.Email}' already exists!"
                };
            }

            CreatePasswordHash(accountDTO.Password, out byte[] hash, out byte[] salt);

            Account account = new Account();
            account.Email = accountDTO.Email;
            account.PasswordHash = hash;
            account.PasswordSalt = salt;

            _databaseContext.Accounts.Add(account);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<CreatedAccountDTO>
            {
                Data = new CreatedAccountDTO() { Id = account.Id }
            };
        }
    }
}
