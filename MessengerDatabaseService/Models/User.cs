namespace MessengerDatabaseService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ProfileName { get; set; } = string.Empty;

        public int AccountId { get; set; }
        public required Account Account { get; set; }
    }
}
