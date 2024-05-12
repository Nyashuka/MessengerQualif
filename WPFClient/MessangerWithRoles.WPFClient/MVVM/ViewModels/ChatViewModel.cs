using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        //private ChatDto _chatDto;
        public int Id { get; set; }
        public string DisplayName { get; private set; }
        public string ImageSource { get; private set; }

        private string _lastMessage;
        public string LastMessage { get => _lastMessage; private set => Set(ref _lastMessage, value); }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }
        public User Parcipient { get; private set; }

        public event Action MessegesListChanged;

        public ChatViewModel(int id, string username, string imageSource, ObservableCollection<Message> messages, User parcipient)
        {
            Id = id;
            Messages = messages;
            DisplayName = username;
            ImageSource = imageSource;
            Parcipient = parcipient;

            if (messages.Count > 0)
                LastMessage = messages.Last().Text;
        }

        public void AddMessage(MessageDto message, bool isReceived)
        {
            Message newMessage = new Message(DisplayName, message.Data, isReceived);
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
