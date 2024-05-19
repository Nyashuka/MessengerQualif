using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class AccountSettingsViewModel : BaseViewModel
    {
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

        private string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private AccountService _accountService;

        public AccountSettingsViewModel()
        {
            _accountService = ServiceLocator.Instance.GetService<AccountService>();

            _displayName = _accountService.User.DisplayName;
            _username = _accountService.User.Username;
            _email = _accountService.Email;
        }
    }
}
