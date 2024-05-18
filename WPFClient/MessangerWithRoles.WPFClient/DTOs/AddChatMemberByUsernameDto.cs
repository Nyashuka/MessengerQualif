using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class AddChatMemberByUsernameDto
    {
        public int ChatId {  get; set; }
        public string Username { get; set; }
    }
}
