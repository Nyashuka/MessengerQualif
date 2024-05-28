using MessagesService.DTOs;
using MessagesService.Models.Requests;

namespace MessagesService.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ServiceResponse<MessageDto>> SendMessage(int senderId, string accessToken, string clientGuid, ClientMessageDTO clientMessageDTO);
        Task<ServiceResponse<bool>> DeleteMessage(int senderId, string accessToken, string clientGuid, int messageId);
        Task<ServiceResponse<List<MessageDto>>> GetAllChatMessages(int chatId);
        Task<ServiceResponse<MessageDto>> GetMessageById(int messageId);
    }
}
