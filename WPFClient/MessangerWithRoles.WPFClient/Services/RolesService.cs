using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services.ServiceLocatorModule
{
    public class RolesService : IService
    {
        private readonly HttpClient _httpClient;

        public RolesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ServiceResponse<Role>> CreateRole(RoleDto role)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.CreateRolePOST}?accessToken={authService.AccessToken}", role);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<Role>>();

            return data;
        }

        public async Task<ServiceResponse<List<RoleWithPermissions>>> GetChatRoles(int chatId)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllChatRolesGET}?accessToken={authService.AccessToken}&chatId={chatId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<RoleWithPermissions>>>();

            return data;
        }

        public async Task<ServiceResponse<List<ChatPermission>>> GetAllPermissions()
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllPermissionsGET}?accessToken={authService.AccessToken}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<ChatPermission>>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> AssignRole(UserRoleRalations userRoleRelation)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.AssignRolePOST}?accessToken={authService.AccessToken}", userRoleRelation);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> UnAssignRole(UserRoleRalations userRoleRelation)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.DeleteAsync($"{APIEndpoints.UnAssignRoleDELETE}?accessToken={authService.AccessToken}&roleId={userRoleRelation.RoleId}&userId={userRoleRelation.UserId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }

        public async Task<ServiceResponse<List<User>>> GetAllAssignes(int roleId)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetAllRoleAssignesGET}?accessToken={authService.AccessToken}&roleId={roleId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data;
        }

        public async Task<ServiceResponse<RoleWithPermissions>> UpdateRole(RoleWithPermissions roleWithPermissions)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.PutAsJsonAsync($"{APIEndpoints.UpdateRolePATCH}?accessToken={authService.AccessToken}", roleWithPermissions);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<RoleWithPermissions>>();

            return data;
        }

        public async Task<ServiceResponse<bool>> DeleteRole(int roleId)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient.DeleteAsync($"{APIEndpoints.DeleteRoleDELETE}?accessToken={authService.AccessToken}&roleId={roleId}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return data;
        }
    }
}
