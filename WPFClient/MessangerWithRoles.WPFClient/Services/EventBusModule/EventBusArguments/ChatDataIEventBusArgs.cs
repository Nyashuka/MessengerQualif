using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    public class ChatDataIEventBusArgs : IEventBusArgs
    {
        public ChatViewModel Chat { get; private set; }

        public ChatDataIEventBusArgs(ChatViewModel chat)
        {
            Chat = chat;
        }
    }
}
