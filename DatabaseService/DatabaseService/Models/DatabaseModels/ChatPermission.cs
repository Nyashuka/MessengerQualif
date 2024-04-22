using System.ComponentModel.DataAnnotations;

namespace DatabaseService.Models.DatabaseModels
{
    public class ChatPermission
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
