namespace ChatsService.ActionAccess.Actions
{
    public class GetChatMembersActionAccess : IActionAccess
    {
        public Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}
