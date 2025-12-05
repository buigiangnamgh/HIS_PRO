using System.Collections.Generic;
using System.Linq;
using ACS.EFMODEL.DataModels;
using HIS.Desktop.LocalStorage.BackendData;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic.Base
{
	internal class GlobalStore
	{
		public static List<HIS_ICD> HisIcds
		{
			get
			{
				return (from o in BackendDataWorker.Get<HIS_ICD>()
					orderby o.ICD_CODE descending
					select o).ToList();
			}
		}

		public static List<ACS_USER> HisAcsUser
		{
			get
			{
				return (from o in BackendDataWorker.Get<ACS_USER>()
					where !string.IsNullOrEmpty(o.USERNAME) && o.IS_ACTIVE == 1
					orderby o.USERNAME
					select o).ToList();
			}
		}
	}
}
