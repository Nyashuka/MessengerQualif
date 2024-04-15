using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessangerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessangerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using System.Net.Http;
using System.Net.Http.Json;
using MessangerWithRoles.WPFClient.Data;
using MessangerWithRoles.WPFClient.MVVM.Models;
using System.Windows;
using MessangerWithRoles.WPFClient.Services;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public ICommand Login { get; }
        private bool CanLoginCommandExecute(object p)
        {
            //if (Email.Length < 2 ||
            //    Password.Length < 3)
            //    return false;

            return true;
        }

        private async void OnLoginCommandExecute(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            bool result = await authService.Login(Email, Password);

            if (result)
            {
                EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

                eventBus.Raise(EventBusDefinitions.LoginedInAccount, new EventBusArgs());
            }
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
            _password = string.Empty;

            ChangeToRegisterWindow = new LambdaCommand(OnChangeToRegisterWindowCommandExecute, CanChangeToRegisterWindowCommandExecute);
            Login = new LambdaCommand(OnLoginCommandExecute, CanLoginCommandExecute);
        }
    }
}
