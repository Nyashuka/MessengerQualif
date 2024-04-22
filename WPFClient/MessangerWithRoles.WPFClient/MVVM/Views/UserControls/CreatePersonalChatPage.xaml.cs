using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;

namespace MessengerWithRoles.WPFClient.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CreatePersonalChatPage.xaml
    /// </summary>
    public partial class CreatePersonalChatPage : UserControl
    {
        public CreatePersonalChatPage()
        {
            InitializeComponent();
        }

        private void CreatePersonalChatPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            var vm = (CreatePersonalChatViewModel)DataContext;

            vm.LoadUsers.Execute(this);
        }
    }
}
