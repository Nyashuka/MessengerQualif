using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IChatMembersService
    {
        Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDto);
        Task<ServiceResponse<bool>> DeleteMember(ChatMemberDTO chatMemberDto);
        Task<ServiceResponse<List<UserDto>>> GetChatMembersByChatId(int chatId);
    }
}
