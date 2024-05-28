using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Microsoft.Win32;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.Data;
using System.Net.Http.Json;
using MessengerWithRoles.WPFClient.Data.Requests;

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

        private string _profilePictureUrl;
        public string ProfilePictureUrl
        {
            get => _profilePictureUrl;
            set => Set(ref _profilePictureUrl, value);
        }

        private AccountService _accountService;

        private async Task UploadFileAsync(string filePath)
        {
            var accountService = ServiceLocator.Instance.GetService<AccountService>();

            var response = await accountService.UpdatePicture(filePath);

            ProfilePictureUrl = response.Data;
        }

        public ICommand UpdateProfilePictureCommand { get; }
        private bool CanExecuteUpdateProfilePictureCommand(object p) => true;
        private async void OnExecuteUpdateProfilePictureCommand(object p)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                await UploadFileAsync(openFileDialog.FileName);
            }
        }

        public ICommand UpdateProfileInfoCommand { get; }
        private bool CanExecuteUpdateProfileInfoCommand(object p) => true;
        private async void OnExecuteUpdateProfileInfoCommand(object p)
        { 
            User user = new User()
            {
                Id = _accountService.User.Id,
                DisplayName = DisplayName,
                Username = Username,
                AvatarURL = _accountService.User.AvatarURL,
            };

            var result = await _accountService.UpdateProfileInfo(user);
            
            if(result.Success) 
            {
                MessageBox.Show("Profile updated succesfully.");
            }
        }

        public AccountSettingsViewModel()
        {
            _accountService = ServiceLocator.Instance.GetService<AccountService>();

            _displayName = _accountService.User.DisplayName;
            _username = _accountService.User.Username;
            _email = _accountService.Email;
            _profilePictureUrl = _accountService.UserProfile.AvatarURL;//string.IsNullOrEmpty(_accountService.User.AvatarURL) ? "" : _accountService.User.AvatarURL;

            UpdateProfilePictureCommand =
                new LambdaCommand(OnExecuteUpdateProfilePictureCommand, CanExecuteUpdateProfilePictureCommand);

            UpdateProfileInfoCommand =
                new LambdaCommand(OnExecuteUpdateProfileInfoCommand, CanExecuteUpdateProfileInfoCommand);
        }
    }
}
