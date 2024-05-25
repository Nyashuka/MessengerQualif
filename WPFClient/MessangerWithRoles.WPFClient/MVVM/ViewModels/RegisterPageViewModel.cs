﻿using MessengerWithRoles.WPFClient.MVVM.Views.Windows;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System.Net.Mail;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        private CreationAccount _creationAccount;
        public CreationAccount CreationAccount
        {
            get => _creationAccount;
            set => Set(ref _creationAccount, value);
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
            if (string.IsNullOrEmpty(CreationAccount.Email))
            {
                ErrorMessage = "Email can't be empty";
                return false;
            }

            if (!MailAddress.TryCreate(CreationAccount.Email, out var mail))
            {
                ErrorMessage = "Email is incorrect";
                return false;
            }

            if (string.IsNullOrEmpty(CreationAccount.DisplayName))
            {
                ErrorMessage = "Display name can't be empty";
                return false;
            }

            if (CreationAccount.Username.Length < 3)
            {
                ErrorMessage = "Username must be more than 2 characters";
                return false;
            }     

            bool passwordsIsSame = CreationAccount.ConfirmPassword.Equals(CreationAccount.Password);

            if (!passwordsIsSame)
            {
                ErrorMessage = "Passwords must be same!";
                return false;
            }

            ErrorMessage = "";

            return passwordsIsSame;
        }

        private async void OnRegisterCommandExecute(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            bool result = await authService.Register(CreationAccount);
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
            _creationAccount = new CreationAccount();
            _errorMessage = string.Empty;

            Register = new LambdaCommand(OnRegisterCommandExecute, CanRegisterCommandExecute);
            ChangeToLoginWindow = new LambdaCommand(OnChangeToLoginWindowCommandExecute, CanChangeToLoginWindowCommandExecute);
        }
    }
}
