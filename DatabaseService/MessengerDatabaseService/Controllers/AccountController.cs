﻿using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<ServiceResponse<AccountDTO>>> GetAccout(string email)
        {
            var account = await _accountService.GetAccount(email);

            if(account.Success == false)
                return BadRequest(account);

            return Ok(account);
        }

        [HttpPost("save-token")]
        public async Task<ActionResult<ServiceResponse<AccountDTO>>> SaveToken(AccessToken accessToken)
        {
            var savingResponse = await _accountService.SaveAccessToken(accessToken);

            if (savingResponse.Success == false)
                return BadRequest(savingResponse);

            return Ok(savingResponse);
        }
    }
}