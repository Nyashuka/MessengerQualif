using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Services;
using System.Windows.Input;
using System;
using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class GroupPageViewModel : BaseViewModel
    {
        public GroupViewModel _group;
        public GroupViewModel Group
        {
            get => _group;
            set => Set(ref _group, value);
        }

        private string _messageText;
        public string MessageText
        {
            get => _messageText;
            set => Set(ref _messageText, value);
        }

        public event Action MessegesListChanged;

        public ICommand SendMessageCommand { get; }

        private bool CanSendMessageExecuteCommand(object p) => !string.IsNullOrEmpty(_messageText);

        private async void OnSendMessageExecuteCommand(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            MessageDto messageDto = new MessageDto()
            {
                ChatId = Group.Id,
                SenderId = authService.User.Id,
                RecipientId = authService.User.Id,
                Data = MessageText
            };

            var messagesService = ServiceLocator.Instance.GetService<MessagesService>();
            var sendMessageResponse = await messagesService.SendMessage(messageDto);

            Group.AddMessage(sendMessageResponse.Data, false);
            MessageText = "";
        }

        private ContentControl _settingsPage;
        public ContentControl SettingsPage
        {
            get => _settingsPage;
            set => Set(ref _settingsPage, value);
        }

        private Visibility _chatVisibility = Visibility.Visible;
        private Visibility _settingsVisibility = Visibility.Collapsed;

        public Visibility ChatVisibility
        {
            get => _chatVisibility;
            set => Set(ref _chatVisibility, value);
        }

        public Visibility SettingsVisibility
        {
            get => _settingsVisibility;
            set => Set(ref _settingsVisibility, value);
        }

        public ICommand OpenSettingsCommand { get; }

        private bool CanExecuteOpenSettingsCommand(object p) => true;

        private void OnExecuteOpenSettingsCommand(object p)
        {
            SettingsPage = new ChatSettings();
            SettingsPage.DataContext = this;

            ChatVisibility = Visibility.Collapsed;
            SettingsVisibility = Visibility.Visible;
        }

        public ICommand CloseSettingsCommand { get; }

        private bool CanExecuteCloseSettingsCommand(object p) => true;

        private void OnExecuteCloseSettingsCommand(object p)
        {
            SettingsPage = null;

            ChatVisibility = Visibility.Visible;
            SettingsVisibility = Visibility.Collapsed;
        }


        private string _usernameToAdd;
        public string UsernameToAdd
        {
            get => _usernameToAdd;
            set => Set(ref _usernameToAdd, value);
        }

        public ICommand AddMemberCommand { get; }

        private bool CanExecuteAddMemberCommand(object p) => true;

        private async void OnExecuteAddMemberCommand(object p)
        {
            var groupService = ServiceLocator.Instance.GetService<GroupsServcie>();

            var resposne = await groupService.AddMemberByUsername(Group.Id, UsernameToAdd);

            if (resposne.Data == null)
            {
                MessageBox.Show(resposne.Message);
                return;
            }

            Group.AddMember(resposne.Data.User);
        }

        public ICommand DeleteMemberCommand { get; }

        private bool CanExecuteDeleteMemberCommand(object p) => true;

        private async void OnExecuteDeleteMemberCommand(object p)
        {
            var groupService = ServiceLocator.Instance.GetService<GroupsServcie>();

            var userToDelete = p as User;
            var resposne = await groupService.DeleteMember(Group.Id, userToDelete.Id);

            if (resposne.Data == null)
            {
                MessageBox.Show(resposne.Message);
                return;
            }

            Group.DeleteMember(userToDelete);
        }

        private void MessegesListChangedInvoked()
        {
            MessegesListChanged?.Invoke();
        }

        public GroupPageViewModel(GroupViewModel group)
        {
            Group = group;

            SendMessageCommand = new LambdaCommand(OnSendMessageExecuteCommand, CanSendMessageExecuteCommand);

            group.MessegesListChanged += MessegesListChangedInvoked;

            OpenSettingsCommand = new LambdaCommand(OnExecuteOpenSettingsCommand, CanExecuteOpenSettingsCommand);
            CloseSettingsCommand = new LambdaCommand(OnExecuteCloseSettingsCommand, CanExecuteCloseSettingsCommand);
            AddMemberCommand = new LambdaCommand(OnExecuteAddMemberCommand, CanExecuteAddMemberCommand);
            DeleteMemberCommand = new LambdaCommand(OnExecuteDeleteMemberCommand, CanExecuteDeleteMemberCommand);
        }


    }
}
