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

        public RolesService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
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

        public Task<ServiceResponse<List<Role>>> GetChatRoles(int chatId)
        {
            var chatRoles = _databaseContext.Roles.Where(x => x.ChatId == chatId).ToList();

            return Task.FromResult(new ServiceResponse<List<Role>>() { Data = chatRoles });
        }

        public async Task<ServiceResponse<Role>> UpdateRole(Role role)
        {
            var roleToUpdate = await _databaseContext.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);

            if (roleToUpdate == null)
            {
                return new ServiceResponse<Role>() { Success = false, Message = "Role is not exists" };
            }

            roleToUpdate.Name = role.Name;

            await _databaseContext.SaveChangesAsync();
        
            return new ServiceResponse<Role> { Data = roleToUpdate };
        }
    }
}
