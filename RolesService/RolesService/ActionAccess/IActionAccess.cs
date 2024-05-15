namespace RolesService.ActionAccess
{
    public interface IActionAccess
    {
        Task<bool> HasAccess(int chatId, int userId);
    }
}