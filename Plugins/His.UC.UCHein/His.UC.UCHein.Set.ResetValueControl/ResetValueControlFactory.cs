using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.ResetValueControl
{
	internal class ResetValueControlFactory
	{
		internal static IResetValueControl MakeIResetValueControl(CommonParam param, UserControl data)
		{
			IResetValueControl resetValueControl = null;
			try
			{
				if (data is Template__HeinBHYT1)
				{
					resetValueControl = new ResetValueControlBhytBehavior(param, data as Template__HeinBHYT1);
				}
				if (resetValueControl == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				resetValueControl = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				resetValueControl = null;
			}
			return resetValueControl;
		}
	}
}
