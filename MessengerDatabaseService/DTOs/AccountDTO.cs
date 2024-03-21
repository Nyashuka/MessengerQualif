namespace MessengerDatabaseService.DTOs
{
    public class AccountDTO
    {
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
