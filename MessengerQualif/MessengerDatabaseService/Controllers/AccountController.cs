using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MessengerDatabaseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountController)
        {
            _accountService = accountController;
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<ServiceResponse<CreatedAccountDTO>>> CreateAccount(AccountDTO accountDTO)
        {
            var response = await _accountService.CreateAccount(accountDTO);

            if(!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
