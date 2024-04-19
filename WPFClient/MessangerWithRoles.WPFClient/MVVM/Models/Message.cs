using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class Message
    {
        public string Sender { get; private set; }
        public string? ImageSource { get; private set; }
        public string? Text { get; private set; }
        public bool IsReceived { get; private set; }
        public DateTime Time { get; private set; }

        public Message(string sender, string text, bool isReceived)
        {
            Sender = sender;
            Text = text;
            IsReceived = isReceived;
        }
    }
}
