using MessengerDatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MessengerDatabaseService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
    }
}
