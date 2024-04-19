using DatabaseService.DataContexts;
using DatabaseService.DTOs;
using DatabaseService.Models;
using DatabaseService.Models.DatabaseModels;
using DatabaseService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatabaseService.Services
{
    public class FriendsService : IFriendsService
    {
        private DatabaseContext _databaseContext;

        public FriendsService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<ServiceResponse<bool>> AddFriend(FriendRelationDTO friend)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            bool alreadyFriend = await _databaseContext.FriendRelations.AnyAsync(x => x.UserId == friend.UserId && x.FriendId == friend.FriendUserId);

            if (alreadyFriend)
            {
                response.Data = true;
                response.ErrorMessage = $"This user with account id={friend.FriendUserId} is already friend";
                return response;
            }

            FriendRelation newFriend = new FriendRelation();
            newFriend.UserId = friend.UserId;
            newFriend.FriendId = friend.FriendUserId;

            _databaseContext.FriendRelations.Add(newFriend);
            await _databaseContext.SaveChangesAsync();

            response.Data = true;
            return response;
        }

        public async Task<ServiceResponse<List<UserDTO>>> GetFriends(int userId)
        {
            var friends = _databaseContext.FriendRelations.Where(x => x.UserId == userId)
                                                          .Select(x => new UserDTO
                                                          {
                                                              Id = x.Friend.Id,
                                                              Username = x.Friend.Username,
                                                              DisplayName = x.Friend.DisplayName
                                                          })
                                                          .ToList();

            return new ServiceResponse<List<UserDTO>> { Data = friends };
        }

        public async Task<ServiceResponse<bool>> RemoveFriend(FriendRelationDTO friend)
        {
            var friendToDelete = await _databaseContext.FriendRelations.FirstOrDefaultAsync(x => x.UserId == friend.UserId &&
                                                                                    x.FriendId == friend.FriendUserId);

            if (friendToDelete == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = $"User with accountid={friend.FriendUserId} not your friend"
                };
            }

            _databaseContext.Remove(friendToDelete);
            await _databaseContext.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
