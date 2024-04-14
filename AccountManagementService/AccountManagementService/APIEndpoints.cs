namespace AccountManagementService
{
    public static class APIEndpoints
    {
        public const string DatabaseServiceIP = "http://127.0.0.1:5291";
        public const string AddFriendPOST = $"{DatabaseServiceIP}/api/Friends/add-friend";
        public const string GetFriendsGET = $"{DatabaseServiceIP}/api/Friends/get-friends";
        public const string RemoveFriendPOST = $"{DatabaseServiceIP}/api/Friends/remove-friend";
        public const string GetUsersGET = $"{DatabaseServiceIP}/api/Users";

        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";
    }
}
