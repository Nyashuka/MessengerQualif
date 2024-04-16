namespace NotificationService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> TryGetAuthenticatedUser(string accessToken);
    }
}
