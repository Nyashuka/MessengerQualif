using MessangerWithRoles.WPFClient.Data;
using MessangerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessangerWithRoles.WPFClient.MVVM.Models;
using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
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
            Users = (await GetAllUsersFromServer()).Select(user => new UserViewModel(user)).ToList();
        }

        public ICommand AddFriend { get; }
        private bool CanAddFriendCommandExecute(object p) => true;
        private void OnAddFriendCommandExecute(object p)
        {
            MessageBox.Show((p as User).Username);
        }

        private async Task<List<User>> GetAllUsersFromServer()
        {
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(APIEndpoints.GetAllUsersGET);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>();

            return data.Data;
        }

        public FriendsPageViewModel() 
        {
            LoadUsers = new LambdaCommand(OnLoadUsersCommandExecute, CanLoadUsersCommandExecute);
            AddFriend = new LambdaCommand(OnAddFriendCommandExecute, CanAddFriendCommandExecute);
        }
    }
}
