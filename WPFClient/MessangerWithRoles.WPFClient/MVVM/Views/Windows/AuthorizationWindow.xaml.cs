using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessengerWithRoles.WPFClient.MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private bool _isChangingIpState;

        public AuthorizationWindow()
        {
            EventBus eventBus = new EventBus();
            eventBus.Subscribe<EventBusArgs>(EventBusDefinitions.LoginedInAccount, OnLoggedInAccount);
            ServiceLocator.Instance.RegisterService(eventBus);

            InitializeComponent();

            _isChangingIpState = false;
            ipTextBox.Text = APIEndpoints.IPAddress.ToString();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnLoggedInAccount(IEventBusArgs eventBusArgs)
        {
            var mainWindow = new MainWindow();
            var current = Application.Current.MainWindow;

            mainWindow.Show();

            Application.Current.MainWindow = mainWindow;
            current.Close();
        }

        private void ChangeSaveIPButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isChangingIpState)
            {
                APIEndpoints.IPAddress = ipTextBox.Text;
                changeSaveIp_Button.Content = "Change";
                ipTextBox.IsReadOnly = true;
                _isChangingIpState = false;
            }
            else
            {
                changeSaveIp_Button.Content = "Save";
                ipTextBox.IsReadOnly = false;
                ipTextBox.Focus();
                ipTextBox.SelectionStart = ipTextBox.Text.Length;
                _isChangingIpState = true;
            }
        }
    }
}