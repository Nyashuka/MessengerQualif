using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseService.Models.DatabaseModels
{
    public class Message
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public Chat? Chat { get; set; }

        public int SenderId { get; set; }
        public User? Sender { get; set; }

        public int? RecipientId { get; set; }
        public User? Recipient { get; set; }

        public byte[]? Data { get; set; }

        public string? MediaUrl { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
