namespace MessagesService.Models
{
    public class ChatMember
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
    }
}