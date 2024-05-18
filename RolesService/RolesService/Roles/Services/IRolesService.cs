using RolesService.Models;
using RolesService.Roles.Dto;
using RolesService.Roles.Models;

namespace RolesService.Roles.Services
{
    public interface IRolesService
    {
        Task<ServiceResponse<List<Role>>> GetAllChatRoles(int chatId);
        Task<ServiceResponse<Role>> CreateRole(RoleDto role);
    }
}
