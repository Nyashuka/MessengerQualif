using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;

namespace DatabaseService.Services
{
    public class ChatMembersService : IChatMemebersService
    {
        private readonly DatabaseContext _databaseContext;

        public ChatMembersService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDTO)
        {
            var chatMember = new ChatMember()
            {
                ChatId = chatMemberDTO.ChatId,
                UserId = chatMemberDTO.UserId,
            };

            _databaseContext.ChatMembers.Add(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<ChatMember> { Data = chatMember };
        }

        public async Task<ServiceResponse<bool>> DeleteMember(int memberId)
        {
            var chatMember = _databaseContext.ChatMembers.FirstOrDefault(x => x.UserId == memberId);

            if (chatMember == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = $"Chat member with id={memberId} is not exists!"
                };
            }

            _databaseContext.ChatMembers.Remove(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> {  Data = true };
        }
    }
}
