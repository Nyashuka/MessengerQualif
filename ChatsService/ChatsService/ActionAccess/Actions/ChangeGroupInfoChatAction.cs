
using ChatsService.ActionAccess.Data;
using ChatsService.Models;

namespace ChatsService.ActionAccess.Actions
{
    public class ChangeGroupInfoChatAction : IChatAction
    {
        private readonly ChangeGroupInfoChatActionData _data;

        public ChangeGroupInfoChatAction(ChangeGroupInfoChatActionData data)
        {
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync(APIEndpoints.CanChangeChatInfoPOST, _data);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return responseData.Data;
        }
    }
}
