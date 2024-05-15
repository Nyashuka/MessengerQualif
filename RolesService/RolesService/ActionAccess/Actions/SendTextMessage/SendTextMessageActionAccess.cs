using RolesService.ActionAccess;

public class SendTextMessageActionAccess : IActionAccess
{
    public Task<bool> HasAccess(int chatId, int userId)
    {
        

        throw new NotImplementedException();
    }
}