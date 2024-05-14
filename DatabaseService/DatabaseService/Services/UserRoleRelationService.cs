using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class UserRoleRelationService : IUserRoleRelationService
    {
        private readonly DatabaseContext _databaseContext;

        public UserRoleRelationService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<List<Role>>> GetUserRolesInChat(int chatId, int userId)
        {
            var userRoles =  _databaseContext.UserRoleRelations.Where(urr => urr.UserId == userId)
                             .Join(_databaseContext.Roles,
                                  urr => urr.RoleId,
                                  role => role.Id,
                                  (urr, role) => role)
                             .ToList();

            return Task.FromResult(new ServiceResponse<List<Role>>() { Data = userRoles });
        }

        public async Task<ServiceResponse<bool>> GiveRole(UserRoleRelationDto userRoleRelationDto)
        {
            if (_databaseContext.UserRoleRelations.Any(x => x.RoleId == userRoleRelationDto.RoleId && x.UserId == userRoleRelationDto.UserId))
            {
                return new ServiceResponse<bool>() { Data = false, Success = false, Message = "User already have this role" };
            }

            var userRoleRelation = new UserRoleRelation()
            {
                UserId = userRoleRelationDto.UserId,
                RoleId = userRoleRelationDto.RoleId,
            };

            _databaseContext.UserRoleRelations.Add(userRoleRelation);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveRole(UserRoleRelationDto userRoleRelationDto)
        {
            var userRoleRelation = await _databaseContext.UserRoleRelations
                .FirstOrDefaultAsync(x => x.RoleId == userRoleRelationDto.RoleId &&
                                     x.UserId == userRoleRelationDto.UserId);

            if (userRoleRelation == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    Message = "User don't have this role"
                };
            }

            _databaseContext.UserRoleRelations.Remove(userRoleRelation);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
