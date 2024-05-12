using MessengerWithRoles.WPFClient.MVVM.Models;

namespace MessengerWithRoles.WPFClient.DTOs
{
    public class GroupChatInfoDto
    {
        public User? Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string AvatarUrl { get; set; }
    }
}
