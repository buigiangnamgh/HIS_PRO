using System;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace His.UC.UCHein.Config
{
	internal class HisConfigCFG
	{
		private const string CONFIG_KEY__WARNINGHEINPATIENTTYPECODE = "HIS.Desktop.Plugins.RegisterV2.WarningHeinPatientTypeCode";

		private const string CONFIG_KEY__HIS_PATIENT_TYPE_PATIENT_TYPE_CODE_BHYT = "HIS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		private const string CONFIG_KEY__IsAllowedRouteTypeByDefault = "HIS.Desktop.Plugins.IsAllowedRouteTypeByDefault";

		internal static string IsAllowedRouteTypeByDefault;

		internal const string CONFIG_KEY__PATIENT_TYPE_CODE__BHYT = "HIS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		private const string CONFIG_KEY__NotDisplayedRouteTypeOver = "HIS.Desktop.Plugins.Register.NotDisplayedRouteTypeOver";

		internal static string NotDisplayedRouteTypeOver;

		private const string CONFIG_KEY__IsNotAutoCheck5Y6M = "MOS.HIS_PATIENT_TYPE_ALTER.NOT_AUTO_CHECK_5_YEAR_6_MONTH";

		public static bool IsNotAutoCheck5Y6M;

		internal static string WarningHeinPatientTypeCode;

		internal static string PatientTypeCodeBHYT;

		public static long PatientTypeId__BHYT;

		public static string PatientTypeCode__BHYT;

		internal static void LoadConfig()
		{
			try
			{
				LogSystem.Debug("LoadConfig => 1");
				IsAllowedRouteTypeByDefault = GetValue("HIS.Desktop.Plugins.IsAllowedRouteTypeByDefault");
				NotDisplayedRouteTypeOver = GetValue("HIS.Desktop.Plugins.Register.NotDisplayedRouteTypeOver");
				WarningHeinPatientTypeCode = GetValue("HIS.Desktop.Plugins.RegisterV2.WarningHeinPatientTypeCode");
				PatientTypeCode__BHYT = GetValue("HIS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT");
				PatientTypeId__BHYT = GetPatientTypeByCode(PatientTypeCode__BHYT).ID;
				IsNotAutoCheck5Y6M = GetValue("MOS.HIS_PATIENT_TYPE_ALTER.NOT_AUTO_CHECK_5_YEAR_6_MONTH") == "1";
				LogSystem.Debug("LoadConfig => 2");
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
				return null;
			}
		}

		private static HIS_PATIENT_TYPE GetPatientTypeByCode(string code)
		{
			HIS_PATIENT_TYPE hIS_PATIENT_TYPE = new HIS_PATIENT_TYPE();
			try
			{
				hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE.ToLower() == code.ToLower().Trim());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return hIS_PATIENT_TYPE ?? new HIS_PATIENT_TYPE();
		}
	}
}
