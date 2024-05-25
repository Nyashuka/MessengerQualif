using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IUserRoleRelationService
    {
        Task<ServiceResponse<bool>> AsignRole(UserRoleRelationDto userRoleRelationDto);
        Task<ServiceResponse<bool>> RemoveRole(int roleId, int userId);
        Task<ServiceResponse<List<UserDto>>> GetAllRoleAssinges(int roleId);
        Task<ServiceResponse<bool>> AsignDefaultRole(int chatId, int userId);
    }
}
