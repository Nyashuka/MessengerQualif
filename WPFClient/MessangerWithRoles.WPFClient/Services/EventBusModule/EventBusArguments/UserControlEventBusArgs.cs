using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    public class UserControlEventBusArgs : IEventBusArgs
    {
        public UserControl UserControl { get; private set; }

        public UserControlEventBusArgs(UserControl userControl) 
        { 
            UserControl = userControl;
        }
    }
}
