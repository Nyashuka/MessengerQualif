using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Services;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        //private ChatDto _chatDto;
        public int Id { get; set; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            private set => Set(ref _displayName, value);
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            private set => Set(ref _imageSource, value);
        }

        private string _lastMessage;
        public string LastMessage
        {
            get => _lastMessage;
            private set => Set(ref _lastMessage, value);
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        public User Parcipient { get; private set; }

        public event Action MessegesListChanged;

        public ChatViewModel(int id, string username,
            string imageSource, ObservableCollection<Message> messages, User parcipient)
        {
            Id = id;
            Messages = messages;
            DisplayName = username;
            ImageSource = imageSource;
            Parcipient = parcipient;

            if (messages.Count > 0)
                LastMessage = messages.Last().Text;

            var eventBus = ServiceLocator.Instance.GetService<EventBus>();
            eventBus.Subscribe<MessageDeletedEventBusArgs>(EventBusDefinitions.MessageDeleted, OnMessageDeleted);
        }

        public void DeleteMessage(Message message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Remove(message);
                UpdateLastMessage();
            });
            MessegesListChanged?.Invoke();
        }

        private void OnMessageDeleted(IEventBusArgs e)
        {
            var deleteMessage = e as MessageDeletedEventBusArgs;

            if (deleteMessage.ChatId == Id)
            {
                DeleteMessageById(deleteMessage.MessageId);
            }
        }

        public void DeleteMessageById(int messageId)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Message? message = Messages.FirstOrDefault(x => x.Id == messageId);
                if (message != null)
                {
                    Messages.Remove(message);
                    UpdateLastMessage();
                }
            });
            MessegesListChanged?.Invoke();
        }

        private void UpdateLastMessage()
        {
            if (Messages != null && Messages.Count > 0)
            {
                var lastMessage = Messages.Last();
                LastMessage = string.IsNullOrEmpty(lastMessage.Text) ? "" : lastMessage.Text;
            }
        }


        public void AddMessage(MessageDto message, bool isReceived)
        {
            Message newMessage = new Message(message.Id, DisplayName, message.Sender.AvatarURL, message.Data, isReceived);
            System.Windows.Application.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                Messages.Add(newMessage);
                LastMessage = message.Data;
            });
            MessegesListChanged?.Invoke();
        }

        public void UpdateMessages(ObservableCollection<Message> messages)
        {
            Messages = messages;
            LastMessage = messages.Last().Text;
            MessegesListChanged?.Invoke();
        }
    }
}
