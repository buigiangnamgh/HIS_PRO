using System.Windows.Forms;
using DevExpress.XtraBars;
using HIS.Desktop.Common;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	internal class PopupMenuProcessor
	{
		internal enum ItemType
		{
			TrackingInMediRecord,
			CheckingTreatmentEmr
		}

		private BarManager _BarManager = null;

		internal PopupMenu _Menu = null;

		private MediRecordMouseRightClick _MouseRightClick;

		private MediRecordADO _MediRecorPoppupPrint;

		private RefeshReference BtnRefreshPhimTat;

		private TreatmentMouseRightClick _TreatMouseRightClick;

		private TreatmentADO _TreatmentPoppupPrint;

		internal PopupMenuProcessor(MediRecordADO currentMediRecor, BarManager barManager, MediRecordMouseRightClick mouseRightClick, RefeshReference _BtnRefreshPhimTat)
		{
			_BarManager = barManager;
			_MouseRightClick = mouseRightClick;
			_MediRecorPoppupPrint = currentMediRecor;
			BtnRefreshPhimTat = _BtnRefreshPhimTat;
		}

		internal PopupMenuProcessor(TreatmentADO currenttreatmentADO, BarManager barManager, TreatmentMouseRightClick treatMouseRightClick, RefeshReference _BtnRefreshPhimTat)
		{
			_BarManager = barManager;
			_TreatMouseRightClick = treatMouseRightClick;
			_TreatmentPoppupPrint = currenttreatmentADO;
			BtnRefreshPhimTat = _BtnRefreshPhimTat;
		}

		internal void InitMenu()
		{
			if (_BarManager != null && _MouseRightClick != null)
			{
				if (_Menu == null)
				{
					_Menu = new PopupMenu(_BarManager);
				}
				_Menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(_BarManager, "Tờ điều trị trong bệnh án", 0);
				barButtonItem.Tag = ItemType.TrackingInMediRecord;
				barButtonItem.ItemClick += new ItemClickEventHandler(_MouseRightClick.Invoke);
				_Menu.AddItems(new BarItem[1] { barButtonItem });
				_Menu.ShowPopup(Cursor.Position);
			}
		}

		internal void InitMenuTreat()
		{
			if (_BarManager != null && _TreatMouseRightClick != null)
			{
				if (_Menu == null)
				{
					_Menu = new PopupMenu(_BarManager);
				}
				_Menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(_BarManager, "Tra soát hồ sơ bệnh án", 0);
				barButtonItem.Tag = ItemType.CheckingTreatmentEmr;
				barButtonItem.ItemClick += new ItemClickEventHandler(_TreatMouseRightClick.Invoke);
				_Menu.AddItems(new BarItem[1] { barButtonItem });
				_Menu.ShowPopup(Cursor.Position);
			}
		}
	}
}
