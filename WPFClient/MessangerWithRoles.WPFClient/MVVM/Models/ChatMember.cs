﻿namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class ChatMember
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
