using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {
        public ChatViewModel Chat { get; set; }

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
                ChatId = Chat.Id,
                SenderId = authService.User.Id,
                RecipientId = Chat.Parcipient.Id,
                Data = MessageText
            };

            var messagesService = ServiceLocator.Instance.GetService<MessagesService>();
            var sendMessageResponse = await messagesService.SendMessage(messageDto);

            Chat.AddMessage(sendMessageResponse.Data, false);
            MessageText = "";
        }

        private void MessegesListChangedInvoked()
        {
            MessegesListChanged?.Invoke();
        }

        public ChatPageViewModel(ChatViewModel chat)
        {
            Chat = chat;

            SendMessageCommand = new LambdaCommand(OnSendMessageExecuteCommand, CanSendMessageExecuteCommand);

            chat.MessegesListChanged += MessegesListChangedInvoked;
        }
    }
}
