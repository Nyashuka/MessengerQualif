using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly RolePermissionRelationService _rolePermissionRelationService;

        public RolesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _rolePermissionRelationService = new RolePermissionRelationService(databaseContext);
        }

        public async Task<ServiceResponse<Role>> CreateRole(RoleDto role)
        {
            var roleToCreate = new Role()
            {
                Name = role.Name,
                ChatId = role.ChatId,
            };

            _databaseContext.Roles.Add(roleToCreate);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<Role> { Data = roleToCreate };
        }

        public Task<ServiceResponse<Role>> DeleteRole(int chatId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Role>>> GetChatRoles(int chatId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Role>>> GetChatRolesByUser(int chatId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<Role>> UpdateRole(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
