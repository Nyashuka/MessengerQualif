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

        public async Task<ServiceResponse<MessageDto>> HandleMessage(int senderId, string accessToken, string clientGuid, ClientMessageDTO clientMessageDTO)
        {
            clientMessageDTO.SenderId = senderId;
            clientMessageDTO.Timestamp = DateTime.UtcNow;

            var saveResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.SaveMessagePOST, clientMessageDTO);
            var message = await saveResponse.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();

            await SendNotify(senderId, accessToken, clientGuid, clientMessageDTO.ChatId, message.Data);

            return message;
        }

        private async Task SendNotify(int senderId, string accessToken, string clientGuid, int chatId, MessageDto message)
        {
            var chatMemberResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatMembersGET}?chatId={chatId}");
            var chatMembers = (await chatMemberResponse.Content.ReadFromJsonAsync<ServiceResponse<List<UserDto>>>()).Data;

            if (chatMembers != null && chatMembers.Count > 0)
            {
                List<int> chatMemberIds = chatMembers.Where(u => u.Id != senderId).Select(x => x.Id).ToList();

                if (chatMembers.Count == 0)
                    return;

                var notifyData = new NotifyDataDto()
                {
                    Message = message,
                    Recipients = chatMemberIds
                };

                var notifyResponse = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.NotifyUsersSendingMessagePOST}?accessToken={accessToken}&clientGuid={clientGuid}", notifyData);
            }
        }
    }
}
