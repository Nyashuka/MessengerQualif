using MessagesService.ActionAccess.Actions;

namespace MessagesService.ActionAccess.Services
{
    public interface IActionAccessService
    {
        Task<bool> HasAccess<T>(T actionAccess, int chatId, int userId) where T : class, IChatAction;
        Task<bool> IsOwner(int chatId, int userId);
    }
}