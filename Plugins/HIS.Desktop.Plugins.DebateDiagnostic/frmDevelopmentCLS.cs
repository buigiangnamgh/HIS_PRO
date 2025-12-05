using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Plugins.DebateDiagnostic.UcDebateDetail;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;

namespace HIS.Desktop.Plugins.DebateDiagnostic
{
	public class frmDevelopmentCLS : Form
	{
		internal long treatmentId;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private TextEdit txtSearch;

		private DateEdit deStartTime;

		private DateEdit deEndTime;

		private GridControl gcDevelopmentCLS;

		private GridView gvDevelopmentCLS;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private SimpleButton btnSelect;

		private GridColumn gridColumn1;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private LayoutControlItem layoutControlItem5;

		private EmptySpaceItem emptySpaceItem1;

		public UcOther ParentUcOther { get; set; }

		private HIS_TRACKING tracking { get; set; }

		public frmDevelopmentCLS(long treatmentId)
		{
			InitializeComponent();
			this.treatmentId = treatmentId;
		}

		private void frmDevelopmentCLS_Load(object sender, EventArgs e)
		{
			try
			{
				deStartTime.EditValue = DateTime.Today.Date;
				deEndTime.EditValue = DateTime.Today.Date.AddHours(23.0).AddMinutes(59.0).AddSeconds(59.0);
				LoadTracking();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gvDevelopmentCLS_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound || (IList)((BaseView)sender).DataSource == null || ((IList)((BaseView)sender).DataSource).Count <= 0)
				{
					return;
				}
				HIS_TRACKING val = (HIS_TRACKING)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (val != null)
				{
					if (e.Column.FieldName == "TRACKING_TIMEStr")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(val.TRACKING_TIME);
					}
				}
				else
				{
					e.Value = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (keyData == (Keys.S | Keys.Control))
			{
				btnSelect.PerformClick();
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
		{
		}

		private void LoadTracking()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				CommonParam val = new CommonParam();
				HisTrackingFilter val2 = new HisTrackingFilter
				{
					TREATMENT_ID = treatmentId,
					TRACKING_TIME_FROM = long.Parse(deStartTime.DateTime.ToString("yyyyMMddHHmmss")),
					TRACKING_TIME_TO = long.Parse(deEndTime.DateTime.ToString("yyyyMMddHHmmss"))
				};
				string obj = txtSearch.Text;
				((FilterBase)val2).KEY_WORD = ((obj != null) ? obj.Trim() : null);
				HisTrackingFilter val3 = val2;
				List<HIS_TRACKING> list = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_TRACKING>>("api/HisTracking/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val3, val);
				if (list != null && list.Any())
				{
					gcDevelopmentCLS.DataSource = list;
				}
				else
				{
					gcDevelopmentCLS.DataSource = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			try
			{
				int[] selectedRows = gvDevelopmentCLS.GetSelectedRows();
				List<HIS_TRACKING> list = new List<HIS_TRACKING>();
				int[] array = selectedRows;
				foreach (int rowHandle in array)
				{
					object rowCellValue = gvDevelopmentCLS.GetRowCellValue(rowHandle, "TRACKING_TIME");
					long tRACKING_TIME = System.Convert.ToInt64((rowCellValue != null) ? rowCellValue.ToString() : null);
					object rowCellValue2 = gvDevelopmentCLS.GetRowCellValue(rowHandle, "CONTENT");
					string cONTENT = ((rowCellValue2 != null) ? rowCellValue2.ToString() : null);
					object rowCellValue3 = gvDevelopmentCLS.GetRowCellValue(rowHandle, "SUBCLINICAL_PROCESSES");
					string sUBCLINICAL_PROCESSES = ((rowCellValue3 != null) ? rowCellValue3.ToString() : null);
					list.Add(new HIS_TRACKING
					{
						TRACKING_TIME = tRACKING_TIME,
						CONTENT = cONTENT,
						SUBCLINICAL_PROCESSES = sUBCLINICAL_PROCESSES
					});
				}
				string treatmentTrackingText = string.Join("; ", list.OrderBy((HIS_TRACKING x) => x.TRACKING_TIME).Select(delegate(HIS_TRACKING x)
				{
					string cONTENT2 = x.CONTENT;
					string text = ((cONTENT2 != null) ? cONTENT2.Trim() : null) ?? "";
					string sUBCLINICAL_PROCESSES2 = x.SUBCLINICAL_PROCESSES;
					string text2 = ((sUBCLINICAL_PROCESSES2 != null) ? sUBCLINICAL_PROCESSES2.Trim() : null) ?? "";
					return (text + " + " + text2).Trim(' ', '+');
				}));
				if (ParentUcOther != null)
				{
					ParentUcOther.SetTreatmentTrackingText(treatmentTrackingText);
				}
				Close();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtSearch_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				string text = txtSearch.Text.Trim().ToLower();
				if (e.KeyCode == Keys.Return)
				{
					LoadTracking();
					if (gvDevelopmentCLS.RowCount > 0)
					{
						gvDevelopmentCLS.FocusedRowHandle = 0;
						gvDevelopmentCLS.FocusedColumn = gvDevelopmentCLS.VisibleColumns[0];
						gvDevelopmentCLS.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void deStartTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadTracking();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void deEndTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadTracking();
				}
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
			this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
			this.txtSearch = new DevExpress.XtraEditors.TextEdit();
			this.deStartTime = new DevExpress.XtraEditors.DateEdit();
			this.deEndTime = new DevExpress.XtraEditors.DateEdit();
			this.gcDevelopmentCLS = new DevExpress.XtraGrid.GridControl();
			this.gvDevelopmentCLS = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtSearch.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.deStartTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.deStartTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.deEndTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.deEndTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gcDevelopmentCLS).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gvDevelopmentCLS).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnSelect);
			this.layoutControl1.Controls.Add(this.txtSearch);
			this.layoutControl1.Controls.Add(this.deStartTime);
			this.layoutControl1.Controls.Add(this.deEndTime);
			this.layoutControl1.Controls.Add(this.gcDevelopmentCLS);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(776, 442);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnSelect.Location = new System.Drawing.Point(668, 418);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(106, 22);
			this.btnSelect.StyleController = this.layoutControl1;
			this.btnSelect.TabIndex = 8;
			this.btnSelect.Text = "Chọn (ctrl S)";
			this.btnSelect.Click += new System.EventHandler(btnSelect_Click);
			this.txtSearch.Location = new System.Drawing.Point(435, 2);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Properties.NullValuePrompt = "Từ khóa tìm kiếm";
			this.txtSearch.Properties.ShowNullValuePromptWhenFocused = true;
			this.txtSearch.Size = new System.Drawing.Size(339, 20);
			this.txtSearch.StyleController = this.layoutControl1;
			this.txtSearch.TabIndex = 7;
			this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(txtSearch_KeyDown);
			this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSearch_KeyPress);
			this.deStartTime.EditValue = null;
			this.deStartTime.Location = new System.Drawing.Point(52, 2);
			this.deStartTime.Name = "deStartTime";
			this.deStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.deStartTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.deStartTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.deStartTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.deStartTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.deStartTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.deStartTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm:ss";
			this.deStartTime.Size = new System.Drawing.Size(180, 20);
			this.deStartTime.StyleController = this.layoutControl1;
			this.deStartTime.TabIndex = 6;
			this.deStartTime.KeyDown += new System.Windows.Forms.KeyEventHandler(deStartTime_KeyDown);
			this.deEndTime.EditValue = null;
			this.deEndTime.Location = new System.Drawing.Point(236, 2);
			this.deEndTime.Name = "deEndTime";
			this.deEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.deEndTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.deEndTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm:ss";
			this.deEndTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.deEndTime.Properties.EditFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
			this.deEndTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.deEndTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm:ss";
			this.deEndTime.Size = new System.Drawing.Size(195, 20);
			this.deEndTime.StyleController = this.layoutControl1;
			this.deEndTime.TabIndex = 5;
			this.deEndTime.KeyDown += new System.Windows.Forms.KeyEventHandler(deEndTime_KeyDown);
			this.gcDevelopmentCLS.Location = new System.Drawing.Point(2, 26);
			this.gcDevelopmentCLS.MainView = this.gvDevelopmentCLS;
			this.gcDevelopmentCLS.Name = "gcDevelopmentCLS";
			this.gcDevelopmentCLS.Size = new System.Drawing.Size(772, 388);
			this.gcDevelopmentCLS.TabIndex = 4;
			this.gcDevelopmentCLS.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gvDevelopmentCLS });
			this.gvDevelopmentCLS.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[3] { this.gridColumn1, this.gridColumn2, this.gridColumn3 });
			this.gvDevelopmentCLS.GridControl = this.gcDevelopmentCLS;
			this.gvDevelopmentCLS.Name = "gvDevelopmentCLS";
			this.gvDevelopmentCLS.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
			this.gvDevelopmentCLS.OptionsSelection.MultiSelect = true;
			this.gvDevelopmentCLS.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
			this.gvDevelopmentCLS.OptionsView.ShowGroupPanel = false;
			this.gvDevelopmentCLS.OptionsView.ShowIndicator = false;
			this.gvDevelopmentCLS.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gvDevelopmentCLS_CustomUnboundColumnData);
			this.gridColumn1.Caption = "Thời gian";
			this.gridColumn1.FieldName = "TRACKING_TIMEStr";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.AllowEdit = false;
			this.gridColumn1.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 1;
			this.gridColumn1.Width = 88;
			this.gridColumn2.Caption = "Diễn biến bệnh";
			this.gridColumn2.FieldName = "CONTENT";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.OptionsColumn.AllowEdit = false;
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 2;
			this.gridColumn2.Width = 250;
			this.gridColumn3.Caption = "Diễn biến CLS";
			this.gridColumn3.FieldName = "SUBCLINICAL_PROCESSES";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.OptionsColumn.AllowEdit = false;
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 3;
			this.gridColumn3.Width = 250;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[6] { this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.emptySpaceItem1 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(776, 442);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.gcDevelopmentCLS;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(776, 392);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.layoutControlItem2.Control = this.deEndTime;
			this.layoutControlItem2.Location = new System.Drawing.Point(234, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(199, 24);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem3.Control = this.deStartTime;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(234, 24);
			this.layoutControlItem3.Text = "Thời gian:";
			this.layoutControlItem3.TextSize = new System.Drawing.Size(47, 13);
			this.layoutControlItem4.Control = this.txtSearch;
			this.layoutControlItem4.Location = new System.Drawing.Point(433, 0);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(343, 24);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.Control = this.btnSelect;
			this.layoutControlItem5.Location = new System.Drawing.Point(666, 416);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(110, 26);
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 416);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(666, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(776, 442);
			base.Controls.Add(this.layoutControl1);
			base.Name = "frmDevelopmentCLS";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chọn diễn biến bệnh, diễn biến CLS tờ điều trị";
			base.Load += new System.EventHandler(frmDevelopmentCLS_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtSearch.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.deStartTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.deStartTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.deEndTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.deEndTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gcDevelopmentCLS).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gvDevelopmentCLS).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
