namespace RolesService.ActionAccess.Actions
{
    public class AddChatMemberChatAction : IChatAction
    {
        public Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId)
        {
            return Task.FromResult(false);
        }
    }
}