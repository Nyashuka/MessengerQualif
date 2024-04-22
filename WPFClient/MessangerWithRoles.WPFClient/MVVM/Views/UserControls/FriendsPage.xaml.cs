using System.Windows;
using System.Windows.Controls;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;

namespace MessengerWithRoles.WPFClient.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for FriendsPage.xaml
    /// </summary>
    public partial class FriendsPage : UserControl
    {
        public FriendsPage()
        {
            InitializeComponent();
            DataContext = new FriendsPageViewModel();
        }

        private void FriendsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var vm = (FriendsPageViewModel)DataContext;

            vm.LoadUsers.Execute(this);
            vm.LoadFriends.Execute(this);
        }
    }
}
