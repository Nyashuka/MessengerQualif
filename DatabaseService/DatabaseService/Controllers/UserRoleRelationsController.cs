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
    public class UserRoleRelationsController : ControllerBase
    {
        private readonly IUserRoleRelationService _userRoleRelationService;

        public UserRoleRelationsController(IUserRoleRelationService userRoleRelationService)
        {
            _userRoleRelationService = userRoleRelationService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<bool>>> GetAllRoleAssignes(int roleId)
        {
            var response = await _userRoleRelationService.GetAllRoleAssinges(roleId);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> AsignRole(UserRoleRelationDto userRoleRelationDto)
        {
            var response = await _userRoleRelationService.AsignRole(userRoleRelationDto);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> UnAsignRole(int roleId, int userId)
        {
            var response = await _userRoleRelationService.RemoveRole(roleId, userId);

            return Ok(response);
        }
    }
}
