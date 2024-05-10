namespace MessengerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        public static string ip = "91.201.235.100";
        // auth service
        public static string CreateAccountPOST = $"http://{ip}:5292/api/Auth/create-account";
        public static string LoginPOST = $"http://{ip}:5292/api/Auth/login";

        // account management service
        public static string GetAllUsersGET = $"http://{ip}:5293/api/Users";
        public static string GetUserGET = $"http://{ip}:5293/api/Users/get-user";

        public static string AddFriendGET = $"http://{ip}:5293/api/FriendsManagement/add-friend";
        public static string RemoveFriendGET = $"http://{ip}:5293/api/FriendsManagement/remove-friend";
        public static string GetAllFriendsGET = $"http://{ip}:5293/api/FriendsManagement";

        // chats
        public static string CreatePersonalChatPOST = $"http://{ip}/api/Chats/create-personal";
        public static string GetPersonalChatPOST = $"http://{ip}:5293/api/Chats/get-personal";
        public static string GetAllChatsGET = $"http://{ip}:5293/api/Chats/get-personal";
        public static string GetChatById = $"http://{ip}:5293/api/Chats/get-personal-by-id";

        // messages service
        public static string GetChatMessagesByChatIdGET = $"http://{ip}:5294/api/Messages";
        public static string SendMessagePOST = $"http://{ip}:5294/api/Messages";

    } 
}
