namespace MessagesService
{
    public static class APIEndpoints
    {
        public const string GetAuthenticatedUserGET = "http://127.0.0.1:5292/api/Auth/is-user-authenticated";

        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string GetChatMembersGET = $"{DatabaseService}/api/Chat";
    }
}
