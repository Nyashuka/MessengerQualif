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
                    User user = _databaseContext.Users.First(u => u.Id == member.UserId);
                    users.Add(new UserDto()
                    {
                        Id = user.Id,
                        Username = user.Username,
                        DisplayName = user.DisplayName,
                    });
                }

                chatListForResponse.Add(new ChatDto()
                {
                    Id = chat.Id,
                    ChatTypeId = chat.ChatTypeId,
                    Members = users
                });
            }

            return Task.FromResult(new ServiceResponse<List<ChatDto>>()
            {
                Data = chatListForResponse
            });
        }

        public Chat? TryGetPersonalChatByUsers(List<UserDto> usersDto)
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

            return existsChat;
        }

        public Task<ServiceResponse<ChatDto>> GetPersonalChatIfExists(List<UserDto> usersDto)
        {
            var existsChat = TryGetPersonalChatByUsers(usersDto);

            if (existsChat == null)
            {
                return Task.FromResult(new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat is not exists!"
                });
            }

            ChatDto chatData = new ChatDto()
            {
                Id = existsChat.Id,
                ChatTypeId = existsChat.ChatTypeId,
                Members = usersDto,
            };

            return Task.FromResult(new ServiceResponse<ChatDto>() { Data = chatData });
        }

        public async Task<ServiceResponse<ChatDto>> CreateGroupChat(ChatDto chatDto)
        {
            var chatType = await _databaseContext.ChatTypes.FirstOrDefaultAsync(x => x.Id == chatDto.ChatTypeId);

            if (chatType == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Chat type with id={chatDto.ChatTypeId} is not exists"
                };
            }

            var newChat = new Chat()
            {
                ChatTypeId = chatDto.ChatTypeId,
                ChatType = chatType
            };

            var chatInfo = new GroupChatInfo()
            {
                ChatId = newChat.Id,
                Chat = newChat,
                OwnerId = chatDto.ChatInfo.OwnerId,
                Owner = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == chatDto.ChatInfo.OwnerId),
                Name = chatDto.ChatInfo.Name,
                Description = chatDto.ChatInfo.Description,
                AvatarUrl = chatDto.ChatInfo.AvatarUrl
            };
            _databaseContext.GroupChatInfos.Add(chatInfo);
            await _databaseContext.SaveChangesAsync();

            ChatDto chatForResponse = new ChatDto()
            {
                Id = newChat.Id,
                ChatTypeId = chatDto.ChatTypeId,
                Members = chatDto.Members,
                ChatInfo = chatInfo
            };
            _databaseContext.Chats.Add(newChat);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<ChatDto>() { Data = chatForResponse };
        }

        public async Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDTO)
        {
            var existsChat = await GetPersonalChatIfExists(chatDTO.Members);

            if (existsChat.Data != null)
                return new ServiceResponse<ChatDto>() { Success = false, Message = "Already exists" };

            var chatType = _databaseContext.ChatTypes.FirstOrDefault(x => x.Id == chatDTO.ChatTypeId);

            if (chatType == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Chat type with id={chatDTO.ChatTypeId} is not exists"
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
                Id = newChat.Id,
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
                    Message = $"Chat with id={id} is not exists!"
                };
            }

            _databaseContext.Chats.Remove(chatToUpdate);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<ChatDto>> GetChatById(int chatId)
        {
            Chat chat = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat == null)
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat does not exists"
                };

            List<UserDto> members = _databaseContext.ChatMembers
                     .Where(cm => cm.ChatId == chatId)
                     .Select(cm => new UserDto
                     {
                         Id = cm.User.Id,
                         Username = cm.User.Username,
                         DisplayName = cm.User.DisplayName
                     })
                     .ToList();

            ChatDto chatToResponse = new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = members,
                ChatInfo = _databaseContext.GroupChatInfos.FirstOrDefault(x => x.Id == chatId)
            };

            return new ServiceResponse<ChatDto>() { Data = chatToResponse };
        }
    }
}
