﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MessengerWithRoles.WPFClient.Common
{
    public static class Extensions
    {
        public static bool IsEqualTo(this SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        public static String ToPlainString(this System.Security.SecureString secureStr)
        {
            String plainStr = new System.Net.NetworkCredential(string.Empty,
                              secureStr).Password;
            return plainStr;
        }

        // convert a plain text string into a secure string
        public static System.Security.SecureString ToSecureString(this String plainStr)
        {
            var secStr = new System.Security.SecureString(); secStr.Clear();
            foreach (char c in plainStr.ToCharArray())
            {
                secStr.AppendChar(c);
            }
            return secStr;
        }
    }
}
