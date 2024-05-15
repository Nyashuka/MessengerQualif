using ChatsService.Groups.Dto;
using ChatsService.Models;

namespace ChatsService.Chats.Services
{
    public interface IChatsService
    {
        Task<ServiceResponse<ChatDto>> GetChatById(int chatId);
    }
}
