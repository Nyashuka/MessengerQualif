using MessagesService.DTOs;

namespace MessagesService.Chat.Dto
{
    public class ChatDto
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<UserDto>? Members { get; set; }
        public GroupChatInfoDto? ChatInfo { get; set; }
    }
}