using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Data.Requests;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class CreatePersonalChatViewModel : BaseViewModel
    {
        private List<UserViewModel> _users;
        public List<UserViewModel> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        public ICommand LoadUsers { get; }
        private bool CanLoadUsersCommandExecute(object p) => true;
        private async void OnLoadUsersCommandExecute(object p)
        {
            Users = (await GetAllFriendsFromServer()).Select(user => new UserViewModel(user)).ToList();
        }

        private async Task<List<User>> GetAllFriendsFromServer()
        {
            HttpClient httpClient = new HttpClient();
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();

            var response = await httpClient.GetAsync($"{APIEndpoints.GetAllFriendsGET}?accessToken={authService.AccessToken}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data.Data;
        }

        public CreatePersonalChatViewModel()
        {
            _users = new List<UserViewModel>();
            LoadUsers = new LambdaCommand(OnLoadUsersCommandExecute, CanLoadUsersCommandExecute);
        }
    }
}
