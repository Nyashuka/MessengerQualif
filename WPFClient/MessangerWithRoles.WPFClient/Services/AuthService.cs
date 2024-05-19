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

        public User User { get; private set; }

        public AuthService()
        {
            IsAuthenticated = false;
            AccessToken = string.Empty;
        }

        public async Task<bool> Login(string email, string password)
        {
            HttpClient httpClient = new HttpClient();

            var loginData = new AccountLogin();
            loginData.Email = email;
            loginData.Password = password;

            HttpResponseMessage? response = null;
            try
            {
                response = await httpClient.PostAsJsonAsync(APIEndpoints.LoginPOST, loginData);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return IsAuthenticated;
            }

            if (response == null)
            {
                MessageBox.Show("Login Response in empty");
                return IsAuthenticated;
            }

            var dataFromResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

            if (dataFromResponse == null)
            {
                MessageBox.Show("Cannot parse data from login response!");
                return IsAuthenticated;
            }

            if (!dataFromResponse.Success)
            {
                MessageBox.Show(dataFromResponse.Message);
                return IsAuthenticated;
            }

            if(dataFromResponse.Data == null)
            {
                MessageBox.Show("Can't get Token, but login request successfully");
                return IsAuthenticated;
            }

            IsAuthenticated = true;
            AccessToken = dataFromResponse.Data;

            var userDataRequest = await httpClient.GetAsync($"{APIEndpoints.GetUserGET}?accessToken={AccessToken}");
            User = (await userDataRequest.Content.ReadFromJsonAsync<ServiceResponse<User>>()).Data;   

            EventBus eventBus = ServiceLocator.Instance.GetService<EventBus>();
            eventBus.Raise(EventBusDefinitions.LoginedInAccount, new EventBusArgs());

            InitServices(email);

            return IsAuthenticated;
        }

        private void InitServices(string email)
        {
            NotificationService notificationService = new NotificationService();
            notificationService.Start(AccessToken);
            ServiceLocator.Instance.RegisterService(notificationService);

            ServiceLocator.Instance.RegisterService(new AccountService(User, email));
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

        public async Task Logout()
        {
            User = null;
            AccessToken = string.Empty;
            IsAuthenticated = false;

            await ServiceLocator.Instance.GetService<NotificationService>().StopReceiving();
            ServiceLocator.Instance.UnregisterService<NotificationService>();

            ServiceLocator.Instance.UnregisterService<AccountService>();
        }
    }
}
