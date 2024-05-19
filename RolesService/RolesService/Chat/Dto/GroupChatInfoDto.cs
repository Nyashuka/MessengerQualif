using RolesService.Users;

namespace RolesService.Chat.Dto
{
    public class GroupChatInfoDto
    {
        public User? Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}