namespace AccountManagementService.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int FriendAccountId { get; set; }
    }
}