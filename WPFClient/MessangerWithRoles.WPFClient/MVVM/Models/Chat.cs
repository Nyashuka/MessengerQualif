using MessengerWithRoles.WPFClient.DTOs;
using System.Collections.Generic;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; }
        public GroupChatInfoDto? ChatInfo { get; set; }
    }
}
