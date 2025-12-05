using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.DebateDiagnostic.Config
{
	internal class HisConfigCFG
	{
		private const string CONFIG_KEY__IS_USE_SIGN_EMR = "HIS.HIS.DESKTOP.IS_USE_SIGN_EMR";

		private const string CONFIG_KEY__DebateDiagnostic_IsDefaultTracking = "HIS.Desktop.Plugins.DebateDiagnostic.IsDefaultTracking";

		internal static bool IsUseSignEmr;

		internal static bool DebateDiagnostic_IsDefaultTracking;

		internal static void LoadConfig()
		{
			try
			{
				IsUseSignEmr = GetValue("HIS.HIS.DESKTOP.IS_USE_SIGN_EMR") == "1";
				DebateDiagnostic_IsDefaultTracking = GetValue("HIS.Desktop.Plugins.DebateDiagnostic.IsDefaultTracking") == "1";
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
