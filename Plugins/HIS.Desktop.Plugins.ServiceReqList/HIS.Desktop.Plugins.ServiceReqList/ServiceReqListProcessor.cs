using System;
using HIS.Desktop.Plugins.ServiceReqList.ServiceReqList;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Core;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	[ExtensionOf(typeof(DesktopRootExtensionPoint), "HIS.Desktop.Plugins.ServiceReqList", "Danh sách yêu cầu dịch vụ", "Common", 68, "y-lenh.png", "A", 2L, true, true)]
	public class ServiceReqListProcessor : ModuleBase, IDesktopRoot
	{
		private CommonParam param;

		public ServiceReqListProcessor()
		{
			param = new CommonParam();
		}

		public ServiceReqListProcessor(CommonParam paramBusiness)
		{
			param = ((paramBusiness != null) ? paramBusiness : new CommonParam());
		}

		object IDesktopRoot.Run(object[] args)
		{
			LogSystem.Info("begin load");
			object obj = null;
			try
			{
				IServiceReqList serviceReqList = ServiceReqListFactory.MakeIServiceReqList(param, args);
				return (serviceReqList != null) ? serviceReqList.Run() : null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public override bool IsEnable()
		{
			return false;
		}
	}
}
