using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsService _permissionsService;

        public PermissionsController(IPermissionsService permissionsService)
        {
            _permissionsService = permissionsService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<ChatPermission>>>> GetAllPermissions()
        {
            var response = await _permissionsService.GetAllPermissions();

            return Ok(response);
        }
    }
}
