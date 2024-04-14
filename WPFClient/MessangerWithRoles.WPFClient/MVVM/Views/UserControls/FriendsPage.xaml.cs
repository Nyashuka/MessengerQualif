using MessangerWithRoles.WPFClient.MVVM.ViewModels;
using System.Windows.Controls;

namespace MessangerWithRoles.WPFClient.MVVM.Views.UserControls
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
