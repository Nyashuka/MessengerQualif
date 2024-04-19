namespace MessagesService.DTOs.Auth
{
    public class AuthUserDataDTO
    {
        public bool HasAccess { get; set; }
        public UserDataByAccessTokenDTO Data { get; set; }
    }
}