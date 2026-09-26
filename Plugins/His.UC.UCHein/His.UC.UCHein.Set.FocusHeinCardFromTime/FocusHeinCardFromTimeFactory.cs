using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.FocusHeinCardFromTime
{
	internal class FocusHeinCardFromTimeFactory
	{
		internal static IFocusHeinCardFromTime MakeIFocusHeinCardFromTime(CommonParam param, UserControl data)
		{
			IFocusHeinCardFromTime focusHeinCardFromTime = null;
			try
			{
				if (data is Template__HeinBHYT1)
				{
					focusHeinCardFromTime = new FocusHeinCardFromTimeBhytBehavior(param, data as Template__HeinBHYT1);
				}
				if (focusHeinCardFromTime == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				focusHeinCardFromTime = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				focusHeinCardFromTime = null;
			}
			return focusHeinCardFromTime;
		}
	}
}
