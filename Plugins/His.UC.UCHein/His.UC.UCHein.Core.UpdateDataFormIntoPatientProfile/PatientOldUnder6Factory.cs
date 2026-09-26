using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.UpdateDataFormIntoPatientProfile
{
	internal class PatientOldUnder6Factory
	{
		internal static IPatientOldUnder6 MakeIPatientOldUnder6(CommonParam param, UserControl UC, bool IsChild)
		{
			IPatientOldUnder6 patientOldUnder = null;
			try
			{
				if (UC is Template__HeinBHYT1)
				{
					patientOldUnder = new PatientOldUnder6Behavior(param, (Template__HeinBHYT1)UC, IsChild);
				}
				if (patientOldUnder == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException)
			{
				patientOldUnder = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				patientOldUnder = null;
			}
			return patientOldUnder;
		}
	}
}
