namespace DatabaseService.Models
{
    public class RolePermissionRelation
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public bool IsAllowed { get; set; }

        public required Role Role { get; set; }

        public int ChatPermissionId {  get; set; }
        public ChatPermission ChatPermission { get; set; }
    }
}
