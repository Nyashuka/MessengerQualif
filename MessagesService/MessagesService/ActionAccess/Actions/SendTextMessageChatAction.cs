
using MessagesService.ActionAccess.Data;
using MessagesService.Models.Requests;

namespace MessagesService.ActionAccess.Actions
{
    public class SendTextMessageChatAction : IChatAction
    {
        private readonly SendMessageChatActionData _data;

        public SendTextMessageChatAction(SendMessageChatActionData data)
        {
            _data = data;
        }

        public async Task<bool> HasAccess(HttpClient httpClient)
        {
            var response = await httpClient.PostAsJsonAsync(APIEndpoints.CandSendTextMessage, _data);

            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return responseData.Data;
        }
    }
}
