using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {
        // auth service
        public const string CreateAccountPOST = "http://127.0.0.1:5292/api/Auth/create-account";
        public const string LoginPOST = "http://127.0.0.1:5292/api/Auth/login";

        // account management service
        public const string GetAllUsersGET = "http://127.0.0.1:5293/api/Users";
        public const string AddFriendPOST = "http://127.0.0.1:5293/api/Friends/add-friend";
        public const string RemoveFriendPOST = "http://127.0.0.1:5293/api/Friends/remove-friend";
        public const string GetAllFriendsPOST = "http://127.0.0.1:5293/api/Friends";


    }
}
