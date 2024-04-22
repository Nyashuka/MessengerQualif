namespace AccountManagementService.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public ChatType? ChatType { get; set; }
    }
}
