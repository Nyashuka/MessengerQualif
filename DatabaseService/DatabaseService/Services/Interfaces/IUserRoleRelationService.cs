using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IUserRoleRelationService
    {
        Task<ServiceResponse<bool>> GiveRole(UserRoleRelationDto userRoleRelationDto);
        Task<ServiceResponse<bool>> RemoveRole(UserRoleRelationDto userRoleRelationDto);
        Task<ServiceResponse<List<Role>>> GetUserRolesInChat(int chatId, int userId);
    }
}
