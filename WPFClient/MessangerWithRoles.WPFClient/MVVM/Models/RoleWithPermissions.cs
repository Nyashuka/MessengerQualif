using System.Collections.Generic;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class RoleWithPermissions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChatPermission>? Permissions { get; set; }
    }
}
