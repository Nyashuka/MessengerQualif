using System.Net;

namespace MessengerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        private static PortList DefaultPorts = new PortList()
        {
            Authorization = 5292,
            AccountManagement = 5293,
            Chats = 5294,
            Messages = 5295,
            Roles = 5296,
            Notification = 6999
        };

        private static PortList RaspPorts = new PortList()
        {
            Authorization = 15292,
            AccountManagement = 15293,
            Messages = 15294,
            Notification = 16999
        };   

        public static string LocalAddress = "127.0.0.1";

        public static string LexyServer = "91.201.235.100";

        public static string IPAddress = LocalAddress; 
        public static PortList Ports = DefaultPorts;

        // auth service
        public static string CreateAccountPOST { get => $"http://{IPAddress}:{Ports.Authorization}/api/Auth/create-account"; }
        public static string LoginPOST { get => $"http://{IPAddress}:{Ports.Authorization}/api/Auth/login"; }

        // account management service
        public static string GetAllUsersGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Users"; }
        public static string GetUserGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Users/get-user"; }
        public static string AddFriendGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement/add-friend"; }
        public static string RemoveFriendGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement/remove-friend"; }
        public static string GetAllFriendsGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement"; }

        // chats
        public static string CreatePersonalChatPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats/create-personal"; }
        public static string GetPersonalChatByMembersPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats/get-personal-by-members"; }
        public static string GetAllChatsGET { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats"; }
        public static string GetChatById { get => $"http://{IPAddress}:{Ports.Chats}/api/Chats/get-by-id"; }
        public static string CreateGroupPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/Groups"; }
        public static string GetAllGroupsGET { get => $"http://{IPAddress}:{Ports.Chats}/api/Groups"; }

        // Chat members
        public static string GetChatMembersGET { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }
        public static string AddChatMemberPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }
        public static string AddChatMemberByUsernamePOST { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers/username"; }
        public static string DeleteChatMemberDELETE { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }

        // Permissions
        public static string GetAllPermissionsGET { get => $"http://{IPAddress}:{Ports.Chats}/api/Permissions"; }

        // messages service
        public static string GetChatMessagesByChatIdGET { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages"; }
        public static string SendMessagePOST { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages"; }

        // notification service
        public static string NotificationsWS { get => $"ws://{IPAddress}:{Ports.Notification}/api/Notification/connect"; }
        
    }
}
