using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraLayout;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Utility;
using His.UC.LibraryMessage;
using HIS.UC.UCOtherServiceReqInfo.Resources;
using HIS.UC.UCOtherServiceReqInfo.Valid;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.TypeConvert;
using Inventec.Desktop.Common.LanguageManager;
using MOS.EFMODEL.DataModels;

namespace HIS.UC.UCOtherServiceReqInfo.FUN
{
	public class frmFun : FormBase
	{
		public delegate void GetString(HIS_TREATMENT hisTreatment);

		public GetString MyGetData;

		private int positionHandle = -1;

		private HIS_TREATMENT _HisTreatment;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private SimpleButton btnSave;

		private DateEdit dtThoiHanDen;

		private DateEdit dtThoiHanTu;

		private DateEdit dtNgayCap;

		private TextEdit txtCongTy;

		private SpinEdit spinHanMuc;

		private TextEdit txtTenKhachHang;

		private TextEdit txtSanPham;

		private TextEdit txtSoThe;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private LayoutControlItem layoutControlItem6;

		private LayoutControlItem layoutControlItem7;

		private LayoutControlItem layoutControlItem8;

		private LayoutControlItem layoutControlItem9;

		private EmptySpaceItem emptySpaceItem1;

		private LabelControl labelControl1;

		private LayoutControlItem layoutControlItem10;

		private DXValidationProvider dxValidationProvider1;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItem__Save;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private SimpleButton btnAddInFor;

		private LayoutControlItem layoutControlItem11;

		public frmFun()
		{
			InitializeComponent();
		}

		public frmFun(HIS_TREATMENT _hisTreatment)
		{
			InitializeComponent();
			try
			{
				_HisTreatment = _hisTreatment;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void frmFun_Load(object sender, EventArgs e)
		{
			try
			{
				SetCaptionByLanguageKey();
				SetIcon();
				SetData();
				ValidTextControlMaxlength(txtSoThe, 255, true);
				ValidTextControlMaxlength(txtTenKhachHang, 200, false);
				ValidTextControlMaxlength(txtCongTy, 200, false);
				ValidTextControlMaxlength(txtSanPham, 200, false);
				txtSoThe.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.UC.UCOtherServiceReqInfo.Resources.Lang", typeof(frmFun).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControl1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnAddInFor.ToolTip = Inventec.Common.Resource.Get.Value("frmFun.btnAddInFor.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("frmFun.btnSave.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem2.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem2.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem6.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem6.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem7.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem7.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem8.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem8.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem4.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem4.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem10.Text = Inventec.Common.Resource.Get.Value("frmFun.layoutControlItem10.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("frmFun.bar1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				barButtonItem__Save.Caption = Inventec.Common.Resource.Get.Value("frmFun.barButtonItem__Save.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmFun.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetData()
		{
			try
			{
				txtSoThe.Text = _HisTreatment.FUND_NUMBER;
				decimal? fUND_BUDGET = _HisTreatment.FUND_BUDGET;
				if ((fUND_BUDGET.GetValueOrDefault() > default(decimal)) & fUND_BUDGET.HasValue)
				{
					spinHanMuc.Value = _HisTreatment.FUND_BUDGET.GetValueOrDefault();
				}
				else
				{
					spinHanMuc.EditValue = null;
				}
				txtCongTy.Text = _HisTreatment.FUND_COMPANY_NAME;
				if (_HisTreatment.FUND_FROM_TIME > 0)
				{
					dtThoiHanTu.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(_HisTreatment.FUND_FROM_TIME.GetValueOrDefault()) ?? DateTime.Now;
				}
				if (_HisTreatment.FUND_TO_TIME > 0)
				{
					dtThoiHanDen.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(_HisTreatment.FUND_TO_TIME.GetValueOrDefault()) ?? DateTime.Now;
				}
				if (_HisTreatment.FUND_ISSUE_TIME > 0)
				{
					dtNgayCap.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(_HisTreatment.FUND_ISSUE_TIME.GetValueOrDefault()) ?? DateTime.Now;
				}
				txtSanPham.Text = _HisTreatment.FUND_TYPE_NAME;
				txtTenKhachHang.Text = _HisTreatment.FUND_CUSTOMER_NAME;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetIcon()
		{
			try
			{
				string filePath = Path.Combine(ApplicationStoreLocation.ApplicationStartupPath, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
				base.Icon = Icon.ExtractAssociatedIcon(filePath);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidTextControlMaxlength(TextEdit control, int maxlength, bool isVali)
		{
			try
			{
				TextEditMaxLengthValidationRule textEditMaxLengthValidationRule = new TextEditMaxLengthValidationRule();
				textEditMaxLengthValidationRule.txtEdit = control;
				textEditMaxLengthValidationRule.maxlength = maxlength;
				textEditMaxLengthValidationRule.isVali = isVali;
				textEditMaxLengthValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, textEditMaxLengthValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidationTime(DateEdit dtTime)
		{
			try
			{
				TimeValidationRule timeValidationRule = new TimeValidationRule();
				timeValidationRule.dtTime = dtTime;
				timeValidationRule.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				timeValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(dtTime, timeValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationText(TextEdit txtText)
		{
			try
			{
				TextValidationRule textValidationRule = new TextValidationRule();
				textValidationRule.txtText = txtText;
				textValidationRule.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				textValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtText, textValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationSpin(SpinEdit spinEdit)
		{
			try
			{
				SpinValidationRule spinValidationRule = new SpinValidationRule();
				spinValidationRule.spinEdit = spinEdit;
				spinValidationRule.ErrorText = MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				spinValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(spinEdit, spinValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			try
			{
				btnSave.Focus();
				positionHandle = -1;
				if (dxValidationProvider1.Validate())
				{
					HIS_TREATMENT val = new HIS_TREATMENT();
					val.FUND_NUMBER = txtSoThe.Text;
					val.FUND_BUDGET = spinHanMuc.Value;
					val.FUND_COMPANY_NAME = txtCongTy.Text;
					if (dtThoiHanTu.EditValue != null && dtThoiHanTu.DateTime != DateTime.MinValue)
					{
						val.FUND_FROM_TIME = Parse.ToInt64(System.Convert.ToDateTime(dtThoiHanTu.EditValue).ToString("yyyyMMdd") + "000000");
					}
					if (dtThoiHanDen.EditValue != null && dtThoiHanDen.DateTime != DateTime.MinValue)
					{
						val.FUND_TO_TIME = Parse.ToInt64(System.Convert.ToDateTime(dtThoiHanDen.EditValue).ToString("yyyyMMdd") + "000000");
					}
					if (dtNgayCap.EditValue != null && dtNgayCap.DateTime != DateTime.MinValue)
					{
						val.FUND_ISSUE_TIME = Parse.ToInt64(System.Convert.ToDateTime(dtNgayCap.EditValue).ToString("yyyyMMdd") + "000000");
					}
					val.FUND_TYPE_NAME = txtSanPham.Text;
					val.FUND_CUSTOMER_NAME = txtTenKhachHang.Text;
					MyGetData(val);
					Close();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void barButtonItem__Save_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				btnSave_Click(null, null);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dxValidationProvider1_ValidationFailed(object sender, ValidationFailedEventArgs e)
		{
			try
			{
				BaseEdit baseEdit = e.InvalidControl as BaseEdit;
				if (baseEdit == null)
				{
					return;
				}
				BaseEditViewInfo baseEditViewInfo = baseEdit.GetViewInfo() as BaseEditViewInfo;
				if (baseEditViewInfo == null)
				{
					return;
				}
				if (positionHandle == -1)
				{
					positionHandle = baseEdit.TabIndex;
					if (baseEdit.Visible)
					{
						baseEdit.SelectAll();
						baseEdit.Focus();
					}
				}
				if (positionHandle > baseEdit.TabIndex)
				{
					positionHandle = baseEdit.TabIndex;
					if (baseEdit.Visible)
					{
						baseEdit.SelectAll();
						baseEdit.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtSoThe_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtSanPham.Focus();
					txtSanPham.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtSanPham_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtThoiHanTu.Focus();
					dtThoiHanTu.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtTenKhachHang_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtCongTy.Focus();
					txtCongTy.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void spinHanMuc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSave.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtCongTy_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtNgayCap.Focus();
					dtNgayCap.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtNgayCap_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					spinHanMuc.Focus();
					spinHanMuc.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtNgayCap_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					spinHanMuc.Focus();
					spinHanMuc.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtThoiHanTu_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtThoiHanDen.Focus();
					dtThoiHanDen.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtThoiHanTu_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtThoiHanDen.Focus();
					dtThoiHanDen.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtThoiHanDen_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					txtTenKhachHang.Focus();
					txtTenKhachHang.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtThoiHanDen_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtTenKhachHang.Focus();
					txtTenKhachHang.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnAddInFor_Click(object sender, EventArgs e)
		{
			try
			{
				string value = "";
				if (!string.IsNullOrEmpty(txtSoThe.Text))
				{
					string text = HisConfigs.Get<string>("HIS.Desktopn.UCOtherServiceReqInfo.FUN.Value_According_To_Card _Number");
					if (string.IsNullOrEmpty(text))
					{
						value = "Không tìm thấy giá trị hạn mức với số thẻ " + txtSoThe.Text;
						LogSystem.Error("--- Giá trị key HIS.Desktopn.UCOtherServiceReqInfo.FUN.Value_According_To_Card _Number ---------- null");
					}
					else
					{
						string[] array = text.Split(',');
						string text2 = "";
						string[] array2 = array;
						foreach (string text3 in array2)
						{
							string[] array3 = text3.Split(':');
							if (txtSoThe.Text.Trim() == array3[0].Trim())
							{
								text2 = array3[1].Trim();
								break;
							}
						}
						if (string.IsNullOrEmpty(text2))
						{
							value = "Không tìm thấy giá trị hạn mức với số thẻ " + txtSoThe.Text;
						}
						else
						{
							spinHanMuc.Value = Parse.ToDecimal(text2);
						}
					}
				}
				else
				{
					value = "Số thẻ không được để trống";
				}
				if (!string.IsNullOrEmpty(value))
				{
					XtraMessageBox.Show(value, "Thông báo");
					return;
				}
				txtSanPham.Text = "Ba\u0309o Viê\u0323t An Gia";
				dtThoiHanTu.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(20190101000000L).GetValueOrDefault();
				dtThoiHanDen.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(20191231000000L).GetValueOrDefault();
				dtNgayCap.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(20190101000000L).GetValueOrDefault();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public override void ProcessDisposeModuleDataAfterClose()
		{
			try
			{
				_HisTreatment = null;
				positionHandle = 0;
				MyGetData = null;
				btnAddInFor.Click -= btnAddInFor_Click;
				btnSave.Click -= btnSave_Click;
				dtThoiHanDen.Closed -= dtThoiHanDen_Closed;
				dtThoiHanDen.KeyDown -= dtThoiHanDen_KeyDown;
				dtThoiHanTu.Closed -= dtThoiHanTu_Closed;
				dtThoiHanTu.KeyDown -= dtThoiHanTu_KeyDown;
				dtNgayCap.Closed -= dtNgayCap_Closed;
				dtNgayCap.KeyDown -= dtNgayCap_KeyDown;
				txtCongTy.PreviewKeyDown -= txtCongTy_PreviewKeyDown;
				spinHanMuc.PreviewKeyDown -= spinHanMuc_PreviewKeyDown;
				txtTenKhachHang.PreviewKeyDown -= txtTenKhachHang_PreviewKeyDown;
				txtSanPham.PreviewKeyDown -= txtSanPham_PreviewKeyDown;
				txtSoThe.PreviewKeyDown -= txtSoThe_PreviewKeyDown;
				dxValidationProvider1.ValidationFailed -= dxValidationProvider1_ValidationFailed;
				barButtonItem__Save.ItemClick -= barButtonItem__Save_ItemClick;
				base.Load -= frmFun_Load;
				layoutControlItem11 = null;
				btnAddInFor = null;
				barDockControlRight = null;
				barDockControlLeft = null;
				barDockControlBottom = null;
				barDockControlTop = null;
				barButtonItem__Save = null;
				bar1 = null;
				barManager1 = null;
				dxValidationProvider1 = null;
				layoutControlItem10 = null;
				labelControl1 = null;
				emptySpaceItem1 = null;
				layoutControlItem9 = null;
				layoutControlItem8 = null;
				layoutControlItem7 = null;
				layoutControlItem6 = null;
				layoutControlItem5 = null;
				layoutControlItem4 = null;
				layoutControlItem3 = null;
				layoutControlItem2 = null;
				layoutControlItem1 = null;
				txtSoThe = null;
				txtSanPham = null;
				txtTenKhachHang = null;
				spinHanMuc = null;
				txtCongTy = null;
				dtNgayCap = null;
				dtThoiHanTu = null;
				dtThoiHanDen = null;
				btnSave = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.UC.UCOtherServiceReqInfo.FUN.frmFun));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.btnAddInFor = new DevExpress.XtraEditors.SimpleButton();
			this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.dtThoiHanDen = new DevExpress.XtraEditors.DateEdit();
			this.dtThoiHanTu = new DevExpress.XtraEditors.DateEdit();
			this.dtNgayCap = new DevExpress.XtraEditors.DateEdit();
			this.txtCongTy = new DevExpress.XtraEditors.TextEdit();
			this.spinHanMuc = new DevExpress.XtraEditors.SpinEdit();
			this.txtTenKhachHang = new DevExpress.XtraEditors.TextEdit();
			this.txtSanPham = new DevExpress.XtraEditors.TextEdit();
			this.txtSoThe = new DevExpress.XtraEditors.TextEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem__Save = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanDen.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanDen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanTu.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanTu.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtNgayCap.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtNgayCap.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtCongTy.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.spinHanMuc.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtTenKhachHang.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtSanPham.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtSoThe.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnAddInFor);
			this.layoutControl1.Controls.Add(this.labelControl1);
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.dtThoiHanDen);
			this.layoutControl1.Controls.Add(this.dtThoiHanTu);
			this.layoutControl1.Controls.Add(this.dtNgayCap);
			this.layoutControl1.Controls.Add(this.txtCongTy);
			this.layoutControl1.Controls.Add(this.spinHanMuc);
			this.layoutControl1.Controls.Add(this.txtTenKhachHang);
			this.layoutControl1.Controls.Add(this.txtSanPham);
			this.layoutControl1.Controls.Add(this.txtSoThe);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 29);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(503, 63, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(631, 127);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnAddInFor.Image = (System.Drawing.Image)resources.GetObject("btnAddInFor.Image");
			this.btnAddInFor.Location = new System.Drawing.Point(301, 12);
			this.btnAddInFor.Name = "btnAddInFor";
			this.btnAddInFor.Size = new System.Drawing.Size(24, 22);
			this.btnAddInFor.StyleController = this.layoutControl1;
			this.btnAddInFor.TabIndex = 14;
			this.btnAddInFor.ToolTip = "Bảo hiểm Bảo Việt";
			this.btnAddInFor.Click += new System.EventHandler(btnAddInFor_Click);
			this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.labelControl1.Location = new System.Drawing.Point(581, 86);
			this.labelControl1.Name = "labelControl1";
			this.labelControl1.Size = new System.Drawing.Size(21, 20);
			this.labelControl1.StyleController = this.layoutControl1;
			this.labelControl1.TabIndex = 13;
			this.btnSave.Location = new System.Drawing.Point(405, 110);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(197, 22);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Lưu (Ctrl S)";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.dtThoiHanDen.EditValue = null;
			this.dtThoiHanDen.Location = new System.Drawing.Point(416, 38);
			this.dtThoiHanDen.Name = "dtThoiHanDen";
			this.dtThoiHanDen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiHanDen.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiHanDen.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtThoiHanDen.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiHanDen.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtThoiHanDen.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiHanDen.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtThoiHanDen.Size = new System.Drawing.Size(186, 20);
			this.dtThoiHanDen.StyleController = this.layoutControl1;
			this.dtThoiHanDen.TabIndex = 11;
			this.dtThoiHanDen.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtThoiHanDen_Closed);
			this.dtThoiHanDen.KeyDown += new System.Windows.Forms.KeyEventHandler(dtThoiHanDen_KeyDown);
			this.dtThoiHanTu.EditValue = null;
			this.dtThoiHanTu.Location = new System.Drawing.Point(127, 38);
			this.dtThoiHanTu.Name = "dtThoiHanTu";
			this.dtThoiHanTu.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiHanTu.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiHanTu.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtThoiHanTu.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiHanTu.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtThoiHanTu.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiHanTu.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtThoiHanTu.Size = new System.Drawing.Size(170, 20);
			this.dtThoiHanTu.StyleController = this.layoutControl1;
			this.dtThoiHanTu.TabIndex = 10;
			this.dtThoiHanTu.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtThoiHanTu_Closed);
			this.dtThoiHanTu.KeyDown += new System.Windows.Forms.KeyEventHandler(dtThoiHanTu_KeyDown);
			this.dtNgayCap.EditValue = null;
			this.dtNgayCap.Location = new System.Drawing.Point(127, 86);
			this.dtNgayCap.Name = "dtNgayCap";
			this.dtNgayCap.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtNgayCap.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtNgayCap.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtNgayCap.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtNgayCap.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtNgayCap.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtNgayCap.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtNgayCap.Size = new System.Drawing.Size(170, 20);
			this.dtNgayCap.StyleController = this.layoutControl1;
			this.dtNgayCap.TabIndex = 9;
			this.dtNgayCap.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtNgayCap_Closed);
			this.dtNgayCap.KeyDown += new System.Windows.Forms.KeyEventHandler(dtNgayCap_KeyDown);
			this.txtCongTy.Location = new System.Drawing.Point(416, 62);
			this.txtCongTy.Name = "txtCongTy";
			this.txtCongTy.Size = new System.Drawing.Size(186, 20);
			this.txtCongTy.StyleController = this.layoutControl1;
			this.txtCongTy.TabIndex = 8;
			this.txtCongTy.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtCongTy_PreviewKeyDown);
			this.spinHanMuc.EditValue = new decimal(new int[4]);
			this.spinHanMuc.Location = new System.Drawing.Point(416, 86);
			this.spinHanMuc.Name = "spinHanMuc";
			this.spinHanMuc.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.spinHanMuc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.spinHanMuc.Size = new System.Drawing.Size(150, 20);
			this.spinHanMuc.StyleController = this.layoutControl1;
			this.spinHanMuc.TabIndex = 7;
			this.spinHanMuc.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(spinHanMuc_PreviewKeyDown);
			this.txtTenKhachHang.Location = new System.Drawing.Point(127, 62);
			this.txtTenKhachHang.Name = "txtTenKhachHang";
			this.txtTenKhachHang.Size = new System.Drawing.Size(170, 20);
			this.txtTenKhachHang.StyleController = this.layoutControl1;
			this.txtTenKhachHang.TabIndex = 6;
			this.txtTenKhachHang.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtTenKhachHang_PreviewKeyDown);
			this.txtSanPham.Location = new System.Drawing.Point(416, 12);
			this.txtSanPham.Name = "txtSanPham";
			this.txtSanPham.Size = new System.Drawing.Size(186, 20);
			this.txtSanPham.StyleController = this.layoutControl1;
			this.txtSanPham.TabIndex = 5;
			this.txtSanPham.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtSanPham_PreviewKeyDown);
			this.txtSoThe.Location = new System.Drawing.Point(127, 12);
			this.txtSoThe.Name = "txtSoThe";
			this.txtSoThe.Properties.MaxLength = 255;
			this.txtSoThe.Size = new System.Drawing.Size(170, 20);
			this.txtSoThe.StyleController = this.layoutControl1;
			this.txtSoThe.TabIndex = 4;
			this.txtSoThe.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtSoThe_PreviewKeyDown);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[12]
			{
				this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem6, this.layoutControlItem9, this.emptySpaceItem1, this.layoutControlItem7, this.layoutControlItem8, this.layoutControlItem5, this.layoutControlItem4,
				this.layoutControlItem10, this.layoutControlItem11
			});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Size = new System.Drawing.Size(614, 144);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.txtSoThe;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(289, 26);
			this.layoutControlItem1.Text = "Số thẻ:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem2.Control = this.txtSanPham;
			this.layoutControlItem2.Location = new System.Drawing.Point(317, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(277, 26);
			this.layoutControlItem2.Text = "Sản phẩm:";
			this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(82, 20);
			this.layoutControlItem2.TextToControlDistance = 5;
			this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem3.Control = this.txtTenKhachHang;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 50);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(289, 24);
			this.layoutControlItem3.Text = "Tên khách hàng:";
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem3.TextToControlDistance = 5;
			this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem6.Control = this.dtNgayCap;
			this.layoutControlItem6.Location = new System.Drawing.Point(0, 74);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(289, 24);
			this.layoutControlItem6.Text = "Ngày cấp:";
			this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem6.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem6.TextToControlDistance = 5;
			this.layoutControlItem9.Control = this.btnSave;
			this.layoutControlItem9.Location = new System.Drawing.Point(393, 98);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.Size = new System.Drawing.Size(201, 26);
			this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem9.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 98);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(393, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem7.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
			this.layoutControlItem7.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem7.Control = this.dtThoiHanTu;
			this.layoutControlItem7.Location = new System.Drawing.Point(0, 26);
			this.layoutControlItem7.Name = "layoutControlItem7";
			this.layoutControlItem7.Size = new System.Drawing.Size(289, 24);
			this.layoutControlItem7.Text = "Thời hạn từ:";
			this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem7.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem7.TextToControlDistance = 5;
			this.layoutControlItem8.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
			this.layoutControlItem8.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem8.Control = this.dtThoiHanDen;
			this.layoutControlItem8.Location = new System.Drawing.Point(289, 26);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(305, 24);
			this.layoutControlItem8.Text = "Thời hạn đến:";
			this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem8.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem8.TextToControlDistance = 5;
			this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem5.Control = this.txtCongTy;
			this.layoutControlItem5.Location = new System.Drawing.Point(289, 50);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(305, 24);
			this.layoutControlItem5.Text = "Công ty:";
			this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem5.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem5.TextToControlDistance = 5;
			this.layoutControlItem4.AppearanceItemCaption.ForeColor = System.Drawing.Color.Black;
			this.layoutControlItem4.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem4.Control = this.spinHanMuc;
			this.layoutControlItem4.Location = new System.Drawing.Point(289, 74);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(269, 24);
			this.layoutControlItem4.Text = "Hạn mức:";
			this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem4.TextSize = new System.Drawing.Size(110, 20);
			this.layoutControlItem4.TextToControlDistance = 5;
			this.layoutControlItem10.Control = this.labelControl1;
			this.layoutControlItem10.Location = new System.Drawing.Point(558, 74);
			this.layoutControlItem10.Name = "layoutControlItem10";
			this.layoutControlItem10.Size = new System.Drawing.Size(36, 24);
			this.layoutControlItem10.Text = "đ";
			this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem10.TextSize = new System.Drawing.Size(6, 20);
			this.layoutControlItem10.TextToControlDistance = 5;
			this.layoutControlItem11.Control = this.btnAddInFor;
			this.layoutControlItem11.Location = new System.Drawing.Point(289, 0);
			this.layoutControlItem11.Name = "layoutControlItem11";
			this.layoutControlItem11.Size = new System.Drawing.Size(28, 26);
			this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem11.TextVisible = false;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar1 });
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[1] { this.barButtonItem__Save });
			this.barManager1.MaxItemId = 1;
			this.bar1.BarName = "Tools";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 0;
			this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[1]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem__Save)
			});
			this.bar1.Text = "Tools";
			this.bar1.Visible = false;
			this.barButtonItem__Save.Caption = "Luu (Ctrl S)";
			this.barButtonItem__Save.Id = 0;
			this.barButtonItem__Save.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem__Save.Name = "barButtonItem__Save";
			this.barButtonItem__Save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem__Save_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(631, 29);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 156);
			this.barDockControlBottom.Size = new System.Drawing.Size(631, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 127);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(631, 29);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 127);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(631, 156);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Name = "frmFun";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Thông tin bổ sung (Đơn vị cùng chi trả)";
			base.Load += new System.EventHandler(frmFun_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanDen.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanDen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanTu.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiHanTu.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtNgayCap.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtNgayCap.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtCongTy.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.spinHanMuc.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtTenKhachHang.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtSanPham.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtSoThe.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
