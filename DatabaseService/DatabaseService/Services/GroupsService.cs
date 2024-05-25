using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class GroupsService : IGroupsService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRolesService _rolesService;

        public GroupsService(DatabaseContext databaseContext, IRolesService rolesService)
        {
            _databaseContext = databaseContext;
            _rolesService = rolesService;
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

            List<UserDto> members = _databaseContext.ChatMembers
                                    .Where(cm => cm.ChatId == chatId)
                                    .Select(cm => new UserDto
                                    {
                                        Id = cm.User.Id,
                                        Username = cm.User.Username,
                                        DisplayName = cm.User.DisplayName
                                    })
                                    .ToList();

            var chatInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == chatId);

            var owner = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == chatInfo.OwnerId);
            var ownerDto = new UserDto()
            {
                Id = owner.Id,
                DisplayName = owner.DisplayName,
                Username = owner.Username,
            };

            var chatInfoDto = new GroupChatInfoDto()
            {
                Name = chatInfo.Name,
                Description = chatInfo.Description,
                AvatarUrl = chatInfo.AvatarUrl,
                Owner = ownerDto
            };

            ChatDto chatToResponse = new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = members,
                ChatInfo = chatInfoDto
            };

            return new ServiceResponse<ChatDto>() { Data = chatToResponse };
        }

        private GroupChatInfoDto? ValidChatInfoData(GroupChatInfoDto chatInfoDto)
        {
            if(chatInfoDto.Owner == null)
            {
                return null;
            }

            if(string.IsNullOrEmpty(chatInfoDto.Name))
            {
                return null;
            }

            return new GroupChatInfoDto()
            {
                Owner = chatInfoDto.Owner,
                Name = chatInfoDto.Name,
                Description = string.IsNullOrEmpty(chatInfoDto.Description) ? "" : chatInfoDto.Description,
                AvatarUrl = string.IsNullOrEmpty(chatInfoDto.AvatarUrl) ? "" : chatInfoDto.AvatarUrl,
            };
        }

        public async Task<ServiceResponse<ChatDto>> CreateGroup(ChatDto chatDto)
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

            if(chatDto.ChatInfo == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Chat info cant be empty for creating group"
                };
            }

            var validChatInfoDto = ValidChatInfoData(chatDto.ChatInfo);

            if(validChatInfoDto == null)
            {
                return new ServiceResponse<ChatDto>()
                {
                    Data = null,
                    Success = false,
                    Message = $"Chat info has not valid data"
                };
            }

            var newChat = new Chat()
            {
                ChatTypeId = chatDto.ChatTypeId,
                ChatType = chatType
            };
            _databaseContext.Chats.Add(newChat);
            await _databaseContext.SaveChangesAsync();

            var chatInfo = new GroupChatInfo()
            {
                ChatId = newChat.Id,
                Chat = newChat,
                OwnerId = validChatInfoDto.Owner.Id,
                Owner = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Id == validChatInfoDto.Owner.Id),
                Name = validChatInfoDto.Name,
                Description = validChatInfoDto.Description,
                AvatarUrl = validChatInfoDto.AvatarUrl
            };
            _databaseContext.GroupChatInfos.Add(chatInfo);
            await _databaseContext.SaveChangesAsync();

            var defaultRoleCreatingResponse = await _rolesService.CreateDefaultRole(newChat.Id);

            var responseChat = new ChatDto()
            {
                Id = newChat.Id,
                ChatTypeId = newChat.ChatTypeId,
                Members = new List<UserDto>(),
                ChatInfo = new GroupChatInfoDto()
                {
                    Owner = chatDto.ChatInfo.Owner,
                    Name = chatInfo.Name,
                    Description = chatInfo.Description,
                    AvatarUrl = chatInfo.AvatarUrl,
                }
            };

            return new ServiceResponse<ChatDto>() { Data = responseChat };
        }

        private ChatDto GetChatDtoFromModel(Chat chat)
        {
            var groupInfo = _databaseContext.GroupChatInfos.First(x => x.ChatId == chat.Id);

            var members = _databaseContext.ChatMembers
                                 .Where(cm => cm.ChatId == chat.Id)
                                 .Select(cm => new UserDto
                                 {
                                     Id = cm.User.Id,
                                     Username = cm.User.Username,
                                     DisplayName = cm.User.DisplayName
                                 })
                                 .ToList();
            var owner = _databaseContext.Users.Select(x => new UserDto()
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Username = x.Username
            }).First(x => x.Id == groupInfo.OwnerId);

            members.Add(owner);

            return new ChatDto()
            {
                Id = chat.Id,
                ChatTypeId = chat.ChatTypeId,
                Members = members,
                ChatInfo = new GroupChatInfoDto()
                {
                    Owner = owner,
                    Name = groupInfo.Name,
                    Description = groupInfo.Description,
                    AvatarUrl = groupInfo.AvatarUrl,
                }
            };
        }

        public async Task<ServiceResponse<List<ChatDto>>> GetAllGroups(int userId)
        {
            var responseData = new List<ChatDto>();

            // 1 - group chat type id
            var groups = _databaseContext.Chats.Where(x => x.ChatTypeId == 1).ToList();

            foreach (var group in groups)
            {
                if (await _databaseContext.ChatMembers.AnyAsync(x => x.ChatId == group.Id && x.UserId == userId))
                {
                    responseData.Add(GetChatDtoFromModel(group));
                    continue;
                }

                var groupInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == group.Id);
                if (groupInfo.OwnerId == userId)
                {
                    responseData.Add(GetChatDtoFromModel(group));
                }
            }

            return new ServiceResponse<List<ChatDto>>() { Data = responseData };
        }

    }
}
