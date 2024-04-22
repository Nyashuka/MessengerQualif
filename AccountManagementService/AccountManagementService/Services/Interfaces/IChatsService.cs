using AccountManagementService.DTOs;
using AccountManagementService.Models;

namespace AccountManagementService.Services.Interfaces
{
    public interface IChatsService
    {
        Task<ServiceResponse<Chat>> CreateGroupChatIfNotExists(ChatDto chatDto);
        Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDto);
        Task<ServiceResponse<List<ChatDto>>> GetAllPersonalChats(int userId);
        Task<ServiceResponse<ChatDto>> GetPersonalChat(List<UserDto> users);
    }
}
