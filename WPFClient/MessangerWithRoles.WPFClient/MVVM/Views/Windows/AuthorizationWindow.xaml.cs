using MessangerWithRoles.WPFClient.MVVM.Views.Windows;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace MessengerWithRoles.WPFClient.MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            EventBus eventBus = new EventBus();
            eventBus.Subscribe<EventBusArgs>(EventBusDefinitions.LoginedInAccount, OnLoginedInAccount);
            ServiceLocator.Instance.RegisterService(eventBus);

            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnLoginedInAccount(IEventBusArgs eventBusArgs)
        {
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            Close();
        }
    }
}