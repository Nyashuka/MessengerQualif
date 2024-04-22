using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {
        public Chat Chat { get; set; }

        public ChatPageViewModel(Chat chat)
        {
            Chat = chat;
        }
    }
}
