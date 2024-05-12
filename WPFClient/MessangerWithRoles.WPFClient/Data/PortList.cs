using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Data
{
    public class PortList
    {
        public int Database { get; set; }
        public int Authorization { get; set; }
        public int AccountManagement { get; set; }
        public int Chats { get; set; }
        public int Messages { get; set; }
        public int Roles { get; set; }
        public int Storage { get; set; }
        public int Notification { get; set; }
    }
}
