namespace RolesService.ActionAccess.Actions
{
    public class GetChatMembersChatAction : IChatAction
    {
        public Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}