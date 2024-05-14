namespace RolesService.Authorization
{
    public interface IAuthService
    {
        Task<AuthUserDataDTO> TryGetAuthenticatedUser(string acessToken);
    }
}