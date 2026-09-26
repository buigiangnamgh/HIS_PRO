using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.SDO;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientProfile
{
	internal class UpdateDataFormIntoPatientProfileBehavior : BeanObjectBase, IUpdateDataFormIntoPatientProfile
	{
		private Template__HeinBHYT1 UC;

		private HisPatientProfileSDO patientProfileSDO;

		internal UpdateDataFormIntoPatientProfileBehavior(CommonParam param, Template__HeinBHYT1 uc, HisPatientProfileSDO patientProfileSDO)
			: base(param)
		{
			UC = uc;
			this.patientProfileSDO = patientProfileSDO;
		}

		void IUpdateDataFormIntoPatientProfile.Run()
		{
			try
			{
				UC.UpdateDataFormIntoPatientProfile(patientProfileSDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
