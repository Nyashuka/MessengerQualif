namespace MessengerDatabaseService.Models
{
    public class ChatMember
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public required Chat Chat { get; set; }
        public int UserId { get; set; }
        public required User User { get; set; }
    }
}
