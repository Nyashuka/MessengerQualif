using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Collections.Generic;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class CreateGroupChatDto
    {
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; } = new List<User>();
        public GroupChatInfoDto? ChatInfo { get; set; } = new GroupChatInfoDto();
    }
}
