﻿namespace DatabaseService.Models.DatabaseModels
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}
