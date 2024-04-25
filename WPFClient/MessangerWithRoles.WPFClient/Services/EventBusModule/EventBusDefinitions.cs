namespace MessengerWithRoles.WPFClient.Services.EventBusModule
{
    public static class EventBusDefinitions
    {
        public const string NeedToChangeWindowContent = "NeedToChangeWindowContent";
        public const string LoginedInAccount = "LoginedInAccount";
        public const string OpenFriendsPageClicked = "OpenFriendsPageClicked";
        public const string OpenChat = "OpenChat";
        public const string ChatCreated = "ChatCreated";
        public const string TextMessageReceived = "TextMessageReceived";
    }
}
