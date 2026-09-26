using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.SDO;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientProfile
{
	internal class UpdateDataFormIntoPatientProfileFactory
	{
		internal static IUpdateDataFormIntoPatientProfile MakeIUpdateDataFormIntoPatientProfile(CommonParam param, UserControl UC, HisPatientProfileSDO patientProfileSDO)
		{
			IUpdateDataFormIntoPatientProfile updateDataFormIntoPatientProfile = null;
			try
			{
				if (UC is Template__HeinBHYT1)
				{
					updateDataFormIntoPatientProfile = new UpdateDataFormIntoPatientProfileBehavior(param, (Template__HeinBHYT1)UC, patientProfileSDO);
				}
				if (updateDataFormIntoPatientProfile == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + patientProfileSDO.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => patientProfileSDO), patientProfileSDO), ex);
				updateDataFormIntoPatientProfile = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				updateDataFormIntoPatientProfile = null;
			}
			return updateDataFormIntoPatientProfile;
		}
	}
}
