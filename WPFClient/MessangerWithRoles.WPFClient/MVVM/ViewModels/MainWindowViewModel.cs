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

        private ChatViewModel _selectedChat;

        public ChatViewModel SelectedChat
        {
            get => _selectedChat;
            set => Set(ref _selectedChat, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

        private ObservableCollection<ChatViewModel> _chats;
        public ObservableCollection<ChatViewModel> Chats
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
            SelectedChat = (ChatViewModel)p;
        }

        public async Task LoadPersonalChats()
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();
            HttpClient httpClient = new HttpClient();

            try
            {
                var result = await httpClient.GetAsync($"{APIEndpoints.GetAllChatsGET}?accessToken={authService.AccessToken}");
                if (!result.IsSuccessStatusCode)
                {
                    MessageBox.Show(result.ReasonPhrase);
                    return;
                }

                var data = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<ChatDto>>>()).Data;

                foreach (var chatDto in data)
                {
                    var messagesResponse = await httpClient.GetAsync($"{APIEndpoints.GetChatMessagesByChatIdGET}?accessToken={authService.AccessToken}&chatId={chatDto.Id}");
                    var messagesResponseData = await messagesResponse.Content.ReadFromJsonAsync<ServiceResponse<List<MessageDto>>>();

                    ObservableCollection<Message> chatMessages = new ObservableCollection<Message>();
                    foreach (var currentMessage in messagesResponseData.Data)
                    {
                        chatMessages.Add(new Message(currentMessage.Sender.DisplayName, 
                            currentMessage.Data, authService.User.Id != currentMessage.Sender.Id));
                    }

                    User parcipient = chatDto.Members.FirstOrDefault(m => m.Id != authService.User.Id);
                    Chats.Add(new ChatViewModel(chatDto.Id, chatDto.Members.First(u => u.Id != authService.User.Id).DisplayName,
                        "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                        chatMessages, parcipient));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void OpenChat(ChatViewModel chat)
        {
            CurrentContent = new ChatPage(new ChatPageViewModel(chat));
        }

        private void OpenChat(IEventBusArgs args)
        {
            var chatArgs = (ChatDataIEventBusArgs)args;

            OpenChat(chatArgs.Chat);
        }

        private void AddChat(IEventBusArgs args)
        {
            var chatArgs = (ChatDataIEventBusArgs)args;

            Chats.Add(chatArgs.Chat);

            OpenChat(chatArgs.Chat);
        }

        private async Task<ChatViewModel> GetChat(int chatId)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{APIEndpoints.GetChatById}?accessToken={authService.AccessToken}&chatId={chatId}");
            var chatData = (await response.Content.ReadFromJsonAsync<ServiceResponse<ChatDto>>()).Data;

            var messagesResponse = await httpClient.GetAsync($"{APIEndpoints.GetChatMessagesByChatIdGET}?accessToken={authService.AccessToken}&chatId={chatId}");
            var messagesResponseData = await messagesResponse.Content.ReadFromJsonAsync<ServiceResponse<List<MessageDto>>>();

            ObservableCollection<Message> chatMessages = new ObservableCollection<Message>();
            foreach (var currentMessage in messagesResponseData.Data)
            {
                chatMessages.Add(new Message(currentMessage.Sender.DisplayName,
                    currentMessage.Data, authService.User.Id != currentMessage.Sender.Id));
            }

            User parcipient = chatData.Members.FirstOrDefault(m => m.Id != authService.User.Id);
            return new ChatViewModel(chatData.Id, chatData.Members.First(u => u.Id != authService.User.Id).DisplayName,
                "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                chatMessages, parcipient);
        }

        private async void UpdateMessages(IEventBusArgs args)
        {
            var messageArgs = (TextMessageEventBusArgs)args;
            var message = messageArgs.Message;

            var chat = Chats.FirstOrDefault(c => c.Id == message.ChatId);
            if(chat == null)
            {
                chat = await GetChat(message.ChatId);

                System.Windows.Application.Current.Dispatcher.Invoke(delegate // <--- HERE
                {
                    Chats.Add(chat);
                });
                return;
            }

            chat.AddMessage(message, true);
        }

        public MainWindowViewModel()
        {
            OpenFriendsWindow = new LambdaCommand(OnExecuteOpenFriendsWindowCommand, CanExecuteOpenFriendsWindowCommand);
            OpenCreateChatWindow = new LambdaCommand(OnExecuteOpenCreateChatWindowCommand, CanExecuteOpenCreateChatWindowCommand);
            SelectedChatCommand = new LambdaCommand(OnExecuteSelectedChatCommandCommand, CanExecuteSelectedChatCommandCommand);

            Chats = new ObservableCollection<ChatViewModel>();

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();
            MessagesService messagesService = new MessagesService();
            messagesService.Start(authService.AccessToken);
            ServiceLocator.Instance.RegisterService(messagesService);

            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.OpenChat, OpenChat);
            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.ChatCreated, AddChat);
            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.TextMessageReceived, UpdateMessages);
        }
    }
}
