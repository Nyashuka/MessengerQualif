using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;

namespace MessengerWithRoles.WPFClient.Services
{
    public class RequestService : IService
    {
        private readonly HttpClient _httpClient;

        public RequestService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ServiceResponse<TReturnType>> PostAsJsonAsync<TReturnType, TPostData>(string endpoint, TPostData postData)
        {
            HttpResponseMessage? response = null;
            try
            {
                response = await _httpClient.PostAsJsonAsync(APIEndpoints.LoginPOST, postData);
            }
            catch (Exception e)
            {
                return new ServiceResponse<TReturnType>($"Request by: \n{endpoint}\n error{e.Message}");
            }

            if (response == null)
            {
                return new ServiceResponse<TReturnType>($"Request by: \n{endpoint}\n can't get response.");
            }

            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponse<TReturnType>($"Request by: \n{endpoint}\n is not success.\nStatus code: {response.StatusCode}\nReasonPhrase: {response.ReasonPhrase}");
            }

            var dataFromResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<TReturnType>>();

            if (dataFromResponse == null)
            {
                return new ServiceResponse<TReturnType>($"Cannot parse data from \n{endpoint}\n response!");
            }

            if (!dataFromResponse.Success)
            {
                return new ServiceResponse<TReturnType>(dataFromResponse.Message);
            }

            if (dataFromResponse.Data == null)
            {
                return new ServiceResponse<TReturnType>("Can't get data");
            }

            return new ServiceResponse<TReturnType>(dataFromResponse.Data, true, dataFromResponse.Message);
        }
    }
}
