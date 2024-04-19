using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services.EventBusModule
{
    public static class EventBusDefinitions
    {
        public const string NeedToChangeWindowContent = "NeedToChangeWindowContent";
        public const string LoginedInAccount = "LoginedInAccount";
        public const string OpenFriendsPageClicked = "OpenFriendsPageClicked";
    }
}
