using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class PopupMenuProcessorMedicine
	{
		internal enum ItemType
		{
			SuaHuongDanSuDung,
			AssignPres,
			AssignPresCabinet,
			PrintServiceReq,
			AssignInKip,
			AssignOutKip,
			AcceptNoExecute,
			UnacceptNoExecute
		}

		public ListMedicineADO listMedicineAdo;

		private BarManager _BarManager = null;

		private PopupMenu _PopupMenu = null;

		private MouseRightClick _MouseRightClick;

		private WorkPlaceSDO _CurrentWorkPlace;

		internal PopupMenuProcessorMedicine(ListMedicineADO data, BarManager barmanager, MouseRightClick mouseRightClick, WorkPlaceSDO currentWorkPlace)
		{
			listMedicineAdo = data;
			_MouseRightClick = mouseRightClick;
			_BarManager = barmanager;
			_CurrentWorkPlace = currentWorkPlace;
		}

		internal void InitMenu()
		{
			try
			{
				if (listMedicineAdo == null || _BarManager == null || _MouseRightClick == null)
				{
					return;
				}
				if (_PopupMenu == null)
				{
					_PopupMenu = new PopupMenu(_BarManager);
				}
				_PopupMenu.ItemLinks.Clear();
				List<HIS_SERVICE_REQ> list = new List<HIS_SERVICE_REQ>();
				if (listMedicineAdo.SERVICE_REQ_ID.HasValue)
				{
					HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
					hisServiceReqFilter.ID = listMedicineAdo.SERVICE_REQ_ID;
					list = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", ApiConsumers.MosConsumer, hisServiceReqFilter, new CommonParam());
				}
				List<BarItem> list2 = new List<BarItem>();
				if (listMedicineAdo.type == 1)
				{
					BarButtonItem barButtonItem = new BarButtonItem(_BarManager, "Sửa thông tin chung", 0);
					barButtonItem.Tag = ItemType.SuaHuongDanSuDung;
					barButtonItem.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem);
				}
				if (listMedicineAdo.TDL_SERVICE_TYPE_ID != 0 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 6 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 7 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 14 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 12 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 16 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 8)
				{
					BarButtonItem barButtonItem2 = new BarButtonItem(_BarManager, "Kê đơn", 0);
					barButtonItem2.Tag = ItemType.AssignPres;
					barButtonItem2.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem2);
					BarButtonItem barButtonItem3 = new BarButtonItem(_BarManager, "Kê đơn tủ trực", 0);
					barButtonItem3.Tag = ItemType.AssignPresCabinet;
					barButtonItem3.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem3);
				}
				if (listMedicineAdo.TDL_SERVICE_TYPE_ID != 0 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 6 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 7 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 14)
				{
					BarButtonItem barButtonItem4 = new BarButtonItem(_BarManager, "In phiếu chỉ định", 0);
					barButtonItem4.Tag = ItemType.PrintServiceReq;
					barButtonItem4.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem4);
				}
				if (listMedicineAdo.TDL_SERVICE_TYPE_ID == 11 || listMedicineAdo.TDL_SERVICE_TYPE_ID == 4)
				{
					BarButtonItem barButtonItem5 = new BarButtonItem(_BarManager, "Chỉ định cùng kíp", 0);
					barButtonItem5.Tag = ItemType.AssignInKip;
					barButtonItem5.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem5);
					BarButtonItem barButtonItem6 = new BarButtonItem(_BarManager, "Chỉ định khác kíp", 0);
					barButtonItem6.Tag = ItemType.AssignOutKip;
					barButtonItem6.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list2.Add(barButtonItem6);
				}
				HIS_EXECUTE_ROOM hIS_EXECUTE_ROOM = BackendDataWorker.Get<HIS_EXECUTE_ROOM>().FirstOrDefault((HIS_EXECUTE_ROOM o) => o.ROOM_ID == _CurrentWorkPlace.RoomId);
				if (listMedicineAdo.TDL_REQUEST_ROOM_ID == _CurrentWorkPlace.RoomId && hIS_EXECUTE_ROOM != null)
				{
					short? iS_EXAM = hIS_EXECUTE_ROOM.IS_EXAM;
					if (iS_EXAM == 1 && iS_EXAM.HasValue && listMedicineAdo.TDL_SERVICE_TYPE_ID != 6 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 7 && listMedicineAdo.TDL_SERVICE_TYPE_ID != 14 && listMedicineAdo.IS_NO_EXECUTE != 1)
					{
						if (((HIS_SERE_SERV)listMedicineAdo).IS_ACCEPTING_NO_EXECUTE != 1 && list != null && list.Count > 0 && (list.FirstOrDefault().SERVICE_REQ_STT_ID == 1 || ((HIS_SERE_SERV)listMedicineAdo).IS_CONFIRM_NO_EXCUTE == 1))
						{
							BarButtonItem barButtonItem7 = new BarButtonItem(_BarManager, "Cho phép không thực hiện", 0);
							barButtonItem7.Tag = ItemType.AcceptNoExecute;
							barButtonItem7.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
							list2.Add(barButtonItem7);
						}
						else if (((HIS_SERE_SERV)listMedicineAdo).IS_ACCEPTING_NO_EXECUTE == 1)
						{
							BarButtonItem barButtonItem8 = new BarButtonItem(_BarManager, "Hủy cho phép không thực hiện", 0);
							barButtonItem8.Tag = ItemType.UnacceptNoExecute;
							barButtonItem8.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
							list2.Add(barButtonItem8);
						}
					}
				}
				_PopupMenu.AddItems(list2.ToArray());
				_PopupMenu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
