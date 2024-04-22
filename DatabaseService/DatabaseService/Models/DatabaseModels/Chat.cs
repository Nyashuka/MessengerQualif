namespace DatabaseService.Models.DatabaseModels
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public ChatType? ChatType { get; set; }
    }
}
