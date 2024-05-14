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
        public async Task<ActionResult<ServiceResponse<Role>>> GetUserRoles(int chatId, int userId)
        {
            var resposne = await _userRoleRelationService.GetUserRolesInChat(chatId, userId);

            return Ok(resposne);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> GiveRole(UserRoleRelationDto userRoleRelationDto)
        {
            var response = await _userRoleRelationService.GiveRole(userRoleRelationDto);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveRole(UserRoleRelationDto userRoleRelationDto)
        {
            var response = await _userRoleRelationService.RemoveRole(userRoleRelationDto);

            return Ok(response);
        }
    }
}
