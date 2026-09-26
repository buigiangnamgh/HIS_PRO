using System;
using System.Windows.Forms;
using His.UC.UCHein.Base;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.FillDataTranPatiInForm
{
	public sealed class FillDataTranPatiInFormBehavior : IRun
	{
		private long treatmentId;

		private UserControl ucParam;

		public FillDataTranPatiInFormBehavior()
		{
		}

		public FillDataTranPatiInFormBehavior(CommonParam param, long treatmentid, UserControl uc)
		{
			treatmentId = treatmentid;
			ucParam = uc;
		}

		object IRun.Run()
		{
			try
			{
				if (ucParam.GetType() == typeof(Template__HeinBHYT1))
				{
					((Template__HeinBHYT1)ucParam).ProcessFillDataTranPatiInForm(treatmentId);
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + ucParam.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => treatmentId), treatmentId), ex);
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
			return null;
		}
	}
}
