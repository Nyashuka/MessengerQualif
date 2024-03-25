﻿using System.ComponentModel.DataAnnotations;

namespace MessengerDatabaseService.DTOs
{
    public class AccountDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
