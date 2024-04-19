using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule
{
    public delegate void EventBusHandler(IEventBusArgs e);
}
