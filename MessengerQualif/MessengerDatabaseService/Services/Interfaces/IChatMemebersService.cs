using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IChatMemebersService
    {
        Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDTO);
        Task<ServiceResponse<bool>> DeleteMember(int memberId);
    }
}
