using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessangerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services.ServiceLocator;
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
            HttpClient httpClient = new HttpClient();

            var loginData = new AccountLogin();
            loginData.Email = Email;
            loginData.Password = Password;

            HttpResponseMessage? response = null;
            try
            {
                response = await httpClient.PostAsJsonAsync(APIEndpoints.LoginPOST, loginData);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            
            if (response == null)
            {
                MessageBox.Show("Login Response in empty");
                return;
            }

            var dataFromResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            if(dataFromResponse == null)
            {
                MessageBox.Show("Cannot parse data from login response!");
                return;
            }

            if(!dataFromResponse.Success)
            {
                MessageBox.Show(dataFromResponse.ErrorMessage);
                return;
            }

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();
            //eventBus.Raise(EventBusDefinitions.NeedToChangeWindowContent, new UserControlEventBusArgs(new MainLoginedPage()));
            eventBus.Raise(EventBusDefinitions.LoginedInAccount, new EventBusArgs());
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
