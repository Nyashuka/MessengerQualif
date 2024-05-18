using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IRolesService
    {
        Task<ServiceResponse<Role>> CreateRole(RoleDto roleDto);
        Task<ServiceResponse<List<RoleWithPermissions>>> GetChatRoles(int chatId);
        Task<ServiceResponse<RoleWithPermissions>> UpdateRole(RoleWithPermissions role);
        Task<ServiceResponse<bool>> DeleteRole(int roleId);
        Task<ServiceResponse<List<RoleWithPermissions>>> GetAllUserRoles(int chatId, int userId);
        Task<ServiceResponse<RoleWithPermissions>> CreateDefaultRole(int chatId);
    }
}
