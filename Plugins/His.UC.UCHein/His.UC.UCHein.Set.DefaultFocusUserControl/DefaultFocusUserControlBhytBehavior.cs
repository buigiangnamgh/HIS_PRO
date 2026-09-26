using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.DefaultFocusUserControl
{
	internal class DefaultFocusUserControlBhytBehavior : BeanObjectBase, IDefaultFocusUserControl
	{
		private Template__HeinBHYT1 UC;

		internal DefaultFocusUserControlBhytBehavior(CommonParam param, Template__HeinBHYT1 uc)
			: base(param)
		{
			UC = uc;
		}

		void IDefaultFocusUserControl.Run()
		{
			try
			{
				UC.DefaultFocusUserControl();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
		}
	}
}
