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
        public User User 
        {
            get => _user; 
            set => Set(ref _user, value); 
        }

        public event Action<User> AddToFriendUserEvent;
        public event Action<User> DeleteFriendEvent;

        public ICommand AddToFriend { get; }
        private bool CanAddToFriendCommandExecute(object p) => true;
        private async void OnAddToFriendCommandExecute(object p)
        {
            AddToFriendUserEvent?.Invoke(User);
        }

        public ICommand DeleteFriend { get; }
        private bool CanDeleteFriendExecute(object p) => true;
        private async void OnDeleteFriendExecute(object p)
        {
            DeleteFriendEvent?.Invoke(User);
        }


        public ICommand CreateOrOpenChat { get; }
        public bool CanExecuteCreateOrOpenChatCommand(object p) => true;

        public async void OnExecuteCreateOrOpenChatCommand(object p)
        {
            var chatsService = ServiceLocator.Instance.GetService<PersonalChatsService>();

            var chatViewModel = await chatsService.GetPersonalChatIfExistsOrCreateOne(User);

            ServiceLocator.Instance.GetService<EventBus>()
                .Raise(EventBusDefinitions.OpenChat, new ChatDataIEventBusArgs(chatViewModel));

        }

        private ChatViewModel ChatDtoToChat(Chat chatDto, int userId)
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();
            var parcipient = chatDto.Members.First(u => u.Id != userId);
            return new ChatViewModel(chatDto.Id, parcipient.DisplayName,
                 parcipient.AvatarURL,
                 new ObservableCollection<Message>(), chatDto.Members.FirstOrDefault(m => m.Id != authService.User.Id));
        }

        public UserViewModel(User user)
        {
            User = user;

            AddToFriend = new LambdaCommand(OnAddToFriendCommandExecute, CanAddToFriendCommandExecute);
            DeleteFriend = new LambdaCommand(OnDeleteFriendExecute, CanDeleteFriendExecute);
            CreateOrOpenChat = new LambdaCommand(OnExecuteCreateOrOpenChatCommand, CanExecuteCreateOrOpenChatCommand);
        }
    }
}
