using DatabaseService.DTOs;
using DatabaseService.Models;

namespace DatabaseService.Services.Interfaces
{
    public interface IFriendsService
    {
        Task<ServiceResponse<List<UserDTO>>> GetFriends(int userId);
        Task<ServiceResponse<bool>> AddFriend(FriendRelationDTO friend);
        Task<ServiceResponse<bool>> RemoveFriend(FriendRelationDTO friend);
    }
}
