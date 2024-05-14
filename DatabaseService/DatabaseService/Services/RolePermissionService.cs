using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class RolePermissionService : IRolePermissionService
    {
        private readonly DatabaseContext _databaseContext;

        public RolePermissionService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<RolePermissionRelation>> ChangeRolePermission(RolePermissionRelationDto rolePermissionRelationDto)
        {
            var rolePermissionRelation = await _databaseContext.RolePermissionRelations
                .FirstOrDefaultAsync(x => x.RoleId == rolePermissionRelationDto.RoleId && 
                                     x.ChatPermissionId == rolePermissionRelationDto.ChatPermissionId);

            if (rolePermissionRelation == null)
            {
                rolePermissionRelation = new RolePermissionRelation()
                {
                    RoleId = rolePermissionRelationDto.RoleId,
                    ChatPermissionId = rolePermissionRelationDto.ChatPermissionId,
                    IsAllowed = rolePermissionRelationDto.IsAllowed
                };    
            }
            else
            {
                rolePermissionRelation.IsAllowed = rolePermissionRelationDto.IsAllowed;
            }

            rolePermissionRelation.ChatPermission = await _databaseContext.ChatPermissions.FirstAsync(x => x.Id == rolePermissionRelationDto.ChatPermissionId);

            _databaseContext.RolePermissionRelations.Add(rolePermissionRelation);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<RolePermissionRelation>() { Data = rolePermissionRelation };
        }

        public Task<ServiceResponse<List<ChatPermission>>> GetRolePermissions(int roleId)
        {
            var permissions = _databaseContext.RolePermissionRelations
                             .Where(rpr => rpr.RoleId == roleId && rpr.IsAllowed)
                             .Join(_databaseContext.ChatPermissions,
                                  rpr => rpr.ChatPermissionId, 
                                  permission => permission.Id,
                                  (rpr, permission) => permission)
                             .ToList();

            return Task.FromResult(new ServiceResponse<List<ChatPermission>>() { Data = permissions });
        }
    }
}
