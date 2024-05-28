using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.Services.Interfaces
{
    public interface IGroupsInfoService
    {
        Task<ServiceResponse<GroupChatInfoDto>> UpdateInfo(GroupChatInfoDto groupChatInfoDto);
        Task<ServiceResponse<string>> UpdateProfilePicture(int chatId, string avatarURL);
    }
}
