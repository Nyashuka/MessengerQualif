using AuthorizationService.DTOs;
using AuthorizationService.Models;
using AuthorizationService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService messengerAuthService)
        {
            _authService = messengerAuthService;
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateAccount(ClientCreationAccountDTO accountDTO)
        {
            var response = await _authService.CreateAccount(accountDTO);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<bool>>> IsUserExists(string email)
        {
            var response = await _authService.IsUserExist(email);

            //if (!response.Success)
            //    return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(AccountLoginDTO accountDTO)
        {
            var response = await _authService.Login(accountDTO.Email, accountDTO.Password);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("is-user-authenticated")]
        public async Task<ActionResult<ServiceResponse<AuthUserDataDTO>>> IsUserAuthenticated(string accessToken)
        {
            var response = await _authService.IsUserAuthenticated(accessToken);

            return Ok(response);
        }
    }
}
