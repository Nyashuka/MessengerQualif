﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessangerWithRoles.WPFClient.Data
{
    public static class APIEndpoints
    {

        public const string CreateAccountPOST = "http://127.0.0.1:5292/api/Auth/create-account";
        public const string LoginPOST = "http://127.0.0.1:5292/api/Auth/login";
    }
}