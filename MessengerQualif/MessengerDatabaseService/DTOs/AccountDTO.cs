using System.ComponentModel.DataAnnotations;

namespace MessengerDatabaseService.DTOs
{
    public class AccountDTO
    {
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
    }
}
