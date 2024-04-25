using MessagesService.DTOs;
using MessagesService.Models;
using MessagesService.Models.Requests;
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

        public async Task<ServiceResponse<List<MessageDto>>> GetAllChatMessages(int chatId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetChatMessagesByChatIdGET}?chatId={chatId}");
            var messages = await response.Content.ReadFromJsonAsync<ServiceResponse<List<MessageDto>>>();

            return messages;
        }

        public async Task<ServiceResponse<MessageDto>> HandleMessage(int senderId, string accessToken, ClientMessageDTO clientMessageDTO)
        {
            clientMessageDTO.SenderId = senderId;
            clientMessageDTO.Timestamp = DateTime.UtcNow;

            var saveResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.SaveMessagePOST, clientMessageDTO);
            var message = await saveResponse.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();

            var chatMemberResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatMembersGET}?chatId={clientMessageDTO.ChatId}");
            List<ChatMember> chatMembers = (await chatMemberResponse.Content.ReadFromJsonAsync<ServiceResponse<List<ChatMember>>>()).Data;

            List<int> chatMemberIds = chatMembers.Where(u => u.UserId != senderId).Select(x => x.UserId).ToList();

            var notifyData = new NotifyDataDto()
            {
                Message = message.Data,
                Recipients = chatMemberIds
            };

            var notifyResponse = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.NotifyUsersSendingMessagePOST}?accessToken={accessToken}", notifyData);

            return message;
        }
    }
}
