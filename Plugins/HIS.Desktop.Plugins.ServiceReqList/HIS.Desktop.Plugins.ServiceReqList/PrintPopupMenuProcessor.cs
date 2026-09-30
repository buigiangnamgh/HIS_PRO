using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using EMR.EFMODEL.DataModels;
using EMR.Filter;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.IsAdmin;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.Plugins.ServiceReqList.ADO;
using HIS.Desktop.Plugins.ServiceReqList.Base;
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
	internal class PrintPopupMenuProcessor
	{
		internal enum ModuleType
		{
			thuocTongHop,
			donThuoc,
			donThuocYHCT,
			chuyenKhoa,
			kham,
			Mps000033,
			Mps000035,
			Mps000097,
			Mps000204,
			Mps000420,
			_testPhieuYeuCau,
			_testDomSoi,
			Edit,
			Delete,
			Print,
			EditIntruction,
			BieuMauKhac,
			EvenLog,
			ExamMain,
			GuiLaiXN,
			BieuMauKhacV2,
			DanhSachVanBanDaKy,
			SendOldSystemIntegration,
			OpenAttachFile,
			EnterInforBeforeSurgery,
			NoExecute,
			Execute,
			SampleInfo,
			TheBenhNhan,
			SampleType,
			PhieuThuKiemPhieuYcKham,
			ChangeRoom,
			AllowNotExecute,
			DisposeAllowNotExecute,
			HenKhamLai,
			DrugInterventionInfo,
			KetQuaHeThongBenhAnhDienTu,
			GiayDeNghiDoiTraDichVu,
			TaoPhieuYeuCauSuDungKhangSinh,
			HuyLayMau,
			ChuyenThanhDonTam,
			InVatTuTSD
		}

		private PrintMedicine_Click PrintMouseClick;

		private BarManager barManager;

		private PopupMenu menu;

		private ServiceReqADO ado;

		private long serviceReqSttId;

		private long serviceReqTypeId;

		private string loginName;

		internal long currentDepartmentId;

		internal V_HIS_ROOM currentRoom;

		internal PrintPopupMenuProcessor(PrintMedicine_Click PrintMouseClick, BarManager barManager, string _loginName)
		{
			this.PrintMouseClick = PrintMouseClick;
			this.barManager = barManager;
			loginName = _loginName;
		}

		internal PrintPopupMenuProcessor(PrintMedicine_Click PrintMouseClick, BarManager barManager, long _serviceReqTypeId, string _loginName)
		{
			this.PrintMouseClick = PrintMouseClick;
			this.barManager = barManager;
			serviceReqTypeId = _serviceReqTypeId;
			loginName = _loginName;
		}

		internal PrintPopupMenuProcessor(PrintMedicine_Click PrintMouseClick, BarManager barManager, long _sereServSTT, long _serviceReqTypeId, string _loginName)
		{
			this.PrintMouseClick = PrintMouseClick;
			this.barManager = barManager;
			serviceReqSttId = _sereServSTT;
			serviceReqTypeId = _serviceReqTypeId;
			loginName = _loginName;
		}

		internal PrintPopupMenuProcessor(PrintMedicine_Click PrintMouseClick, BarManager barManager, ServiceReqADO ado, string _loginName, V_HIS_ROOM _currentRoom = null)
		{
			this.PrintMouseClick = PrintMouseClick;
			this.barManager = barManager;
			this.ado = ado;
			loginName = _loginName;
			currentRoom = _currentRoom;
		}

		internal void RightMenu()
		{
			try
			{
				if (menu == null)
				{
					menu = new PopupMenu(barManager);
				}
				menu.ItemLinks.Clear();
				if ((ado.CREATOR == loginName || ado.REQUEST_LOGINNAME == loginName) && ((ado.SERVICE_REQ_STT_ID == 1 && ado.IS_NO_EXECUTE != 1) || (ado.SERVICE_REQ_STT_ID == 2 && HisConfigs.Get<string>("MOS.HIS_SERVICE_REQ.ALLOW_MODIFYING_OF_STARTED") == "1")))
				{
					BarButtonItem barButtonItem = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.Edit", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
					barButtonItem.Tag = ModuleType.Edit;
					barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem });
				}
				if ((ado.CREATOR == loginName || ado.REQUEST_LOGINNAME == loginName || ado.DeleteCheck) && ado.SERVICE_REQ_STT_ID == 1)
				{
					BarButtonItem barButtonItem2 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.Delete", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 2);
					barButtonItem2.Tag = ModuleType.Delete;
					barButtonItem2.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem2 });
				}
				BarButtonItem barButtonItem3 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.Print", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 3);
				barButtonItem3.Tag = ModuleType.Print;
				barButtonItem3.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem3 });
				if (!ado.IS_MAIN_EXAM.HasValue && ado.SERVICE_REQ_TYPE_ID == 1 && CheckLoginAdmin.IsAdmin(loginName))
				{
					BarButtonItem barButtonItem4 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.MainExam", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 4);
					barButtonItem4.Tag = ModuleType.ExamMain;
					barButtonItem4.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem4 });
				}
				if (ado.SERVICE_REQ_STT_ID == 3 || ado.SERVICE_REQ_STT_ID == 2)
				{
					BarButtonItem barButtonItem5 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.KetQuaHeThongBenhAnhDienTu", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 0);
					barButtonItem5.Tag = ModuleType.KetQuaHeThongBenhAnhDienTu;
					barButtonItem5.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem5 });
				}
				BarButtonItem barButtonItem6 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.EditCommonInfo", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 4);
				barButtonItem6.Tag = ModuleType.EditIntruction;
				barButtonItem6.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem6 });
				if (ado != null && !string.IsNullOrEmpty(ado.JSON_PRINT_ID))
				{
					BarButtonItem barButtonItem7 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.OtherPrint", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 5);
					barButtonItem7.Tag = ModuleType.BieuMauKhac;
					barButtonItem7.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem7 });
				}
				BarButtonItem barButtonItem8 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.OtherPrintByRequest", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 5);
				barButtonItem8.Tag = ModuleType.BieuMauKhacV2;
				barButtonItem8.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem8 });
				BarButtonItem barButtonItem9 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.EmrDocumentList", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 5);
				barButtonItem9.Tag = ModuleType.DanhSachVanBanDaKy;
				barButtonItem9.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem9 });
				BarButtonItem barButtonItem10 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.EventLog", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 6);
				barButtonItem10.Tag = ModuleType.EvenLog;
				barButtonItem10.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem10 });
				if (ado != null && (ado.SERVICE_REQ_TYPE_ID == 2 || ado.SERVICE_REQ_TYPE_ID == 3 || ado.SERVICE_REQ_TYPE_ID == 9 || ado.SERVICE_REQ_TYPE_ID == 8 || ado.SERVICE_REQ_TYPE_ID == 5 || ado.SERVICE_REQ_TYPE_ID == 13 || ado.SERVICE_REQ_TYPE_ID == 4 || ado.SERVICE_REQ_TYPE_ID == 10 || ado.SERVICE_REQ_TYPE_ID == 12 || ado.SERVICE_REQ_TYPE_ID == 11))
				{
					BarButtonItem barButtonItem11 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.ReSendAssignToLIS", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 7);
					barButtonItem11.Tag = ModuleType.GuiLaiXN;
					barButtonItem11.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem11 });
				}
				if (ado != null && HisConfigCFG.IsOldSystemIntegration && CheckLoginAdmin.IsAdmin(loginName))
				{
					BarButtonItem barButtonItem12 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.SendAssignToOldSystem", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 8);
					barButtonItem12.Tag = ModuleType.SendOldSystemIntegration;
					barButtonItem12.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem12 });
				}
				if (ado != null && !string.IsNullOrEmpty(ado.ATTACHMENT_FILE_URL))
				{
					BarButtonItem barButtonItem13 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.AttackTreatment", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 9);
					barButtonItem13.Tag = ModuleType.OpenAttachFile;
					barButtonItem13.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem13 });
				}
				if (ado != null && ado.AddInforPTTT)
				{
					BarButtonItem barButtonItem14 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.InputBeforeSurg", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 9);
					barButtonItem14.Tag = ModuleType.EnterInforBeforeSurgery;
					barButtonItem14.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem14 });
				}
				if (ado.SERVICE_REQ_STT_ID != 3 && (ado.SERVICE_REQ_TYPE_ID == 6 || ado.SERVICE_REQ_TYPE_ID == 14) && ado.SERVICE_REQ_TYPE_ID != 15)
				{
					LogSystem.Debug("RightMenu____" + LogUtil.TraceData(LogUtil.GetMemberName(() => ado), ado));
					BarButtonItem barButtonItem = new BarButtonItem(barManager, (ado.IS_NO_EXECUTE == 1) ? Get.Value("ServiceReq.RightMenu.Execute", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()) : Get.Value("ServiceReq.RightMenu.NoExecute", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
					barButtonItem.Tag = ((ado.IS_NO_EXECUTE == 1) ? ModuleType.Execute : ModuleType.NoExecute);
					barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem });
				}
				if (ado.SERVICE_REQ_TYPE_ID == 6 || ado.SERVICE_REQ_TYPE_ID == 15 || ado.SERVICE_REQ_TYPE_ID == 14)
				{
					BarButtonItem barButtonItem15 = new BarButtonItem(barManager, "Tạo phiếu yêu cầu sử dụng kháng sinh", 9);
					barButtonItem15.Tag = ModuleType.TaoPhieuYeuCauSuDungKhangSinh;
					barButtonItem15.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem15 });
				}
				if (ado.SERVICE_REQ_TYPE_ID == 2 && (!ado.IS_NO_EXECUTE.HasValue || ado.IS_NO_EXECUTE.Value != 1) && ado.REQUEST_DEPARTMENT_ID == currentDepartmentId && !((HIS_SERVICE_REQ)ado).SAMPLE_TIME.HasValue)
				{
					BarButtonItem barButtonItem16 = new BarButtonItem(barManager, "Lấy mẫu bệnh phẩm", 10);
					barButtonItem16.Tag = ModuleType.SampleInfo;
					barButtonItem16.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem16 });
				}
				if (ado.SERVICE_REQ_TYPE_ID == 2 && ado.REQUEST_DEPARTMENT_ID == currentDepartmentId && ((HIS_SERVICE_REQ)ado).SAMPLE_TIME.HasValue && !HisConfigCFG.IsUseInventecLis)
				{
					BarButtonItem barButtonItem17 = new BarButtonItem(barManager, "Hủy lấy mẫu", 11);
					barButtonItem17.Tag = ModuleType.HuyLayMau;
					barButtonItem17.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem17 });
				}
				if (ado.SERVICE_REQ_TYPE_ID == 2)
				{
					BarButtonItem barButtonItem18 = new BarButtonItem(barManager, "Cập nhật loại bệnh phẩm", 0);
					barButtonItem18.Tag = ModuleType.SampleType;
					barButtonItem18.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem18 });
				}
				BarButtonItem barButtonItem19 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.ChangeRoom", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
				barButtonItem19.Tag = ModuleType.ChangeRoom;
				barButtonItem19.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem19 });
				if ((ado.SERVICE_REQ_TYPE_ID == 6 || ado.SERVICE_REQ_TYPE_ID == 14 || ado.SERVICE_REQ_TYPE_ID == 15) && HisConfigCFG.ConnectDrugInterventionInfo)
				{
					BarButtonItem barButtonItem20 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.ConnectDrugInterventionInfo", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
					barButtonItem20.Tag = ModuleType.DrugInterventionInfo;
					barButtonItem20.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem20 });
				}
				List<HIS_ROOM> list = BackendDataWorker.Get<HIS_ROOM>();
				HIS_ROOM hIS_ROOM = ((list != null && currentRoom != null) ? list.FirstOrDefault((HIS_ROOM o) => o.ID == currentRoom.ID) : null);
				if (currentRoom != null && ado.REQUEST_ROOM_ID == currentRoom.ID && hIS_ROOM != null && hIS_ROOM.ROOM_TYPE_ID != 4 && ado.SERVICE_REQ_TYPE_ID != 16 && ado.IS_NO_EXECUTE != 1 && ado.SERVICE_REQ_STT_ID == 1)
				{
					if (((HIS_SERVICE_REQ)ado).IS_ACCEPTING_NO_EXECUTE != 1)
					{
						BarButtonItem barButtonItem21 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.AllowNotExecute", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
						barButtonItem21.Tag = ModuleType.AllowNotExecute;
						barButtonItem21.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
						menu.AddItems(new BarItem[1] { barButtonItem21 });
					}
					else
					{
						BarButtonItem barButtonItem22 = new BarButtonItem(barManager, Get.Value("ServiceReq.RightMenu.DisposeAllowNotExecute", ResourceLanguageManager.LanguagefrmServiceReqList, LanguageManager.GetCulture()), 1);
						barButtonItem22.Tag = ModuleType.DisposeAllowNotExecute;
						barButtonItem22.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
						menu.AddItems(new BarItem[1] { barButtonItem22 });
					}
				}
				BarButtonItem barButtonItem23 = new BarButtonItem(barManager, "Giấy đề nghị đổi trả dịch vụ", 11);
				barButtonItem23.Tag = ModuleType.GiayDeNghiDoiTraDichVu;
				barButtonItem23.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem23 });
				if ((CheckLoginAdmin.IsAdmin(loginName) || ado.REQUEST_LOGINNAME.Equals(loginName)) && ado.SERVICE_REQ_TYPE_ID == 15 && LoadDataToCurrentTreatmentData(ado.TREATMENT_ID).IS_PAUSE != 1)
				{
					EMR_TREATMENT eMR_TREATMENT = LoadDataToCurrentEmrTreatmentData(ado.TDL_TREATMENT_CODE);
					if (eMR_TREATMENT == null || !eMR_TREATMENT.STORE_TIME.HasValue || eMR_TREATMENT.STORE_TIME == 0)
					{
						BarButtonItem barButtonItem24 = new BarButtonItem(barManager, "Chuyển thành đơn tạm", 11);
						barButtonItem24.Tag = ModuleType.ChuyenThanhDonTam;
						barButtonItem24.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
						menu.AddItems(new BarItem[1] { barButtonItem24 });
					}
				}
				if (ado != null && (ado.SERVICE_REQ_TYPE_ID == 6 || ado.SERVICE_REQ_TYPE_ID == 15 || ado.SERVICE_REQ_TYPE_ID == 14))
				{
					BarButtonItem barButtonItem25 = new BarButtonItem(barManager, "In tem vật tư tái sử dụng", 11);
					barButtonItem25.Tag = ModuleType.InVatTuTSD;
					barButtonItem25.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem25 });
				}
				menu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private HIS_TREATMENT LoadDataToCurrentTreatmentData(long treatmentId)
		{
			HIS_TREATMENT result = null;
			try
			{
				CommonParam commonParam = new CommonParam();
				HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
				hisTreatmentFilter.ID = treatmentId;
				List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", ApiConsumers.MosConsumer, hisTreatmentFilter, new Action(SessionManager.ActionLostToken), commonParam);
				if (list != null && list.Count > 0)
				{
					result = list[0];
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private EMR_TREATMENT LoadDataToCurrentEmrTreatmentData(string treatmentCode)
		{
			EMR_TREATMENT result = null;
			try
			{
				CommonParam commonParam = new CommonParam();
				EmrTreatmentFilter emrTreatmentFilter = new EmrTreatmentFilter();
				emrTreatmentFilter.TREATMENT_CODE__EXACT = treatmentCode;
				List<EMR_TREATMENT> list = new BackendAdapter(commonParam).Get<List<EMR_TREATMENT>>("api/EmrTreatment/Get", ApiConsumers.EmrConsumer, emrTreatmentFilter, new Action(SessionManager.ActionLostToken), commonParam);
				if (list != null && list.Count > 0)
				{
					result = list[0];
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal void InitMenu()
		{
			try
			{
				if (menu == null)
				{
					menu = new PopupMenu(barManager);
				}
				menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__DON_THUOC", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 1);
				barButtonItem.Tag = ModuleType.donThuoc;
				barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem });
				BarButtonItem barButtonItem2 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__THUOC_TONG_HOP", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem2.Tag = ModuleType.thuocTongHop;
				barButtonItem2.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem2 });
				BarButtonItem barButtonItem3 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__DON_THUOC_YHCT", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 3);
				barButtonItem3.Tag = ModuleType.donThuocYHCT;
				barButtonItem3.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem3 });
				menu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void InitMenuKham(ServiceReqADO ServiceReqADO)
		{
			try
			{
				if (menu == null)
				{
					menu = new PopupMenu(barManager);
				}
				menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__KHAM", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 1);
				barButtonItem.Tag = ModuleType.kham;
				barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem });
				BarButtonItem barButtonItem2 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__PHIEU_THU_KIEM_YC_KHAM", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem2.Tag = ModuleType.PhieuThuKiemPhieuYcKham;
				barButtonItem2.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem2 });
				BarButtonItem barButtonItem3 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__CHUYEN_KHOA", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem3.Tag = ModuleType.chuyenKhoa;
				barButtonItem3.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem3 });
				BarButtonItem barButtonItem4 = new BarButtonItem(barManager, "In thẻ bệnh nhân", 3);
				barButtonItem4.Tag = ModuleType.TheBenhNhan;
				barButtonItem4.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem4 });
				if (ServiceReqADO.SERVICE_REQ_STT_ID == 3 && ((HIS_SERVICE_REQ)ServiceReqADO).APPOINTMENT_TIME.HasValue)
				{
					BarButtonItem barButtonItem5 = new BarButtonItem(barManager, "In phiếu hẹn khám lại", 3);
					barButtonItem5.Tag = ModuleType.HenKhamLai;
					barButtonItem5.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem5 });
				}
				menu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void InitMenuPttt()
		{
			try
			{
				if (menu == null)
				{
					menu = new PopupMenu(barManager);
				}
				menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__PHIEU_PHAU_THUAT", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 1);
				barButtonItem.Tag = ModuleType.Mps000033;
				barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem });
				BarButtonItem barButtonItem2 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__GIAY_CAM_DOAN", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem2.Tag = ModuleType.Mps000035;
				barButtonItem2.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem2 });
				BarButtonItem barButtonItem3 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__CACH_THUC_PHAU_THUAT", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem3.Tag = ModuleType.Mps000097;
				barButtonItem3.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem3 });
				if (serviceReqSttId == 3 && serviceReqTypeId != 4)
				{
					BarButtonItem barButtonItem4 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__GIAY_CHUNG_NHAN_PHAU_THUAT", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
					barButtonItem4.Tag = ModuleType.Mps000204;
					barButtonItem4.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
					menu.AddItems(new BarItem[1] { barButtonItem4 });
				}
				menu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void InitMenuXetNghiem()
		{
			try
			{
				if (menu == null)
				{
					menu = new PopupMenu(barManager);
				}
				menu.ItemLinks.Clear();
				BarButtonItem barButtonItem = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__PHIEU_YEU_CAU_XET_NGHIEM", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 1);
				barButtonItem.Tag = ModuleType._testPhieuYeuCau;
				barButtonItem.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem });
				BarButtonItem barButtonItem2 = new BarButtonItem(barManager, Get.Value("IVT_LANGUAGE_KEY__FORM_SERVICE_REQ_LIST__CLICK__BIEU_MAU_KHAC", ResourceLangManager.LanguageFrmServiceReqList, LanguageManager.GetCulture()), 2);
				barButtonItem2.Tag = ModuleType._testDomSoi;
				barButtonItem2.ItemClick += new ItemClickEventHandler(PrintMouseClick.Invoke);
				menu.AddItems(new BarItem[1] { barButtonItem2 });
				menu.ShowPopup(Cursor.Position);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
