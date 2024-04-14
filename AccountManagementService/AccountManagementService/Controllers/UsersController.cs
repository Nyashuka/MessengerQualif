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

        public UsersController(IUsersService usersService) 
        {
            _userService = usersService;    
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();

            return Ok(response);
        }
    }
}
