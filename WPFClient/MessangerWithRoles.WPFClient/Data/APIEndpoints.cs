using System.Net;

namespace MessengerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        public static string IPAddress = "91.201.235.100";
        public static string LocalAddress = "{IPAddress}";

        // auth service
        public static string CreateAccountPOST { get => $"http://{IPAddress}:5292/api/Auth/create-account"; }
        public static string LoginPOST { get => $"http://{IPAddress}:5292/api/Auth/login"; }

        // account management service
        public static string GetAllUsersGET { get => $"http://{IPAddress}:5293/api/Users"; }
        public static string GetUserGET { get => $"http://{IPAddress}:5293/api/Users/get-user"; }

        public static string AddFriendGET { get => $"http://{IPAddress}:5293/api/FriendsManagement/add-friend"; }
        public static string RemoveFriendGET { get => $"http://{IPAddress}:5293/api/FriendsManagement/remove-friend"; }
        public static string GetAllFriendsGET { get => $"http://{IPAddress}:5293/api/FriendsManagement"; }

        // chats
        public static string CreatePersonalChatPOST { get => $"http://{IPAddress}:5293/api/Chats/create-personal"; }
        public static string GetPersonalChatPOST { get => $"http://{IPAddress}:5293/api/Chats/get-personal"; }
        public static string GetAllChatsGET { get => $"http://{IPAddress}:5293/api/Chats/get-personal"; }
        public static string GetChatById { get => $"http://{IPAddress}:5293/api/Chats/get-personal-by-id"; }

        // messages service
        public static string GetChatMessagesByChatIdGET { get => $"http://{IPAddress}:5294/api/Messages"; }
        public static string SendMessagePOST { get => $"http://{IPAddress}:5294/api/Messages"; }

        // notification service
        public static string NotificationsWS { get => $"ws://{IPAddress}:6999/api/Notification/connect"; }
    }
}
