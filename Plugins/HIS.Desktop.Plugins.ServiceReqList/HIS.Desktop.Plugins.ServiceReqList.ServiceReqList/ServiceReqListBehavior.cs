using System;
using System.Linq;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Tools;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList.ServiceReqList
{
	internal class ServiceReqListBehavior : Tool<IDesktopToolContext>, IServiceReqList
	{
		private object[] entity;

		internal ServiceReqListBehavior()
		{
		}

		internal ServiceReqListBehavior(CommonParam param, object[] data)
		{
			entity = data;
		}

		object IServiceReqList.Run()
		{
			object obj = null;
			try
			{
				Module module = null;
				HIS_TREATMENT hIS_TREATMENT = null;
				V_HIS_PATIENT v_HIS_PATIENT = null;
				if (entity != null && entity.Count() > 0)
				{
					for (int i = 0; i < entity.Count(); i++)
					{
						if (entity[i] is HIS_TREATMENT)
						{
							hIS_TREATMENT = (HIS_TREATMENT)entity[i];
						}
						else if (entity[i] is Module)
						{
							module = (Module)entity[i];
						}
						else if (entity[i] is V_HIS_PATIENT)
						{
							v_HIS_PATIENT = (V_HIS_PATIENT)entity[i];
						}
					}
				}
				obj = ((module == null) ? null : ((hIS_TREATMENT != null) ? new frmServiceReqList(module, hIS_TREATMENT) : ((v_HIS_PATIENT == null) ? new frmServiceReqList(module) : new frmServiceReqList(module, v_HIS_PATIENT))));
				if (obj == null)
				{
					throw new NullReferenceException(LogUtil.TraceData(LogUtil.GetMemberName(() => entity), entity));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				obj = null;
			}
			return obj;
		}
	}
}
