using System;
using System.Windows.Forms;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using His.UC.UCHein.Design.TemplateHeinBHYT1;
using Inventec.Common.Logging;
using Inventec.Core;

namespace His.UC.UCHein.Core.SetResultDataADO
{
	internal class SetResultDataADOFactory
	{
		internal static ISetResultDataADO MakeISetResultDataADO(CommonParam param, UserControl data, ResultDataADO ResultDataADO)
		{
			ISetResultDataADO setResultDataADO = null;
			try
			{
				if (data is Template__HeinBHYT1)
				{
					setResultDataADO = new SetResultDataADOBehavior(param, data as Template__HeinBHYT1, ResultDataADO);
				}
				if (setResultDataADO == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				setResultDataADO = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				setResultDataADO = null;
			}
			return setResultDataADO;
		}
	}
}
