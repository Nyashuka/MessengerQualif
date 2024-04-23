using MessagesService.DTOs;

namespace MessagesService.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public ChatDto? Chat { get; set; }

        public int SenderId { get; set; }
        public UserDto? Sender { get; set; }

        public int? RecipientId { get; set; }
        public UserDto? Recipient { get; set; }

        public string? Data { get; set; }

        public string? MediaUrl { get; set; }

        public DateTime Timestamp { get; set; }
    }
}