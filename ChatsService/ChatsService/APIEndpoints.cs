namespace ChatsService
{
    public static class APIEndpoints
    {
        // create group
        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string CreateGroupPOST = $"{DatabaseService}/api/Groups";
        public const string GetAllGroupsByUserIdGET = $"{DatabaseService}/api/Groups";

        // Auth service
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";

    }
}
