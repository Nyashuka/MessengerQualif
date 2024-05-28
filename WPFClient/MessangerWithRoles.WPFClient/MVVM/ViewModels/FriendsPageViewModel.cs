using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
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
            var users = await GetOtherUsersFromServer();
            Users = new List<UserViewModel>();

            foreach (var user in users)
            { 
                var userViewModel = new UserViewModel(user);
                userViewModel.AddToFriendUserEvent += OnAddUserToFried;
                Users.Add(userViewModel);
            }
        }

        private async void OnAddUserToFried(User user)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"{APIEndpoints.AddFriendGET}?accessToken={authService.AccessToken}&friendUserId={user.Id}");
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            if (data == null)
            {
                MessageBox.Show("Cant pars data");
                return;
            }

            if (!data.Success)
            {
                MessageBox.Show(data.Message);
                return;
            }

            var friends = Friends;
            friends.Add(Users.First(x => x.User.Id == user.Id));
            Friends = null;
            Friends = friends;
            MessageBox.Show("Added to friends " + user.DisplayName);
        }

        private async void OnDeleteFriend(User user)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();

            HttpClient httpClient = new HttpClient();

            var response = await httpClient
                .GetAsync($"{APIEndpoints.RemoveFriendGET}?accessToken={authService.AccessToken}&friendUserId={user.Id}");
            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            if (data == null)
            {
                MessageBox.Show("Cant pars data");
                return;
            }

            if (!data.Success)
            {
                MessageBox.Show(data.Message);
                return;
            }

            MessageBox.Show("Deleted friends " + user.DisplayName);

            var friends = Friends;
            friends.Remove(Friends.First(x => x.User.Id == user.Id));
            Friends = null;
            Friends = friends;
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
            var users = await GetAllFriendsFromServer();
            var friends = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = new UserViewModel(user);
                userViewModel.DeleteFriendEvent += OnDeleteFriend;
                friends.Add(userViewModel);
            }

            Friends = friends;
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
        }
    }
}
