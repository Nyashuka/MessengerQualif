using DatabaseService.DataContexts;
using DatabaseService.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DatabaseService.Services.Roles
{
    public class RolePermissionRelationService
    {
        private readonly DatabaseContext _databaseContext;

        public RolePermissionRelationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task DeleteAllRelationsForRole(int roleId)
        {
            var relationsToDelete = _databaseContext.RolePermissionRelations.Where(x => x.RoleId == roleId).ToList();

            _databaseContext.RolePermissionRelations.RemoveRange(relationsToDelete);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task ChangePermissionForRole(int roleId, int permissionId, bool isAllowed)
        {
            var rolePermissionRelation = await _databaseContext.
                RolePermissionRelations.FirstOrDefaultAsync(x => x.RoleId == roleId &&
                                            x.ChatPermissionId == permissionId);

            if (rolePermissionRelation != null) 
            {
                rolePermissionRelation.IsAllowed = isAllowed;
                await _databaseContext.SaveChangesAsync();
                return;
            }

            rolePermissionRelation = new RolePermissionRelation()
            {
                RoleId = roleId,
                IsAllowed = isAllowed,
                ChatPermissionId = permissionId
            };

            _databaseContext.RolePermissionRelations.Add(rolePermissionRelation);
            await _databaseContext.SaveChangesAsync();
        }

  
    }
}
