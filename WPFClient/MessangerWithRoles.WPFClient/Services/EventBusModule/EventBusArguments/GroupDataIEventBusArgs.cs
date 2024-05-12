using MessengerWithRoles.WPFClient.MVVM.ViewModels;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments
{
    public class GroupDataIEventBusArgs : IEventBusArgs
    {
        public GroupViewModel Group { get; private set; }

        public GroupDataIEventBusArgs(GroupViewModel group)
        {
            Group = group;
        }
    }
}
