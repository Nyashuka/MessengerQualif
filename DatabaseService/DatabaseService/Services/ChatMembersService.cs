using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class ChatMembersService : IChatMembersService
    {
        private readonly DatabaseContext _databaseContext;

        public ChatMembersService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetChatMembersByChatId(int chatId)
        {
            var chatMembers = _databaseContext.ChatMembers.Where(x => x.ChatId == chatId).ToList();

            List<UserDto> chatMemberUsersDto = new List<UserDto>();
            foreach (var chatMember in chatMembers)
            {
                var user = await _databaseContext.Users.FirstOrDefaultAsync(x => chatMember.UserId == x.Id);

                if (user == null)
                    continue;

                chatMemberUsersDto.Add(new UserDto() 
                {
                    Id = user.Id,
                    Username = user.Username,
                    DisplayName = user.DisplayName,
                });
            }

            return new ServiceResponse<List<UserDto>> { Data = chatMemberUsersDto };
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
                    Message = $"Chat member with id={chatMemberDto.UserId} is not exists!"
                };
            }

            _databaseContext.ChatMembers.Remove(chatMember);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> {  Data = true };
        }
    }
}
