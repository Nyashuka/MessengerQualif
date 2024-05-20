using DatabaseService.DataContexts;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;

namespace DatabaseService.Services
{
    public class PermissionsService : IPermissionsService
    {
        private readonly DatabaseContext _databaseContext;

        public PermissionsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<List<ChatPermission>>> GetAllPermissions()
        {
            var permissions = _databaseContext.ChatPermissions.ToList();

            return Task.FromResult(new ServiceResponse<List<ChatPermission>>()
            {
                Data = permissions
            });
        }


    }
}
