using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Collections.Generic;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class ChatDto
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; }
        public GroupChatInfoDto? ChatInfo { get; set; }
    }
}
