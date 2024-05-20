namespace ChatsService.ActionAccess.Data
{
    public class AddChatMemberChatActionData
    {
        public int ChatId { get; set; }
        public int RequesterId { get; set; }
        public string UsernameToAdd { get; set; }
    }
}
