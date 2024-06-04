using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using System.Net.Http;
using System.Net.Http.Json;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Windows;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System.Net.Mail;
using System.Security;
using MessengerWithRoles.WPFClient.Common;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private SecureString _password;
        public SecureString Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        public ICommand Login { get; }
        private bool CanLoginCommandExecute(object p)
        {
            if (!MailAddress.TryCreate(Email, out var mail))
            {
                ErrorMessage = "Email is incorrect";
                return false;
            }

            return true;
        }

        private async void OnLoginCommandExecute(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            await authService.Login(Email, Password.ToPlainString());
        }

        public ICommand ChangeToRegisterWindow { get; }
        private bool CanChangeToRegisterWindowCommandExecute(object p)
        {
            return true;
        }

        private void OnChangeToRegisterWindowCommandExecute(object p)
        {
            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            eventBus.Raise(EventBusDefinitions.NeedToChangeWindowContent, new UserControlEventBusArgs(new RegisterPage()));
        }

        public LoginPageViewModel()
        {
            _email = string.Empty;
            _password = "123321".ToSecureString();

            ChangeToRegisterWindow = new LambdaCommand(OnChangeToRegisterWindowCommandExecute, CanChangeToRegisterWindowCommandExecute);
            Login = new LambdaCommand(OnLoginCommandExecute, CanLoginCommandExecute);

            Email = "user@gmail.com";
            Password = "123321".ToSecureString();
        }
    }
}
