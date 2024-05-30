using MessagesService.ActionAccess.Actions;
using MessagesService.ActionAccess.Data;
using MessagesService.ActionAccess.Services;
using MessagesService.Chat.Dto;
using MessagesService.DTOs;
using MessagesService.Models;
using MessagesService.Models.Requests;
using MessagesService.Services.Interfaces;

namespace MessagesService.Services
{
    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly IChatService _chatService;
        private readonly IActionAccessService _actionAccessService;

        public MessageService(HttpClient httpClient, IChatService chatService, IActionAccessService actionAccessService)
        {
            _httpClient = httpClient;
            _chatService = chatService;
            _actionAccessService = actionAccessService;
        }

        public async Task<ServiceResponse<List<MessageDto>>> GetAllChatMessages(int chatId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetChatMessagesByChatIdGET}?chatId={chatId}");
            var messages = await response.Content.ReadFromJsonAsync<ServiceResponse<List<MessageDto>>>();

            return messages;
        }

        private async Task<ServiceResponse<MessageDto>> SaveMessage(ClientMessageDTO clientMessageDTO)
        {
            var saveResponse = await _httpClient.PostAsJsonAsync(APIEndpoints.SaveMessagePOST, clientMessageDTO);

            return await saveResponse.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();
        }

        public async Task<ServiceResponse<MessageDto>> SendMessage(int senderId, string accessToken, string clientGuid, ClientMessageDTO clientMessageDTO)
        {
            clientMessageDTO.SenderId = senderId;
            clientMessageDTO.Timestamp = DateTime.UtcNow;

            var message = await SaveMessage(clientMessageDTO);

            await SendNotifySendMessage(senderId, accessToken, clientGuid, clientMessageDTO.ChatId, message.Data);

            return message;
        }

        private async Task SendNotifySendMessage(int senderId, string accessToken, string clientGuid, int chatId, MessageDto message)
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

        public async Task<ServiceResponse<MessageDto>> GetMessageById(int messageId)
        {
            var response = await _httpClient.GetAsync($"{APIEndpoints.GetChatMessageByIdGET}?messageId={messageId}");
            var message = await response.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();

            return message;
        }

        public async Task<ServiceResponse<bool>> DeleteMessage(int senderId, string accessToken, string clientGuid, int messageId)
        {

            var chat = await _chatService.GetChatByMessageId(messageId);

            if (!await _chatService.IsUserChatMember(chat.Data.Id, senderId))
                return new ServiceResponse<bool>() { Data = false, Success = false, Message = "You dont have access to this chat" };

            if (chat == null || chat.Data == null)
            {
                throw new Exception("Chat with this message not exists.");
            }

            var messageToDelete = await GetMessageById(messageId);

            bool deletedSuccess = false;

            if (messageToDelete.Data.SenderId == senderId)
            {
                deletedSuccess = await DeleteMessageFromDb(messageToDelete.Data.Id);
            }
            else if (!IsPersonalChat(chat.Data))
            {
                if (await HasAccessToDeleteMessage(messageId, chat.Data.Id, senderId))
                {
                    deletedSuccess = await DeleteMessageFromDb(messageToDelete.Data.Id);
                }
                else
                {
                    return new ServiceResponse<bool>()
                    {
                        Data = false,
                        Success = false,
                        Message = "You don't have access to delete messages"
                    };
                }
            }

            if (deletedSuccess)
            {
                await SendNotifyDeleteMessage(senderId, accessToken, clientGuid, chat.Data.Id, messageId);
                return new ServiceResponse<bool>() { Data = true };
            }

            return new ServiceResponse<bool>() { Data = false, Success = false, Message = "Delete message error." };
        }

        private async Task SendNotifyDeleteMessage(int senderId, string accessToken, string clientGuid, int chatId, int messageId)
        {
            var chatMemberResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatMembersGET}?chatId={chatId}");
            var chatMembers = (await chatMemberResponse.Content.ReadFromJsonAsync<ServiceResponse<List<UserDto>>>()).Data;

            if (chatMembers != null && chatMembers.Count > 0)
            {
                List<int> chatMemberIds = chatMembers.Where(u => u.Id != senderId).Select(x => x.Id).ToList();

                if (chatMembers.Count == 0)
                    return;

                var notifyData = new NotifyDeleteMessageDto()
                {
                    ChatId = chatId,
                    MessageId = messageId,
                    Recipients = chatMemberIds
                };

                var notifyResponse = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.NotifyUsersDeleteMessagePOST}?accessToken={accessToken}&clientGuid={clientGuid}", notifyData);
            }
        }

        private async Task<bool> DeleteMessageFromDb(int messageId)
        {
            var response = await _httpClient.DeleteAsync($"{APIEndpoints.DeleteMessageDELETE}?messageId={messageId}");
            var messages = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return messages.Data;
        }

        private async Task<bool> HasAccessToDeleteMessage(int messageId, int chatId, int userId)
        {
            var data = new DeleteMessageChatActionData()
            {
                RequesterId = userId,
                ChatId = chatId,
                MessageId = messageId
            };

            var result = await _actionAccessService.HasAccess(new DeleteMessageChatAction(data), chatId, userId);

            return result;
        }

        private bool IsPersonalChat(ChatDto chat)
        {
            return chat.ChatTypeId == 0;
        }
    }
}
