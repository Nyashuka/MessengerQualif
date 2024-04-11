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

        public async Task<ServiceResponse<AccountDTO>> GetAccount(string email) 
        {
            var account = await _databaseContext.Accounts.FirstOrDefaultAsync(x => x.Email.Equals(email));

            if (account == null)
            {
                return new ServiceResponse<AccountDTO>() 
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Account does not exists!"
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

        public async Task<ServiceResponse<AccessToken>> GetAccessToken(int accountId)
        {
            AccessToken? token = await _databaseContext.AccessTokens.FirstOrDefaultAsync(x => x.AccountId == accountId);

            if (token == null)
            {
                return new ServiceResponse<AccessToken>() { Data = null, Success = false, ErrorMessage = "Token does not exists!" };
            }

            return new ServiceResponse<AccessToken>() { Data = token };
        }

        public async Task<ServiceResponse<bool>> SaveAccessToken(AccessToken accessToken)
        {
            bool exists = await _databaseContext.AccessTokens.AnyAsync(x => x.AccountId == accessToken.AccountId);

            if (exists)
                return new ServiceResponse<bool>() { Data = true };

            _databaseContext.AccessTokens.Add(accessToken);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
