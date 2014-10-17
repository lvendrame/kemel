using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Tools.General.Text
{
    using Resources;

    public static class AppEncoding
    {
        static Encoding _innerEncoding = null;

        static AppEncoding()
        {
            _innerEncoding = Encoding.GetEncoding(GlobalResources.SystemEncoding);
        }

        public static string GetString(byte[] buffer)
        {
            return _innerEncoding.GetString(buffer);
        }

        public static byte[] GetBytes(string value)
        {
            return _innerEncoding.GetBytes(value);
        }

        public static string GetBase64String(string value)
        {
            return Convert.ToBase64String(AppEncoding.GetBytes(value));
        }

        public static string GetStringFromBase64(string base64)
        {
            return AppEncoding.GetString(Convert.FromBase64String(base64));
        }
    }
}
