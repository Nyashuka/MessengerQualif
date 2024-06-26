﻿using RolesService.Chat.Dto;
using RolesService.Models;
using RolesService.Permissions.Models;
using RolesService.Roles.Dto;
using RolesService.Roles.Models;
using RolesService.Users;

namespace RolesService.Roles.Services
{
    public class RolesService : IRolesService
    {
        private readonly HttpClient _httpClient;

        public RolesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<bool>> AssignRole(UserRoleRelationDto relation)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.AssignRolePOST, relation);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> UnAssingRole(int roleId, int userId)
        {
            var databaseResponse = await _httpClient
                                   .DeleteAsync($"{APIEndpoints.AssignRolePOST}?roleId={roleId}&userId={userId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<Role>> CreateRole(RoleDto role)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreateRolePOST, role);

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<Role>>();

            return data;
        }

        public async Task<ServiceResponse<List<User>>> GetAllAssingners(int roleId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllRoleAssignersGET}?roleId={roleId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data;
        }

        public async Task<ServiceResponse<List<RoleWithPermissions>>> GetAllChatRoles(int chatId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllGroupRolesGET}?chatId={chatId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<RoleWithPermissions>>>();

            return data;
        }

        public async Task<ServiceResponse<RoleWithPermissions>> UpdateRole(RoleWithPermissions role)
        {
            var databaseResponse = await _httpClient.PatchAsJsonAsync(APIEndpoints.UpdateRolePATCH, role);
           
            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<RoleWithPermissions>>();

            return data;
        }

        public async Task<ServiceResponse<List<Permission>>> GetAllUserPermissions(int chatId, int userId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllUserPermissionsGET}?chatId={chatId}&userId={userId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<Permission>>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> DeleteRole(int roleId)
        {
            var databaseResponse = await _httpClient.DeleteAsync($"{APIEndpoints.DeleteRoleDELETE}?roleId={roleId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<ChatDto>> GetChatByRoleId(int roleId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatByRoleIdGET}?roleId={roleId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return data;
        }
    }
}
