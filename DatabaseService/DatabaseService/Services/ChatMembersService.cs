using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;

namespace DatabaseService.Services
{
    public class ChatMembersService : IChatMembersService
    {
        private readonly DatabaseContext _databaseContext;

        public ChatMembersService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<List<ChatMember>>> GetChatMembersByChatId(int chatId)
        {
            var chatMembers = _databaseContext.ChatMembers.Where(m => m.ChatId == chatId).ToList();

            return new ServiceResponse<List<ChatMember>> { Data = chatMembers };
        }

        public async Task<ServiceResponse<ChatMember>> AddMember(ChatMemberDTO chatMemberDto)
        {
            var chatMember = new ChatMember()
            {
                ChatId = chatMemberDto.ChatId,
                UserId = chatMemberDto.UserId,
            };

            _databaseContext.ChatMembers.Add(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<ChatMember> { Data = chatMember };
        }

        public async Task<ServiceResponse<bool>> DeleteMember(ChatMemberDTO chatMemberDto)
        {
            var chatMember = _databaseContext.ChatMembers
                            .FirstOrDefault(x => x.ChatId == chatMemberDto.ChatId && 
                                                           x.UserId == chatMemberDto.UserId);

            if (chatMember == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = $"Chat member with id={chatMemberDto.UserId} is not exists!"
                };
            }

            _databaseContext.ChatMembers.Remove(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> {  Data = true };
        }
    }
}
