using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event Action<UserControl> NeedChangePage;

        public event PropertyChangedEventHandler? PropertyChanged;

        public void InvokeNeedChangePage(UserControl userControl)
        {
            NeedChangePage?.Invoke(userControl);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

    }
}
