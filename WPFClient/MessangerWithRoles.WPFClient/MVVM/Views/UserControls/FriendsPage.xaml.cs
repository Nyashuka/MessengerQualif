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
    }
}
