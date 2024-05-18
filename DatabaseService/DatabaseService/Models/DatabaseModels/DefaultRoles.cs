namespace DatabaseService.Models.DatabaseModels
{
    public class DefaultRoles
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public Chat? Chat { get; set; }

        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
