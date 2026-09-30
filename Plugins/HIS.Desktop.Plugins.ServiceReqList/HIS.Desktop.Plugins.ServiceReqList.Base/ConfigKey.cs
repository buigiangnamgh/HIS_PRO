using System;
using System.Linq;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.LocalData;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList.Base
{
	internal class ConfigKey
	{
		internal const string PrintNow_NumCopy = "CONFIG_KEY__HIS_DESKTOP_PRINT_NOW__NUM_COPY";

		internal const string HIS_DEPOSIT__DEFAULT_PRICE_FOR_BHYT_OUT_PATIENT = "HIS_RS.HIS_DEPOSIT.DEFAULT_PRICE_FOR_BHYT_OUT_PATIENT";

		internal const string Filter_Type_For_Treatment_Patient = "HIS.Desktop.Plugins.ServiceReqList.Filter_Type_For_Treatment_Patient";

		internal const string HIS_Desktop_Plugins_AssignServicePrintTEST = "HIS.Desktop.Plugins.AssignServicePrintTEST";

		internal const string PATIENT_TYPE_ID__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		internal const string MpsTotalToBordereau = "HIS.Desktop.Plugins.ServiceReqList.MpsTotalToBordereau";

		private static string heinLevelCodeCurrent;

		public static string HEIN_LEVEL_CODE__CURRENT
		{
			get
			{
				try
				{
					HIS_BRANCH hIS_BRANCH = BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == WorkPlace.GetBranchId());
					if (hIS_BRANCH != null)
					{
						heinLevelCodeCurrent = hIS_BRANCH.HEIN_LEVEL_CODE;
					}
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
				}
				return heinLevelCodeCurrent;
			}
			set
			{
				heinLevelCodeCurrent = value;
			}
		}
	}
}
