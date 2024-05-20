namespace RolesService.ActionAccess.Actions
{
    public class GetChatMembersChatAction : IChatAction
    {
        public Task<bool> HasAccess(HttpClient httpClient)
        {
            return Task.FromResult(true);
        }
    }
}