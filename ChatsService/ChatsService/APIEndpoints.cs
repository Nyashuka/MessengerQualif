namespace ChatsService
{
    public static class APIEndpoints
    {
        // DATABASE
        public const string DatabaseService = "http://127.0.0.1:5291";
        // [group chats]
        public const string CreateGroupPOST = $"{DatabaseService}/api/Groups";
        public const string GetAllGroupsByUserIdGET = $"{DatabaseService}/api/Groups";
        public const string GetGroupByIdGET = $"{DatabaseService}/api/Groups/group-by-id";
        // [chats]
        public const string CreatePersonalChat_POST = $"{DatabaseService}/api/Chats/create-personal";
        public const string GetAllPersonalChats_GET = $"{DatabaseService}/api/Chats/get-personal";
        public const string GetPersonalChat_POST = $"{DatabaseService}/api/Chats/get-personal";
        public const string GetChatByIdGET = $"{DatabaseService}/api/Chats/get-chat-by-id";
        // [chat members]
        public const string AddMemberToChatPOST = $"{DatabaseService}/api/ChatMembers";
        public const string DeleteMemberDELETE = $"{DatabaseService}/api/ChatMembers";
        public const string GetChatMembersGET = $"{DatabaseService}/api/ChatMembers";

        // AUTHENTICATE
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";


    }
}
