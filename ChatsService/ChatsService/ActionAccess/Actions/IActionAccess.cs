namespace ChatsService.ActionAccess.Actions
{
    public interface IActionAccess
    {
        Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId);
    }
}
