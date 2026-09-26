using System;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Get.ExpriedTimeHeinCardBhyt
{
	internal class ExpriedTimeHeinCardBhytBehavior : BeanObjectBase, IExpriedTimeHeinCardBhyt
	{
		private Template__HeinBHYT1 UC;

		private long alertExpriedTimeHeinCardBhyt;

		private long resultDayAlert;

		internal ExpriedTimeHeinCardBhytBehavior(CommonParam param, Template__HeinBHYT1 uc, long alertExpriedCard, ref long resultdayAlert)
			: base(param)
		{
			UC = uc;
			alertExpriedTimeHeinCardBhyt = alertExpriedCard;
			resultDayAlert = resultdayAlert;
		}

		long IExpriedTimeHeinCardBhyt.Run()
		{
			long result = 0L;
			try
			{
				result = UC.GetExpriedTimeHeinCardBhyt(alertExpriedTimeHeinCardBhyt, ref resultDayAlert);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				base.param.HasException = true;
			}
			return result;
		}
	}
}
