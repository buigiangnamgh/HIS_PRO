using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientProfile
{
	internal class PatientOldUnder6Behavior : BeanObjectBase, IPatientOldUnder6
	{
		private Template__HeinBHYT1 UC;

		private bool IsChild;

		internal PatientOldUnder6Behavior(CommonParam param, Template__HeinBHYT1 uc, bool IsChild)
			: base(param)
		{
			UC = uc;
			this.IsChild = IsChild;
		}

		void IPatientOldUnder6.Run()
		{
			try
			{
				UC.FillDataPatientOldYnder6(IsChild);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
