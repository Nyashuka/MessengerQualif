using Microsoft.AspNetCore.Mvc;
using RolesService.Models;
using RolesService.Permissions.Models;
using RolesService.Permissions.Services;

namespace RolesService.Permissions
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
        public async Task<ActionResult<ServiceResponse<List<Permission>>>> GetAllPermissions(string accessToken)
        {
            var response = await _permissionsService.GetAllPermissions();

            return Ok(response);
        }

      
    }
}
