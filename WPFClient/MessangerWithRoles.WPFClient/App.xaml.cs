using MessengerWithRoles.WPFClient.Services;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MessengerWithRoles.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            RegisterServices();
        }

        private void RegisterServices()
        {
            ServiceLocator.Instance.RegisterService(new RequestService());
            ServiceLocator.Instance.RegisterService(new AuthService());
            ServiceLocator.Instance.RegisterService(new ChatsService());
            ServiceLocator.Instance.RegisterService(new GroupsServcie());
            ServiceLocator.Instance.RegisterService(new MessagesService());
        }
    }

}
