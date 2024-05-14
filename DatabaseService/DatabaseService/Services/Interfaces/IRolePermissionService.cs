using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IRolePermissionService
    {
        Task<ServiceResponse<List<ChatPermission>>> GetRolePermissions(int roleId);
        Task<ServiceResponse<RolePermissionRelation>> ChangeRolePermission(RolePermissionRelationDto rolePermissionRelationDto);
    }
}
