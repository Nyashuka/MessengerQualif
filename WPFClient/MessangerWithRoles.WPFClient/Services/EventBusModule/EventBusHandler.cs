using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;

namespace MessangerWithRoles.WPFClient.Services.EventBusModule
{
    public delegate void EventBusHandler(IEventBusArgs e);
}
