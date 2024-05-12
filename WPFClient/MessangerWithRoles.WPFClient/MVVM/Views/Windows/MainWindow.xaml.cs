﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;

namespace MessengerWithRoles.WPFClient.MVVM.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow == null)
                return;

            Application.Current.MainWindow.WindowState = 
                Application.Current.MainWindow.WindowState != WindowState.Maximized ? 
                    WindowState.Maximized : WindowState.Normal;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;

            await vm.LoadChats();
        }

        private void Chat_Selected(object sender, SelectionChangedEventArgs e)
        {
            var chat = (ChatViewModel)((ListView)sender).SelectedItem;

            ((MainWindowViewModel)DataContext).OpenChatWindow(chat);
        }

        private void Group_Selected(object sender, SelectionChangedEventArgs e)
        {
            var group = (GroupViewModel)((ListView)sender).SelectedItem;

            ((MainWindowViewModel)DataContext).OpenGroupWindow(group);
        }
    }
}
