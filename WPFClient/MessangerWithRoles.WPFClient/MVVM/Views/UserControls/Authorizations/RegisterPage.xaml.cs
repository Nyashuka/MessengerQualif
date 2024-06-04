using MessengerWithRoles.WPFClient.MVVM.ViewModels;
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

        private void PasswordBox_ConfirmPasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((RegisterPageViewModel)this.DataContext).ConfirmPassword = ((PasswordBox)sender).SecurePassword; }
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((RegisterPageViewModel)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
