namespace MessengerDatabaseService.Models
{
    public class ChatMember
    {
        public int Id { get; set; }
        public required int ChatId { get; set; }
        public ChatType? Chat { get; set; }
        public required int UserId { get; set; }
        public User? User { get; set; }
    }
}
