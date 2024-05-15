using ChatsService.ActionAccess.Actions;

namespace ChatsService.ActionAccess.Services
{
    public interface IActionAccessService
    {
        Task<bool> HasAccess<T>(T actionAccess, int chatId, int userId) where T : class, IActionAccess;
        Task<bool> IsOwner(int chatId, int userId);
    }
}