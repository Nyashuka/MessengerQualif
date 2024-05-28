using MessengerWithRoles.WPFClient.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        private string? _avatarURL;
        public string? AvatarURL
        {
            get => _avatarURL;
            set => _avatarURL = $"{APIEndpoints.ProfileServer}/{value}?timestamp={DateTime.Now.Ticks}";
        }
    }
}
