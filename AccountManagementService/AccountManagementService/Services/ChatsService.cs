using AccountManagementService.DTOs;
using AccountManagementService.Models;
using AccountManagementService.Services.Interfaces;

namespace AccountManagementService.Services
{
    public class ChatsService : IChatsService
    {
        private readonly HttpClient _httpClient;

        public ChatsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<ChatDto>> CreatePersonalChatIfNotExists(ChatDto chatDto)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreatePersonalChat_POST, chatDto);

            var responseData = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return responseData;
        }

        public async Task<ServiceResponse<List<ChatDto>>> GetAllPersonalChats(int userId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetAllPersonalChats_GET}?userId={userId}");

            var responseData = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<List<ChatDto>>>();

            return responseData;
        }

        public async Task<ServiceResponse<ChatDto>> GetPersonalChat(List<UserDto> users)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.GetPersonalChat_POST, users);

            var responseData = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return responseData;
        }

        public Task<ServiceResponse<Chat>> CreateGroupChatIfNotExists(ChatDto chatDto)
        {
            throw new NotImplementedException();
        }
    }
}
