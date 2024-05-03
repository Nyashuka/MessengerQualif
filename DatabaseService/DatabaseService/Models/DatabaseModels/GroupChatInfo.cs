namespace DatabaseService.Models.DatabaseModels
{
    public class GroupChatInfo
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}
