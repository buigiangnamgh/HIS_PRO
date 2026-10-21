using System;
using System.Linq;
using Inventec.Common.Logging;
using Inventec.Core;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.Core;
using Inventec.Desktop.Core.Tools;

namespace HIS.Desktop.Plugins.MedicalStoreV2.MedicalStoreV2
{
	public sealed class MedicalStoreV2Behavior : Tool<IDesktopToolContext>, IMedicalStoreV2
	{
		private object[] entity;

		public MedicalStoreV2Behavior()
		{
		}

		public MedicalStoreV2Behavior(CommonParam param, object[] filter)
		{
			entity = filter;
		}

		object IMedicalStoreV2.Run()
		{
			object obj = null;
			try
			{
				Module module = null;
				if (entity != null && entity.Count() > 0)
				{
					for (int i = 0; i < entity.Count(); i++)
					{
						if (entity[i] is Module)
						{
							module = (Module)entity[i];
						}
					}
				}
				if (module != null)
				{
					return new UCMedicalStoreV2(module);
				}
				return null;
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
