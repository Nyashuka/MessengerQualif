using RolesService.Models;
using RolesService.Permissions.Models;

namespace RolesService.Permissions.Services
{
    public interface IPermissionsService
    {
        Task<ServiceResponse<List<Permission>>> GetAllPermissions();
    }
}
