using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.SDO;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientTypeAlter
{
	internal class UpdateDataFormIntoPatientTypeAlterFactory
	{
		internal static IUpdateDataFormIntoPatientTypeAlter MakeIUpdateDataFormIntoPatientProfile(CommonParam param, UserControl UC, HisPatientProfileSDO patientProfileSDO)
		{
			IUpdateDataFormIntoPatientTypeAlter updateDataFormIntoPatientTypeAlter = null;
			try
			{
				if (UC is Template__HeinBHYT1)
				{
					updateDataFormIntoPatientTypeAlter = new UpdateDataFormIntoPatientTypeAlterBehavior(param, (Template__HeinBHYT1)UC, patientProfileSDO);
				}
				if (updateDataFormIntoPatientTypeAlter == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + ((UC != null) ? UC.GetType().ToString() : patientProfileSDO.GetType().ToString()) + LogUtil.TraceData(LogUtil.GetMemberName(() => patientProfileSDO), patientProfileSDO), ex);
				updateDataFormIntoPatientTypeAlter = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				updateDataFormIntoPatientTypeAlter = null;
			}
			return updateDataFormIntoPatientTypeAlter;
		}
	}
}
