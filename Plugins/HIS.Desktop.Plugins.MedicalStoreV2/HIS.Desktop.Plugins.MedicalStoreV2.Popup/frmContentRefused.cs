using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;
using HIS.Desktop.Plugins.MedicalStoreV2.Resources;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Modules;
using MOS.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Popup
{
	public class frmContentRefused : Form
	{
		private Module currentModule;

		private TreatmentADO TreatmentADO;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private SimpleButton btnClose;

		private MemoEdit txtRecordInspectionRejectNote;

		private MemoEdit txtRejectStoreReason;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private EmptySpaceItem emptySpaceItem1;

		public frmContentRefused(Module _currentModule, TreatmentADO _TreatmentADO)
		{
			InitializeComponent();
			try
			{
				currentModule = _currentModule;
				TreatmentADO = _TreatmentADO;
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

		private void frmContentRefused_Load(object sender, EventArgs e)
		{
			SetCaptionByLanguageKey();
			SetDefaultData();
		}

		private void SetDefaultData()
		{
			try
			{
				if (TreatmentADO != null)
				{
					txtRejectStoreReason.Text = ((L_HIS_TREATMENT_3)TreatmentADO).REJECT_STORE_REASON;
					txtRecordInspectionRejectNote.Text = ((L_HIS_TREATMENT_3)TreatmentADO).RECORD_INSPECTION_REJECT_NOTE;
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
				ResourceLanguageManager.LanguageResource_frmContentRefused = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Lang", typeof(frmContentRefused).Assembly);
				layoutControl1.Text = Get.Value("frmContentRefused.layoutControl1.Text", ResourceLanguageManager.LanguageResource_frmContentRefused, LanguageManager.GetCulture());
				layoutControlItem1.Text = Get.Value("frmContentRefused.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource_frmContentRefused, LanguageManager.GetCulture());
				layoutControlItem2.Text = Get.Value("frmContentRefused.layoutControlItem2.Text", ResourceLanguageManager.LanguageResource_frmContentRefused, LanguageManager.GetCulture());
				btnClose.Text = Get.Value("frmContentRefused.btnClose.Text", ResourceLanguageManager.LanguageResource_frmContentRefused, LanguageManager.GetCulture());
				Text = Get.Value("frmContentRefused.Text", ResourceLanguageManager.LanguageResource_frmContentRefused, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				Close();
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
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.txtRejectStoreReason = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.txtRecordInspectionRejectNote = new DevExpress.XtraEditors.MemoEdit();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.btnClose = new DevExpress.XtraEditors.SimpleButton();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtRejectStoreReason.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtRecordInspectionRejectNote.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnClose);
			this.layoutControl1.Controls.Add(this.txtRecordInspectionRejectNote);
			this.layoutControl1.Controls.Add(this.txtRejectStoreReason);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(487, 279);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.False;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[4] { this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.emptySpaceItem1 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Size = new System.Drawing.Size(487, 279);
			this.layoutControlGroup1.TextVisible = false;
			this.txtRejectStoreReason.Location = new System.Drawing.Point(107, 2);
			this.txtRejectStoreReason.Name = "txtRejectStoreReason";
			this.txtRejectStoreReason.Properties.ReadOnly = true;
			this.txtRejectStoreReason.Size = new System.Drawing.Size(378, 113);
			this.txtRejectStoreReason.StyleController = this.layoutControl1;
			this.txtRejectStoreReason.TabIndex = 4;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.layoutControlItem1.Control = this.txtRejectStoreReason;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(487, 117);
			this.layoutControlItem1.Text = "Lý do từ chối duyệt lưu trữ:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.txtRecordInspectionRejectNote.Location = new System.Drawing.Point(107, 119);
			this.txtRecordInspectionRejectNote.Name = "txtRecordInspectionRejectNote";
			this.txtRecordInspectionRejectNote.Properties.ReadOnly = true;
			this.txtRecordInspectionRejectNote.Size = new System.Drawing.Size(378, 132);
			this.txtRecordInspectionRejectNote.StyleController = this.layoutControl1;
			this.txtRecordInspectionRejectNote.TabIndex = 5;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.layoutControlItem2.Control = this.txtRecordInspectionRejectNote;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 117);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(487, 136);
			this.layoutControlItem2.Text = "Lý do từ chối duyệt giám định:";
			this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem2.TextToControlDistance = 5;
			this.btnClose.Location = new System.Drawing.Point(358, 255);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(127, 22);
			this.btnClose.StyleController = this.layoutControl1;
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Đóng";
			this.btnClose.Click += new System.EventHandler(btnClose_Click);
			this.layoutControlItem3.Control = this.btnClose;
			this.layoutControlItem3.Location = new System.Drawing.Point(356, 253);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(131, 26);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 253);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(356, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(487, 279);
			base.Controls.Add(this.layoutControl1);
			base.Name = "frmContentRefused";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lý do từ chối duyệt của lần duyệt trước đó";
			base.Load += new System.EventHandler(frmContentRefused_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtRejectStoreReason.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtRecordInspectionRejectNote.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
