using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.Config
{
	internal class BHXHLoginCFG
	{
		private const string CONFIG_KEY = "HIS.CHECK_HEIN_CARD.BHXH.LOGIN.USER_PASS";

		private const string CONFIG_KEY_ADDRESS = "HIS.CHECK_HEIN_CARD.BHXH__ADDRESS";

		public static string USERNAME;

		public static string PASSWORD;

		public static string ADDRESS;

		public static void LoadConfig()
		{
			try
			{
				USERNAME = Get(HisConfigs.Get<string>("HIS.CHECK_HEIN_CARD.BHXH.LOGIN.USER_PASS"), 0);
				PASSWORD = Get(HisConfigs.Get<string>("HIS.CHECK_HEIN_CARD.BHXH.LOGIN.USER_PASS"), 1);
				ADDRESS = HisConfigs.Get<string>("HIS.CHECK_HEIN_CARD.BHXH__ADDRESS").Trim();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private static string Get(string value, int index)
		{
			string result = "";
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					string[] array = value.Split(':');
					if (array != null && array.Length >= index)
					{
						result = array[index].Trim();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				result = "";
			}
			return result;
		}
	}
}
