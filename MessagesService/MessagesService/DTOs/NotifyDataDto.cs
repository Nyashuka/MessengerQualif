using MessagesService.Models;

namespace MessagesService.DTOs
{
    public class NotifyDataDto
    {
        public ChatMessage? Message { get; set; }

        public List<int>? Recipients { get; set; }
    }
}
