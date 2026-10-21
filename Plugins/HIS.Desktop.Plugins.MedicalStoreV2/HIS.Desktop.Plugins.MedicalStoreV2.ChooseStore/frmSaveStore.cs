using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;
using HIS.Desktop.Plugins.MedicalStoreV2.Popup;
using HIS.Desktop.Plugins.MedicalStoreV2.Resources;
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
using Inventec.Desktop.Common.Modules;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.SDO;

namespace HIS.Desktop.Plugins.MedicalStoreV2.ChooseStore
{
	public class frmSaveStore : Form
	{
		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private RefeshReference refeshData;

		private Module currentModule;

		private int positionHandle = -1;

		private List<HIS_DATA_STORE> dataStores = null;

		private bool isInit = true;

		private List<HIS_LOCATION_STORE> lstLocationStore;

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

		private DateEdit dtSaveTime;

		private LayoutControlItem layoutControlItem1;

		private GridLookUpEdit cboDataStore;

		private GridView gridLookUpEdit1View;

		private TextEdit txtDataStoreCode;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private GridLookUpEdit cboMediRecordType;

		private GridView gridLookUpEdit3View;

		private GridLookUpEdit cboProgram;

		private GridView gridLookUpEdit2View;

		private CheckEdit chkBANgoaiTru;

		private LayoutControlItem layoutControlItem5;

		private LayoutControlItem lciProgram;

		private LayoutControlItem layoutControlItem7;

		private EmptySpaceItem emptySpaceItem2;

		private EmptySpaceItem emptySpaceItem3;

		private DXValidationProvider dxValidationProvider1;

		private CheckEdit checkFilterDataStoreByEndInfo;

		private LayoutControlItem lciFilterDataStoreByEndInfo;

		private CheckEdit checkFilterDataStoreByTreatmentEndType;

		private LayoutControlItem lciShowDataStoreByEndTreatmentType;

		private CheckEdit chkAutoSetStoreCode;

		private TextEdit txtStoreCode;

		private LayoutControlItem lciStoreCode;

		private LayoutControlItem layoutControlItem8;

		private LinkLabel linkLabel1;

		private LayoutControlItem layoutControlItem9;

		private CheckEdit chkIsTreatmentType;

		private LayoutControlItem layoutControlItem6;

		private EmptySpaceItem emptySpaceItem1;

		private CheckEdit chkUseEndCode;

		private LayoutControlItem layoutControlItem10;

		private TextEdit txtLocationStoreCode;

		private LayoutControlItem layoutControlItem11;

		private LayoutControlItem layoutControlItem12;

		private TreeListLookUpEdit cboLoacationStore;

		private TreeList treeList1;

		internal List<TreatmentADO> currentTreatment { get; set; }

		public frmSaveStore(Module currentModule, List<TreatmentADO> currentMediRecord, RefeshReference refeshData, List<ControlStateRDO> controlStateRDO, ControlStateWorker ctrlStateWorker)
		{
			InitializeComponent();
			try
			{
				this.currentModule = currentModule;
				currentTreatment = currentMediRecord;
				this.refeshData = refeshData;
				controlStateWorker = ctrlStateWorker;
				currentControlStateRDO = controlStateRDO;
				SetIcon();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public frmSaveStore(Module currentModule, List<L_HIS_TREATMENT_3> lstTreatments, RefeshReference refeshData)
		{
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
				LoadComboLocationStore();
				InitControlState();
				SetDefaultData();
				FocusButtonSave();
				LogSystem.Info("currentTreatment: " + LogUtil.TraceData(LogUtil.GetMemberName(() => currentTreatment), currentTreatment));
				if (currentTreatment != null && currentTreatment.Count > 1)
				{
					chkAutoSetStoreCode.Checked = false;
					chkAutoSetStoreCode.Enabled = false;
				}
				TreatmentADO treatmentADO = currentTreatment.FirstOrDefault();
				if (!string.IsNullOrEmpty(((L_HIS_TREATMENT_3)treatmentADO).RECORD_INSPECTION_REJECT_NOTE) || !string.IsNullOrEmpty(((L_HIS_TREATMENT_3)treatmentADO).REJECT_STORE_REASON))
				{
					LogSystem.Warn("Visible true");
				}
				else
				{
					linkLabel1.Text = "";
					LogSystem.Warn("Visible false");
				}
				isInit = false;
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
				cboLoacationStore.Properties.ImmediatePopup = true;
				cboLoacationStore.Properties.PopupFormMinSize = new Size(420, 300);
				cboLoacationStore.Properties.PopupFormSize = new Size(520, 320);
				treeList.OptionsView.ShowCheckBoxes = false;
				treeList.OptionsSelection.EnableAppearanceFocusedCell = false;
				treeList.OptionsView.ShowIndicator = false;
				treeList.OptionsView.AutoWidth = false;
				treeList.OptionsBehavior.EnableFiltering = true;
				treeList.ExpandAll();
				cboLoacationStore.Properties.Buttons.Clear();
				cboLoacationStore.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
				cboLoacationStore.Properties.Buttons.Add(new EditorButton(ButtonPredefines.Delete));
				cboLoacationStore.Popup -= new EventHandler(cboLoacationStore_Popup_BestFit);
				cboLoacationStore.Popup += new EventHandler(cboLoacationStore_Popup_BestFit);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLoacationStore_Popup_BestFit(object sender, EventArgs e)
		{
			TreeListLookUpEdit treeListLookUpEdit = sender as TreeListLookUpEdit;
			object obj;
			if (treeListLookUpEdit == null)
			{
				obj = null;
			}
			else
			{
				RepositoryItemTreeListLookUpEdit properties = treeListLookUpEdit.Properties;
				obj = ((properties != null) ? properties.TreeList : null);
			}
			TreeList treeList = (TreeList)obj;
			if (treeList != null)
			{
				treeList.BestFitColumns();
				TreeListColumn treeListColumn = treeList.Columns.ColumnByFieldName("LOCATION_STORE_CODE");
				TreeListColumn treeListColumn2 = treeList.Columns.ColumnByFieldName("LOCATION_STORE_NAME");
				if (treeListColumn != null)
				{
					treeListColumn.Width = Math.Min(treeListColumn.Width, 150);
				}
				if (treeListColumn2 != null)
				{
					treeListColumn2.Width = Math.Max(treeListColumn2.Width, 300);
				}
				int num = (from TreeListColumn c in treeList.Columns
					where c.Visible
					select c).Sum((TreeListColumn c) => c.Width);
				num += 70;
				int num2 = Math.Max(treeListLookUpEdit.Properties.PopupFormMinSize.Width, Math.Min(900, num));
				treeListLookUpEdit.Properties.PopupFormSize = new Size(num2, treeListLookUpEdit.Properties.PopupFormSize.Height);
			}
		}

		private void InitControlState()
		{
			try
			{
				if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
				{
					return;
				}
				foreach (ControlStateRDO item in currentControlStateRDO)
				{
					if (item.KEY == "checkFilterDataStoreByEndInfo")
					{
						checkFilterDataStoreByEndInfo.Checked = item.VALUE == "1";
					}
					else if (item.KEY == "checkFilterDataStoreByTreatmentEndType")
					{
						checkFilterDataStoreByTreatmentEndType.Checked = item.VALUE == "1";
					}
					else if (item.KEY == chkAutoSetStoreCode.Name)
					{
						chkAutoSetStoreCode.Checked = item.VALUE == "1";
					}
					else if (item.KEY == chkIsTreatmentType.Name)
					{
						chkIsTreatmentType.Checked = item.VALUE == "1";
					}
					else if (item.KEY == chkUseEndCode.Name)
					{
						chkUseEndCode.Checked = item.VALUE == "1";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetDefaultData()
		{
			try
			{
				List<HIS_PROGRAM> programADOList = (from o in BackendDataWorker.Get<HIS_PROGRAM>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				LoadDataTocboProram(programADOList);
				dtSaveTime.DateTime = DateTime.Now;
				GetDataStore();
				LoadDataTocboDataStore(dataStores);
				SetDefaultDataStore();
				LoadDataTocboMediRecordType((from o in BackendDataWorker.Get<HIS_MEDI_RECORD_TYPE>()
					where o.IS_ACTIVE == 1
					select o).ToList());
				SetDefaultMediRecordType();
				lciProgram.Visibility = ((!chkBANgoaiTru.Checked) ? LayoutVisibility.Never : LayoutVisibility.Always);
				linkLabel1.Text = "Trước đó, hồ sơ đã bị từ chối duyệt. Bạn có thể click vào đây để xem nội dung từ chối trước khi lưu trữ bệnh án";
				linkLabel1.LinkArea = new LinkArea(58, 3);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void GetDataStore()
		{
			try
			{
				string loginname = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
				List<V_HIS_USER_ROOM> list = (from o in BackendDataWorker.Get<V_HIS_USER_ROOM>()
					where o.LOGINNAME == loginname && o.IS_ACTIVE == 1
					select o).ToList();
				if (list != null && list.Count > 0)
				{
					List<long> roomIds = list.Select((V_HIS_USER_ROOM o) => o.ROOM_ID).Distinct().ToList();
					dataStores = (from o in BackendDataWorker.Get<HIS_DATA_STORE>()
						where roomIds.Contains(o.ROOM_ID) && o.IS_ACTIVE == 1
						select o).ToList();
				}
				if (checkFilterDataStoreByEndInfo.Checked && dataStores != null && dataStores.Count > 0)
				{
					List<long> departmentIds = (from s in currentTreatment
						where ((L_HIS_TREATMENT_3)s).END_DEPARTMENT_ID.HasValue
						select ((L_HIS_TREATMENT_3)s).END_DEPARTMENT_ID.Value).Distinct().ToList();
					List<long> roomIds2 = (from s in currentTreatment
						where ((L_HIS_TREATMENT_3)s).END_ROOM_ID.HasValue
						select ((L_HIS_TREATMENT_3)s).END_ROOM_ID.Value).Distinct().ToList();
					dataStores = dataStores.Where((HIS_DATA_STORE o) => (!o.STORED_DEPARTMENT_ID.HasValue || !departmentIds.Any((long a) => a != o.STORED_DEPARTMENT_ID.Value)) && (!o.STORED_ROOM_ID.HasValue || !roomIds2.Any((long a) => a != o.STORED_ROOM_ID.Value))).ToList();
				}
				if (checkFilterDataStoreByTreatmentEndType.Checked && dataStores != null && dataStores.Count > 0)
				{
					if (currentTreatment.Exists((TreatmentADO e) => !((L_HIS_TREATMENT_3)e).TREATMENT_END_TYPE_ID.HasValue))
					{
						dataStores = new List<HIS_DATA_STORE>();
					}
					else
					{
						List<long> TreatmentEndTypeIds = currentTreatment.Select((TreatmentADO s) => ((L_HIS_TREATMENT_3)s).TREATMENT_END_TYPE_ID.Value).Distinct().ToList();
						dataStores = dataStores.Where((HIS_DATA_STORE o) => string.IsNullOrWhiteSpace(o.TREATMENT_END_TYPE_IDS) || !TreatmentEndTypeIds.Any((long a) => !CheckContainsTreatmentEndType(o.TREATMENT_END_TYPE_IDS, a))).ToList();
					}
				}
				if (!chkIsTreatmentType.Checked || dataStores == null || dataStores.Count <= 0)
				{
					return;
				}
				List<string> treatmentTypeIds = (from s in currentTreatment
					where ((L_HIS_TREATMENT_3)s).TDL_TREATMENT_TYPE_ID.HasValue
					select ((L_HIS_TREATMENT_3)s).TDL_TREATMENT_TYPE_ID.Value.ToString()).Distinct().ToList();
				dataStores = dataStores.Where((HIS_DATA_STORE o) => string.IsNullOrEmpty(o.TREATMENT_TYPE_IDS) || o.TREATMENT_TYPE_IDS.Split(',').ToList().Exists((string p) => treatmentTypeIds.Contains(p))).ToList();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool CheckContainsTreatmentEndType(string treatmentEndTypeIds, long treatmentEndTypeId)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(treatmentEndTypeIds) && string.Format(",{0},", treatmentEndTypeIds).Contains(string.Format(",{0},", treatmentEndTypeId)))
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return false;
		}

		private void SetDefaultDataStore()
		{
			try
			{
				if (dataStores != null && dataStores.Count == 1)
				{
					cboDataStore.EditValue = dataStores[0].ID;
				}
				else
				{
					if (dataStores != null && cboDataStore.EditValue != null && dataStores.Any((HIS_DATA_STORE a) => a.ID == System.Convert.ToInt64(cboDataStore.EditValue)))
					{
						return;
					}
					cboDataStore.EditValue = null;
					if (GlobalVariables.ListDataStoreUseTime != null && GlobalVariables.ListDataStoreUseTime is List<LocalStoreADO> && dataStores != null)
					{
						List<LocalStoreADO> source = ((List<LocalStoreADO>)GlobalVariables.ListDataStoreUseTime).OrderByDescending((LocalStoreADO o) => o.LastTime).ToList();
						LocalStoreADO localStoreADO = source.FirstOrDefault((LocalStoreADO o) => dataStores.Any((HIS_DATA_STORE a) => a.ID == o.DataStoreId));
						if (localStoreADO != null)
						{
							cboDataStore.EditValue = localStoreADO.DataStoreId;
						}
					}
					if (cboDataStore.EditValue != null)
					{
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetDefaultMediRecordType()
		{
			try
			{
				List<HIS_MEDI_RECORD_TYPE> dataMediRecordType = cboMediRecordType.Properties.DataSource as List<HIS_MEDI_RECORD_TYPE>;
				if (dataMediRecordType != null && dataMediRecordType.Count == 1)
				{
					cboMediRecordType.EditValue = dataMediRecordType[0].ID;
				}
				else
				{
					if (dataMediRecordType != null && cboMediRecordType.EditValue != null && dataMediRecordType.Any((HIS_MEDI_RECORD_TYPE a) => a.ID == System.Convert.ToInt64(cboMediRecordType.EditValue)))
					{
						return;
					}
					cboMediRecordType.EditValue = null;
					if (GlobalVariables.ListDataStoreUseTime != null && GlobalVariables.ListDataStoreUseTime is List<LocalStoreADO> && dataMediRecordType != null)
					{
						List<LocalStoreADO> source = ((List<LocalStoreADO>)GlobalVariables.ListDataStoreUseTime).OrderByDescending((LocalStoreADO o) => o.LastTime).ToList();
						LocalStoreADO localStoreADO = source.FirstOrDefault((LocalStoreADO o) => dataMediRecordType.Any((HIS_MEDI_RECORD_TYPE a) => a.ID == o.MediRecordTypeId));
						if (localStoreADO != null)
						{
							cboMediRecordType.EditValue = localStoreADO.MediRecordTypeId;
						}
					}
					if (cboMediRecordType.EditValue != null)
					{
						return;
					}
					try
					{
						List<long> list = (from s in currentTreatment
							where ((L_HIS_TREATMENT_3)s).MEDI_RECORD_TYPE_ID.HasValue
							select ((L_HIS_TREATMENT_3)s).MEDI_RECORD_TYPE_ID.Value).Distinct().ToList();
						List<long> list2 = currentTreatment.Select((TreatmentADO s) => ((L_HIS_TREATMENT_3)s).TREATMENT_END_TYPE_ID.GetValueOrDefault()).Distinct().ToList();
						if (list != null && list.Count == 1)
						{
							cboMediRecordType.EditValue = list[0];
						}
						else if (list2 != null && list2.Count == 1 && list2[0] == 1)
						{
							cboMediRecordType.EditValue = 1L;
						}
						else
						{
							cboMediRecordType.EditValue = 4L;
						}
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Error(ex);
						return;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void ValidateForm()
		{
			try
			{
				ValidateGridLookupWithTextEdit(cboDataStore, txtDataStoreCode);
				ValidateThoiGian();
				ValidateStoreCode();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateStoreCode()
		{
			try
			{
				ControlMaxLengthValidationRule controlMaxLengthValidationRule = new ControlMaxLengthValidationRule();
				controlMaxLengthValidationRule.editor = txtStoreCode;
				controlMaxLengthValidationRule.IsRequired = false;
				controlMaxLengthValidationRule.maxLength = 20;
				controlMaxLengthValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtStoreCode, controlMaxLengthValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
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

		private void ValidateThoiGian()
		{
			try
			{
				DateTimeValidationRule dateTimeValidationRule = new DateTimeValidationRule();
				dateTimeValidationRule.dtTime = dtSaveTime;
				dateTimeValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(dtSaveTime, dateTimeValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataTocboProram(List<HIS_PROGRAM> ProgramADOList)
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("PROGRAM_CODE", "", 150, 1));
				list.Add(new ColumnInfo("PROGRAM_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("PROGRAM_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(cboProgram, ProgramADOList, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataTocboMediRecordType(List<HIS_MEDI_RECORD_TYPE> ProgramADOList)
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("MEDI_RECORD_TYPE_CODE", "", 150, 1));
				list.Add(new ColumnInfo("MEDI_RECORD_TYPE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("MEDI_RECORD_TYPE_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(cboMediRecordType, ProgramADOList, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataTocboDataStore(List<HIS_DATA_STORE> ProgramADOList)
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("DATA_STORE_CODE", "", 150, 1));
				list.Add(new ColumnInfo("DATA_STORE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("DATA_STORE_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(cboDataStore, ProgramADOList, controlEditorADO);
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
				ResourceLanguageManager.LanguageResource_frmSaveStore = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Lang", typeof(frmSaveStore).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControl1.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				linkLabel1.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.linkLabel1.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				chkAutoSetStoreCode.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.chkAutoSetStoreCode.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.bar1.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				barButtonItem_Save.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.barButtonItem_Save.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				barButtonItem__Print.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.barButtonItem__Print.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				checkFilterDataStoreByTreatmentEndType.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.checkFilterDataStoreByTreatmentEndType.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				checkFilterDataStoreByEndInfo.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.checkFilterDataStoreByEndInfo.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				cboMediRecordType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmSaveStore.cboMediRecordType.Properties.NullText", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				cboProgram.Properties.NullText = Inventec.Common.Resource.Get.Value("frmSaveStore.cboProgram.Properties.NullText", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				chkBANgoaiTru.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.chkBANgoaiTru.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				cboDataStore.Properties.NullText = Inventec.Common.Resource.Get.Value("frmSaveStore.cboDataStore.Properties.NullText", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				btnSaveData.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.btnSaveData.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem5.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem5.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				lciProgram.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.lciProgram.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem7.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem7.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				lciFilterDataStoreByEndInfo.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.lciFilterDataStoreByEndInfo.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				lciShowDataStoreByEndTreatmentType.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.lciShowDataStoreByEndTreatmentType.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				lciStoreCode.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.lciStoreCode.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem9.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem9.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				chkIsTreatmentType.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.chkIsTreatmentType.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				layoutControlItem6.Text = Inventec.Common.Resource.Get.Value("frmSaveStore.layoutControlItem6.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmSaveStore.Text", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				chkUseEndCode.Properties.Caption = Inventec.Common.Resource.Get.Value("frmSaveStore.chkUseEndCode.Properties.Caption", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
				chkUseEndCode.ToolTip = Inventec.Common.Resource.Get.Value("frmSaveStore.chkUseEndCode.ToolTip", ResourceLanguageManager.LanguageResource_frmSaveStore, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusButtonSave()
		{
			try
			{
				if (cboMediRecordType.EditValue != null && cboDataStore.EditValue != null)
				{
					base.ActiveControl = btnSaveData;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
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
				HisTreatmentStoreSDO hisTreatmentStoreSDO = new HisTreatmentStoreSDO();
				if (cboDataStore.EditValue != null)
				{
					hisTreatmentStoreSDO.DataStoreId = System.Convert.ToInt32(cboDataStore.EditValue.ToString());
				}
				if (cboMediRecordType.EditValue != null)
				{
					hisTreatmentStoreSDO.MediRecordTypeId = System.Convert.ToInt32(cboMediRecordType.EditValue.ToString());
				}
				hisTreatmentStoreSDO.IsOutPatient = chkBANgoaiTru.CheckState == CheckState.Checked;
				if (chkBANgoaiTru.CheckState == CheckState.Checked && cboProgram.EditValue != null)
				{
					hisTreatmentStoreSDO.ProgramId = System.Convert.ToInt32(cboProgram.EditValue.ToString());
				}
				if (dtSaveTime != null && dtSaveTime.DateTime != DateTime.MinValue)
				{
					hisTreatmentStoreSDO.StoreTime = Parse.ToInt64(dtSaveTime.DateTime.ToString("yyyyMMddHHmm") + "59");
				}
				if (txtStoreCode.Enabled && !string.IsNullOrEmpty(txtStoreCode.Text))
				{
					hisTreatmentStoreSDO.StoreCode = txtStoreCode.Text.Trim();
				}
				if (chkUseEndCode.CheckState == CheckState.Checked)
				{
					hisTreatmentStoreSDO.IsUseEndCode = true;
				}
				if (cboLoacationStore.EditValue != null)
				{
					hisTreatmentStoreSDO.LocationStoreId = long.Parse(cboLoacationStore.EditValue.ToString());
				}
				bool flag = false;
				string text = "";
				List<HIS_TREATMENT> list = null;
				if (currentTreatment != null && currentTreatment.Count > 0)
				{
					hisTreatmentStoreSDO.TreatmentIds = new List<long>();
					hisTreatmentStoreSDO.TreatmentIds = currentTreatment.Select((TreatmentADO o) => ((L_HIS_TREATMENT_3)o).ID).Distinct().ToList();
					list = new BackendAdapter(commonParam).Post<List<HIS_TREATMENT>>("api/HisTreatment/Store", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentStoreSDO, commonParam);
					if (list != null && list.Count > 0)
					{
						flag = true;
						foreach (HIS_TREATMENT item in list)
						{
							text = text + item.STORE_CODE + ", ";
						}
						if (list.Count == 1)
						{
							txtStoreCode.Text = list.First().STORE_CODE;
						}
						refeshData();
						SetLastTimeToLocal(hisTreatmentStoreSDO.DataStoreId, hisTreatmentStoreSDO.MediRecordTypeId);
					}
				}
				if (list != null && list.Count > 0)
				{
					MessageManager.Show(ResourceMessage.XuLyThanhCong + " " + ResourceMessage.SoluuTruCuHoSoLa + text);
				}
				else
				{
					MessageManager.Show(base.ParentForm, commonParam, flag);
				}
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

		private void checkEdit1_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				lciProgram.Visibility = ((chkBANgoaiTru.CheckState != CheckState.Checked) ? LayoutVisibility.Never : LayoutVisibility.Always);
				if (chkBANgoaiTru.CheckState == CheckState.Unchecked)
				{
					cboProgram.EditValue = null;
				}
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

		private void cboDataStore_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && cboDataStore.EditValue != null)
				{
					HIS_DATA_STORE hIS_DATA_STORE = dataStores.SingleOrDefault((HIS_DATA_STORE o) => o.ID == Parse.ToInt64((cboDataStore.EditValue ?? "").ToString()));
					if (hIS_DATA_STORE != null)
					{
						txtDataStoreCode.Text = hIS_DATA_STORE.DATA_STORE_CODE;
						cboDataStore.Properties.Buttons[1].Visible = true;
						chkBANgoaiTru.Focus();
						FocusButtonSave();
					}
					else
					{
						cboDataStore.Focus();
						cboDataStore.ShowPopup();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDataStore_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				bool flag = false;
				if (!string.IsNullOrEmpty(cboDataStore.Text))
				{
					string key = cboDataStore.Text.ToLower();
					List<HIS_DATA_STORE> list = dataStores.Where((HIS_DATA_STORE o) => o.DATA_STORE_CODE.ToLower().Contains(key) || o.DATA_STORE_NAME.ToLower().Contains(key)).ToList();
					if (list != null && list.Count == 1)
					{
						flag = true;
						cboDataStore.EditValue = list.First().ID;
						txtDataStoreCode.Text = list.First().DATA_STORE_CODE;
					}
				}
				if (!flag)
				{
					cboDataStore.Focus();
					cboDataStore.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtDataStoreCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string dataStoreCode = (sender as TextEdit).Text;
					LoadDataStore(dataStoreCode);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataStore(string _DataStoreCode)
		{
			try
			{
				List<HIS_DATA_STORE> list = new List<HIS_DATA_STORE>();
				list = dataStores.Where((HIS_DATA_STORE o) => !string.IsNullOrWhiteSpace(o.DATA_STORE_CODE) && o.DATA_STORE_CODE.StartsWith(_DataStoreCode)).ToList();
				List<ColumnInfo> list2 = new List<ColumnInfo>();
				list2.Add(new ColumnInfo("DATA_STORE_CODE", "", 100, 1));
				list2.Add(new ColumnInfo("DATA_STORE_NAME", "", 250, 2));
				if (list.Count == 1)
				{
					cboDataStore.EditValue = list[0].ID;
					txtDataStoreCode.Text = list[0].DATA_STORE_CODE;
					chkBANgoaiTru.Focus();
				}
				else if (list.Count > 1)
				{
					cboDataStore.EditValue = null;
					cboDataStore.Focus();
					cboDataStore.ShowPopup();
				}
				else
				{
					cboDataStore.EditValue = null;
					cboDataStore.Focus();
					cboDataStore.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtSaveTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtDataStoreCode.Focus();
					txtDataStoreCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkBANgoaiTru_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboProgram.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboProgram_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboProgram_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboMediRecordType.Focus();
					cboMediRecordType.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboMediRecordType_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSaveData.Focus();
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkFilterDataStoreByEndInfo_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				GetDataStore();
				LoadDataTocboDataStore(dataStores);
				SetDefaultDataStore();
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == "checkFilterDataStoreByEndInfo" && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (checkFilterDataStoreByEndInfo.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = "checkFilterDataStoreByEndInfo";
					csAddOrUpdate.VALUE = (checkFilterDataStoreByEndInfo.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetLastTimeToLocal(long dataStoreId, long? mediRecordTypeId)
		{
			try
			{
				if (dataStoreId > 0)
				{
					long value = Inventec.Common.DateTime.Get.Now().Value;
					List<LocalStoreADO> list = null;
					list = (List<LocalStoreADO>)((GlobalVariables.ListDataStoreUseTime != null && GlobalVariables.ListDataStoreUseTime is List<LocalStoreADO>) ? ((List<LocalStoreADO>)GlobalVariables.ListDataStoreUseTime) : (GlobalVariables.ListDataStoreUseTime = new List<LocalStoreADO>()));
					LocalStoreADO localStoreADO = list.FirstOrDefault((LocalStoreADO o) => o.DataStoreId == dataStoreId || o.MediRecordTypeId == mediRecordTypeId);
					if (localStoreADO != null)
					{
						localStoreADO.LastTime = value;
						return;
					}
					localStoreADO = new LocalStoreADO();
					localStoreADO.DataStoreId = dataStoreId;
					localStoreADO.MediRecordTypeId = mediRecordTypeId;
					localStoreADO.LastTime = value;
					list.Add(localStoreADO);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboDataStore_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				txtDataStoreCode.Text = "";
				cboLoacationStore.EditValue = null;
				if (cboDataStore.EditValue != null)
				{
					HIS_DATA_STORE data = dataStores.FirstOrDefault((HIS_DATA_STORE o) => o.ID == System.Convert.ToInt64(cboDataStore.EditValue));
					if (data != null)
					{
						txtDataStoreCode.Text = data.DATA_STORE_CODE;
						List<HIS_LOCATION_STORE> dataSource = (from o in lstLocationStore
							where !o.DATA_STORE_ID.HasValue || o.DATA_STORE_ID == data.ID
							orderby o.LOCATION_STORE_CODE, o.LOCATION_STORE_NAME
							select o).ToList();
						cboLoacationStore.Properties.DataSource = dataSource;
						cboLoacationStore.Properties.TreeList.ExpandAll();
						if (data.LOCATION_STORE_ID.HasValue)
						{
							cboLoacationStore.EditValue = data.LOCATION_STORE_ID.Value;
						}
					}
				}
				else
				{
					cboLoacationStore.Properties.DataSource = lstLocationStore;
					cboLoacationStore.Properties.TreeList.ExpandAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void checkFilterDataStoreByTreatmentEndType_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				GetDataStore();
				LoadDataTocboDataStore(dataStores);
				SetDefaultDataStore();
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == "checkFilterDataStoreByTreatmentEndType" && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (checkFilterDataStoreByTreatmentEndType.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = "checkFilterDataStoreByTreatmentEndType";
					csAddOrUpdate.VALUE = (checkFilterDataStoreByTreatmentEndType.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMediRecordType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					FocusButtonSave();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkAutoSetStoreCode_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkAutoSetStoreCode.Checked)
				{
					txtStoreCode.Enabled = true;
					chkUseEndCode.Checked = false;
					chkUseEndCode.Enabled = false;
				}
				else
				{
					txtStoreCode.Enabled = false;
					txtStoreCode.Text = "";
					chkUseEndCode.Enabled = true;
				}
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkAutoSetStoreCode.Name && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkAutoSetStoreCode.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkAutoSetStoreCode.Name;
					csAddOrUpdate.VALUE = (chkAutoSetStoreCode.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				frmContentRefused frmContentRefused2 = new frmContentRefused(currentModule, currentTreatment.FirstOrDefault());
				frmContentRefused2.ShowDialog();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkIsTreatmentType_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				GetDataStore();
				LoadDataTocboDataStore(dataStores);
				SetDefaultDataStore();
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkIsTreatmentType.Name && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkIsTreatmentType.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkIsTreatmentType.Name;
					csAddOrUpdate.VALUE = (chkIsTreatmentType.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkUseEndCode_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkUseEndCode.Checked)
				{
					txtStoreCode.Enabled = false;
					txtStoreCode.Text = "";
					chkAutoSetStoreCode.Checked = false;
					chkAutoSetStoreCode.Enabled = false;
				}
				else
				{
					chkAutoSetStoreCode.Enabled = true;
				}
				if (isInit)
				{
					return;
				}
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkUseEndCode.Name && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkUseEndCode.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkUseEndCode.Name;
					csAddOrUpdate.VALUE = (chkUseEndCode.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLoacationStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboLoacationStore.EditValue = null;
					txtLocationStoreCode.Text = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtLocationStoreCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (!string.IsNullOrEmpty(txtLocationStoreCode.Text.Trim()))
				{
					HIS_LOCATION_STORE val = lstLocationStore.FirstOrDefault((HIS_LOCATION_STORE o) => o.LOCATION_STORE_CODE == txtLocationStoreCode.Text.Trim());
					if (val != null)
					{
						cboLoacationStore.EditValue = val.ID;
						chkBANgoaiTru.Focus();
						return;
					}
				}
				cboLoacationStore.Focus();
				cboLoacationStore.ShowPopup();
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
				if (cboLoacationStore.EditValue != null)
				{
					HIS_LOCATION_STORE val = lstLocationStore.FirstOrDefault((HIS_LOCATION_STORE o) => o.ID == long.Parse(cboLoacationStore.EditValue.ToString()));
					if (val != null)
					{
						txtLocationStoreCode.Text = val.LOCATION_STORE_CODE;
					}
				}
				else
				{
					txtLocationStoreCode.Text = null;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.Desktop.Plugins.MedicalStoreV2.ChooseStore.frmSaveStore));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem_Save = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem__Print = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.txtLocationStoreCode = new DevExpress.XtraEditors.TextEdit();
			this.chkUseEndCode = new DevExpress.XtraEditors.CheckEdit();
			this.chkIsTreatmentType = new DevExpress.XtraEditors.CheckEdit();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.chkAutoSetStoreCode = new DevExpress.XtraEditors.CheckEdit();
			this.txtStoreCode = new DevExpress.XtraEditors.TextEdit();
			this.checkFilterDataStoreByTreatmentEndType = new DevExpress.XtraEditors.CheckEdit();
			this.checkFilterDataStoreByEndInfo = new DevExpress.XtraEditors.CheckEdit();
			this.cboMediRecordType = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit3View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.cboProgram = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.chkBANgoaiTru = new DevExpress.XtraEditors.CheckEdit();
			this.cboDataStore = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.txtDataStoreCode = new DevExpress.XtraEditors.TextEdit();
			this.dtSaveTime = new DevExpress.XtraEditors.DateEdit();
			this.btnSaveData = new DevExpress.XtraEditors.SimpleButton();
			this.cboLoacationStore = new DevExpress.XtraEditors.TreeListLookUpEdit();
			this.treeList1 = new DevExpress.XtraTreeList.TreeList();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciProgram = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.lciFilterDataStoreByEndInfo = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciShowDataStoreByEndTreatmentType = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciStoreCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtLocationStoreCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkUseEndCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkIsTreatmentType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkAutoSetStoreCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkFilterDataStoreByTreatmentEndType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkFilterDataStoreByEndInfo.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboMediRecordType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit3View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboProgram.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkBANgoaiTru.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboDataStore.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtDataStoreCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtSaveTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtSaveTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboLoacationStore.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.treeList1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciProgram).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciFilterDataStoreByEndInfo).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciShowDataStoreByEndTreatmentType).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciStoreCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.txtLocationStoreCode);
			this.layoutControl1.Controls.Add(this.chkUseEndCode);
			this.layoutControl1.Controls.Add(this.chkIsTreatmentType);
			this.layoutControl1.Controls.Add(this.linkLabel1);
			this.layoutControl1.Controls.Add(this.chkAutoSetStoreCode);
			this.layoutControl1.Controls.Add(this.txtStoreCode);
			this.layoutControl1.Controls.Add(this.checkFilterDataStoreByTreatmentEndType);
			this.layoutControl1.Controls.Add(this.checkFilterDataStoreByEndInfo);
			this.layoutControl1.Controls.Add(this.cboMediRecordType);
			this.layoutControl1.Controls.Add(this.cboProgram);
			this.layoutControl1.Controls.Add(this.chkBANgoaiTru);
			this.layoutControl1.Controls.Add(this.cboDataStore);
			this.layoutControl1.Controls.Add(this.txtDataStoreCode);
			this.layoutControl1.Controls.Add(this.dtSaveTime);
			this.layoutControl1.Controls.Add(this.btnSaveData);
			this.layoutControl1.Controls.Add(this.cboLoacationStore);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 38);
			this.layoutControl1.Margin = new System.Windows.Forms.Padding(4);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(482, 113, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(601, 337);
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
			this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4);
			this.barDockControlTop.Size = new System.Drawing.Size(601, 38);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 375);
			this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4);
			this.barDockControlBottom.Size = new System.Drawing.Size(601, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 38);
			this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 337);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(601, 38);
			this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 337);
			this.txtLocationStoreCode.Location = new System.Drawing.Point(107, 162);
			this.txtLocationStoreCode.Margin = new System.Windows.Forms.Padding(4);
			this.txtLocationStoreCode.MenuManager = this.barManager1;
			this.txtLocationStoreCode.Name = "txtLocationStoreCode";
			this.txtLocationStoreCode.Size = new System.Drawing.Size(152, 22);
			this.txtLocationStoreCode.StyleController = this.layoutControl1;
			this.txtLocationStoreCode.TabIndex = 26;
			this.txtLocationStoreCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtLocationStoreCode_PreviewKeyDown);
			this.chkUseEndCode.Location = new System.Drawing.Point(475, 3);
			this.chkUseEndCode.Margin = new System.Windows.Forms.Padding(4);
			this.chkUseEndCode.MenuManager = this.barManager1;
			this.chkUseEndCode.Name = "chkUseEndCode";
			this.chkUseEndCode.Properties.Caption = "Lấy số ra viện";
			this.chkUseEndCode.Size = new System.Drawing.Size(123, 20);
			this.chkUseEndCode.StyleController = this.layoutControl1;
			this.chkUseEndCode.TabIndex = 25;
			this.chkUseEndCode.ToolTip = "Sử dụng số ra viện làm mã lưu trữ bệnh án";
			this.chkUseEndCode.CheckedChanged += new System.EventHandler(chkUseEndCode_CheckedChanged);
			this.chkIsTreatmentType.Location = new System.Drawing.Point(108, 111);
			this.chkIsTreatmentType.Margin = new System.Windows.Forms.Padding(4);
			this.chkIsTreatmentType.MenuManager = this.barManager1;
			this.chkIsTreatmentType.Name = "chkIsTreatmentType";
			this.chkIsTreatmentType.Properties.Caption = "Hiển thị theo diện điều trị";
			this.chkIsTreatmentType.Size = new System.Drawing.Size(490, 20);
			this.chkIsTreatmentType.StyleController = this.layoutControl1;
			this.chkIsTreatmentType.TabIndex = 24;
			this.chkIsTreatmentType.CheckedChanged += new System.EventHandler(chkIsTreatmentType_CheckedChanged);
			this.linkLabel1.LinkColor = System.Drawing.Color.Red;
			this.linkLabel1.Location = new System.Drawing.Point(108, 271);
			this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(490, 30);
			this.linkLabel1.TabIndex = 23;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "Trước đó, hồ sơ đã bị từ chối duyệt. Bạn có thể click vào đây để xem nội dung từ chối trước khi lưu trữ bệnh án";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
			this.chkAutoSetStoreCode.Location = new System.Drawing.Point(384, 3);
			this.chkAutoSetStoreCode.Margin = new System.Windows.Forms.Padding(4);
			this.chkAutoSetStoreCode.MenuManager = this.barManager1;
			this.chkAutoSetStoreCode.Name = "chkAutoSetStoreCode";
			this.chkAutoSetStoreCode.Properties.Caption = "Tự nhập";
			this.chkAutoSetStoreCode.Size = new System.Drawing.Size(85, 20);
			this.chkAutoSetStoreCode.StyleController = this.layoutControl1;
			this.chkAutoSetStoreCode.TabIndex = 21;
			this.chkAutoSetStoreCode.CheckedChanged += new System.EventHandler(chkAutoSetStoreCode_CheckedChanged);
			this.txtStoreCode.Enabled = false;
			this.txtStoreCode.Location = new System.Drawing.Point(108, 3);
			this.txtStoreCode.Margin = new System.Windows.Forms.Padding(4);
			this.txtStoreCode.MenuManager = this.barManager1;
			this.txtStoreCode.Name = "txtStoreCode";
			this.txtStoreCode.Properties.MaxLength = 20;
			this.txtStoreCode.Size = new System.Drawing.Size(270, 22);
			this.txtStoreCode.StyleController = this.layoutControl1;
			this.txtStoreCode.TabIndex = 20;
			this.checkFilterDataStoreByTreatmentEndType.Location = new System.Drawing.Point(108, 85);
			this.checkFilterDataStoreByTreatmentEndType.Margin = new System.Windows.Forms.Padding(4);
			this.checkFilterDataStoreByTreatmentEndType.MenuManager = this.barManager1;
			this.checkFilterDataStoreByTreatmentEndType.Name = "checkFilterDataStoreByTreatmentEndType";
			this.checkFilterDataStoreByTreatmentEndType.Properties.Caption = "Chỉ hiển thị tủ theo loại ra viện";
			this.checkFilterDataStoreByTreatmentEndType.Size = new System.Drawing.Size(490, 20);
			this.checkFilterDataStoreByTreatmentEndType.StyleController = this.layoutControl1;
			this.checkFilterDataStoreByTreatmentEndType.TabIndex = 19;
			this.checkFilterDataStoreByTreatmentEndType.CheckedChanged += new System.EventHandler(checkFilterDataStoreByTreatmentEndType_CheckedChanged);
			this.checkFilterDataStoreByEndInfo.Location = new System.Drawing.Point(108, 59);
			this.checkFilterDataStoreByEndInfo.Margin = new System.Windows.Forms.Padding(4);
			this.checkFilterDataStoreByEndInfo.MenuManager = this.barManager1;
			this.checkFilterDataStoreByEndInfo.Name = "checkFilterDataStoreByEndInfo";
			this.checkFilterDataStoreByEndInfo.Properties.Caption = "Chỉ hiển thị tủ tương ứng với khoa/phòng kết thúc";
			this.checkFilterDataStoreByEndInfo.Size = new System.Drawing.Size(490, 20);
			this.checkFilterDataStoreByEndInfo.StyleController = this.layoutControl1;
			this.checkFilterDataStoreByEndInfo.TabIndex = 18;
			this.checkFilterDataStoreByEndInfo.CheckedChanged += new System.EventHandler(checkFilterDataStoreByEndInfo_CheckedChanged);
			this.cboMediRecordType.Location = new System.Drawing.Point(108, 243);
			this.cboMediRecordType.Margin = new System.Windows.Forms.Padding(4);
			this.cboMediRecordType.MenuManager = this.barManager1;
			this.cboMediRecordType.Name = "cboMediRecordType";
			this.cboMediRecordType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboMediRecordType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboMediRecordType.Properties.NullText = "";
			this.cboMediRecordType.Properties.View = this.gridLookUpEdit3View;
			this.cboMediRecordType.Size = new System.Drawing.Size(490, 22);
			this.cboMediRecordType.StyleController = this.layoutControl1;
			this.cboMediRecordType.TabIndex = 17;
			this.cboMediRecordType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboMediRecordType_Closed);
			this.cboMediRecordType.KeyDown += new System.Windows.Forms.KeyEventHandler(cboMediRecordType_KeyDown);
			this.gridLookUpEdit3View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit3View.Name = "gridLookUpEdit3View";
			this.gridLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit3View.OptionsView.ShowGroupPanel = false;
			this.cboProgram.Location = new System.Drawing.Point(108, 215);
			this.cboProgram.Margin = new System.Windows.Forms.Padding(4);
			this.cboProgram.MenuManager = this.barManager1;
			this.cboProgram.Name = "cboProgram";
			this.cboProgram.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboProgram.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboProgram.Properties.NullText = "";
			this.cboProgram.Properties.View = this.gridLookUpEdit2View;
			this.cboProgram.Size = new System.Drawing.Size(490, 22);
			this.cboProgram.StyleController = this.layoutControl1;
			this.cboProgram.TabIndex = 16;
			this.cboProgram.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboProgram_Closed);
			this.cboProgram.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cboProgram_PreviewKeyDown);
			this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
			this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
			this.chkBANgoaiTru.Location = new System.Drawing.Point(108, 189);
			this.chkBANgoaiTru.Margin = new System.Windows.Forms.Padding(4);
			this.chkBANgoaiTru.MenuManager = this.barManager1;
			this.chkBANgoaiTru.Name = "chkBANgoaiTru";
			this.chkBANgoaiTru.Properties.Caption = "";
			this.chkBANgoaiTru.Properties.FullFocusRect = true;
			this.chkBANgoaiTru.Size = new System.Drawing.Size(72, 19);
			this.chkBANgoaiTru.StyleController = this.layoutControl1;
			this.chkBANgoaiTru.TabIndex = 15;
			this.chkBANgoaiTru.EditValueChanged += new System.EventHandler(checkEdit1_EditValueChanged);
			this.chkBANgoaiTru.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkBANgoaiTru_PreviewKeyDown);
			this.cboDataStore.Location = new System.Drawing.Point(259, 136);
			this.cboDataStore.Margin = new System.Windows.Forms.Padding(4);
			this.cboDataStore.MenuManager = this.barManager1;
			this.cboDataStore.Name = "cboDataStore";
			this.cboDataStore.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboDataStore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboDataStore.Properties.NullText = "";
			this.cboDataStore.Properties.View = this.gridLookUpEdit1View;
			this.cboDataStore.Size = new System.Drawing.Size(340, 22);
			this.cboDataStore.StyleController = this.layoutControl1;
			this.cboDataStore.TabIndex = 14;
			this.cboDataStore.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboDataStore_Closed);
			this.cboDataStore.EditValueChanged += new System.EventHandler(cboDataStore_EditValueChanged);
			this.cboDataStore.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cboDataStore_PreviewKeyDown);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.txtDataStoreCode.Location = new System.Drawing.Point(107, 136);
			this.txtDataStoreCode.Margin = new System.Windows.Forms.Padding(4);
			this.txtDataStoreCode.MenuManager = this.barManager1;
			this.txtDataStoreCode.Name = "txtDataStoreCode";
			this.txtDataStoreCode.Size = new System.Drawing.Size(152, 22);
			this.txtDataStoreCode.StyleController = this.layoutControl1;
			this.txtDataStoreCode.TabIndex = 13;
			this.txtDataStoreCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtDataStoreCode_PreviewKeyDown);
			this.dtSaveTime.EditValue = null;
			this.dtSaveTime.Location = new System.Drawing.Point(108, 31);
			this.dtSaveTime.Margin = new System.Windows.Forms.Padding(4);
			this.dtSaveTime.MenuManager = this.barManager1;
			this.dtSaveTime.Name = "dtSaveTime";
			this.dtSaveTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtSaveTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtSaveTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.dtSaveTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtSaveTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.dtSaveTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtSaveTime.Size = new System.Drawing.Size(220, 22);
			this.dtSaveTime.StyleController = this.layoutControl1;
			this.dtSaveTime.TabIndex = 12;
			this.dtSaveTime.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(dtSaveTime_PreviewKeyDown);
			this.btnSaveData.Location = new System.Drawing.Point(454, 307);
			this.btnSaveData.Margin = new System.Windows.Forms.Padding(4);
			this.btnSaveData.Name = "btnSaveData";
			this.btnSaveData.Size = new System.Drawing.Size(144, 27);
			this.btnSaveData.StyleController = this.layoutControl1;
			this.btnSaveData.TabIndex = 11;
			this.btnSaveData.Text = "Lưu (Ctrl S)";
			this.btnSaveData.Click += new System.EventHandler(btnSaveData_Click);
			this.cboLoacationStore.Location = new System.Drawing.Point(259, 162);
			this.cboLoacationStore.Margin = new System.Windows.Forms.Padding(4);
			this.cboLoacationStore.MenuManager = this.barManager1;
			this.cboLoacationStore.Name = "cboLoacationStore";
			this.cboLoacationStore.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboLoacationStore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
			});
			this.cboLoacationStore.Properties.NullText = "";
			this.cboLoacationStore.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboLoacationStore.Properties.TreeList = this.treeList1;
			this.cboLoacationStore.Size = new System.Drawing.Size(340, 22);
			this.cboLoacationStore.StyleController = this.layoutControl1;
			this.cboLoacationStore.TabIndex = 27;
			this.cboLoacationStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboLoacationStore_ButtonClick);
			this.cboLoacationStore.EditValueChanged += new System.EventHandler(cboLoacationStore_EditValueChanged);
			this.treeList1.Location = new System.Drawing.Point(0, 0);
			this.treeList1.Name = "treeList1";
			this.treeList1.OptionsBehavior.EnableFiltering = true;
			this.treeList1.OptionsView.ShowIndentAsRowStyle = true;
			this.treeList1.Size = new System.Drawing.Size(400, 200);
			this.treeList1.TabIndex = 0;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[19]
			{
				this.layoutControlItem2, this.layoutControlItem1, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.lciProgram, this.layoutControlItem7, this.emptySpaceItem2, this.emptySpaceItem3, this.lciFilterDataStoreByEndInfo,
				this.lciShowDataStoreByEndTreatmentType, this.lciStoreCode, this.layoutControlItem8, this.layoutControlItem6, this.layoutControlItem9, this.emptySpaceItem1, this.layoutControlItem10, this.layoutControlItem11, this.layoutControlItem12
			});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(601, 337);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem2.Control = this.btnSaveData;
			this.layoutControlItem2.Location = new System.Drawing.Point(451, 304);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(150, 33);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.dtSaveTime;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 28);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(331, 28);
			this.layoutControlItem1.Text = "Thời gian:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem3.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem3.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem3.Control = this.txtDataStoreCode;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 134);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem3.Size = new System.Drawing.Size(259, 26);
			this.layoutControlItem3.Text = "Tủ bệnh án:";
			this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem3.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem3.TextToControlDistance = 5;
			this.layoutControlItem4.Control = this.cboDataStore;
			this.layoutControlItem4.Location = new System.Drawing.Point(259, 134);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem4.Size = new System.Drawing.Size(342, 26);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem5.Control = this.chkBANgoaiTru;
			this.layoutControlItem5.Location = new System.Drawing.Point(0, 186);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.OptionsToolTip.ToolTip = "Bệnh án ngoại trú";
			this.layoutControlItem5.Size = new System.Drawing.Size(183, 26);
			this.layoutControlItem5.Text = "BA ngoại trú:";
			this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem5.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem5.TextToControlDistance = 5;
			this.lciProgram.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciProgram.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciProgram.Control = this.cboProgram;
			this.lciProgram.Location = new System.Drawing.Point(0, 212);
			this.lciProgram.Name = "lciProgram";
			this.lciProgram.Size = new System.Drawing.Size(601, 28);
			this.lciProgram.Text = "Chương trình:";
			this.lciProgram.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciProgram.TextSize = new System.Drawing.Size(100, 20);
			this.lciProgram.TextToControlDistance = 5;
			this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem7.Control = this.cboMediRecordType;
			this.layoutControlItem7.Location = new System.Drawing.Point(0, 240);
			this.layoutControlItem7.Name = "layoutControlItem7";
			this.layoutControlItem7.Size = new System.Drawing.Size(601, 28);
			this.layoutControlItem7.Text = "Loại bệnh án:";
			this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem7.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem7.TextToControlDistance = 5;
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(331, 28);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(270, 28);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			this.emptySpaceItem3.AllowHotTrack = false;
			this.emptySpaceItem3.Location = new System.Drawing.Point(183, 186);
			this.emptySpaceItem3.Name = "emptySpaceItem3";
			this.emptySpaceItem3.Size = new System.Drawing.Size(418, 26);
			this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
			this.lciFilterDataStoreByEndInfo.Control = this.checkFilterDataStoreByEndInfo;
			this.lciFilterDataStoreByEndInfo.Location = new System.Drawing.Point(0, 56);
			this.lciFilterDataStoreByEndInfo.Name = "lciFilterDataStoreByEndInfo";
			this.lciFilterDataStoreByEndInfo.Size = new System.Drawing.Size(601, 26);
			this.lciFilterDataStoreByEndInfo.Text = " ";
			this.lciFilterDataStoreByEndInfo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciFilterDataStoreByEndInfo.TextSize = new System.Drawing.Size(100, 13);
			this.lciFilterDataStoreByEndInfo.TextToControlDistance = 5;
			this.lciShowDataStoreByEndTreatmentType.Control = this.checkFilterDataStoreByTreatmentEndType;
			this.lciShowDataStoreByEndTreatmentType.Location = new System.Drawing.Point(0, 82);
			this.lciShowDataStoreByEndTreatmentType.Name = "lciShowDataStoreByEndTreatmentType";
			this.lciShowDataStoreByEndTreatmentType.Size = new System.Drawing.Size(601, 26);
			this.lciShowDataStoreByEndTreatmentType.Text = " ";
			this.lciShowDataStoreByEndTreatmentType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciShowDataStoreByEndTreatmentType.TextSize = new System.Drawing.Size(100, 20);
			this.lciShowDataStoreByEndTreatmentType.TextToControlDistance = 5;
			this.lciStoreCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciStoreCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciStoreCode.Control = this.txtStoreCode;
			this.lciStoreCode.Location = new System.Drawing.Point(0, 0);
			this.lciStoreCode.Name = "lciStoreCode";
			this.lciStoreCode.Size = new System.Drawing.Size(381, 28);
			this.lciStoreCode.Text = "Mã lưu trữ:";
			this.lciStoreCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciStoreCode.TextSize = new System.Drawing.Size(100, 13);
			this.lciStoreCode.TextToControlDistance = 5;
			this.layoutControlItem8.Control = this.chkAutoSetStoreCode;
			this.layoutControlItem8.Location = new System.Drawing.Point(381, 0);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(91, 28);
			this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem8.TextVisible = false;
			this.layoutControlItem6.Control = this.chkIsTreatmentType;
			this.layoutControlItem6.Location = new System.Drawing.Point(0, 108);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(601, 26);
			this.layoutControlItem6.Text = " ";
			this.layoutControlItem6.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem6.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem6.TextToControlDistance = 5;
			this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem9.AppearanceItemCaption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.layoutControlItem9.Control = this.linkLabel1;
			this.layoutControlItem9.Location = new System.Drawing.Point(0, 268);
			this.layoutControlItem9.MaxSize = new System.Drawing.Size(0, 40);
			this.layoutControlItem9.MinSize = new System.Drawing.Size(129, 35);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.Size = new System.Drawing.Size(601, 36);
			this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem9.Text = " ";
			this.layoutControlItem9.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem9.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem9.TextToControlDistance = 5;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 304);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(451, 33);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem10.Control = this.chkUseEndCode;
			this.layoutControlItem10.Location = new System.Drawing.Point(472, 0);
			this.layoutControlItem10.Name = "layoutControlItem10";
			this.layoutControlItem10.Size = new System.Drawing.Size(129, 28);
			this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem10.TextVisible = false;
			this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem11.Control = this.txtLocationStoreCode;
			this.layoutControlItem11.Location = new System.Drawing.Point(0, 160);
			this.layoutControlItem11.Name = "layoutControlItem11";
			this.layoutControlItem11.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem11.Size = new System.Drawing.Size(259, 26);
			this.layoutControlItem11.Text = "Vị trí:";
			this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem11.TextSize = new System.Drawing.Size(100, 20);
			this.layoutControlItem11.TextToControlDistance = 5;
			this.layoutControlItem12.Control = this.cboLoacationStore;
			this.layoutControlItem12.Location = new System.Drawing.Point(259, 160);
			this.layoutControlItem12.Name = "layoutControlItem12";
			this.layoutControlItem12.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem12.Size = new System.Drawing.Size(342, 26);
			this.layoutControlItem12.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem12.TextVisible = false;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(601, 375);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "frmSaveStore";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lưu kho";
			base.Load += new System.EventHandler(frmDataStore_Load);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtLocationStoreCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkUseEndCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkIsTreatmentType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkAutoSetStoreCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtStoreCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkFilterDataStoreByTreatmentEndType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkFilterDataStoreByEndInfo.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboMediRecordType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit3View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboProgram.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkBANgoaiTru.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboDataStore.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtDataStoreCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtSaveTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtSaveTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboLoacationStore.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.treeList1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciProgram).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciFilterDataStoreByEndInfo).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciShowDataStoreByEndTreatmentType).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciStoreCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
