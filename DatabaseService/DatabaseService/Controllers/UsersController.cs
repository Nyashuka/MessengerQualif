﻿using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDto>>>> GetOtherUsersForUser(int userId)
        {
            var response = await _userService.GetOtherUsersForUser(userId);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult<ServiceResponse<User>>> UpdateUser(UserDto userDto)
        {
            var response = await _userService.UpdateUser(userDto);

            return Ok(response);
        }

        [HttpGet("get-user")]
        public async Task<ActionResult<ServiceResponse<UserDto>>> GetUserByUserId(int userId)
        {
            var response = await _userService.GetUserByUserId(userId);

            return Ok(response);
        }

        [HttpGet("get-user-by-account-id")]
        public async Task<ActionResult<ServiceResponse<UserDto>>> GetUserByAccountId(int accountId)
        {
            var response = await _userService.GetUserByAccountId(accountId);

            return Ok(response);
        }

        [HttpGet("picture")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateProfilePicture(int userId, string avatarURL)
        {
            var response = await _userService.UpdateProfilePicture(userId, avatarURL);

            return Ok(response);
        }
    }
}
