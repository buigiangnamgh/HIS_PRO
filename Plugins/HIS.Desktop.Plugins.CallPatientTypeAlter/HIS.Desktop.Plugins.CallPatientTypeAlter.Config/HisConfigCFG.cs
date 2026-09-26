using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using Inventec.Common.TypeConvert;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.Config
{
	internal class HisConfigCFG
	{
		private const string CONFIG_KEY__IS_AUTO_FILL_DATA_RECENT_SERVICE_ROOM = "HIS.Desktop.Plugins.Register.IsAutoFillDataRecentServiceRoom";

		private const string CONFIG_KEY__IS_CHECK_HEIN_CARD = "HIS.Desktop.Plugins.Register.IsCheckHeinCard";

		private const string CONFIG_KEY__IS_CHECK_PREVIOUS_DEBT = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_DEBT";

		private const string CONFIG_KEY__IS_CHECK_PREVIOUS_PRESCRIPTION = "MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_PRESCRIPTION";

		private const string CONFIG_KEY__IS_DEFAULT_RIGHT_ROUTE_TYPE = "HIS.Desktop.Plugins.Register.IsDefaultRightRouteType";

		private const string CONFIG_KEY__ICD_GENERATE = "HIS.Desktop.Plugins.AutoCheckIcd";

		private const string CONFIG_KEY__PATIENT_TYPE_CODE__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		private const string CONFIG_KEY__PATIENT_TYPE_CODE__QN = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.QN";

		private const string CONFIG_KEY__PATIENT_TYPE_CODE__KSK = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSK";

		private const string CONFIG_KEY__MOS__HIS_PATIENT__MUST_HAVE_NCS_INFO_FOR_CHILD = "MOS.HIS_PATIENT.MUST_HAVE_NCS_INFO_FOR_CHILD";

		private const string CONFIG_KEY__GENDER_CODE_BASE = "RAE.HIS_GENDER_CODE__BASE";

		private const string CONFIG_KEY__CAREER_CODE__UNDER_6_AGE = "EXE.HIS_CAREER_CODE__UNDER_6_AGE";

		private const string CONFIG_KEY__CAREER_CODE__HOC_SINH = "HIS.DESKTOP.REGISTER.HIS_CAREER.CARRER_CODE_HS";

		private const string HIS_DESKTOP_REGISER__EXECUTE_ROOM_SHOW = "HIS.HIS_DESKTOP_REGISTER.EXECUTE_ROOM_CODE.SHOW";

		private const string CONFIG_KEY__NOT_CHECK_EXPIRED_IS_SHOW = "HIS.DESKTOP.REGISTER.HEIN_CARD.NOT_CHECK_EXPIRED.IS_SHOW";

		private const string Key__WarningOverCeiling__Exam__Out__In = "HIS.Desktop.Plugins.WarningOverCeiling.Exam__Out__In";

		private const string HIS_UC_UCHein_IsTempQN = "HIS.UC.UCHein.IsTempQN";

		private const string HIS_UC_UCHein_IS_OBLIGATORY_TRANFER_MEDI_ORG = "HIS.UC.UCHein.IS_OBLIGATORY_TRANFER_MEDI_ORG";

		private const string CONFIG_KEY__IS_CHECK_EXAM_HISTORY_TODAY = "HIS.Desktop.Plugins.Register.IS_CHECK_EXAM_HISTORY_TODAY";

		private const string CONFIG_KEY__HIS_DESKTOP__PLUGINS_AUTO_CHECK_HEIN_DATE_TO = "CONFIG_KEY__HIS_DESKTOP__PLUGINS_AUTO_CHECK_HEIN_DATE_TO";

		private const string CONFIG_KEY_MOS_SET_PRIMARY_PATIENT_TYPE = "MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE";

		private const string CONFIG_KEY_IsNotRequiredRightTypeInCaseOfHavingAreaCode = "HIS.Desktop.Plugins.Register.IsNotRequiredRightTypeInCaseOfHavingAreaCode";

		private const string IS_BLOCK_INVALID_BHYT = "HIS.Desktop.Plugins.IsBlockingInvalidBhyt";

		private const string valueString__true = "1";

		private const int valueInt__true = 1;

		internal static bool IsCheckExamHistory;

		internal static string IsShowCheckExpired;

		internal static bool IsCheckHeinCard;

		internal static bool IsCheckPreviousDebt;

		internal static bool IsCheckPreviousPrescription;

		internal static string IsDefaultRightRouteType;

		internal static string AutoCheckIcd;

		internal static string PatientTypeCode__BHYT;

		internal static string PatientTypeCode__KSK;

		internal static string PatientTypeCode__QN;

		internal static long PatientTypeId__BHYT;

		internal static long PatientTypeId__KSK;

		internal static long PatientTypeId__QN;

		internal static string CheckTempQN;

		internal static bool IsObligatoryTranferMediOrg;

		internal static string ObligatoryTranferMediOrg;

		internal static string IsSetPrimaryPatientType;

		public static bool IsBlockingInvalidBhyt;

		internal static bool MustHaveNCSInfoForChild;

		public static bool IsNotRequiredRightTypeInCaseOfHavingAreaCode;

		internal static HIS_CAREER CareerHS;

		internal static HIS_CAREER CareerUnder6Age;

		public static long CheDoTuDongCheckThongTinTheBHYT { get; set; }

		public static decimal WarningOverCeiling__Exam { get; set; }

		public static decimal WarningOverCeiling__Out { get; set; }

		public static decimal WarningOverCeiling__In { get; set; }

		internal static void LoadConfig()
		{
			try
			{
				LogSystem.Debug("LoadConfig => 1");
				IsNotRequiredRightTypeInCaseOfHavingAreaCode = GetValue("HIS.Desktop.Plugins.Register.IsNotRequiredRightTypeInCaseOfHavingAreaCode") == "1";
				MustHaveNCSInfoForChild = GetValue("MOS.HIS_PATIENT.MUST_HAVE_NCS_INFO_FOR_CHILD") == "1";
				PatientTypeCode__BHYT = GetValue("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT");
				PatientTypeId__BHYT = GetPatientTypeByCode(PatientTypeCode__BHYT).ID;
				PatientTypeCode__KSK = GetValue("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSK");
				PatientTypeId__KSK = GetPatientTypeByCode(PatientTypeCode__KSK).ID;
				AutoCheckIcd = GetValue("HIS.Desktop.Plugins.AutoCheckIcd");
				IsShowCheckExpired = GetValue("HIS.DESKTOP.REGISTER.HEIN_CARD.NOT_CHECK_EXPIRED.IS_SHOW");
				IsCheckHeinCard = GetValue("HIS.Desktop.Plugins.Register.IsCheckHeinCard") == "1";
				IsCheckPreviousDebt = GetValue("MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_DEBT") == "1";
				IsCheckPreviousPrescription = GetValue("MOS.HIS_TREATMENT.IS_CHECK_PREVIOUS_PRESCRIPTION") == "1";
				CheckTempQN = GetValue("HIS.UC.UCHein.IsTempQN");
				ObligatoryTranferMediOrg = GetValue("HIS.UC.UCHein.IS_OBLIGATORY_TRANFER_MEDI_ORG");
				IsObligatoryTranferMediOrg = ObligatoryTranferMediOrg == "1";
				PatientTypeCode__QN = GetValue("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.QN");
				PatientTypeId__QN = GetPatientTypeByCode(PatientTypeCode__QN).ID;
				IsDefaultRightRouteType = GetValue("HIS.Desktop.Plugins.Register.IsDefaultRightRouteType");
				CareerHS = GetCareerByCode(GetValue("HIS.DESKTOP.REGISTER.HIS_CAREER.CARRER_CODE_HS"));
				CareerUnder6Age = GetCareerByCode(GetValue("EXE.HIS_CAREER_CODE__UNDER_6_AGE"));
				IsSetPrimaryPatientType = GetValue("MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE");
				IsCheckExamHistory = GetValue("HIS.Desktop.Plugins.Register.IS_CHECK_EXAM_HISTORY_TODAY") == "1";
				IsBlockingInvalidBhyt = GetValue("HIS.Desktop.Plugins.IsBlockingInvalidBhyt") == "1";
				try
				{
					CheDoTuDongCheckThongTinTheBHYT = ConfigApplicationWorker.Get<long>("CONFIG_KEY__HIS_DESKTOP__PLUGINS_AUTO_CHECK_HEIN_DATE_TO");
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
				}
				LogSystem.Debug("LoadConfig => 2");
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		public static void InitWarningOverCeiling()
		{
			try
			{
				string value = GetValue("HIS.Desktop.Plugins.WarningOverCeiling.Exam__Out__In");
				if (!string.IsNullOrEmpty(value))
				{
					string[] array = value.Split(new string[1] { "|" }, StringSplitOptions.RemoveEmptyEntries);
					if (array != null && array.Length == 3)
					{
						WarningOverCeiling__Exam = Parse.ToDecimal(array[0]);
						WarningOverCeiling__Out = Parse.ToDecimal(array[1]);
						WarningOverCeiling__In = Parse.ToDecimal(array[2]);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private static HIS_CAREER GetCareerByCode(string code)
		{
			HIS_CAREER hIS_CAREER = new HIS_CAREER();
			try
			{
				hIS_CAREER = BackendDataWorker.Get<HIS_CAREER>().FirstOrDefault((HIS_CAREER o) => o.CAREER_CODE == code);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return hIS_CAREER ?? new HIS_CAREER();
		}

		private static HIS_GENDER GetGenderByCode(string code)
		{
			HIS_GENDER hIS_GENDER = new HIS_GENDER();
			try
			{
				hIS_GENDER = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.GENDER_CODE == code);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return hIS_GENDER ?? new HIS_GENDER();
		}

		private static HIS_PATIENT_TYPE GetPatientTypeByCode(string code)
		{
			HIS_PATIENT_TYPE hIS_PATIENT_TYPE = new HIS_PATIENT_TYPE();
			try
			{
				hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE == code);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return hIS_PATIENT_TYPE ?? new HIS_PATIENT_TYPE();
		}

		private static string GetValue(string key)
		{
			try
			{
				return HisConfigs.Get<string>(key);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return "";
		}

		private static List<string> GetListValue(string key)
		{
			try
			{
				return HisConfigs.Get<List<string>>(key);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return null;
		}
	}
}
