using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.ResetValidationControl
{
	internal class ResetValidationControlFactory
	{
		internal static IResetValidationControl MakeIResetValidationControl(CommonParam param, UserControl uc)
		{
			IResetValidationControl resetValidationControl = null;
			try
			{
				if (uc is Template__HeinBHYT1)
				{
					resetValidationControl = new ResetValidationControlBehavior(param, (Template__HeinBHYT1)uc);
				}
				if (resetValidationControl == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + uc.GetType().ToString(), ex);
				resetValidationControl = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				resetValidationControl = null;
			}
			return resetValidationControl;
		}
	}
}
