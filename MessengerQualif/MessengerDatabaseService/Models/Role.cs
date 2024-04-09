namespace MessengerDatabaseService.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int ChatId { get; set; }
        public required ChatType Chat { get; set; }
    }
}
