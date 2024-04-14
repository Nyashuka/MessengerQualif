using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;
using MessengerDatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessengerDatabaseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<UserDTO>>>> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            return Ok(response);
        }

        [HttpGet("get-user")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUserByAccountId(int accountId)
        {
            var response = await _userService.GetUserByAccountId(accountId);

            return Ok(response);
        }

    }
}
