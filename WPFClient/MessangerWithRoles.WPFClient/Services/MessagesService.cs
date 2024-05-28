using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.DTOs;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services
{
    public class MessagesService : IService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public MessagesService()
        {
            _httpClient = new HttpClient();
            _authService = ServiceLocator.Instance.GetService<AuthService>();

        }

        public async Task<ServiceResponse<MessageDto>> SendMessage(MessageDto messageDto)
        {
            var notificationService = ServiceLocator.Instance.GetService<NotificationService>();
            var response = await _httpClient.PostAsJsonAsync($"{APIEndpoints.SendMessagePOST}?accessToken={_authService.AccessToken}&clientGuid={notificationService.ClientGuid}", messageDto);
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<MessageDto>>();

            return responseData;
        }

        public async Task<ServiceResponse<bool>> DeleteMessage(int messageId)
        {
            var notificationService = ServiceLocator.Instance.GetService<NotificationService>();
            var response = await _httpClient.DeleteAsync($"{APIEndpoints.SendMessagePOST}?accessToken={_authService.AccessToken}&clientGuid={notificationService.ClientGuid}&messageId={messageId}");
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();

            return responseData;
        }
    }
}
