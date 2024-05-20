using MessagesService.Chat.Dto;
using MessagesService.Models.Requests;

namespace MessagesService.Services.Interfaces
{
    public interface IChatService
    {
        Task<bool> IsUserChatMember(int userId);
        Task<ServiceResponse<ChatDto>> GetChat(int chatId);
    }
}
