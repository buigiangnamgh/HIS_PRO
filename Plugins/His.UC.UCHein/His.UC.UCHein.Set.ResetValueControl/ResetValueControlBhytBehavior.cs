using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.ResetValueControl
{
	internal class ResetValueControlBhytBehavior : BeanObjectBase, IResetValueControl
	{
		private UserControl UC;

		internal ResetValueControlBhytBehavior(CommonParam param, UserControl uc)
			: base(param)
		{
			UC = uc;
		}

		void IResetValueControl.Run()
		{
			try
			{
				((Template__HeinBHYT1)UC).ResetValue();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
		}
	}
}
