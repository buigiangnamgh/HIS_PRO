using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using His.UC.UCHein.Base;
using His.UC.UCHein.Resources;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Resource;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using MOS.EFMODEL.DataModels;
using MOS.SDO;

namespace His.UC.UCHein.Design
{
	public class frmPatientChoice : Form
	{
		private FillDataPatientSDOToRegisterForm updatePatientInfo;

		private List<HisPatientSDO> currentListPatient;

		private List<HIS_GENDER> Genders;

		private Dictionary<long, string> dicGender;

		private IContainer components;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private SimpleButton btnClose;

		private GridControl grdInformation;

		private GridView gridView1;

		private GridColumn grdChoose;

		private GridColumn grdName;

		private GridColumn grdDate;

		private GridColumn grdGender;

		private GridColumn grdAddress;

		private RepositoryItemRadioGroup radianChoose;

		private LayoutControlItem lciPatientInformation;

		private LayoutControlItem layoutControlItem2;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GridColumn grdCode;

		private LabelControl lblDescription;

		private LayoutControlItem layoutControlItem3;

		public frmPatientChoice(List<HisPatientSDO> currentListPatient, FillDataPatientSDOToRegisterForm updatePatientInfo, List<HIS_GENDER> genders)
		{
			this.currentListPatient = currentListPatient;
			this.updatePatientInfo = updatePatientInfo;
			Genders = genders;
			InitializeComponent();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Hide();
		}

		private void PopupPatientInformation_Load(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				SetCaptionByLanguageKeyNew();
				if (Genders != null)
				{
					foreach (HIS_GENDER gender in Genders)
					{
						dicGender.Add(gender.ID, gender.GENDER_NAME);
					}
				}
				grdInformation.DataSource = currentListPatient;
				gridView1.FocusedRowHandle = 0;
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				WaitingManager.Hide();
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguagefrmPatientChoice = new ResourceManager("His.UC.UCHein.Design.Resources.Lang", typeof(frmPatientChoice).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.layoutControl1.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				lblDescription.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.lblDescription.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				btnClose.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.btnClose.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdChoose.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdChoose.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdCode.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdCode.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdName.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdName.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdDate.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdDate.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdGender.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdGender.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdAddress.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdAddress.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.layoutControlItem3.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetCaptionByLanguageKeyNew()
		{
			try
			{
				ResourceLanguageManager.LanguagefrmPatientChoice = new ResourceManager("His.UC.UCHein.Resources.Lang", typeof(frmPatientChoice).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.layoutControl1.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				lblDescription.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.lblDescription.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				btnClose.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.btnClose.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdChoose.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdChoose.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdCode.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdCode.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdName.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdName.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdDate.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdDate.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdGender.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdGender.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				grdAddress.Caption = Inventec.Common.Resource.Get.Value("frmPatientChoice.grdAddress.Caption", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.layoutControlItem3.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmPatientChoice.Text", ResourceLanguageManager.LanguagefrmPatientChoice, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				HisPatientSDO hisPatientSDO = (HisPatientSDO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (e.Column.FieldName == "DOB_DISPLAY")
				{
					try
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(hisPatientSDO.DOB);
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Warn("Loi set gia tri cho cot ngay tao CREATE_TIME", ex);
						return;
					}
				}
				if (e.Column.FieldName == "GENDER_NAME")
				{
					try
					{
						e.Value = dicGender[hisPatientSDO.GENDER_ID];
						return;
					}
					catch (Exception ex2)
					{
						LogSystem.Warn("Loi set gia tri cho cot GENDER_NAME", ex2);
						return;
					}
				}
			}
			catch (Exception ex3)
			{
				LogSystem.Warn(ex3);
			}
		}

		private void ProcessSelectedPatientSdo(ref HisPatientSDO patient)
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientWarningSDO hisPatientWarningSDO = new BackendAdapter(commonParam).Get<List<HisPatientWarningSDO>>("api/HisPatient/GetSdoAdvance", ApiConsumerStore.MosConsumer, patient.ID, commonParam).SingleOrDefault();
				if (hisPatientWarningSDO == null)
				{
					throw new ArgumentNullException("patientWarningSDO");
				}
				patient.PreviousPrescriptions = hisPatientWarningSDO.PreviousPrescriptions;
				patient.PreviousDebtTreatments = hisPatientWarningSDO.PreviousDebtTreatments;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			try
			{
				HisPatientSDO patient = (HisPatientSDO)gridView1.GetFocusedRow();
				if (patient != null)
				{
					ProcessSelectedPatientSdo(ref patient);
					updatePatientInfo(patient);
					Close();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void grdInformation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					HisPatientSDO patient = (HisPatientSDO)gridView1.GetFocusedRow();
					if (patient != null)
					{
						ProcessSelectedPatientSdo(ref patient);
						updatePatientInfo(patient);
						Close();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void grdInformation_DoubleClick(object sender, EventArgs e)
		{
			try
			{
				HisPatientSDO patient = (HisPatientSDO)gridView1.GetFocusedRow();
				if (patient != null)
				{
					ProcessSelectedPatientSdo(ref patient);
					updatePatientInfo(patient);
					Close();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView1_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					return;
				}
				GridView gridView = sender as GridView;
				GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.Location);
				if (!gridHitInfo.InRowCell || !(gridHitInfo.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit)))
				{
					return;
				}
				gridView.FocusedRowHandle = gridHitInfo.RowHandle;
				gridView.FocusedColumn = gridHitInfo.Column;
				gridView.ShowEditor();
				CheckEdit checkEdit = gridView.ActiveEditor as CheckEdit;
				Rectangle glyphRect = ((CheckEditViewInfo)checkEdit.GetViewInfo()).CheckInfo.GlyphRect;
				GridViewInfo gridViewInfo = gridView.GetViewInfo() as GridViewInfo;
				Rectangle rectangle = new Rectangle(gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.X + glyphRect.X, gridViewInfo.GetGridCellInfo(gridHitInfo).Bounds.Y + glyphRect.Y, glyphRect.Width, glyphRect.Height);
				if (!rectangle.Contains(e.Location))
				{
					gridView.CloseEditor();
					if (!gridView.IsCellSelected(gridHitInfo.RowHandle, gridHitInfo.Column))
					{
						gridView.SelectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
					else
					{
						gridView.UnselectCell(gridHitInfo.RowHandle, gridHitInfo.Column);
					}
				}
				else
				{
					checkEdit.Checked = !checkEdit.Checked;
					gridView.CloseEditor();
				}
				(e as DXMouseEventArgs).Handled = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void frmPatientChoice_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				dicGender = null;
				Genders = null;
				currentListPatient = null;
				updatePatientInfo = null;
				btnClose.Click -= new EventHandler(btnClose_Click);
				grdInformation.DoubleClick -= new EventHandler(grdInformation_DoubleClick);
				grdInformation.PreviewKeyDown -= new PreviewKeyDownEventHandler(grdInformation_PreviewKeyDown);
				gridView1.CustomUnboundColumnData -= new CustomColumnDataEventHandler(gridView1_CustomUnboundColumnData);
				gridView1.MouseDown -= new MouseEventHandler(gridView1_MouseDown);
				base.Load -= new EventHandler(PopupPatientInformation_Load);
				gridView1.GridControl.DataSource = null;
				grdInformation.DataSource = null;
				layoutControlItem3 = null;
				lblDescription = null;
				grdCode = null;
				repositoryItemCheckEdit1 = null;
				layoutControlItem2 = null;
				lciPatientInformation = null;
				radianChoose = null;
				grdAddress = null;
				grdGender = null;
				grdDate = null;
				grdName = null;
				grdChoose = null;
				gridView1 = null;
				grdInformation = null;
				btnClose = null;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(His.UC.UCHein.Design.frmPatientChoice));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.lblDescription = new DevExpress.XtraEditors.LabelControl();
			this.btnClose = new DevExpress.XtraEditors.SimpleButton();
			this.grdInformation = new DevExpress.XtraGrid.GridControl();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.grdChoose = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
			this.grdCode = new DevExpress.XtraGrid.Columns.GridColumn();
			this.grdName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.grdDate = new DevExpress.XtraGrid.Columns.GridColumn();
			this.grdGender = new DevExpress.XtraGrid.Columns.GridColumn();
			this.grdAddress = new DevExpress.XtraGrid.Columns.GridColumn();
			this.radianChoose = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lciPatientInformation = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.grdInformation).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.radianChoose).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciPatientInformation).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.lblDescription);
			this.layoutControl1.Controls.Add(this.btnClose);
			this.layoutControl1.Controls.Add(this.grdInformation);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(456, 128, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1114, 511);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.lblDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblDescription.Location = new System.Drawing.Point(2, 487);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(1035, 13);
			this.lblDescription.StyleController = this.layoutControl1;
			this.lblDescription.TabIndex = 7;
			this.lblDescription.Text = "Chọn bệnh nhân bằng cách bấm enter hoặc nháy đúp chuột vào bệnh nhân. Thêm hồ sơ bệnh nhân mới chọn Bỏ qua";
			this.btnClose.Location = new System.Drawing.Point(1041, 487);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(71, 22);
			this.btnClose.StyleController = this.layoutControl1;
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Bỏ qua";
			this.btnClose.Click += new System.EventHandler(btnClose_Click);
			this.grdInformation.Location = new System.Drawing.Point(2, 2);
			this.grdInformation.MainView = this.gridView1;
			this.grdInformation.Name = "grdInformation";
			this.grdInformation.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[2] { this.radianChoose, this.repositoryItemCheckEdit1 });
			this.grdInformation.Size = new System.Drawing.Size(1110, 481);
			this.grdInformation.TabIndex = 4;
			this.grdInformation.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridView1 });
			this.grdInformation.DoubleClick += new System.EventHandler(grdInformation_DoubleClick);
			this.grdInformation.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(grdInformation_PreviewKeyDown);
			this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[6] { this.grdChoose, this.grdCode, this.grdName, this.grdDate, this.grdGender, this.grdAddress });
			this.gridView1.GridControl = this.grdInformation;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsView.ShowDetailButtons = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.gridView1.OptionsView.ShowIndicator = false;
			this.gridView1.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridView1_CustomUnboundColumnData);
			this.gridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(gridView1_MouseDown);
			this.grdChoose.AppearanceCell.Options.UseTextOptions = true;
			this.grdChoose.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.grdChoose.Caption = "Chọn";
			this.grdChoose.ColumnEdit = this.repositoryItemCheckEdit1;
			this.grdChoose.Name = "grdChoose";
			this.grdChoose.OptionsColumn.ShowCaption = false;
			this.grdChoose.Width = 30;
			this.repositoryItemCheckEdit1.AutoHeight = false;
			this.repositoryItemCheckEdit1.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
			this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
			this.grdCode.Caption = "Mã bệnh nhân";
			this.grdCode.FieldName = "PATIENT_CODE";
			this.grdCode.Name = "grdCode";
			this.grdCode.OptionsColumn.AllowEdit = false;
			this.grdCode.Visible = true;
			this.grdCode.VisibleIndex = 0;
			this.grdCode.Width = 116;
			this.grdName.Caption = "Tên bệnh nhân";
			this.grdName.FieldName = "VIR_PATIENT_NAME";
			this.grdName.Name = "grdName";
			this.grdName.OptionsColumn.AllowEdit = false;
			this.grdName.Visible = true;
			this.grdName.VisibleIndex = 1;
			this.grdName.Width = 150;
			this.grdDate.AppearanceCell.Options.UseTextOptions = true;
			this.grdDate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.grdDate.Caption = "Ngày sinh";
			this.grdDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.grdDate.FieldName = "DOB_DISPLAY";
			this.grdDate.Name = "grdDate";
			this.grdDate.OptionsColumn.AllowEdit = false;
			this.grdDate.Tag = new System.DateTime(2016, 10, 1, 11, 58, 9, 631);
			this.grdDate.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.grdDate.Visible = true;
			this.grdDate.VisibleIndex = 2;
			this.grdDate.Width = 120;
			this.grdGender.Caption = "Giới tính";
			this.grdGender.FieldName = "GENDER_NAME";
			this.grdGender.Name = "grdGender";
			this.grdGender.OptionsColumn.AllowEdit = false;
			this.grdGender.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.grdGender.Visible = true;
			this.grdGender.VisibleIndex = 3;
			this.grdGender.Width = 97;
			this.grdAddress.Caption = "Địa chỉ";
			this.grdAddress.FieldName = "VIR_ADDRESS";
			this.grdAddress.Name = "grdAddress";
			this.grdAddress.OptionsColumn.AllowEdit = false;
			this.grdAddress.Visible = true;
			this.grdAddress.VisibleIndex = 4;
			this.grdAddress.Width = 595;
			this.radianChoose.Name = "radianChoose";
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[3] { this.lciPatientInformation, this.layoutControlItem2, this.layoutControlItem3 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1114, 511);
			this.layoutControlGroup1.TextVisible = false;
			this.lciPatientInformation.Control = this.grdInformation;
			this.lciPatientInformation.Location = new System.Drawing.Point(0, 0);
			this.lciPatientInformation.Name = "lciPatientInformation";
			this.lciPatientInformation.Size = new System.Drawing.Size(1114, 485);
			this.lciPatientInformation.TextSize = new System.Drawing.Size(0, 0);
			this.lciPatientInformation.TextVisible = false;
			this.layoutControlItem2.Control = this.btnClose;
			this.layoutControlItem2.Location = new System.Drawing.Point(1039, 485);
			this.layoutControlItem2.MaxSize = new System.Drawing.Size(75, 26);
			this.layoutControlItem2.MinSize = new System.Drawing.Size(70, 26);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(75, 26);
			this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem3.Control = this.lblDescription;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 485);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(1039, 26);
			this.layoutControlItem3.Text = resources.GetString("layoutControlItem3.Text");
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextToControlDistance = 0;
			this.layoutControlItem3.TextVisible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1114, 511);
			base.Controls.Add(this.layoutControl1);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.Name = "frmPatientChoice";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Chọn thông tin bệnh nhân";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmPatientChoice_FormClosed);
			base.Load += new System.EventHandler(PopupPatientInformation_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.grdInformation).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.radianChoose).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciPatientInformation).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			base.ResumeLayout(false);
		}
	}
}
