using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Get.ExpriedTimeHeinCardBhyt
{
	internal class ExpriedTimeHeinCardBhytFactory
	{
		internal static IExpriedTimeHeinCardBhyt MakeIExpriedTimeHeinCardBhyt(CommonParam param, object data, object alertExpriedTimeHeinCardBhyt, ref long resultDayAlert)
		{
			IExpriedTimeHeinCardBhyt expriedTimeHeinCardBhyt = null;
			try
			{
				if (data.GetType() == typeof(Template__HeinBHYT1))
				{
					expriedTimeHeinCardBhyt = new ExpriedTimeHeinCardBhytBehavior(param, (Template__HeinBHYT1)data, (long)alertExpriedTimeHeinCardBhyt, ref resultDayAlert);
				}
				if (expriedTimeHeinCardBhyt == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				expriedTimeHeinCardBhyt = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				expriedTimeHeinCardBhyt = null;
			}
			return expriedTimeHeinCardBhyt;
		}
	}
}
