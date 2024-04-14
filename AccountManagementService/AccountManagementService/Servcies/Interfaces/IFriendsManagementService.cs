using AccountManagementService.DTOs;
using AccountManagementService.Models;

namespace AccountManagementService.Servcies.Interfaces
{
    public interface IFriendsManagementService
    {
        Task<ServiceResponse<List<User>>> GetFriends(int userId);
        Task<ServiceResponse<bool>> AddFriend(int userId, int friendUserId);
        Task<ServiceResponse<bool>> RemoveFriend(int userId, int friendUserId);
    }
}
