using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using System.Net.Http;
using System.Net.Http.Json;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Data.Requests;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private User _user;
        public User User { get => _user; set => Set(ref _user, value); }

        public ICommand AddToFriend { get; }
        private bool CanAddToFriendCommandExecute(object p) => true;
        private async void OnAddToFriendCommandExecute(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{APIEndpoints.AddFriendGET}?accessToken={authService.AccessToken}&friendUserId={User.Id}");
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            if (data == null)
            {
                MessageBox.Show("Cant pars data");
                return;
            }

            if (!data.Success)
            {
                MessageBox.Show(data.Message);
                return;
            }

            MessageBox.Show("Added to friends " + User.DisplayName);
        }

        public ICommand CreateOrOpenChat { get; }
        public bool CanExecuteCreateOrOpenChatCommand(object p) => true;

        public async void OnExecuteCreateOrOpenChatCommand(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            HttpClient httpClient = new HttpClient();

            List<User> members = new List<User>()
            {
                authService.User,
                User
            };

            var result = await httpClient.PostAsJsonAsync($"{APIEndpoints.GetPersonalChatPOST}?accessToken={authService.AccessToken}", members);
            var chat = await result.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();

            if (chat.Data != null)
            {
                ServiceLocator.Instance.GetService<EventBus>()
                    .Raise(EventBusDefinitions.OpenChat,
                        new ChatDataIEventBusArgs(ChatDtoToChat(chat.Data, authService.User.Id)));
                return;
            }

            var dataForRequest = new CreateChatDto()
            {
                ChatTypeId = 0,
                Members = new List<User>() { authService.User, User }
            };


            result = await httpClient.PostAsJsonAsync($"{APIEndpoints.CreatePersonalChatPOST}?accessToken={authService.AccessToken}", dataForRequest);

            var createdChat = await result.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>();
            if (createdChat == null)
            {
                MessageBox.Show(result.ReasonPhrase);
                return;
            }

            if (!createdChat.Success)
            {
                MessageBox.Show(createdChat.Message);
                return;
            }

            ServiceLocator.Instance.GetService<EventBus>()
                .Raise(EventBusDefinitions.ChatCreated, new ChatDataIEventBusArgs(ChatDtoToChat(createdChat.Data, authService.User.Id)));
        }

        private ChatViewModel ChatDtoToChat(ChatDto chatDto, int userId)
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();

            return new ChatViewModel(chatDto.Id, chatDto.Members.First(u => u.Id != userId).DisplayName,
                 "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                 new ObservableCollection<Message>(), chatDto.Members.FirstOrDefault(m => m.Id != authService.User.Id));
        }

        public UserViewModel(User user)
        {
            User = user;

            AddToFriend = new LambdaCommand(OnAddToFriendCommandExecute, CanAddToFriendCommandExecute);
            CreateOrOpenChat = new LambdaCommand(OnExecuteCreateOrOpenChatCommand, CanExecuteCreateOrOpenChatCommand);
        }
    }
}
