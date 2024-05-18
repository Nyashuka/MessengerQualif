using DatabaseService.Data;
using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace DatabaseService.Services
{
    public class RolesService : IRolesService
    {
        private readonly DatabaseContext _databaseContext;

        private const string DefaultRoleName = "Member";
        
        private readonly List<int> DefaultPermissions = new List<int>()
        {
            Convert.ToInt32(PermissionEnum.SendTextMessages),
        };

        public RolesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<RoleWithPermissions>> CreateDefaultRole(int chatId)
        {
            var role = new Role()
            {
                Name = DefaultRoleName,
                ChatId = chatId,
            };

            _databaseContext.Roles.Add(role);
            await _databaseContext.SaveChangesAsync();

            foreach (var permissionId in DefaultPermissions)
            {
                var rolePermissionRelation = new RolePermissionRelation()
                {
                    ChatPermissionId = permissionId,
                    RoleId = role.Id
                };

                _databaseContext.RolePermissionRelations.Add(rolePermissionRelation);
            }

            await _databaseContext.SaveChangesAsync();

            var roleWithPermissions = GetRoleWithPermissions(role);

            return new ServiceResponse<RoleWithPermissions>() { Data = roleWithPermissions };
        }

        public async Task<ServiceResponse<Role>> CreateRole(RoleDto roleDto)
        {
            Role role = new Role()
            {
                Name = roleDto.Name,
                ChatId = roleDto.ChatId,
            };

            _databaseContext.Roles.Add(role);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<Role>() { Data = role };
        }

        public async Task<ServiceResponse<bool>> DeleteRole(int roleId)
        {
            var role = await _databaseContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return new ServiceResponse<bool>() { Success = false, Message = "Role is not exists!" };
            }

            var userRoleRelations = _databaseContext.UserRoleRelations.Where(x => x.RoleId == role.Id).ToList();

            if (userRoleRelations != null && userRoleRelations.Count > 0)
                _databaseContext.UserRoleRelations.RemoveRange(userRoleRelations);

            _databaseContext.Roles.Remove(role);

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }

        public Task<ServiceResponse<List<RoleWithPermissions>>> GetChatRoles(int chatId)
        {
            var chatRoles = _databaseContext.Roles.Where(x => x.ChatId == chatId).ToList();

            var rolesWithPermissions = GetRolesWithPermissions(chatRoles);

            return Task.FromResult(new ServiceResponse<List<RoleWithPermissions>>()
            {
                Data = rolesWithPermissions
            });
        }

        public Task<ServiceResponse<List<RoleWithPermissions>>> GetAllUserRoles(int chatId, int userId)
        {
            var userRoles = _databaseContext.UserRoleRelations
                            .Where(x => x.UserId == userId)
                            .Select(r => new Role()
                            {
                                Id = r.Id,
                                ChatId = r.Role.ChatId,
                                Name = r.Role.Name,
                            })
                            .Where(x => x.ChatId == chatId).ToList();

            var userRolesWithPermissions = GetRolesWithPermissions(userRoles);

            return Task.FromResult(new ServiceResponse<List<RoleWithPermissions>>()
            {
                Data = userRolesWithPermissions
            });
        }

        private List<RoleWithPermissions> GetRolesWithPermissions(List<Role> roles)
        {
            var rolesWithPermissions = new List<RoleWithPermissions>();
            foreach (var role in roles)
            {
                var roleWithPermission = GetRoleWithPermissions(role);

                rolesWithPermissions.Add(roleWithPermission);
            }

            return rolesWithPermissions;
        }

        private RoleWithPermissions GetRoleWithPermissions(Role role)
        {

            var roleWithPermission = new RoleWithPermissions()
            {
                Id = role.Id,
                Name = role.Name,
                ChatId = role.ChatId,
                Permissions = _databaseContext.RolePermissionRelations
                                .Where(x => x.RoleId == role.Id && x.IsAllowed == true)
                                .Select(p => new ChatPermission()
                                {
                                    Id = p.ChatPermissionId,
                                    Name = p.ChatPermission.Name
                                }).ToList()
            };

            return roleWithPermission;
        }

        public async Task<ServiceResponse<RoleWithPermissions>> UpdateRole(RoleWithPermissions role)
        {
            var roleToUpdate = await _databaseContext.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);

            if (roleToUpdate == null)
            {
                return new ServiceResponse<RoleWithPermissions>() { Success = false, Message = "Role is not exists" };
            }

            roleToUpdate.Name = role.Name;

            var permissionRelations = _databaseContext.RolePermissionRelations.Where(x => x.RoleId == role.Id);

            _databaseContext.RolePermissionRelations.RemoveRange(permissionRelations);

            if (role.Permissions != null)
            {
                foreach (var permission in role.Permissions)
                {
                    var rolePermissionRelation = new RolePermissionRelation()
                    {
                        RoleId = role.Id,
                        ChatPermissionId = permission.Id,
                        IsAllowed = true,
                    };

                    _databaseContext.RolePermissionRelations.Add(rolePermissionRelation);
                }

            }

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<RoleWithPermissions> { Data = role };
        }

        
    }
}
