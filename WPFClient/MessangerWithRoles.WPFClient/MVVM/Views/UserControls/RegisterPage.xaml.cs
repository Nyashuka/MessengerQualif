using MessangerWithRoles.WPFClient.MVVM.ViewModels;
using System.Windows.Controls;

namespace MessengerWithRoles.WPFClient.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : UserControl
    {
        public RegisterPage()
        {
            InitializeComponent();

            DataContext = new RegisterPageViewModel();
        }
    }
}
