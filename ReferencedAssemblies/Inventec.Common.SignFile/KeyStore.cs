using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignFile
{
    internal class KeyStore
    {
        internal const string SERIALNUMBER_KEY = "SERIALNUMBER";
        internal const string CHANGE_USB_KEY = "CHANGE_USB";
        internal static RegistryKey appFolder = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("IVTCPSSTORE").CreateSubKey("KEYSTORAGE");

        internal static bool Check(string key)
        {
            string value = (string)appFolder.GetValue(key, "");
            return (!String.IsNullOrEmpty(value));
        }

        internal static bool Clear()
        {
            appFolder.DeleteValue(SERIALNUMBER_KEY, false);
            appFolder.DeleteValue(CHANGE_USB_KEY, false);
            return true;
        }

        internal static bool SetValue(string key, string value)
        {
            appFolder.SetValue(key, Base64Encode(value));

            return true;
        }

        public static string GetValue(string key)
        {
            string vl = (string)appFolder.GetValue(key, "");
            return (!String.IsNullOrEmpty(vl) ? Base64Decode(vl) : vl);
        }

        internal static string Base64Decode(string encodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(encodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        internal static string Base64Encode(string encodedData)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(encodedData));
        }
    }
}
