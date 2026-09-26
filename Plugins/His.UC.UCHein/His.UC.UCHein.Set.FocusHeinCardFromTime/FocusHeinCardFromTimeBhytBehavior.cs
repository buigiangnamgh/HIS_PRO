using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.FocusHeinCardFromTime
{
	internal class FocusHeinCardFromTimeBhytBehavior : BeanObjectBase, IFocusHeinCardFromTime
	{
		private Template__HeinBHYT1 UC;

		internal FocusHeinCardFromTimeBhytBehavior(CommonParam param, Template__HeinBHYT1 uc)
			: base(param)
		{
			UC = uc;
		}

		void IFocusHeinCardFromTime.Run()
		{
			try
			{
				UC.FocusHeinCardFromTime();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
		}
	}
}
