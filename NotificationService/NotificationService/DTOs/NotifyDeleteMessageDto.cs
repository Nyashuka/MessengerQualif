namespace NotificationService.DTOs
{
    public class NotifyDeleteMessageDto
    {
        public int ChatId { get; set; }
        public int MessageId { get; set; }
        public List<int>? Recipients { get; set; }
    }
}