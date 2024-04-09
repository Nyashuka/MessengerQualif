using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IChatService
    {
        Task<ServiceResponse<Chat>> CreateChat(ChatDTO chatDTO);
        Task<ServiceResponse<bool>> DeleteChat(int id);
    }
}
