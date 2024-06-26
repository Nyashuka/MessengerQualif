﻿using DatabaseService.Data;
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
        private readonly IChatMembersService _chatMembersService;

        public ChatService(DatabaseContext databaseContext, IChatMembersService chatMembersService)
        {
            _databaseContext = databaseContext;
            _chatMembersService = chatMembersService;
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
                        AvatarURL = user.AvatarURL,
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
            var chat = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == chatId);

            if (chat == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat does not exists"
                };
            }          

            var chatToResponse = new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = (await _chatMembersService.GetChatMembersByChatId(chatId)).Data,
                ChatInfo = chat.ChatTypeId == Convert.ToInt32(ChatTypeEnum.group) ? await GetChatInfoDto(chatId) : null
            };

            return new ServiceResponse<ChatDto>() { Data = chatToResponse };
        }

        private async Task<GroupChatInfoDto> GetChatInfoDto(int chatId)
        {
            var groupChatInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync();
            var owner = await _databaseContext.Users.FirstAsync(x => x.Id == groupChatInfo.OwnerId);

            return new GroupChatInfoDto()
            {
                Owner = new UserDto()
                {
                    Id = owner.Id,
                    DisplayName = owner.DisplayName,
                    Username = owner.Username,
                    AvatarURL = owner.AvatarURL,
                },
                Name = groupChatInfo.Name,
                Description = groupChatInfo.Description,
                AvatarUrl = groupChatInfo.AvatarUrl,
            };
        }

        public async Task<ServiceResponse<ChatDto>> GetChatByMessageId(int messageId)
        {
            var message = await _databaseContext.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

            if (message == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat or message does not exists"
                };
            }

            var chat = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == message.ChatId);

            if (chat == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat does not exists"
                };
            }

            var chatToResponse = new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = (await _chatMembersService.GetChatMembersByChatId(chat.Id)).Data,
                ChatInfo = chat.ChatTypeId == Convert.ToInt32(ChatTypeEnum.group) ? await GetChatInfoDto(chat.Id) : null
            };

            return new ServiceResponse<ChatDto>() { Data = chatToResponse };
        }

        public async Task<ServiceResponse<ChatDto>> GetChatByRoleId(int roleId)
        {
            var role = await _databaseContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId);

            if (role == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat or message does not exists"
                };
            }

            var chat = await _databaseContext.Chats.FirstOrDefaultAsync(x => x.Id == role.ChatId);

            if (chat == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = "Chat does not exists"
                };
            }

            var chatToResponse = new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = (await _chatMembersService.GetChatMembersByChatId(chat.Id)).Data,
                ChatInfo = chat.ChatTypeId == Convert.ToInt32(ChatTypeEnum.group) ? await GetChatInfoDto(chat.Id) : null
            };

            return new ServiceResponse<ChatDto>() { Data = chatToResponse };
        }
    }
}
