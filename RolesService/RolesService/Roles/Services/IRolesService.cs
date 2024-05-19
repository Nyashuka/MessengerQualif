using RolesService.Models;
using RolesService.Roles.Dto;
using RolesService.Roles.Models;
using RolesService.Users;

namespace RolesService.Roles.Services
{
    public interface IRolesService
    {
        Task<ServiceResponse<List<Role>>> GetAllChatRoles(int chatId);
        Task<ServiceResponse<Role>> CreateRole(RoleDto role);
        Task<ServiceResponse<RoleWithPermissions>> UpdateRole(RoleWithPermissions role);
        Task<ServiceResponse<bool>> AssignRole(UserRoleRelationDto relation);
        Task<ServiceResponse<bool>> UnAssingRole(int roleId, int userId);
        Task<ServiceResponse<List<User>>> GetAllAssinges(int roleId);
    }
}
