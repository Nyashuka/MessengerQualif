using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    internal class ChatDataIEventBusArgs : IEventBusArgs
    {
        public Chat Chat { get; private set; }

        public ChatDataIEventBusArgs(Chat chat)
        {
            Chat = chat;
        }
    }
}
