using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.MedicalStoreV2.ADO
{
	public class DataStoreADO : V_HIS_DATA_STORE_1
	{
		public bool CheckStore { get; set; }

		public string DataStoreNameWithCountTreatment { get; set; }

		public bool IsChuaLuu { get; set; }

		public DataStoreADO()
		{
		}

		public DataStoreADO(V_HIS_DATA_STORE_1 data)
		{
			if (data != null)
			{
				DataObjectMapper.Map<DataStoreADO>(this, data);
				DataStoreNameWithCountTreatment = ((V_HIS_DATA_STORE_1)this).DATA_STORE_NAME + " (" + string.Format("{0:0.####}", ((V_HIS_DATA_STORE_1)this).MEDI_RECORD_COUNT) + ")";
			}
		}
	}
}
