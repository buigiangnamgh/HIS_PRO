using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Resources;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public class UC_KskContract : UserControl
	{
		internal List<V_HIS_KSK_CONTRACT> listKskContract;

		private IContainer components;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		internal LabelControl lblNgayHetHan;

		internal LabelControl lblNgayHieuLuc;

		internal LabelControl lblTyLeThanhToan;

		internal LabelControl lblTenCongTy;

		internal GridLookUpEdit cboContract;

		private GridView gridLookUpEdit1View;

		internal LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private LayoutControlItem layoutControlItem6;

		public UC_KskContract()
		{
			InitializeComponent();
		}

		private void UC_KskContract_Load(object sender, EventArgs e)
		{
			try
			{
				InitComboContract();
				SetCaptionByLanguageKey();
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
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Lang", typeof(UC_KskContract).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControl1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboContract.Properties.NullText = Inventec.Common.Resource.Get.Value("UC_KskContract.cboContract.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem4.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControlItem4.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem6.Text = Inventec.Common.Resource.Get.Value("UC_KskContract.layoutControlItem6.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboContract()
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisKskContractViewFilter hisKskContractViewFilter = new HisKskContractViewFilter();
				hisKskContractViewFilter.IS_ACTIVE = (short)1;
				hisKskContractViewFilter.ORDER_DIRECTION = "DESC";
				hisKskContractViewFilter.ORDER_FIELD = "KSK_CONTRACT_CODE";
				listKskContract = new BackendAdapter(commonParam).Get<List<V_HIS_KSK_CONTRACT>>("api/HisKskContract/GetView", ApiConsumers.MosConsumer, hisKskContractViewFilter, commonParam).ToList();
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("KSK_CONTRACT_CODE", "Mã hợp đồng", 100, 1));
				list.Add(new ColumnInfo("WORK_PLACE_NAME", "Tên công ty", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("WORK_PLACE_NAME", "ID", list, true, 350);
				ControlEditorLoader.Load(cboContract, listKskContract, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboContract_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboContract.Enabled = false;
				cboContract.Enabled = true;
				if (cboContract.EditValue != null)
				{
					V_HIS_KSK_CONTRACT v_HIS_KSK_CONTRACT = listKskContract.FirstOrDefault((V_HIS_KSK_CONTRACT o) => o.ID == Parse.ToInt64(cboContract.EditValue.ToString()));
					if (v_HIS_KSK_CONTRACT != null)
					{
						lblNgayHetHan.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(v_HIS_KSK_CONTRACT.EXPIRY_DATE.GetValueOrDefault());
						lblNgayHieuLuc.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(v_HIS_KSK_CONTRACT.EFFECT_DATE.GetValueOrDefault());
						lblTenCongTy.Text = v_HIS_KSK_CONTRACT.WORK_PLACE_NAME;
						lblTyLeThanhToan.Text = System.Convert.ToInt64(v_HIS_KSK_CONTRACT.PAYMENT_RATIO * 100m) + "%";
					}
					else
					{
						lblNgayHetHan.Text = "";
						lblNgayHieuLuc.Text = "";
						lblTenCongTy.Text = "";
						lblTyLeThanhToan.Text = "";
					}
				}
				else
				{
					lblNgayHetHan.Text = "";
					lblNgayHieuLuc.Text = "";
					lblTenCongTy.Text = "";
					lblTyLeThanhToan.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboContract_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string displayText = "";
				if (cboContract.EditValue != null && listKskContract != null && listKskContract.Count > 0)
				{
					V_HIS_KSK_CONTRACT v_HIS_KSK_CONTRACT = listKskContract.FirstOrDefault((V_HIS_KSK_CONTRACT o) => o.ID == Parse.ToInt64(cboContract.EditValue.ToString()));
					if (v_HIS_KSK_CONTRACT != null)
					{
						displayText = v_HIS_KSK_CONTRACT.KSK_CONTRACT_CODE + " - " + v_HIS_KSK_CONTRACT.WORK_PLACE_NAME;
					}
				}
				e.DisplayText = displayText;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void DisposeControl()
		{
			try
			{
				listKskContract = null;
				cboContract.EditValueChanged -= new EventHandler(cboContract_EditValueChanged);
				cboContract.CustomDisplayText -= new CustomDisplayTextEventHandler(cboContract_CustomDisplayText);
				base.Load -= new EventHandler(UC_KskContract_Load);
				gridLookUpEdit1View.GridControl.DataSource = null;
				cboContract.Properties.DataSource = null;
				layoutControlItem6 = null;
				layoutControlItem5 = null;
				layoutControlItem4 = null;
				layoutControlItem3 = null;
				layoutControlItem1 = null;
				gridLookUpEdit1View = null;
				cboContract = null;
				lblTenCongTy = null;
				lblTyLeThanhToan = null;
				lblNgayHieuLuc = null;
				lblNgayHetHan = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
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
			this.lblNgayHetHan = new DevExpress.XtraEditors.LabelControl();
			this.lblNgayHieuLuc = new DevExpress.XtraEditors.LabelControl();
			this.cboContract = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.lblTyLeThanhToan = new DevExpress.XtraEditors.LabelControl();
			this.lblTenCongTy = new DevExpress.XtraEditors.LabelControl();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.cboContract.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.lblNgayHetHan);
			this.layoutControl1.Controls.Add(this.lblNgayHieuLuc);
			this.layoutControl1.Controls.Add(this.cboContract);
			this.layoutControl1.Controls.Add(this.lblTyLeThanhToan);
			this.layoutControl1.Controls.Add(this.lblTenCongTy);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(46, 123, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1260, 26);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.lblNgayHetHan.Location = new System.Drawing.Point(1079, 2);
			this.lblNgayHetHan.Name = "lblNgayHetHan";
			this.lblNgayHetHan.Size = new System.Drawing.Size(179, 20);
			this.lblNgayHetHan.StyleController = this.layoutControl1;
			this.lblNgayHetHan.TabIndex = 7;
			this.lblNgayHieuLuc.Location = new System.Drawing.Point(854, 2);
			this.lblNgayHieuLuc.Name = "lblNgayHieuLuc";
			this.lblNgayHieuLuc.Size = new System.Drawing.Size(126, 20);
			this.lblNgayHieuLuc.StyleController = this.layoutControl1;
			this.lblNgayHieuLuc.TabIndex = 6;
			this.cboContract.Location = new System.Drawing.Point(97, 2);
			this.cboContract.Name = "cboContract";
			this.cboContract.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboContract.Properties.AutoComplete = false;
			this.cboContract.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboContract.Properties.NullText = "";
			this.cboContract.Properties.View = this.gridLookUpEdit1View;
			this.cboContract.Size = new System.Drawing.Size(173, 20);
			this.cboContract.StyleController = this.layoutControl1;
			this.cboContract.TabIndex = 4;
			this.cboContract.EditValueChanged += new System.EventHandler(cboContract_EditValueChanged);
			this.cboContract.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(cboContract_CustomDisplayText);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.lblTyLeThanhToan.Location = new System.Drawing.Point(686, 2);
			this.lblTyLeThanhToan.Name = "lblTyLeThanhToan";
			this.lblTyLeThanhToan.Size = new System.Drawing.Size(69, 20);
			this.lblTyLeThanhToan.StyleController = this.layoutControl1;
			this.lblTyLeThanhToan.TabIndex = 5;
			this.lblTenCongTy.Location = new System.Drawing.Point(369, 2);
			this.lblTenCongTy.Name = "lblTenCongTy";
			this.lblTenCongTy.Size = new System.Drawing.Size(218, 20);
			this.lblTenCongTy.StyleController = this.layoutControl1;
			this.lblTenCongTy.TabIndex = 4;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[5] { this.layoutControlItem1, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.layoutControlItem6 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1260, 26);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.cboContract;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(272, 26);
			this.layoutControlItem1.Text = "Hợp đồng:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem3.Control = this.lblTenCongTy;
			this.layoutControlItem3.Location = new System.Drawing.Point(272, 0);
			this.layoutControlItem3.MaxSize = new System.Drawing.Size(0, 24);
			this.layoutControlItem3.MinSize = new System.Drawing.Size(100, 24);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(317, 26);
			this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem3.Text = "Tên công ty:";
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem3.TextToControlDistance = 5;
			this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem4.Control = this.lblTyLeThanhToan;
			this.layoutControlItem4.Location = new System.Drawing.Point(589, 0);
			this.layoutControlItem4.MaxSize = new System.Drawing.Size(0, 24);
			this.layoutControlItem4.MinSize = new System.Drawing.Size(100, 24);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(168, 26);
			this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem4.Text = "Tỷ lệ thanh toán:";
			this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem4.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem4.TextToControlDistance = 5;
			this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem5.Control = this.lblNgayHieuLuc;
			this.layoutControlItem5.Location = new System.Drawing.Point(757, 0);
			this.layoutControlItem5.MaxSize = new System.Drawing.Size(0, 24);
			this.layoutControlItem5.MinSize = new System.Drawing.Size(100, 24);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(225, 26);
			this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem5.Text = "Ngày hiệu lực:";
			this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem5.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem5.TextToControlDistance = 5;
			this.layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem6.Control = this.lblNgayHetHan;
			this.layoutControlItem6.Location = new System.Drawing.Point(982, 0);
			this.layoutControlItem6.MaxSize = new System.Drawing.Size(0, 24);
			this.layoutControlItem6.MinSize = new System.Drawing.Size(100, 24);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(278, 26);
			this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem6.Text = "Ngày hết hạn:";
			this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem6.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem6.TextToControlDistance = 5;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.layoutControl1);
			base.Name = "UC_KskContract";
			base.Size = new System.Drawing.Size(1260, 26);
			base.Load += new System.EventHandler(UC_KskContract_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.cboContract.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			base.ResumeLayout(false);
		}
	}
}
