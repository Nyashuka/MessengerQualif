namespace DatabaseService.Models.DatabaseModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public int AccountId { get; set; }
        public Account Account { get; set; }
    }
}
