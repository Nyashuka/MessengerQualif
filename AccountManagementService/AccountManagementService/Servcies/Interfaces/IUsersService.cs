using AccountManagementService.Models;

namespace AccountManagementService.Servcies.Interfaces
{
    public interface IUsersService
    {
        Task<ServiceResponse<List<User>>> GetOtherUsersForUser(int userId);

    }
}
