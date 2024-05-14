using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService;

        public RolePermissionsController(IRolePermissionService rolePermissionService)
        {
            _rolePermissionService = rolePermissionService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<ChatPermission>>> GetRolePermission(int roleId)
        {
            var response = await _rolePermissionService.GetRolePermissions(roleId);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<RolePermissionRelation>>> ChangePermission(RolePermissionRelationDto rolePermissionRelationDto)
        {
            var response = await _rolePermissionService.ChangeRolePermission(rolePermissionRelationDto);

            return Ok(response);
        }
    }
}
