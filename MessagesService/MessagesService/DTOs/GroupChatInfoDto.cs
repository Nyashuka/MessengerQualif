
using MessagesService.DTOs;

public class GroupChatInfoDto
    {
        public int Id { get; set; }
        public UserDto? OwnerUser { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }

