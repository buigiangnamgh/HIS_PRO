using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.SetFocusUserByLiveAreaCode
{
	internal class SetFocusUserByLiveAreaCodeBhytBehavior : BeanObjectBase, ISetFocusUserByLiveAreaCode
	{
		private Template__HeinBHYT1 UC;

		internal SetFocusUserByLiveAreaCodeBhytBehavior(CommonParam param, Template__HeinBHYT1 uc)
			: base(param)
		{
			UC = uc;
		}

		void ISetFocusUserByLiveAreaCode.Run()
		{
			try
			{
				UC.SetFocusUserByLiveAreaCode();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
		}
	}
}
