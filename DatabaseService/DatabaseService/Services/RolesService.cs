using DatabaseService.Data;
using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Runtime.CompilerServices;

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
            var defaultRoleIsExists = await _databaseContext.DefaultRoles.AnyAsync(x => x.ChatId == chatId);

            if (defaultRoleIsExists)
            {
                return new ServiceResponse<RoleWithPermissions>() { Success = false, Message = "Default role for this chat already exists" };
            }

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
                    RoleId = role.Id,
                    IsAllowed = true
                };

                _databaseContext.RolePermissionRelations.Add(rolePermissionRelation);
            }

            await _databaseContext.SaveChangesAsync();

            var defaultRole = new DefaultRole()
            {
                ChatId = chatId,
                RoleId = role.Id
            };
            _databaseContext.DefaultRoles.Add(defaultRole);
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
            var isDefaultRole = await _databaseContext.DefaultRoles.AnyAsync(x => x.RoleId == roleId);

            if (isDefaultRole)
            {
                return new ServiceResponse<bool>() { Success = false, Message = "Default role can't be deleted!" };
            }

            var role = await _databaseContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return new ServiceResponse<bool>() { Success = false, Message = "Role is not exists!" };
            }

            var userRoleRelations = _databaseContext.UserRoleRelations.Where(x => x.RoleId == role.Id).ToList();

            if (userRoleRelations != null && userRoleRelations.Count > 0)
                _databaseContext.UserRoleRelations.RemoveRange(userRoleRelations);

            var rolePermissionsRelations = _databaseContext.RolePermissionRelations.Where(x => x.RoleId == role.Id).ToList();

            if (userRoleRelations != null && userRoleRelations.Count > 0)
                _databaseContext.RolePermissionRelations.RemoveRange(rolePermissionsRelations);

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
                                Id = r.RoleId,
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
            var rolePermissionRelations = _databaseContext.RolePermissionRelations
                .Where(x => x.RoleId == role.Id && x.IsAllowed == true)
                .ToList();

            var permissions = new List<ChatPermission>();
            foreach (var permissionRelation in rolePermissionRelations)
            {
                permissions.Add(_databaseContext.ChatPermissions.First(x => x.Id == permissionRelation.ChatPermissionId));
            }

            var roleWithPermission = new RoleWithPermissions()
            {
                Id = role.Id,
                Name = role.Name,
                ChatId = role.ChatId,
                Permissions = permissions
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

        private class ChatPermissionEqualityComparer : IEqualityComparer<ChatPermission>
        {
            public bool Equals(ChatPermission? cp1, ChatPermission? cp2)
            {
                if (ReferenceEquals(cp1, cp2))
                    return true;

                if (cp1 is null || cp2 is null)
                    return false;

                return cp1.Id == cp2.Id;
            }

            public int GetHashCode(ChatPermission cp) => cp.Id;
        }

        public async Task<ServiceResponse<List<ChatPermission>>> GetAllPermissions(int chatId, int userId)
        {
            var userRoles = (await GetAllUserRoles(chatId, userId)).Data;

            ChatPermissionEqualityComparer comparer = new();
            HashSet<ChatPermission> uniqePermissions = new HashSet<ChatPermission>(comparer);

            foreach (var userRole in userRoles)
            {
                if (userRole.Permissions != null && userRole.Permissions.Count > 0)
                {
                    uniqePermissions.UnionWith(userRole.Permissions);
                }
            }

            return new ServiceResponse<List<ChatPermission>> { Data = new List<ChatPermission>(uniqePermissions) };
        }
    }
}
