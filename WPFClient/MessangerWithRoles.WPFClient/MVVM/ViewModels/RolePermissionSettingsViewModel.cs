using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class RolePermissionSettingsViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private bool _isAllowed;
        public bool IsAllowed
        {
            get => _isAllowed;
            set => Set(ref _isAllowed, value);
        }
    }
}
