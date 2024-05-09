using AccountManagementService.Models;

namespace AccountManagementService.Services.GroupsManagement
{
    public interface IGroupManagementService
    {
        Task<ServiceResponse<User>> AddMember(User user);
        Task<ServiceResponse<User>> DeleteMember(User user);
        Task<ServiceResponse<GroupChatInfo>> ChangeGroupInfo(GroupChatInfo groupChatInfo);
    }
}
