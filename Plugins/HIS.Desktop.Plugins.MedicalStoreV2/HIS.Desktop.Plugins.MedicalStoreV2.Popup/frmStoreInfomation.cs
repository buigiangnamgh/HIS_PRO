using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.Location;
using Inventec.Common.Adapter;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.MedicalStoreV2.Popup
{
	public class frmStoreInfomation : Form
	{
		private List<HIS_LOCATION_STORE> lstLocationStore;

		private long mediRecordId;

		private HIS_MEDI_RECORD currentMediRecord;

		private Action refeshData;

		private long? originalLocationStoreId;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private SimpleButton btnSave;

		private TextEdit txtStoreCode;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private EmptySpaceItem emptySpaceItem1;

		private LayoutControlItem layoutControlItem1;

		private TreeListLookUpEdit cboLoacationStore;

		private TreeList treeListLookUpEdit1TreeList;

		public frmStoreInfomation(long _mediRecordId, Action _refeshData)
		{
			InitializeComponent();
			try
			{
				mediRecordId = _mediRecordId;
				refeshData = _refeshData;
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

		private void frmStoreInfomation_Load(object sender, EventArgs e)
		{
			try
			{
				LoadComboLocationStore();
				LoadMediRecordData();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadMediRecordData()
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisMediRecordFilter hisMediRecordFilter = new HisMediRecordFilter();
				hisMediRecordFilter.ID = mediRecordId;
				List<HIS_MEDI_RECORD> list = new BackendAdapter(commonParam).Get<List<HIS_MEDI_RECORD>>("api/HisMediRecord/Get", ApiConsumers.MosConsumer, hisMediRecordFilter, commonParam);
				if (list != null && list.Count > 0)
				{
					currentMediRecord = list.First();
					SetDefaultData();
				}
				else
				{
					MessageBox.Show("Không tìm thấy thông tin bệnh án", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					Close();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadComboLocationStore()
		{
			try
			{
				lstLocationStore = (from o in BackendDataWorker.Get<HIS_LOCATION_STORE>()
					where o.IS_ACTIVE == 1
					orderby o.LOCATION_STORE_CODE, o.LOCATION_STORE_NAME
					select o).ToList();
				cboLoacationStore.Properties.DataSource = lstLocationStore;
				cboLoacationStore.Properties.DisplayMember = "LOCATION_STORE_NAME";
				cboLoacationStore.Properties.ValueMember = "ID";
				TreeList treeList = cboLoacationStore.Properties.TreeList;
				treeList.KeyFieldName = "ID";
				treeList.ParentFieldName = "PARENT_ID";
				treeList.Columns.Clear();
				TreeListColumn treeListColumn = treeList.Columns.Add();
				treeListColumn.FieldName = "LOCATION_STORE_CODE";
				treeListColumn.Caption = "Mã";
				treeListColumn.Visible = true;
				treeListColumn.VisibleIndex = 0;
				TreeListColumn treeListColumn2 = treeList.Columns.Add();
				treeListColumn2.FieldName = "LOCATION_STORE_NAME";
				treeListColumn2.Caption = "Tên vị trí";
				treeListColumn2.Visible = true;
				treeListColumn2.VisibleIndex = 1;
				cboLoacationStore.Properties.PopupFormSize = new Size(600, 300);
				cboLoacationStore.Properties.ImmediatePopup = true;
				treeList.OptionsView.ShowCheckBoxes = false;
				treeList.OptionsSelection.EnableAppearanceFocusedCell = false;
				treeList.OptionsView.ShowIndicator = false;
				treeList.OptionsView.AutoWidth = true;
				treeList.OptionsBehavior.EnableFiltering = true;
				treeList.ExpandAll();
				treeList.BestFitColumns();
				cboLoacationStore.Properties.Buttons.Clear();
				cboLoacationStore.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
				cboLoacationStore.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetDefaultData()
		{
			try
			{
				if (currentMediRecord != null)
				{
					txtStoreCode.Text = currentMediRecord.STORE_CODE;
					txtStoreCode.Enabled = false;
					if (currentMediRecord.LOCATION_STORE_ID.HasValue)
					{
						cboLoacationStore.EditValue = currentMediRecord.LOCATION_STORE_ID.Value;
						originalLocationStoreId = currentMediRecord.LOCATION_STORE_ID.Value;
					}
					else
					{
						originalLocationStoreId = null;
					}
					btnSave.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			try
			{
				CommonParam commonParam = new CommonParam();
				bool value = false;
				if (currentMediRecord == null)
				{
					MessageBox.Show("Không có thông tin bệnh án để cập nhật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				HIS_MEDI_RECORD val = new HIS_MEDI_RECORD();
				DataObjectMapper.Map<HIS_MEDI_RECORD>(val, currentMediRecord);
				if (cboLoacationStore.EditValue != null)
				{
					val.LOCATION_STORE_ID = Convert.ToInt64(cboLoacationStore.EditValue);
				}
				else
				{
					val.LOCATION_STORE_ID = null;
				}
				val.LOCATION_NODE_ID = null;
				val.LOCATION_FULL_PATH = null;
				WaitingManager.Show();
				LogSystem.Debug(LogUtil.TraceData("HisMediRecord/Update Input", val));
				HIS_MEDI_RECORD val2 = new BackendAdapter(commonParam).Post<HIS_MEDI_RECORD>("api/HisMediRecord/Update", ApiConsumers.MosConsumer, val, commonParam);
				WaitingManager.Hide();
				LogSystem.Debug(LogUtil.TraceData("HisMediRecord/Update Output", val2));
				if (val2 != null)
				{
					value = true;
					MessageManager.Show(this, commonParam, value);
					if (refeshData != null)
					{
						refeshData();
					}
					Close();
				}
				else
				{
					MessageManager.Show(this, commonParam, value);
				}
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
				MessageBox.Show("Có lỗi xảy ra khi cập nhật thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void cboLoacationStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboLoacationStore.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLoacationStore_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				long? currentValue = null;
				if (cboLoacationStore.EditValue != null)
				{
					currentValue = Convert.ToInt64(cboLoacationStore.EditValue);
					HIS_LOCATION_STORE val = lstLocationStore.FirstOrDefault((HIS_LOCATION_STORE o) => o.ID == currentValue);
				}
				btnSave.Enabled = currentValue != originalLocationStoreId;
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
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.txtStoreCode = new DevExpress.XtraEditors.TextEdit();
			this.cboLoacationStore = new DevExpress.XtraEditors.TreeListLookUpEdit();
			this.treeListLookUpEdit1TreeList = new DevExpress.XtraTreeList.TreeList();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboLoacationStore.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.treeListLookUpEdit1TreeList).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.txtStoreCode);
			this.layoutControl1.Controls.Add(this.cboLoacationStore);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(657, 93);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnSave.Location = new System.Drawing.Point(563, 59);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(91, 27);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Lưu";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.txtStoreCode.Location = new System.Drawing.Point(128, 3);
			this.txtStoreCode.Name = "txtStoreCode";
			this.txtStoreCode.Size = new System.Drawing.Size(526, 22);
			this.txtStoreCode.StyleController = this.layoutControl1;
			this.txtStoreCode.TabIndex = 4;
			this.cboLoacationStore.Location = new System.Drawing.Point(128, 31);
			this.cboLoacationStore.Name = "cboLoacationStore";
			this.cboLoacationStore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.cboLoacationStore.Properties.NullText = "";
			this.cboLoacationStore.Properties.PopupSizeable = false;
			this.cboLoacationStore.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboLoacationStore.Properties.TreeList = this.treeListLookUpEdit1TreeList;
			this.cboLoacationStore.Size = new System.Drawing.Size(526, 22);
			this.cboLoacationStore.StyleController = this.layoutControl1;
			this.cboLoacationStore.TabIndex = 5;
			this.cboLoacationStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboLoacationStore_ButtonClick);
			this.cboLoacationStore.EditValueChanged += new System.EventHandler(cboLoacationStore_EditValueChanged);
			this.treeListLookUpEdit1TreeList.Location = new System.Drawing.Point(0, 0);
			this.treeListLookUpEdit1TreeList.Name = "treeListLookUpEdit1TreeList";
			this.treeListLookUpEdit1TreeList.OptionsBehavior.EnableFiltering = true;
			this.treeListLookUpEdit1TreeList.OptionsView.ShowIndentAsRowStyle = true;
			this.treeListLookUpEdit1TreeList.Size = new System.Drawing.Size(400, 200);
			this.treeListLookUpEdit1TreeList.TabIndex = 0;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[4] { this.layoutControlItem2, this.layoutControlItem3, this.emptySpaceItem1, this.layoutControlItem1 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 4;
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(657, 93);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem2.Control = this.cboLoacationStore;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 28);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(657, 28);
			this.layoutControlItem2.Text = "Vị trí:";
			this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(120, 20);
			this.layoutControlItem2.TextToControlDistance = 5;
			this.layoutControlItem3.Control = this.btnSave;
			this.layoutControlItem3.Location = new System.Drawing.Point(560, 56);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(97, 37);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 56);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(560, 37);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.txtStoreCode;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(657, 28);
			this.layoutControlItem1.Text = "Mã lưu trữ: ";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(120, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(657, 93);
			base.Controls.Add(this.layoutControl1);
			base.Name = "frmStoreInfomation";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Thông tin lưu trữ";
			base.Load += new System.EventHandler(frmStoreInfomation_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboLoacationStore.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.treeListLookUpEdit1TreeList).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
