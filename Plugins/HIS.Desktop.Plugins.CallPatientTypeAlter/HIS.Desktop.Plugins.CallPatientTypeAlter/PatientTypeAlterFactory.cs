using System;
using System.Collections.Generic;
using System.Linq;
using HIS.Desktop.ADO;
using HIS.Desktop.Common;
using HIS.Desktop.LibraryMessage;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	internal class PatientTypeAlterFactory
	{
		internal static IPatientTypeAlter MakeIPatientTypeAlter(CommonParam param, object[] data)
		{
			IPatientTypeAlter patientTypeAlter = null;
			Module module = null;
			long treatmentId = 0L;
			PatientTypeDepartmentADO hisTreatmentLogSDO = null;
			bool? isView = false;
			RefeshReference refeshReference = null;
			List<PatientTypeDepartmentADO> lstTreatmentLog = null;
			try
			{
				if (data.GetType() == typeof(object[]) && data != null && data.Count() > 0)
				{
					for (int i = 0; i < data.Count(); i++)
					{
						if (data[i] is RefeshReference)
						{
							refeshReference = (RefeshReference)data[i];
						}
						else if (data[i] is Module)
						{
							module = (Module)data[i];
						}
						else if (data[i] is PatientTypeDepartmentADO)
						{
							hisTreatmentLogSDO = (PatientTypeDepartmentADO)data[i];
						}
						else if (data[i] is bool?)
						{
							isView = (bool?)data[i];
						}
						else if (data[i] is long)
						{
							treatmentId = (long)data[i];
						}
						else if (data[i] is List<PatientTypeDepartmentADO>)
						{
							lstTreatmentLog = (List<PatientTypeDepartmentADO>)data[i];
						}
					}
					if (module != null)
					{
						patientTypeAlter = new PatientTypeAlterBehavior(param, module, hisTreatmentLogSDO, treatmentId, isView, lstTreatmentLog, refeshReference);
					}
					else
					{
						MessageManager.Show(MessageUtil.GetMessage(Message.Enum.TaiKhoanKhongCoQuyenThucHienChucNang));
					}
				}
				if (patientTypeAlter == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				patientTypeAlter = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				patientTypeAlter = null;
			}
			return patientTypeAlter;
		}
	}
}
