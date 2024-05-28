using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace MessengerWithRoles.WPFClient.MVVM.Views.UserControls
{
    /// <summary>
    /// Interaction logic for GroupPage.xaml
    /// </summary>
    public partial class GroupPage : UserControl
    {
        public GroupPage(GroupPageViewModel groupViewModel)
        {
            DataContext = groupViewModel;

            InitializeComponent();

            ((INotifyCollectionChanged)messagesListView.ItemsSource).CollectionChanged +=
             (s, e) =>
             {
                 if (e.Action == NotifyCollectionChangedAction.Add && messagesListView.Items.Count > 0)
                 {
                     messagesListView.ScrollIntoView(messagesListView.Items[messagesListView.Items.Count - 1]);
                 }
             };

            groupViewModel.MessegesListChanged += ScrollIntoLastMessage;
        }

        public void ScrollIntoLastMessage()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                if (messagesListView.Items != null && messagesListView.Items.Count > 0)
                    messagesListView.ScrollIntoView(messagesListView.Items[messagesListView.Items.Count - 1]);
            });
        }

        private void MyListView_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = (sender as ListView)?.SelectedItem;
            if (item != null)
            {
                messagesListView.ContextMenu.DataContext = item;
                messagesListView.ContextMenu.IsOpen = true;
            }
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = messagesListView.ContextMenu.DataContext as Message;
            if (item != null)
            {
                 await ((GroupPageViewModel)DataContext).RemoveMessage(item);
            }
        }

        private void messagesListView_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollIntoLastMessage();
        }
    }
}
