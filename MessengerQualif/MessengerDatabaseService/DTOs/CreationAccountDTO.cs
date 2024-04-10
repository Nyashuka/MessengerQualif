namespace MessengerDatabaseService.DTOs
{
    public class CreationAccountDTO
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
