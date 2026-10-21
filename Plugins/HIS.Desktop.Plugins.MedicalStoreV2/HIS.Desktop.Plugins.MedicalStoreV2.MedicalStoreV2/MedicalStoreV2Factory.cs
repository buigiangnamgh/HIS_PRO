using System;
using Inventec.Common.Logging;
using Inventec.Core;

namespace HIS.Desktop.Plugins.MedicalStoreV2.MedicalStoreV2
{
	internal class MedicalStoreV2Factory
	{
		internal static IMedicalStoreV2 MakeIMedicalStore(CommonParam param, object[] data)
		{
			IMedicalStoreV2 medicalStoreV = null;
			try
			{
				medicalStoreV = new MedicalStoreV2Behavior(param, data);
				if (medicalStoreV == null)
				{
					throw new NullReferenceException();
				}
			}
			catch (NullReferenceException ex)
			{
				LogSystem.Error("Factory khong khoi tao duoc doi tuong." + data.GetType().ToString() + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data), ex);
				medicalStoreV = null;
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
				medicalStoreV = null;
			}
			return medicalStoreV;
		}
	}
}
