using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.ResetValidationControl
{
	internal class ResetValidationControlBehavior : BeanObjectBase, IResetValidationControl
	{
		private Template__HeinBHYT1 UC;

		internal ResetValidationControlBehavior(CommonParam param, Template__HeinBHYT1 uc)
			: base(param)
		{
			UC = uc;
		}

		void IResetValidationControl.Run()
		{
			try
			{
				UC.ResetValidationControl();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
