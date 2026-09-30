using System;
using System.Collections.Generic;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList.ADO
{
	internal class ThreadChiDinhDichVuADO
	{
		public HIS_TREATMENT hisTreatment { get; set; }

		public List<V_HIS_SERE_SERV> listVHisSereServ { get; set; }

		public V_HIS_PATIENT_TYPE_ALTER vHisPatientTypeAlter { get; set; }

		public ServiceReqADO vHisServiceReq2Print { get; set; }

		public List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit { get; set; }

		public List<HIS_SERE_SERV_BILL> ListSereServBill { get; set; }

		public ThreadChiDinhDichVuADO()
		{
		}

		public ThreadChiDinhDichVuADO(ServiceReqADO data)
		{
			try
			{
				if (data != null)
				{
					vHisServiceReq2Print = data;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
