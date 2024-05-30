using System.Net;

namespace MessengerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        private static PortList DefaultPorts = new PortList()
        {
            Authorization = 5292,
            AccountManagement = 5293,
            Chats = 5294,
            Messages = 5295,
            Roles = 5296,
            Notification = 6999
        };

        private static PortList RaspPorts = new PortList()
        {
            Authorization = 15292,
            AccountManagement = 15293,
            Messages = 15294,
            Notification = 16999
        };   

        public static string LocalAddress = "127.0.0.1";

        public static string Server = "91.201.235.100";

        public static string IPAddress = Server; 
        public static PortList Ports = DefaultPorts;

        // auth service
        public static string CreateAccountPOST { get => $"http://{IPAddress}:{Ports.Authorization}/api/Auth/create-account"; }
        public static string LoginPOST { get => $"http://{IPAddress}:{Ports.Authorization}/api/Auth/login"; }

        // account management service
        public static string GetAllUsersGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Users"; }
        public static string GetUserGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Users/get-user"; }
        public static string AddFriendGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement/add-friend"; }
        public static string RemoveFriendGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement/remove-friend"; }
        public static string GetAllFriendsGET { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/FriendsManagement"; }
        public static string UpdateProfilePicturePOST { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Profile/picture"; }
        public static string UpdateProfileInfoPOST { get => $"http://{IPAddress}:{Ports.AccountManagement}/api/Profile"; }

        // chats
        public static string CreatePersonalChatPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats/create-personal"; }
        public static string GetPersonalChatByMembersPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats/get-personal-by-members"; }
        public static string GetAllChatsGET { get => $"http://{IPAddress}:{Ports.Chats}/api/PersonalChats"; }
        public static string GetChatById { get => $"http://{IPAddress}:{Ports.Chats}/api/Chats/get-by-id"; }
        public static string CreateGroupPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/Groups"; }
        public static string GetAllGroupsGET { get => $"http://{IPAddress}:{Ports.Chats}/api/Groups"; }
        public static string UpdateGroupChatInfoPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/GroupsInfo"; }
        public static string UpdateGroupChatPicture { get => $"http://{IPAddress}:{Ports.Chats}/api/GroupsInfo/picture"; }

        // Chat members
        public static string GetChatMembersGET { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }
        public static string AddChatMemberPOST { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }
        public static string AddChatMemberByUsernamePOST { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers/username"; }
        public static string DeleteChatMemberDELETE { get => $"http://{IPAddress}:{Ports.Chats}/api/ChatMembers"; }

        // Permissions
        public static string GetAllPermissionsGET { get => $"http://{IPAddress}:{Ports.Roles}/api/Permissions"; }

        // Roles
        public static string CreateRolePOST { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles"; }
        public static string GetAllChatRolesGET { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles"; }
        public static string AssignRolePOST { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles/assignes"; }
        public static string UnAssignRoleDELETE{ get => $"http://{IPAddress}:{Ports.Roles}/api/Roles/assignes"; }
        public static string GetAllRoleAssignesGET { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles/assignes"; }
        public static string UpdateRolePATCH { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles"; }
        public static string DeleteRoleDELETE { get => $"http://{IPAddress}:{Ports.Roles}/api/Roles"; }

        // messages service
        public static string GetChatMessagesByChatIdGET { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages"; }
        public static string SendMessagePOST { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages"; }
        public static string GetMessageByIdGET { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages/get-by-id"; }
        public static string DeleteMessagePOST { get => $"http://{IPAddress}:{Ports.Messages}/api/Messages"; }

        // notification service
        public static string NotificationsWS { get => $"ws://{IPAddress}:{Ports.Notification}/api/Notification/connect"; }

        // Profile uploads
        public static string ProfileServer { get => $"http://{IPAddress}:{Ports.AccountManagement}"; }
        public static string ChatsServer { get => $"http://{IPAddress}:{Ports.Chats}"; }


    }
}
