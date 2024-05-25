using ChatsService.Groups.Models;

namespace ChatsService.Groups.Dto
{
    public class GroupChatInfoDto
    {
        public User? Owner { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? AvatarUrl { get; set; }
    }
}