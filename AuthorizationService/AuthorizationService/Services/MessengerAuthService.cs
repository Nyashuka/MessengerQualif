using AuthorizationService.DTOs;
using AuthorizationService.Models;
using AuthorizationService.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Cryptography;

namespace AuthorizationService.Services
{
    public class MessengerAuthService : IMessengerAuthService
    {
        private readonly HttpClient _httpClient;

        public MessengerAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsUserExist(string email)
        {
            return false;
            //await _dataContext.Users.AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<int>> CreateAccount(ClientCreationAccountDTO clientAccountDTO)
        {
            CreatePasswordHash(clientAccountDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            CreationAccountDTO account = new CreationAccountDTO();
            account.Email = clientAccountDTO.Email;
            account.DisplayName = clientAccountDTO.Email;
            account.Username = clientAccountDTO.Username;
            account.DisplayName = clientAccountDTO.ProfileName;
            account.PasswordHash = Convert.ToBase64String(passwordHash);
            account.PasswordSalt = Convert.ToBase64String(passwordSalt); 

            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreateAccountPOST, account);
            
            if(!databaseResponse.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync(databaseResponse.ReasonPhrase);
            }

            var parsedResponse = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<int>>();

            if (parsedResponse == null)
            {
                return new ServiceResponse<int>()
                {
                    Data = -1,
                    Success = false,
                    ErrorMessage = "Database service not return data. Maybe bad request"
                };
            }

            return parsedResponse;
        }
    }
}
