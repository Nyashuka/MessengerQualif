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
    }
}
