using MessangerWithRoles.WPFClient.Services.ServiceLocator;
using System.Net.Http;
using System.Net.Http.Json;

namespace MessangerWithRoles.WPFClient.Services
{
    public class RequestService : IService
    {
        private readonly HttpClient _httpClient;

        public RequestService() 
        { 
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string endpoint, T obj)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, obj);
        }
    }
}
