using ChatsService.ChatMembers.Dto;
using ChatsService.ChatMembers.Models;
using ChatsService.Models;

namespace ChatsService.ChatMembers.Services
{
    public interface IChatMembersService
    {
        Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDto chatMemberDto);
        Task<ServiceResponse<bool>> DeleteMember(int chatId, int userId);
        Task<ServiceResponse<List<UserDto>>> GetChatMembersByChatId(int chatId);
    }
}
