using MessangerWithRoles.WPFClient.Data;
using MessangerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessangerWithRoles.WPFClient.MVVM.Models;
using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessangerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessangerWithRoles.WPFClient.MVVM.Views.Windows;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.ServiceLocator;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => Set(ref _confirmPassword, value);
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => Set(ref _errorMessage, value);
        }

        public ICommand Register { get; }
        private bool CanRegisterCommandExecute(object p)
        {
            bool passwordsIsSame = _confirmPassword.Equals(_password);

            if(!passwordsIsSame)
            {
                ErrorMessage = "Passwords must be same!";
            }
            else
            {
                ErrorMessage = "";
            }

            return passwordsIsSame;
        }

        private async void OnRegisterCommandExecute(object p)
        {
            HttpClient client = new HttpClient();

            var accountData = new CreationAccountDTO() 
            {
                Email = _email,
                Password = _password,
                ProfileName = _displayName,
                Username = _username
            };

            HttpResponseMessage? response;

            try
            {
                response = await client.PostAsJsonAsync(APIEndpoints.CreateAccountPOST, accountData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if(!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.ReasonPhrase);
                return;
            }

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            if (data == null)
            {
                MessageBox.Show("Response is success, but data cannot be parsed!");
                return;
            }
                
            if(data.Success)
                MessageBox.Show($"Account created successfully with id={data.Data}");

            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public ICommand ChangeToLoginWindow { get; }
        private bool CanChangeToLoginWindowCommandExecute(object p)
        {
            return true;
        }

        private void OnChangeToLoginWindowCommandExecute(object p)
        {
            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            eventBus.Raise(EventBusDefinitions.NeedToChangeWindowContent, new UserControlEventBusArgs(new LoginPage()));
        }

        public RegisterPageViewModel() 
        {
            _email = string.Empty;
            _password = string.Empty;
            _confirmPassword = string.Empty;
            _displayName = string.Empty;
            _username = string.Empty;
            _errorMessage = string.Empty;

            Register = new LambdaCommand(OnRegisterCommandExecute, CanRegisterCommandExecute);
            ChangeToLoginWindow = new LambdaCommand(OnChangeToLoginWindowCommandExecute, CanChangeToLoginWindowCommandExecute);
        }
    }
}
