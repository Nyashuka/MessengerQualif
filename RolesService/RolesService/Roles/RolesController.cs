using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolesService.Authorization;
using RolesService.Models;
using RolesService.Roles.Dto;
using RolesService.Roles.Models;
using RolesService.Roles.Services;

namespace RolesService.Roles
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IAuthService _authService;

        public RolesController(IRolesService rolesService, IAuthService authService)
        {
            _rolesService = rolesService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Role>>>> GetAllGroupRoles(string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var response = await _rolesService.GetAllChatRoles(chatId);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Role>>>> CreateRole(string accessToken, RoleDto roleDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var response = await _rolesService.CreateRole(roleDto);

            return Ok(response);
        }
    }
}
