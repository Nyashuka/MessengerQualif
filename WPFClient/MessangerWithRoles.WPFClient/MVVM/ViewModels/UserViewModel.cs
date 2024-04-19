using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using System.Net.Http;
using System.Net.Http.Json;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private User _user;
        public User User { get => _user; set => Set(ref _user, value); }

        public ICommand AddToFriend { get; }
        private bool CanAddToFriendCommandExecute(object p) => true;
        private async void OnAddToFriendCommandExecute(object p)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{APIEndpoints.AddFriendGET}?accessToken={authService.AccessToken}&friendUserId={User.Id}");
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            if(data == null)
            {
                MessageBox.Show("Cant pars data");
                return;
            }

            if (!data.Success)
            {
                MessageBox.Show(data.ErrorMessage);
                return;
            }

            MessageBox.Show("Added to friends " + User.DisplayName);
        }

        public UserViewModel(User user)
        {
            User = user;

            AddToFriend = new LambdaCommand(OnAddToFriendCommandExecute, CanAddToFriendCommandExecute);
        }
    }
}
