using MessangerWithRoles.WPFClient.MVVM.ViewModels;
using System.Windows.Controls;

namespace MessangerWithRoles.WPFClient.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();

            DataContext = new LoginPageViewModel();
        }


    }
}
