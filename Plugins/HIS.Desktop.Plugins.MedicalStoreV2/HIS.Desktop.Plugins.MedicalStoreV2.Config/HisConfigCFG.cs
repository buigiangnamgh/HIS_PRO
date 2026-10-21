using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Config
{
	internal class HisConfigCFG
	{
		private const string CONFIG_KEY__RED_WARNING_STORE_TIME = "HIS.Desktop.Plugins.MedicalStore.RED_WARNING_STORE_TIME";

		private const string CONFIG_KEY__BLUE_WARNING_STORE_TIME = "HIS.Desktop.Plugins.MedicalStore.BLUE_WARNING_STORE_TIME";

		private const string CONFIG_KEY__ORANGE_WARNING_STORE_TIME = "HIS.Desktop.Plugins.MedicalStore.ORANGE_WARNING_STORE_TIME";

		private const string CONFIG_KEY__IS_USING_STORE_CHECK = "MOS.HIS_TREATMENT.IS_USING_STORE_CHECK";

		internal static long? RedWarningStoreTime;

		internal static long? BlueWarningStoreTime;

		internal static long? OrangeWarningStoreTime;

		internal static bool IsUsingStoreCheck;

		internal static void LoadConfig()
		{
			try
			{
				string value = GetValue("HIS.Desktop.Plugins.MedicalStore.BLUE_WARNING_STORE_TIME");
				string value2 = GetValue("HIS.Desktop.Plugins.MedicalStore.RED_WARNING_STORE_TIME");
				string value3 = GetValue("HIS.Desktop.Plugins.MedicalStore.ORANGE_WARNING_STORE_TIME");
				IsUsingStoreCheck = GetValue("MOS.HIS_TREATMENT.IS_USING_STORE_CHECK") == "1";
				if (!string.IsNullOrWhiteSpace(value))
				{
					BlueWarningStoreTime = Convert.ToInt32(value ?? "");
				}
				else
				{
					BlueWarningStoreTime = null;
				}
				if (!string.IsNullOrWhiteSpace(value2))
				{
					RedWarningStoreTime = Convert.ToInt32(value2 ?? "");
				}
				else
				{
					RedWarningStoreTime = null;
				}
				if (!string.IsNullOrWhiteSpace(value3))
				{
					OrangeWarningStoreTime = Convert.ToInt32(value3 ?? "");
				}
				else
				{
					OrangeWarningStoreTime = null;
				}
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
				LogSystem.Warn(ex);
				text = null;
			}
			return text;
		}
	}
}
