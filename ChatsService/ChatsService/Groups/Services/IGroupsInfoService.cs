using ChatsService.Groups.Dto;
using ChatsService.Groups.Models;
using ChatsService.Models;

namespace ChatsService.Groups.Services
{
    public interface IGroupsInfoService
    {
        Task<ServiceResponse<GroupChatInfoDto>> UpdateGroupInfo(GroupChatInfoDto groupChatInfoDto);
        Task<ServiceResponse<string>> UpdateGroupPicture(int chatId, string filePath);
    }
}
