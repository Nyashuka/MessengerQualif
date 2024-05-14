namespace RolesService.Authorization
{
    public class AuthUserDataDTO
    {
        public bool HasAccess { get; set; }
        public UserDataByAccessTokenDTO? Data { get; set; }
    }
}