

using MessengerWithRoles.WPFClient.MVVM.Models;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using System.Text.RegularExpressions;

namespace MessengerWithRoles.WPFClient.MVVM.Views.UserControls.ChatSettingsPages
{
    /// <summary>
    /// Interaction logic for RoleListSettings.xaml
    /// </summary>
    public partial class RoleListSettings : UserControl
    {
        private Point _dragStartPoint;

        public RoleListSettings()
        {
            InitializeComponent();
        }
        //private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (sender is ListBox listBox)
        //    {
        //        _dragStartPoint = e.GetPosition(null);
        //    }
        //}

        //private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    Point mousePos = e.GetPosition(null);
        //    Vector diff = _dragStartPoint - mousePos;

        //    if (e.LeftButton == MouseButtonState.Pressed &&
        //        (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //         Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        //    {
        //        if (sender is ListBox listBox && listBox.SelectedItem is RoleWithPermissions selectedRole)
        //        {
        //            DragDrop.DoDragDrop(listBox, selectedRole, DragDropEffects.Move);
        //        }
        //    }
        //}

        //private void ListBox_Drop(object sender, DragEventArgs e)
        //{
        //    if (sender is ListBox listBox)
        //    {
        //        RoleWithPermissions droppedData = e.Data.GetData(typeof(RoleWithPermissions)) as RoleWithPermissions;
        //        RoleWithPermissions target = ((FrameworkElement)e.OriginalSource).DataContext as RoleWithPermissions;

        //        if (droppedData != null && target != null && droppedData != target)
        //        {
        //            int removedIdx = listBox.Items.IndexOf(droppedData);
        //            int targetIdx = listBox.Items.IndexOf(target);

        //            if (removedIdx < targetIdx)
        //            {
        //                (listBox.DataContext as GroupPageViewModel).Group.Roles.Insert(targetIdx + 1, droppedData);
        //                (listBox.DataContext as GroupPageViewModel).Group.Roles.RemoveAt(removedIdx);
        //            }
        //            else
        //            {
        //                int remIdx = removedIdx + 1;
        //                if ((listBox.DataContext as GroupPageViewModel).Group.Roles.Count + 1 > remIdx)
        //                {
        //                    (listBox.DataContext as GroupPageViewModel).Group.Roles.Insert(targetIdx, droppedData);
        //                    (listBox.DataContext as GroupPageViewModel).Group.Roles.RemoveAt(remIdx);
        //                }
        //            }

        //            // Перенумерація пріоритетів
        //            for (int i = 0; i < (listBox.DataContext as GroupPageViewModel).Group.Roles.Count; i++)
        //            {
        //                (listBox.DataContext as GroupPageViewModel).Group.Roles[i].Priority = i + 1;
        //            }
        //        }
        //    }
        //}
    }
}
