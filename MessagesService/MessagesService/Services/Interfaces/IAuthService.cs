namespace MessagesService.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Return -1, if user not authorized
        /// </summary>
        Task<int> TryGetAuthenticatedUser(string accessToken);
    }
}