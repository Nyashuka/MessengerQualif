using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IChatTypeService
    {
        Task<ServiceResponse<ChatType>> CreateChatType(ChatTypeDTO dto);
        Task<ServiceResponse<ChatType>> UpdateChatType(ChatType dto);
        Task<ServiceResponse<bool>> DeleteChatType(int dto);
    }
}
