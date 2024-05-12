namespace ChatsService.Groups.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; }
        public GroupChatInfo? ChatInfo { get; set; }
    }
}