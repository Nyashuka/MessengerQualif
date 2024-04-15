using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerWithRoles.WPFClient.Services.EventBusModule
{
    public class EventBus : IService
    {
        private readonly Dictionary<string, List<EventBusHandler>> _subscribersByEventName;

        public EventBus()
        {
            _subscribersByEventName = new Dictionary<string, List<EventBusHandler>>();
        }

        public void Subscribe<T>(string eventName, EventBusHandler action) where T : IEventBusArgs
        {
            if (!_subscribersByEventName.ContainsKey(eventName))
            {
                _subscribersByEventName.Add(eventName, new List<EventBusHandler>());
            }
            _subscribersByEventName[eventName].Add(action);
        }

        public void Unsubscribe<T>(string eventName, EventBusHandler action) where T : IEventBusArgs
        {
            if (_subscribersByEventName.ContainsKey(eventName))
            {
                _subscribersByEventName[eventName].Remove(action);
            }
        }

        public void Raise(string eventName, IEventBusArgs arguments)
        {
            if (!_subscribersByEventName.TryGetValue(eventName, out var subscribers)) return;

            foreach (var subscriber in subscribers)
            {
                subscriber(arguments);
            }
        }
    }
}
