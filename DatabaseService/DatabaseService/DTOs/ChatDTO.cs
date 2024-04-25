﻿using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.DTOs
{
    public class ChatDto
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<UserDto>? Members { get; set; }
        public GroupChatInfo? ChatInfo { get; set; }
    }
}
