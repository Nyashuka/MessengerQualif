using ChatsService.Groups.Dto;
using ChatsService.Groups.Interfaces;
using ChatsService.Groups.Models;
using ChatsService.Models;

namespace ChatsService.Groups
{
    public class GroupsService : IGroupsService
    {
        private readonly HttpClient _httpClient;

        public GroupsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<Chat>> CreateGroup(ChatDto chatDto)
        {
            var createGroupResposne = await _httpClient.PostAsJsonAsync(APIEndpoints.CreateGroupPOST, chatDto);

            if (createGroupResposne == null)
            {
                return new ServiceResponse<Chat>() { Success = false, Message = "Can't get response from database." };
            }

            var chatData = await createGroupResposne.Content.ReadFromJsonAsync<ServiceResponse<Chat>>();

            if (chatData == null)
            {
                return new ServiceResponse<Chat>() { Success = false, Message = "Can't parse data got from database service." };
            }

            if (chatData.Data == null)
                return chatData;

            return chatData;
        }

        public async Task<ServiceResponse<List<Chat>>> GetAllGroupsByUserId(int userId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllGroupsByUserIdGET}?userId={userId}");

            if (!databaseResponse.IsSuccessStatusCode)
            {
                return new ServiceResponse<List<Chat>>() { Success = false, Message = "Database service error: " + databaseResponse.ReasonPhrase };
            }

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<Chat>>>();

            if(data == null)
            {
                return new ServiceResponse<List<Chat>>() 
                {
                    Success = false,
                    Message = "Chat's service can't parse data got from database service"
                };
            }

            return data;
        }
    }
}
