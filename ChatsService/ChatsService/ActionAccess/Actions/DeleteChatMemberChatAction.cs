
using ChatsService.ActionAccess.Data;
using ChatsService.Models;

namespace ChatsService.ActionAccess.Actions
{
    public class DeleteChatMemberChatAction : IChatAction
    {
        private readonly DeleteChatMemberChatActionData _data;

        public DeleteChatMemberChatAction(DeleteChatMemberChatActionData data)
        {
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync(APIEndpoints.CanDeleteMemberPOST, _data);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>(); 

            return responseData.Data;
        }
    }
}
