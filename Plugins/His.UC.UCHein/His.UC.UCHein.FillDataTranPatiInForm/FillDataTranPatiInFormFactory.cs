using System;
using System.Windows.Forms;
using His.UC.UCHein.Base;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.FillDataTranPatiInForm
{
	internal class FillDataTranPatiInFormFactory
	{
		internal static IRun MakeIFillDataTranPatiInForm(CommonParam param, long data, UserControl uc)
		{
			IRun run = null;
			try
			{
				if (uc.GetType() == typeof(Template__HeinBHYT1))
				{
					run = new FillDataTranPatiInFormBehavior(param, data, uc);
				}
				if (run == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + uc.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				run = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				run = null;
			}
			return run;
		}
	}
}
