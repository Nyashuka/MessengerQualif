using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Role>>>> GetAllRoles(int chatId)
        {
            var response = await _rolesService.GetChatRoles(chatId);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Role>>> CreateRole(RoleDto roleDto)
        {
            var resposne = await _rolesService.CreateRole(roleDto);

            return Ok(resposne);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRole(Role role)
        {
            var response = await _rolesService.DeleteRole(role.Id);

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult<ServiceResponse<Role>>> UpdateRole(Role role)
        {
            var response = await _rolesService.UpdateRole(role);

            return Ok(response);
        }
    }
}
