using MessagesService.DTOs;

namespace MessagesService.Chat.Dto
{
    public class GroupChatInfoDto
    {
        public UserDto? Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}