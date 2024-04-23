namespace NotificationService.DTOs
{
    public class ChatDto
    {
        public int ChatTypeId { get; set; }
        public List<UserDto>? Members { get; set; }
    }
}