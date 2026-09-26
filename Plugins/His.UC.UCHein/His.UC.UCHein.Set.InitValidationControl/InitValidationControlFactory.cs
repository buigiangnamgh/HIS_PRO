using System;
using System.Windows.Forms;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Set.InitValidationControl
{
	internal class InitValidationControlFactory
	{
		internal static IInitValidationControl MakeIInitValidationControl(CommonParam param, UserControl data)
		{
			IInitValidationControl initValidationControl = null;
			try
			{
				if (data is Template__HeinBHYT1)
				{
					initValidationControl = new InitValidationControlBhytBehavior(param, data as Template__HeinBHYT1);
				}
				if (initValidationControl == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				initValidationControl = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				initValidationControl = null;
			}
			return initValidationControl;
		}
	}
}
