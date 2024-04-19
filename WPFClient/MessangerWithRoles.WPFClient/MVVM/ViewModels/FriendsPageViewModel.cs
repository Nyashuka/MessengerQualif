using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class FriendsPageViewModel : BaseViewModel
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value);
        }

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
            Users = (await GetOtherUsersFromServer()).Select(user => new UserViewModel(user)).ToList();
        }

        private List<UserViewModel> _friends;
        public List<UserViewModel> Friends
        {
            get => _friends;
            set => Set(ref _friends, value);
        }

        public ICommand LoadFriends { get; }
        private bool CanLoadFriendsCommandExecute(object p) => true;
        private async void OnLoadFriendsCommandExecute(object p)
        {
            Friends = (await GetAllFriendsFromServer()).Select(user => new UserViewModel(user)).ToList();
        }

        public ICommand AddFriend { get; }
        private bool CanAddFriendCommandExecute(object p) => true;
        private void OnAddFriendCommandExecute(object p)
        {
            MessageBox.Show((p as User).Username);
        }

        private async Task<List<User>> GetOtherUsersFromServer()
        {
            HttpClient httpClient = new HttpClient();
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();

            var response = await httpClient.GetAsync($"{APIEndpoints.GetAllUsersGET}?accessToken={authService.AccessToken}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data.Data;
        }

        private async Task<List<User>> GetAllFriendsFromServer()
        {
            HttpClient httpClient = new HttpClient();
            AuthService authService = ServiceLocator.Instance.GetService<AuthService>();

            var response = await httpClient.GetAsync($"{APIEndpoints.GetAllFriendsGET}?accessToken={authService.AccessToken}");

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data.Data;
        }

        public FriendsPageViewModel() 
        {
            LoadUsers = new LambdaCommand(OnLoadUsersCommandExecute, CanLoadUsersCommandExecute);
            LoadFriends = new LambdaCommand(OnLoadFriendsCommandExecute, CanLoadFriendsCommandExecute);
            AddFriend = new LambdaCommand(OnAddFriendCommandExecute, CanAddFriendCommandExecute);

            LoadFriends.Execute(this);
        }
    }
}
