using System;
using Inventec.Common.Logging;
using Inventec.Core;

namespace HIS.Desktop.Plugins.CheckInfoBHYT.CheckInfoBHYT
{
	internal class CheckInfoBHYTFactory
	{
		internal static ICheckInfoBHYT MakeICheckInfoBHYT(CommonParam param, object[] data)
		{
			ICheckInfoBHYT checkInfoBHYT = null;
			try
			{
				checkInfoBHYT = new CheckInfoBHYTBehavior(param, data);
				if (checkInfoBHYT == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				checkInfoBHYT = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				checkInfoBHYT = null;
			}
			return checkInfoBHYT;
		}
	}
}
