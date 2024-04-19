using NotificationService.Models;

namespace NotificationService.DTOs
{
    public class MessageSendingData
    {
        public ChatMessage? Message { get; set; }

        public List<int>? Users { get; set; }
    }
}
