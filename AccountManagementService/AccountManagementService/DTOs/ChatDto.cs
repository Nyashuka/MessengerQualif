﻿namespace AccountManagementService.DTOs
{
    public class ChatDto
    {
        public int ChatTypeId { get; set; }
        public List<UserDto>? Members { get; set; }
        public GroupChatInfoDto? ChatInfo { get; set; }
    }
}
