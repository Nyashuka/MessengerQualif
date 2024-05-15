using ChatsService.ChatMembers.Dto;
using ChatsService.Groups.Dto;
using ChatsService.Models;

namespace ChatsService.PersonalChats.Services
{
    public interface IPersonalChatsService
    {
        Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDto);
        Task<ServiceResponse<List<ChatDto>>> GetAllPersonalChats(int userId);
        Task<ServiceResponse<ChatDto>> GetPersonalChatByMembers(List<UserDto> users);
    }
}