using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Services;
using System.Windows.Input;
using System;
using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Collections.ObjectModel;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class GroupPageViewModel : BaseViewModel
    {
        public GroupViewModel Group { get; set; }

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

        private void MessegesListChangedInvoked()
        {
            MessegesListChanged?.Invoke();
        }

        public GroupPageViewModel(GroupViewModel group)
        {
            Group = group;

            SendMessageCommand = new LambdaCommand(OnSendMessageExecuteCommand, CanSendMessageExecuteCommand);

            group.MessegesListChanged += MessegesListChangedInvoked;
        }

       
    }
}
