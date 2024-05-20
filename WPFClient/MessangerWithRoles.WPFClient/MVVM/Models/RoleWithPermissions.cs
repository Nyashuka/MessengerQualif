using System.Collections.Generic;
using System.ComponentModel;

namespace MessengerWithRoles.WPFClient.MVVM.Models
{
    public class RoleWithPermissions : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private int _priority;

        public int Priority
        {
            get => _priority;
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    OnPropertyChanged(nameof(Priority));
                }
            }
        }
        public List<ChatPermission>? Permissions { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
