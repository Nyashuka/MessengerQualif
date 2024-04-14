namespace MessengerDatabaseService.Models
{
    public class AccessToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string Token { get; set; }
    }
}
