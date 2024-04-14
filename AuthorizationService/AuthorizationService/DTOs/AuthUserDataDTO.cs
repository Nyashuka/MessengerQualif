using AuthorizationService.Models;

namespace AuthorizationService.DTOs
{
    public class AuthUserDataDTO
    {
        public bool HasAccess { get; set; }
        public UserDataByAccessTokenDTO Data { get; set; }
    }
}
