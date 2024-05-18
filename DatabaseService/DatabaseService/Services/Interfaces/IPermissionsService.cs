using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IPermissionsService
    {
        Task<ServiceResponse<List<ChatPermission>>> GetAllPermissions();
    }
}
