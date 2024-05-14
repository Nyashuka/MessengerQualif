﻿namespace ChatsService
{
    public static class APIEndpoints
    {
        // create group
        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string CreateGroupPOST = $"{DatabaseService}/api/Groups";
        public const string GetAllGroupsByUserIdGET = $"{DatabaseService}/api/Groups";
        // chat members
        public const string AddMemberToChatPOST = $"{DatabaseService}/api/ChatMembers";
        public const string DeleteMemberDELETE = $"{DatabaseService}/api/ChatMembers";
        public const string GetChatMembersGET = $"{DatabaseService}/api/ChatMembers";

        // Auth service
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";


    }
}
