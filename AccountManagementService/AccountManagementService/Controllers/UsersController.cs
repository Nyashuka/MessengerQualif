using AccountManagementService.Models;
using AccountManagementService.Servcies.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;
        private IAuthService _authService;

        public UsersController(IUsersService usersService, IAuthService authService) 
        {
            _userService = usersService;    
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetOtherUsersForUser(string accessToken)
        {
            var authenticatedUser = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authenticatedUser.Success == false)
            {
                var badResponse = new ServiceResponse<List<User>>
                {
                    Success = false,
                    ErrorMessage = authenticatedUser.ErrorMessage
                };
                return BadRequest(badResponse);
            }

            var response = await _userService.GetOtherUsersForUser(authenticatedUser.Data.Data.UserId);

            return Ok(response);
        }
    }
}
