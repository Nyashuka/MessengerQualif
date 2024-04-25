namespace MessagesService
{
    public static class APIEndpoints
    {
        public const string GetAuthenticatedUserGET = "http://127.0.0.1:5292/api/Auth/is-user-authenticated";

        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string GetChatMembersGET = $"{DatabaseService}/api/Chats/get-chat-members";

        public const string SaveMessagePOST = $"{DatabaseService}/api/Messages/save";
        public const string GetMessagesByChatIdGET = $"{DatabaseService}/api/Messages";

        public const string GetChatByIdGET = $"{DatabaseService}/api/Chats/get-chat-by-id";
        public const string GetChatMessagesByChatIdGET = $"{DatabaseService}/api/Messages";

        public const string NotificationService = "http://127.0.0.1:6999";
        public const string NotifyUsersSendingMessagePOST = $"{NotificationService}/api/Notification/notify";
    }
}
