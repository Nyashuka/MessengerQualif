using MessengerDatabaseService.Models;

namespace MessengerDatabaseService.DTOs
{
    public class ChatMemberDTO
    {
        public required int ChatId { get; set; }
        public required int UserId { get; set; }
    }
}
