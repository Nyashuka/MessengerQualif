﻿namespace ChatsService
{
    public static class APIEndpoints
    {
        // DATABASE
        public const string DatabaseService = "http://127.0.0.1:5291";
        // [group chats]
        public const string CreateGroupPOST = $"{DatabaseService}/api/Groups";
        public const string GetAllGroupsByUserIdGET = $"{DatabaseService}/api/Groups";
        public const string GetGroupByIdGET = $"{DatabaseService}/api/Groups/group-by-id";
        public const string UpdateGroupInfoPATCH= $"{DatabaseService}/api/GroupInfo";
        public const string UpdateGroupPictureGET = $"{DatabaseService}/api/GroupInfo/picture";
        // [chats]
        public const string CreatePersonalChat_POST = $"{DatabaseService}/api/Chats/create-personal";
        public const string GetAllPersonalChats_GET = $"{DatabaseService}/api/Chats/get-personal";
        public const string GetPersonalChat_POST = $"{DatabaseService}/api/Chats/get-personal";
        public const string GetChatByIdGET = $"{DatabaseService}/api/Chats/get-chat-by-id";
        // [chat members]
        public const string AddMemberToChatPOST = $"{DatabaseService}/api/ChatMembers";
        public const string AddMemberByUsernameToChatPOST = $"{DatabaseService}/api/ChatMembers/username";
        public const string DeleteMemberDELETE = $"{DatabaseService}/api/ChatMembers";
        public const string GetChatMembersGET = $"{DatabaseService}/api/ChatMembers";

        // AUTHENTICATE
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";

        // CAN DO IT????
        public const string RolesService = "http://127.0.0.1:5296";
        public const string CanDeleteMemberPOST = $"{RolesService}/api/ActionAccess/can-delete-member";
        public const string CanAddMemberPOST = $"{RolesService}/api/ActionAccess/can-add-member";
        public const string CanChangeChatInfoPOST = $"{RolesService}/api/ActionAccess/can-change-chat-info";
    }
}
