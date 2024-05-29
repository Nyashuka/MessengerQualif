using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class GroupViewModel : BaseViewModel
    {
        public int Id { get; private set; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => Set(ref _displayName, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private string _lastMessage;
        public string LastMessage { get => _lastMessage; private set => Set(ref _lastMessage, value); }

        private ObservableCollection<User> _members;
        public ObservableCollection<User> Members
        {
            get => _members;
            set => Set(ref _members, value);
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set => Set(ref _messages, value);
        }

        private ObservableCollection<RoleWithPermissions> _roles;
        public ObservableCollection<RoleWithPermissions> Roles
        {
            get => _roles;
            set => Set(ref _roles, value);
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => Set(ref _imageSource, value);
        }

        private string _status;
        public string Status
        {
            get => _status;
            set => Set(ref _status, _status);
        }

        public event Action MessegesListChanged;

        public GroupViewModel(int id, string displayName, string description,
                              string imageSource,
                              ObservableCollection<User> members,
                              ObservableCollection<Message> messages,
                              ObservableCollection<RoleWithPermissions> roles)
        {
            Id = id;
            _displayName = displayName;
            _description = description;

            _members = members;
            _messages = messages;

            _roles = roles;

            _status = GetChatMembersStatus();

            UpdateLastMessage();

            _imageSource = $"{APIEndpoints.ChatsServer}/{imageSource}?timestamp={DateTime.Now.Ticks}";

            var eventBus = ServiceLocator.Instance.GetService<EventBus>();
            eventBus.Subscribe<MessageDeletedEventBusArgs>(EventBusDefinitions.MessageDeleted, OnMessageDeleted);
        }
        private void OnMessageDeleted(IEventBusArgs e)
        {
            var deleteMessage = e as MessageDeletedEventBusArgs;

            if (deleteMessage.ChatId == Id)
            {
                DeleteMessageById(deleteMessage.MessageId);
            }
        }

        public void AddMessage(MessageDto message, bool isReceived)
        {
            Message newMessage = new Message(message.Id, message.Sender.DisplayName, message.Sender.AvatarURL, message.Data, isReceived);
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Add(newMessage);
                LastMessage = message.Data;
            });
            MessegesListChanged?.Invoke();
        }

        public void DeleteMessage(Message message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Remove(message);
                UpdateLastMessage();
            });
            MessegesListChanged?.Invoke();
        }

        public void DeleteMessageById(int messageId)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Message? message = Messages.FirstOrDefault(x => x.Id == messageId);
                if (message != null)
                {
                    Messages.Remove(message);
                    UpdateLastMessage();
                }
            });
            MessegesListChanged?.Invoke();
        }

        private void UpdateLastMessage()
        {
            if (Messages != null && Messages.Count > 0)
            {
                var lastMessage = Messages.Last();
                LastMessage = lastMessage.Text == null ? "" : lastMessage.Text;
            }
        }

        public string GetChatMembersStatus()
            => Members.Count() > 1 ? Members.Count() + " Members" : Members.Count() + " Member";

        public void UpdateMessages(ObservableCollection<Message> messages)
        {
            Messages = messages;
            UpdateLastMessage();
            MessegesListChanged?.Invoke();
        }

        public void AddMember(User user)
        {
            Members.Add(user);
            Status = GetChatMembersStatus();
        }

        public void DeleteMember(User user)
        {
            Members.Remove(user);
            Status = GetChatMembersStatus();
        }

        public void AddRole(RoleWithPermissions role)
        {
            Roles.Add(role);
        }

        public void UpdateInfo(GroupChatInfoDto groupChatInfoDto)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                DisplayName = groupChatInfoDto.Name;
                Description = groupChatInfoDto.Description;
            });
        }

        public void UpdateRole(RoleWithPermissions role)
        {
            var roleToUpdate = Roles.FirstOrDefault(x => x.Id == role.Id);

            if (roleToUpdate != null)
            {
                Roles.Remove(roleToUpdate);
                Roles.Add(role);
                var roleList = new List<RoleWithPermissions>(Roles);
                roleList.Sort((x, y) => x.Id.CompareTo(y.Id));
                Roles = new ObservableCollection<RoleWithPermissions>(roleList);
            }
        }
    }
}
