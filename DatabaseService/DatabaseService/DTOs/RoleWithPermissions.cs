using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.DTOs
{
    public class RoleWithPermissions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChatId { get; set; }
        public List<ChatPermission>? Permissions { get; set; }
    }
}
