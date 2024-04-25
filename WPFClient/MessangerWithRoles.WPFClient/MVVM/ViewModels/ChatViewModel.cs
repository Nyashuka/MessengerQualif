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
        public string Username { get; private set; }
        public string ImageSource { get; private set; }

        private string _lastMessage;
        public string LastMessage { get => _lastMessage; private set => Set(ref _lastMessage, value); }
        public ObservableCollection<Message> Messages { get; private set; }
        public User Parcipient { get; private set; }

        public ChatViewModel(int id, string username, string imageSource, ObservableCollection<Message> messages, User parcipient)
        {
            Id = id;
            Messages = messages;
            Username = username;
            ImageSource = imageSource;
            Parcipient = parcipient;

            if (messages.Count > 0)
                LastMessage = messages.Last().Text;
        }

        public void AddMessage(MessageDto message, bool isReceived)
        {
            Message newMessage = new Message(Username, message.Data, isReceived);
            System.Windows.Application.Current.Dispatcher.Invoke(delegate // <--- HERE
            {
                Messages.Add(newMessage);
                LastMessage = message.Data;
            });

        }

        
    }
}
