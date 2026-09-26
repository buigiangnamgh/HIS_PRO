using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.InitValidationControl
{
	internal class InitValidationControlBhytBehavior : BeanObjectBase, IInitValidationControl
	{
		private Template__HeinBHYT1 UC;

		internal InitValidationControlBhytBehavior(CommonParam param, Template__HeinBHYT1 uc)
			: base(param)
		{
			UC = uc;
		}

		bool IInitValidationControl.Run()
		{
			try
			{
				return UC.GetInvalidControls();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return false;
		}
	}
}
