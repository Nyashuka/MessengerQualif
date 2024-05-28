using MessengerWithRoles.WPFClient.Data;
using MessengerWithRoles.WPFClient.Data.Requests;
using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.MVVM.ViewModels;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MessengerWithRoles.WPFClient.Services
{
    public class AccountService : IService
    {
        private readonly HttpClient _httpClient;
        public User User { get; private set; }
        public UserProfileViewModel UserProfile { get; private set; }
        public string Email { get; private set; }

        public AccountService(User user, string email)
        {
            _httpClient = new HttpClient();
            User = user;
            UserProfile = new UserProfileViewModel(User);
            Email = email;
        }

        public async Task<ServiceResponse<string>> UpdatePicture(string filePath)
        {
            var file = File.OpenRead(filePath);
            var fileContent = new StreamContent(file);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileContent, "file", Path.GetFileName(filePath));

                var authService = ServiceLocator.Instance.GetService<AuthService>();

                var response = await client.PostAsync($"{APIEndpoints.UpdateProfilePicturePOST}?accessToken={authService.AccessToken}", formData);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<string>>();

                    UserProfile.AvatarURL = $"{APIEndpoints.ProfileServer}/{responseData.Data}?timestamp={DateTime.Now.Ticks}";

                    return new ServiceResponse<string>() { Data = UserProfile.AvatarURL };
                    
                }
                else
                {
                    return new ServiceResponse<string>() { Data = "" };
                }

            }
        }

        public async Task<ServiceResponse<User>> UpdateProfileInfo(User user)
        {
            var authService = ServiceLocator.Instance.GetService<AuthService>();
            var response = await _httpClient
                .PostAsJsonAsync($"{APIEndpoints.UpdateProfileInfoPOST}?accessToken={authService.AccessToken}", user);

            var data = await response.Content.ReadFromJsonAsync<ServiceResponse<User>>();

            User = data.Data;
            UserProfile.Update(User);

            return data;
        }
    }
}
