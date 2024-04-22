using DatabaseService.Models.DatabaseModels;

namespace DatabaseService.DTOs
{
    public class PersonalChatDto
    {
        public int ChatTypeId { get; set; }
        public User? FirstUser { get; set; }
        public User? SecondUser { get; set; }
    }
}
