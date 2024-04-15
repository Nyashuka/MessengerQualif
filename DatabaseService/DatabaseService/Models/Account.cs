namespace DatabaseService.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required byte[] PasswordHash { get; set; } = new byte[32];
        public required byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
