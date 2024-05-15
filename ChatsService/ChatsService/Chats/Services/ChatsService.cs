using ChatsService.Groups.Dto;
using ChatsService.Models;

namespace ChatsService.Chats.Services
{
    public class ChatsService : IChatsService
    {
        private readonly HttpClient _httpClient;

        public ChatsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<ChatDto>> GetChatById(int chatId)
        {
            var chatResponse = await _httpClient
                .GetAsync($"{APIEndpoints.GetChatByIdGET}?chatId={chatId}"); 
            
            var chatDto = await chatResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return chatDto;
        }
    }
}
