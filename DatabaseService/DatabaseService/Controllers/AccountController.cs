using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountController)
        {
            _accountService = accountController;
        }

        [HttpPost("create-account")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateAccount(CreationAccountDTO creationAccountDTO)
        {
            var response = await _accountService.CreateAccount(creationAccountDTO);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("user-exists")]
        public async Task<ActionResult<ServiceResponse<bool>>> IsUserExists(string email)
        {
            bool isExists = await _accountService.IsAccountExists(email);
            var response = new ServiceResponse<bool>()
            {
                Data = isExists
            };

            return Ok(response);
        }

        [HttpGet("get-account")]
        public async Task<ActionResult<ServiceResponse<AccountDTO>>> GetAccount(string email)
        {
            var account = await _accountService.GetAccount(email);

            if(account.Success == false)
                return BadRequest(account);

            return Ok(account);
        }
    }
}
