using MessangerWithRoles.WPFClient.Services.ServiceLocator;
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
    }

}
