using NotificationService.Models;

namespace NotificationService.DTOs
{
    public class NotifyDataDto
    {
        public ChatMessage? Message { get; set; }

        public List<int>? Recipients { get; set; }
    }
}
