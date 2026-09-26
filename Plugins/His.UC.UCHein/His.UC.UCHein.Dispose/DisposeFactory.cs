using System;
using System.Windows.Forms;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Dispose
{
	internal class DisposeFactory
	{
		internal static IDispose MakeIDispose(CommonParam param, UserControl uc)
		{
			IDispose dispose = null;
			try
			{
				if (uc != null)
				{
					dispose = new DisposeBehavior(param, uc);
				}
				if (dispose == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + uc.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => uc), uc), ex);
				dispose = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				dispose = null;
			}
			return dispose;
		}
	}
}
