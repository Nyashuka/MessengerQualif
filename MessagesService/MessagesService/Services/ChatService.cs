using MessagesService.Chat.Dto;
using MessagesService.Models.Requests;
using MessagesService.Services.Interfaces;

namespace MessagesService.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;

        public ChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsUserChatMember(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<ChatDto>> GetChat(int chatId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatByIdGET}?chatId={chatId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return data;
        }
    }
}
