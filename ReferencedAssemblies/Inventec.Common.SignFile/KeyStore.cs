using System;
using System.Text;
using Microsoft.Win32;

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
			return !string.IsNullOrEmpty(value);
		}

		internal static bool Clear()
		{
			appFolder.DeleteValue("SERIALNUMBER", false);
			appFolder.DeleteValue("CHANGE_USB", false);
			return true;
		}

		internal static bool SetValue(string key, string value)
		{
			appFolder.SetValue(key, Base64Encode(value));
			return true;
		}

		public static string GetValue(string key)
		{
			string text = (string)appFolder.GetValue(key, "");
			return (!string.IsNullOrEmpty(text)) ? Base64Decode(text) : text;
		}

		internal static string Base64Decode(string encodedData)
		{
			byte[] bytes = Convert.FromBase64String(encodedData);
			return Encoding.UTF8.GetString(bytes);
		}

		internal static string Base64Encode(string encodedData)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(encodedData));
		}
	}
}
