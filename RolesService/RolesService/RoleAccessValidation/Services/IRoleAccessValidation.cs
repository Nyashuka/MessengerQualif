using RolesService.Models;

namespace RolesService.RoleAccessValidation.Services
{
    public interface IRoleAccessValidation
    {
        Task<ServiceResponse<bool>> DoesUserHavePermission(int chatId, int userId, int permissionId);
    }
}
