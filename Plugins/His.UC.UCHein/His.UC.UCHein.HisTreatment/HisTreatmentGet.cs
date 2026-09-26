using System.Collections.Generic;
using System.Linq;
using His.UC.UCHein.Base;
using Inventec.Common.Adapter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace His.UC.UCHein.HisTreatment
{
	internal class HisTreatmentGet
	{
		internal static V_HIS_TREATMENT_4 GetById(long treatmentId)
		{
			CommonParam commonParam = new CommonParam();
			HisTreatmentView4Filter hisTreatmentView4Filter = new HisTreatmentView4Filter();
			hisTreatmentView4Filter.ID = treatmentId;
			return new BackendAdapter(commonParam).Get<List<V_HIS_TREATMENT_4>>("/api/HisTreatment/GetView4", ApiConsumerStore.MosConsumer, hisTreatmentView4Filter, commonParam).SingleOrDefault();
		}
	}
}
