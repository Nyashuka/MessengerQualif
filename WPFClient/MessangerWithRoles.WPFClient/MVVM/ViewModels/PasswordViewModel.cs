using MessengerWithRoles.WPFClient.Common;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System.Security;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class PasswordViewModel : BaseViewModel
    {
        private SecureString _securedPassword;
        public SecureString SecuredPassword
        {
            get => _securedPassword;
            set => Set(ref _securedPassword, value);
        }

        private string _unsecuredPassword;
        public string UnsecuredPassword
        {
            get => _unsecuredPassword;
            set => Set(ref _unsecuredPassword, value);
        }

        public PasswordViewModel()
        {
            _securedPassword = new SecureString();
            _unsecuredPassword = "";
        }
    }
}
