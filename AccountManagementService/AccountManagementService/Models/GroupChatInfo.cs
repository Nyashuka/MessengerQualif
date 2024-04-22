namespace AccountManagementService.Models
{
    public class GroupChatInfo
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public int OwnerUserId { get; set; }
        public User? OwnerUser { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}
