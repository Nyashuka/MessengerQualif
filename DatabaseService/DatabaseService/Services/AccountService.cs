using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DatabaseService.Services
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

        public async Task<ServiceResponse<AccountDTO>> GetAccount(string email) 
        {
            var account = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(email));

            if (account == null)
            {
                return new ServiceResponse<AccountDTO>() 
                {
                    Data = null,
                    Success = false,
                    Message = "Account does not exists!"
                };
            }

            AccountDTO accountDTO = new AccountDTO();
            accountDTO.Id = account.Id;
            accountDTO.Email = account.Email;
            accountDTO.PasswordHash = Convert.ToBase64String(account.PasswordHash);
            accountDTO.PasswordSalt = Convert.ToBase64String(account.PasswordSalt);

            return new ServiceResponse<AccountDTO>() { Data = accountDTO };
        }

        public async Task<ServiceResponse<int>> CreateAccount(CreationAccountDTO accountDTO)
        {
            if (await IsAccountExists(accountDTO.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = $"Account with email '{accountDTO.Email}' already exists!"
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

        private bool IsTokensSame(string token1, string token2)
        {
            string token1Substring = token1.Substring(0, token1.IndexOf('.'));
            string token2Substring = token2.Substring(0, token2.IndexOf('.'));

            return token1Substring.Equals(token2Substring);
        }
    }
}
