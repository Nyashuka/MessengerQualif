﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolesService.ActionAccess.Actions;
using RolesService.ActionAccess.Data;
using RolesService.ActionAccess.Services;
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
        private readonly IActionAccessService _actionAccessService;

        public RolesController(IRolesService rolesService, IAuthService authService, IActionAccessService actionAccessService)
        {
            _rolesService = rolesService;
            _authService = authService;
            _actionAccessService = actionAccessService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<RoleWithPermissions>>>> GetAllGroupRoles(string accessToken, int chatId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var response = await _rolesService.GetAllChatRoles(chatId);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Role>>> CreateRole(string accessToken, RoleDto roleDto)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
                new CanManageRolesChatAction(new CanManageRolesChatActionData()),
                roleDto.ChatId,
                authData.Data.UserId))
            {
                return Ok(new ServiceResponse<Role>()
                {
                    Data = null,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.CreateRole(roleDto);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<Role>>> DeleteRole(string accessToken, int roleId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var chat = await _rolesService.GetChatByRoleId(roleId);

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
               new CanManageRolesChatAction(new CanManageRolesChatActionData()),
               chat.Data.Id,
               authData.Data.UserId))
            {
                return Ok(new ServiceResponse<Role>()
                {
                    Data = null,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.DeleteRole(roleId);

            return Ok(response);
        }

        [HttpPost("assignes")]
        public async Task<ActionResult<ServiceResponse<bool>>> AssignRole(string accessToken, UserRoleRelationDto relation)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var chat = await _rolesService.GetChatByRoleId(relation.RoleId);

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
               new CanManageRolesChatAction(new CanManageRolesChatActionData()),
               chat.Data.Id,
               authData.Data.UserId))
            {
                return Ok(new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.AssignRole(relation);

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<RoleWithPermissions>>> UpdateRole(string accessToken, RoleWithPermissions role)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var chat = await _rolesService.GetChatByRoleId(role.Id);

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
               new CanManageRolesChatAction(new CanManageRolesChatActionData()),
               chat.Data.Id,
               authData.Data.UserId))
            {
                return Ok(new ServiceResponse<RoleWithPermissions>()
                {
                    Data = null,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.UpdateRole(role);

            return Ok(response);
        }

        [HttpDelete("assignes")]
        public async Task<ActionResult<ServiceResponse<bool>>> UnAssignRole(string accessToken, int roleId, int userId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var chat = await _rolesService.GetChatByRoleId(roleId);

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
               new CanManageRolesChatAction(new CanManageRolesChatActionData()),
               chat.Data.Id,
               authData.Data.UserId))
            {
                return Ok(new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.UnAssingRole(roleId, userId);

            return Ok(response);
        }

        [HttpGet("assignes")]
        public async Task<ActionResult<ServiceResponse<Role>>> GetAllAssinges(string accessToken, int roleId)
        {
            var authData = await _authService.TryGetAuthenticatedUser(accessToken);

            if (authData.Data == null || !authData.HasAccess)
                return Unauthorized();

            var chat = await _rolesService.GetChatByRoleId(roleId);

            if (!await _actionAccessService.HasAccess<CanManageRolesChatAction>(
               new CanManageRolesChatAction(new CanManageRolesChatActionData()),
               chat.Data.Id,
               authData.Data.UserId))
            {
                return Ok(new ServiceResponse<Role>()
                {
                    Data = null,
                    Success = false,
                    Message = "You dont have access to manage roles"
                });
            }

            var response = await _rolesService.GetAllAssingners(roleId);

            return Ok(response);
        }
    }
}
