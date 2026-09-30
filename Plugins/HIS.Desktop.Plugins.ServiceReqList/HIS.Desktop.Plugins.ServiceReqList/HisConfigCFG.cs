using System;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class HisConfigCFG
	{
		private const string CONFIG_KEY__OLD_SYSTEM_INTEGRATION_TYPE = "MOS.OLD_SYSTEM.INTEGRATION_TYPE";

		private const string CONFIG_KEY__LIS_VERSION = "MOS.LIS.INTEGRATION_VERSION";

		private const string CONFIG_KEY__LIS_OPTION = "MOS.LIS.INTEGRATE_OPTION";

		private const string CONFIG_KEY__LIS_TYPE = "MOS.LIS.INTEGRATION_TYPE";

		private const string CONFIG_KEY__IsUseGetDynamic = "HIS.Desktop.Plugins.IsUseGetDynamic";

		internal const string CONFIG_KEY__OptionMergePrintIsmerge = "HIS.Desktop.Plugins.OptionMergePrint.Ismerge";

		private const string CONFIG_KEY__ConnectDrugInterventionInfo = "HIS.Desktop.Plugins.AssignPrescription.ConnectDrugInterventionInfo";

		private const string CONFIG_KEY__ShowPresAmount = "HIS.Desktop.Plugins.AssignPrescriptionPK.ShowPresAmount";

		private const string CONFIG_KEY__AutoDeleteEmrDocumentWhenEditReq = "HIS.Desktop.Plugins.ServiceReqList.AutoDeleteEmrDocumentWhenEditReq";

		internal const string CONFIG_KEY__ShowResultWhenReqComplete = "HIS.Desktop.Plugins.ContentSubclinical.ShowResultWhenReqComplete";

		internal static bool IsShowPresAmount;

		internal static bool IsOldSystemIntegration;

		internal static bool IsUseInventecLis;

		internal static bool IsUseGetDynamic;

		internal static bool IsmergeOptionMergePrint;

		internal static bool ConnectDrugInterventionInfo;

		internal static string AutoDeleteEmrDocumentWhenEditReq;

		internal static string ShowResultWhenReqComplete;

		internal static void LoadConfig()
		{
			try
			{
				IsmergeOptionMergePrint = GetValue("HIS.Desktop.Plugins.OptionMergePrint.Ismerge") == "1";
				string value = GetValue("MOS.OLD_SYSTEM.INTEGRATION_TYPE");
				IsUseGetDynamic = GetValue("HIS.Desktop.Plugins.IsUseGetDynamic") == "1";
				IsOldSystemIntegration = !string.IsNullOrWhiteSpace(value) && value != "0";
				IsUseInventecLis = CheckUserInventecLis();
				ConnectDrugInterventionInfo = GetValue("HIS.Desktop.Plugins.AssignPrescription.ConnectDrugInterventionInfo") == "1";
				IsShowPresAmount = GetValue("HIS.Desktop.Plugins.AssignPrescriptionPK.ShowPresAmount") == "1";
				AutoDeleteEmrDocumentWhenEditReq = GetValue("HIS.Desktop.Plugins.ServiceReqList.AutoDeleteEmrDocumentWhenEditReq");
				ShowResultWhenReqComplete = GetValue("HIS.Desktop.Plugins.ContentSubclinical.ShowResultWhenReqComplete");
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

		private static bool CheckUserInventecLis()
		{
			try
			{
				string text = HisConfigs.Get<string>("MOS.LIS.INTEGRATION_VERSION");
				string text2 = HisConfigs.Get<string>("MOS.LIS.INTEGRATE_OPTION");
				string text3 = HisConfigs.Get<string>("MOS.LIS.INTEGRATION_TYPE");
				if (text == "1" && text2 == "1")
				{
					return true;
				}
				if (text == "2" && text3 == "1")
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return false;
		}
	}
}
