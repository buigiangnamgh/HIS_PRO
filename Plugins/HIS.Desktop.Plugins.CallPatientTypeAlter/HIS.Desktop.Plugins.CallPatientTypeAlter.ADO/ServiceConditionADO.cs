using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter.ADO
{
	public class ServiceConditionADO : HIS_SERVICE_CONDITION
	{
		public long SERVICE_CONDITION_ID { get; set; }

		public ServiceConditionADO(HIS_SERVICE_CONDITION data)
		{
			if (data != null)
			{
				DataObjectMapper.Map<HIS_SERVICE_CONDITION>(this, data);
				if (((HIS_SERVICE_CONDITION)this).HEIN_RATIO.HasValue)
				{
					((HIS_SERVICE_CONDITION)this).HEIN_RATIO = ((HIS_SERVICE_CONDITION)this).HEIN_RATIO * (decimal?)100;
				}
			}
		}
	}
}
