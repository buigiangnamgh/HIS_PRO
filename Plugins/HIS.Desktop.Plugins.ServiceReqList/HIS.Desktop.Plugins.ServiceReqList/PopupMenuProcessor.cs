using System;
using System.Windows.Forms;
using DevExpress.XtraBars;
using HIS.Desktop.Plugins.ServiceReqList.Base;
using Inventec.Common.LocalStorage.SdaConfig;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.ServiceReqList
{
	internal class PopupMenuProcessor
	{
		internal enum ItemType
		{
			PhieuThuThanhToan,
			PhieuTamUng,
			PhieuHoanUng,
			HoaDonTTTheoYeuCauDichVu,
			HoaDonTTChiTietDichVu,
			PhieuChiDinh,
			BienLaiPhiLePhi,
			PhieuThuPhiDichVu
		}

		private V_HIS_TRANSACTION _Transaction = null;

		private BarManager _BarManager = null;

		private PopupMenu _PopupMenu = null;

		private TransactionMouseRightClick _MouseRightClick;

		internal PopupMenuProcessor(V_HIS_TRANSACTION transaction, BarManager barmanager, TransactionMouseRightClick mouseRightClick)
		{
			_Transaction = transaction;
			_MouseRightClick = mouseRightClick;
			_BarManager = barmanager;
		}

		internal void InitMenu()
		{
			try
			{
				if (_Transaction == null || _BarManager == null || _MouseRightClick == null)
				{
					return;
				}
				if (_Transaction.IS_CANCEL == 1)
				{
					LogSystem.Info("giao dich da bi huy: " + LogUtil.TraceData(LogUtil.GetMemberName(() => _Transaction), _Transaction));
					return;
				}
				if (_PopupMenu == null)
				{
					_PopupMenu = new PopupMenu(_BarManager);
				}
				_PopupMenu.ItemLinks.Clear();
				if (_Transaction.TRANSACTION_TYPE_CODE == SdaConfigs.Get<string>("DBCODE.HIS_RS.HIS_TRANSACTION_TYPE.TRANSACTION_TYPE_CODE.BILL"))
				{
					BarButtonItem barButtonItem = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_PHIEUTHUTHANHTOAN", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem.Tag = ItemType.PhieuThuThanhToan;
					barButtonItem.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					BarButtonItem barButtonItem2 = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_HOADONTHANHTOANTHEOYEUCAUDICHVU", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 1);
					barButtonItem2.Tag = ItemType.HoaDonTTTheoYeuCauDichVu;
					barButtonItem2.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					BarButtonItem barButtonItem3 = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_BIENLAITHUPHILEPHI", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 3);
					barButtonItem3.Tag = ItemType.BienLaiPhiLePhi;
					barButtonItem3.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					BarButtonItem barButtonItem4 = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_PHIEUCHIDINH", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 4);
					barButtonItem4.Tag = ItemType.PhieuChiDinh;
					barButtonItem4.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					_PopupMenu.AddItems(new BarItem[4] { barButtonItem, barButtonItem2, barButtonItem3, barButtonItem4 });
					_PopupMenu.ShowPopup(Cursor.Position);
				}
				else if (_Transaction.TRANSACTION_TYPE_CODE == SdaConfigs.Get<string>("DBCODE.HIS_RS.HIS_TRANSACTION_TYPE.TRANSACTION_TYPE_CODE.DEPOSIT"))
				{
					BarButtonItem barButtonItem5 = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_PHIEUTAMUNG", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem5.Tag = ItemType.PhieuTamUng;
					barButtonItem5.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					_PopupMenu.AddItems(new BarItem[1] { barButtonItem5 });
					_PopupMenu.ShowPopup(Cursor.Position);
				}
				else if (_Transaction.TRANSACTION_TYPE_CODE == SdaConfigs.Get<string>("DBCODE.HIS_RS.HIS_TRANSACTION_TYPE.TRANSACTION_TYPE_CODE.REPAY"))
				{
					BarButtonItem barButtonItem6 = new BarButtonItem(_BarManager, Get.Value("IVT_LANGUAGE_KEY__FRM_TRANSACTION_LIST__POPUP_MENU__ITEM_PHIEUHOANUNG", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem6.Tag = ItemType.PhieuTamUng;
					barButtonItem6.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
					_PopupMenu.AddItems(new BarItem[1] { barButtonItem6 });
					_PopupMenu.ShowPopup(Cursor.Position);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}
	}
}
