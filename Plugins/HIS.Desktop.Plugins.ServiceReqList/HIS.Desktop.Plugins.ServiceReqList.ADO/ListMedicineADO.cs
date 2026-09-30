using System;
using System.Linq;
using System.Reflection;
using HIS.Desktop.LocalStorage.BackendData;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000014.PDO;

namespace HIS.Desktop.Plugins.ServiceReqList.ADO
{
	public class ListMedicineADO : HIS_SERE_SERV
	{
		public long ExpMestMedicineId { get; set; }

		public long NUM_ORDER { get; set; }

		public string SERVICE_UNIT_NAME { get; set; }

		public int kind { get; set; }

		public string HuongDanSuDung { get; set; }

		public decimal? TocDoTruyen { get; set; }

		public int type { get; set; }

		public short? subPress { get; set; }

		public short? isStartMark { get; set; }

		public decimal? CONVERT_RATIO { get; set; }

		public string CONVERT_UNIT_NAME { get; set; }

		public decimal? CONVERT_AMOUNT { get; set; }

		public long? USE_TIME_TO { get; set; }

		public HIS_SERVICE_REQ_METY serviceReqMety { get; set; }

		public long? PREVIOUS_USE_DAY { get; set; }

		public HIS_EXP_MEST_MEDICINE xpMestMedicine { get; set; }

		public short? IS_RATION { get; set; }

		public string PATIENT_TYPE_NAME { get; set; }

		public decimal? PRES_AMOUNT { get; set; }

		public long? USE_TIME { get; set; }

		public string PTTT_GROUP_NAME { get; set; }

		public ListMedicineADO()
		{
		}

		public ListMedicineADO(HIS_SERE_SERV data)
		{
			try
			{
				if (data == null)
				{
					return;
				}
				PropertyInfo[] array = Inventec.Common.Repository.Properties.Get<HIS_SERE_SERV>();
				PropertyInfo[] array2 = array;
				foreach (PropertyInfo propertyInfo in array2)
				{
					propertyInfo.SetValue(this, propertyInfo.GetValue(data));
				}
				CommonParam commonParam = new CommonParam();
				HIS_SERVICE_UNIT unit = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == data.TDL_SERVICE_UNIT_ID);
				if (unit == null)
				{
					return;
				}
				SERVICE_UNIT_NAME = unit.SERVICE_UNIT_NAME;
				CONVERT_RATIO = unit.CONVERT_RATIO;
				if (unit.CONVERT_ID.HasValue && unit.CONVERT_RATIO.HasValue)
				{
					HIS_SERVICE_UNIT hIS_SERVICE_UNIT = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == unit.CONVERT_ID);
					if (hIS_SERVICE_UNIT != null)
					{
						CONVERT_UNIT_NAME = hIS_SERVICE_UNIT.SERVICE_UNIT_NAME;
						CONVERT_AMOUNT = base.AMOUNT * unit.CONVERT_RATIO.Value;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public ListMedicineADO(SereServNumOder data)
		{
			try
			{
				if (data == null)
				{
					return;
				}
				PropertyInfo[] array = Inventec.Common.Repository.Properties.Get<HIS_SERE_SERV>();
				PropertyInfo[] array2 = array;
				foreach (PropertyInfo propertyInfo in array2)
				{
					propertyInfo.SetValue(this, propertyInfo.GetValue(data));
				}
				HIS_SERVICE_UNIT unit = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == data.TDL_SERVICE_UNIT_ID);
				if (unit == null)
				{
					return;
				}
				SERVICE_UNIT_NAME = unit.SERVICE_UNIT_NAME;
				CONVERT_RATIO = unit.CONVERT_RATIO;
				if (unit.CONVERT_ID.HasValue && unit.CONVERT_RATIO.HasValue)
				{
					HIS_SERVICE_UNIT hIS_SERVICE_UNIT = BackendDataWorker.Get<HIS_SERVICE_UNIT>().FirstOrDefault((HIS_SERVICE_UNIT o) => o.ID == unit.CONVERT_ID);
					if (hIS_SERVICE_UNIT != null)
					{
						CONVERT_UNIT_NAME = hIS_SERVICE_UNIT.SERVICE_UNIT_NAME;
						CONVERT_AMOUNT = base.AMOUNT * unit.CONVERT_RATIO.Value;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
