using MessangerWithRoles.WPFClient.MVVM.Models;
using MessangerWithRoles.WPFClient.Services.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerWithRoles.WPFClient.Services
{
    public class AuthService : IService
    {
        private readonly string _accesToken;
        public string AccesToken => _accesToken;

        public AuthService(string accessToken) 
        {
            _accesToken = accessToken;   
        }
    }
}
