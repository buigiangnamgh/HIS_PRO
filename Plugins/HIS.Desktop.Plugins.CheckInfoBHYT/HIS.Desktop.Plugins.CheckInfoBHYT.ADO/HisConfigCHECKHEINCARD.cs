using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.CheckInfoBHYT.ADO
{
	internal class HisConfigCHECKHEINCARD
	{
		internal const string HIS_CHECK_HEIN_CARD_BHXH__API = "HIS.CHECK_HEIN_CARD.BHXH__API";

		internal static string CHECK_HEIN_CARD_BHXH__API;

		internal static void LoadConfig()
		{
			try
			{
				CHECK_HEIN_CARD_BHXH__API = GetValue("HIS.CHECK_HEIN_CARD.BHXH__API");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private static string GetValue(string code)
		{
			string text = null;
			try
			{
				return HisConfigs.Get<string>(code);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				text = null;
			}
			return text;
		}
	}
}
