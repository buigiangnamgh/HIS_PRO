using System;
using System.Windows.Forms;
using His.UC.UCHein.Base;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.SDO;

namespace His.UC.UCHein.FillDataToHeinInsuranceInfoByOldPatient
{
	public sealed class FillDataToHeinInsuranceInfoByOldPatientBehaviorSDO : IRun
	{
		private HisPatientSDO patientParam;

		private UserControl ucParam;

		public FillDataToHeinInsuranceInfoByOldPatientBehaviorSDO()
		{
		}

		public FillDataToHeinInsuranceInfoByOldPatientBehaviorSDO(CommonParam param, HisPatientSDO patient, UserControl uc)
		{
			patientParam = patient;
			ucParam = uc;
		}

		object IRun.Run()
		{
			try
			{
				if (ucParam.GetType() == typeof(Template__HeinBHYT1))
				{
					((Template__HeinBHYT1)ucParam).FillDataToHeinInsuranceInfoByOldPatient(patientParam);
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + patientParam.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => patientParam), patientParam), ex);
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
			return null;
		}
	}
}
