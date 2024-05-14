using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IRolesService
    {
        Task<ServiceResponse<Role>> CreateRole(RoleDto roleDto);
        Task<ServiceResponse<List<Role>>> GetChatRoles(int chatId);
        Task<ServiceResponse<Role>> UpdateRole(Role role);
        Task<ServiceResponse<bool>> DeleteRole(int roleId);
    }
}
