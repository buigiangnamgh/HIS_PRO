using System;
using System.Linq;
using System.Reflection;
using HIS.Desktop.LocalStorage.BackendData;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList.ADO
{
	public class ServiceReqADO : HIS_SERVICE_REQ
	{
		public bool isCheck { get; set; }

		public string EXECUTE_DEPARTMENT_CODE { get; set; }

		public string EXECUTE_DEPARTMENT_NAME { get; set; }

		public string EXECUTE_ROOM_CODE { get; set; }

		public string EXECUTE_ROOM_NAME { get; set; }

		public string EXECUTE_ROOM_ADDRESS { get; set; }

		public string REQUEST_DEPARTMENT_CODE { get; set; }

		public string REQUEST_DEPARTMENT_NAME { get; set; }

		public string REQUEST_ROOM_CODE { get; set; }

		public string REQUEST_ROOM_NAME { get; set; }

		public string SERVICE_REQ_STT_CODE { get; set; }

		public string SERVICE_REQ_STT_NAME { get; set; }

		public string SERVICE_REQ_TYPE_CODE { get; set; }

		public string SERVICE_REQ_TYPE_NAME { get; set; }

		public bool DeleteCheck { get; set; }

		public bool AddInforPTTT { get; set; }

		public string SAMPLE_ROOM_CODE { get; set; }

		public string SAMPLE_ROOM_NAME { get; set; }

		public ServiceReqADO()
		{
		}

		public ServiceReqADO(HIS_SERVICE_REQ data)
		{
			try
			{
				if (data == null)
				{
					return;
				}
				PropertyInfo[] array = Inventec.Common.Repository.Properties.Get<HIS_SERVICE_REQ>();
				PropertyInfo[] array2 = array;
				foreach (PropertyInfo propertyInfo in array2)
				{
					propertyInfo.SetValue(this, propertyInfo.GetValue(data));
				}
				V_HIS_ROOM v_HIS_ROOM = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == data.EXECUTE_ROOM_ID);
				if (v_HIS_ROOM != null)
				{
					EXECUTE_DEPARTMENT_CODE = v_HIS_ROOM.DEPARTMENT_CODE;
					EXECUTE_DEPARTMENT_NAME = v_HIS_ROOM.DEPARTMENT_NAME;
					EXECUTE_ROOM_CODE = v_HIS_ROOM.ROOM_CODE;
					EXECUTE_ROOM_NAME = v_HIS_ROOM.ROOM_NAME;
					EXECUTE_ROOM_ADDRESS = v_HIS_ROOM.ADDRESS;
				}
				else
				{
					base.EXECUTE_DEPARTMENT_ID = data.EXECUTE_DEPARTMENT_ID;
					base.EXECUTE_ROOM_ID = data.EXECUTE_ROOM_ID;
				}
				V_HIS_ROOM v_HIS_ROOM2 = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == data.REQUEST_ROOM_ID);
				if (v_HIS_ROOM2 != null)
				{
					REQUEST_DEPARTMENT_CODE = v_HIS_ROOM2.DEPARTMENT_CODE;
					REQUEST_DEPARTMENT_NAME = v_HIS_ROOM2.DEPARTMENT_NAME;
					REQUEST_ROOM_CODE = v_HIS_ROOM2.ROOM_CODE;
					REQUEST_ROOM_NAME = v_HIS_ROOM2.ROOM_NAME;
				}
				else
				{
					base.REQUEST_DEPARTMENT_ID = data.REQUEST_DEPARTMENT_ID;
					base.REQUEST_ROOM_ID = data.REQUEST_ROOM_ID;
				}
				HIS_SERVICE_REQ_STT hIS_SERVICE_REQ_STT = BackendDataWorker.Get<HIS_SERVICE_REQ_STT>().FirstOrDefault((HIS_SERVICE_REQ_STT o) => o.ID == data.SERVICE_REQ_STT_ID);
				if (hIS_SERVICE_REQ_STT != null)
				{
					SERVICE_REQ_STT_CODE = hIS_SERVICE_REQ_STT.SERVICE_REQ_STT_CODE;
					SERVICE_REQ_STT_NAME = hIS_SERVICE_REQ_STT.SERVICE_REQ_STT_NAME;
				}
				HIS_SERVICE_REQ_TYPE hIS_SERVICE_REQ_TYPE = BackendDataWorker.Get<HIS_SERVICE_REQ_TYPE>().FirstOrDefault((HIS_SERVICE_REQ_TYPE o) => o.ID == data.SERVICE_REQ_TYPE_ID);
				if (hIS_SERVICE_REQ_TYPE != null)
				{
					SERVICE_REQ_TYPE_CODE = hIS_SERVICE_REQ_TYPE.SERVICE_REQ_TYPE_CODE;
					SERVICE_REQ_TYPE_NAME = hIS_SERVICE_REQ_TYPE.SERVICE_REQ_TYPE_NAME;
				}
				if (data.SAMPLE_ROOM_ID.HasValue)
				{
					HIS_SAMPLE_ROOM hIS_SAMPLE_ROOM = BackendDataWorker.Get<HIS_SAMPLE_ROOM>().FirstOrDefault((HIS_SAMPLE_ROOM o) => o.ID == data.SAMPLE_ROOM_ID);
					if (hIS_SAMPLE_ROOM != null)
					{
						SAMPLE_ROOM_CODE = hIS_SAMPLE_ROOM.SAMPLE_ROOM_CODE;
						SAMPLE_ROOM_NAME = hIS_SAMPLE_ROOM.SAMPLE_ROOM_NAME;
					}
				}
				isCheck = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
