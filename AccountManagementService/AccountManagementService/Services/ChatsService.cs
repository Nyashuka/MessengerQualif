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

        public async Task<ServiceResponse<ChatDto>> CreateGroupChatIfNotExists(ChatDto chatDto)
        {
            if(chatDto.ChatTypeId == 1)
            {
                return await CreateGroupChat(chatDto);
            }

            return new ServiceResponse<ChatDto>() { Success = false, ErrorMessage = "Chat type is not exists!"};
        }

        private async Task<ServiceResponse<ChatDto>> CreateGroupChat(ChatDto chatDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.CreateGroupChat_POST}", chatDto);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return responseData;
        }

        private async Task<ServiceResponse<ChatDto>> CreatePersonalChat(ChatDto chatDto)
        {
            var databaseResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.CreatePersonalChat_POST, chatDto);

            var responseData = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return responseData;
        }

        public async Task<ServiceResponse<ChatDto>> GetPersonalChatById(int chatId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetChatByIdGET}?chatId={chatId}");
            var chat = await response.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return chat;
        }
    }
}
