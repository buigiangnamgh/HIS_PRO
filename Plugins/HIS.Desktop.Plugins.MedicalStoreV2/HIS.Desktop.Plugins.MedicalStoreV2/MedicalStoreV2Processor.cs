using System;
using HIS.Desktop.Plugins.MedicalStoreV2.MedicalStoreV2;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Core;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	[ExtensionOf(typeof(DesktopRootExtensionPoint), "HIS.Desktop.Plugins.MedicalStoreV2", "Kho bệnh án", "Common", 14, "tu-benh-an.png", "A", 1L, true, true)]
	public class MedicalStoreV2Processor : ModuleBase, IDesktopRoot
	{
		private CommonParam param;

		public MedicalStoreV2Processor()
		{
			param = new CommonParam();
		}

		public MedicalStoreV2Processor(CommonParam paramBusiness)
		{
			param = ((paramBusiness != null) ? paramBusiness : new CommonParam());
		}

		public object Run(object[] args)
		{
			object obj = null;
			try
			{
				IMedicalStoreV2 medicalStoreV = MedicalStoreV2Factory.MakeIMedicalStore(param, args);
				return (medicalStoreV != null) ? medicalStoreV.Run() : null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public override bool IsEnable()
		{
			bool flag = false;
			try
			{
				return true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return false;
			}
		}
	}
}
