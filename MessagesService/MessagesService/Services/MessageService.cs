using MessagesService.DTOs;
using MessagesService.Services.Interfaces;

namespace MessagesService.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task HandleMessage(int senderId, ClientMessageDTO clientMessageDTO)
        {

        }
    }
}
