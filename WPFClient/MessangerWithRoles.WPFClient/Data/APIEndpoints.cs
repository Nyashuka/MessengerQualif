namespace MessengerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        // auth service
        public const string CreateAccountPOST = "http://127.0.0.1:5292/api/Auth/create-account";
        public const string LoginPOST = "http://127.0.0.1:5292/api/Auth/login";

        // account management service
        public const string GetAllUsersGET = "http://127.0.0.1:5293/api/Users";
        public const string GetUserGET = "http://127.0.0.1:5293/api/Users/get-user";

        public const string AddFriendGET = "http://127.0.0.1:5293/api/FriendsManagement/add-friend";
        public const string RemoveFriendGET = "http://127.0.0.1:5293/api/FriendsManagement/remove-friend";
        public const string GetAllFriendsGET = "http://127.0.0.1:5293/api/FriendsManagement";

        // chats
        public const string CreatePersonalChatPOST = "http://127.0.0.1:5293/api/Chats/create-personal";
        public const string GetPersonalChatPOST = "http://127.0.0.1:5293/api/Chats/get-personal";
        public const string GetAllChatsGET = "http://127.0.0.1:5293/api/Chats/get-personal";
    }
}
