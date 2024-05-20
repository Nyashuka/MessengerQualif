
using ChatsService.ActionAccess.Data;
using ChatsService.Models;

namespace ChatsService.ActionAccess.Actions
{
    public class AddChatMemberChatAction : IChatAction
    {
        private readonly AddChatMemberChatActionData _data;

        public AddChatMemberChatAction(AddChatMemberChatActionData data)
        {
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync(APIEndpoints.CanAddMemberPOST, _data);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return responseData.Data;
        }
    }
}
