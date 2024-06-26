﻿using AccountManagementService.Models;

namespace AccountManagementService.DTOs
{
    public class GroupChatInfoDto
    {
        public int Id { get; set; }
        public User? Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}
