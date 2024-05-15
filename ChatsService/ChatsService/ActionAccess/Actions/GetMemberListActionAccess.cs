namespace ChatsService.ActionAccess.Actions
{
    public class GetMemberListActionAccess : IActionAccess
    {
        public Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
