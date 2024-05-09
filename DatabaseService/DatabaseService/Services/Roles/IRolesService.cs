using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Roles
{
    public interface IRolesService
    {
        Task<ServiceResponse<Role>> CreateRole(RoleDto role);
        Task<ServiceResponse<Role>> UpdateRole(Role role);
        Task<ServiceResponse<Role>> DeleteRole(int chatId);
        Task<ServiceResponse<List<Role>>> GetChatRoles(int chatId);
        Task<ServiceResponse<List<Role>>> GetChatRolesByUser(int chatId, int userId);
    }
}
