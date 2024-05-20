namespace ChatsService.ActionAccess.Data
{
    public class DeleteChatMemberChatActionData
    {
        public int ChatId { get; set; }
        public int RequesterId { get; set; }
        public int UserIdToDelete { get; set; }
    }
}
