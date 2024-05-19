using MessengerWithRoles.WPFClient.MVVM.Models;
using MessengerWithRoles.WPFClient.Services.ServiceLocatorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Services
{
    public class AccountService : IService
    {
        private readonly HttpClient _httpClient;
        public User User { get; private set; }
        public string Email { get; private set; }

        public AccountService(User user, string email)
        {
            _httpClient = new HttpClient();
            User = user;
            Email = email;
        }
    }
}
