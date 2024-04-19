using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private UserControl _currentContent;
        public UserControl CurrentContent { get => _currentContent; set => Set(ref _currentContent, value); }

        public ObservableCollection<Chat> Chats { get; private set; }
        public ObservableCollection<Message> Messages { get; private set; }

        public ICommand OpenFriendsWindow { get; }

        public bool CanExecuteOpenFriendsWindowCommand(object p) => true;

        public void OnExecuteOpenFriendsWindowCommand(object p)
        {
            CurrentContent = new FriendsPage();
        }

        public ICommand OpenCreateChatWindow { get; }

        public bool CanExecuteOpenCreateChatWindowCommand(object p) => true;

        public void OnExecuteOpenCreateChatWindowCommand(object p)
        {
            CurrentContent = new CreatePersonalChatPage();
            CurrentContent.DataContext = new CreatePersonalChatViewModel();
        }

        public MainWindowViewModel()
        {
            OpenFriendsWindow = new LambdaCommand(OnExecuteOpenFriendsWindowCommand, CanExecuteOpenFriendsWindowCommand);
            OpenCreateChatWindow = new LambdaCommand(OnExecuteOpenCreateChatWindowCommand, CanExecuteOpenCreateChatWindowCommand);

            Chats = new ObservableCollection<Chat>();
            for (int i = 0; i < 100; i++)
            {
                Chats.Add(new Chat("Evelin Parker " + i,
                    "https://i.pinimg.com/originals/e7/da/8d/e7da8d8b6a269d073efa11108041928d.jpg",
                    new ObservableCollection<Message>()));
            }

            Messages = new ObservableCollection<Message>();

            Messages.Add(new Message("Evelin Parker", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", true));
            Messages.Add(new Message("Evelin Parker", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", true));
            Messages.Add(new Message("Evelin Parker", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", true));
            Messages.Add(new Message("Me", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", false));
            Messages.Add(new Message("Me", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", false));
            Messages.Add(new Message("Me", "dajasjdsadjaskldjaslkdjaslkjdlkasjdlkajlsdjk", false));
        }
    }
}
