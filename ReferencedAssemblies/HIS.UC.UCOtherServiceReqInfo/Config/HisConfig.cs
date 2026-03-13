using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.UC.UCOtherServiceReqInfo.Config
{
	public class HisConfig
	{
		private static string IsRequiredPriorityTypeInCaseOfCheckingPriorityString = "HIS.Desktop.Plugins.Register.IsRequiredPriorityTypeInCaseOfCheckingPriority";

		private static string KEY__IS_MANUAL_IN_CODE = "MOS.HIS_TREATMENT.IS_MANUAL_IN_CODE";

		private static string KEY__IsRequestSkinCare = "HIS.Desktop.Plugins.RegisterV2.RequestSkinCare";

		internal static bool IsRequiredPriorityTypeInCaseOfCheckingPriority { get; set; }

		internal static bool IsManualInCode { get; set; }

		internal static bool IsNotCareerRequired { get; set; }

		internal static string RequestSkinCare { get; set; }

		internal static void LoadConfig()
		{
			try
			{
				IsRequiredPriorityTypeInCaseOfCheckingPriority = HisConfigs.Get<string>(IsRequiredPriorityTypeInCaseOfCheckingPriorityString) == "1";
				IsManualInCode = HisConfigs.Get<string>(KEY__IS_MANUAL_IN_CODE) == "1";
				RequestSkinCare = HisConfigs.Get<string>(KEY__IsRequestSkinCare);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
