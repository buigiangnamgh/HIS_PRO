using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.IsAdmin;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using HIS.Desktop.Plugins.ServiceReqList.Resources;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class PopupMenuProcessorCheck
	{
		internal enum ItemType
		{
			In,
			Xoa,
			ChuyenPhong,
			InKemKetQua,
			KetQuaHeThongBenhAnhDienTu
		}

		private BarManager _BarManager = null;

		private MouseRightClick _MouseRightClick;

		private PopupMenu _PopupMenu = null;

		private List<ServiceReqADO> ListAdos;

		private V_HIS_ROOM currentRoom;

		private string loginName = null;

		private List<ServiceReqADO> lstSerSelected = null;

		internal PopupMenuProcessorCheck(BarManager barmanager, MouseRightClick mouseRightClick, List<ServiceReqADO> listAdos, string loginname, V_HIS_ROOM currentRoom)
		{
			_MouseRightClick = mouseRightClick;
			_BarManager = barmanager;
			ListAdos = listAdos;
			loginName = loginname;
			this.currentRoom = currentRoom;
		}

		internal PopupMenuProcessorCheck(BarManager barmanager, MouseRightClick mouseRightClick, List<ServiceReqADO> listAdos, string loginname, V_HIS_ROOM currentRoom, List<ServiceReqADO> _lstSerSelected)
		{
			_MouseRightClick = mouseRightClick;
			_BarManager = barmanager;
			ListAdos = listAdos;
			loginName = loginname;
			this.currentRoom = currentRoom;
			lstSerSelected = _lstSerSelected;
		}

		internal void InitMenu()
		{
			try
			{
				if (ListAdos == null || ListAdos.Count == 0 || _BarManager == null || _MouseRightClick == null)
				{
					return;
				}
				if (_PopupMenu == null)
				{
					_PopupMenu = new PopupMenu(_BarManager);
				}
				_PopupMenu.ItemLinks.Clear();
				List<BarItem> list = new List<BarItem>();
				BarButtonItem barButtonItem = new BarButtonItem(_BarManager, Get.Value("ServiceReq.RightMenu.PrintSelectedItem", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 0);
				barButtonItem.Tag = ItemType.In;
				barButtonItem.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
				list.Add(barButtonItem);
				List<HIS_SERVICE_REQ> source = new List<HIS_SERVICE_REQ>();
				if (lstSerSelected != null && lstSerSelected.Count() > 0)
				{
					HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
					hisServiceReqFilter.IDs = lstSerSelected.Select((ServiceReqADO o) => o.ID).ToList();
					source = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", ApiConsumers.MosConsumer, hisServiceReqFilter, new CommonParam());
				}
				IEnumerable<HIS_SERVICE_REQ> source2 = source.Where((HIS_SERVICE_REQ o) => o.SERVICE_REQ_STT_ID == 1);
				if (source2.Count() == 0)
				{
					BarButtonItem barButtonItem2 = new BarButtonItem(_BarManager, Get.Value("ServiceReq.RightMenu.KetQuaHeThongBenhAnhDienTu", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem2.Tag = ItemType.KetQuaHeThongBenhAnhDienTu;
					barButtonItem2.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list.Add(barButtonItem2);
				}
				bool flag = true;
				foreach (ServiceReqADO listAdo in ListAdos)
				{
					if ((!(listAdo.CREATOR == loginName) && !(listAdo.REQUEST_LOGINNAME == loginName) && !CheckLoginAdmin.IsAdmin(loginName) && (currentRoom == null || listAdo.REQUEST_DEPARTMENT_ID != currentRoom.DEPARTMENT_ID || listAdo.SERVICE_REQ_TYPE_ID != 1)) || listAdo.SERVICE_REQ_STT_ID != 1)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					BarButtonItem barButtonItem3 = new BarButtonItem(_BarManager, Get.Value("ServiceReq.RightMenu.DeleteSelectedItem", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem3.Tag = ItemType.Xoa;
					barButtonItem3.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					list.Add(barButtonItem3);
				}
				BarButtonItem barButtonItem4 = new BarButtonItem(_BarManager, "In phiếu chỉ định kèm phiếu kết quả", 0);
				barButtonItem4.Tag = ItemType.InKemKetQua;
				barButtonItem4.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
				list.Add(barButtonItem4);
				BarButtonItem barButtonItem5 = new BarButtonItem(_BarManager, Get.Value("ServiceReq.RightMenu.ChangeRoom", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 0);
				barButtonItem5.Tag = ItemType.ChuyenPhong;
				barButtonItem5.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
				list.Add(barButtonItem5);
				_PopupMenu.AddItems(list.ToArray());
				_PopupMenu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
