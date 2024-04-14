using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Servcies.Interfaces;

namespace AccountManagementService.Servcies
{
    public class FriendsManagementServcie : IFriendsManagementService
    {
        private HttpClient _httpClient;

        public FriendsManagementServcie(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<bool>> AddFriend(int userId, int friendUserId)
        {
            var friendRelation = new FriendRelationDTO()
            {
                UserId = userId,
                FriendUserId = friendUserId
            };

            var response = await _httpClient.PostAsJsonAsync(APIEndpoints.AddFriendPOST, friendRelation);

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<bool>()
                {
                    Data = false,
                    Success = false,
                    ErrorMessage = response.ReasonPhrase
                };
            }

            return new ServiceResponse<bool>()
            {
                Data = true,
            };
        }

        public async Task<ServiceResponse<List<User>>> GetFriends(int userId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetFriendsGET}?userId={userId}");
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>(); 

            return new ServiceResponse<List<User>>()
            {
                Data = data.Data
            };
        }

        public async Task<ServiceResponse<bool>> RemoveFriend(int userId, int friendUserId)
        {
            var friendRelation = new FriendRelationDTO()
            {
                UserId = userId,
                FriendUserId = friendUserId
            };

            var response = await _httpClient.PostAsJsonAsync(APIEndpoints.RemoveFriendPOST, friendRelation);
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return new ServiceResponse<bool>()
            {
                Data = data.Data
            };
        }
    }
}
