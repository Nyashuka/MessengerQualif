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
        public string MessengerName
        {
            get { return "Nyatter"; }
        }

        public App()
        {
            InitializeComponent();

            ServiceLocator.Instance.RegisterService(new AuthService());
        }

    }

}
