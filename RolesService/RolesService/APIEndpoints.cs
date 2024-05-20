namespace RolesService
{
    public static class APIEndpoints
    {
        // create group
        public const string DatabaseService = "http://127.0.0.1:5291";
        public const string CreateGroupPOST = $"{DatabaseService}/api/Groups";
        public const string GetAllGroupsByUserIdGET = $"{DatabaseService}/api/Groups";

        public const string GetGroupByIdGET = $"{DatabaseService}/api/Groups/group-by-id";

        // chat members
        public const string CreateRolePOST = $"{DatabaseService}/api/Roles";
        public const string GetAllGroupRolesGET = $"{DatabaseService}/api/Roles";
        public const string UpdateRolePATCH = $"{DatabaseService}/api/Roles";

        // permisions
        public const string GetAllPermissionsGET = $"{DatabaseService}/api/Permissions";

        // roles
        public const string AssignRolePOST = $"{DatabaseService}/api/UserRoleRelations";
        public const string UnAssignRoleDELETE = $"{DatabaseService}/api/UserRoleRelations";
        public const string GetAllRoleAssignersGET = $"{DatabaseService}/api/UserRoleRelations";
        public const string GetAllUserPermissionsGET = $"{DatabaseService}/api/Roles/permissions";
        
        // Auth service
        public const string AuthorizationServiceIP = "http://127.0.0.1:5292";
        public const string IsUserAuthenticatedGET = $"{AuthorizationServiceIP}/api/Auth/is-user-authenticated";
    }
}