namespace RolesService.ActionAccess.Data
{
    public class AddChatMemberChatActionData
    {
        public int ChatId { get; set; }
        public int RequesterId { get; set; }
        public int UserIdToAdd { get; set; }
    }
}
