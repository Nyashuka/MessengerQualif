using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Windows;
using MessengerWithRoles.WPFClient.Common;

namespace MessengerWithRoles.WPFClient.Services
{
    public class PersonalChatsService : IService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public PersonalChatsService()
        {
            _httpClient = new HttpClient();
            _authService = ServiceLocator.Instance.GetService<AuthService>();
        }

        public async Task<ChatViewModel> GetPersonalChatIfExistsOrCreateOne(User parcipient)
        {
            var chatResponse = await GetPersonalChatByMembers(parcipient);
            
            if(chatResponse.Data == null)
            {
                return await CreatePersonalChat(parcipient);
            }

            return await GetExistsPersonalChat(chatResponse.Data.Id);
        }

        private async Task<ServiceResponse<Chat>> GetPersonalChatByMembers(User parcipient)
        {
            List<User> members = new List<User> { _authService.User, parcipient };

            var result = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.GetPersonalChatByMembersPOST}?accessToken={_authService.AccessToken}", 
                                 members);

            var chat = await result.Content.ReadFromJsonAsync<ServiceResponse<Chat>>();

            return chat;
        }

        public async Task<ChatViewModel> GetExistsPersonalChat(int chatId)
        {
            var chatDto = await TryGetChatIfExists(chatId);
            var chat = chatDto.Data;

            var messages = await GetChatMessages(chatId);

            var parcipient = chat.Members.FirstOrDefault(x => x.Id != _authService.User.Id);  

            return new ChatViewModel(chat.Id, parcipient.DisplayName, "", messages, parcipient);
        }

        private async Task<ChatViewModel> CreatePersonalChat(User parcipient)
        {
            var dataForRequest = new CreateChatDto()
            {
                ChatTypeId = Convert.ToInt32(ChatTypeEnum.personal),
                Members = new List<User>() { _authService.User, parcipient }
            };

            var result = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.CreatePersonalChatPOST}?accessToken={_authService.AccessToken}", 
                                                                                    dataForRequest);

            var createdChat = await result.Content.ReadFromJsonAsync<ServiceResponse<Chat>>();
            if (createdChat == null)
            {
                MessageBox.Show(result.ReasonPhrase);
            }

            if (!createdChat.Success)
            {
                MessageBox.Show(createdChat.Message);
            }

            var chat = createdChat.Data;

            return new ChatViewModel(chat.Id, parcipient.Username, "", new ObservableCollection<Message>(), parcipient);
        }

        public async Task<ServiceResponse<Chat>> TryGetChatIfExists(int chatId)
        {
            var response = await _httpClient
                .GetAsync($"{APIEndpoints.GetChatById}?accessToken={_authService.AccessToken}&chatId={chatId}");

            var chatData = await response.Content.ReadFromJsonAsync<ServiceResponse<Chat>>();

            return chatData;
        }

        public async Task<ObservableCollection<Message>> GetChatMessages(int chatId)
        {
            var messagesResponse = await _httpClient.GetAsync($"{APIEndpoints.GetChatMessagesByChatIdGET}?accessToken={_authService.AccessToken}&chatId={chatId}");
            var messagesResponseData = await messagesResponse.Content.ReadFromJsonAsync<ServiceResponse<List<MessageDto>>>();

            ObservableCollection<Message> chatMessages = new ObservableCollection<Message>();
            foreach (var currentMessage in messagesResponseData.Data)
            {
                chatMessages.Add(new Message(currentMessage.Id, currentMessage.Sender.DisplayName, 
                    currentMessage.Sender.AvatarURL,
                    currentMessage.Data, _authService.User.Id != currentMessage.Sender.Id));
            }

            return chatMessages;
        }
    }
}
