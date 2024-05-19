using MessengerWithRoles.WPFClient.MVVM.Infrastracture.Commands;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels.Base;
using MessengerWithRoles.WPFClient.MVVM.Views.UserControls.ChatSettingsPages;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MessengerWithRoles.WPFClient.MVVM.ViewModels
{
    public class RoleToConfigureViewModel : BaseViewModel
    {
        public RoleWithPermissions Role { get; private set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private List<RolePermissionSettingsViewModel> _permissions;
        public List<RolePermissionSettingsViewModel> Permissions
        {
            get => _permissions;
            set => Set(ref _permissions, value);
        }

        private List<RoleAssignmentViewModel> _assignments;
        public List<RoleAssignmentViewModel> Assignments
        {
            get => _assignments;
            set => Set(ref _assignments, value);
        }

        public ICommand AsignUserCommand { get; }

        private bool CanExecuteAsignUserCommand(object p) => true;

        private async void OnExecuteAsignUserCommand(object p)
        {
            var user = p as RoleAssignmentViewModel;

            var rolesService = ServiceLocator.Instance.GetService<RolesService>();

            var userRoleRelation = new UserRoleRalations()
            {
                RoleId = Role.Id,
                UserId = user.User.Id,
            };

            if (user.Asigned)
            {
                var response = await rolesService.AssignRole(userRoleRelation);

                if (!response.Success)
                {
                    MessageBox.Show(response.Message);
                }
            }
            else
            {
                var response = await rolesService.UnAssignRole(userRoleRelation);

                if (!response.Success)
                {
                    MessageBox.Show(response.Message);
                }
            }

        }


        public RoleToConfigureViewModel(RoleWithPermissions role, List<ChatPermission> allPermissions,
                                        List<User> members, List<User> alreadyAssigned)
        {
            AsignUserCommand = new LambdaCommand(OnExecuteAsignUserCommand, CanExecuteAsignUserCommand);

            Role = role;

            Name = role.Name;

            Permissions = new List<RolePermissionSettingsViewModel>();
            foreach (var permission in allPermissions)
            {
                if (Role.Permissions.Any(x => x.Id == permission.Id))
                    Permissions.Add(new RolePermissionSettingsViewModel()
                    {
                        Id = permission.Id,
                        Name = permission.Name,
                        IsAllowed = true
                    });
                else
                {
                    Permissions.Add(new RolePermissionSettingsViewModel()
                    {
                        Id = permission.Id,
                        Name = permission.Name,
                        IsAllowed = false
                    });
                }
            }

            Assignments = new List<RoleAssignmentViewModel>();
            foreach (var member in members)
            {
                if (alreadyAssigned.Any(x => x.Id == member.Id))
                {
                    Assignments.Add(new RoleAssignmentViewModel()
                    {
                        User = member,
                        Asigned = true
                    });
                }
                else
                {
                    Assignments.Add(new RoleAssignmentViewModel()
                    {
                        User = member,
                        Asigned = false
                    });
                }
            }
        }

        public List<ChatPermission> GetChatPermissions()
        {
            var chatPermissions = new List<ChatPermission>();

            foreach(var permission in Permissions)
            {
                if (permission.IsAllowed)
                {
                    chatPermissions.Add(new ChatPermission()
                    {
                        Id = permission.Id,
                        Name = permission.Name,
                    });
                }
            }

            return chatPermissions;
        }

    }
}
