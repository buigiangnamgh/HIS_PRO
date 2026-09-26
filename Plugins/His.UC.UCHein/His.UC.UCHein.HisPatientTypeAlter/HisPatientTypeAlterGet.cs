using System.Collections.Generic;
using System.Linq;
using His.UC.UCHein.Base;
using Inventec.Common.Adapter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace His.UC.UCHein.HisPatientTypeAlter
{
	internal class HisPatientTypeAlterGet
	{
		internal static V_HIS_PATIENT_TYPE_ALTER GetById(long patientTypeAlterId)
		{
			new CommonParam();
			return GetView(new HisPatientTypeAlterViewFilter
			{
				ID = patientTypeAlterId
			}).SingleOrDefault((V_HIS_PATIENT_TYPE_ALTER o) => o.ID == patientTypeAlterId);
		}

		internal static List<HIS_PATIENT_TYPE_ALTER> Get(HisPatientTypeAlterFilter filter)
		{
			CommonParam commonParam = new CommonParam();
			return new BackendAdapter(commonParam).Get<List<HIS_PATIENT_TYPE_ALTER>>("/api/HisPatientTypeAlter/GetView", ApiConsumerStore.MosConsumer, filter, commonParam);
		}

		internal static List<V_HIS_PATIENT_TYPE_ALTER> GetView(HisPatientTypeAlterViewFilter filter)
		{
			CommonParam commonParam = new CommonParam();
			return new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT_TYPE_ALTER>>("/api/HisPatientTypeAlter/GetView", ApiConsumerStore.MosConsumer, filter, commonParam);
		}
	}
}
