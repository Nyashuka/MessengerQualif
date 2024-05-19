
namespace ChatsService.ActionAccess.Actions
{
    public class DeleteChatMemberChatAction : IChatAction
    {
        public Task<bool> HasAccess(HttpClient httpClient, int chatId, int userId)
        {
            return Task.FromResult(false);
        }
    }
}
