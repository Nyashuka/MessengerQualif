using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private UserControl _currentContent;
        public UserControl CurrentContent { get => _currentContent; set => Set(ref _currentContent, value); }

        private Chat _selectedChat;

        public Chat SelectedChat
        {
            get => _selectedChat;
            set => Set(ref _selectedChat, value);
        }

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set => Set(ref _chats, value);
        }

        public MainWindowViewModel DataContext => this;

        public ObservableCollection<Message> Messages { get; private set; }

        public ICommand OpenFriendsWindow { get; }

        public bool CanExecuteOpenFriendsWindowCommand(object p) => true;

        public void OnExecuteOpenFriendsWindowCommand(object p)
        {
            CurrentContent = new FriendsPage();
        }

        public ICommand OpenCreateChatWindow { get; }

        public bool CanExecuteOpenCreateChatWindowCommand(object p) => true;

        public void OnExecuteOpenCreateChatWindowCommand(object p)
        {
            CurrentContent = new CreatePersonalChatPage();
            CurrentContent.DataContext = new CreatePersonalChatViewModel();
        }

        public ICommand SelectedChatCommand { get; }

        public bool CanExecuteSelectedChatCommandCommand(object p) => true;

        public void OnExecuteSelectedChatCommandCommand(object p)
        {
            SelectedChat = (Chat)p;
        }

        public async Task LoadPersonalChats()
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();
            HttpClient httpClient = new HttpClient();

            try
            {
                var result = await httpClient.GetAsync($"{APIEndpoints.GetAllChatsGET}?accessToken={authService.AccessToken}");
                if (!result.IsSuccessStatusCode!)
                {
                    MessageBox.Show(result.ReasonPhrase);
                    return;
                }

                var data = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ChatDto>>>()).Data;

                foreach (var chatDto in data)
                {
                    Chats.Add(new Chat(chatDto.Members.First(u => u.Id != authService.User.Id).DisplayName,
                        "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                        new ObservableCollection<Message>()));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void OpenChat(IEventBusArgs args)
        {
            var chatArgs = (ChatDataIEventBusArgs)args;

            CurrentContent = new ChatPage();
            CurrentContent.DataContext = new ChatPageViewModel(chatArgs.Chat);
        }

        private void AddChat(IEventBusArgs args)
        {
            var chatArgs = (ChatDataIEventBusArgs)args;

            Chats.Add(chatArgs.Chat);

            CurrentContent = new ChatPage();
            CurrentContent.DataContext = new ChatPageViewModel(chatArgs.Chat);
        }

        public MainWindowViewModel()
        {
            OpenFriendsWindow = new LambdaCommand(OnExecuteOpenFriendsWindowCommand, CanExecuteOpenFriendsWindowCommand);
            OpenCreateChatWindow = new LambdaCommand(OnExecuteOpenCreateChatWindowCommand, CanExecuteOpenCreateChatWindowCommand);
            SelectedChatCommand = new LambdaCommand(OnExecuteSelectedChatCommandCommand, CanExecuteSelectedChatCommandCommand);

            Chats = new ObservableCollection<Chat>();

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.OpenChat, OpenChat);
            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.ChatCreated, AddChat);
        }
    }
}
