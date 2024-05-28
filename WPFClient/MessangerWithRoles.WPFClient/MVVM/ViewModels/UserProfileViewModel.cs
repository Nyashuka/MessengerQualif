using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class UserProfileViewModel : BaseViewModel
    {
        public int Id { get; set; }


        private string _email;
        public string Email 
        {
            get => _email; 
            set => Set(ref _email, value); 
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => Set(ref _displayName, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }


        private string _avatarURL;
        public string AvatarURL
        {
            get => _avatarURL;
            set => Set(ref _avatarURL, value);
        }

        public UserProfileViewModel(User user) 
        {
            Id = user.Id;
            _displayName = user.DisplayName;
            _username = user.Username;
            _avatarURL = user.AvatarURL;
        }

        public void Update(User user)
        {
            DisplayName = user.DisplayName;
            Username = user.Username;
        }
    }
}
