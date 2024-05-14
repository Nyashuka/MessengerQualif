using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.DTOs
{
    public class RolePermissionRelationDto
    {
        public int RoleId { get; set; }
        public int ChatPermissionId { get; set; }
        public bool IsAllowed { get; set; }
        public ChatPermission? ChatPermission { get; set; }
    }
}
