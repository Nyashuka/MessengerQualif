using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly IAuthService _authService;

        public UsersController(IUsersService usersService, IAuthService authService)
        {
            _userService = usersService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetOtherUsersForUser(string accessToken)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            var response = await _userService.GetOtherUsersForUser(authenticatedUser.Data.UserId);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<User>>>> UpdateUser(string accessToken, User user)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null || authenticatedUser.Data.UserId != user.Id)
                return Unauthorized();

            var response = await _userService.UpdateUser(user);

            return Ok(response);
        }

        [HttpGet("get-user")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUserData(string accessToken)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (!authenticatedUser.HasAccess || authenticatedUser.Data == null)
                return Unauthorized();

            var response = await _userService.GetUserData(authenticatedUser.Data.UserId);

            return Ok(response);
        }
    }
}
