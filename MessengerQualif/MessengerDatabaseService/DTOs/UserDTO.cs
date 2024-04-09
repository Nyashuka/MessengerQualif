namespace MessengerDatabaseService.DTOs
{
    public class UserDTO
    {
        public required int AccountId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string ProfileName { get; set; } = string.Empty;
    }
}
