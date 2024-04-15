using DatabaseService.DTOs;
using DatabaseService.Models;

namespace DatabaseService.Services.Interfaces
{
    public interface IChatMemebersService
    {
        Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDTO);
        Task<ServiceResponse<bool>> DeleteMember(int memberId);
    }
}
