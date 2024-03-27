namespace MessengerDatabaseService.Models
{
    public class Message
    {
        public int Id { get; set; }
        public required byte[] Data { get; set; }
        public DateTime Timestamp { get; set; }

        public int ChatId { get; set; }
        public required Chat Chat { get; set; }

        public int SenderId { get; set; }
        public required User User { get; set; }
    }
}
