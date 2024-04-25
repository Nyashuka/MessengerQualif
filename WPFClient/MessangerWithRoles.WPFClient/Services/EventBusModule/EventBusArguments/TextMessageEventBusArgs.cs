using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    public class TextMessageEventBusArgs : IEventBusArgs
    {
        public MessageDto Message { get; private set; }

        public TextMessageEventBusArgs(MessageDto message)
        {
            Message = message;
        }
    }
}
