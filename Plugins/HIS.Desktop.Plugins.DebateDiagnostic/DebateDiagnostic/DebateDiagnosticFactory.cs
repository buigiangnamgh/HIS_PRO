using System;
using System.Linq.Expressions;
using Inventec.Common.Logging;
using Inventec.Core;

namespace HIS.Desktop.Plugins.DebateDiagnostic.DebateDiagnostic
{
	internal class DebateDiagnosticFactory
	{
		internal static IDebateDiagnostic MakeIDebateDiagnostic(CommonParam param, object[] data)
		{
			IDebateDiagnostic debateDiagnostic = null;
			try
			{
				debateDiagnostic = new DebateDiagnosticBehavior(param, data);
				if (debateDiagnostic == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName<object[]>((Expression<Func<object[]>>)(() => data)), (object)data), (Exception)ex);
				debateDiagnostic = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				debateDiagnostic = null;
			}
			return debateDiagnostic;
		}
	}
}
