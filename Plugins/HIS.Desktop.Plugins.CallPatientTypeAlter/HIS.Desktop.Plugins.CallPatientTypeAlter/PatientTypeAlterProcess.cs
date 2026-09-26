using System;
using HIS.Desktop.LocalStorage.LocalData;
using Inventec.Common.LocalStorage.SdaConfig;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Core;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	[ExtensionOf(typeof(DesktopRootExtensionPoint), "HIS.Desktop.Plugins.CallPatientTypeAlter", "Đối tượng điều trị", "Common", 14, "newitem_32x32.png", "A", 2L, true, true)]
	public class PatientTypeAlterProcess : ModuleBase, IDesktopRoot
	{
		private CommonParam param;

		public PatientTypeAlterProcess()
		{
			param = new CommonParam();
		}

		public PatientTypeAlterProcess(CommonParam paramBusiness)
		{
			param = ((paramBusiness != null) ? paramBusiness : new CommonParam());
		}

		public object Run(object[] args)
		{
			object obj = null;
			try
			{
				IPatientTypeAlter patientTypeAlter = PatientTypeAlterFactory.MakeIPatientTypeAlter(param, args);
				return (patientTypeAlter != null) ? patientTypeAlter.Run() : null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public override bool IsEnable()
		{
			bool flag = false;
			try
			{
				if (GlobalVariables.CurrentRoomTypeCode.Contains(SdaConfigs.Get<string>("DBCODE.HIS_RS.HIS_ROOM_TYPE.ROOM_TYPE_CODE.RECEPTION")))
				{
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return false;
			}
		}
	}
}
