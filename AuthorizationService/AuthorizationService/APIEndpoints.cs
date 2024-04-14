namespace AuthorizationService
{
    public static class APIEndpoints
    {
        public const string DatabaseServiceEndpoint = "http://127.0.0.1:5291";
        public const string CreateAccountPOST = $"{DatabaseServiceEndpoint}/api/account/create-account";
        public const string IsUserExistsGET = $"{DatabaseServiceEndpoint}/api/account/user-exists";
        public const string GetAccountGET = $"{DatabaseServiceEndpoint}/api/account/get-account";
        public const string SaveTokenPOST = $"{DatabaseServiceEndpoint}/api/account/save-token";
        public const string GetTokenGET = $"{DatabaseServiceEndpoint}/api/account/get-token";
        public const string GetAccountByAccessTokenGET = $"{DatabaseServiceEndpoint}/api/account/get-user-by-token";
    }
}
