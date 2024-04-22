using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models.DatabaseModels
{
    public class Account
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public required byte[] PasswordHash { get; set; } = new byte[32];
        
        [Required]
        public required byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
