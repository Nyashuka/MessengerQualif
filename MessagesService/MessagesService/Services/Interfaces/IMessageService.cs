using MessagesService.DTOs;
using MessagesService.Models.Requests;

namespace MessagesService.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ServiceResponse<MessageDto>> HandleMessage(int senderId, string accessToken, ClientMessageDTO clientMessageDTO);
        Task<ServiceResponse<List<MessageDto>>> GetAllChatMessages(int chatId);
    }
}
