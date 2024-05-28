using MessagesService.ActionAccess.Data;
using MessagesService.Models.Requests;

namespace MessagesService.ActionAccess.Actions
{
    public class DeleteMessageChatAction : IChatAction
    {
        private readonly DeleteMessageChatActionData _data;

        public DeleteMessageChatAction(DeleteMessageChatActionData data)
        {
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync(APIEndpoints.CandDeleteMessages, _data);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return responseData.Data;
        }
    }
}
