namespace MessengerDatabaseService.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatTypeId {  get; set; }
        public required ChatType ChatType { get; set; }
    }
}
