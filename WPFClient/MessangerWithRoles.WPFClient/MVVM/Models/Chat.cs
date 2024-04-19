using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class Chat
    {
        public string Username { get; private set; }
        public string ImageSource { get; private set; }
        public ObservableCollection<Message> Messages { get; private set; }
        public string LastMessage { get; private set; }

        public Chat(string username, string imageSource, ObservableCollection<Message> messages)
        {
            Messages = messages;
            Username = username;
            ImageSource = imageSource;

            if (messages.Count > 0)
                LastMessage = messages.Last().Text;
        }
    }
}
