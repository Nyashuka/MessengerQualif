using System.Collections.Generic;
using MessengerWithRoles.WPFClient.MVVM.Models;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class CreateChatDto
    {
        public int ChatTypeId { get; set; }
        public List<User>? Members { get; set; }
    }
}
