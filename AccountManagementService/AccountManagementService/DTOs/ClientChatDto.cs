namespace AccountManagementService.DTOs
{
    public class ClientChatDto
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<UserDto>? Members { get; set; }
       // public ChatInfo? ChatInfo{ get; set; }
    }
}
