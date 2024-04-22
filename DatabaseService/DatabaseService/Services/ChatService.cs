using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class ChatService : IChatService
    {
        private readonly DatabaseContext _databaseContext;

        public ChatService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<ServiceResponse<List<ChatDto>>> GetAllPersonalChats(int userId)
        {
            var chats = _databaseContext.Chats.Where(c => c.ChatTypeId == 0 && 
                                                           _databaseContext.ChatMembers.Any(m => m.ChatId == c.Id && m.UserId == userId)).ToList();
            
            List<ChatDto> chatListForResponse = new List<ChatDto>();
            foreach (var chat in chats)
            {
                var chatMembers = 
                    _databaseContext.ChatMembers.Where(cm => cm.ChatId == chat.Id).ToList();

                var users = new List<UserDto>();
                foreach (var member in chatMembers)
                {
                    User user =_databaseContext.Users.First(u => u.Id == member.UserId);
                    users.Add(new UserDto()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        DisplayName = user.DisplayName,
                    });
                }

                chatListForResponse.Add(new ChatDto()
                {
                    ChatTypeId = chat.ChatTypeId,
                    Members = users
                });
            }

            return Task.FromResult( new ServiceResponse<List<ChatDto>>()
            {
                Data = chatListForResponse
            });
        }

        public Task<ServiceResponse<ChatDto>> GetPersonalChatIfExists(List<UserDto> usersDto)
        {
            var userIds = usersDto.Select(i => i.Id).ToList();

            var chatWithUsers = _databaseContext.ChatMembers
                .Where(cm => userIds.Contains(cm.UserId) && cm.Chat.ChatTypeId == 0)
                .GroupBy(cm => cm.ChatId)
                .Where(g => g.Count() == userIds.Count)
                .Select(g => g.Key)
                .ToList();

            var existsChat = _databaseContext.ChatMembers
                .Where(cm => chatWithUsers.Contains(cm.ChatId))
                .GroupBy(cm => cm.ChatId)
                .Where(g => g.Count() == userIds.Count)
                .Select(g => g.FirstOrDefault().Chat)
                .FirstOrDefault();

            if (existsChat == null)
            {
                return Task.FromResult(new ServiceResponse<ChatDto>() { Data = null });
            }

            ChatDto chatData = new ChatDto()
            {
                ChatTypeId = existsChat.ChatTypeId,
                Members = usersDto,
            };

            return Task.FromResult(new ServiceResponse<ChatDto>() { Data = chatData });
        }

        public async Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDTO)
        {
            var existsChat = await GetPersonalChatIfExists(chatDTO.Members);

            if (existsChat.Data != null)
                return new ServiceResponse<ChatDto>() { Success = false, ErrorMessage = "Already exists"};

            var chatType = _databaseContext.ChatTypes.FirstOrDefault(x => x.Id == chatDTO.ChatTypeId);

            if (chatType == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    ErrorMessage = $"Chat type with id={chatDTO.ChatTypeId} is not exists"
                };
            }

            var newChat = new Chat()
            {
                ChatTypeId = chatDTO.ChatTypeId,
                ChatType = chatType
            };

            _databaseContext.Chats.Add(newChat);
            await _databaseContext.SaveChangesAsync();

            foreach (var memberDto in chatDTO.Members)
            {
                var member = new ChatMember
                {
                    ChatId = newChat.Id,
                    UserId = memberDto.Id
                };

                _databaseContext.ChatMembers.Add(member);
            }
            await _databaseContext.SaveChangesAsync();

            ChatDto chatForResponse = new ChatDto()
            {
                ChatTypeId = chatDTO.ChatTypeId,
                Members = chatDTO.Members
            };

            return new ServiceResponse<ChatDto> { Data = chatForResponse };
        }

        public async Task<ServiceResponse<bool>> DeleteChat(int id)
        {
            var chatToUpdate = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == id);

            if (chatToUpdate == null)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = $"Chat with id={id} is not exists!"
                };
            }

            _databaseContext.Chats.Remove(chatToUpdate);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
