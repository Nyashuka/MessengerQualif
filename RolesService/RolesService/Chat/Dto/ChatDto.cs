using RolesService.Users;

namespace RolesService.Chat.Dto
{
    public class ChatDto
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; }
        public GroupChatInfoDto? ChatInfo { get; set; }
    }
}