using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
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
