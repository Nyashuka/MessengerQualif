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
    }
}
