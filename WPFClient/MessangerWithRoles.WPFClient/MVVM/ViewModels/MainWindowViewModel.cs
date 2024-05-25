using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
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

        private Visibility _chatListVisibility = Visibility.Visible;
        private Visibility _groupListVisibility = Visibility.Collapsed;

        public Visibility ChatListVisibility
        {
            get => _chatListVisibility;
            set => Set(ref _chatListVisibility, value);
        }

        public Visibility GroupListVisibility
        {
            get => _groupListVisibility;
            set => Set(ref _groupListVisibility, value);
        }

        private ObservableCollection<ChatViewModel> _chats;
        public ObservableCollection<ChatViewModel> Chats
        {
            get => _chats;
            set => Set(ref _chats, value);
        }

        private ObservableCollection<GroupViewModel> _groups;
        public ObservableCollection<GroupViewModel> Groups
        {
            get => _groups;
            set => Set(ref _groups, value);
        }

        public ICommand OpenFriendsWindow { get; }

        public bool CanExecuteOpenFriendsWindowCommand(object p) => true;

        public void OnExecuteOpenFriendsWindowCommand(object p)
        {
            CurrentContent = new FriendsPage();
        }

        public ICommand OpenCreateGroupWindow { get; }

        public bool CanExecuteOpenCreateGroupWindowCommand(object p) => true;

        public void OnExecuteOpenCreateGroupWindowCommand(object p)
        {
            CurrentContent = new CreateGroupPage();
            CurrentContent.DataContext = this;
        }

        public ICommand SelectedChatCommand { get; }

        public bool CanExecuteSelectedChatCommandCommand(object p) => true;

        public void OnExecuteSelectedChatCommandCommand(object p)
        {
            SelectedChat = (ChatViewModel)p;
        }

        private async Task LoadPersonalChats()
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();
            var chatsService = ServiceLocator.Instance.GetService<PersonalChatsService>();
            HttpClient httpClient = new HttpClient();

            var result = await httpClient.GetAsync($"{APIEndpoints.GetAllChatsGET}?accessToken={authService.AccessToken}");
            if (!result.IsSuccessStatusCode)
            {
                MessageBox.Show(result.ReasonPhrase);
                return;
            }

            var data = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<Chat>>>()).Data;
            Chats = new ObservableCollection<ChatViewModel>();

            foreach (var chatDto in data)
            {
                ObservableCollection<Message> chatMessages = await chatsService.GetChatMessages(chatDto.Id);

                User parcipient = chatDto.Members.FirstOrDefault(m => m.Id != authService.User.Id);
                Chats.Add(new ChatViewModel(chatDto.Id, chatDto.Members.First(u => u.Id != authService.User.Id).DisplayName,
                    "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                    chatMessages, parcipient));
            }
        }

        private async Task LoadGroups()
        {
            var groupsService = ServiceLocator.Instance.GetService<GroupsServcie>();

            var groupsResposne = await groupsService.GetGroups();

            Groups = new ObservableCollection<GroupViewModel>();

            if (groupsResposne.Data == null)
            {
                return;
            }

            foreach (var group in groupsResposne.Data)
            {
                Groups.Add(await groupsService.GroupModelToViewModel(group));
            }
        }

        public async Task LoadChats()
        {
            try
            {
                await LoadPersonalChats();
                await LoadGroups();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void OpenChatWindow(ChatViewModel chat)
        {
            CurrentContent = new ChatPage(new ChatPageViewModel(chat));
        }

        private void OpenChat(IEventBusArgs args)
        {
            var chatArgs = (ChatDataIEventBusArgs)args;

            var chat = Chats.FirstOrDefault(x => x.Id == chatArgs.Chat.Id);

            if (chat == null)
            {
                Chats.Add(chatArgs.Chat);
            }

            OpenChatWindow(chatArgs.Chat);
        }

        public void OpenGroupWindow(GroupViewModel chat)
        {
            CurrentContent = new GroupPage(new GroupPageViewModel(chat));
        }

        private void OpenGroup(IEventBusArgs args)
        {
            var groupArgs = (GroupDataIEventBusArgs)args;

            if (Groups.Any(x => x.Id != groupArgs.Group.Id))
            {
                Groups.Add(groupArgs.Group);
            }

            OpenGroupWindow(groupArgs.Group);
        }

        private async void UpdateMessages(IEventBusArgs args)
        {
            var messageArgs = (TextMessageEventBusArgs)args;
            var message = messageArgs.Message;
            var chatsService = ServiceLocator.Instance.GetService<PersonalChatsService>();

            var chatFromDB = await chatsService.TryGetChatIfExists(message.ChatId);

            if(Convert.ToInt32(ChatTypeEnum.personal) == chatFromDB.Data.ChatTypeId)
            {
                UpdatePersonalMessages(chatsService, message);
            }
            else if (Convert.ToInt32(ChatTypeEnum.group) == chatFromDB.Data.ChatTypeId)
            {
                UpdateGroupMessages(chatsService, message);
            }
        }

        private async Task UpdatePersonalMessages(PersonalChatsService personalChatsService, MessageDto messageDto)
        {
            var chat = Chats.FirstOrDefault(c => c.Id == messageDto.ChatId);
            var chatFromDB = await personalChatsService.GetExistsPersonalChat(messageDto.ChatId);

            if (chat == null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Chats.Add(chatFromDB);
                });
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                chat.UpdateMessages(chatFromDB.Messages);
            });
        }

        private async Task UpdateGroupMessages(PersonalChatsService personalChatsService, MessageDto messageDto)
        {
            var chat = Groups.FirstOrDefault(c => c.Id == messageDto.ChatId);
            var chatFromDB = await personalChatsService.TryGetChatIfExists(messageDto.ChatId);
            var messages = await personalChatsService.GetChatMessages(messageDto.ChatId);     

            if (chat == null)
            {
                var newChat = new GroupViewModel(chatFromDB.Data.Id, chatFromDB.Data.ChatInfo.Name,
                    chatFromDB.Data.ChatInfo.Description, 
                    new ObservableCollection<User>(chatFromDB.Data.Members), messages, chat.Roles);

                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Groups.Add(newChat);
                });
                return;
            }
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                chat.UpdateMessages(messages);
            });
        }

        private CreateGroupChatDto _reateGroupChatDto;
        public CreateGroupChatDto CreateGroupChatDto
        {
            get => _reateGroupChatDto;
            set => Set(ref _reateGroupChatDto, value);
        }

        public ICommand CreateGroupCommand { get; }
        public bool CanExecuteCreateGroupCommandCommand(object p) => true;
        public async void OnExecuteCreateGroupCommandCommand(object p)
        {
            var groupsService = ServiceLocator.Instance.GetService<GroupsServcie>();
            var rolesService = ServiceLocator.Instance.GetService<RolesService>();
            var createGroupResponse = await groupsService.CreateGroup(CreateGroupChatDto);

            if (createGroupResponse.Data == null)
            {
                MessageBox.Show(createGroupResponse.Message);
                return;
            }

            var group = createGroupResponse.Data;

            var roles = await rolesService.GetChatRoles(group.Id);

            GroupViewModel groupViewModel = new GroupViewModel(group.Id, group.ChatInfo.Name,
                group.ChatInfo.Description,
                new ObservableCollection<User>() { group.ChatInfo.Owner },
                new ObservableCollection<Message>(),
                new ObservableCollection<RoleWithPermissions>(roles.Data));

            Groups.Add(groupViewModel);
        }

        public ICommand OpenGroupListCommand { get; }
        public bool CanOpenGroupListCommand(object p) => true;
        public async void OnExecuteOpenGroupListCommand(object p)
        {
            await LoadGroups();

            ChatListVisibility = Visibility.Collapsed;
            GroupListVisibility = Visibility.Visible;  
        }

        public ICommand OpenPersonalChatsListCommand { get; }
        public bool CanOpenPersonalChatsListCommand(object p) => true;
        public async void OnExecuteOpenPersonalChatsListCommand(object p)
        {
            await LoadPersonalChats();

            GroupListVisibility = Visibility.Collapsed;
            ChatListVisibility = Visibility.Visible;
        }

        public ICommand OpenAccountSettingsCommand { get; }

        public bool CanExecuteOpenAccountSettingsCommand(object p) => true;

        public void OnExecuteOpenAccountSettingsCommand(object p)
        {
            CurrentContent = new AccountSettings();
            CurrentContent.DataContext = new AccountSettingsViewModel();
        }

        private string _currentDisplayName;
        public string CurrentDisplayName { get => _currentDisplayName; set => Set(ref _currentDisplayName, value); }

        private string _currentUsername;
        public string CurrentUsername { get => _currentUsername; set => Set(ref _currentUsername, value); }

        public void UpdateProfile()
        {
            var authService = ServiceLocator.Instance.GetService<AccountService>();
            CurrentDisplayName = authService.User.DisplayName;
            CurrentUsername = '@' + authService.User.Username;
        }

        public MainWindowViewModel()
        {

            Chats = new ObservableCollection<ChatViewModel>();
            Groups = new ObservableCollection<GroupViewModel>();

            CreateGroupChatDto = new CreateGroupChatDto();

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.OpenChat, OpenChat);
            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.OpenGroup, OpenGroup);
            eventBus.Subscribe<ChatDataIEventBusArgs>(EventBusDefinitions.TextMessageReceived, UpdateMessages);

            OpenPersonalChatsListCommand = new LambdaCommand(OnExecuteOpenPersonalChatsListCommand, CanOpenPersonalChatsListCommand);
            OpenGroupListCommand = new LambdaCommand(OnExecuteOpenGroupListCommand, CanOpenGroupListCommand);

            OpenFriendsWindow = new LambdaCommand(OnExecuteOpenFriendsWindowCommand, CanExecuteOpenFriendsWindowCommand);
            OpenCreateGroupWindow = new LambdaCommand(OnExecuteOpenCreateGroupWindowCommand, CanExecuteOpenCreateGroupWindowCommand);
            SelectedChatCommand = new LambdaCommand(OnExecuteSelectedChatCommandCommand, CanExecuteSelectedChatCommandCommand);
            CreateGroupCommand = new LambdaCommand(OnExecuteCreateGroupCommandCommand, CanExecuteCreateGroupCommandCommand);
            OpenAccountSettingsCommand = new LambdaCommand(OnExecuteOpenAccountSettingsCommand, CanExecuteOpenAccountSettingsCommand);

            UpdateProfile();
        }
    }
}
