using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.DTOs
{
    public class UserRoleRelationDto
    {
        public int UserId { get; set; }
        public UserDto? User { get; set; }

        public int RoleId { get; set; }
        public RoleDto? Role { get; set; }
    }
}
