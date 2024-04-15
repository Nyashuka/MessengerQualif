using AuthorizationService.DTOs;
using AuthorizationService.Models;
using AuthorizationService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthorizationService.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient,
                                    IConfiguration configuration,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsUserExist(string email)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.IsUserExistsGET}?email={email}");
            if (!response.IsSuccessStatusCode)
                return false;

            var parsedResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            if (parsedResponse == null)
                return false;

            if (!parsedResponse.Success)
                return false;

            return parsedResponse.Data;
        }

        public async Task<ServiceResponse<Account>> GetAccount(string email)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAccountGET}?email={email}");
            if (response == null)
            {
                return new ServiceResponse<Account>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Can't get response from database service!"
                };
            }

            var parsedResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<Account>>();

            if (parsedResponse == null)
            {
                return new ServiceResponse<Account>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = "Can't parse data from response!"
                };
            }

            return parsedResponse;
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
            account.DisplayName = clientAccountDTO.DisplayName;
            account.PasswordHash = Convert.ToBase64String(passwordHash);
            account.PasswordSalt = Convert.ToBase64String(passwordSalt);

            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreateAccountPOST, account);

            if (!databaseResponse.IsSuccessStatusCode)
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

        private async Task<ServiceResponse<AccessTokenDTO>> TryGetTokenIfExistsInDatabase(int accountId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetTokenGET}?accountId={accountId}");

            if (response == null)
            {
                return new ServiceResponse<AccessTokenDTO>() { Data = null, Success = false, ErrorMessage = "Request cannot execute!" };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<AccessTokenDTO>() { Data = null, Success = false, ErrorMessage = response.ReasonPhrase };
            }

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<AccessTokenDTO>>();

            if (responseData == null)
            {
                return new ServiceResponse<AccessTokenDTO>() { Success = false, ErrorMessage = "Can't parse data from json" };
            }

            return responseData;
        }

        private string CreateToken(Account account)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value
                ));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public ClaimsPrincipal ValidateTokenOrGetNull(string token)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                         _configuration.GetSection("AppSettings:Token").Value
            ));

            // Налаштування параметрів перевірки токена
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(
                    token, tokenValidationParameters, out SecurityToken validatedToken);

                return principal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private async Task<ServiceResponse<bool>> SaveTokenToDatabase(int accountId, string token)
        {
            var accessToken = new AccessTokenDTO();
            accessToken.AccountId = accountId;
            accessToken.Token = token;

            var response = await _httpClient.PostAsJsonAsync(APIEndpoints.SaveTokenPOST, accessToken);

            return new ServiceResponse<bool>() { Data = true };
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                response.Success = false;
                response.ErrorMessage = "Login data can't be empty!";
                return response;
            }

            var account = await GetAccount(email);

            if (account == null)
            {
                response.Success = false;
                response.ErrorMessage = "Account is not found!";
            }
            else if (account.Data == null)
            {
                response.Success = false;
                response.ErrorMessage = account.ErrorMessage;
            }
            else if (!VerifyPasswordHash(password, Convert.FromBase64String(account.Data.PasswordHash),
                                    Convert.FromBase64String(account.Data.PasswordSalt)))
            {
                response.Success = false;
                response.ErrorMessage = "Wrong password!";
            }
            else
            {
                var tokenFromDatabase = await TryGetTokenIfExistsInDatabase(account.Data.Id);
                if (tokenFromDatabase.Data != null)
                {
                    response.Data = tokenFromDatabase.Data.Token;
                    return response;
                }
                var token = CreateToken(account.Data);
                response.Data = token;
                await SaveTokenToDatabase(account.Data.Id, token);
            }

            return response;
        }

        public async Task<ServiceResponse<AuthUserDataDTO>> TryValidateAndGetAccountFromToken(string accessToken)
        {
            var claimsPrincipal = ValidateTokenOrGetNull(accessToken);

            if (claimsPrincipal == null)
            {
                return new ServiceResponse<AuthUserDataDTO>()
                {
                    Success = false,
                    ErrorMessage = "Token is not valid!"
                };
            }

            var id = Convert.ToInt32(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _httpClient.GetAsync($"{APIEndpoints.GetUserGet}?accountId={id}");
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<UserDTO>>();

            return new ServiceResponse<AuthUserDataDTO>()
            {
                Data = new AuthUserDataDTO()
                {
                    HasAccess = true,
                    Data = new UserDataByAccessTokenDTO() { AccountId = id, UserId = responseData.Data.Id},
                },
            };


            //var response = await _httpClient.GetAsync($"{APIEndpoints.GetAccountByAccessTokenGET}?accessToken={accessToken}");
            //var dataFromResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<UserDataByAccessTokenDTO>>();

            //if (dataFromResponse == null)
            //{
            //    return new ServiceResponse<AuthUserDataDTO>
            //    {
            //        Data = null,
            //        Success = false,
            //        ErrorMessage = "Cannot parse from database response"
            //    };
            //}

            //if (!dataFromResponse.Success)
            //{
            //    return new ServiceResponse<AuthUserDataDTO>
            //    {
            //        Success = false,
            //        ErrorMessage = dataFromResponse.ErrorMessage
            //    };
            //}

            //return new ServiceResponse<AuthUserDataDTO>
            //{
            //    Data = new AuthUserDataDTO()
            //    {
            //        HasAccess = true,
            //        Data = dataFromResponse.Data
            //    }
            //};
        }
    }
}
