using MessangerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessangerWithRoles.WPFClient.MVVM.Views.UserControls;
using MessangerWithRoles.WPFClient.Services.EventBusModule;
using MessangerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessangerWithRoles.WPFClient.Services.ServiceLocator;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MessangerWithRoles.WPFClient.MVVM.ViewModels
{
    public class AuthorizationWindowViewModel : BaseViewModel
    {
        private UserControl _currentWindowContent;

        public UserControl CurrentWindowContent
        {
            get => _currentWindowContent;
            set => Set(ref _currentWindowContent, value);
        }

        private void ChangeWindowsControl(IEventBusArgs userControlEventBusArgs)
        {
            CurrentWindowContent = ((UserControlEventBusArgs)userControlEventBusArgs).UserControl;
        }

        public AuthorizationWindowViewModel() 
        {
            _currentWindowContent = new LoginPage();

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();

            eventBus.Subscribe<UserControlEventBusArgs>(EventBusDefinitions.NeedToChangeWindowContent, ChangeWindowsControl);
        }    
    }
}
