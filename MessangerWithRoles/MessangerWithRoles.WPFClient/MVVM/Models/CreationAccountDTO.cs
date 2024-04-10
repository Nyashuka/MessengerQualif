using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerWithRoles.WPFClient.MVVM.Models
{
    public class CreationAccountDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ProfileName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
