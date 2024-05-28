namespace MessagesService
{
    public static class APIEndpoints
    {
        public const string GetAuthenticatedUserGET = "http://127.0.0.1:5292/api/Auth/is-user-authenticated";

        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string GetChatMembersGET = $"{DatabaseService}/api/ChatMembers";

        public const string SaveMessagePOST = $"{DatabaseService}/api/Messages/save";
        public const string GetMessagesByChatIdGET = $"{DatabaseService}/api/Messages";
        public const string DeleteMessageDELETE = $"{DatabaseService}/api/Messages";

        public const string GetChatByIdGET = $"{DatabaseService}/api/Chats/get-chat-by-id";
        public const string GetChatByMessageIdGET = $"{DatabaseService}/api/Chats/get-chat-by-message-id";
        public const string GetChatMessagesByChatIdGET = $"{DatabaseService}/api/Messages";
        public const string GetChatMessageByIdGET = $"{DatabaseService}/api/Messages/get-by-id";

        public const string GetGroupByIdGET = $"{DatabaseService}/api/Groups/group-by-id";

        public const string NotificationService = "http://127.0.0.1:6999";
        public const string NotifyUsersSendingMessagePOST = $"{NotificationService}/api/Notification/notify";
        public const string NotifyUsersDeleteMessagePOST = $"{NotificationService}/api/Notification/notify-delete-message";

        // CAN DO IT????
        public const string RolesService = "http://127.0.0.1:5296";
        public const string CanSendTextMessage = $"{RolesService}/api/ActionAccess/can-send-text-message";
        public const string CandDeleteMessages = $"{RolesService}/api/ActionAccess/can-delete-messages";
    }
}
