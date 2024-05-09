using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http.Json;
using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.Services.EventBusModule;
using MessengerWithRoles.WPFClient.Services.EventBusModule.EventBusArguments;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using MessengerWithRoles.WPFClient.Data.Requests;

namespace MessengerWithRoles.WPFClient.Services
{
    public class AuthService : IService
    {
        public bool IsAuthenticated { get; private set; }
        public string AccessToken { get; private set; }
        private RequestService _requestService;

        public User User { get; private set; }

        public AuthService()
        {
            IsAuthenticated = false;
            AccessToken = string.Empty;
            _requestService = ServiceLocator.Instance.GetService<RequestService>();
        }

        public async Task<bool> Login(string email, string password)
        {
            HttpClient httpClient = new HttpClient();

            var loginData = new AccountLogin();
            loginData.Email = email;
            loginData.Password = password;


            var loginResponse = await _requestService
                                         .PostAsJsonAsync<string, AccountLogin>($"{APIEndpoints.LoginPOST}?accessToken={AccessToken}", loginData);

            if(!loginResponse.Success || loginResponse.Data == null)
            {
                MessageBox.Show(loginResponse.Message);
                return false; 
            }    

            IsAuthenticated = true;
            AccessToken = loginResponse.Data;

            var userDataRequest = await httpClient.GetAsync($"{APIEndpoints.GetUserGET}?accessToken={AccessToken}");
            User = (await userDataRequest.Content.ReadFromJsonAsync<ServiceResponse<User>>()).Data;

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();
            eventBus.Raise(EventBusDefinitions.LoginedInAccount, new EventBusArgs());

            return IsAuthenticated;
        }

        public async Task<bool> Register(CreationAccount accountData)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage? response;

            try
            {
                response = await client.PostAsJsonAsync(APIEndpoints.CreateAccountPOST, accountData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show(response.ReasonPhrase);
                return false;
            }

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            if (data == null)
            {
                MessageBox.Show("Response is success, but data cannot be parsed!");
                return false;
            }

            if (data.Success)
                MessageBox.Show($"Account created successfully with id={data.Data}");

            IsAuthenticated = await Login(accountData.Email, accountData.Password);

            return IsAuthenticated;
        }
    }
}
