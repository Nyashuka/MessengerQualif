﻿using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string endpoint, T obj)
        {
            return await _httpClient.PostAsJsonAsync(endpoint, obj);
        }
    }
}
