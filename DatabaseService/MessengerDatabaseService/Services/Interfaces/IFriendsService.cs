using MessengerDatabaseService.DTOs;
using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.Services.Interfaces
{
    public interface IFriendsService
    {
        Task<ServiceResponse<List<UserDTO>>> GetFriends(int userId);
        Task<ServiceResponse<bool>> AddFriend(FriendRelationDTO friend);
        Task<ServiceResponse<bool>> RemoveFriend(FriendRelationDTO friend);
    }
}
