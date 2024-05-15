using ChatsService.ChatMembers.Dto;
using ChatsService.Groups.Dto;
using ChatsService.Models;

namespace ChatsService.PersonalChats.Services
{
    public class PersonalChatsService : IPersonalChatsService
    {
        private readonly HttpClient _httpClient;

        public PersonalChatsService(HttpClient httpClient)
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

        public async Task<ServiceResponse<ChatDto>> GetPersonalChatByMembers(List<UserDto> users)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.GetPersonalChat_POST, users);

            var responseData = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return responseData;
        }
    }
}