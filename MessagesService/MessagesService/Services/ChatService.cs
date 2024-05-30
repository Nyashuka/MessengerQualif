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

        public async Task<bool> IsUserChatMember(int chatId, int userId)
        {
            var chat = await GetChatById(chatId);

            if (chat.Data.Members.Any(x => x.Id == userId))
            {
                return true;
            }

            return false;
        }

        public async Task<ServiceResponse<ChatDto>> GetChatById(int chatId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatByIdGET}?chatId={chatId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return data;
        }

        public async Task<ServiceResponse<ChatDto>> GetChatByMessageId(int messageId)
        {
            var databaseResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatByMessageIdGET}?messageId={messageId}");

            var data = await databaseResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            return data;
        }
    }
}
