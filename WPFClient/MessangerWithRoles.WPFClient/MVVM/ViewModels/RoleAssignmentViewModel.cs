using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class RoleAssignmentViewModel : BaseViewModel
    {
        public User User { get; set; }

        private bool _asigned;
        public bool Asigned
        {
            get => _asigned;
            set => Set(ref _asigned, value);
        }
    }
}
