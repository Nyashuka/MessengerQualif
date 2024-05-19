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

        public async Task<ServiceResponse<bool>> AsignRole(UserRoleRelationDto userRoleRelationDto)
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

        public async Task<ServiceResponse<List<UserDto>>> GetAllRoleAssinges(int roleId)
        {
            if (!await _databaseContext.Roles.AnyAsync(x => x.Id == roleId))
            {
                return new ServiceResponse<List<UserDto>>() { Success = false, Message = "This role is not exists" };
            }

            var assignes = _databaseContext.UserRoleRelations.Where(x => x.RoleId == roleId)
                                           .Select(x => new UserDto()
                                           {
                                               Id = x.User.Id,
                                               DisplayName = x.User.DisplayName,
                                               Username = x.User.Username
                                           }).ToList();

            return new ServiceResponse<List<UserDto>>() { Data = assignes };
        }

        public async Task<ServiceResponse<bool>> RemoveRole(int roleId, int userId)
        {
            var userRoleRelation = await _databaseContext.UserRoleRelations
                .FirstOrDefaultAsync(x => x.RoleId == roleId &&
                                     x.UserId == userId);

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
