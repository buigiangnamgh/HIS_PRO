using Inventec.Common.LocalStorage.SdaConfig;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class CheckPreviousDebtCFG
	{
		private const string CONFIG_KEY = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_DEBT";

		private const string DEFAULT__IS_CHECK_PREVIOUS_DEBT = "1";

		private static bool? isCheckPreviousDebt;

		public static bool IsCheckPreviousDebt
		{
			get
			{
				if (!isCheckPreviousDebt.HasValue)
				{
					isCheckPreviousDebt = GetIsCheck(SdaConfigs.Get<string>("MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_DEBT"));
				}
				return isCheckPreviousDebt.Value;
			}
		}

		private static bool GetIsCheck(string value)
		{
			return value == "1";
		}
	}
}
