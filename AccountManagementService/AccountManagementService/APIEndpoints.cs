// ReSharper disable InconsistentNaming
namespace AccountManagementService
{
    public static class APIEndpoints
    {
        // Database service
        public const string DatabaseServiceIP = "http://127.0.0.1:5291";
        // [friends]
        public const string AddFriendPOST = $"{DatabaseServiceIP}/api/Friends/add-friend";
        public const string GetFriendsGET = $"{DatabaseServiceIP}/api/Friends/get-friends";
        public const string RemoveFriendPOST = $"{DatabaseServiceIP}/api/Friends/remove-friend";
        // [users]
        public const string GetOtherUsersForUserGET = $"{DatabaseServiceIP}/api/Users";
        public const string GetUsersGET = $"{DatabaseServiceIP}/api/Users";
        public const string GetUserDataGET = $"{DatabaseServiceIP}/api/Users/get-user";
        

        // Auth service
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";
    }
}
