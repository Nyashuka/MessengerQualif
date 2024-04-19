namespace MessagesService.Services.Interfaces
{
    public interface IChatService
    {
        Task<bool> IsUserChatMember(int userId);
    }
}
