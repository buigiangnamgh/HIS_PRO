using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;
using HIS.Desktop.Plugins.MedicalStoreV2.Resources;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Borrow
{
	public class TreatmentBorrow : FormBase
	{
		private int positionHandle = -1;

		private MediRecordADO currentAdo;

		private RefeshReference refeshData;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private SimpleButton btnSave;

		private GridLookUpEdit cboTaiKhoan;

		private GridView gridLookUpEdit2View;

		private TextEdit txtTaiKhoan;

		private GridLookUpEdit cboDepartment;

		private GridView gridLookUpEdit1View;

		private TextEdit txtDepartmentCode;

		private LayoutControlGroup layoutControlGroup1;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private EmptySpaceItem emptySpaceItem2;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItem1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private DXValidationProvider dxValidationProvider1;

		private DateEdit dtThoiGianHenTra;

		private LayoutControlItem layoutControlItem6;

		private DateEdit dtGiveTime;

		private LayoutControlItem layoutControlItem7;

		private TextEdit txtBorrowPeople;

		private LayoutControlItem layoutControlItem8;

		private TextEdit txtPhoneNumber;

		private LayoutControlItem layoutControlItem9;

		public TreatmentBorrow(MediRecordADO ado, RefeshReference refeshData)
		{
			InitializeComponent();
			try
			{
				currentAdo = ado;
				this.refeshData = refeshData;
				SetIcon();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			try
			{
				positionHandle = -1;
				bool value = false;
				CommonParam commonParam = new CommonParam();
				if (!dxValidationProvider1.Validate())
				{
					return;
				}
				HIS_MEDI_RECORD_BORROW treatmentBorrow = new HIS_MEDI_RECORD_BORROW();
				treatmentBorrow.MEDI_RECORD_ID = ((V_HIS_MEDI_RECORD_1)currentAdo).ID;
				if (cboDepartment.EditValue != null)
				{
					treatmentBorrow.DEPARTMENT_ID = Parse.ToInt64(cboDepartment.EditValue.ToString());
				}
				if (cboTaiKhoan.EditValue != null && !string.IsNullOrEmpty(cboTaiKhoan.EditValue.ToString()))
				{
					treatmentBorrow.BORROW_LOGINNAME = cboTaiKhoan.EditValue.ToString();
					treatmentBorrow.BORROW_USERNAME = BackendDataWorker.Get<ACS_USER>().FirstOrDefault((ACS_USER o) => o.LOGINNAME == treatmentBorrow.BORROW_LOGINNAME).USERNAME;
				}
				if (dtThoiGianHenTra.EditValue != null)
				{
					treatmentBorrow.APPOINTMENT_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dtThoiGianHenTra.DateTime);
				}
				else
				{
					treatmentBorrow.APPOINTMENT_TIME = null;
				}
				if (dtGiveTime.EditValue != null)
				{
					treatmentBorrow.GIVE_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dtGiveTime.DateTime);
				}
				else
				{
					treatmentBorrow.GIVE_TIME = null;
				}
				treatmentBorrow.BORROW_USERNAME = txtBorrowPeople.Text;
				treatmentBorrow.BORROW_PHONE = txtPhoneNumber.Text;
				LogSystem.Debug(LogUtil.TraceData("api/HisMediRecordBorrow/Create Input", treatmentBorrow));
				WaitingManager.Show();
				HIS_MEDI_RECORD_BORROW val = new BackendAdapter(commonParam).Post<HIS_MEDI_RECORD_BORROW>("api/HisMediRecordBorrow/Create", ApiConsumers.MosConsumer, treatmentBorrow, commonParam);
				WaitingManager.Hide();
				if (val != null)
				{
					value = true;
					if (refeshData != null)
					{
						refeshData();
					}
					Close();
				}
				MessageManager.Show(this, commonParam, value);
				SessionManager.ProcessTokenLost(commonParam);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void TreatmentBorrow_Load(object sender, EventArgs e)
		{
			try
			{
				SetCaptionByLanguageKey();
				ValidateForm();
				InitComboDepartment();
				InitComboTaiKhoan();
				dtGiveTime.DateTime = DateTime.Now;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateForm()
		{
			try
			{
				ValidateGridLookupWithTextEdit(cboDepartment, txtDepartmentCode);
				ValidateThoiGianHenTra();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateGridLookupWithTextEdit(GridLookUpEdit cbo, TextEdit textEdit)
		{
			try
			{
				GridLookupEditWithTextEditValidationRule gridLookupEditWithTextEditValidationRule = new GridLookupEditWithTextEditValidationRule();
				gridLookupEditWithTextEditValidationRule.txtTextEdit = textEdit;
				gridLookupEditWithTextEditValidationRule.cbo = cbo;
				gridLookupEditWithTextEditValidationRule.ErrorText = MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				gridLookupEditWithTextEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(textEdit, gridLookupEditWithTextEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateThoiGianHenTra()
		{
			try
			{
				ValidateThoiGianHenTra validateThoiGianHenTra = new ValidateThoiGianHenTra();
				validateThoiGianHenTra.dtTime = dtThoiGianHenTra;
				validateThoiGianHenTra.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(dtThoiGianHenTra, validateThoiGianHenTra);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboDepartment()
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("DEPARTMENT_CODE", "", 100, 1));
				list.Add(new ColumnInfo("DEPARTMENT_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("DEPARTMENT_NAME", "ID", list, false, 350);
				ControlEditorLoader.Load(cboDepartment, BackendDataWorker.Get<HIS_DEPARTMENT>(), controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboTaiKhoan()
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("LOGINNAME", "", 100, 1));
				list.Add(new ColumnInfo("USERNAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("USERNAME", "LOGINNAME", list, false, 350);
				ControlEditorLoader.Load(cboTaiKhoan, BackendDataWorker.Get<ACS_USER>(), controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetIcon()
		{
			try
			{
				base.Icon = Icon.ExtractAssociatedIcon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtDepartmentCode_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (string.IsNullOrEmpty(txtDepartmentCode.Text))
				{
					cboDepartment.EditValue = null;
					cboDepartment.Focus();
					cboDepartment.ShowPopup();
					return;
				}
				List<HIS_DEPARTMENT> list = null;
				List<HIS_DEPARTMENT> list2 = (from o in BackendDataWorker.Get<HIS_DEPARTMENT>()
					where o.DEPARTMENT_CODE.ToUpper().Contains(txtDepartmentCode.Text.ToUpper())
					select o).ToList();
				if (list2 != null && list2.Count > 0)
				{
					list = ((list2.Count == 1) ? list2 : list2.Where((HIS_DEPARTMENT o) => o.DEPARTMENT_CODE.ToUpper() == txtDepartmentCode.Text.ToUpper()).ToList());
				}
				if (list != null && list.Count == 1)
				{
					txtDepartmentCode.Text = list[0].DEPARTMENT_CODE;
					cboDepartment.EditValue = list[0].ID;
					txtTaiKhoan.Focus();
					txtTaiKhoan.SelectAll();
				}
				else
				{
					cboDepartment.Focus();
					cboDepartment.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboDepartment_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && cboDepartment.EditValue != null)
				{
					HIS_DEPARTMENT hIS_DEPARTMENT = BackendDataWorker.Get<HIS_DEPARTMENT>().SingleOrDefault((HIS_DEPARTMENT o) => o.ID == Parse.ToInt64((cboDepartment.EditValue ?? "").ToString()));
					if (hIS_DEPARTMENT != null)
					{
						txtDepartmentCode.Text = hIS_DEPARTMENT.DEPARTMENT_CODE;
						txtDepartmentCode.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtTaiKhoan_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (string.IsNullOrEmpty(txtTaiKhoan.Text))
				{
					cboTaiKhoan.EditValue = null;
					cboTaiKhoan.Focus();
					cboTaiKhoan.ShowPopup();
					return;
				}
				List<ACS_USER> list = null;
				List<ACS_USER> list2 = (from o in BackendDataWorker.Get<ACS_USER>()
					where o.LOGINNAME.ToUpper().Contains(txtTaiKhoan.Text.ToUpper())
					select o).ToList();
				if (list2 != null && list2.Count > 0)
				{
					list = ((list2.Count == 1) ? list2 : list2.Where((ACS_USER o) => o.LOGINNAME.ToUpper() == txtTaiKhoan.Text.ToUpper()).ToList());
				}
				if (list != null && list.Count == 1)
				{
					txtTaiKhoan.Text = list[0].LOGINNAME;
					cboTaiKhoan.EditValue = list[0].LOGINNAME;
					cboTaiKhoan.Properties.Buttons[1].Visible = true;
					txtBorrowPeople.Text = list[0].USERNAME;
					txtBorrowPeople.ReadOnly = true;
					txtPhoneNumber.Focus();
				}
				else
				{
					cboTaiKhoan.Focus();
					cboTaiKhoan.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboTaiKhoan_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboTaiKhoan.EditValue != null && !string.IsNullOrEmpty(cboTaiKhoan.EditValue.ToString()))
				{
					ACS_USER aCS_USER = BackendDataWorker.Get<ACS_USER>().SingleOrDefault((ACS_USER o) => o.LOGINNAME == (cboTaiKhoan.EditValue ?? "").ToString());
					if (aCS_USER != null)
					{
						txtTaiKhoan.Text = aCS_USER.LOGINNAME;
						txtPhoneNumber.Focus();
						txtBorrowPeople.Text = aCS_USER.USERNAME;
						txtBorrowPeople.ReadOnly = true;
						cboTaiKhoan.Properties.Buttons[1].Visible = true;
					}
				}
				else
				{
					txtBorrowPeople.Text = "";
					txtBorrowPeople.ReadOnly = false;
					txtBorrowPeople.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
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
				if (baseEditViewInfo != null)
				{
					if (positionHandle == -1)
					{
						positionHandle = baseEdit.TabIndex;
						baseEdit.SelectAll();
						baseEdit.Focus();
					}
					if (positionHandle > baseEdit.TabIndex)
					{
						positionHandle = baseEdit.TabIndex;
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

		private void dtThoiGianHenTra_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtGiveTime_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtDepartmentCode.Focus();
					txtDepartmentCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtGiveTime_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dtGiveTime.EditValue != null)
				{
					long? num = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dtGiveTime.DateTime);
					double value = HisConfigs.Get<double>("HIS.Desktop.Plugins.MedicalStoreV2.TreatmentBorrow.OverAppointment");
					long? num2 = Calculation.Add(num.GetValueOrDefault(), value, Calculation.UnitDifferenceTime.DAY);
					dtThoiGianHenTra.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(num2.GetValueOrDefault()) ?? DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtBorrowPeople_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtPhoneNumber.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtPhoneNumber_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtThoiGianHenTra.Focus();
					dtThoiGianHenTra.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTaiKhoan_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboTaiKhoan.EditValue == null || string.IsNullOrEmpty(cboTaiKhoan.EditValue.ToString()))
				{
					txtTaiKhoan.Text = "";
					txtBorrowPeople.Text = "";
					txtBorrowPeople.ReadOnly = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTaiKhoan_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboTaiKhoan.Properties.Buttons[1].Visible = false;
					cboTaiKhoan.EditValue = null;
					txtTaiKhoan.Text = "";
					txtBorrowPeople.Text = "";
					txtBorrowPeople.ReadOnly = false;
					txtTaiKhoan.Focus();
					txtTaiKhoan.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtPhoneNumber_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == '\r')
				{
					dtThoiGianHenTra.Focus();
					dtThoiGianHenTra.SelectAll();
				}
				else if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
				{
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtThoiGianHenTra_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					btnSave.Focus();
				}
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
				ResourceLanguageManager.LanguageResource_TreatmentBorrow = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Lang", typeof(TreatmentBorrow).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControl1.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.bar1.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				barButtonItem1.Caption = Inventec.Common.Resource.Get.Value("TreatmentBorrow.barButtonItem1.Caption", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.btnSave.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				cboTaiKhoan.Properties.NullText = Inventec.Common.Resource.Get.Value("TreatmentBorrow.cboTaiKhoan.Properties.NullText", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				cboDepartment.Properties.NullText = Inventec.Common.Resource.Get.Value("TreatmentBorrow.cboDepartment.Properties.NullText", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem6.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem6.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem7.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem7.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem8.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem8.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				layoutControlItem9.Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.layoutControlItem9.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("TreatmentBorrow.Text", ResourceLanguageManager.LanguageResource_TreatmentBorrow, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
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
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.txtPhoneNumber = new DevExpress.XtraEditors.TextEdit();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.txtBorrowPeople = new DevExpress.XtraEditors.TextEdit();
			this.dtGiveTime = new DevExpress.XtraEditors.DateEdit();
			this.dtThoiGianHenTra = new DevExpress.XtraEditors.DateEdit();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.cboTaiKhoan = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.txtTaiKhoan = new DevExpress.XtraEditors.TextEdit();
			this.cboDepartment = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.txtDepartmentCode = new DevExpress.XtraEditors.TextEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtPhoneNumber.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtBorrowPeople.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtGiveTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtGiveTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiGianHenTra.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiGianHenTra.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboTaiKhoan.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtTaiKhoan.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboDepartment.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtDepartmentCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.txtPhoneNumber);
			this.layoutControl1.Controls.Add(this.txtBorrowPeople);
			this.layoutControl1.Controls.Add(this.dtGiveTime);
			this.layoutControl1.Controls.Add(this.dtThoiGianHenTra);
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.cboTaiKhoan);
			this.layoutControl1.Controls.Add(this.txtTaiKhoan);
			this.layoutControl1.Controls.Add(this.cboDepartment);
			this.layoutControl1.Controls.Add(this.txtDepartmentCode);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 29);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(88, 121, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(391, 171);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.txtPhoneNumber.Location = new System.Drawing.Point(137, 98);
			this.txtPhoneNumber.MenuManager = this.barManager1;
			this.txtPhoneNumber.Name = "txtPhoneNumber";
			this.txtPhoneNumber.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.txtPhoneNumber.Properties.MaxLength = 11;
			this.txtPhoneNumber.Size = new System.Drawing.Size(252, 20);
			this.txtPhoneNumber.StyleController = this.layoutControl1;
			this.txtPhoneNumber.TabIndex = 12;
			this.txtPhoneNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPhoneNumber_KeyPress);
			this.txtPhoneNumber.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtPhoneNumber_PreviewKeyDown);
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar1 });
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[1] { this.barButtonItem1 });
			this.barManager1.MaxItemId = 1;
			this.bar1.BarName = "Tools";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 0;
			this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[1]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1)
			});
			this.bar1.Text = "Tools";
			this.bar1.Visible = false;
			this.barButtonItem1.Caption = "Lưu";
			this.barButtonItem1.Id = 0;
			this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem1.Name = "barButtonItem1";
			this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem1_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(391, 29);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 200);
			this.barDockControlBottom.Size = new System.Drawing.Size(391, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 171);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(391, 29);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 171);
			this.txtBorrowPeople.Location = new System.Drawing.Point(137, 74);
			this.txtBorrowPeople.MenuManager = this.barManager1;
			this.txtBorrowPeople.Name = "txtBorrowPeople";
			this.txtBorrowPeople.Size = new System.Drawing.Size(252, 20);
			this.txtBorrowPeople.StyleController = this.layoutControl1;
			this.txtBorrowPeople.TabIndex = 11;
			this.txtBorrowPeople.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtBorrowPeople_PreviewKeyDown);
			this.dtGiveTime.EditValue = null;
			this.dtGiveTime.Location = new System.Drawing.Point(137, 2);
			this.dtGiveTime.MenuManager = this.barManager1;
			this.dtGiveTime.Name = "dtGiveTime";
			this.dtGiveTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtGiveTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtGiveTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.dtGiveTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtGiveTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.dtGiveTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtGiveTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm:ss";
			this.dtGiveTime.Size = new System.Drawing.Size(252, 20);
			this.dtGiveTime.StyleController = this.layoutControl1;
			this.dtGiveTime.TabIndex = 10;
			this.dtGiveTime.EditValueChanged += new System.EventHandler(dtGiveTime_EditValueChanged);
			this.dtGiveTime.KeyUp += new System.Windows.Forms.KeyEventHandler(dtGiveTime_KeyUp);
			this.dtThoiGianHenTra.EditValue = null;
			this.dtThoiGianHenTra.Location = new System.Drawing.Point(137, 122);
			this.dtThoiGianHenTra.MenuManager = this.barManager1;
			this.dtThoiGianHenTra.Name = "dtThoiGianHenTra";
			this.dtThoiGianHenTra.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiGianHenTra.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtThoiGianHenTra.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.dtThoiGianHenTra.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiGianHenTra.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.dtThoiGianHenTra.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtThoiGianHenTra.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm:ss";
			this.dtThoiGianHenTra.Size = new System.Drawing.Size(252, 20);
			this.dtThoiGianHenTra.StyleController = this.layoutControl1;
			this.dtThoiGianHenTra.TabIndex = 9;
			this.dtThoiGianHenTra.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtThoiGianHenTra_Closed);
			this.dtThoiGianHenTra.KeyUp += new System.Windows.Forms.KeyEventHandler(dtThoiGianHenTra_KeyUp);
			this.btnSave.Location = new System.Drawing.Point(277, 146);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(112, 22);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "Lưu (Ctrl S)";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.cboTaiKhoan.Location = new System.Drawing.Point(217, 50);
			this.cboTaiKhoan.Name = "cboTaiKhoan";
			this.cboTaiKhoan.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboTaiKhoan.Properties.AutoComplete = false;
			this.cboTaiKhoan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.cboTaiKhoan.Properties.NullText = "";
			this.cboTaiKhoan.Properties.View = this.gridLookUpEdit2View;
			this.cboTaiKhoan.Size = new System.Drawing.Size(172, 20);
			this.cboTaiKhoan.StyleController = this.layoutControl1;
			this.cboTaiKhoan.TabIndex = 7;
			this.cboTaiKhoan.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboTaiKhoan_Closed);
			this.cboTaiKhoan.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboTaiKhoan_ButtonClick);
			this.cboTaiKhoan.EditValueChanged += new System.EventHandler(cboTaiKhoan_EditValueChanged);
			this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
			this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
			this.txtTaiKhoan.Location = new System.Drawing.Point(137, 50);
			this.txtTaiKhoan.Name = "txtTaiKhoan";
			this.txtTaiKhoan.Size = new System.Drawing.Size(80, 20);
			this.txtTaiKhoan.StyleController = this.layoutControl1;
			this.txtTaiKhoan.TabIndex = 6;
			this.txtTaiKhoan.KeyUp += new System.Windows.Forms.KeyEventHandler(txtTaiKhoan_KeyUp);
			this.cboDepartment.Location = new System.Drawing.Point(217, 26);
			this.cboDepartment.Name = "cboDepartment";
			this.cboDepartment.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboDepartment.Properties.AutoComplete = false;
			this.cboDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboDepartment.Properties.NullText = "";
			this.cboDepartment.Properties.View = this.gridLookUpEdit1View;
			this.cboDepartment.Size = new System.Drawing.Size(172, 20);
			this.cboDepartment.StyleController = this.layoutControl1;
			this.cboDepartment.TabIndex = 5;
			this.cboDepartment.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboDepartment_Closed);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.txtDepartmentCode.Location = new System.Drawing.Point(137, 26);
			this.txtDepartmentCode.Name = "txtDepartmentCode";
			this.txtDepartmentCode.Size = new System.Drawing.Size(80, 20);
			this.txtDepartmentCode.StyleController = this.layoutControl1;
			this.txtDepartmentCode.TabIndex = 4;
			this.txtDepartmentCode.KeyUp += new System.Windows.Forms.KeyEventHandler(txtDepartmentCode_KeyUp);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[10] { this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.emptySpaceItem2, this.layoutControlItem6, this.layoutControlItem7, this.layoutControlItem8, this.layoutControlItem9 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(391, 171);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.txtDepartmentCode;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem1.Size = new System.Drawing.Size(217, 24);
			this.layoutControlItem1.Text = "Khoa mượn:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem2.Control = this.cboDepartment;
			this.layoutControlItem2.Location = new System.Drawing.Point(217, 24);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem2.Size = new System.Drawing.Size(174, 24);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem3.Control = this.txtTaiKhoan;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem3.Size = new System.Drawing.Size(217, 24);
			this.layoutControlItem3.Text = "Tài khoản mượn:";
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem3.TextToControlDistance = 5;
			this.layoutControlItem4.Control = this.cboTaiKhoan;
			this.layoutControlItem4.Location = new System.Drawing.Point(217, 48);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem4.Size = new System.Drawing.Size(174, 24);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.Control = this.btnSave;
			this.layoutControlItem5.Location = new System.Drawing.Point(275, 144);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(116, 27);
			this.layoutControlItem5.Text = "L";
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextVisible = false;
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(0, 144);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(275, 27);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem6.Control = this.dtThoiGianHenTra;
			this.layoutControlItem6.Location = new System.Drawing.Point(0, 120);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(391, 24);
			this.layoutControlItem6.Text = "Thời gian hẹn trả:";
			this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem6.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem6.TextToControlDistance = 5;
			this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem7.Control = this.dtGiveTime;
			this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem7.Name = "layoutControlItem7";
			this.layoutControlItem7.Size = new System.Drawing.Size(391, 24);
			this.layoutControlItem7.Text = "Thời gian mượn:";
			this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem7.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem7.TextToControlDistance = 5;
			this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem8.Control = this.txtBorrowPeople;
			this.layoutControlItem8.Location = new System.Drawing.Point(0, 72);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(391, 24);
			this.layoutControlItem8.Text = "Người mượn:";
			this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem8.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem8.TextToControlDistance = 5;
			this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem9.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem9.Control = this.txtPhoneNumber;
			this.layoutControlItem9.Location = new System.Drawing.Point(0, 96);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.Size = new System.Drawing.Size(391, 24);
			this.layoutControlItem9.Text = "Số điện thoại người mượn:";
			this.layoutControlItem9.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem9.TextSize = new System.Drawing.Size(130, 20);
			this.layoutControlItem9.TextToControlDistance = 5;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(391, 200);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Name = "TreatmentBorrow";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Mượn hồ sơ bệnh án";
			base.Load += new System.EventHandler(TreatmentBorrow_Load);
			base.Controls.SetChildIndex(this.barDockControlTop, 0);
			base.Controls.SetChildIndex(this.barDockControlBottom, 0);
			base.Controls.SetChildIndex(this.barDockControlRight, 0);
			base.Controls.SetChildIndex(this.barDockControlLeft, 0);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtPhoneNumber.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtBorrowPeople.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtGiveTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtGiveTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiGianHenTra.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtThoiGianHenTra.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboTaiKhoan.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtTaiKhoan.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboDepartment.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtDepartmentCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
