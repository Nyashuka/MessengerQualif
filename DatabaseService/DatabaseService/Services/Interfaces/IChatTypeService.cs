using DatabaseService.DTOs;
using DatabaseService.Models;

namespace DatabaseService.Services.Interfaces
{
    public interface IChatTypeService
    {
        Task<ServiceResponse<ChatType>> CreateChatType(ChatTypeDTO dto);
        Task<ServiceResponse<ChatType>> UpdateChatType(ChatType dto);
        Task<ServiceResponse<bool>> DeleteChatType(int dto);
    }
}
