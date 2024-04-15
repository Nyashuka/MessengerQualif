namespace NotificationService.Models
{
    public class WaitingClient
    {
        public string User { get; set; }
        public TaskCompletionSource<List<ChatMessage>> TaskCompletionSource { get; set; }
    }
}
