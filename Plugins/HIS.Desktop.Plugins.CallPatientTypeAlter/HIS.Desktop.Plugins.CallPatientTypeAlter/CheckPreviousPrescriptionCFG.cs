using Inventec.Common.LocalStorage.SdaConfig;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class CheckPreviousPrescriptionCFG
	{
		private const string CONFIG_KEY = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_PRESCRIPTION";

		private const string DEFAULT__IS_CHECK_PREVIOUS_PRESCRIPTION = "1";

		private static bool? isCheckPreviousPrescription;

		public static bool IsCheckPreviousPrescription
		{
			get
			{
				if (!isCheckPreviousPrescription.HasValue)
				{
					isCheckPreviousPrescription = GetIsCheck(SdaConfigs.Get<string>("MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_PRESCRIPTION"));
				}
				return isCheckPreviousPrescription.Value;
			}
		}

		private static bool GetIsCheck(string value)
		{
			return value == "1";
		}
	}
}
