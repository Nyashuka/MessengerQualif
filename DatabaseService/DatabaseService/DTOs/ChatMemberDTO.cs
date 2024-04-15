using DatabaseService.Models;

namespace DatabaseService.DTOs
{
    public class ChatMemberDTO
    {
        public required int ChatId { get; set; }
        public required int UserId { get; set; }
    }
}
