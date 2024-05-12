using DatabaseService.Models;
using DatabaseService.DTOs;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IGroupsService
    {
        Task<ServiceResponse<ChatDto>> CreateGroup(ChatDto chatDTO);
        Task<ServiceResponse<List<ChatDto>>> GetAllGroups(int userId);
    }
}
