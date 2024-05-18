using MessengerWithRoles.WPFClient.MVVM.Models;
using System;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public Chat? Chat { get; set; }

        public int SenderId { get; set; }
        public User? Sender { get; set; }

        public int? RecipientId { get; set; }
        public User? Recipient { get; set; }

        public string? Data { get; set; }

        public string? MediaUrl { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
