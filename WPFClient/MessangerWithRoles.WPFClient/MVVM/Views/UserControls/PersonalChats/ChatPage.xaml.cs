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
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : UserControl
    {
        public ChatPage(ChatPageViewModel chatPageViewModel)
        {
            DataContext = chatPageViewModel;

            InitializeComponent();

            ((INotifyCollectionChanged)messagesListView.ItemsSource).CollectionChanged +=
             (s, e) =>
             {
                 if (e.Action == NotifyCollectionChangedAction.Add && messagesListView.Items.Count > 0)
                 {
                     messagesListView.ScrollIntoView(messagesListView.Items[messagesListView.Items.Count - 1]);
                 }
             };

            chatPageViewModel.MessegesListChanged += ScrollIntoLastMessage;
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
            var listView = sender as ListView;
            var item = GetAncestorOfType<ListViewItem>((DependencyObject)e.OriginalSource);

            if (item != null)
            {
                listView.SelectedItem = item.Content;
                listView.ContextMenu.DataContext = item.Content;
                listView.ContextMenu.IsOpen = true;
            }
        }

        private T GetAncestorOfType<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        private async void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = messagesListView.ContextMenu.DataContext as Message;
            if (item != null)
            {
                await ((ChatPageViewModel)DataContext).RemoveMessage(item);
            }
        }

        private void messagesListView_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollIntoLastMessage();
        }
    }
}
