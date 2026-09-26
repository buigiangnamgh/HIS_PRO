using System;
using HIS.Desktop.Plugins.CheckInfoBHYT.CheckInfoBHYT;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Core;

namespace HIS.Desktop.Plugins.CheckInfoBHYT
{
	[ExtensionOf(typeof(DesktopRootExtensionPoint), "HIS.Desktop.Plugins.CheckInfoBHYT", "Kiểm tra thông tin thẻ BHYT", "Common", 59, "CheckInfoBHYT.png", "A", 2L, true, true)]
	public class CheckInfoBHYTProcessor : ModuleBase, IDesktopRoot
	{
		private CommonParam param;

		public CheckInfoBHYTProcessor()
		{
			param = new CommonParam();
		}

		public CheckInfoBHYTProcessor(CommonParam paramBusiness)
		{
			param = ((paramBusiness != null) ? paramBusiness : new CommonParam());
		}

		object IDesktopRoot.Run(object[] args)
		{
			object obj = null;
			try
			{
				ICheckInfoBHYT checkInfoBHYT = CheckInfoBHYTFactory.MakeICheckInfoBHYT(param, args);
				return (checkInfoBHYT != null) ? checkInfoBHYT.Run() : null;
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
