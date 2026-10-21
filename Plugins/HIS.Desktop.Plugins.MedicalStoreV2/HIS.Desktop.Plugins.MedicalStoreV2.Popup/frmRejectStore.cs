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
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;
using HIS.Desktop.Plugins.MedicalStoreV2.Resources;
using HIS.Desktop.Plugins.MedicalStoreV2.Validtion;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using MOS.EFMODEL.DataModels;
using MOS.SDO;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Popup
{
	public class frmRejectStore : Form
	{
		private DelegateSelectData refeshData;

		private Module currentModule;

		private int positionHandle = -1;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItem_Save;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private BarButtonItem barButtonItem__Print;

		private SimpleButton btnSaveData;

		private LayoutControlItem layoutControlItem2;

		private EmptySpaceItem emptySpaceItem1;

		private DXValidationProvider dxValidationProvider1;

		private MemoEdit txtRejectStoreReason;

		private LabelControl lblPatientName;

		private LabelControl lblTreatmentCode;

		private LabelControl lblPatientCode;

		private LayoutControlItem layoutControlItem6;

		private LayoutControlItem layoutControlItem8;

		private LayoutControlItem layoutControlItem9;

		private LayoutControlItem layoutControlItem10;

		internal TreatmentADO currentTreatment { get; set; }

		public frmRejectStore(Module currentModule, TreatmentADO currentMediRecord, DelegateSelectData refeshData)
		{
			InitializeComponent();
			try
			{
				this.currentModule = currentModule;
				currentTreatment = currentMediRecord;
				this.refeshData = refeshData;
				SetIcon();
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
				base.Icon = Icon.ExtractAssociatedIcon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void frmDataStore_Load(object sender, EventArgs e)
		{
			try
			{
				SetCaptionByLanguageKey();
				ValidateForm();
				AssignLabel();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidateForm()
		{
			try
			{
				ValidMaxlengthTxtReason();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void AssignLabel()
		{
			try
			{
				lblPatientCode.Text = ((L_HIS_TREATMENT_3)currentTreatment).TDL_PATIENT_CODE;
				lblPatientName.Text = ((L_HIS_TREATMENT_3)currentTreatment).TDL_PATIENT_NAME;
				lblTreatmentCode.Text = ((L_HIS_TREATMENT_3)currentTreatment).TREATMENT_CODE;
				txtRejectStoreReason.Text = ((L_HIS_TREATMENT_3)currentTreatment).REJECT_STORE_REASON;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateTextEdit(BaseEdit textEdit)
		{
			try
			{
				ControlEditValidationRule controlEditValidationRule = new ControlEditValidationRule();
				controlEditValidationRule.editor = textEdit;
				controlEditValidationRule.ErrorText = MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				controlEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(textEdit, controlEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidMaxlengthTxtReason()
		{
			try
			{
				ValidateMaxLength validateMaxLength = new ValidateMaxLength();
				validateMaxLength.textEdit = txtRejectStoreReason;
				validateMaxLength.maxLength = 4000;
				dxValidationProvider1.SetValidationRule(txtRejectStoreReason, validateMaxLength);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguageResource_frmRejectStore = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Lang", typeof(frmRejectStore).Assembly);
				layoutControl1.Text = Get.Value("frmRejectStore.layoutControl1.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				bar1.Text = Get.Value("frmRejectStore.bar1.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				barButtonItem_Save.Caption = Get.Value("frmRejectStore.barButtonItem_Save.Caption", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				barButtonItem__Print.Caption = Get.Value("frmRejectStore.barButtonItem__Print.Caption", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				btnSaveData.Text = Get.Value("frmRejectStore.btnSaveData.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				layoutControlItem6.Text = Get.Value("frmRejectStore.layoutControlItem6.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				layoutControlItem8.Text = Get.Value("frmRejectStore.layoutControlItem8.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				layoutControlItem9.Text = Get.Value("frmRejectStore.layoutControlItem9.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				layoutControlItem10.Text = Get.Value("frmRejectStore.layoutControlItem10.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
				Text = Get.Value("frmRejectStore.Text", ResourceLanguageManager.LanguageResource_frmRejectStore, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSaveData_Click(object sender, EventArgs e)
		{
			try
			{
				positionHandle = -1;
				CommonParam commonParam = new CommonParam();
				if (!dxValidationProvider1.Validate())
				{
					return;
				}
				HisTreatmentRejectStoreSDO hisTreatmentRejectStoreSDO = new HisTreatmentRejectStoreSDO();
				bool flag = false;
				HIS_TREATMENT hIS_TREATMENT = null;
				if (currentTreatment != null && currentTreatment != null)
				{
					hisTreatmentRejectStoreSDO.TreatmentId = ((L_HIS_TREATMENT_3)currentTreatment).ID;
					hisTreatmentRejectStoreSDO.RejectReason = txtRejectStoreReason.Text.Trim();
					hIS_TREATMENT = new BackendAdapter(commonParam).Post<HIS_TREATMENT>("api/HisTreatment/RejectStore", ApiConsumers.MosConsumer, hisTreatmentRejectStoreSDO, commonParam);
					if (hIS_TREATMENT != null)
					{
						flag = true;
						if (refeshData != null)
						{
							refeshData(flag);
						}
					}
				}
				MessageManager.Show(base.ParentForm, commonParam, flag);
				SessionManager.ProcessTokenLost(commonParam);
				if (flag)
				{
					Close();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void barButtonItem_Save_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				btnSaveData_Click(null, null);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void barButtonItem__Print_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				btnPrint_Click(null, null);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.Desktop.Plugins.MedicalStoreV2.Popup.frmRejectStore));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem_Save = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem__Print = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.btnSaveData = new DevExpress.XtraEditors.SimpleButton();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			this.lblPatientCode = new DevExpress.XtraEditors.LabelControl();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblTreatmentCode = new DevExpress.XtraEditors.LabelControl();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblPatientName = new DevExpress.XtraEditors.LabelControl();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.txtRejectStoreReason = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtRejectStoreReason.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.txtRejectStoreReason);
			this.layoutControl1.Controls.Add(this.lblPatientName);
			this.layoutControl1.Controls.Add(this.lblTreatmentCode);
			this.layoutControl1.Controls.Add(this.lblPatientCode);
			this.layoutControl1.Controls.Add(this.btnSaveData);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 29);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(482, 113, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(440, 197);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar1 });
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[2] { this.barButtonItem_Save, this.barButtonItem__Print });
			this.barManager1.MaxItemId = 2;
			this.bar1.BarName = "Tools";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 0;
			this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem_Save),
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem__Print)
			});
			this.bar1.Text = "Tools";
			this.bar1.Visible = false;
			this.barButtonItem_Save.Caption = "Lưu (Ctrl S)";
			this.barButtonItem_Save.Id = 0;
			this.barButtonItem_Save.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem_Save.Name = "barButtonItem_Save";
			this.barButtonItem_Save.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem_Save_ItemClick);
			this.barButtonItem__Print.Caption = "In barCode";
			this.barButtonItem__Print.Id = 1;
			this.barButtonItem__Print.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Control);
			this.barButtonItem__Print.Name = "barButtonItem__Print";
			this.barButtonItem__Print.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem__Print_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(440, 29);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 226);
			this.barDockControlBottom.Size = new System.Drawing.Size(440, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 197);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(440, 29);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 197);
			this.btnSaveData.Location = new System.Drawing.Point(338, 173);
			this.btnSaveData.Name = "btnSaveData";
			this.btnSaveData.Size = new System.Drawing.Size(100, 22);
			this.btnSaveData.StyleController = this.layoutControl1;
			this.btnSaveData.TabIndex = 11;
			this.btnSaveData.Text = "Lưu (Ctrl S)";
			this.btnSaveData.Click += new System.EventHandler(btnSaveData_Click);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[6] { this.layoutControlItem2, this.emptySpaceItem1, this.layoutControlItem6, this.layoutControlItem8, this.layoutControlItem9, this.layoutControlItem10 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(440, 197);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem2.Control = this.btnSaveData;
			this.layoutControlItem2.Location = new System.Drawing.Point(336, 171);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(104, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 171);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(336, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			this.lblPatientCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblPatientCode.Location = new System.Drawing.Point(97, 2);
			this.lblPatientCode.Name = "lblPatientCode";
			this.lblPatientCode.Size = new System.Drawing.Size(119, 20);
			this.lblPatientCode.StyleController = this.layoutControl1;
			this.lblPatientCode.TabIndex = 20;
			this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem6.Control = this.lblPatientCode;
			this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(218, 24);
			this.layoutControlItem6.Text = "Mã bệnh nhân:";
			this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem6.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem6.TextToControlDistance = 5;
			this.lblTreatmentCode.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblTreatmentCode.Location = new System.Drawing.Point(315, 2);
			this.lblTreatmentCode.Name = "lblTreatmentCode";
			this.lblTreatmentCode.Size = new System.Drawing.Size(123, 20);
			this.lblTreatmentCode.StyleController = this.layoutControl1;
			this.lblTreatmentCode.TabIndex = 21;
			this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem8.Control = this.lblTreatmentCode;
			this.layoutControlItem8.Location = new System.Drawing.Point(218, 0);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(222, 24);
			this.layoutControlItem8.Text = "Mã điều trị:";
			this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem8.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem8.TextToControlDistance = 5;
			this.lblPatientName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblPatientName.Location = new System.Drawing.Point(97, 26);
			this.lblPatientName.Name = "lblPatientName";
			this.lblPatientName.Size = new System.Drawing.Size(341, 20);
			this.lblPatientName.StyleController = this.layoutControl1;
			this.lblPatientName.TabIndex = 22;
			this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem9.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem9.Control = this.lblPatientName;
			this.layoutControlItem9.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.Size = new System.Drawing.Size(440, 24);
			this.layoutControlItem9.Text = "Bệnh nhân:";
			this.layoutControlItem9.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem9.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem9.TextToControlDistance = 5;
			this.txtRejectStoreReason.Location = new System.Drawing.Point(97, 50);
			this.txtRejectStoreReason.MenuManager = this.barManager1;
			this.txtRejectStoreReason.Name = "txtRejectStoreReason";
			this.txtRejectStoreReason.Size = new System.Drawing.Size(341, 119);
			this.txtRejectStoreReason.StyleController = this.layoutControl1;
			this.txtRejectStoreReason.TabIndex = 23;
			this.layoutControlItem10.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem10.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem10.Control = this.txtRejectStoreReason;
			this.layoutControlItem10.Location = new System.Drawing.Point(0, 48);
			this.layoutControlItem10.Name = "layoutControlItem10";
			this.layoutControlItem10.Size = new System.Drawing.Size(440, 123);
			this.layoutControlItem10.Text = "Lý do:";
			this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem10.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem10.TextToControlDistance = 5;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(440, 226);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "frmRejectStore";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Từ chối duyệt bệnh án";
			base.Load += new System.EventHandler(frmDataStore_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtRejectStoreReason.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
