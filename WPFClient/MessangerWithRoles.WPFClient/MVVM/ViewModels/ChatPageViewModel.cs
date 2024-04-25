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
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
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

        public ICommand SendMessageCommand { get; }

        private bool CanSendMessageExecuteCommand(object p) => true;
        private async void OnSendMessageExecuteCommand(object p)
        {
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();

            MessageDto messageDto = new MessageDto()
            {
                ChatId = Chat.Id,
                SenderId = authService.User.Id,
                RecipientId = Chat.Parcipient.Id,
                Data = MessageText
            };

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.PostAsJsonAsync($"{APIEndpoints.SendMessagePOST}?accessToken={authService.AccessToken}", messageDto);
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();

            Chat.AddMessage(responseData.Data, false);
            MessageText = "";
        }

        public ChatPageViewModel(ChatViewModel chat)
        {
            Chat = chat;

            SendMessageCommand = new LambdaCommand(OnSendMessageExecuteCommand, CanSendMessageExecuteCommand);
        }
    }
}
