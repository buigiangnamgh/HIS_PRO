using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class TreatmentADO : L_HIS_TREATMENT_3
	{
		public bool CheckTreatment { get; set; }

		public bool CheckStore { get; set; }

		public bool IsStoreChecked { get; set; }

		public TreatmentADO()
		{
		}

		public TreatmentADO(L_HIS_TREATMENT_3 data)
		{
			if (data != null)
			{
				DataObjectMapper.Map<TreatmentADO>(this, data);
				IsStoreChecked = data.STORE_CHECK_TIME.HasValue;
			}
		}
	}
}
