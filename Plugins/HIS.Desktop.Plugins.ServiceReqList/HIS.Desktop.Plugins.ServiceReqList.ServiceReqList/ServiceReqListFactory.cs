using System;
using Inventec.Common.Logging;
using Inventec.Core;

namespace HIS.Desktop.Plugins.ServiceReqList.ServiceReqList
{
	internal class ServiceReqListFactory
	{
		internal static IServiceReqList MakeIServiceReqList(CommonParam param, object[] data)
		{
			IServiceReqList serviceReqList = null;
			try
			{
				serviceReqList = new ServiceReqListBehavior(param, data);
				if (serviceReqList == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				serviceReqList = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				serviceReqList = null;
			}
			return serviceReqList;
		}
	}
}
