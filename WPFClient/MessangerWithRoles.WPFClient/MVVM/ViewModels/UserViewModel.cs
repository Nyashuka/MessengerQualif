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
using MessangerWithRoles.WPFClient.Services.ServiceLocator;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services;
using System.Net.Http;
using MessangerWithRoles.WPFClient.Data;
using System.Net.Http.Json;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
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

            var response = await httpClient.GetAsync($"{APIEndpoints.AddFriendGET}?accessToken={authService.AccesToken}&friendUserId={User.Id}");
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
