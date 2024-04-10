namespace AuthorizationService
{
    public static class APIEndpoints
    {
        public const string DatabaseServiceEndpoint = "http://127.0.0.1:5291";
        public const string CreateAccountPOST = $"{DatabaseServiceEndpoint}/api/account/create-account";
    }
}
