using MessangerWithRoles.WPFClient.MVVM.Models;
using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MessangerWithRoles.WPFClient.MVVM.Infrastracture.Commands;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private User _user;
        public User User { get => _user; set => Set(ref _user, value); }

        public ICommand AddToFriend { get; }
        private bool CanAddToFriendCommandExecute(object p) => true;
        private void OnAddToFriendCommandExecute(object p)
        {
            MessageBox.Show(_user.Username);
        }

        public UserViewModel(User user)
        {
            User = user;

            AddToFriend = new LambdaCommand(OnAddToFriendCommandExecute, CanAddToFriendCommandExecute);
        }
    }
}
