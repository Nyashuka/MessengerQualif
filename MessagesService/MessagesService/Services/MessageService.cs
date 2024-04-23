using MessagesService.DTOs;
using MessagesService.Models;
using MessagesService.Models.Requests;
using MessagesService.Services.Interfaces;
using Microsoft.AspNetCore.Routing.Matching;

namespace MessagesService.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;

        public MessageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task HandleMessage(int senderId, string accessToken, ClientMessageDTO clientMessageDTO)
        {
            clientMessageDTO.SenderId = senderId;
            clientMessageDTO.Timestamp = DateTime.UtcNow;

            var saveResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.SaveMessagePOST, clientMessageDTO);
            var message = await saveResponse.Content.ReadFromJsonAsync<ServiceResponse<ChatMessage>>();

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
        }
    }
}
