﻿using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
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

        public event Action MessegesListChanged;

        public GroupViewModel(int id, string displayName, string description, 
                              ObservableCollection<User> members,
                              ObservableCollection<Message> messages)
        {
            Id = id;
            _displayName = displayName;
            _description = description;

            _members = members;
            _messages = messages;
        }

        public void AddMessage(MessageDto message, bool isReceived)
        {
            Message newMessage = new Message(message.Sender.DisplayName, message.Data, isReceived);
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Add(newMessage);
                LastMessage = message.Data;
            });
            MessegesListChanged?.Invoke();
        }

        public void UpdateMessages(ObservableCollection<Message> messages)
        {
            Messages = messages;
            LastMessage = messages.Last().Text;
            MessegesListChanged?.Invoke();
        }
    }
}
