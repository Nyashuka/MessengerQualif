namespace DatabaseService.Models.DatabaseModels
{
    public class RolePermissionRelation
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public int ChatPermissionId { get; set; }
        public ChatPermission ChatPermission { get; set; }

        public bool IsAllowed { get; set; }
    }
}
