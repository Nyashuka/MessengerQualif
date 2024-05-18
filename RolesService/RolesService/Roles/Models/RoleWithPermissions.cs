using RolesService.Permissions.Models;

namespace RolesService.Roles.Models
{
    public class RoleWithPermissions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChatId { get; set; }
        public List<Permission>? Permissions { get; set; }
    }
}
