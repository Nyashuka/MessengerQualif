using AuthorizationService.DTOs;
using AuthorizationService.Models;
using AuthorizationService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMessengerAuthService _messengerAuthService;

        public AuthController(IMessengerAuthService messengerAuthService)
        {
            _messengerAuthService = messengerAuthService;
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateAccount(ClientCreationAccountDTO accountDTO)
        {
            var response = await _messengerAuthService.CreateAccount(accountDTO);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
