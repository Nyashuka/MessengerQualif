namespace MessagesService.DTOs
{
    public class ClientMessageDTO
    {
        public int ChatId { get; set; }
        public int RecipientId { get; set; }
        public byte[]? Data { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
