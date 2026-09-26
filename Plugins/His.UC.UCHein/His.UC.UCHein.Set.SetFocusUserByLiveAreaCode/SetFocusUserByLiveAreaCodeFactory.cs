using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.SetFocusUserByLiveAreaCode
{
	internal class SetFocusUserByLiveAreaCodeFactory
	{
		internal static ISetFocusUserByLiveAreaCode MakeISetFocusUserByLiveAreaCode(CommonParam param, UserControl data)
		{
			ISetFocusUserByLiveAreaCode setFocusUserByLiveAreaCode = null;
			try
			{
				if (data is Template__HeinBHYT1)
				{
					setFocusUserByLiveAreaCode = new SetFocusUserByLiveAreaCodeBhytBehavior(param, data as Template__HeinBHYT1);
				}
				if (setFocusUserByLiveAreaCode == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				setFocusUserByLiveAreaCode = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				setFocusUserByLiveAreaCode = null;
			}
			return setFocusUserByLiveAreaCode;
		}
	}
}
