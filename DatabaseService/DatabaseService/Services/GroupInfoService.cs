using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class GroupInfoService : IGroupsInfoService
    {
        private readonly DatabaseContext _databaseContext;

        public GroupInfoService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<GroupChatInfoDto>> UpdateInfo(GroupChatInfoDto groupChatInfoDto)
        {
            var groupInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == groupChatInfoDto.ChatId);
            
            if(groupInfo == null)
            {
                return new ServiceResponse<GroupChatInfoDto>()
                {
                    Success = false,
                    Message = "Group does not exists"
                };
            }

            groupInfo.Name = groupChatInfoDto.Name;
            groupInfo.Description = groupChatInfoDto.Description;

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<GroupChatInfoDto>()
            {
                Data = new GroupChatInfoDto()
                {
                    ChatId = groupInfo.ChatId,
                    Owner = groupChatInfoDto.Owner,
                    Name = groupInfo.Name,
                    Description = groupInfo.Description,
                    AvatarUrl = groupInfo.AvatarUrl,
                },
            };
        }

        public async Task<ServiceResponse<string>> UpdateProfilePicture(int chatId, string avatarURL)
        {
            var groupInfo = await _databaseContext.GroupChatInfos.FirstOrDefaultAsync(x => x.ChatId == chatId);

            if (groupInfo == null)
            {
                return new ServiceResponse<string>()
                {
                    Success = false,
                    Message = "Group does not exists"
                };
            }

            groupInfo.AvatarUrl = avatarURL;

            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<string>()
            {
                Data = avatarURL
            };
        }
    }
}
