using MessagesService.Models;

namespace MessagesService.DTOs
{
    public class NotifyDataDto
    {
        public MessageDto? Message { get; set; }

        public List<int>? Recipients { get; set; }
    }
}
