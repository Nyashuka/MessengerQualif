namespace DatabaseService.Models.DatabaseModels
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public List<ChatPermission>? Permissions { get; set; }
    }
}
