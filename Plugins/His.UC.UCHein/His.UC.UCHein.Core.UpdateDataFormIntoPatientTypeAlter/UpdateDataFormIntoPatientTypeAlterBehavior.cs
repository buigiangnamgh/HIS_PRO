using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.SDO;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientTypeAlter
{
	internal class UpdateDataFormIntoPatientTypeAlterBehavior : BeanObjectBase, IUpdateDataFormIntoPatientTypeAlter
	{
		private Template__HeinBHYT1 UC;

		private HisPatientProfileSDO patientProfileSDO;

		internal UpdateDataFormIntoPatientTypeAlterBehavior(CommonParam param, Template__HeinBHYT1 uc, HisPatientProfileSDO patientProfileSDO)
			: base(param)
		{
			UC = uc;
			this.patientProfileSDO = patientProfileSDO;
		}

		void IUpdateDataFormIntoPatientTypeAlter.Run()
		{
			try
			{
				UC.UpdateDataFormIntoPatientTypeAlter(patientProfileSDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
