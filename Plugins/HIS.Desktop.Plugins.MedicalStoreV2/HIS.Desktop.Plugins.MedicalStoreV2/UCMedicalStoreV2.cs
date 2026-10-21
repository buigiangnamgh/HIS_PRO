using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using DevExpress.Data;
using DevExpress.Images;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.Nodes.Operations;
using DevExpress.XtraTreeList.ViewInfo;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.Library.EmrGenerate;
using HIS.Desktop.Plugins.MedicalStoreV2.ADO;
using HIS.Desktop.Plugins.MedicalStoreV2.Borrow;
using HIS.Desktop.Plugins.MedicalStoreV2.ChooseStore;
using HIS.Desktop.Plugins.MedicalStoreV2.Config;
using HIS.Desktop.Plugins.MedicalStoreV2.Popup;
using HIS.Desktop.Plugins.MedicalStoreV2.Resources;
using HIS.Desktop.Utilities.Extensions;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Resource;
using Inventec.Common.RichEditor;
using Inventec.Common.RichEditor.Base;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.CustomControl;
using Inventec.Desktop.CustomControl.CustomGrid;
using Inventec.UC.Paging;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
using MPS;
using MPS.Processor.Mps000094;
using MPS.ProcessorBase;
using MPS.ProcessorBase.Core;

namespace HIS.Desktop.Plugins.MedicalStoreV2
{
	public class UCMedicalStoreV2 : UserControlBase
	{
		private class SelectNodeOperation : TreeListOperation
		{
			public override void Execute(TreeListNode node)
			{
				node.Selected = true;
			}
		}

		internal enum PrintTypeMediRecord
		{
			IN_BARCODE_MEDI_RECORD_CODE
		}

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private Module currentModule = null;

		private int startPageTreatment = 0;

		private int startPageMediRecord = 0;

		private int rowCountTreatment = 0;

		private int rowCountMediRecord = 0;

		private int dataTotalTreatment = 0;

		private int dataTotalMediRecord = 0;

		private List<HIS_TREATMENT_TYPE> _TreatmentTypeSelecteds;

		private List<HIS_DEPARTMENT> _EndDepartmentSelects;

		private List<HIS_DEPARTMENT> _ExecuteDepartmentSelects;

		private List<HIS_PROGRAM> _ProgramSelects;

		private double? blueConfig;

		private double? orangeConfig;

		private string IsRequiredLatchApproveStore = "0";

		private List<HIS_PATIENT_TYPE> patientTypeSelecteds;

		internal RepositoryItemCheckEdit checkEdit;

		private PopupMenuProcessor popupMenuProcessor = null;

		private HIS_TREATMENT currentTreatment;

		private bool isInit = true;

		private List<HIS_LOCATION_STORE> lstLocationStore;

		private List<ACS_CONTROL> controlAcs;

		private string storeCheckStatusState;

		private bool isEnforcingStoreCheckColumn = false;

		private bool isProcessingStoreCheck = false;

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private GridControl gridControlMediRecord;

		private GridView gridViewMediRecord;

		private TreeList treeListMedicalStore;

		private TreeListColumn treeListColumn1;

		private RepositoryItemCheckEdit repositoryItemCheckEdit_CheckStore;

		private TreeListColumn treeListColumn2;

		private SimpleButton btnSaveTreatment;

		private GridColumn gridColumn1;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_Print_Enable;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_Delete_Enable;

		private GridColumn gridColumn3;

		private GridColumn gridColumn4;

		private GridColumn gridColumn5;

		private GridColumn gridColumn6;

		private GridColumn gridColumn8;

		private GridColumn gridColumn9;

		private GridColumn gridColumn10;

		private GridColumn gridColumn11;

		private GridColumn gridColumn12;

		private GridColumn gridColumn13;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_Delete_Disable;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_Print_Disable;

		private GridLookUpEdit gridLookUpEdit1;

		private SimpleButton btnSearchTreatment;

		private GridColumn gridColumn14;

		private RepositoryItemCheckEdit CheckEnable;

		private RepositoryItemCheckEdit CheckDisable;

		private GridColumn gridColumn15;

		private GridLookUpEdit cboStatusEnd;

		private GridView gridLookUpEdit2View;

		private TextEdit txtTreatmentCodeSearch;

		private TextEdit txtKeywordTreatment;

		private GridColumn gridColumn24;

		private GridLookUpEdit cboTreatmentType;

		private GridView gridLookUpEdit3View;

		private GridLookUpEdit cboEndDepartment;

		private GridView gridView1;

		private TextEdit txtStoreCodeMediRecord;

		private GridColumn gridColumn16;

		private RepositoryItemButtonEdit Btn_TreatmentBorrow_Enable;

		private RepositoryItemButtonEdit Btn_TreatmentBorrow_Disable;

		private DateEdit dtOutTimeTo;

		private DateEdit dtOutTimeFrom;

		private DateEdit dtInTimeTo;

		private DateEdit dtInTimeFrom;

		private ImageCollection imageCollectionCheck;

		private GridColumn gridColumn26;

		internal GridLookUpEdit cboPatientType;

		private GridView gridView2;

		private GridColumn gridColumn27;

		private TextEdit txtKeywordMediRecord;

		private GridColumn gridColumn28;

		private BarManager barManager1;

		private Bar bar1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private DateEdit dtStoreTime_From;

		private DateEdit dtStoreTime_To;

		private GridControl gridControlTreatment;

		private GridView gridViewTreatment;

		private GridColumn gridColumn57;

		private RepositoryItemCheckEdit repositoryItemCheckEdit3;

		private GridColumn gridColumn58;

		private GridColumn gridColumn59;

		private RepositoryItemButtonEdit repositoryItemButtonEdit7;

		private GridColumn gridColumn60;

		private RepositoryItemButtonEdit repositoryItemButtonEdit8;

		private GridColumn gridColumn61;

		private GridColumn gridColumn63;

		private GridColumn gridColumn64;

		private GridColumn gridColumn65;

		private GridColumn gridColumn68;

		private GridColumn gridColumn70;

		private GridColumn gridColumn71;

		private GridColumn gridColumn72;

		private GridColumn gridColumn73;

		private GridColumn gridColumn75;

		private RepositoryItemButtonEdit repositoryItemButtonEdit9;

		private RepositoryItemButtonEdit repositoryItemButtonEdit10;

		private RepositoryItemCheckEdit repositoryItemCheckEdit4;

		private RepositoryItemButtonEdit repositoryItemButtonEdit11;

		private RepositoryItemButtonEdit repositoryItemButtonEdit12;

		private GridControl gridControlTreatmentOfMediRecord;

		private GridView gridViewTreatmentOfMediRecord;

		private GridColumn gridColumn29;

		private RepositoryItemCheckEdit repositoryItemCheckEdit1;

		private GridColumn gridColumn30;

		private GridColumn gridColumn31;

		private RepositoryItemButtonEdit ButtonEditDeleteDisable;

		private GridColumn gridColumn32;

		private RepositoryItemButtonEdit ButtonEditDeleteEnable;

		private GridColumn gridColumn35;

		private GridColumn gridColumn36;

		private GridColumn gridColumn37;

		private GridColumn gridColumn40;

		private GridColumn gridColumn41;

		private GridColumn gridColumn42;

		private GridColumn gridColumn43;

		private GridColumn gridColumn44;

		private GridColumn gridColumn45;

		private GridColumn gridColumn46;

		private GridColumn gridColumn47;

		private GridColumn gridColumn48;

		private GridColumn gridColumn49;

		private LabelControl lblDanhSachHoSoChuaLuuTru;

		private LabelControl lblDanhSachBenhAn;

		private SimpleButton btnSearchMediRecord;

		private TextEdit txtTreatmentCodeMediRecord;

		internal GridLookUpEdit cboProgram;

		private GridView gridView5;

		private GridColumn gridColumn2;

		private RepositoryItemButtonEdit ButtonEditReceiveTreatment_Enabed;

		private RepositoryItemButtonEdit ButtonEditReceiveTreatment_Disabed;

		private GridColumn grdColView;

		private RepositoryItemButtonEdit btnViewTreatment;

		private GridColumn gridColumn7;

		private RepositoryItemButtonEdit ButtonEditImport;

		private RepositoryItemButtonEdit ButtonEditUnImport;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_ViewEmrTreatment;

		private GridColumn gridColumn17;

		private TextEdit txtPatientCodeSearch;

		private CheckEdit checkAutoShowStore;

		private GridColumn gridColumn18;

		private GridColumn gridColumn19;

		private GridColumn gridColumn23;

		private GridColumn gridColumn20;

		private GridColumn gridColumn21;

		private GridColumn gridColumn22;

		private LayoutControlItem layoutControlItem28;

		private LayoutControlItem lciProgramFilter;

		private LayoutControlItem lciStoreCodeMediRecord;

		private LayoutControlItem layoutControlItem17;

		private LayoutControlItem lciKeywordMediRecord;

		private LayoutControlItem lciStoreTimeFilter;

		private LayoutControlItem lciTreatmentCodeMediRecord;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem23;

		private LayoutControlItem layoutControlItem16;

		private LayoutControlItem layoutControlItem21;

		private LayoutControlGroup lciAreaNotStore;

		private LayoutControlItem lciStatusEnd;

		private LayoutControlItem lciPatientType;

		private LayoutControlItem lciOutTimeFilter;

		private LayoutControlItem lciTreatmentCodeSearch;

		private LayoutControlItem lciKeywordTreatment;

		private LayoutControlItem lciPatientCodeSearch;

		private LayoutControlItem lciInTimeFilter;

		private LayoutControlItem layoutControlItem9;

		private LayoutControlItem layoutControlItem11;

		private LayoutControlItem lciTreatmentType;

		private LayoutControlItem lciEndDepartment;

		private LayoutControlItem layoutControlItem18;

		private LayoutControlItem layoutControlItem19;

		private LayoutControlItem layoutControlItem20;

		private LayoutControlItem layoutControlItem26;

		private LayoutControlItem layoutControlItem25;

		private LayoutControlGroup lciAreaStored;

		private LayoutControlGroup lciDataStoreList;

		private GridColumn gridColumn25;

		private GridLookUpEdit cboMediRecordType;

		private GridView gridView3;

		private LayoutControlItem lciFilterMediRecordType;

		private TextEdit txtPatientCodeMediRecord;

		private LayoutControlItem lciPatientCodeMedRecord;

		private CheckEdit rdoStatus__Stored;

		private CheckEdit rdoStatus__UnStored;

		private CheckEdit rdoStatus__All;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private SimpleButton btnImportStore;

		private LayoutControlItem layoutControlItem6;

		private EmptySpaceItem emptySpaceItem3;

		private EmptySpaceItem emptySpaceItem2;

		private GridColumn gridColumnCheckStore;

		private RepositoryItemCheckEdit repositoryItemCheckEdit_Store__Enable;

		private RepositoryItemCheckEdit repositoryItemCheckEdit_Store__Disable;

		private UcPaging ucPagingMediRecord;

		private UcPaging ucPagingTreatment;

		private LayoutControlItem layoutControlItem7;

		private LayoutControlItem layoutControlItem5;

		private GridColumn gridColumn34;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_UnRejectStore;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_RejectStore;

		private GridColumn gridColumn38;

		private RepositoryItemButtonEdit repositoryItemButtonView_Disable;

		private CheckEdit checkJustShowTreatmentLatch;

		private LayoutControlItem lciJustShowTreatmentLatch;

		private GridLookUpEdit cboExecuteDepartment;

		private GridView gridView4;

		private LayoutControlItem layoutControlItem8;

		private LayoutControlItem lciStoreCheckStatus;

		private GridLookUpEdit cboStoreCheckStatus;

		private GridView gridLookUpEditStoreCheckView;

		private GridColumn gridColStoreCheck;

		private GridColumn gridColStoreCheckUser;

		private GridColumn gridColStoreCheckTime;

		private RepositoryItemCheckEdit repositoryItemCheckEdit_StoreCheck__Enable;

		private RepositoryItemCheckEdit repositoryItemCheckEdit_StoreCheck__Disable;

		private GridColumn gridColumn39;

		private ImageCollection imageCollection1;

		private GridColumn gridColumn50;

		private RepositoryItemButtonEdit repositoryItemButtonEditInventory;

		private GridColumn gridColumn51;

		private SimpleButton btnExportExcel;

		private LayoutControlItem layoutControlItem10;

		private SaveFileDialog saveFileDialog1;

		private LayoutControlItem layoutControlItem12;

		private GridColumn gridColumn52;

		private Inventec.Desktop.CustomControl.RepositoryItemCustomGridLookUpEdit repLocationStore;

		private CustomGridViewWithFilterMultiColumn repositoryItemCustomGridLookUpEdit1View;

		private Inventec.Desktop.CustomControl.CustomGrid.RepositoryItemCustomGridLookUpEdit repLocationStoreDis;

		private Inventec.Desktop.CustomControl.CustomGrid.CustomGridView customGridView1;

		private CustomGridLookUpEditWithFilterMultiColumn cboLocationStore;

		private CustomGridViewWithFilterMultiColumn customGridLookUpEditWithFilterMultiColumn1View;

		private GridColumn gridColumn53;

		private GridColumn gridColumn55;

		private GridColumn gridColumn54;

		private GridColumn gridColumn56;

		private GridColumn gridColumn62;

		private GridColumn gridColumn66;

		private GridColumn gridColumnMediInTime;

		private GridColumn gridColumnMediOutTime;

		private GridColumn gridColumn67;

		private RepositoryItemButtonEdit repositoryItemButtonEdit_Edit;
        private ImageCollection imageCollectionRoom1;

		private MediRecordADO currentMediRecord = null;

		internal HisTreatmentLView3Filter _FilterLoadTree { get; set; }

		public void BtnRefreshs()
		{
			try
			{
				LoadDataToGridControlTreatment();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Treatment_MouseRightClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (e.Item is BarButtonItem)
				{
					BarButtonItem barButtonItem = sender as BarButtonItem;
					PopupMenuProcessor.ItemType itemType = (PopupMenuProcessor.ItemType)e.Item.Tag;
					PopupMenuProcessor.ItemType itemType2 = itemType;
					PopupMenuProcessor.ItemType itemType3 = itemType2;
					if (itemType3 == PopupMenuProcessor.ItemType.CheckingTreatmentEmr)
					{
						CheckingTreatmentEmr();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewMediRecord_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			GridHitInfo hitInfo = e.HitInfo;
			if (hitInfo.InRowCell)
			{
				int visibleRowHandle = gridViewMediRecord.GetVisibleRowHandle(hitInfo.RowHandle);
				MediRecordADO currentMediRecor = (MediRecordADO)gridViewMediRecord.GetRow(visibleRowHandle);
				gridViewMediRecord.OptionsSelection.EnableAppearanceFocusedCell = true;
				gridViewMediRecord.OptionsSelection.EnableAppearanceFocusedRow = true;
				if (barManager1 == null)
				{
					barManager1 = new BarManager();
					barManager1.Form = this;
				}
				PopupMenuProcessor popupMenuProcessor = new PopupMenuProcessor(currentMediRecor, barManager1, new MediRecordMouseRightClick(MediRecord_MouseRightClick), new RefeshReference(BtnRefreshs));
				popupMenuProcessor.InitMenu();
			}
		}

		private void MediRecord_MouseRightClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (e.Item is BarButtonItem)
				{
					BarButtonItem barButtonItem = sender as BarButtonItem;
					if ((PopupMenuProcessor.ItemType)e.Item.Tag == PopupMenuProcessor.ItemType.TrackingInMediRecord)
					{
						btnTrackingInMediRecord();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnTrackingInMediRecord()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			try
			{
				V_HIS_MEDI_RECORD MediRecord = new V_HIS_MEDI_RECORD();
				DataObjectMapper.Map<V_HIS_MEDI_RECORD>(MediRecord, gridViewMediRecord.GetFocusedRow());
				LogSystem.Info("MediRecord: " + LogUtil.TraceData(LogUtil.GetMemberName(() => MediRecord), MediRecord));
				List<object> list = new List<object>();
				list.Add(MediRecord);
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.TrackingInMediRecord", currentModule.RoomId, currentModule.RoomTypeId, list);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}

		private void CheckingTreatmentEmr()
		{
			try
			{
				currentTreatment = new HIS_TREATMENT();
				DataObjectMapper.Map<HIS_TREATMENT>(currentTreatment, gridViewTreatment.GetFocusedRow());
				if (currentTreatment != null)
				{
					List<object> list = new List<object>();
					list.Add(currentTreatment.ID);
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.HisTreatmentRecordChecking", currentModule.RoomId, currentModule.RoomTypeId, list);
					BtnRefreshs();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public UCMedicalStoreV2()
		{
			InitializeComponent();
		}

		public UCMedicalStoreV2(Module module)
			: base(module)
		{
			InitializeComponent();
			try
			{
				currentModule = module;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UCMedicalStore_Load(object sender, EventArgs e)
		{
			try
			{
				HisConfigCFG.LoadConfig();
				checkEdit = (RepositoryItemCheckEdit)treeListMedicalStore.RepositoryItems.Add("CheckEdit");
                gridColumnCheckStore.Image = imageCollectionRoom1.Images[0];
				dtOutTimeFrom.DateTime = DateTime.Now;
				dtOutTimeTo.DateTime = DateTime.Now;
				rdoStatus__All.Checked = true;
				SetCaptionByLanguageKey();
				LoadConfig();
				InitControlState();
				LoadComboStatus();
				LoadComboStoreCheckStatus();
				LoadComboMediRecordType();
				InitializePermissions();
				InitializeGrid();
				LoaddataToTreeList();
				LoadDataToGridControlTreatment();
				LoadDataToGridControlMediRecord();
				InitGridLookUpEdit_MultiSelect(cboTreatmentType, new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__TreatmentType));
				InitGridLookUpEdit_MultiSelect(cboEndDepartment, new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__EndDepartment));
				InitGridLookUpEdit_MultiSelect(cboExecuteDepartment, new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__ExecuteDepartment));
				InitGridLookUpEdit_MultiSelect(cboPatientType, new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__PatientType));
				InitGridLookUpEdit_MultiSelect(cboProgram, new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(SelectionGrid__Program));
				LoadComboEndDepartment();
				LoadComboExecuteDepartment();
				LoadComboTreatmentType();
				InitCombo(cboPatientType, BackendDataWorker.Get<HIS_PATIENT_TYPE>(), "PATIENT_TYPE_NAME", "ID");
				InitCombo(cboProgram, BackendDataWorker.Get<HIS_PROGRAM>(), "PROGRAM_NAME", "ID");
				LoadComboLocationStore();
				LoadComboLocationRepGrid();
				isInit = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboLocationRepGrid()
		{
			try
			{
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_CODE", "Mã", 100, 1));
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_NAME", "Tên", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("LOCATION_STORE_NAME", "ID", list, true, 350);
				ControlEditorLoader.Load(repLocationStore, lstLocationStore, controlEditorADO);
				repLocationStore.ImmediatePopup = true;
				repLocationStore.PopupFormSize = new Size(350, repLocationStore.PopupFormSize.Height);
				ControlEditorLoader.Load(repLocationStoreDis, lstLocationStore, controlEditorADO);
				repLocationStoreDis.ImmediatePopup = true;
				repLocationStoreDis.PopupFormSize = new Size(350, repLocationStoreDis.PopupFormSize.Height);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadConfig()
		{
			try
			{
				string text = HisConfigs.Get<string>("HIS.Desktop.Plugins.MedicalStore.BLUE_WARNING_STORE_TIME");
				string text2 = HisConfigs.Get<string>("HIS.Desktop.Plugins.MedicalStore.ORANGE_WARNING_STORE_TIME");
				IsRequiredLatchApproveStore = HisConfigs.Get<string>("HIS.Desktop.Plugins.MedicalStoreV2.IsRequiredLatchApproveStore");
				if (!string.IsNullOrEmpty(text))
				{
					blueConfig = Parse.ToDouble(text);
				}
				if (!string.IsNullOrEmpty(text2))
				{
					orangeConfig = Parse.ToDouble(text2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectionGrid__PatientType(object sender, EventArgs e)
		{
			try
			{
				patientTypeSelecteds = new List<HIS_PATIENT_TYPE>();
				foreach (HIS_PATIENT_TYPE item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
				{
					if (item != null)
					{
						patientTypeSelecteds.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitGridLookUpEdit_MultiSelect(GridLookUpEdit cbo, HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler eventSelect)
		{
			try
			{
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection(cbo.Properties);
				gridCheckMarksSelection.SelectionChanged += new HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection.SelectionChangedEventHandler(eventSelect.Invoke);
				cbo.Properties.Tag = gridCheckMarksSelection;
				cbo.Properties.View.OptionsSelection.MultiSelect = true;
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection2 = cbo.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
				if (gridCheckMarksSelection2 != null)
				{
					gridCheckMarksSelection2.ClearSelection(cbo.Properties.View);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectionGrid__EndDepartment(object sender, EventArgs e)
		{
			try
			{
				_EndDepartmentSelects = new List<HIS_DEPARTMENT>();
				foreach (HIS_DEPARTMENT item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
				{
					if (item != null)
					{
						_EndDepartmentSelects.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectionGrid__ExecuteDepartment(object sender, EventArgs e)
		{
			try
			{
				_ExecuteDepartmentSelects = new List<HIS_DEPARTMENT>();
				foreach (HIS_DEPARTMENT item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
				{
					if (item != null)
					{
						_ExecuteDepartmentSelects.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectionGrid__TreatmentType(object sender, EventArgs e)
		{
			try
			{
				_TreatmentTypeSelecteds = new List<HIS_TREATMENT_TYPE>();
				foreach (HIS_TREATMENT_TYPE item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
				{
					if (item != null)
					{
						_TreatmentTypeSelecteds.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SelectionGrid__Program(object sender, EventArgs e)
		{
			try
			{
				_ProgramSelects = new List<HIS_PROGRAM>();
				foreach (HIS_PROGRAM item in (sender as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection).Selection)
				{
					if (item != null)
					{
						_ProgramSelects.Add(item);
					}
				}
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
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.MedicalStoreV2.Resources.Lang", typeof(UCMedicalStoreV2).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.layoutControl1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnExportExcel.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnExportExcel.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboExecuteDepartment.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboExecuteDepartment.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.bar1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				checkJustShowTreatmentLatch.Properties.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.checkJustShowTreatmentLatch.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				checkJustShowTreatmentLatch.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.checkJustShowTreatmentLatch.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnImportStore.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnImportStore.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				rdoStatus__Stored.Properties.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.rdoStatus__Stored.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				rdoStatus__UnStored.Properties.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.rdoStatus__UnStored.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				rdoStatus__All.Properties.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.rdoStatus__All.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtPatientCodeMediRecord.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtPatientCodeMediRecord.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboMediRecordType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboMediRecordType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lblDanhSachHoSoChuaLuuTru.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lblDanhSachHoSoChuaLuuTru.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lblDanhSachBenhAn.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lblDanhSachBenhAn.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				treeListColumn1.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.treeListColumn1.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				treeListColumn1.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.treeListColumn1.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				treeListColumn2.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.treeListColumn2.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				treeListColumn2.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.treeListColumn2.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn29.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn29.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn30.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn30.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn31.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn31.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn32.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn32.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn35.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn35.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn35.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn35.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn44.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn44.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn45.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn45.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn46.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn46.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn47.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn47.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn23.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn23.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn48.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn48.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn49.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn49.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn36.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn36.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn36.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn36.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn37.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn37.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn37.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn37.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn40.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn40.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn41.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn41.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn41.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn41.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn42.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn42.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn42.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn42.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn43.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn43.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				checkAutoShowStore.Properties.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.checkAutoShowStore.Properties.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnSearchMediRecord.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnSearchMediRecord.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn14.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn14.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn15.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn15.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumnCheckStore.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn33.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn17.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn17.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn1.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn1.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn16.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn16.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn2.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn2.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn7.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn7.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn3.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn3.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn3.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn3.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn5.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn5.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn5.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn5.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn4.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn4.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn4.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn4.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn6.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn6.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn6.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn6.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn28.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn28.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn26.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn26.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn8.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn8.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn8.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn8.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn20.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn20.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn21.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn21.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn22.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn22.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn24.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn24.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn27.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn27.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn27.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn27.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn9.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn9.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn9.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn9.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn10.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn10.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn10.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn10.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn11.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn11.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn11.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn11.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn12.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn12.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn12.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn12.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn13.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn13.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn13.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn13.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtTreatmentCodeMediRecord.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtTreatmentCodeMediRecord.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn57.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn57.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				grdColView.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.grdColView.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn34.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn34.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn34.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn34.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn50.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn50.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn58.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn58.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn59.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn59.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn60.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn60.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn61.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn61.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn63.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn63.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn63.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn63.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn64.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn64.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn64.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn64.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn65.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn65.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn65.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn65.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn68.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn68.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn70.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn70.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn70.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn70.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn72.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn72.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn51.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn51.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn73.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn73.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn39.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn39.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn75.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn75.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn18.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn18.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn19.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn19.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn25.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn25.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn71.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn71.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn38.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn38.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn53.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn53.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn53.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn53.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboProgram.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboProgram.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboPatientType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboPatientType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnSaveTreatment.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnSaveTreatment.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnSaveTreatment.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnSaveTreatment.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtKeywordMediRecord.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtKeywordMediRecord.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboTreatmentType.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboTreatmentType.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboStatusEnd.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboStatusEnd.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnSearchTreatment.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.btnSearchTreatment.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtTreatmentCodeSearch.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtTreatmentCodeSearch.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtPatientCodeSearch.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtPatientCodeSearch.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				txtKeywordTreatment.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.txtKeywordTreatment.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboEndDepartment.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboEndDepartment.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciStatusEnd.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciStatusEnd.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciStoreCheckStatus.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciStoreCheckStatus.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				cboStoreCheckStatus.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.cboStoreCheckStatus.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColStoreCheck.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColStoreCheck.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColStoreCheck.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColStoreCheck.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColStoreCheckUser.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColStoreCheckUser.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColStoreCheckTime.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColStoreCheckTime.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciEndDepartment.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciEndDepartment.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciOutTimeFilter.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciOutTimeFilter.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciInTimeFilter.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciInTimeFilter.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem8.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.layoutControlItem8.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciTreatmentType.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciTreatmentType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciPatientType.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciPatientType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciStoreTimeFilter.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciStoreTimeFilter.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciProgramFilter.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciProgramFilter.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lciFilterMediRecordType.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.lciFilterMediRecordType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem3.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.layoutControlItem3.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem4.Text = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.layoutControlItem4.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridLookUpEdit1.Properties.NullText = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridLookUpEdit1.Properties.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn55.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn55.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn55.ToolTip = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn55.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColumn54.Caption = Inventec.Common.Resource.Get.Value("UCMedicalStoreV2.gridColumn54.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoaddataToTreeList()
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisDataStoreView1Filter hisDataStoreView1Filter = new HisDataStoreView1Filter();
				hisDataStoreView1Filter.IS_ACTIVE = 1;
				List<V_HIS_DATA_STORE_1> list = (from p in new BackendAdapter(commonParam).Get<List<V_HIS_DATA_STORE_1>>("api/HisDataStore/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisDataStoreView1Filter, commonParam)
					where p.ROOM_ID == currentModule.RoomId || p.ROOM_TYPE_ID == currentModule.RoomTypeId
					select p).ToList();
				if (list != null && list.Count > 0)
				{
					List<DataStoreADO> list2 = new List<DataStoreADO>();
					Dictionary<long, bool> dictionary = new Dictionary<long, bool>();
					BindingList<DataStoreADO> bindingList = treeListMedicalStore.DataSource as BindingList<DataStoreADO>;
					if (bindingList != null)
					{
						foreach (DataStoreADO item in bindingList)
						{
							dictionary[((V_HIS_DATA_STORE_1)item).ID] = item.CheckStore;
						}
					}
					foreach (V_HIS_DATA_STORE_1 item2 in list)
					{
						DataStoreADO dataStoreADO = new DataStoreADO(item2);
						if (dictionary.ContainsKey(((V_HIS_DATA_STORE_1)dataStoreADO).ID))
						{
							dataStoreADO.CheckStore = dictionary[((V_HIS_DATA_STORE_1)dataStoreADO).ID];
						}
						else
						{
							dataStoreADO.CheckStore = true;
						}
						list2.Add(dataStoreADO);
					}
					BindingList<DataStoreADO> dataSource = new BindingList<DataStoreADO>(list2);
					treeListMedicalStore.KeyFieldName = "ID";
					treeListMedicalStore.ParentFieldName = "PARENT_ID";
					treeListMedicalStore.DataSource = dataSource;
				}
				checkEdit.ValueChecked = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void RefreshDataAfterSave()
		{
			try
			{
				LoaddataToTreeList();
				LoadDataToGridControlTreatment();
				LoadDataToGridControlMediRecord();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataToGridControlTreatment()
		{
			try
			{
				int num = 0;
				num = ((ucPagingTreatment.pagingGrid == null) ? ConfigApplicationWorker.Get<int>("CONFIG_KEY__NUM_PAGESIZE") : ucPagingTreatment.pagingGrid.PageSize);
				TreatmentPaging(new CommonParam(0, num));
				CommonParam commonParam = new CommonParam();
				commonParam.Limit = rowCountTreatment;
				commonParam.Count = dataTotalTreatment;
				ucPagingTreatment.Init(new LoadDataDelegate(TreatmentPaging), commonParam, num, gridControlTreatment);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboStatus()
		{
			try
			{
				List<Status> list = new List<Status>();
				list.Add(new Status(1L, ResourceMessage.DaKetThuc));
				list.Add(new Status(2L, ResourceMessage.ChuaKetThuc));
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list2 = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list2.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("statusName", "", 300, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("statusName", "id", list2, false, 350);
				ControlEditorLoader.Load(cboStatusEnd, list, controlEditorADO);
				cboStatusEnd.EditValue = list[0].id;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadComboStoreCheckStatus()
		{
			try
			{
				List<Status> list = new List<Status>();
				list.Add(new Status(0L, ResourceMessage.TatCa));
				list.Add(new Status(1L, ResourceMessage.DaKiemTra));
				list.Add(new Status(2L, ResourceMessage.ChuaKiemTra));
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list2 = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list2.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("statusName", "", 300, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("statusName", "id", list2, false, 350);
				ControlEditorLoader.Load(cboStoreCheckStatus, list, controlEditorADO);
				cboStoreCheckStatus.EditValue = null;
				if (!string.IsNullOrEmpty(storeCheckStatusState))
				{
					cboStoreCheckStatus.EditValue = Parse.ToInt64(storeCheckStatusState);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboMediRecordType()
		{
			try
			{
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("MEDI_RECORD_TYPE_CODE", "", 70, 1));
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("MEDI_RECORD_TYPE_NAME", "", 230, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("MEDI_RECORD_TYPE_NAME", "ID", list, false, 350);
				ControlEditorLoader.Load(cboMediRecordType, BackendDataWorker.Get<HIS_MEDI_RECORD_TYPE>(), controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TreatmentPaging(object param)
		{
			try
			{
				InitRestoreLayoutGridViewFromXml(gridViewTreatment);
				InitializeStoreCheckControls();
				WaitingManager.Show();
				startPageTreatment = ((CommonParam)param).Start.GetValueOrDefault();
				int valueOrDefault = ((CommonParam)param).Limit.GetValueOrDefault();
				CommonParam commonParam = new CommonParam(startPageTreatment, valueOrDefault);
				ApiResultObject<List<L_HIS_TREATMENT_3>> apiResultObject = null;
				HisTreatmentLView3Filter filter = new HisTreatmentLView3Filter();
				SetFilterTreatment(ref filter);
				LogSystem.Debug(LogUtil.TraceData("HisTreatment/GetLView3 filter ", filter));
				apiResultObject = new BackendAdapter(commonParam).GetRO<List<L_HIS_TREATMENT_3>>("api/HisTreatment/GetLView3", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, commonParam);
				gridControlTreatment.BeginUpdate();
				if (apiResultObject != null)
				{
					List<L_HIS_TREATMENT_3> data = apiResultObject.Data;
					if (data != null && data.Count > 0)
					{
						List<TreatmentADO> list = new List<TreatmentADO>();
						foreach (L_HIS_TREATMENT_3 item in data)
						{
							TreatmentADO treatmentADO = new TreatmentADO(item);
							treatmentADO.CheckStore = false;
							list.Add(treatmentADO);
						}
						gridControlTreatment.DataSource = null;
						gridControlTreatment.DataSource = list;
						gridControlTreatment.Focus();
						gridViewTreatment.FocusedRowHandle = 0;
						rowCountTreatment = ((data != null) ? data.Count : 0);
						dataTotalTreatment = ((apiResultObject.Param != null) ? apiResultObject.Param.Count.GetValueOrDefault() : 0);
					}
					else
					{
						rowCountTreatment = 0;
						dataTotalTreatment = 0;
						gridControlTreatment.DataSource = null;
					}
				}
				else
				{
					rowCountTreatment = 0;
					dataTotalTreatment = 0;
					gridControlTreatment.DataSource = null;
				}
				gridControlTreatment.EndUpdate();
				WaitingManager.Hide();
				SessionManager.ProcessTokenLost(commonParam);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}

		private void InitControlState()
		{
			try
			{
				controlStateWorker = new ControlStateWorker();
				currentControlStateRDO = controlStateWorker.GetData("HIS.Desktop.Plugins.MedicalStoreV2");
				if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
				{
					return;
				}
				foreach (ControlStateRDO item in currentControlStateRDO)
				{
					if (item.KEY == "checkAutoShowStore")
					{
						checkAutoShowStore.Checked = item.VALUE == "1";
					}
					else if (item.KEY == "checkJustShowTreatmentLatch")
					{
						checkJustShowTreatmentLatch.Checked = item.VALUE == "1";
					}
					else if (item.KEY == "cboStoreCheckStatus")
					{
						storeCheckStatusState = item.VALUE;
					}
					if (IsRequiredLatchApproveStore == "1")
					{
						checkJustShowTreatmentLatch.CheckState = CheckState.Checked;
						checkJustShowTreatmentLatch_CheckedChanged(null, null);
						checkJustShowTreatmentLatch.Enabled = false;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool checkDigit(string s)
		{
			bool result = false;
			try
			{
				for (int i = 0; i < s.Length; i++)
				{
					result = (char.IsDigit(s[i]) ? true : false);
				}
				return result;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return result;
			}
		}

		private void SetFilterMediRecord(ref HisMediRecordView1Filter filter)
		{
			try
			{
				_FilterLoadTree = new HisTreatmentLView3Filter();
				filter.ORDER_DIRECTION = "DESC";
				filter.ORDER_FIELD = "STORE_CODE";
				if (!string.IsNullOrEmpty(txtTreatmentCodeMediRecord.Text))
				{
					string text = txtTreatmentCodeMediRecord.Text.Trim();
					if (text.Length < 12 && checkDigit(text))
					{
						text = string.Format("{0:000000000000}", System.Convert.ToInt64(text));
						txtTreatmentCodeMediRecord.Text = text;
					}
					filter.TREATMENT_CODE = text;
				}
				if (!string.IsNullOrEmpty(txtPatientCodeMediRecord.Text))
				{
					string text2 = txtPatientCodeMediRecord.Text.Trim();
					if (text2.Length < 12 && checkDigit(text2))
					{
						text2 = string.Format("{0:0000000000}", System.Convert.ToInt64(text2));
						txtPatientCodeMediRecord.Text = text2;
					}
					filter.PATIENT_CODE__EXACT = text2;
				}
				if (!string.IsNullOrEmpty(txtStoreCodeMediRecord.Text))
				{
					filter.STORE_CODE__EXACT = txtStoreCodeMediRecord.Text.Trim();
				}
				if (!string.IsNullOrEmpty(txtKeywordMediRecord.Text))
				{
					string kEY_WORD = txtKeywordMediRecord.Text.Trim();
					filter.KEY_WORD = kEY_WORD;
				}
				if (dtStoreTime_From.EditValue != null)
				{
					filter.STORE_TIME_FROM = Parse.ToInt64(dtStoreTime_From.DateTime.ToString("yyyyMMdd") + "000000");
				}
				if (dtStoreTime_To.EditValue != null)
				{
					filter.STORE_TIME_TO = Parse.ToInt64(dtStoreTime_To.DateTime.ToString("yyyyMMdd") + "235959");
				}
				if (_ProgramSelects != null && _ProgramSelects.Count > 0)
				{
					filter.PROGRAM_IDs = _ProgramSelects.Select((HIS_PROGRAM o) => o.ID).ToList();
				}
				if (rdoStatus__UnStored.Checked)
				{
					filter.IS_NOT_STORED = true;
				}
				else if (rdoStatus__Stored.Checked)
				{
					filter.IS_NOT_STORED = false;
				}
				else
				{
					filter.IS_NOT_STORED = null;
				}
				if (cboMediRecordType.EditValue != null)
				{
					filter.MEDI_RECORD_TYPE_ID = System.Convert.ToInt64(cboMediRecordType.EditValue);
				}
				List<DataStoreADO> list = new List<DataStoreADO>();
				BindingList<DataStoreADO> bindingList = (BindingList<DataStoreADO>)treeListMedicalStore.DataSource;
				if (bindingList != null && bindingList.Count > 0)
				{
					list = bindingList.ToList();
				}
				List<long> list2 = ((list != null) ? (from o in list
					where o.CheckStore
					select ((V_HIS_DATA_STORE_1)o).ID).ToList() : null);
				if (list2 != null && list2.Count > 0)
				{
					filter.DATA_STORE_IDs = list2;
				}
				else
				{
					filter.DATA_STORE_ID = 0L;
				}
				if (cboLocationStore.EditValue != null)
				{
					filter.LOCATION_STORE_ID = long.Parse(cboLocationStore.EditValue.ToString());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetFilterTreatment(ref HisTreatmentLView3Filter filter)
		{
			try
			{
				filter.ORDER_DIRECTION = "DESC";
				filter.ORDER_FIELD = "MODIFY_TIME";
				filter.IS_STORED = false;
				if (checkJustShowTreatmentLatch.Checked)
				{
					filter.APPROVAL_STORE_STT_ID = 1L;
				}
				if (!string.IsNullOrEmpty(txtTreatmentCodeSearch.Text))
				{
					string text = txtTreatmentCodeSearch.Text.Trim();
					if (text.Length < 12 && checkDigit(text))
					{
						text = string.Format("{0:000000000000}", System.Convert.ToInt64(text));
						txtTreatmentCodeSearch.Text = text;
					}
					filter.TREATMENT_CODE__EXACT = text;
				}
				if (!string.IsNullOrEmpty(txtPatientCodeSearch.Text))
				{
					string text2 = txtPatientCodeSearch.Text.Trim();
					if (text2.Length < 10 && checkDigit(text2))
					{
						text2 = string.Format("{0:0000000000}", System.Convert.ToInt64(text2));
						txtPatientCodeSearch.Text = text2;
					}
					filter.PATIENT_CODE__EXACT = text2;
				}
				if (!string.IsNullOrEmpty(txtKeywordTreatment.Text))
				{
					filter.KEY_WORD = txtKeywordTreatment.Text.Trim();
				}
				if (cboStatusEnd.EditValue != null)
				{
					if (Parse.ToInt32((cboStatusEnd.EditValue ?? "").ToString()) == 1)
					{
						filter.IS_PAUSE = true;
					}
					if (Parse.ToInt32((cboStatusEnd.EditValue ?? "").ToString()) == 2)
					{
						filter.IS_PAUSE = false;
					}
				}
				if (_EndDepartmentSelects != null && _EndDepartmentSelects.Count > 0)
				{
					filter.END_DEPARTMENT_IDs = _EndDepartmentSelects.Select((HIS_DEPARTMENT o) => o.ID).Distinct().ToList();
				}
				if (_ExecuteDepartmentSelects != null && _ExecuteDepartmentSelects.Count > 0)
				{
					filter.LAST_DEPARTMENT_IDs = _ExecuteDepartmentSelects.Select((HIS_DEPARTMENT o) => o.ID).Distinct().ToList();
				}
				if (_TreatmentTypeSelecteds != null && _TreatmentTypeSelecteds.Count > 0)
				{
					filter.TDL_TREATMENT_TYPE_IDs = _TreatmentTypeSelecteds.Select((HIS_TREATMENT_TYPE o) => o.ID).Distinct().ToList();
				}
				if (patientTypeSelecteds != null && patientTypeSelecteds.Count > 0)
				{
					filter.TDL_PATIENT_TYPE_IDs = patientTypeSelecteds.Select((HIS_PATIENT_TYPE o) => o.ID).ToList();
				}
				if (dtOutTimeFrom.EditValue != null)
				{
					filter.OUT_DATE_FROM = Parse.ToInt64(dtOutTimeFrom.DateTime.ToString("yyyyMMdd") + "000000");
				}
				if (dtOutTimeTo.EditValue != null)
				{
					filter.OUT_DATE_TO = Parse.ToInt64(dtOutTimeTo.DateTime.ToString("yyyyMMdd") + "235959");
				}
				if (dtInTimeFrom.EditValue != null)
				{
					filter.IN_DATE_FROM = Parse.ToInt64(dtInTimeFrom.DateTime.ToString("yyyyMMdd") + "000000");
				}
				if (dtInTimeTo.EditValue != null)
				{
					filter.IN_DATE_TO = Parse.ToInt64(dtInTimeTo.DateTime.ToString("yyyyMMdd") + "235959");
				}
				if (HisConfigCFG.IsUsingStoreCheck && cboStoreCheckStatus.EditValue != null)
				{
					long num = Parse.ToInt64(cboStoreCheckStatus.EditValue.ToString());
					if (num != 0)
					{
						filter.IS_STORE_CHECKED = num == 1;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				List<TreatmentADO> list = new List<TreatmentADO>();
				List<TreatmentADO> source = (List<TreatmentADO>)gridControlTreatment.DataSource;
				list = source.Where((TreatmentADO o) => o.CheckTreatment).ToList();
				if (list != null && list.Count > 0)
				{
					frmSaveStore frmSaveStore2 = new frmSaveStore(currentModule, list, new RefeshReference(RefreshDataAfterSave), currentControlStateRDO, controlStateWorker);
					frmSaveStore2.ShowDialog();
					txtPatientCodeSearch.Focus();
					txtPatientCodeSearch.SelectAll();
				}
				else
				{
					XtraMessageBox.Show(ResourceMessage.VuiLongChonHoSoCanLuuTru, ResourceMessage.ThongBao);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtSearch_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadDataToGridControlTreatment();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			try
			{
				LoadDataToGridControlTreatment();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitCombo(GridLookUpEdit cbo, object data, string DisplayValue, string ValueMember)
		{
			try
			{
				cbo.Properties.DataSource = data;
				cbo.Properties.DisplayMember = DisplayValue;
				cbo.Properties.ValueMember = ValueMember;
				GridColumn gridColumn = cbo.Properties.View.Columns.AddField(DisplayValue);
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 300;
				gridColumn.Caption = "All";
				cbo.Properties.PopupFormWidth = 300;
				cbo.Properties.View.OptionsView.ShowColumnHeaders = true;
				cbo.Properties.View.OptionsSelection.MultiSelect = true;
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = cbo.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboTreatmentType()
		{
			try
			{
				InitCombo(cboTreatmentType, BackendDataWorker.Get<HIS_TREATMENT_TYPE>(), "TREATMENT_TYPE_NAME", "ID");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboEndDepartment()
		{
			try
			{
				InitCombo(cboEndDepartment, (from p in BackendDataWorker.Get<HIS_DEPARTMENT>()
					where p.IS_ACTIVE == 1
					select p).ToList(), "DEPARTMENT_NAME", "ID");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboExecuteDepartment()
		{
			try
			{
				InitCombo(cboExecuteDepartment, (from p in BackendDataWorker.Get<HIS_DEPARTMENT>()
					where p.IS_ACTIVE == 1 && p.IS_CLINICAL == 1
					select p).ToList(), "DEPARTMENT_NAME", "ID");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void bbtnSEARCH()
		{
			try
			{
				if (btnSearchTreatment.Enabled)
				{
					btnSearch_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void bbtnSEARCHMediRecord()
		{
			try
			{
				if (btnSearchMediRecord.Enabled)
				{
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void bbtnImportStore()
		{
			try
			{
				if (btnImportStore.Enabled)
				{
					btnImportStore_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void bbtnSAVE()
		{
			try
			{
				if (btnSaveTreatment.Enabled)
				{
					btnSaveTreatment.Focus();
					btnSave_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void keyF2Focused()
		{
			try
			{
				if (txtTreatmentCodeSearch.Enabled)
				{
					txtTreatmentCodeSearch.Focus();
					txtTreatmentCodeSearch.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void keyF3Focused()
		{
			try
			{
				if (txtPatientCodeSearch.Enabled)
				{
					txtPatientCodeSearch.Focus();
					txtPatientCodeSearch.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitializePermissions()
		{
			try
			{
				if (GlobalVariables.AcsAuthorizeSDO != null)
				{
					controlAcs = GlobalVariables.AcsAuthorizeSDO.ControlInRoles;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitializeGrid()
		{
			if (HasEditPermission())
			{
				gridViewMediRecord.Columns["Edit"].Visible = true;
			}
			else
			{
				gridViewMediRecord.Columns["Edit"].Visible = false;
			}
			InitializeStoreCheckControls();
		}

		private void InitializeStoreCheckControls()
		{
			try
			{
				bool isUsingStoreCheck = HisConfigCFG.IsUsingStoreCheck;
				repositoryItemCheckEdit_StoreCheck__Disable.Enabled = false;
				gridColStoreCheck.Visible = isUsingStoreCheck && HasStoreCheckPermission();
				if (gridColStoreCheck.Visible)
				{
					gridColStoreCheck.VisibleIndex = gridColumn34.VisibleIndex + 1;
				}
				gridColStoreCheckUser.Visible = isUsingStoreCheck;
				gridColStoreCheckTime.Visible = isUsingStoreCheck;
				if (isUsingStoreCheck)
				{
					gridColStoreCheckUser.VisibleIndex = gridViewTreatment.VisibleColumns.Count - 1;
					gridColStoreCheckTime.VisibleIndex = gridViewTreatment.VisibleColumns.Count - 1;
				}
				lciStoreCheckStatus.Visibility = ((!isUsingStoreCheck) ? LayoutVisibility.Never : LayoutVisibility.Always);
				gridViewTreatment.ColumnPositionChanged -= new EventHandler(gridViewTreatment_ColumnPositionChanged);
				gridViewTreatment.ColumnPositionChanged += new EventHandler(gridViewTreatment_ColumnPositionChanged);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool HasEditPermission()
		{
			if (controlAcs != null)
			{
				return controlAcs.FirstOrDefault((ACS_CONTROL o) => o.CONTROL_CODE == "HIS000049") != null;
			}
			return false;
		}

		private void gridViewTreatment_ColumnPositionChanged(object sender, EventArgs e)
		{
			if (isEnforcingStoreCheckColumn)
			{
				return;
			}
			try
			{
				isEnforcingStoreCheckColumn = true;
				if (gridColStoreCheck.Visible && (!HisConfigCFG.IsUsingStoreCheck || !HasStoreCheckPermission()))
				{
					gridColStoreCheck.Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			finally
			{
				isEnforcingStoreCheckColumn = false;
			}
		}

		private bool HasStoreCheckPermission()
		{
			if (controlAcs != null)
			{
				return controlAcs.FirstOrDefault((ACS_CONTROL o) => o.CONTROL_CODE == "HIS000059") != null;
			}
			return false;
		}

		private void gridViewMediRecord_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				if (e.RowHandle < 0)
				{
					return;
				}
				long num = Parse.ToInt64((gridViewMediRecord.GetRowCellValue(e.RowHandle, "DATA_STORE_ID") ?? "").ToString());
				long num2 = Parse.ToInt64((gridViewMediRecord.GetRowCellValue(e.RowHandle, "IS_NOT_STORED") ?? "").ToString());
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetRow(e.RowHandle);
				if (mediRecordADO == null)
				{
					return;
				}
				if (e.Column.FieldName == "Edit")
				{
					if (HasEditPermission())
					{
						e.RepositoryItem = repositoryItemButtonEdit_Edit;
					}
					else
					{
						e.RepositoryItem = null;
					}
				}
				if (e.Column.FieldName == "Delete")
				{
					if (((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue)
					{
						e.RepositoryItem = repositoryItemButtonEdit_Delete_Disable;
					}
					else
					{
						e.RepositoryItem = ((num > 0) ? repositoryItemButtonEdit_Delete_Enable : repositoryItemButtonEdit_Delete_Disable);
					}
				}
				else if (e.Column.FieldName == "Print")
				{
					e.RepositoryItem = ((num > 0) ? repositoryItemButtonEdit_Print_Enable : repositoryItemButtonEdit_Print_Disable);
				}
				else if (e.Column.FieldName == "TreatmentBorrow")
				{
					e.RepositoryItem = ((!((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue || (((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue && ((V_HIS_MEDI_RECORD_1)mediRecordADO).RECEIVE_DATE.HasValue)) ? Btn_TreatmentBorrow_Enable : Btn_TreatmentBorrow_Disable);
				}
				else if (e.Column.FieldName == "ReceiveTreatment")
				{
					e.RepositoryItem = ((((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue && !((V_HIS_MEDI_RECORD_1)mediRecordADO).RECEIVE_DATE.HasValue) ? ButtonEditReceiveTreatment_Enabed : ButtonEditReceiveTreatment_Disabed);
				}
				else if (e.Column.FieldName == "IMPORT_OR_UNIMPORT")
				{
					if (((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED == 1)
					{
						e.RepositoryItem = ButtonEditImport;
					}
					else if (!((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED.HasValue)
					{
						e.RepositoryItem = ButtonEditUnImport;
					}
				}
				else if (e.Column.FieldName == "CheckStore")
				{
					if (((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED == 1)
					{
						e.RepositoryItem = repositoryItemCheckEdit_Store__Enable;
					}
					else if (!((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED.HasValue)
					{
						e.RepositoryItem = repositoryItemCheckEdit_Store__Disable;
					}
				}
				else if (e.Column.FieldName == "ViewEmr")
				{
					if (string.IsNullOrWhiteSpace(((V_HIS_MEDI_RECORD_1)mediRecordADO).TDL_TREATMENT_CODES))
					{
						e.RepositoryItem = repositoryItemButtonView_Disable;
					}
					else if (!((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED.HasValue)
					{
						e.RepositoryItem = repositoryItemButtonEdit_ViewEmrTreatment;
					}
				}
				else if (e.Column.FieldName == "LOCATION_STORE_ID")
				{
					if (num2 == 1)
					{
						e.RepositoryItem = repLocationStore;
					}
					else
					{
						e.RepositoryItem = repLocationStoreDis;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatment_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				MediRecordADO mediRecordADO = (MediRecordADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (mediRecordADO != null)
				{
					if (e.Column.FieldName == "STT")
					{
						e.Value = e.ListSourceRowIndex + 1 + startPageTreatment;
					}
					else if (e.Column.FieldName == "TDL_TREATMENT_IN_CODES_STR")
					{
						e.Value = ((V_HIS_MEDI_RECORD_1)mediRecordADO).TDL_TREATMENT_IN_CODES;
					}
					else if (e.Column.FieldName == "CREATE_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((V_HIS_MEDI_RECORD_1)mediRecordADO).CREATE_TIME.GetValueOrDefault());
					}
					else if (e.Column.FieldName == "MODIFY_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((V_HIS_MEDI_RECORD_1)mediRecordADO).MODIFY_TIME.GetValueOrDefault());
					}
					else if (e.Column.FieldName == "DOB_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(((V_HIS_MEDI_RECORD_1)mediRecordADO).DOB);
					}
					else if (!(e.Column.FieldName == "STORE_CODE_CONFIG"))
					{
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatment_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetRow(e.RowHandle);
				if (mediRecordADO == null)
				{
					return;
				}
				if (((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue && !((V_HIS_MEDI_RECORD_1)mediRecordADO).RECEIVE_DATE.HasValue)
				{
					int num = Calculation.DifferenceDate((long)((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.Value, Inventec.Common.DateTime.Get.Now().GetValueOrDefault());
					if (num <= 10)
					{
						e.Appearance.ForeColor = Color.Green;
					}
					else
					{
						e.Appearance.ForeColor = Color.Red;
					}
				}
				if (((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED == 1 && ((V_HIS_MEDI_RECORD_1)mediRecordADO).STORE_TIME.HasValue)
				{
					TimeSpan timeSpan = DateTime.Now - Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(((V_HIS_MEDI_RECORD_1)mediRecordADO).STORE_TIME.Value).GetValueOrDefault();
					if (HisConfigCFG.BlueWarningStoreTime.HasValue && timeSpan.TotalHours > (double)HisConfigCFG.BlueWarningStoreTime.Value)
					{
						e.Appearance.ForeColor = Color.Blue;
					}
					if (HisConfigCFG.OrangeWarningStoreTime.HasValue && timeSpan.TotalHours > (double)HisConfigCFG.OrangeWarningStoreTime.Value)
					{
						e.Appearance.ForeColor = Color.Orange;
					}
					if (HisConfigCFG.RedWarningStoreTime.HasValue && timeSpan.TotalHours > (double)HisConfigCFG.RedWarningStoreTime.Value)
					{
						e.Appearance.ForeColor = Color.Red;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void treeListMedicalStore_CustomDrawColumnHeader(object sender, CustomDrawColumnHeaderEventArgs e)
		{
			try
			{
				TreeList treeList = sender as TreeList;
				if (e.Column != null && e.Column.VisibleIndex == 0)
				{
					Rectangle r = new Rectangle(e.Bounds.Left + treeList.Columns.FirstOrDefault((TreeListColumn o) => o.FieldName == "CheckStore").Width / 2 + 5, e.Bounds.Top + 2, 12, 12);
					DevExpress.XtraTreeList.ViewInfo.ColumnInfo columnInfo = (DevExpress.XtraTreeList.ViewInfo.ColumnInfo)e.ObjectArgs;
					if (columnInfo.CaptionRect.Left < 15)
					{
						columnInfo.CaptionRect = new Rectangle(new Point(columnInfo.CaptionRect.Left + 15, columnInfo.CaptionRect.Top), columnInfo.CaptionRect.Size);
					}
					e.Painter.DrawObject(columnInfo);
					DrawCheckBox(e.Graphics, checkEdit, r, IsAllSelected(sender as TreeList));
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void treeList1_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				TreeList treeList = sender as TreeList;
				Point pt = new Point(e.X, e.Y);
				TreeListHitInfo treeListHitInfo = treeList.CalcHitInfo(pt);
				if (treeListHitInfo.Column == null)
				{
					return;
				}
				DevExpress.XtraTreeList.ViewInfo.ColumnInfo columnInfo = treeList.ViewInfo.ColumnsInfo[treeListHitInfo.Column];
				Rectangle rectangle = new Rectangle(columnInfo.Bounds.Left + treeList.Columns.FirstOrDefault((TreeListColumn o) => o.FieldName == "CheckStore").Width / 2 + 5, columnInfo.Bounds.Top + 2, 12, 12);
				if (!rectangle.Contains(pt))
				{
					return;
				}
				List<DataStoreADO> list = new List<DataStoreADO>();
				BindingList<DataStoreADO> bindingList = (BindingList<DataStoreADO>)treeListMedicalStore.DataSource;
				if (bindingList != null && bindingList.Count > 0)
				{
					list = bindingList.ToList();
				}
				if (list != null && list.Count > 0)
				{
					if ((bool)checkEdit.ValueChecked)
					{
						foreach (DataStoreADO item in list)
						{
							item.CheckStore = true;
						}
						checkEdit.ValueChecked = false;
					}
					else
					{
						foreach (DataStoreADO item2 in list)
						{
							item2.CheckStore = false;
						}
						checkEdit.ValueChecked = true;
					}
				}
				if (list != null)
				{
					BindingList<DataStoreADO> dataSource = new BindingList<DataStoreADO>(list);
					treeListMedicalStore.KeyFieldName = "ID";
					treeListMedicalStore.ParentFieldName = "PARENT_ID";
					treeListMedicalStore.DataSource = dataSource;
				}
				LoadDataToGridControlMediRecord();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repositoryItemCheckEdit_CheckStore_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				treeListMedicalStore.PostEditor();
				WaitingManager.Show();
				DataStoreADO ado = (DataStoreADO)treeListMedicalStore.GetDataRecordByNode(treeListMedicalStore.FocusedNode);
				List<DataStoreADO> list = null;
				BindingList<DataStoreADO> bindingList = (BindingList<DataStoreADO>)treeListMedicalStore.DataSource;
				if (bindingList != null && bindingList.Count > 0)
				{
					list = bindingList.ToList();
				}
				if (ado != null && list != null && list.Count > 0)
				{
					list.ForEach(delegate(DataStoreADO o)
					{
						if (((V_HIS_DATA_STORE_1)o).PARENT_ID == ((V_HIS_DATA_STORE_1)ado).ID)
						{
							o.CheckStore = ado.CheckStore;
						}
					});
					BindingList<DataStoreADO> dataSource = new BindingList<DataStoreADO>(list);
					treeListMedicalStore.KeyFieldName = "ID";
					treeListMedicalStore.ParentFieldName = "PARENT_ID";
					treeListMedicalStore.DataSource = dataSource;
				}
				gridViewMediRecord.Focus();
				LoadDataToGridControlMediRecord();
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemButtonEdit_Delete_Enable_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewMediRecord.GetFocusedRow();
				if (treatmentADO != null)
				{
					bool value = false;
					CommonParam commonParam = new CommonParam();
					L_HIS_TREATMENT_3 val = new BackendAdapter(commonParam).Post<L_HIS_TREATMENT_3>("api/HisTreatment/UpdateDataStoreId", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, treatmentADO, commonParam);
					if (val != null)
					{
						value = true;
						LoaddataToTreeList();
						LoadDataToGridControlTreatment();
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemButtonEdit_Print_Enable_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					PrintMediRecord(mediRecordADO);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Btn_AddPatientProgram_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewMediRecord.GetFocusedRow();
				if (treatmentADO == null)
				{
					return;
				}
				Module module = GlobalVariables.currentModuleRaws.Where((Module o) => o.ModuleLink == "HIS.Desktop.Plugins.HisPatientProgram").FirstOrDefault();
				if (module == null)
				{
					LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.HisPatientProgram");
				}
				if (module.IsPlugin && module.ExtensionInfo != null)
				{
					List<object> arrParams = new List<object>();
					object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, currentModule.RoomId, currentModule.RoomTypeId), arrParams);
					if (pluginInstance == null)
					{
						throw new ArgumentNullException("moduleData is null");
					}
					((Form)pluginInstance).ShowDialog();
					LoadDataToGridControlTreatment();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Btn_TreatmentBorrow_Enable_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					TreatmentBorrow treatmentBorrow = new TreatmentBorrow(mediRecordADO, new RefeshReference(RefreshDataAfterSaveMediRecord));
					treatmentBorrow.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void RefreshDataAfterSaveMediRecord()
		{
			try
			{
				LoaddataToTreeList();
				LoadDataToGridControlMediRecord();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboStatusEnd_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboStatusEnd.Properties.Buttons[1].Visible = true;
					cboStatusEnd.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtSearchTreatment_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadDataToGridControlTreatment();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtProgramCode_KeyUp(object sender, KeyEventArgs e)
		{
		}

		private void cboEndDepartment_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboEndDepartment.EditValue = null;
					_EndDepartmentSelects = null;
					ResetCombo(cboEndDepartment);
					cboStatusEnd.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPatientType.EditValue = null;
					patientTypeSelecteds = null;
					ResetCombo(cboPatientType);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTreatmentType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboTreatmentType.EditValue = null;
					_TreatmentTypeSelecteds = null;
					ResetCombo(cboTreatmentType);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtStoreCode_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboEndDepartment_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string text = "";
				if (_EndDepartmentSelects != null && _EndDepartmentSelects.Count > 0)
				{
					foreach (HIS_DEPARTMENT endDepartmentSelect in _EndDepartmentSelects)
					{
						text = text + endDepartmentSelect.DEPARTMENT_NAME + ", ";
					}
					cboStatusEnd.EditValue = 1;
				}
				else
				{
					cboStatusEnd.Enabled = true;
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTreatmentType_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string text = "";
				if (_TreatmentTypeSelecteds != null && _TreatmentTypeSelecteds.Count > 0)
				{
					foreach (HIS_TREATMENT_TYPE treatmentTypeSelected in _TreatmentTypeSelecteds)
					{
						text = text + treatmentTypeSelected.TREATMENT_TYPE_NAME + ", ";
					}
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientType_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string text = "";
				if (patientTypeSelecteds != null && patientTypeSelecteds.Count > 0)
				{
					foreach (HIS_PATIENT_TYPE patientTypeSelected in patientTypeSelecteds)
					{
						text = text + patientTypeSelected.PATIENT_TYPE_NAME + ", ";
					}
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ResetCombo(GridLookUpEdit cbo)
		{
			try
			{
				HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection gridCheckMarksSelection = cbo.Properties.Tag as HIS.Desktop.Utilities.Extensions.GridCheckMarksSelection;
				if (gridCheckMarksSelection != null)
				{
					gridCheckMarksSelection.ClearSelection(cbo.Properties.View);
					cbo.Enabled = false;
					cbo.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool IsAllSelected(TreeList tree)
		{
			return tree.Selection.Count > 0 && tree.Selection.Count == tree.AllNodesCount;
		}

		protected void DrawCheckBox(Graphics g, RepositoryItemCheckEdit edit, Rectangle r, bool Checked)
		{
			CheckEditViewInfo checkEditViewInfo = edit.CreateViewInfo() as CheckEditViewInfo;
			CheckEditPainter checkEditPainter = edit.CreatePainter() as CheckEditPainter;
			checkEditViewInfo.EditValue = Checked;
			checkEditViewInfo.Bounds = r;
			checkEditViewInfo.CalcViewInfo(g);
			ControlGraphicsInfoArgs controlGraphicsInfoArgs = new ControlGraphicsInfoArgs(checkEditViewInfo, new GraphicsCache(g), r);
			checkEditPainter.Draw(controlGraphicsInfoArgs);
			controlGraphicsInfoArgs.Cache.Dispose();
		}

		private void EmbeddedCheckBoxChecked(TreeList tree)
		{
			if (IsAllSelected(tree))
			{
				tree.Selection.Clear();
			}
			else
			{
				SelectAll(tree);
			}
		}

		private void SelectAll(TreeList tree)
		{
			tree.BeginUpdate();
			tree.NodesIterator.DoOperation(new SelectNodeOperation());
			tree.EndUpdate();
		}

		private void gridViewTreatment_CustomUnboundColumnData_1(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				TreatmentADO treatmentADO = (TreatmentADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (treatmentADO == null)
				{
					return;
				}
				if (e.Column.FieldName == "STT")
				{
					e.Value = e.ListSourceRowIndex + 1 + startPageTreatment;
				}
				else if (e.Column.FieldName == "IN_CODE_STR")
				{
					e.Value = ((L_HIS_TREATMENT_3)treatmentADO).IN_CODE;
				}
				else if (e.Column.FieldName == "IN_TIME_STR")
				{
					e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).IN_TIME);
				}
				else if (e.Column.FieldName == "OUT_TIME_STR")
				{
					e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).OUT_TIME.GetValueOrDefault());
				}
				else if (e.Column.FieldName == "DOB_STR")
				{
					e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(((L_HIS_TREATMENT_3)treatmentADO).TDL_PATIENT_DOB);
				}
				else if (e.Column.FieldName == "AGE")
				{
					e.Value = AgeHelper.CalculateAgeFromYear(((L_HIS_TREATMENT_3)treatmentADO).TDL_PATIENT_DOB) + "  tuổi";
				}
				else if (!(e.Column.FieldName == "TDL_PATIENT_GENDER_NAME"))
				{
					if (e.Column.FieldName == "REJECT_STORE_REASON_STR")
					{
						e.Value = ((L_HIS_TREATMENT_3)treatmentADO).REJECT_STORE_REASON;
					}
					else if (e.Column.FieldName == "STORE_CHECK_TIME_STR")
					{
						e.Value = (((L_HIS_TREATMENT_3)treatmentADO).STORE_CHECK_TIME.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).STORE_CHECK_TIME.Value) : "");
					}
					else if (e.Column.FieldName == "CLINICAL_IN_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).CLINICAL_IN_TIME.GetValueOrDefault());
					}
					else if (e.Column.FieldName == "APPROVAL_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).APPROVAL_TIME.GetValueOrDefault());
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatmentOfMediRecord_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				TreatmentADO treatmentADO = (TreatmentADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (treatmentADO == null)
				{
					return;
				}
				if (e.Column.FieldName == "STT")
				{
					e.Value = e.ListSourceRowIndex + 1 + startPageTreatment;
				}
				else if (!(e.Column.FieldName == "CREATE_TIME_STR") && !(e.Column.FieldName == "MODIFY_TIME_STR"))
				{
					if (e.Column.FieldName == "IN_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).IN_TIME);
					}
					else if (e.Column.FieldName == "OUT_TIME_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((L_HIS_TREATMENT_3)treatmentADO).OUT_TIME.GetValueOrDefault());
					}
					else if (e.Column.FieldName == "DOB_STR")
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(((L_HIS_TREATMENT_3)treatmentADO).TDL_PATIENT_DOB);
					}
					else if (e.Column.FieldName == "AGE")
					{
						e.Value = AgeHelper.CalculateAgeFromYear(((L_HIS_TREATMENT_3)treatmentADO).TDL_PATIENT_DOB) + "  tuổi";
					}
					else if (!(e.Column.FieldName == "STORE_CODE_CONFIG"))
					{
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSearchMediRecord_Click(object sender, EventArgs e)
		{
			try
			{
				LoadDataToGridControlMediRecord();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataToGridControlMediRecord()
		{
			try
			{
				int num = 0;
				num = ((ucPagingMediRecord.pagingGrid == null) ? ConfigApplicationWorker.Get<int>("CONFIG_KEY__NUM_PAGESIZE") : ucPagingMediRecord.pagingGrid.PageSize);
				MediRecordPaging(new CommonParam(0, num));
				CommonParam commonParam = new CommonParam();
				commonParam.Limit = rowCountMediRecord;
				commonParam.Count = dataTotalMediRecord;
				ucPagingMediRecord.Init(new LoadDataDelegate(MediRecordPaging), commonParam, num, gridControlMediRecord);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

        List<MediRecordADO> listMediRecordPage = new List<MediRecordADO>();

		private void MediRecordPaging(object param)
		{
			try
			{
				InitRestoreLayoutGridViewFromXml(gridViewMediRecord);
				WaitingManager.Show();
				startPageMediRecord = ((CommonParam)param).Start.GetValueOrDefault();
				int valueOrDefault = ((CommonParam)param).Limit.GetValueOrDefault();
				CommonParam commonParam = new CommonParam(startPageMediRecord, valueOrDefault);
				ApiResultObject<List<V_HIS_MEDI_RECORD_1>> apiResultObject = null;
				HisMediRecordView1Filter filter = new HisMediRecordView1Filter();
				SetFilterMediRecord(ref filter);
				LogSystem.Debug(LogUtil.TraceData("api/HisMediRecord/GetView1 filter", filter));
				apiResultObject = new BackendAdapter(commonParam).GetRO<List<V_HIS_MEDI_RECORD_1>>("api/HisMediRecord/GetView1", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, commonParam);
				gridControlMediRecord.BeginUpdate();
				if (apiResultObject != null)
				{
					List<V_HIS_MEDI_RECORD_1> data = apiResultObject.Data;
					if (data != null && data.Count > 0)
					{
						
						foreach (V_HIS_MEDI_RECORD_1 item in data)
						{
							MediRecordADO mediRecordADO = new MediRecordADO(item);
							mediRecordADO.CheckStore = false;
                            listMediRecordPage.Add(mediRecordADO);
						}
						gridControlMediRecord.DataSource = null;
                        gridControlMediRecord.DataSource = listMediRecordPage;
						rowCountMediRecord = ((data != null) ? data.Count : 0);
						dataTotalMediRecord = ((apiResultObject.Param != null) ? apiResultObject.Param.Count.GetValueOrDefault() : 0);
					}
					else
					{
						rowCountMediRecord = 0;
						dataTotalMediRecord = 0;
						gridControlMediRecord.DataSource = null;
					}
				}
				else
				{
					rowCountMediRecord = 0;
					dataTotalMediRecord = 0;
					gridControlMediRecord.DataSource = null;
				}
				gridControlMediRecord.EndUpdate();
				WaitingManager.Hide();
				SessionManager.ProcessTokenLost(commonParam);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}

		private void cboProgram_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboProgram.EditValue = null;
					_ProgramSelects = null;
					ResetCombo(cboProgram);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboProgram_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string text = "";
				if (_ProgramSelects != null && _ProgramSelects.Count > 0)
				{
					foreach (HIS_PROGRAM programSelect in _ProgramSelects)
					{
						text = text + programSelect.PROGRAM_NAME + ", ";
					}
				}
				else
				{
					cboStatusEnd.Enabled = true;
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewMediRecord_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
				{
					return;
				}
				GridView gridView = sender as GridView;
				GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.Location);
				if (gridHitInfo.InRowCell || (gridHitInfo.Column.RealColumnEdit.GetType() == typeof(RepositoryItemCheckEdit)))
				{
                    gridView.FocusedRowHandle = gridHitInfo.RowHandle;
                    gridView.FocusedColumn = gridHitInfo.Column;
                    gridView.ShowEditor();
                    CheckEdit checkEdit = gridView.ActiveEditor as CheckEdit;
                    if (checkEdit == null)
                    {
                        return;
                    }
                    CheckEditViewInfo checkEditViewInfo = (CheckEditViewInfo)checkEdit.GetViewInfo();
                    Rectangle glyphRect = checkEditViewInfo.CheckInfo.GlyphRect;
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
                if (gridHitInfo.HitTest == GridHitTest.Column && gridHitInfo.Column.FieldName == "CheckStore")
                {
                    List<MediRecordADO> list = listMediRecordPage;
                    List<MediRecordADO> list2 = new List<MediRecordADO>();
                    if (list != null && list.Count > 0)
                    {
                        int num = listMediRecordPage.Where((MediRecordADO o) => o.CheckStore).Count();
                        int num2 = listMediRecordPage.Count();
                        if ((num > 0 && num < num2) || num == 0)
                        {
                            isCheckAll = true;
                            gridHitInfo.Column.Image = imageCollectionRoom1.Images[1];
                        }
                        if (num == num2)
                        {
                            isCheckAll = false;
                            gridHitInfo.Column.Image = imageCollectionRoom1.Images[0];
                        }
                        if (isCheckAll)
                        {
                            foreach (MediRecordADO item in list)
                            {
                                long iD = item.ID;
                                bool flag = 1 == 0;
                                item.CheckStore = true;
                                list2.Add(item);
                            }
                            isCheckAll = false;
                        }
                        else
                        {
                            foreach (MediRecordADO item2 in list)
                            {
                                long iD2 = item2.ID;
                                bool flag = 1 == 0;
                                item2.CheckStore = false;
                                list2.Add(item2);
                            }
                            isCheckAll = true;
                        }
                        RoomProcessor.Reload(ucGridControlRoom, list2);
                    }
                }
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatment_RowCellStyle_1(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetRow(e.RowHandle);
				if (treatmentADO == null)
				{
					return;
				}
				if (((L_HIS_TREATMENT_3)treatmentADO).TDL_HEIN_CARD_NUMBER != null)
				{
					e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
				}
				if (!((L_HIS_TREATMENT_3)treatmentADO).FEE_LOCK_TIME.HasValue)
				{
					return;
				}
				try
				{
					TimeSpan timeSpan = DateTime.Now - Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(((L_HIS_TREATMENT_3)treatmentADO).FEE_LOCK_TIME.GetValueOrDefault()).GetValueOrDefault();
					if (blueConfig.HasValue)
					{
						if (orangeConfig.HasValue)
						{
							if (timeSpan.TotalHours > orangeConfig)
							{
								if (orangeConfig > blueConfig)
								{
									e.Appearance.ForeColor = Color.Orange;
								}
								else if (timeSpan.TotalHours > blueConfig)
								{
									e.Appearance.ForeColor = Color.Blue;
								}
							}
							else if (timeSpan.TotalHours > blueConfig)
							{
								e.Appearance.ForeColor = Color.Blue;
							}
						}
						else if (timeSpan.TotalHours > blueConfig)
						{
							e.Appearance.ForeColor = Color.Blue;
						}
					}
					else if (orangeConfig.HasValue && timeSpan.TotalHours > orangeConfig)
					{
						e.Appearance.ForeColor = Color.Orange;
					}
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void gridViewMediRecord_RowCellClick(object sender, RowCellClickEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					InitRestoreLayoutGridViewFromXml(gridViewTreatmentOfMediRecord);
					HisTreatmentLView3Filter hisTreatmentLView3Filter = new HisTreatmentLView3Filter();
					hisTreatmentLView3Filter.MEDI_RECORD_ID = ((V_HIS_MEDI_RECORD_1)mediRecordADO).ID;
					WaitingManager.Show();
					List<L_HIS_TREATMENT_3> list = new BackendAdapter(new CommonParam()).Get<List<L_HIS_TREATMENT_3>>("api/HisTreatment/GetLView3", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentLView3Filter, null);
					WaitingManager.Hide();
					if (list != null && list.Count > 0)
					{
						gridControlTreatmentOfMediRecord.BeginUpdate();
						List<TreatmentADO> list2 = new List<TreatmentADO>();
						foreach (L_HIS_TREATMENT_3 item in list)
						{
							TreatmentADO treatmentADO = new TreatmentADO(item);
							treatmentADO.CheckStore = true;
							list2.Add(treatmentADO);
						}
						gridControlTreatmentOfMediRecord.DataSource = null;
						gridControlTreatmentOfMediRecord.DataSource = list2;
						gridControlTreatmentOfMediRecord.EndUpdate();
					}
					else
					{
						gridControlTreatmentOfMediRecord.BeginUpdate();
						gridControlTreatmentOfMediRecord.DataSource = null;
						gridControlTreatmentOfMediRecord.EndUpdate();
					}
				}
				else
				{
					gridControlTreatmentOfMediRecord.BeginUpdate();
					gridControlTreatmentOfMediRecord.DataSource = null;
					gridControlTreatmentOfMediRecord.EndUpdate();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ButtonEditReceiveTreatment_Enabed_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO == null)
				{
					return;
				}
				bool value = false;
				CommonParam commonParam = new CommonParam();
				HisMediRecordBorrowFilter hisMediRecordBorrowFilter = new HisMediRecordBorrowFilter();
				hisMediRecordBorrowFilter.MEDI_RECORD_ID = ((V_HIS_MEDI_RECORD_1)mediRecordADO).ID;
				hisMediRecordBorrowFilter.IS_RECEIVE = false;
				List<HIS_MEDI_RECORD_BORROW> list = new BackendAdapter(new CommonParam()).Get<List<HIS_MEDI_RECORD_BORROW>>("api/HisMediRecordBorrow/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMediRecordBorrowFilter, null);
				if (list != null && list.Count > 0)
				{
					HIS_MEDI_RECORD_BORROW val = list.OrderByDescending((HIS_MEDI_RECORD_BORROW x) => x.ID).FirstOrDefault();
					if (val != null)
					{
						HIS_MEDI_RECORD_BORROW val2 = new HIS_MEDI_RECORD_BORROW();
						val2.MEDI_RECORD_ID = ((V_HIS_MEDI_RECORD_1)mediRecordADO).ID;
						val2.ID = val.ID;
						WaitingManager.Show();
						HIS_MEDI_RECORD_BORROW val3 = new BackendAdapter(commonParam).Post<HIS_MEDI_RECORD_BORROW>("api/HisMediRecordBorrow/Receive", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, val2, commonParam);
						WaitingManager.Hide();
						if (val3 != null)
						{
							value = true;
							btnSearchMediRecord_Click(null, null);
						}
						MessageManager.Show(base.ParentForm, commonParam, value);
						SessionManager.ProcessTokenLost(commonParam);
					}
				}
				else
				{
					LogSystem.Debug("mediRecordBorrows null");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ButtonEditDeleteEnable_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatmentOfMediRecord.GetFocusedRow();
				if (treatmentADO != null && XtraMessageBox.Show(ResourceMessage.BanCoMuonXoaDuLieuKhong, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					bool value = false;
					CommonParam commonParam = new CommonParam();
					HIS_TREATMENT hIS_TREATMENT = new HIS_TREATMENT();
					hIS_TREATMENT.ID = ((L_HIS_TREATMENT_3)treatmentADO).ID;
					hIS_TREATMENT.STORE_CODE = null;
					WaitingManager.Show();
					HIS_TREATMENT hIS_TREATMENT2 = new BackendAdapter(commonParam).Post<HIS_TREATMENT>("api/HisTreatment/OutOfMediRecord", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hIS_TREATMENT, commonParam);
					WaitingManager.Hide();
					if (hIS_TREATMENT2 != null)
					{
						value = true;
						gridViewTreatmentOfMediRecord.DeleteRow(gridViewTreatmentOfMediRecord.FocusedRowHandle);
						LoadDataToGridControlTreatment();
						LoadDataToGridControlMediRecord();
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatmentOfMediRecord_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatmentOfMediRecord.GetRow(e.RowHandle);
				if (treatmentADO != null && ((L_HIS_TREATMENT_3)treatmentADO).TDL_HEIN_CARD_NUMBER != null)
				{
					e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewTreatmentOfMediRecord_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				if (e.RowHandle >= 0)
				{
					MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
					if (mediRecordADO != null && e.Column.FieldName == "Delete")
					{
						e.RepositoryItem = ((!((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue || (((V_HIS_MEDI_RECORD_1)mediRecordADO).GIVE_DATE.HasValue && ((V_HIS_MEDI_RECORD_1)mediRecordADO).RECEIVE_DATE.HasValue)) ? ButtonEditDeleteEnable : ButtonEditDeleteDisable);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtTreatmentCodeMediRecord_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnViewTreatment_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				if (treatmentADO != null)
				{
					WaitingManager.Show();
					List<object> list = new List<object>();
					list.Add(((L_HIS_TREATMENT_3)treatmentADO).TREATMENT_CODE);
					if (currentModule != null)
					{
						CallModule callModule = new CallModule("HIS.Desktop.Plugins.EmrDocument", currentModule.RoomId, currentModule.RoomTypeId, list);
					}
					else
					{
						CallModule callModule2 = new CallModule("HIS.Desktop.Plugins.EmrDocument", 0L, 0L, list);
					}
					WaitingManager.Hide();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ButtonEditImport_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					bool value = false;
					CommonParam commonParam = new CommonParam();
					WaitingManager.Show();
					HisMediRecordStoreSDO sdo = new HisMediRecordStoreSDO();
					sdo.id = ((V_HIS_MEDI_RECORD_1)mediRecordADO).ID;
					sdo.LocationStoreId = ((V_HIS_MEDI_RECORD_1)mediRecordADO).LOCATION_STORE_ID;
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => sdo), sdo));
					HIS_MEDI_RECORD val = new BackendAdapter(commonParam).Post<HIS_MEDI_RECORD>("api/HisMediRecord/Store", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, sdo, commonParam);
					WaitingManager.Hide();
					if (val != null)
					{
						value = true;
						btnSearchMediRecord_Click(null, null);
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ButtonEditUnImport_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					bool value = false;
					CommonParam commonParam = new CommonParam();
					WaitingManager.Show();
					HIS_MEDI_RECORD val = new BackendAdapter(commonParam).Post<HIS_MEDI_RECORD>("api/HisMediRecord/UnStore", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, ((V_HIS_MEDI_RECORD_1)mediRecordADO).ID, commonParam);
					WaitingManager.Hide();
					if (val != null)
					{
						value = true;
						btnSearchMediRecord_Click(null, null);
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtKeywordMediRecord_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemButtonEdit_ViewEmrTreatment_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					WaitingManager.Show();
					List<object> list = new List<object>();
					List<string> item = ((V_HIS_MEDI_RECORD_1)mediRecordADO).TDL_TREATMENT_CODES.Split(',').ToList();
					list.Add(item);
					list.Add(true);
					if (currentModule != null)
					{
						CallModule callModule = new CallModule("HIS.Desktop.Plugins.EmrDocument", currentModule.RoomId, currentModule.RoomTypeId, list);
					}
					else
					{
						CallModule callModule2 = new CallModule("HIS.Desktop.Plugins.EmrDocument", 0L, 0L, list);
					}
					WaitingManager.Hide();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkAutoShowStore_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (isInit)
				{
					return;
				}
				WaitingManager.Show();
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == "checkAutoShowStore" && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (checkAutoShowStore.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = "checkAutoShowStore";
					csAddOrUpdate.VALUE = (checkAutoShowStore.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtPatientCodeSearch_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadDataToGridControlTreatment();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemCheckEdit3_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				gridViewTreatment.PostEditor();
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				if (checkAutoShowStore.Checked && treatmentADO != null && treatmentADO.CheckTreatment)
				{
					List<TreatmentADO> list = new List<TreatmentADO>();
					List<TreatmentADO> source = (List<TreatmentADO>)gridControlTreatment.DataSource;
					list = source.Where((TreatmentADO o) => o.CheckTreatment).ToList();
					if (list != null && list.Count > 0)
					{
						frmSaveStore frmSaveStore2 = new frmSaveStore(currentModule, list, new RefeshReference(RefreshDataAfterSave), currentControlStateRDO, controlStateWorker);
						frmSaveStore2.ShowDialog();
						txtPatientCodeSearch.Focus();
						txtPatientCodeSearch.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMediRecordType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboMediRecordType.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtPatientCodeMediRecord_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rdoStatus__All_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (rdoStatus__All.Checked)
				{
					rdoStatus__UnStored.Checked = false;
					rdoStatus__Stored.Checked = false;
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rdoStatus__Stored_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (rdoStatus__UnStored.Checked)
				{
					rdoStatus__All.Checked = false;
					rdoStatus__Stored.Checked = false;
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rdoStatus__UnStored_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (rdoStatus__Stored.Checked)
				{
					rdoStatus__All.Checked = false;
					rdoStatus__UnStored.Checked = false;
					btnSearchMediRecord_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnImportStore_Click(object sender, EventArgs e)
		{
			try
			{
				List<MediRecordADO> list = (List<MediRecordADO>)gridControlMediRecord.DataSource;
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<MediRecordADO> list2 = list.Where((MediRecordADO o) => o.CheckStore).ToList();
				if (list2 != null && list2.Count > 0)
				{
					LogSystem.Debug(LogUtil.TraceData("btnImportStore_Click mediRecordCheck ", list2));
					CommonParam commonParam = new CommonParam();
					var list3 = list2.Select((MediRecordADO o) => new
					{
						((V_HIS_MEDI_RECORD_1)o).ID,
						((V_HIS_MEDI_RECORD_1)o).LOCATION_STORE_ID
					}).Distinct().ToList();
					bool value = false;
					List<HIS_MEDI_RECORD> list4 = null;
					List<HisMediRecordStoreSDO> lstSdo = new List<HisMediRecordStoreSDO>();
					foreach (var item in list3)
					{
						HisMediRecordStoreSDO hisMediRecordStoreSDO = new HisMediRecordStoreSDO();
						hisMediRecordStoreSDO.id = item.ID;
						hisMediRecordStoreSDO.LocationStoreId = item.LOCATION_STORE_ID;
						lstSdo.Add(hisMediRecordStoreSDO);
					}
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstSdo), lstSdo));
					list4 = new BackendAdapter(commonParam).Post<List<HIS_MEDI_RECORD>>("api/HisMediRecord/StoreList", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, lstSdo, commonParam);
					if (list4 != null && list4.Count > 0)
					{
						value = true;
						btnSearchMediRecord_Click(null, null);
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
				else
				{
					MessageManager.Show(ResourceMessage.VuiLongChonHoSoCanLuuTru);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repositoryItemButtonEdit_RejectStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				if (treatmentADO != null)
				{
					frmRejectStore frmRejectStore2 = new frmRejectStore(currentModule, treatmentADO, new DelegateSelectData(refreshDataTreatment));
					frmRejectStore2.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repositoryItemCheckEdit_StoreCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (isProcessingStoreCheck)
			{
				return;
			}
			try
			{
				CheckEdit checkEdit = sender as CheckEdit;
				if (checkEdit == null || !checkEdit.Checked)
				{
					return;
				}
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				if (treatmentADO == null || treatmentADO.IsStoreChecked)
				{
					return;
				}
				isProcessingStoreCheck = true;
				try
				{
					if (!HasStoreCheckPermission())
					{
						RevertStoreCheckCell(checkEdit);
						XtraMessageBox.Show(ResourceMessage.KhongDuQuyenDanhDauDaKiemTra, ResourceMessage.ThongBao, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
					else if (XtraMessageBox.Show(ResourceMessage.XacNhanDanhDauDaKiemTra, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						RevertStoreCheckCell(checkEdit);
					}
					else if (StoreCheckProcess(treatmentADO))
					{
						gridViewTreatment.HideEditor();
						LoadDataToGridControlTreatment();
					}
					else
					{
						RevertStoreCheckCell(checkEdit);
					}
				}
				finally
				{
					isProcessingStoreCheck = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void RevertStoreCheckCell(CheckEdit editor)
		{
			try
			{
				if (editor != null)
				{
					editor.Checked = false;
				}
				gridViewTreatment.HideEditor();
				gridViewTreatment.CancelUpdateCurrentRow();
				gridViewTreatment.RefreshRow(gridViewTreatment.FocusedRowHandle);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool StoreCheckProcess(TreatmentADO row)
		{
			CommonParam commonParam = new CommonParam();
			bool flag = false;
			try
			{
				List<long> treatmentIds = new List<long>();
				treatmentIds.Add(((L_HIS_TREATMENT_3)row).ID);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => treatmentIds), treatmentIds));
				WaitingManager.Show();
				List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Post<List<HIS_TREATMENT>>("api/HisTreatment/CheckStore", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, treatmentIds, commonParam);
				WaitingManager.Hide();
				flag = list != null && list.Count > 0;
				if (flag)
				{
					HIS_TREATMENT hIS_TREATMENT = list.FirstOrDefault((HIS_TREATMENT o) => o.ID == ((L_HIS_TREATMENT_3)row).ID);
					if (hIS_TREATMENT != null)
					{
						((L_HIS_TREATMENT_3)row).STORE_CHECK_TIME = hIS_TREATMENT.STORE_CHECK_TIME;
						((L_HIS_TREATMENT_3)row).STORE_CHECK_LOGINNAME = hIS_TREATMENT.STORE_CHECK_LOGINNAME;
						((L_HIS_TREATMENT_3)row).STORE_CHECK_USERNAME = hIS_TREATMENT.STORE_CHECK_USERNAME;
						row.IsStoreChecked = true;
					}
					gridViewTreatment.RefreshRow(gridViewTreatment.FocusedRowHandle);
				}
				MessageManager.Show(base.ParentForm, commonParam, flag);
				SessionManager.ProcessTokenLost(commonParam);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				flag = false;
				LogSystem.Error("Danh dau ho so da kiem tra that bai." + LogUtil.TraceData(LogUtil.GetMemberName(() => row), row), ex);
			}
			return flag;
		}

		private void cboStoreCheckStatus_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Index == 1)
				{
					cboStoreCheckStatus.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboStoreCheckStatus_EditValueChanged(object sender, EventArgs e)
		{
			if (isInit)
			{
				return;
			}
			try
			{
				string vALUE = ((cboStoreCheckStatus.EditValue == null) ? "" : cboStoreCheckStatus.EditValue.ToString());
				ControlStateRDO controlStateRDO = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == "cboStoreCheckStatus" && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				if (controlStateRDO != null)
				{
					controlStateRDO.VALUE = vALUE;
				}
				else
				{
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					controlStateRDO = new ControlStateRDO();
					controlStateRDO.KEY = "cboStoreCheckStatus";
					controlStateRDO.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					controlStateRDO.VALUE = vALUE;
					currentControlStateRDO.Add(controlStateRDO);
				}
				controlStateWorker.SetData(currentControlStateRDO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void refreshDataTreatment(object data)
		{
			try
			{
				if (data != null)
				{
					btnSearch_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repositoryItemButtonEdit_UnRejectStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				if (treatmentADO != null && MessageBox.Show(ResourceMessage.BanCothucSuMuonHuyTuChoiDuyetBenhAn, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					HisTreatmentRejectStoreSDO hisTreatmentRejectStoreSDO = new HisTreatmentRejectStoreSDO();
					hisTreatmentRejectStoreSDO.TreatmentId = ((L_HIS_TREATMENT_3)treatmentADO).ID;
					bool value = false;
					HIS_TREATMENT hIS_TREATMENT = null;
					hIS_TREATMENT = new BackendAdapter(commonParam).Post<HIS_TREATMENT>("api/HisTreatment/UnrejectStore", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentRejectStoreSDO, commonParam);
					if (hIS_TREATMENT != null)
					{
						value = true;
						btnSearch_Click(null, null);
					}
					MessageManager.Show(base.ParentForm, commonParam, value);
					SessionManager.ProcessTokenLost(commonParam);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewTreatment_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				if (e.RowHandle < 0)
				{
					return;
				}
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetRow(e.RowHandle);
				if (treatmentADO == null)
				{
					return;
				}
				if (e.Column.FieldName == "REJECT_STORE_STT")
				{
					if (((L_HIS_TREATMENT_3)treatmentADO).APPROVAL_STORE_STT_ID == 2)
					{
						e.RepositoryItem = repositoryItemButtonEdit_UnRejectStore;
					}
					else
					{
						e.RepositoryItem = repositoryItemButtonEdit_RejectStore;
					}
				}
				else if (e.Column.FieldName == "CheckTreatment")
				{
					if (((L_HIS_TREATMENT_3)treatmentADO).APPROVAL_STORE_STT_ID == 2)
					{
						e.RepositoryItem = repositoryItemCheckEdit_Store__Disable;
					}
					else
					{
						e.RepositoryItem = repositoryItemCheckEdit3;
					}
				}
				else if (e.Column.FieldName == "IsStoreChecked")
				{
					bool flag = !treatmentADO.IsStoreChecked && ((L_HIS_TREATMENT_3)treatmentADO).APPROVAL_STORE_STT_ID.HasValue && ((L_HIS_TREATMENT_3)treatmentADO).APPROVAL_STORE_STT_ID.Value == 1;
					e.RepositoryItem = (flag ? repositoryItemCheckEdit_StoreCheck__Enable : repositoryItemCheckEdit_StoreCheck__Disable);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkJustShowTreatmentLatch_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (isInit)
				{
					return;
				}
				WaitingManager.Show();
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == "checkJustShowTreatmentLatch" && o.MODULE_LINK == "HIS.Desktop.Plugins.MedicalStoreV2").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (checkJustShowTreatmentLatch.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = "checkJustShowTreatmentLatch";
					csAddOrUpdate.VALUE = (checkJustShowTreatmentLatch.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = "HIS.Desktop.Plugins.MedicalStoreV2";
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(csAddOrUpdate);
				}
				controlStateWorker.SetData(currentControlStateRDO);
				LoadDataToGridControlTreatment();
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboStatusEnd_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (cboStatusEnd.EditValue != null && System.Convert.ToInt16(cboStatusEnd.EditValue) == 2)
				{
					dtOutTimeFrom.EditValue = null;
					dtOutTimeTo.EditValue = null;
					ResetCombo(cboEndDepartment);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnExportExcel_Click(object sender, EventArgs e)
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisMediRecordView2Filter filter = new HisMediRecordView2Filter();
				SetFilterMediRecord2(ref filter);
				List<V_HIS_MEDI_RECORD_2> data = new BackendAdapter(commonParam).Get<List<V_HIS_MEDI_RECORD_2>>("api/HisMediRecord/GetView2", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, filter, commonParam);
				Export(data);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetFilterMediRecord2(ref HisMediRecordView2Filter filter)
		{
			try
			{
				filter.ORDER_DIRECTION = "DESC";
				filter.ORDER_FIELD = "MODIFY_TIME";
				if (!string.IsNullOrEmpty(txtTreatmentCodeMediRecord.Text))
				{
					string text = txtTreatmentCodeMediRecord.Text.Trim();
					if (text.Length < 12 && checkDigit(text))
					{
						text = string.Format("{0:000000000000}", System.Convert.ToInt64(text));
						txtTreatmentCodeMediRecord.Text = text;
					}
					filter.TREATMENT_CODE = text;
				}
				if (!string.IsNullOrEmpty(txtPatientCodeMediRecord.Text))
				{
					string text2 = txtPatientCodeMediRecord.Text.Trim();
					if (text2.Length < 12 && checkDigit(text2))
					{
						text2 = string.Format("{0:0000000000}", System.Convert.ToInt64(text2));
						txtPatientCodeMediRecord.Text = text2;
					}
					filter.PATIENT_CODE__EXACT = text2;
				}
				if (!string.IsNullOrEmpty(txtStoreCodeMediRecord.Text))
				{
					filter.STORE_CODE__EXACT = txtStoreCodeMediRecord.Text.Trim();
				}
				if (!string.IsNullOrEmpty(txtKeywordMediRecord.Text))
				{
					string kEY_WORD = txtKeywordMediRecord.Text.Trim();
					filter.KEY_WORD = kEY_WORD;
				}
				if (dtStoreTime_From.EditValue != null)
				{
					filter.STORE_TIME_FROM = Parse.ToInt64(dtStoreTime_From.DateTime.ToString("yyyyMMdd") + "000000");
				}
				if (dtStoreTime_To.EditValue != null)
				{
					filter.STORE_TIME_TO = Parse.ToInt64(dtStoreTime_To.DateTime.ToString("yyyyMMdd") + "235959");
				}
				if (_ProgramSelects != null && _ProgramSelects.Count > 0)
				{
					filter.PROGRAM_IDs = _ProgramSelects.Select((HIS_PROGRAM o) => o.ID).ToList();
				}
				if (rdoStatus__UnStored.Checked)
				{
					filter.IS_NOT_STORED = true;
				}
				else if (rdoStatus__Stored.Checked)
				{
					filter.IS_NOT_STORED = false;
				}
				else
				{
					filter.IS_NOT_STORED = null;
				}
				if (cboMediRecordType.EditValue != null)
				{
					filter.MEDI_RECORD_TYPE_ID = System.Convert.ToInt64(cboMediRecordType.EditValue);
				}
				List<DataStoreADO> list = new List<DataStoreADO>();
				BindingList<DataStoreADO> bindingList = (BindingList<DataStoreADO>)treeListMedicalStore.DataSource;
				if (bindingList != null && bindingList.Count > 0)
				{
					list = bindingList.ToList();
				}
				List<long> list2 = ((list != null) ? (from o in list
					where o.CheckStore
					select ((V_HIS_DATA_STORE_1)o).ID).ToList() : null);
				if (list2 != null && list2.Count > 0)
				{
					filter.DATA_STORE_IDs = list2;
				}
				else
				{
					filter.DATA_STORE_ID = 0L;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Export(List<V_HIS_MEDI_RECORD_2> data)
		{
			try
			{
				Thread thread = new Thread(new ParameterizedThreadStart(CreateExport));
				thread.Priority = ThreadPriority.Normal;
				thread.IsBackground = true;
				thread.SetApartmentState(ApartmentState.STA);
				try
				{
					thread.Start(data);
				}
				catch (Exception ex)
				{
					LogSystem.Error(ex);
					thread.Abort();
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void CreateExport(object obj)
		{
			try
			{
				List<V_HIS_MEDI_RECORD_2> data = obj as List<V_HIS_MEDI_RECORD_2>;
				List<string> list = new List<string>();
				Store store = new Store(true);
				string text = Path.Combine(Application.StartupPath + "\\Tmp\\Exp", "EXPORT_TUBENHAN.xlsx");
				saveFileDialog1.Filter = "Excel 2007 later file (*.xlsx)|*.xlsx|Excel 97-2003 file(*.xls)|*.xls";
				if (saveFileDialog1.ShowDialog() != DialogResult.OK)
				{
					return;
				}
				WaitingManager.Show();
				if (string.IsNullOrEmpty(text))
				{
					store = null;
					XtraMessageBox.Show(string.Format(ResourceMessage.KhongTimThayFile, text));
					return;
				}
				store.ReadTemplate(Path.GetFullPath(text));
				if (store.TemplatePath == "")
				{
					XtraMessageBox.Show(string.Format(ResourceMessage.BieuMauDangMoHoacKhongTonTaiFile, text));
					return;
				}
				ProcessData(data, ref store);
				WaitingManager.Hide();
				if (store != null)
				{
					try
					{
						if (store.OutFile(saveFileDialog1.FileName))
						{
							XtraMessageBox.Show(ResourceMessage.XuatFileThanhCong);
							if (MessageBox.Show(ResourceMessage.BanCoMuonMoFile, ResourceMessage.ThongBao, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
							{
								Process.Start(saveFileDialog1.FileName);
							}
						}
						return;
					}
					catch (Exception ex)
					{
						LogSystem.Warn(ex);
						return;
					}
				}
				XtraMessageBox.Show(ResourceMessage.XuLyThatBai);
			}
			catch (Exception ex2)
			{
				LogSystem.Error(ex2);
			}
		}

		private void ProcessData(List<V_HIS_MEDI_RECORD_2> data, ref Store store)
		{
			try
			{
				ProcessSingleTag processSingleTag = new ProcessSingleTag();
				ProcessObjectTag processObjectTag = new ProcessObjectTag();
				store.SetCommonFunctions();
				List<MediRecordADO2> list = new List<MediRecordADO2>();
				foreach (V_HIS_MEDI_RECORD_2 datum in data)
				{
					MediRecordADO2 item = new MediRecordADO2(datum);
					list.Add(item);
				}
				processObjectTag.AddObjectData(store, "MedicalStore", list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				store = null;
			}
		}

		private void LoadComboLocationStore()
		{
			try
			{
				lstLocationStore = (from o in BackendDataWorker.Get<HIS_LOCATION_STORE>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_CODE", "Mã", 100, 1));
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_NAME", "Tên", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("LOCATION_STORE_NAME", "ID", list, true, 350);
				ControlEditorLoader.Load(cboLocationStore, lstLocationStore, controlEditorADO);
				cboLocationStore.Properties.ImmediatePopup = true;
				cboLocationStore.Properties.PopupFormSize = new Size(350, cboLocationStore.Properties.PopupFormSize.Height);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLocationStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboLocationStore.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewMediRecord_ShownEditor(object sender, EventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				MediRecordADO mediRecordADO = gridView.GetFocusedRow() as MediRecordADO;
				if (gridView.FocusedColumn.FieldName == "LOCATION_STORE_ID" && gridView.ActiveEditor is GridLookUpEdit && ((V_HIS_MEDI_RECORD_1)mediRecordADO).IS_NOT_STORED == 1)
				{
					GridLookUpEdit gridLookUpEdit = gridView.ActiveEditor as GridLookUpEdit;
					LoadComboLocationGrid(gridLookUpEdit);
					gridLookUpEdit.EditValue = ((V_HIS_MEDI_RECORD_1)mediRecordADO).LOCATION_STORE_ID;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadComboLocationGrid(GridLookUpEdit editor)
		{
			try
			{
				List<Inventec.Common.Controls.EditorLoader.ColumnInfo> list = new List<Inventec.Common.Controls.EditorLoader.ColumnInfo>();
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_CODE", "Mã", 100, 1));
				list.Add(new Inventec.Common.Controls.EditorLoader.ColumnInfo("LOCATION_STORE_NAME", "Tên", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("LOCATION_STORE_NAME", "ID", list, true, 350);
				ControlEditorLoader.Load(editor, lstLocationStore, controlEditorADO);
				editor.Properties.ImmediatePopup = true;
				editor.Properties.PopupFormSize = new Size(350, editor.Properties.PopupFormSize.Height);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repLocationStore_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
					if (mediRecordADO != null)
					{
						((V_HIS_MEDI_RECORD_1)mediRecordADO).LOCATION_STORE_ID = null;
					}
					gridViewMediRecord.GridControl.RefreshDataSource();
					gridViewMediRecord.FocusedRowHandle -= 1;
					gridViewMediRecord.FocusedRowHandle += 1;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repLocationStore_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				GridLookUpEdit gridLookUpEdit = (GridLookUpEdit)sender;
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					((V_HIS_MEDI_RECORD_1)mediRecordADO).LOCATION_STORE_ID = long.Parse(gridLookUpEdit.EditValue.ToString());
				}
				gridViewMediRecord.GridControl.RefreshDataSource();
				gridViewMediRecord.FocusedRowHandle -= 1;
				gridViewMediRecord.FocusedRowHandle += 1;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboExecuteDepartment_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboExecuteDepartment.EditValue = null;
					_ExecuteDepartmentSelects = null;
					ResetCombo(cboExecuteDepartment);
					cboStatusEnd.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemButtonEdit_Edit_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				MediRecordADO mediRecordADO = (MediRecordADO)gridViewMediRecord.GetFocusedRow();
				if (mediRecordADO != null)
				{
					frmStoreInfomation frmStoreInfomation2 = new frmStoreInfomation(((V_HIS_MEDI_RECORD_1)mediRecordADO).ID, new Action(RefreshDataAfterSaveMediRecord));
					frmStoreInfomation2.ShowDialog();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboExecuteDepartment_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
		{
			try
			{
				e.DisplayText = "";
				string text = "";
				if (_ExecuteDepartmentSelects != null && _ExecuteDepartmentSelects.Count > 0)
				{
					foreach (HIS_DEPARTMENT executeDepartmentSelect in _ExecuteDepartmentSelects)
					{
						text = text + executeDepartmentSelect.DEPARTMENT_NAME + ", ";
					}
				}
				else
				{
					cboStatusEnd.Enabled = true;
				}
				e.DisplayText = text;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemButtonEditInventory_Click(object sender, EventArgs e)
		{
			try
			{
				TreatmentADO treatmentADO = (TreatmentADO)gridViewTreatment.GetFocusedRow();
				MRSummaryDetailADO mRSummaryDetailADO = new MRSummaryDetailADO();
				mRSummaryDetailADO.TreatmentId = ((L_HIS_TREATMENT_3)treatmentADO).ID;
				mRSummaryDetailADO.processType = MRSummaryDetailADO.OpenFrom.MedicalStoreV2;
				List<object> list = new List<object>();
				list.Add(mRSummaryDetailADO);
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.MRSummaryList", currentModule.RoomId, currentModule.RoomTypeId, list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewTreatment_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			try
			{
				GridHitInfo hitInfo = e.HitInfo;
				if (hitInfo.InRowCell)
				{
					int visibleRowHandle = gridViewTreatment.GetVisibleRowHandle(hitInfo.RowHandle);
					TreatmentADO currenttreatmentADO = (TreatmentADO)gridViewTreatment.GetRow(visibleRowHandle);
					gridViewTreatment.OptionsSelection.EnableAppearanceFocusedCell = true;
					gridViewTreatment.OptionsSelection.EnableAppearanceFocusedRow = true;
					if (barManager1 == null)
					{
						barManager1 = new BarManager();
						barManager1.Form = this;
					}
					List<TreatmentADO> list = (List<TreatmentADO>)gridControlTreatment.DataSource;
					PopupMenuProcessor popupMenuProcessor = new PopupMenuProcessor(currenttreatmentADO, barManager1, new TreatmentMouseRightClick(Treatment_MouseRightClick), new RefeshReference(BtnRefreshs));
					popupMenuProcessor.InitMenuTreat();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCMedicalStoreV2));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject13 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject14 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject15 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject16 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject17 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject18 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject19 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject20 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject21 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject22 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject23 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject24 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject25 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject26 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject27 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject28 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject29 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject30 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject31 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject32 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject33 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject34 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject35 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject36 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject37 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject38 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject39 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject40 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject41 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject42 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject43 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject44 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject45 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject46 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject47 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject48 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject49 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject50 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject51 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject52 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject53 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject54 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject55 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject56 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject57 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject58 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject59 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject60 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject61 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject62 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject63 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject64 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject65 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject66 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject67 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject68 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject69 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject70 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject71 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject72 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject73 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject74 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject75 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject76 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject77 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject78 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject79 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject80 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject81 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject82 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject83 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject84 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject85 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject86 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject87 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject88 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject89 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject90 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject91 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject92 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject93 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject94 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject95 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject96 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject97 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject98 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject99 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject100 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.btnExportExcel = new DevExpress.XtraEditors.SimpleButton();
            this.cboExecuteDepartment = new DevExpress.XtraEditors.GridLookUpEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.checkJustShowTreatmentLatch = new DevExpress.XtraEditors.CheckEdit();
            this.ucPagingMediRecord = new Inventec.UC.Paging.UcPaging();
            this.ucPagingTreatment = new Inventec.UC.Paging.UcPaging();
            this.btnImportStore = new DevExpress.XtraEditors.SimpleButton();
            this.rdoStatus__Stored = new DevExpress.XtraEditors.CheckEdit();
            this.rdoStatus__UnStored = new DevExpress.XtraEditors.CheckEdit();
            this.rdoStatus__All = new DevExpress.XtraEditors.CheckEdit();
            this.txtPatientCodeMediRecord = new DevExpress.XtraEditors.TextEdit();
            this.cboMediRecordType = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lblDanhSachHoSoChuaLuuTru = new DevExpress.XtraEditors.LabelControl();
            this.lblDanhSachBenhAn = new DevExpress.XtraEditors.LabelControl();
            this.treeListMedicalStore = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit_CheckStore = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridControlTreatmentOfMediRecord = new DevExpress.XtraGrid.GridControl();
            this.gridViewTreatmentOfMediRecord = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ButtonEditDeleteDisable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ButtonEditDeleteEnable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn35 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn44 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn45 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn46 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn47 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn48 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn49 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn36 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn37 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn40 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn41 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.checkAutoShowStore = new DevExpress.XtraEditors.CheckEdit();
            this.btnSearchMediRecord = new DevExpress.XtraEditors.SimpleButton();
            this.dtOutTimeTo = new DevExpress.XtraEditors.DateEdit();
            this.gridControlMediRecord = new DevExpress.XtraGrid.GridControl();
            this.gridViewMediRecord = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CheckEnable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnCheckStore = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit_ViewEmrTreatment = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit_Print_Enable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn67 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit_Edit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn66 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn27 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn52 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repLocationStore = new Inventec.Desktop.CustomControl.RepositoryItemCustomGridLookUpEdit();
            this.repositoryItemCustomGridLookUpEdit1View = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
            this.gridColumn56 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMediInTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnMediOutTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit_Delete_Enable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit_Delete_Disable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit_Print_Disable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.CheckDisable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Btn_TreatmentBorrow_Enable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.Btn_TreatmentBorrow_Disable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ButtonEditReceiveTreatment_Enabed = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ButtonEditReceiveTreatment_Disabed = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ButtonEditImport = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ButtonEditUnImport = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit_Store__Enable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemCheckEdit_Store__Disable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemButtonView_Disable = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repLocationStoreDis = new Inventec.Desktop.CustomControl.CustomGrid.RepositoryItemCustomGridLookUpEdit();
            this.customGridView1 = new Inventec.Desktop.CustomControl.CustomGrid.CustomGridView();
            this.dtInTimeFrom = new DevExpress.XtraEditors.DateEdit();
            this.txtTreatmentCodeMediRecord = new DevExpress.XtraEditors.TextEdit();
            this.gridControlTreatment = new DevExpress.XtraGrid.GridControl();
            this.gridViewTreatment = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn57 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grdColView = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnViewTreatment = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn34 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColStoreCheck = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit_StoreCheck__Enable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn50 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEditInventory = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn58 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn59 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn60 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.gridColumn61 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn63 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn64 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn65 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn68 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn70 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn62 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn53 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn72 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn51 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn73 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn39 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn75 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn55 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn54 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn71 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn38 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColStoreCheckUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColStoreCheckTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemButtonEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit10 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemButtonEdit11 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit12 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit_UnRejectStore = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit_RejectStore = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemCheckEdit_StoreCheck__Disable = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.cboProgram = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView5 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboPatientType = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dtStoreTime_From = new DevExpress.XtraEditors.DateEdit();
            this.btnSaveTreatment = new DevExpress.XtraEditors.SimpleButton();
            this.txtKeywordMediRecord = new DevExpress.XtraEditors.TextEdit();
            this.cboTreatmentType = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit3View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.dtStoreTime_To = new DevExpress.XtraEditors.DateEdit();
            this.cboStatusEnd = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboStoreCheckStatus = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEditStoreCheckView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.txtStoreCodeMediRecord = new DevExpress.XtraEditors.TextEdit();
            this.dtOutTimeFrom = new DevExpress.XtraEditors.DateEdit();
            this.btnSearchTreatment = new DevExpress.XtraEditors.SimpleButton();
            this.txtTreatmentCodeSearch = new DevExpress.XtraEditors.TextEdit();
            this.txtPatientCodeSearch = new DevExpress.XtraEditors.TextEdit();
            this.dtInTimeTo = new DevExpress.XtraEditors.DateEdit();
            this.txtKeywordTreatment = new DevExpress.XtraEditors.TextEdit();
            this.cboEndDepartment = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboLocationStore = new Inventec.Desktop.CustomControl.CustomGridLookUpEditWithFilterMultiColumn();
            this.customGridLookUpEditWithFilterMultiColumn1View = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciAreaNotStore = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem26 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem20 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem18 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem19 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem25 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStatusEnd = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciEndDepartment = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciOutTimeFilter = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciInTimeFilter = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciKeywordTreatment = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTreatmentCodeSearch = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPatientCodeSearch = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciJustShowTreatmentLatch = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStoreCheckStatus = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTreatmentType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPatientType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciAreaStored = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem21 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciDataStoreList = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem28 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStoreTimeFilter = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciTreatmentCodeMediRecord = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciStoreCodeMediRecord = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciProgramFilter = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciFilterMediRecordType = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPatientCodeMedRecord = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem23 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciKeywordMediRecord = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridLookUpEdit1 = new DevExpress.XtraEditors.GridLookUpEdit();
            this.imageCollectionCheck = new DevExpress.Utils.ImageCollection();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.imageCollectionRoom1 = new DevExpress.Utils.ImageCollection();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cboExecuteDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkJustShowTreatmentLatch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__Stored.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__UnStored.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__All.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientCodeMediRecord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMediRecordType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListMedicalStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_CheckStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTreatmentOfMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTreatmentOfMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditDeleteDisable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditDeleteEnable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkAutoShowStore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckEnable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_ViewEmrTreatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Print_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Edit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLocationStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCustomGridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Delete_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Delete_Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Print_Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckDisable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_TreatmentBorrow_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_TreatmentBorrow_Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditReceiveTreatment_Enabed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditReceiveTreatment_Disabed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditUnImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Store__Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Store__Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonView_Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLocationStoreDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentCodeMediRecord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTreatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTreatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnViewTreatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_StoreCheck__Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditInventory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_UnRejectStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_RejectStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_StoreCheck__Disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProgram.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPatientType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_From.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_From.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordMediRecord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTreatmentType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit3View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_To.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_To.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit2View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStoreCheckStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEditStoreCheckView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreCodeMediRecord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentCodeSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientCodeSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordTreatment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndDepartment.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLocationStore.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridLookUpEditWithFilterMultiColumn1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAreaNotStore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStatusEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOutTimeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciInTimeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKeywordTreatment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentCodeSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientCodeSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciJustShowTreatmentLatch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreCheckStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAreaStored)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDataStoreList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem28)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreTimeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentCodeMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreCodeMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProgramFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFilterMediRecordType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientCodeMedRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKeywordMediRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionRoom1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.btnExportExcel);
            this.layoutControl1.Controls.Add(this.cboExecuteDepartment);
            this.layoutControl1.Controls.Add(this.checkJustShowTreatmentLatch);
            this.layoutControl1.Controls.Add(this.ucPagingMediRecord);
            this.layoutControl1.Controls.Add(this.ucPagingTreatment);
            this.layoutControl1.Controls.Add(this.btnImportStore);
            this.layoutControl1.Controls.Add(this.rdoStatus__Stored);
            this.layoutControl1.Controls.Add(this.rdoStatus__UnStored);
            this.layoutControl1.Controls.Add(this.rdoStatus__All);
            this.layoutControl1.Controls.Add(this.txtPatientCodeMediRecord);
            this.layoutControl1.Controls.Add(this.cboMediRecordType);
            this.layoutControl1.Controls.Add(this.lblDanhSachHoSoChuaLuuTru);
            this.layoutControl1.Controls.Add(this.lblDanhSachBenhAn);
            this.layoutControl1.Controls.Add(this.treeListMedicalStore);
            this.layoutControl1.Controls.Add(this.gridControlTreatmentOfMediRecord);
            this.layoutControl1.Controls.Add(this.checkAutoShowStore);
            this.layoutControl1.Controls.Add(this.btnSearchMediRecord);
            this.layoutControl1.Controls.Add(this.dtOutTimeTo);
            this.layoutControl1.Controls.Add(this.gridControlMediRecord);
            this.layoutControl1.Controls.Add(this.dtInTimeFrom);
            this.layoutControl1.Controls.Add(this.txtTreatmentCodeMediRecord);
            this.layoutControl1.Controls.Add(this.gridControlTreatment);
            this.layoutControl1.Controls.Add(this.cboProgram);
            this.layoutControl1.Controls.Add(this.cboPatientType);
            this.layoutControl1.Controls.Add(this.dtStoreTime_From);
            this.layoutControl1.Controls.Add(this.btnSaveTreatment);
            this.layoutControl1.Controls.Add(this.txtKeywordMediRecord);
            this.layoutControl1.Controls.Add(this.cboTreatmentType);
            this.layoutControl1.Controls.Add(this.dtStoreTime_To);
            this.layoutControl1.Controls.Add(this.cboStatusEnd);
            this.layoutControl1.Controls.Add(this.cboStoreCheckStatus);
            this.layoutControl1.Controls.Add(this.txtStoreCodeMediRecord);
            this.layoutControl1.Controls.Add(this.dtOutTimeFrom);
            this.layoutControl1.Controls.Add(this.btnSearchTreatment);
            this.layoutControl1.Controls.Add(this.txtTreatmentCodeSearch);
            this.layoutControl1.Controls.Add(this.txtPatientCodeSearch);
            this.layoutControl1.Controls.Add(this.dtInTimeTo);
            this.layoutControl1.Controls.Add(this.txtKeywordTreatment);
            this.layoutControl1.Controls.Add(this.cboEndDepartment);
            this.layoutControl1.Controls.Add(this.cboLocationStore);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 29);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(46, 179, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1360, 739);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(1260, 97);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(76, 22);
            this.btnExportExcel.StyleController = this.layoutControl1;
            this.btnExportExcel.TabIndex = 47;
            this.btnExportExcel.Text = "Xuất excel";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // cboExecuteDepartment
            // 
            this.cboExecuteDepartment.Location = new System.Drawing.Point(119, 49);
            this.cboExecuteDepartment.MenuManager = this.barManager1;
            this.cboExecuteDepartment.Name = "cboExecuteDepartment";
            this.cboExecuteDepartment.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboExecuteDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboExecuteDepartment.Properties.NullText = "";
            this.cboExecuteDepartment.Properties.View = this.gridView4;
            this.cboExecuteDepartment.Size = new System.Drawing.Size(189, 20);
            this.cboExecuteDepartment.StyleController = this.layoutControl1;
            this.cboExecuteDepartment.TabIndex = 46;
            this.cboExecuteDepartment.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboExecuteDepartment_ButtonClick);
            this.cboExecuteDepartment.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboExecuteDepartment_CustomDisplayText);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection1;
            this.barManager1.MaxItemId = 0;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1360, 29);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 768);
            this.barDockControlBottom.Size = new System.Drawing.Size(1360, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 739);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1360, 29);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 739);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.InsertGalleryImage("gradientlightbluedatabar_16x16.png", "images/data%20bars/gradientlightbluedatabar_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/data%20bars/gradientlightbluedatabar_16x16.png"), 0);
            this.imageCollection1.Images.SetKeyName(0, "gradientlightbluedatabar_16x16.png");
            // 
            // gridView4
            // 
            this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // checkJustShowTreatmentLatch
            // 
            this.checkJustShowTreatmentLatch.Location = new System.Drawing.Point(307, 715);
            this.checkJustShowTreatmentLatch.MenuManager = this.barManager1;
            this.checkJustShowTreatmentLatch.Name = "checkJustShowTreatmentLatch";
            this.checkJustShowTreatmentLatch.Properties.Caption = "Chỉ hiển thị hồ sơ đã chốt";
            this.checkJustShowTreatmentLatch.Size = new System.Drawing.Size(262, 19);
            this.checkJustShowTreatmentLatch.StyleController = this.layoutControl1;
            this.checkJustShowTreatmentLatch.TabIndex = 45;
            this.checkJustShowTreatmentLatch.ToolTip = "Chỉ hiển thị các hồ sơ đã được khoa phòng chốt duyệt bệnh án";
            this.checkJustShowTreatmentLatch.CheckedChanged += new System.EventHandler(this.checkJustShowTreatmentLatch_CheckedChanged);
            // 
            // ucPagingMediRecord
            // 
            this.ucPagingMediRecord.Location = new System.Drawing.Point(784, 534);
            this.ucPagingMediRecord.Name = "ucPagingMediRecord";
            this.ucPagingMediRecord.Size = new System.Drawing.Size(552, 30);
            this.ucPagingMediRecord.TabIndex = 44;
            // 
            // ucPagingTreatment
            // 
            this.ucPagingTreatment.Location = new System.Drawing.Point(24, 691);
            this.ucPagingTreatment.Name = "ucPagingTreatment";
            this.ucPagingTreatment.Size = new System.Drawing.Size(545, 20);
            this.ucPagingTreatment.TabIndex = 43;
            // 
            // btnImportStore
            // 
            this.btnImportStore.Location = new System.Drawing.Point(1076, 97);
            this.btnImportStore.Name = "btnImportStore";
            this.btnImportStore.Size = new System.Drawing.Size(89, 22);
            this.btnImportStore.StyleController = this.layoutControl1;
            this.btnImportStore.TabIndex = 42;
            this.btnImportStore.Text = "Nhập kho (Ctrl I)";
            this.btnImportStore.Click += new System.EventHandler(this.btnImportStore_Click);
            // 
            // rdoStatus__Stored
            // 
            this.rdoStatus__Stored.Location = new System.Drawing.Point(1299, 25);
            this.rdoStatus__Stored.MenuManager = this.barManager1;
            this.rdoStatus__Stored.Name = "rdoStatus__Stored";
            this.rdoStatus__Stored.Properties.Caption = "";
            this.rdoStatus__Stored.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.rdoStatus__Stored.Properties.FullFocusRect = true;
            this.rdoStatus__Stored.Size = new System.Drawing.Size(37, 19);
            this.rdoStatus__Stored.StyleController = this.layoutControl1;
            this.rdoStatus__Stored.TabIndex = 41;
            this.rdoStatus__Stored.CheckedChanged += new System.EventHandler(this.rdoStatus__UnStored_CheckedChanged);
            // 
            // rdoStatus__UnStored
            // 
            this.rdoStatus__UnStored.Location = new System.Drawing.Point(1217, 25);
            this.rdoStatus__UnStored.MenuManager = this.barManager1;
            this.rdoStatus__UnStored.Name = "rdoStatus__UnStored";
            this.rdoStatus__UnStored.Properties.Caption = "";
            this.rdoStatus__UnStored.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.rdoStatus__UnStored.Properties.FullFocusRect = true;
            this.rdoStatus__UnStored.Size = new System.Drawing.Size(23, 19);
            this.rdoStatus__UnStored.StyleController = this.layoutControl1;
            this.rdoStatus__UnStored.TabIndex = 40;
            this.rdoStatus__UnStored.CheckedChanged += new System.EventHandler(this.rdoStatus__Stored_CheckedChanged);
            // 
            // rdoStatus__All
            // 
            this.rdoStatus__All.Location = new System.Drawing.Point(1119, 25);
            this.rdoStatus__All.MenuManager = this.barManager1;
            this.rdoStatus__All.Name = "rdoStatus__All";
            this.rdoStatus__All.Properties.Caption = "";
            this.rdoStatus__All.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            this.rdoStatus__All.Properties.FullFocusRect = true;
            this.rdoStatus__All.Size = new System.Drawing.Size(29, 19);
            this.rdoStatus__All.StyleController = this.layoutControl1;
            this.rdoStatus__All.TabIndex = 39;
            this.rdoStatus__All.CheckedChanged += new System.EventHandler(this.rdoStatus__All_CheckedChanged);
            // 
            // txtPatientCodeMediRecord
            // 
            this.txtPatientCodeMediRecord.Location = new System.Drawing.Point(877, 73);
            this.txtPatientCodeMediRecord.MenuManager = this.barManager1;
            this.txtPatientCodeMediRecord.Name = "txtPatientCodeMediRecord";
            this.txtPatientCodeMediRecord.Properties.NullValuePrompt = "Mã bệnh nhân";
            this.txtPatientCodeMediRecord.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtPatientCodeMediRecord.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtPatientCodeMediRecord.Size = new System.Drawing.Size(83, 20);
            this.txtPatientCodeMediRecord.StyleController = this.layoutControl1;
            this.txtPatientCodeMediRecord.TabIndex = 19;
            this.txtPatientCodeMediRecord.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPatientCodeMediRecord_KeyUp);
            // 
            // cboMediRecordType
            // 
            this.cboMediRecordType.Location = new System.Drawing.Point(1144, 49);
            this.cboMediRecordType.MenuManager = this.barManager1;
            this.cboMediRecordType.Name = "cboMediRecordType";
            this.cboMediRecordType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboMediRecordType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboMediRecordType.Properties.NullText = "";
            this.cboMediRecordType.Properties.View = this.gridView3;
            this.cboMediRecordType.Size = new System.Drawing.Size(192, 20);
            this.cboMediRecordType.StyleController = this.layoutControl1;
            this.cboMediRecordType.TabIndex = 29;
            this.cboMediRecordType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboMediRecordType_ButtonClick);
            // 
            // gridView3
            // 
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // lblDanhSachHoSoChuaLuuTru
            // 
            this.lblDanhSachHoSoChuaLuuTru.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblDanhSachHoSoChuaLuuTru.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDanhSachHoSoChuaLuuTru.Location = new System.Drawing.Point(24, 4);
            this.lblDanhSachHoSoChuaLuuTru.Name = "lblDanhSachHoSoChuaLuuTru";
            this.lblDanhSachHoSoChuaLuuTru.Size = new System.Drawing.Size(545, 17);
            this.lblDanhSachHoSoChuaLuuTru.StyleController = this.layoutControl1;
            this.lblDanhSachHoSoChuaLuuTru.TabIndex = 30;
            this.lblDanhSachHoSoChuaLuuTru.Text = "Danh sách đợt điều trị chưa tạo bệnh án";
            // 
            // lblDanhSachBenhAn
            // 
            this.lblDanhSachBenhAn.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblDanhSachBenhAn.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDanhSachBenhAn.Location = new System.Drawing.Point(579, 4);
            this.lblDanhSachBenhAn.Name = "lblDanhSachBenhAn";
            this.lblDanhSachBenhAn.Size = new System.Drawing.Size(757, 17);
            this.lblDanhSachBenhAn.StyleController = this.layoutControl1;
            this.lblDanhSachBenhAn.TabIndex = 31;
            this.lblDanhSachBenhAn.Text = "Danh sách bệnh án";
            // 
            // treeListMedicalStore
            // 
            this.treeListMedicalStore.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2});
            this.treeListMedicalStore.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeListMedicalStore.Location = new System.Drawing.Point(599, 25);
            this.treeListMedicalStore.Name = "treeListMedicalStore";
            this.treeListMedicalStore.OptionsBehavior.EnableFiltering = true;
            this.treeListMedicalStore.OptionsView.AutoWidth = false;
            this.treeListMedicalStore.OptionsView.ShowIndicator = false;
            this.treeListMedicalStore.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit_CheckStore});
            this.treeListMedicalStore.Size = new System.Drawing.Size(180, 708);
            this.treeListMedicalStore.TabIndex = 4;
            this.treeListMedicalStore.CustomDrawColumnHeader += new DevExpress.XtraTreeList.CustomDrawColumnHeaderEventHandler(this.treeListMedicalStore_CustomDrawColumnHeader);
            this.treeListMedicalStore.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseUp);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = " ";
            this.treeListColumn1.ColumnEdit = this.repositoryItemCheckEdit_CheckStore;
            this.treeListColumn1.FieldName = "CheckStore";
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowSort = false;
            this.treeListColumn1.OptionsFilter.AllowAutoFilter = false;
            this.treeListColumn1.OptionsFilter.AllowFilter = false;
            this.treeListColumn1.ToolTip = "Chọn tất cả";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 30;
            // 
            // repositoryItemCheckEdit_CheckStore
            // 
            this.repositoryItemCheckEdit_CheckStore.AutoHeight = false;
            this.repositoryItemCheckEdit_CheckStore.Name = "repositoryItemCheckEdit_CheckStore";
            this.repositoryItemCheckEdit_CheckStore.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit_CheckStore.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit_CheckStore_CheckedChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "Kho lưu trữ";
            this.treeListColumn2.FieldName = "DataStoreNameWithCountTreatment";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.ToolTip = "Kho lưu trữ";
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 1;
            this.treeListColumn2.Width = 400;
            // 
            // gridControlTreatmentOfMediRecord
            // 
            this.gridControlTreatmentOfMediRecord.Location = new System.Drawing.Point(782, 566);
            this.gridControlTreatmentOfMediRecord.MainView = this.gridViewTreatmentOfMediRecord;
            this.gridControlTreatmentOfMediRecord.Name = "gridControlTreatmentOfMediRecord";
            this.gridControlTreatmentOfMediRecord.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ButtonEditDeleteEnable,
            this.ButtonEditDeleteDisable,
            this.repositoryItemCheckEdit1});
            this.gridControlTreatmentOfMediRecord.Size = new System.Drawing.Size(556, 170);
            this.gridControlTreatmentOfMediRecord.TabIndex = 27;
            this.gridControlTreatmentOfMediRecord.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTreatmentOfMediRecord});
            // 
            // gridViewTreatmentOfMediRecord
            // 
            this.gridViewTreatmentOfMediRecord.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn29,
            this.gridColumn30,
            this.gridColumn31,
            this.gridColumn32,
            this.gridColumn35,
            this.gridColumn44,
            this.gridColumn45,
            this.gridColumn46,
            this.gridColumn47,
            this.gridColumn23,
            this.gridColumn48,
            this.gridColumn49,
            this.gridColumn36,
            this.gridColumn37,
            this.gridColumn40,
            this.gridColumn41,
            this.gridColumn42,
            this.gridColumn43});
            this.gridViewTreatmentOfMediRecord.GridControl = this.gridControlTreatmentOfMediRecord;
            this.gridViewTreatmentOfMediRecord.Name = "gridViewTreatmentOfMediRecord";
            this.gridViewTreatmentOfMediRecord.OptionsCustomization.AllowFilter = false;
            this.gridViewTreatmentOfMediRecord.OptionsCustomization.AllowSort = false;
            this.gridViewTreatmentOfMediRecord.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gridViewTreatmentOfMediRecord.OptionsView.ColumnAutoWidth = false;
            this.gridViewTreatmentOfMediRecord.OptionsView.ShowGroupPanel = false;
            this.gridViewTreatmentOfMediRecord.OptionsView.ShowIndicator = false;
            this.gridViewTreatmentOfMediRecord.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridViewTreatmentOfMediRecord_RowCellStyle);
            this.gridViewTreatmentOfMediRecord.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewTreatmentOfMediRecord_CustomRowCellEdit);
            this.gridViewTreatmentOfMediRecord.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewTreatmentOfMediRecord_CustomUnboundColumnData);
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = " ";
            this.gridColumn29.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn29.FieldName = "CheckTreatment";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.Width = 30;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // gridColumn30
            // 
            this.gridColumn30.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn30.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn30.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn30.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn30.Caption = "STT";
            this.gridColumn30.FieldName = "STT";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 0;
            this.gridColumn30.Width = 41;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "In";
            this.gridColumn31.ColumnEdit = this.ButtonEditDeleteDisable;
            this.gridColumn31.FieldName = "Print";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.OptionsColumn.ShowCaption = false;
            this.gridColumn31.Width = 28;
            // 
            // ButtonEditDeleteDisable
            // 
            this.ButtonEditDeleteDisable.AutoHeight = false;
            this.ButtonEditDeleteDisable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditDeleteDisable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "Xóa", null, null, true)});
            this.ButtonEditDeleteDisable.Name = "ButtonEditDeleteDisable";
            this.ButtonEditDeleteDisable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Xóa";
            this.gridColumn32.ColumnEdit = this.ButtonEditDeleteEnable;
            this.gridColumn32.FieldName = "Delete";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.OptionsColumn.ShowCaption = false;
            this.gridColumn32.Visible = true;
            this.gridColumn32.VisibleIndex = 1;
            this.gridColumn32.Width = 30;
            // 
            // ButtonEditDeleteEnable
            // 
            this.ButtonEditDeleteEnable.AutoHeight = false;
            this.ButtonEditDeleteEnable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditDeleteEnable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "Xóa", null, null, true)});
            this.ButtonEditDeleteEnable.Name = "ButtonEditDeleteEnable";
            this.ButtonEditDeleteEnable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.ButtonEditDeleteEnable.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonEditDeleteEnable_ButtonClick);
            // 
            // gridColumn35
            // 
            this.gridColumn35.Caption = "Mã điều trị";
            this.gridColumn35.FieldName = "TREATMENT_CODE";
            this.gridColumn35.Name = "gridColumn35";
            this.gridColumn35.OptionsColumn.AllowEdit = false;
            this.gridColumn35.ToolTip = "Mã HSDT";
            this.gridColumn35.Visible = true;
            this.gridColumn35.VisibleIndex = 2;
            this.gridColumn35.Width = 100;
            // 
            // gridColumn44
            // 
            this.gridColumn44.Caption = "Ngày vào viện";
            this.gridColumn44.FieldName = "IN_TIME_STR";
            this.gridColumn44.Name = "gridColumn44";
            this.gridColumn44.OptionsColumn.AllowEdit = false;
            this.gridColumn44.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn44.Visible = true;
            this.gridColumn44.VisibleIndex = 3;
            this.gridColumn44.Width = 111;
            // 
            // gridColumn45
            // 
            this.gridColumn45.Caption = "Ngày ra viện";
            this.gridColumn45.FieldName = "OUT_TIME_STR";
            this.gridColumn45.Name = "gridColumn45";
            this.gridColumn45.OptionsColumn.AllowEdit = false;
            this.gridColumn45.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn45.Visible = true;
            this.gridColumn45.VisibleIndex = 4;
            this.gridColumn45.Width = 114;
            // 
            // gridColumn46
            // 
            this.gridColumn46.Caption = "Loại ra viện";
            this.gridColumn46.FieldName = "TREATMENT_END_TYPE_NAME";
            this.gridColumn46.Name = "gridColumn46";
            this.gridColumn46.OptionsColumn.AllowEdit = false;
            this.gridColumn46.Visible = true;
            this.gridColumn46.VisibleIndex = 5;
            this.gridColumn46.Width = 99;
            // 
            // gridColumn47
            // 
            this.gridColumn47.Caption = "Khoa kết thúc điều trị";
            this.gridColumn47.FieldName = "DEPARTMENT_NAME";
            this.gridColumn47.Name = "gridColumn47";
            this.gridColumn47.OptionsColumn.AllowEdit = false;
            this.gridColumn47.Visible = true;
            this.gridColumn47.VisibleIndex = 6;
            this.gridColumn47.Width = 135;
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "Phòng kết thúc điều trị";
            this.gridColumn23.FieldName = "ROOM_NAME";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.OptionsColumn.AllowEdit = false;
            this.gridColumn23.Visible = true;
            this.gridColumn23.VisibleIndex = 7;
            this.gridColumn23.Width = 120;
            // 
            // gridColumn48
            // 
            this.gridColumn48.Caption = "Mã bệnh chính";
            this.gridColumn48.FieldName = "ICD_CODE";
            this.gridColumn48.Name = "gridColumn48";
            this.gridColumn48.OptionsColumn.AllowEdit = false;
            this.gridColumn48.Visible = true;
            this.gridColumn48.VisibleIndex = 8;
            this.gridColumn48.Width = 80;
            // 
            // gridColumn49
            // 
            this.gridColumn49.Caption = "Tên bệnh chính";
            this.gridColumn49.FieldName = "ICD_NAME";
            this.gridColumn49.Name = "gridColumn49";
            this.gridColumn49.OptionsColumn.AllowEdit = false;
            this.gridColumn49.Visible = true;
            this.gridColumn49.VisibleIndex = 9;
            this.gridColumn49.Width = 151;
            // 
            // gridColumn36
            // 
            this.gridColumn36.Caption = "Mã bệnh nhân";
            this.gridColumn36.FieldName = "TDL_PATIENT_CODE";
            this.gridColumn36.Name = "gridColumn36";
            this.gridColumn36.OptionsColumn.AllowEdit = false;
            this.gridColumn36.ToolTip = "Mã bệnh nhân";
            this.gridColumn36.Visible = true;
            this.gridColumn36.VisibleIndex = 10;
            this.gridColumn36.Width = 99;
            // 
            // gridColumn37
            // 
            this.gridColumn37.Caption = "Tên bệnh nhân";
            this.gridColumn37.FieldName = "TDL_PATIENT_NAME";
            this.gridColumn37.Name = "gridColumn37";
            this.gridColumn37.OptionsColumn.AllowEdit = false;
            this.gridColumn37.ToolTip = "Tên bệnh nhân";
            this.gridColumn37.Visible = true;
            this.gridColumn37.VisibleIndex = 11;
            this.gridColumn37.Width = 151;
            // 
            // gridColumn40
            // 
            this.gridColumn40.Caption = "Ngày sinh";
            this.gridColumn40.FieldName = "DOB_STR";
            this.gridColumn40.Name = "gridColumn40";
            this.gridColumn40.OptionsColumn.AllowEdit = false;
            this.gridColumn40.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn40.Visible = true;
            this.gridColumn40.VisibleIndex = 12;
            this.gridColumn40.Width = 84;
            // 
            // gridColumn41
            // 
            this.gridColumn41.Caption = "Tuổi";
            this.gridColumn41.FieldName = "AGE";
            this.gridColumn41.Name = "gridColumn41";
            this.gridColumn41.OptionsColumn.AllowEdit = false;
            this.gridColumn41.ToolTip = "Tuổi";
            this.gridColumn41.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn41.Visible = true;
            this.gridColumn41.VisibleIndex = 13;
            this.gridColumn41.Width = 52;
            // 
            // gridColumn42
            // 
            this.gridColumn42.Caption = "Giới tính";
            this.gridColumn42.FieldName = "GENDER_NAME";
            this.gridColumn42.Name = "gridColumn42";
            this.gridColumn42.OptionsColumn.AllowEdit = false;
            this.gridColumn42.ToolTip = "Giới tính";
            this.gridColumn42.Visible = true;
            this.gridColumn42.VisibleIndex = 14;
            this.gridColumn42.Width = 72;
            // 
            // gridColumn43
            // 
            this.gridColumn43.Caption = "Địa chỉ";
            this.gridColumn43.FieldName = "TDL_PATIENT_ADDRESS";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowEdit = false;
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 15;
            this.gridColumn43.Width = 188;
            // 
            // checkAutoShowStore
            // 
            this.checkAutoShowStore.Location = new System.Drawing.Point(24, 715);
            this.checkAutoShowStore.MenuManager = this.barManager1;
            this.checkAutoShowStore.Name = "checkAutoShowStore";
            this.checkAutoShowStore.Properties.Caption = "Tự động hiển thị tủ lưu trữ khi tích chọn hồ sơ";
            this.checkAutoShowStore.Size = new System.Drawing.Size(279, 19);
            this.checkAutoShowStore.StyleController = this.layoutControl1;
            this.checkAutoShowStore.TabIndex = 38;
            this.checkAutoShowStore.CheckedChanged += new System.EventHandler(this.checkAutoShowStore_CheckedChanged);
            // 
            // btnSearchMediRecord
            // 
            this.btnSearchMediRecord.Location = new System.Drawing.Point(1169, 97);
            this.btnSearchMediRecord.Name = "btnSearchMediRecord";
            this.btnSearchMediRecord.Size = new System.Drawing.Size(87, 22);
            this.btnSearchMediRecord.StyleController = this.layoutControl1;
            this.btnSearchMediRecord.TabIndex = 33;
            this.btnSearchMediRecord.Text = "Tìm (Ctrl Shirt F)";
            this.btnSearchMediRecord.Click += new System.EventHandler(this.btnSearchMediRecord_Click);
            // 
            // dtOutTimeTo
            // 
            this.dtOutTimeTo.EditValue = null;
            this.dtOutTimeTo.Location = new System.Drawing.Point(221, 97);
            this.dtOutTimeTo.Name = "dtOutTimeTo";
            this.dtOutTimeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtOutTimeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtOutTimeTo.Size = new System.Drawing.Size(87, 20);
            this.dtOutTimeTo.StyleController = this.layoutControl1;
            this.dtOutTimeTo.TabIndex = 20;
            // 
            // gridControlMediRecord
            // 
            this.gridControlMediRecord.Location = new System.Drawing.Point(782, 121);
            this.gridControlMediRecord.MainView = this.gridViewMediRecord;
            this.gridControlMediRecord.Name = "gridControlMediRecord";
            this.gridControlMediRecord.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit_Delete_Enable,
            this.repositoryItemButtonEdit_Delete_Disable,
            this.repositoryItemButtonEdit_Print_Enable,
            this.repositoryItemButtonEdit_Print_Disable,
            this.CheckEnable,
            this.CheckDisable,
            this.Btn_TreatmentBorrow_Enable,
            this.Btn_TreatmentBorrow_Disable,
            this.ButtonEditReceiveTreatment_Enabed,
            this.ButtonEditReceiveTreatment_Disabed,
            this.ButtonEditImport,
            this.ButtonEditUnImport,
            this.repositoryItemButtonEdit_ViewEmrTreatment,
            this.repositoryItemCheckEdit_Store__Enable,
            this.repositoryItemCheckEdit_Store__Disable,
            this.repositoryItemButtonView_Disable,
            this.repLocationStore,
            this.repLocationStoreDis,
            this.repositoryItemButtonEdit_Edit});
            this.gridControlMediRecord.Size = new System.Drawing.Size(556, 411);
            this.gridControlMediRecord.TabIndex = 5;
            this.gridControlMediRecord.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMediRecord});
            // 
            // gridViewMediRecord
            // 
            this.gridViewMediRecord.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumnCheckStore,
            this.gridColumn17,
            this.gridColumn1,
            this.gridColumn16,
            this.gridColumn2,
            this.gridColumn7,
            this.gridColumn67,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn28,
            this.gridColumn66,
            this.gridColumn26,
            this.gridColumn8,
            this.gridColumn20,
            this.gridColumn21,
            this.gridColumn22,
            this.gridColumn24,
            this.gridColumn27,
            this.gridColumn9,
            this.gridColumn52,
            this.gridColumn56,
            this.gridColumnMediInTime,
            this.gridColumnMediOutTime,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.gridViewMediRecord.GridControl = this.gridControlMediRecord;
            this.gridViewMediRecord.Name = "gridViewMediRecord";
            this.gridViewMediRecord.OptionsCustomization.AllowFilter = false;
            this.gridViewMediRecord.OptionsCustomization.AllowSort = false;
            this.gridViewMediRecord.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gridViewMediRecord.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMediRecord.OptionsView.ColumnAutoWidth = false;
            this.gridViewMediRecord.OptionsView.ShowGroupPanel = false;
            this.gridViewMediRecord.OptionsView.ShowIndicator = false;
            this.gridViewMediRecord.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridViewMediRecord_RowCellClick);
            this.gridViewMediRecord.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridViewTreatment_RowCellStyle);
            this.gridViewMediRecord.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewMediRecord_CustomRowCellEdit);
            this.gridViewMediRecord.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridViewMediRecord_PopupMenuShowing);
            this.gridViewMediRecord.ShownEditor += new System.EventHandler(this.gridViewMediRecord_ShownEditor);
            this.gridViewMediRecord.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewTreatment_CustomUnboundColumnData);
            this.gridViewMediRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewMediRecord_MouseDown);
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = " ";
            this.gridColumn14.ColumnEdit = this.CheckEnable;
            this.gridColumn14.FieldName = "CheckTreatment";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Width = 30;
            // 
            // CheckEnable
            // 
            this.CheckEnable.AutoHeight = false;
            this.CheckEnable.Name = "CheckEnable";
            this.CheckEnable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn15.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "STT";
            this.gridColumn15.FieldName = "STT";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 0;
            this.gridColumn15.Width = 35;
            // 
            // gridColumnCheckStore
            // 
            this.gridColumnCheckStore.Caption = "gridColumn33";
            this.gridColumnCheckStore.FieldName = "CheckStore";
            this.gridColumnCheckStore.Name = "gridColumnCheckStore";
            this.gridColumnCheckStore.OptionsColumn.ShowCaption = false;
            this.gridColumnCheckStore.Visible = true;
            this.gridColumnCheckStore.VisibleIndex = 1;
            this.gridColumnCheckStore.Width = 25;
            // 
            // gridColumn17
            // 
            this.gridColumn17.Caption = "Chi tiết";
            this.gridColumn17.ColumnEdit = this.repositoryItemButtonEdit_ViewEmrTreatment;
            this.gridColumn17.FieldName = "ViewEmr";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.ShowCaption = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 2;
            this.gridColumn17.Width = 25;
            // 
            // repositoryItemButtonEdit_ViewEmrTreatment
            // 
            this.repositoryItemButtonEdit_ViewEmrTreatment.AutoHeight = false;
            this.repositoryItemButtonEdit_ViewEmrTreatment.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_ViewEmrTreatment.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "Văn bản ký điện tử trong hồ sơ bệnh án", null, null, true)});
            this.repositoryItemButtonEdit_ViewEmrTreatment.Name = "repositoryItemButtonEdit_ViewEmrTreatment";
            this.repositoryItemButtonEdit_ViewEmrTreatment.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_ViewEmrTreatment.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_ViewEmrTreatment_ButtonClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "In";
            this.gridColumn1.ColumnEdit = this.repositoryItemButtonEdit_Print_Enable;
            this.gridColumn1.FieldName = "Print";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.ShowCaption = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 25;
            // 
            // repositoryItemButtonEdit_Print_Enable
            // 
            this.repositoryItemButtonEdit_Print_Enable.AutoHeight = false;
            this.repositoryItemButtonEdit_Print_Enable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_Print_Enable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject13, serializableAppearanceObject14, serializableAppearanceObject15, serializableAppearanceObject16, "In", null, null, true)});
            this.repositoryItemButtonEdit_Print_Enable.Name = "repositoryItemButtonEdit_Print_Enable";
            this.repositoryItemButtonEdit_Print_Enable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_Print_Enable.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_Print_Enable_ButtonClick);
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "Mượn HSBA";
            this.gridColumn16.FieldName = "TreatmentBorrow";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.ShowCaption = false;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 4;
            this.gridColumn16.Width = 25;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Trả bệnh án";
            this.gridColumn2.FieldName = "ReceiveTreatment";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.ShowCaption = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 5;
            this.gridColumn2.Width = 25;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Nhập kho/ hủy nhập kho";
            this.gridColumn7.FieldName = "IMPORT_OR_UNIMPORT";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.ShowCaption = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 25;
            // 
            // gridColumn67
            // 
            this.gridColumn67.Caption = "Sửa";
            this.gridColumn67.ColumnEdit = this.repositoryItemButtonEdit_Edit;
            this.gridColumn67.FieldName = "Edit";
            this.gridColumn67.Name = "gridColumn67";
            this.gridColumn67.OptionsColumn.ShowCaption = false;
            this.gridColumn67.Visible = true;
            this.gridColumn67.VisibleIndex = 7;
            this.gridColumn67.Width = 25;
            // 
            // repositoryItemButtonEdit_Edit
            // 
            this.repositoryItemButtonEdit_Edit.AutoHeight = false;
            this.repositoryItemButtonEdit_Edit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_Edit.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject17, serializableAppearanceObject18, serializableAppearanceObject19, serializableAppearanceObject20, "Sửa", null, null, true)});
            this.repositoryItemButtonEdit_Edit.Name = "repositoryItemButtonEdit_Edit";
            this.repositoryItemButtonEdit_Edit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_Edit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_Edit_ButtonClick);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Mã lưu trữ";
            this.gridColumn3.FieldName = "STORE_CODE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.ToolTip = "Mã lưu trữ";
            this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 8;
            this.gridColumn3.Width = 80;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Mã bệnh nhân";
            this.gridColumn5.FieldName = "PATIENT_CODE";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.ToolTip = "Mã bệnh nhân";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 9;
            this.gridColumn5.Width = 80;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Mã điều trị";
            this.gridColumn4.FieldName = "TDL_TREATMENT_CODES";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.ToolTip = "Mã điều trị";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 10;
            this.gridColumn4.Width = 100;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Tên bệnh nhân";
            this.gridColumn6.FieldName = "VIR_PATIENT_NAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.ToolTip = "Tên bệnh nhân";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 11;
            this.gridColumn6.Width = 130;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "Tên chương trình";
            this.gridColumn28.FieldName = "PROGRAM_NAME";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.AllowEdit = false;
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 12;
            this.gridColumn28.Width = 123;
            // 
            // gridColumn66
            // 
            this.gridColumn66.Caption = "Số vào viện";
            this.gridColumn66.FieldName = "TDL_TREATMENT_IN_CODES_STR";
            this.gridColumn66.Name = "gridColumn66";
            this.gridColumn66.OptionsColumn.AllowEdit = false;
            this.gridColumn66.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn66.Visible = true;
            this.gridColumn66.VisibleIndex = 13;
            this.gridColumn66.Width = 150;
            // 
            // gridColumn26
            // 
            this.gridColumn26.Caption = "Ngày sinh";
            this.gridColumn26.FieldName = "DOB_STR";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.OptionsColumn.AllowEdit = false;
            this.gridColumn26.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 14;
            this.gridColumn26.Width = 120;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Giới tính";
            this.gridColumn8.FieldName = "GENDER_NAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.ToolTip = "Giới tính";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 15;
            this.gridColumn8.Width = 72;
            // 
            // gridColumn20
            // 
            this.gridColumn20.Caption = "Khoa kết thúc điều trị";
            this.gridColumn20.FieldName = "END_DEPARTMENT_NAME";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.OptionsColumn.AllowEdit = false;
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 16;
            this.gridColumn20.Width = 130;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "Phòng kết thúc điều trị";
            this.gridColumn21.FieldName = "END_ROOM_NAME";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.OptionsColumn.AllowEdit = false;
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 17;
            this.gridColumn21.Width = 120;
            // 
            // gridColumn22
            // 
            this.gridColumn22.Caption = "Chẩn đoán";
            this.gridColumn22.FieldName = "ICD_NAME";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.OptionsColumn.AllowEdit = false;
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 18;
            this.gridColumn22.Width = 130;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "Địa chỉ";
            this.gridColumn24.FieldName = "VIR_ADDRESS";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.AllowEdit = false;
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 19;
            this.gridColumn24.Width = 188;
            // 
            // gridColumn27
            // 
            this.gridColumn27.Caption = "Mã chương trình";
            this.gridColumn27.FieldName = "PROGRAM_CODE";
            this.gridColumn27.Name = "gridColumn27";
            this.gridColumn27.OptionsColumn.AllowEdit = false;
            this.gridColumn27.ToolTip = "Mã được cấp cho bệnh nhân trong chương trình";
            this.gridColumn27.Visible = true;
            this.gridColumn27.VisibleIndex = 20;
            this.gridColumn27.Width = 100;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Loại bệnh án";
            this.gridColumn9.FieldName = "MEDI_RECORD_TYPE_NAME";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.ToolTip = "Kho dữ liệu";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 21;
            this.gridColumn9.Width = 120;
            // 
            // gridColumn52
            // 
            this.gridColumn52.Caption = "Vị trí";
            this.gridColumn52.ColumnEdit = this.repLocationStore;
            this.gridColumn52.FieldName = "LOCATION_STORE_ID";
            this.gridColumn52.Name = "gridColumn52";
            this.gridColumn52.Visible = true;
            this.gridColumn52.VisibleIndex = 22;
            this.gridColumn52.Width = 100;
            // 
            // repLocationStore
            // 
            this.repLocationStore.AutoComplete = false;
            this.repLocationStore.AutoHeight = false;
            this.repLocationStore.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.repLocationStore.Name = "repLocationStore";
            this.repLocationStore.NullText = "";
            this.repLocationStore.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repLocationStore.View = this.repositoryItemCustomGridLookUpEdit1View;
            this.repLocationStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repLocationStore_ButtonClick);
            this.repLocationStore.EditValueChanged += new System.EventHandler(this.repLocationStore_EditValueChanged);
            // 
            // repositoryItemCustomGridLookUpEdit1View
            // 
            this.repositoryItemCustomGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.repositoryItemCustomGridLookUpEdit1View.Name = "repositoryItemCustomGridLookUpEdit1View";
            this.repositoryItemCustomGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.repositoryItemCustomGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn56
            // 
            this.gridColumn56.Caption = "Số lưu trữ bảng kê";
            this.gridColumn56.FieldName = "TDL_STORE_BORDEREAU_CODES";
            this.gridColumn56.Name = "gridColumn56";
            this.gridColumn56.OptionsColumn.AllowEdit = false;
            this.gridColumn56.Visible = true;
            this.gridColumn56.VisibleIndex = 23;
            this.gridColumn56.Width = 120;
            // 
            // gridColumnMediInTime
            // 
            this.gridColumnMediInTime.Caption = "Ngày vào viện";
            this.gridColumnMediInTime.FieldName = "INTIME_SPLCONCAT";
            this.gridColumnMediInTime.Name = "gridColumnMediInTime";
            this.gridColumnMediInTime.OptionsColumn.AllowEdit = false;
            this.gridColumnMediInTime.ToolTip = "Ngày vào viện theo đợt điều trị của bệnh án";
            this.gridColumnMediInTime.Visible = true;
            this.gridColumnMediInTime.VisibleIndex = 24;
            this.gridColumnMediInTime.Width = 130;
            // 
            // gridColumnMediOutTime
            // 
            this.gridColumnMediOutTime.Caption = "Ngày ra viện";
            this.gridColumnMediOutTime.FieldName = "OUTTIME_SPLCONCAT";
            this.gridColumnMediOutTime.Name = "gridColumnMediOutTime";
            this.gridColumnMediOutTime.OptionsColumn.AllowEdit = false;
            this.gridColumnMediOutTime.ToolTip = "Ngày ra viện theo đợt điều trị của bệnh án (trống nếu chưa ra viện)";
            this.gridColumnMediOutTime.Visible = true;
            this.gridColumnMediOutTime.VisibleIndex = 25;
            this.gridColumnMediOutTime.Width = 130;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Thời gian tạo";
            this.gridColumn10.FieldName = "CREATE_TIME_STR";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.ToolTip = "Thời gian tạo";
            this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 26;
            this.gridColumn10.Width = 123;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Người tạo";
            this.gridColumn11.FieldName = "CREATOR";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.ToolTip = "Người tạo";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 27;
            this.gridColumn11.Width = 91;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Thời gian sửa";
            this.gridColumn12.FieldName = "MODIFY_TIME_STR";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.ToolTip = "Thời gian sửa";
            this.gridColumn12.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 28;
            this.gridColumn12.Width = 108;
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "Người sửa";
            this.gridColumn13.FieldName = "MODIFIER";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.ToolTip = "Người sửa";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 29;
            this.gridColumn13.Width = 127;
            // 
            // repositoryItemButtonEdit_Delete_Enable
            // 
            this.repositoryItemButtonEdit_Delete_Enable.AutoHeight = false;
            this.repositoryItemButtonEdit_Delete_Enable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_Delete_Enable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject21, serializableAppearanceObject22, serializableAppearanceObject23, serializableAppearanceObject24, "Xóa", null, null, true)});
            this.repositoryItemButtonEdit_Delete_Enable.Name = "repositoryItemButtonEdit_Delete_Enable";
            this.repositoryItemButtonEdit_Delete_Enable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_Delete_Enable.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_Delete_Enable_ButtonClick);
            // 
            // repositoryItemButtonEdit_Delete_Disable
            // 
            this.repositoryItemButtonEdit_Delete_Disable.AutoHeight = false;
            this.repositoryItemButtonEdit_Delete_Disable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_Delete_Disable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject25, serializableAppearanceObject26, serializableAppearanceObject27, serializableAppearanceObject28, "Xóa", null, null, true)});
            this.repositoryItemButtonEdit_Delete_Disable.Name = "repositoryItemButtonEdit_Delete_Disable";
            this.repositoryItemButtonEdit_Delete_Disable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemButtonEdit_Print_Disable
            // 
            this.repositoryItemButtonEdit_Print_Disable.AutoHeight = false;
            this.repositoryItemButtonEdit_Print_Disable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_Print_Disable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject29, serializableAppearanceObject30, serializableAppearanceObject31, serializableAppearanceObject32, "In", null, null, true)});
            this.repositoryItemButtonEdit_Print_Disable.Name = "repositoryItemButtonEdit_Print_Disable";
            this.repositoryItemButtonEdit_Print_Disable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // CheckDisable
            // 
            this.CheckDisable.AutoHeight = false;
            this.CheckDisable.Name = "CheckDisable";
            this.CheckDisable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.CheckDisable.ReadOnly = true;
            // 
            // Btn_TreatmentBorrow_Enable
            // 
            this.Btn_TreatmentBorrow_Enable.AutoHeight = false;
            this.Btn_TreatmentBorrow_Enable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Btn_TreatmentBorrow_Enable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject33, serializableAppearanceObject34, serializableAppearanceObject35, serializableAppearanceObject36, "Mượn bệnh án", null, null, true)});
            this.Btn_TreatmentBorrow_Enable.Name = "Btn_TreatmentBorrow_Enable";
            this.Btn_TreatmentBorrow_Enable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.Btn_TreatmentBorrow_Enable.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Btn_TreatmentBorrow_Enable_ButtonClick);
            // 
            // Btn_TreatmentBorrow_Disable
            // 
            this.Btn_TreatmentBorrow_Disable.AutoHeight = false;
            this.Btn_TreatmentBorrow_Disable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Btn_TreatmentBorrow_Disable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject37, serializableAppearanceObject38, serializableAppearanceObject39, serializableAppearanceObject40, "Mượn bệnh án", null, null, true)});
            this.Btn_TreatmentBorrow_Disable.Name = "Btn_TreatmentBorrow_Disable";
            this.Btn_TreatmentBorrow_Disable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // ButtonEditReceiveTreatment_Enabed
            // 
            this.ButtonEditReceiveTreatment_Enabed.AutoHeight = false;
            this.ButtonEditReceiveTreatment_Enabed.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditReceiveTreatment_Enabed.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject41, serializableAppearanceObject42, serializableAppearanceObject43, serializableAppearanceObject44, "Trả bệnh án", null, null, true)});
            this.ButtonEditReceiveTreatment_Enabed.Name = "ButtonEditReceiveTreatment_Enabed";
            this.ButtonEditReceiveTreatment_Enabed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.ButtonEditReceiveTreatment_Enabed.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonEditReceiveTreatment_Enabed_ButtonClick);
            // 
            // ButtonEditReceiveTreatment_Disabed
            // 
            this.ButtonEditReceiveTreatment_Disabed.AutoHeight = false;
            this.ButtonEditReceiveTreatment_Disabed.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditReceiveTreatment_Disabed.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject45, serializableAppearanceObject46, serializableAppearanceObject47, serializableAppearanceObject48, "Trả bệnh án", null, null, true)});
            this.ButtonEditReceiveTreatment_Disabed.Name = "ButtonEditReceiveTreatment_Disabed";
            this.ButtonEditReceiveTreatment_Disabed.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // ButtonEditImport
            // 
            this.ButtonEditImport.AutoHeight = false;
            this.ButtonEditImport.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditImport.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject49, serializableAppearanceObject50, serializableAppearanceObject51, serializableAppearanceObject52, "Nhập kho", null, null, true)});
            this.ButtonEditImport.Name = "ButtonEditImport";
            this.ButtonEditImport.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.ButtonEditImport.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonEditImport_ButtonClick);
            // 
            // ButtonEditUnImport
            // 
            this.ButtonEditUnImport.AutoHeight = false;
            this.ButtonEditUnImport.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ButtonEditUnImport.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject53, serializableAppearanceObject54, serializableAppearanceObject55, serializableAppearanceObject56, "Hủy nhập kho", null, null, true)});
            this.ButtonEditUnImport.Name = "ButtonEditUnImport";
            this.ButtonEditUnImport.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.ButtonEditUnImport.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonEditUnImport_ButtonClick);
            // 
            // repositoryItemCheckEdit_Store__Enable
            // 
            this.repositoryItemCheckEdit_Store__Enable.AutoHeight = false;
            this.repositoryItemCheckEdit_Store__Enable.Name = "repositoryItemCheckEdit_Store__Enable";
            this.repositoryItemCheckEdit_Store__Enable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // repositoryItemCheckEdit_Store__Disable
            // 
            this.repositoryItemCheckEdit_Store__Disable.AutoHeight = false;
            this.repositoryItemCheckEdit_Store__Disable.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Style2;
            this.repositoryItemCheckEdit_Store__Disable.Name = "repositoryItemCheckEdit_Store__Disable";
            this.repositoryItemCheckEdit_Store__Disable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit_Store__Disable.ReadOnly = true;
            this.repositoryItemCheckEdit_Store__Disable.ValueChecked = false;
            // 
            // repositoryItemButtonView_Disable
            // 
            this.repositoryItemButtonView_Disable.AutoHeight = false;
            this.repositoryItemButtonView_Disable.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonView_Disable.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject57, serializableAppearanceObject58, serializableAppearanceObject59, serializableAppearanceObject60, "Văn bản ký điện tử trong hồ sơ bệnh án", null, null, true)});
            this.repositoryItemButtonView_Disable.Name = "repositoryItemButtonView_Disable";
            this.repositoryItemButtonView_Disable.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repLocationStoreDis
            // 
            this.repLocationStoreDis.AutoComplete = false;
            this.repLocationStoreDis.AutoHeight = false;
            this.repLocationStoreDis.Name = "repLocationStoreDis";
            this.repLocationStoreDis.NullText = "";
            this.repLocationStoreDis.ReadOnly = true;
            this.repLocationStoreDis.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repLocationStoreDis.View = this.customGridView1;
            // 
            // customGridView1
            // 
            this.customGridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.customGridView1.Name = "customGridView1";
            this.customGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.customGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // dtInTimeFrom
            // 
            this.dtInTimeFrom.EditValue = null;
            this.dtInTimeFrom.Location = new System.Drawing.Point(407, 97);
            this.dtInTimeFrom.Name = "dtInTimeFrom";
            this.dtInTimeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtInTimeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtInTimeFrom.Size = new System.Drawing.Size(81, 20);
            this.dtInTimeFrom.StyleController = this.layoutControl1;
            this.dtInTimeFrom.TabIndex = 21;
            // 
            // txtTreatmentCodeMediRecord
            // 
            this.txtTreatmentCodeMediRecord.Location = new System.Drawing.Point(964, 73);
            this.txtTreatmentCodeMediRecord.Name = "txtTreatmentCodeMediRecord";
            this.txtTreatmentCodeMediRecord.Properties.NullValuePrompt = "Mã điều trị";
            this.txtTreatmentCodeMediRecord.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtTreatmentCodeMediRecord.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtTreatmentCodeMediRecord.Size = new System.Drawing.Size(81, 20);
            this.txtTreatmentCodeMediRecord.StyleController = this.layoutControl1;
            this.txtTreatmentCodeMediRecord.TabIndex = 20;
            this.txtTreatmentCodeMediRecord.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtTreatmentCodeMediRecord_PreviewKeyDown);
            // 
            // gridControlTreatment
            // 
            this.gridControlTreatment.Location = new System.Drawing.Point(24, 171);
            this.gridControlTreatment.MainView = this.gridViewTreatment;
            this.gridControlTreatment.Name = "gridControlTreatment";
            this.gridControlTreatment.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit8,
            this.repositoryItemButtonEdit9,
            this.repositoryItemButtonEdit7,
            this.repositoryItemButtonEdit10,
            this.repositoryItemCheckEdit3,
            this.repositoryItemCheckEdit4,
            this.repositoryItemButtonEdit11,
            this.repositoryItemButtonEdit12,
            this.btnViewTreatment,
            this.repositoryItemButtonEdit_UnRejectStore,
            this.repositoryItemButtonEdit_RejectStore,
            this.repositoryItemButtonEditInventory,
            this.repositoryItemCheckEdit_StoreCheck__Enable,
            this.repositoryItemCheckEdit_StoreCheck__Disable});
            this.gridControlTreatment.Size = new System.Drawing.Size(545, 516);
            this.gridControlTreatment.TabIndex = 28;
            this.gridControlTreatment.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTreatment});
            // 
            // gridViewTreatment
            // 
            this.gridViewTreatment.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn57,
            this.grdColView,
            this.gridColumn34,
            this.gridColStoreCheck,
            this.gridColumn50,
            this.gridColumn58,
            this.gridColumn59,
            this.gridColumn60,
            this.gridColumn61,
            this.gridColumn63,
            this.gridColumn64,
            this.gridColumn65,
            this.gridColumn68,
            this.gridColumn70,
            this.gridColumn62,
            this.gridColumn53,
            this.gridColumn72,
            this.gridColumn51,
            this.gridColumn73,
            this.gridColumn39,
            this.gridColumn75,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn25,
            this.gridColumn55,
            this.gridColumn54,
            this.gridColumn71,
            this.gridColumn38,
            this.gridColStoreCheckUser,
            this.gridColStoreCheckTime});
            this.gridViewTreatment.GridControl = this.gridControlTreatment;
            this.gridViewTreatment.Name = "gridViewTreatment";
            this.gridViewTreatment.OptionsCustomization.AllowFilter = false;
            this.gridViewTreatment.OptionsCustomization.AllowSort = false;
            this.gridViewTreatment.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gridViewTreatment.OptionsView.ColumnAutoWidth = false;
            this.gridViewTreatment.OptionsView.ShowGroupPanel = false;
            this.gridViewTreatment.OptionsView.ShowIndicator = false;
            this.gridViewTreatment.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridViewTreatment_RowCellStyle_1);
            this.gridViewTreatment.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gridViewTreatment_CustomRowCellEdit);
            this.gridViewTreatment.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridViewTreatment_PopupMenuShowing);
            this.gridViewTreatment.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.gridViewTreatment_CustomUnboundColumnData_1);
            // 
            // gridColumn57
            // 
            this.gridColumn57.Caption = " ";
            this.gridColumn57.ColumnEdit = this.repositoryItemCheckEdit3;
            this.gridColumn57.FieldName = "CheckTreatment";
            this.gridColumn57.Name = "gridColumn57";
            this.gridColumn57.Visible = true;
            this.gridColumn57.VisibleIndex = 0;
            this.gridColumn57.Width = 25;
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            this.repositoryItemCheckEdit3.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit3.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit3_CheckedChanged);
            // 
            // grdColView
            // 
            this.grdColView.Caption = "Xem";
            this.grdColView.ColumnEdit = this.btnViewTreatment;
            this.grdColView.FieldName = "TreatmentView";
            this.grdColView.Name = "grdColView";
            this.grdColView.OptionsColumn.ShowCaption = false;
            this.grdColView.Visible = true;
            this.grdColView.VisibleIndex = 1;
            this.grdColView.Width = 25;
            // 
            // btnViewTreatment
            // 
            this.btnViewTreatment.AutoHeight = false;
            this.btnViewTreatment.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("btnViewTreatment.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject61, serializableAppearanceObject62, serializableAppearanceObject63, serializableAppearanceObject64, "Xem", null, null, true)});
            this.btnViewTreatment.Name = "btnViewTreatment";
            this.btnViewTreatment.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.btnViewTreatment.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnViewTreatment_ButtonClick);
            // 
            // gridColumn34
            // 
            this.gridColumn34.Caption = "Từ chối/ Hủy từ chối duyệt bệnh án";
            this.gridColumn34.FieldName = "REJECT_STORE_STT";
            this.gridColumn34.Name = "gridColumn34";
            this.gridColumn34.OptionsColumn.ShowCaption = false;
            this.gridColumn34.ToolTip = "Từ chối/ Hủy từ chối duyệt bệnh án";
            this.gridColumn34.Visible = true;
            this.gridColumn34.VisibleIndex = 2;
            this.gridColumn34.Width = 25;
            // 
            // gridColStoreCheck
            // 
            this.gridColStoreCheck.Caption = "Đã kiểm tra";
            this.gridColStoreCheck.ColumnEdit = this.repositoryItemCheckEdit_StoreCheck__Enable;
            this.gridColStoreCheck.FieldName = "IsStoreChecked";
            this.gridColStoreCheck.Name = "gridColStoreCheck";
            this.gridColStoreCheck.ToolTip = "Đánh dấu hồ sơ đã được kiểm tra đủ giấy tờ và chữ ký";
            this.gridColStoreCheck.Width = 80;
            // 
            // repositoryItemCheckEdit_StoreCheck__Enable
            // 
            this.repositoryItemCheckEdit_StoreCheck__Enable.AutoHeight = false;
            this.repositoryItemCheckEdit_StoreCheck__Enable.Name = "repositoryItemCheckEdit_StoreCheck__Enable";
            this.repositoryItemCheckEdit_StoreCheck__Enable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit_StoreCheck__Enable.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit_StoreCheck_CheckedChanged);
            // 
            // gridColumn50
            // 
            this.gridColumn50.Caption = "Bảng kiểm thực hiện quy chế hồ sơ bệnh án";
            this.gridColumn50.ColumnEdit = this.repositoryItemButtonEditInventory;
            this.gridColumn50.FieldName = "BANG_KIEM";
            this.gridColumn50.Name = "gridColumn50";
            this.gridColumn50.OptionsColumn.ShowCaption = false;
            this.gridColumn50.Visible = true;
            this.gridColumn50.VisibleIndex = 3;
            this.gridColumn50.Width = 25;
            // 
            // repositoryItemButtonEditInventory
            // 
            this.repositoryItemButtonEditInventory.AutoHeight = false;
            this.repositoryItemButtonEditInventory.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEditInventory.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject65, serializableAppearanceObject66, serializableAppearanceObject67, serializableAppearanceObject68, "Bảng kiểm thực hiện quy chế hồ sơ bệnh án", null, null, true)});
            this.repositoryItemButtonEditInventory.Name = "repositoryItemButtonEditInventory";
            this.repositoryItemButtonEditInventory.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEditInventory.Click += new System.EventHandler(this.repositoryItemButtonEditInventory_Click);
            // 
            // gridColumn58
            // 
            this.gridColumn58.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn58.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn58.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn58.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn58.Caption = "STT";
            this.gridColumn58.FieldName = "STT";
            this.gridColumn58.Name = "gridColumn58";
            this.gridColumn58.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn58.Visible = true;
            this.gridColumn58.VisibleIndex = 4;
            this.gridColumn58.Width = 35;
            // 
            // gridColumn59
            // 
            this.gridColumn59.Caption = "In";
            this.gridColumn59.ColumnEdit = this.repositoryItemButtonEdit7;
            this.gridColumn59.FieldName = "Print";
            this.gridColumn59.Name = "gridColumn59";
            this.gridColumn59.OptionsColumn.ShowCaption = false;
            this.gridColumn59.Width = 28;
            // 
            // repositoryItemButtonEdit7
            // 
            this.repositoryItemButtonEdit7.AutoHeight = false;
            this.repositoryItemButtonEdit7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit7.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject69, serializableAppearanceObject70, serializableAppearanceObject71, serializableAppearanceObject72, "In", null, null, true)});
            this.repositoryItemButtonEdit7.Name = "repositoryItemButtonEdit7";
            this.repositoryItemButtonEdit7.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // gridColumn60
            // 
            this.gridColumn60.Caption = "Xóa";
            this.gridColumn60.ColumnEdit = this.repositoryItemButtonEdit8;
            this.gridColumn60.FieldName = "Delete";
            this.gridColumn60.Name = "gridColumn60";
            this.gridColumn60.OptionsColumn.ShowCaption = false;
            this.gridColumn60.Width = 30;
            // 
            // repositoryItemButtonEdit8
            // 
            this.repositoryItemButtonEdit8.AutoHeight = false;
            this.repositoryItemButtonEdit8.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit8.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject73, serializableAppearanceObject74, serializableAppearanceObject75, serializableAppearanceObject76, "Xóa", null, null, true)});
            this.repositoryItemButtonEdit8.Name = "repositoryItemButtonEdit8";
            this.repositoryItemButtonEdit8.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // gridColumn61
            // 
            this.gridColumn61.Caption = "Mượn HSBA";
            this.gridColumn61.FieldName = "TreatmentBorrow";
            this.gridColumn61.Name = "gridColumn61";
            this.gridColumn61.OptionsColumn.ShowCaption = false;
            this.gridColumn61.Width = 28;
            // 
            // gridColumn63
            // 
            this.gridColumn63.Caption = "Mã điều trị";
            this.gridColumn63.FieldName = "TREATMENT_CODE";
            this.gridColumn63.Name = "gridColumn63";
            this.gridColumn63.OptionsColumn.AllowEdit = false;
            this.gridColumn63.ToolTip = "Mã HSDT";
            this.gridColumn63.Visible = true;
            this.gridColumn63.VisibleIndex = 5;
            this.gridColumn63.Width = 95;
            // 
            // gridColumn64
            // 
            this.gridColumn64.Caption = "Mã bệnh nhân";
            this.gridColumn64.FieldName = "TDL_PATIENT_CODE";
            this.gridColumn64.Name = "gridColumn64";
            this.gridColumn64.OptionsColumn.AllowEdit = false;
            this.gridColumn64.ToolTip = "Mã bệnh nhân";
            this.gridColumn64.Visible = true;
            this.gridColumn64.VisibleIndex = 6;
            this.gridColumn64.Width = 85;
            // 
            // gridColumn65
            // 
            this.gridColumn65.Caption = "Tên bệnh nhân";
            this.gridColumn65.FieldName = "TDL_PATIENT_NAME";
            this.gridColumn65.Name = "gridColumn65";
            this.gridColumn65.OptionsColumn.AllowEdit = false;
            this.gridColumn65.ToolTip = "Tên bệnh nhân";
            this.gridColumn65.Visible = true;
            this.gridColumn65.VisibleIndex = 7;
            this.gridColumn65.Width = 140;
            // 
            // gridColumn68
            // 
            this.gridColumn68.Caption = "Ngày sinh";
            this.gridColumn68.FieldName = "DOB_STR";
            this.gridColumn68.Name = "gridColumn68";
            this.gridColumn68.OptionsColumn.AllowEdit = false;
            this.gridColumn68.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn68.Visible = true;
            this.gridColumn68.VisibleIndex = 8;
            this.gridColumn68.Width = 95;
            // 
            // gridColumn70
            // 
            this.gridColumn70.Caption = "Giới tính";
            this.gridColumn70.FieldName = "GENDER_NAME";
            this.gridColumn70.Name = "gridColumn70";
            this.gridColumn70.OptionsColumn.AllowEdit = false;
            this.gridColumn70.ToolTip = "Giới tính";
            this.gridColumn70.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn70.Visible = true;
            this.gridColumn70.VisibleIndex = 9;
            this.gridColumn70.Width = 50;
            // 
            // gridColumn62
            // 
            this.gridColumn62.Caption = "Số vào viện";
            this.gridColumn62.FieldName = "IN_CODE_STR";
            this.gridColumn62.Name = "gridColumn62";
            this.gridColumn62.OptionsColumn.AllowEdit = false;
            this.gridColumn62.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn62.Visible = true;
            this.gridColumn62.VisibleIndex = 10;
            this.gridColumn62.Width = 150;
            // 
            // gridColumn53
            // 
            this.gridColumn53.Caption = "Thời gian duyệt HSBA";
            this.gridColumn53.FieldName = "APPROVAL_TIME_STR";
            this.gridColumn53.Name = "gridColumn53";
            this.gridColumn53.OptionsColumn.AllowEdit = false;
            this.gridColumn53.ToolTip = "Thời gian duyệt hồ sơ bệnh án";
            this.gridColumn53.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn53.Visible = true;
            this.gridColumn53.VisibleIndex = 11;
            // 
            // gridColumn72
            // 
            this.gridColumn72.Caption = "Ngày vào viện";
            this.gridColumn72.FieldName = "IN_TIME_STR";
            this.gridColumn72.Name = "gridColumn72";
            this.gridColumn72.OptionsColumn.AllowEdit = false;
            this.gridColumn72.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn72.Visible = true;
            this.gridColumn72.VisibleIndex = 12;
            this.gridColumn72.Width = 125;
            // 
            // gridColumn51
            // 
            this.gridColumn51.Caption = "Ngày nhập viện";
            this.gridColumn51.FieldName = "CLINICAL_IN_TIME_STR";
            this.gridColumn51.Name = "gridColumn51";
            this.gridColumn51.OptionsColumn.AllowEdit = false;
            this.gridColumn51.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn51.Visible = true;
            this.gridColumn51.VisibleIndex = 13;
            this.gridColumn51.Width = 125;
            // 
            // gridColumn73
            // 
            this.gridColumn73.Caption = "Ngày ra viện";
            this.gridColumn73.FieldName = "OUT_TIME_STR";
            this.gridColumn73.Name = "gridColumn73";
            this.gridColumn73.OptionsColumn.AllowEdit = false;
            this.gridColumn73.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn73.Visible = true;
            this.gridColumn73.VisibleIndex = 14;
            this.gridColumn73.Width = 125;
            // 
            // gridColumn39
            // 
            this.gridColumn39.Caption = "Khoa điều trị";
            this.gridColumn39.FieldName = "LAST_DEPARTMENT_NAME";
            this.gridColumn39.Name = "gridColumn39";
            this.gridColumn39.OptionsColumn.AllowEdit = false;
            this.gridColumn39.Visible = true;
            this.gridColumn39.VisibleIndex = 15;
            this.gridColumn39.Width = 135;
            // 
            // gridColumn75
            // 
            this.gridColumn75.Caption = "Khoa kết thúc điều trị";
            this.gridColumn75.FieldName = "DEPARTMENT_NAME";
            this.gridColumn75.Name = "gridColumn75";
            this.gridColumn75.OptionsColumn.AllowEdit = false;
            this.gridColumn75.Visible = true;
            this.gridColumn75.VisibleIndex = 16;
            this.gridColumn75.Width = 135;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Phòng kết thúc điều trị";
            this.gridColumn18.FieldName = "ROOM_NAME";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 17;
            this.gridColumn18.Width = 130;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "Chẩn đoán";
            this.gridColumn19.FieldName = "ICD_NAME";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.AllowEdit = false;
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 18;
            this.gridColumn19.Width = 130;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "Loại ra viện";
            this.gridColumn25.FieldName = "TREATMENT_END_TYPE_NAME";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsColumn.AllowEdit = false;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 19;
            this.gridColumn25.Width = 80;
            // 
            // gridColumn55
            // 
            this.gridColumn55.Caption = "CĐ vào viện";
            this.gridColumn55.FieldName = "IN_ICD_NAME";
            this.gridColumn55.Name = "gridColumn55";
            this.gridColumn55.OptionsColumn.AllowEdit = false;
            this.gridColumn55.ToolTip = "Chẩn đoán vào viện";
            this.gridColumn55.Visible = true;
            this.gridColumn55.VisibleIndex = 20;
            this.gridColumn55.Width = 120;
            // 
            // gridColumn54
            // 
            this.gridColumn54.Caption = "Số ra viện";
            this.gridColumn54.FieldName = "END_CODE";
            this.gridColumn54.Name = "gridColumn54";
            this.gridColumn54.OptionsColumn.AllowEdit = false;
            this.gridColumn54.Visible = true;
            this.gridColumn54.VisibleIndex = 22;
            // 
            // gridColumn71
            // 
            this.gridColumn71.Caption = "Địa chỉ";
            this.gridColumn71.FieldName = "TDL_PATIENT_ADDRESS";
            this.gridColumn71.Name = "gridColumn71";
            this.gridColumn71.OptionsColumn.AllowEdit = false;
            this.gridColumn71.Visible = true;
            this.gridColumn71.VisibleIndex = 21;
            this.gridColumn71.Width = 200;
            // 
            // gridColumn38
            // 
            this.gridColumn38.Caption = "Lý do từ chối";
            this.gridColumn38.FieldName = "REJECT_STORE_REASON_STR";
            this.gridColumn38.Name = "gridColumn38";
            this.gridColumn38.OptionsColumn.AllowEdit = false;
            this.gridColumn38.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColumn38.Visible = true;
            this.gridColumn38.VisibleIndex = 23;
            this.gridColumn38.Width = 120;
            // 
            // gridColStoreCheckUser
            // 
            this.gridColStoreCheckUser.Caption = "Người kiểm tra";
            this.gridColStoreCheckUser.FieldName = "STORE_CHECK_USERNAME";
            this.gridColStoreCheckUser.Name = "gridColStoreCheckUser";
            this.gridColStoreCheckUser.OptionsColumn.AllowEdit = false;
            this.gridColStoreCheckUser.Width = 120;
            // 
            // gridColStoreCheckTime
            // 
            this.gridColStoreCheckTime.Caption = "Thời gian kiểm tra";
            this.gridColStoreCheckTime.FieldName = "STORE_CHECK_TIME_STR";
            this.gridColStoreCheckTime.Name = "gridColStoreCheckTime";
            this.gridColStoreCheckTime.OptionsColumn.AllowEdit = false;
            this.gridColStoreCheckTime.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.gridColStoreCheckTime.Width = 130;
            // 
            // repositoryItemButtonEdit9
            // 
            this.repositoryItemButtonEdit9.AutoHeight = false;
            this.repositoryItemButtonEdit9.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit9.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject77, serializableAppearanceObject78, serializableAppearanceObject79, serializableAppearanceObject80, "Xóa", null, null, true)});
            this.repositoryItemButtonEdit9.Name = "repositoryItemButtonEdit9";
            this.repositoryItemButtonEdit9.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemButtonEdit10
            // 
            this.repositoryItemButtonEdit10.AutoHeight = false;
            this.repositoryItemButtonEdit10.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit10.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject81, serializableAppearanceObject82, serializableAppearanceObject83, serializableAppearanceObject84, "In", null, null, true)});
            this.repositoryItemButtonEdit10.Name = "repositoryItemButtonEdit10";
            this.repositoryItemButtonEdit10.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemCheckEdit4
            // 
            this.repositoryItemCheckEdit4.AutoHeight = false;
            this.repositoryItemCheckEdit4.Name = "repositoryItemCheckEdit4";
            this.repositoryItemCheckEdit4.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit4.ReadOnly = true;
            // 
            // repositoryItemButtonEdit11
            // 
            this.repositoryItemButtonEdit11.AutoHeight = false;
            this.repositoryItemButtonEdit11.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit11.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject85, serializableAppearanceObject86, serializableAppearanceObject87, serializableAppearanceObject88, "Mượn hồ sơ bệnh án", null, null, true)});
            this.repositoryItemButtonEdit11.Name = "repositoryItemButtonEdit11";
            this.repositoryItemButtonEdit11.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemButtonEdit12
            // 
            this.repositoryItemButtonEdit12.AutoHeight = false;
            this.repositoryItemButtonEdit12.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit12.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject89, serializableAppearanceObject90, serializableAppearanceObject91, serializableAppearanceObject92, "Mượn hồ sơ bệnh án", null, null, true)});
            this.repositoryItemButtonEdit12.Name = "repositoryItemButtonEdit12";
            this.repositoryItemButtonEdit12.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // repositoryItemButtonEdit_UnRejectStore
            // 
            this.repositoryItemButtonEdit_UnRejectStore.AutoHeight = false;
            this.repositoryItemButtonEdit_UnRejectStore.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_UnRejectStore.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject93, serializableAppearanceObject94, serializableAppearanceObject95, serializableAppearanceObject96, "Hủy từ chối duyệt", null, null, true)});
            this.repositoryItemButtonEdit_UnRejectStore.Name = "repositoryItemButtonEdit_UnRejectStore";
            this.repositoryItemButtonEdit_UnRejectStore.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_UnRejectStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_UnRejectStore_ButtonClick);
            // 
            // repositoryItemButtonEdit_RejectStore
            // 
            this.repositoryItemButtonEdit_RejectStore.AutoHeight = false;
            this.repositoryItemButtonEdit_RejectStore.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit_RejectStore.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject97, serializableAppearanceObject98, serializableAppearanceObject99, serializableAppearanceObject100, "Từ chối duyệt", null, null, true)});
            this.repositoryItemButtonEdit_RejectStore.Name = "repositoryItemButtonEdit_RejectStore";
            this.repositoryItemButtonEdit_RejectStore.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.repositoryItemButtonEdit_RejectStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repositoryItemButtonEdit_RejectStore_ButtonClick);
            // 
            // repositoryItemCheckEdit_StoreCheck__Disable
            // 
            this.repositoryItemCheckEdit_StoreCheck__Disable.AutoHeight = false;
            this.repositoryItemCheckEdit_StoreCheck__Disable.Name = "repositoryItemCheckEdit_StoreCheck__Disable";
            this.repositoryItemCheckEdit_StoreCheck__Disable.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit_StoreCheck__Disable.ReadOnly = true;
            // 
            // cboProgram
            // 
            this.cboProgram.EnterMoveNextControl = true;
            this.cboProgram.Location = new System.Drawing.Point(879, 49);
            this.cboProgram.Name = "cboProgram";
            this.cboProgram.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboProgram.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboProgram.Properties.NullText = "";
            this.cboProgram.Properties.View = this.gridView5;
            this.cboProgram.Size = new System.Drawing.Size(166, 20);
            this.cboProgram.StyleController = this.layoutControl1;
            this.cboProgram.TabIndex = 28;
            this.cboProgram.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboProgram_ButtonClick);
            this.cboProgram.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboProgram_CustomDisplayText);
            // 
            // gridView5
            // 
            this.gridView5.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView5.Name = "gridView5";
            this.gridView5.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView5.OptionsView.ShowGroupPanel = false;
            // 
            // cboPatientType
            // 
            this.cboPatientType.EnterMoveNextControl = true;
            this.cboPatientType.Location = new System.Drawing.Point(407, 73);
            this.cboPatientType.Name = "cboPatientType";
            this.cboPatientType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboPatientType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboPatientType.Properties.NullText = "";
            this.cboPatientType.Properties.View = this.gridView2;
            this.cboPatientType.Size = new System.Drawing.Size(162, 20);
            this.cboPatientType.StyleController = this.layoutControl1;
            this.cboPatientType.TabIndex = 23;
            this.cboPatientType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboPatientType_ButtonClick);
            this.cboPatientType.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboPatientType_CustomDisplayText);
            // 
            // gridView2
            // 
            this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // dtStoreTime_From
            // 
            this.dtStoreTime_From.EditValue = null;
            this.dtStoreTime_From.Location = new System.Drawing.Point(879, 25);
            this.dtStoreTime_From.MenuManager = this.barManager1;
            this.dtStoreTime_From.Name = "dtStoreTime_From";
            this.dtStoreTime_From.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.dtStoreTime_From.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStoreTime_From.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStoreTime_From.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtStoreTime_From.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtStoreTime_From.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtStoreTime_From.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtStoreTime_From.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtStoreTime_From.Size = new System.Drawing.Size(81, 20);
            this.dtStoreTime_From.StyleController = this.layoutControl1;
            this.dtStoreTime_From.TabIndex = 25;
            // 
            // btnSaveTreatment
            // 
            this.btnSaveTreatment.Location = new System.Drawing.Point(456, 145);
            this.btnSaveTreatment.Name = "btnSaveTreatment";
            this.btnSaveTreatment.Size = new System.Drawing.Size(113, 22);
            this.btnSaveTreatment.StyleController = this.layoutControl1;
            this.btnSaveTreatment.TabIndex = 9;
            this.btnSaveTreatment.Text = "Tạo và lưu BA (Ctrl S)";
            this.btnSaveTreatment.ToolTip = "Tạo và lưu bệnh án lưu trữ";
            this.btnSaveTreatment.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtKeywordMediRecord
            // 
            this.txtKeywordMediRecord.Location = new System.Drawing.Point(784, 97);
            this.txtKeywordMediRecord.Name = "txtKeywordMediRecord";
            this.txtKeywordMediRecord.Properties.NullValuePrompt = "Từ khóa tìm kiếm";
            this.txtKeywordMediRecord.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtKeywordMediRecord.Size = new System.Drawing.Size(261, 20);
            this.txtKeywordMediRecord.StyleController = this.layoutControl1;
            toolTipItem1.Text = "Mã được cấp cho bệnh nhân trong chương trình";
            superToolTip1.Items.Add(toolTipItem1);
            this.txtKeywordMediRecord.SuperTip = superToolTip1;
            this.txtKeywordMediRecord.TabIndex = 24;
            this.txtKeywordMediRecord.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtProgramCode_KeyUp);
            this.txtKeywordMediRecord.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtKeywordMediRecord_PreviewKeyDown);
            // 
            // cboTreatmentType
            // 
            this.cboTreatmentType.Location = new System.Drawing.Point(119, 73);
            this.cboTreatmentType.Name = "cboTreatmentType";
            this.cboTreatmentType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboTreatmentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboTreatmentType.Properties.NullText = "";
            this.cboTreatmentType.Properties.View = this.gridLookUpEdit3View;
            this.cboTreatmentType.Size = new System.Drawing.Size(189, 20);
            this.cboTreatmentType.StyleController = this.layoutControl1;
            this.cboTreatmentType.TabIndex = 17;
            this.cboTreatmentType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboTreatmentType_ButtonClick);
            this.cboTreatmentType.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboTreatmentType_CustomDisplayText);
            // 
            // gridLookUpEdit3View
            // 
            this.gridLookUpEdit3View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit3View.Name = "gridLookUpEdit3View";
            this.gridLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit3View.OptionsView.ShowGroupPanel = false;
            // 
            // dtStoreTime_To
            // 
            this.dtStoreTime_To.EditValue = null;
            this.dtStoreTime_To.Location = new System.Drawing.Point(964, 25);
            this.dtStoreTime_To.MenuManager = this.barManager1;
            this.dtStoreTime_To.Name = "dtStoreTime_To";
            this.dtStoreTime_To.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.dtStoreTime_To.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStoreTime_To.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStoreTime_To.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtStoreTime_To.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtStoreTime_To.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtStoreTime_To.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtStoreTime_To.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtStoreTime_To.Size = new System.Drawing.Size(81, 20);
            this.dtStoreTime_To.StyleController = this.layoutControl1;
            this.dtStoreTime_To.TabIndex = 26;
            // 
            // cboStatusEnd
            // 
            this.cboStatusEnd.EditValue = "";
            this.cboStatusEnd.Location = new System.Drawing.Point(119, 25);
            this.cboStatusEnd.Name = "cboStatusEnd";
            this.cboStatusEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboStatusEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboStatusEnd.Properties.NullText = "";
            this.cboStatusEnd.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cboStatusEnd.Properties.View = this.gridLookUpEdit2View;
            this.cboStatusEnd.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboStatusEnd_Properties_ButtonClick);
            this.cboStatusEnd.Size = new System.Drawing.Size(189, 20);
            this.cboStatusEnd.StyleController = this.layoutControl1;
            this.cboStatusEnd.TabIndex = 12;
            this.cboStatusEnd.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.cboStatusEnd_Closed);
            // 
            // gridLookUpEdit2View
            // 
            this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
            this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
            // 
            // cboStoreCheckStatus
            // 
            this.cboStoreCheckStatus.EditValue = "";
            this.cboStoreCheckStatus.Location = new System.Drawing.Point(442, 25);
            this.cboStoreCheckStatus.Name = "cboStoreCheckStatus";
            this.cboStoreCheckStatus.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboStoreCheckStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboStoreCheckStatus.Properties.NullText = "";
            this.cboStoreCheckStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cboStoreCheckStatus.Properties.View = this.gridLookUpEditStoreCheckView;
            this.cboStoreCheckStatus.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboStoreCheckStatus_Properties_ButtonClick);
            this.cboStoreCheckStatus.Size = new System.Drawing.Size(127, 20);
            this.cboStoreCheckStatus.StyleController = this.layoutControl1;
            this.cboStoreCheckStatus.TabIndex = 13;
            this.cboStoreCheckStatus.EditValueChanged += new System.EventHandler(this.cboStoreCheckStatus_EditValueChanged);
            // 
            // gridLookUpEditStoreCheckView
            // 
            this.gridLookUpEditStoreCheckView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEditStoreCheckView.Name = "gridLookUpEditStoreCheckView";
            this.gridLookUpEditStoreCheckView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEditStoreCheckView.OptionsView.ShowGroupPanel = false;
            // 
            // txtStoreCodeMediRecord
            // 
            this.txtStoreCodeMediRecord.EditValue = "";
            this.txtStoreCodeMediRecord.Location = new System.Drawing.Point(784, 73);
            this.txtStoreCodeMediRecord.Name = "txtStoreCodeMediRecord";
            this.txtStoreCodeMediRecord.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtStoreCodeMediRecord.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtStoreCodeMediRecord.Size = new System.Drawing.Size(89, 20);
            this.txtStoreCodeMediRecord.StyleController = this.layoutControl1;
            this.txtStoreCodeMediRecord.TabIndex = 18;
            this.txtStoreCodeMediRecord.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtStoreCode_KeyUp);
            // 
            // dtOutTimeFrom
            // 
            this.dtOutTimeFrom.EditValue = null;
            this.dtOutTimeFrom.Location = new System.Drawing.Point(119, 97);
            this.dtOutTimeFrom.Name = "dtOutTimeFrom";
            this.dtOutTimeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtOutTimeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtOutTimeFrom.Size = new System.Drawing.Size(98, 20);
            this.dtOutTimeFrom.StyleController = this.layoutControl1;
            this.dtOutTimeFrom.TabIndex = 19;
            // 
            // btnSearchTreatment
            // 
            this.btnSearchTreatment.Location = new System.Drawing.Point(361, 145);
            this.btnSearchTreatment.Name = "btnSearchTreatment";
            this.btnSearchTreatment.Size = new System.Drawing.Size(91, 22);
            this.btnSearchTreatment.StyleController = this.layoutControl1;
            this.btnSearchTreatment.TabIndex = 11;
            this.btnSearchTreatment.Text = "Tìm (Ctr F)";
            this.btnSearchTreatment.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtTreatmentCodeSearch
            // 
            this.txtTreatmentCodeSearch.Location = new System.Drawing.Point(24, 121);
            this.txtTreatmentCodeSearch.Name = "txtTreatmentCodeSearch";
            this.txtTreatmentCodeSearch.Properties.NullValuePrompt = "Mã điều trị (F2)";
            this.txtTreatmentCodeSearch.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtTreatmentCodeSearch.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtTreatmentCodeSearch.Size = new System.Drawing.Size(140, 20);
            this.txtTreatmentCodeSearch.StyleController = this.layoutControl1;
            this.txtTreatmentCodeSearch.TabIndex = 13;
            this.txtTreatmentCodeSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchTreatment_KeyUp);
            // 
            // txtPatientCodeSearch
            // 
            this.txtPatientCodeSearch.Location = new System.Drawing.Point(168, 121);
            this.txtPatientCodeSearch.MenuManager = this.barManager1;
            this.txtPatientCodeSearch.Name = "txtPatientCodeSearch";
            this.txtPatientCodeSearch.Properties.NullValuePrompt = "Mã bệnh nhân (F3)";
            this.txtPatientCodeSearch.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtPatientCodeSearch.Properties.ShowNullValuePromptWhenFocused = true;
            this.txtPatientCodeSearch.Size = new System.Drawing.Size(140, 20);
            this.txtPatientCodeSearch.StyleController = this.layoutControl1;
            this.txtPatientCodeSearch.TabIndex = 14;
            this.txtPatientCodeSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPatientCodeSearch_KeyUp);
            // 
            // dtInTimeTo
            // 
            this.dtInTimeTo.EditValue = null;
            this.dtInTimeTo.Location = new System.Drawing.Point(492, 97);
            this.dtInTimeTo.Name = "dtInTimeTo";
            this.dtInTimeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtInTimeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtInTimeTo.Size = new System.Drawing.Size(77, 20);
            this.dtInTimeTo.StyleController = this.layoutControl1;
            this.dtInTimeTo.TabIndex = 22;
            // 
            // txtKeywordTreatment
            // 
            this.txtKeywordTreatment.Location = new System.Drawing.Point(312, 121);
            this.txtKeywordTreatment.Name = "txtKeywordTreatment";
            this.txtKeywordTreatment.Properties.NullValuePrompt = "Từ khóa tìm kiếm";
            this.txtKeywordTreatment.Properties.NullValuePromptShowForEmptyValue = true;
            this.txtKeywordTreatment.Size = new System.Drawing.Size(257, 20);
            this.txtKeywordTreatment.StyleController = this.layoutControl1;
            this.txtKeywordTreatment.TabIndex = 15;
            this.txtKeywordTreatment.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // cboEndDepartment
            // 
            this.cboEndDepartment.Location = new System.Drawing.Point(407, 49);
            this.cboEndDepartment.Name = "cboEndDepartment";
            this.cboEndDepartment.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboEndDepartment.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboEndDepartment.Properties.NullText = "";
            this.cboEndDepartment.Properties.View = this.gridView1;
            this.cboEndDepartment.Size = new System.Drawing.Size(162, 20);
            this.cboEndDepartment.StyleController = this.layoutControl1;
            this.cboEndDepartment.TabIndex = 16;
            this.cboEndDepartment.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboEndDepartment_ButtonClick);
            this.cboEndDepartment.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.cboEndDepartment_CustomDisplayText);
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // cboLocationStore
            // 
            this.cboLocationStore.Location = new System.Drawing.Point(1144, 73);
            this.cboLocationStore.MenuManager = this.barManager1;
            this.cboLocationStore.Name = "cboLocationStore";
            this.cboLocationStore.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.cboLocationStore.Properties.AutoComplete = false;
            this.cboLocationStore.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)});
            this.cboLocationStore.Properties.NullText = "";
            this.cboLocationStore.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.cboLocationStore.Properties.View = this.customGridLookUpEditWithFilterMultiColumn1View;
            this.cboLocationStore.Size = new System.Drawing.Size(192, 20);
            this.cboLocationStore.StyleController = this.layoutControl1;
            this.cboLocationStore.TabIndex = 48;
            this.cboLocationStore.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cboLocationStore_ButtonClick);
            // 
            // customGridLookUpEditWithFilterMultiColumn1View
            // 
            this.customGridLookUpEditWithFilterMultiColumn1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.customGridLookUpEditWithFilterMultiColumn1View.Name = "customGridLookUpEditWithFilterMultiColumn1View";
            this.customGridLookUpEditWithFilterMultiColumn1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.customGridLookUpEditWithFilterMultiColumn1View.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciAreaNotStore,
            this.lciAreaStored});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1360, 739);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lciAreaNotStore
            // 
            this.lciAreaNotStore.ExpandButtonVisible = true;
            this.lciAreaNotStore.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem26,
            this.layoutControlItem20,
            this.layoutControlItem18,
            this.layoutControlItem19,
            this.layoutControlItem25,
            this.lciStatusEnd,
            this.lciEndDepartment,
            this.lciOutTimeFilter,
            this.lciInTimeFilter,
            this.layoutControlItem11,
            this.layoutControlItem9,
            this.lciKeywordTreatment,
            this.lciTreatmentCodeSearch,
            this.lciPatientCodeSearch,
            this.emptySpaceItem3,
            this.layoutControlItem7,
            this.lciJustShowTreatmentLatch,
            this.layoutControlItem8,
            this.lciStoreCheckStatus,
            this.lciTreatmentType,
            this.lciPatientType});
            this.lciAreaNotStore.Location = new System.Drawing.Point(0, 0);
            this.lciAreaNotStore.Name = "lciAreaNotStore";
            this.lciAreaNotStore.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciAreaNotStore.Size = new System.Drawing.Size(574, 739);
            this.lciAreaNotStore.TextLocation = DevExpress.Utils.Locations.Left;
            this.lciAreaNotStore.TextVisible = false;
            // 
            // layoutControlItem26
            // 
            this.layoutControlItem26.Control = this.checkAutoShowStore;
            this.layoutControlItem26.Location = new System.Drawing.Point(0, 711);
            this.layoutControlItem26.Name = "layoutControlItem26";
            this.layoutControlItem26.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem26.OptionsTableLayoutItem.RowIndex = 8;
            this.layoutControlItem26.Size = new System.Drawing.Size(283, 23);
            this.layoutControlItem26.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem26.TextVisible = false;
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.Control = this.gridControlTreatment;
            this.layoutControlItem20.Location = new System.Drawing.Point(0, 167);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem20.OptionsTableLayoutItem.RowIndex = 7;
            this.layoutControlItem20.Size = new System.Drawing.Size(549, 520);
            this.layoutControlItem20.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem20.TextVisible = false;
            // 
            // layoutControlItem18
            // 
            this.layoutControlItem18.Control = this.btnSearchTreatment;
            this.layoutControlItem18.Location = new System.Drawing.Point(337, 141);
            this.layoutControlItem18.Name = "layoutControlItem18";
            this.layoutControlItem18.OptionsTableLayoutItem.ColumnIndex = 1;
            this.layoutControlItem18.OptionsTableLayoutItem.RowIndex = 6;
            this.layoutControlItem18.Size = new System.Drawing.Size(95, 26);
            this.layoutControlItem18.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem18.TextVisible = false;
            // 
            // layoutControlItem19
            // 
            this.layoutControlItem19.Control = this.btnSaveTreatment;
            this.layoutControlItem19.Location = new System.Drawing.Point(432, 141);
            this.layoutControlItem19.Name = "layoutControlItem19";
            this.layoutControlItem19.OptionsTableLayoutItem.RowIndex = 7;
            this.layoutControlItem19.Size = new System.Drawing.Size(117, 26);
            this.layoutControlItem19.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem19.TextVisible = false;
            // 
            // layoutControlItem25
            // 
            this.layoutControlItem25.Control = this.lblDanhSachHoSoChuaLuuTru;
            this.layoutControlItem25.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem25.Name = "layoutControlItem25";
            this.layoutControlItem25.Size = new System.Drawing.Size(549, 21);
            this.layoutControlItem25.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem25.TextVisible = false;
            // 
            // lciStatusEnd
            // 
            this.lciStatusEnd.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciStatusEnd.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciStatusEnd.Control = this.cboStatusEnd;
            this.lciStatusEnd.Location = new System.Drawing.Point(0, 21);
            this.lciStatusEnd.Name = "lciStatusEnd";
            this.lciStatusEnd.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciStatusEnd.Size = new System.Drawing.Size(288, 24);
            this.lciStatusEnd.Text = "Trạng thái:";
            this.lciStatusEnd.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciStatusEnd.TextSize = new System.Drawing.Size(90, 20);
            this.lciStatusEnd.TextToControlDistance = 5;
            // 
            // lciEndDepartment
            // 
            this.lciEndDepartment.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciEndDepartment.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciEndDepartment.Control = this.cboEndDepartment;
            this.lciEndDepartment.Location = new System.Drawing.Point(288, 45);
            this.lciEndDepartment.Name = "lciEndDepartment";
            this.lciEndDepartment.OptionsTableLayoutItem.RowIndex = 1;
            this.lciEndDepartment.Size = new System.Drawing.Size(261, 24);
            this.lciEndDepartment.Text = "Khoa kết thúc:";
            this.lciEndDepartment.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciEndDepartment.TextSize = new System.Drawing.Size(90, 20);
            this.lciEndDepartment.TextToControlDistance = 5;
            // 
            // lciOutTimeFilter
            // 
            this.lciOutTimeFilter.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciOutTimeFilter.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciOutTimeFilter.Control = this.dtOutTimeFrom;
            this.lciOutTimeFilter.Location = new System.Drawing.Point(0, 93);
            this.lciOutTimeFilter.Name = "lciOutTimeFilter";
            this.lciOutTimeFilter.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciOutTimeFilter.OptionsTableLayoutItem.RowIndex = 2;
            this.lciOutTimeFilter.Size = new System.Drawing.Size(197, 24);
            this.lciOutTimeFilter.Text = "Thời gian ra:";
            this.lciOutTimeFilter.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciOutTimeFilter.TextSize = new System.Drawing.Size(90, 20);
            this.lciOutTimeFilter.TextToControlDistance = 5;
            // 
            // lciInTimeFilter
            // 
            this.lciInTimeFilter.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciInTimeFilter.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciInTimeFilter.Control = this.dtInTimeFrom;
            this.lciInTimeFilter.Location = new System.Drawing.Point(288, 93);
            this.lciInTimeFilter.Name = "lciInTimeFilter";
            this.lciInTimeFilter.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciInTimeFilter.OptionsTableLayoutItem.RowIndex = 3;
            this.lciInTimeFilter.Size = new System.Drawing.Size(180, 24);
            this.lciInTimeFilter.Text = "Thời gian vào:";
            this.lciInTimeFilter.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciInTimeFilter.TextSize = new System.Drawing.Size(90, 20);
            this.lciInTimeFilter.TextToControlDistance = 5;
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.Control = this.dtOutTimeTo;
            this.layoutControlItem11.Location = new System.Drawing.Point(197, 93);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.OptionsTableLayoutItem.RowIndex = 3;
            this.layoutControlItem11.Size = new System.Drawing.Size(91, 24);
            this.layoutControlItem11.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem11.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.dtInTimeTo;
            this.layoutControlItem9.Location = new System.Drawing.Point(468, 93);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.OptionsTableLayoutItem.RowIndex = 4;
            this.layoutControlItem9.Size = new System.Drawing.Size(81, 24);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // lciKeywordTreatment
            // 
            this.lciKeywordTreatment.Control = this.txtKeywordTreatment;
            this.lciKeywordTreatment.Location = new System.Drawing.Point(288, 117);
            this.lciKeywordTreatment.Name = "lciKeywordTreatment";
            this.lciKeywordTreatment.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciKeywordTreatment.OptionsTableLayoutItem.RowIndex = 5;
            this.lciKeywordTreatment.Size = new System.Drawing.Size(261, 24);
            this.lciKeywordTreatment.TextSize = new System.Drawing.Size(0, 0);
            this.lciKeywordTreatment.TextVisible = false;
            // 
            // lciTreatmentCodeSearch
            // 
            this.lciTreatmentCodeSearch.Control = this.txtTreatmentCodeSearch;
            this.lciTreatmentCodeSearch.Location = new System.Drawing.Point(0, 117);
            this.lciTreatmentCodeSearch.Name = "lciTreatmentCodeSearch";
            this.lciTreatmentCodeSearch.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciTreatmentCodeSearch.OptionsTableLayoutItem.RowIndex = 4;
            this.lciTreatmentCodeSearch.Size = new System.Drawing.Size(144, 24);
            this.lciTreatmentCodeSearch.TextSize = new System.Drawing.Size(0, 0);
            this.lciTreatmentCodeSearch.TextVisible = false;
            // 
            // lciPatientCodeSearch
            // 
            this.lciPatientCodeSearch.Control = this.txtPatientCodeSearch;
            this.lciPatientCodeSearch.Location = new System.Drawing.Point(144, 117);
            this.lciPatientCodeSearch.Name = "lciPatientCodeSearch";
            this.lciPatientCodeSearch.OptionsTableLayoutItem.RowIndex = 5;
            this.lciPatientCodeSearch.Size = new System.Drawing.Size(144, 24);
            this.lciPatientCodeSearch.TextSize = new System.Drawing.Size(0, 0);
            this.lciPatientCodeSearch.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            this.emptySpaceItem3.AllowHotTrack = false;
            this.emptySpaceItem3.Location = new System.Drawing.Point(0, 141);
            this.emptySpaceItem3.Name = "emptySpaceItem3";
            this.emptySpaceItem3.Size = new System.Drawing.Size(337, 26);
            this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.ucPagingTreatment;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 687);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(549, 24);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // lciJustShowTreatmentLatch
            // 
            this.lciJustShowTreatmentLatch.Control = this.checkJustShowTreatmentLatch;
            this.lciJustShowTreatmentLatch.Location = new System.Drawing.Point(283, 711);
            this.lciJustShowTreatmentLatch.Name = "lciJustShowTreatmentLatch";
            this.lciJustShowTreatmentLatch.Size = new System.Drawing.Size(266, 23);
            this.lciJustShowTreatmentLatch.TextSize = new System.Drawing.Size(0, 0);
            this.lciJustShowTreatmentLatch.TextVisible = false;
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem8.Control = this.cboExecuteDepartment;
            this.layoutControlItem8.Location = new System.Drawing.Point(0, 45);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(288, 24);
            this.layoutControlItem8.Text = "Khoa điều trị:";
            this.layoutControlItem8.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem8.TextSize = new System.Drawing.Size(90, 20);
            this.layoutControlItem8.TextToControlDistance = 5;
            // 
            // lciStoreCheckStatus
            // 
            this.lciStoreCheckStatus.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciStoreCheckStatus.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciStoreCheckStatus.Control = this.cboStoreCheckStatus;
            this.lciStoreCheckStatus.Location = new System.Drawing.Point(288, 21);
            this.lciStoreCheckStatus.Name = "lciStoreCheckStatus";
            this.lciStoreCheckStatus.Size = new System.Drawing.Size(261, 24);
            this.lciStoreCheckStatus.Text = "Tình trạng kiểm tra:";
            this.lciStoreCheckStatus.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciStoreCheckStatus.TextSize = new System.Drawing.Size(125, 20);
            this.lciStoreCheckStatus.TextToControlDistance = 5;
            // 
            // lciTreatmentType
            // 
            this.lciTreatmentType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciTreatmentType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciTreatmentType.Control = this.cboTreatmentType;
            this.lciTreatmentType.Location = new System.Drawing.Point(0, 69);
            this.lciTreatmentType.Name = "lciTreatmentType";
            this.lciTreatmentType.OptionsTableLayoutItem.RowIndex = 2;
            this.lciTreatmentType.Size = new System.Drawing.Size(288, 24);
            this.lciTreatmentType.Text = "Diện điều trị:";
            this.lciTreatmentType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciTreatmentType.TextSize = new System.Drawing.Size(90, 20);
            this.lciTreatmentType.TextToControlDistance = 5;
            // 
            // lciPatientType
            // 
            this.lciPatientType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciPatientType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciPatientType.Control = this.cboPatientType;
            this.lciPatientType.Location = new System.Drawing.Point(288, 69);
            this.lciPatientType.Name = "lciPatientType";
            this.lciPatientType.OptionsTableLayoutItem.ColumnIndex = 1;
            this.lciPatientType.OptionsTableLayoutItem.RowIndex = 1;
            this.lciPatientType.Size = new System.Drawing.Size(261, 24);
            this.lciPatientType.Text = "Đối tượng:";
            this.lciPatientType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciPatientType.TextSize = new System.Drawing.Size(90, 20);
            this.lciPatientType.TextToControlDistance = 5;
            // 
            // lciAreaStored
            // 
            this.lciAreaStored.ExpandButtonVisible = true;
            this.lciAreaStored.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem21,
            this.layoutControlItem2,
            this.layoutControlItem16,
            this.lciDataStoreList,
            this.layoutControlItem17,
            this.lciStoreTimeFilter,
            this.lciTreatmentCodeMediRecord,
            this.lciStoreCodeMediRecord,
            this.lciProgramFilter,
            this.lciFilterMediRecordType,
            this.lciPatientCodeMedRecord,
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem23,
            this.layoutControlItem6,
            this.emptySpaceItem2,
            this.layoutControlItem5,
            this.layoutControlItem10,
            this.layoutControlItem12,
            this.lciKeywordMediRecord});
            this.lciAreaStored.Location = new System.Drawing.Point(574, 0);
            this.lciAreaStored.Name = "lciAreaStored";
            this.lciAreaStored.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciAreaStored.Size = new System.Drawing.Size(786, 739);
            this.lciAreaStored.TextLocation = DevExpress.Utils.Locations.Right;
            this.lciAreaStored.TextVisible = false;
            // 
            // layoutControlItem21
            // 
            this.layoutControlItem21.Control = this.lblDanhSachBenhAn;
            this.layoutControlItem21.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.Size = new System.Drawing.Size(761, 21);
            this.layoutControlItem21.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem21.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.gridControlMediRecord;
            this.layoutControlItem2.Location = new System.Drawing.Point(205, 119);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem2.Size = new System.Drawing.Size(556, 411);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem16
            // 
            this.layoutControlItem16.Control = this.gridControlTreatmentOfMediRecord;
            this.layoutControlItem16.Location = new System.Drawing.Point(205, 564);
            this.layoutControlItem16.Name = "layoutControlItem16";
            this.layoutControlItem16.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem16.Size = new System.Drawing.Size(556, 170);
            this.layoutControlItem16.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem16.TextVisible = false;
            // 
            // lciDataStoreList
            // 
            this.lciDataStoreList.ExpandButtonVisible = true;
            this.lciDataStoreList.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem28});
            this.lciDataStoreList.Location = new System.Drawing.Point(0, 21);
            this.lciDataStoreList.Name = "lciDataStoreList";
            this.lciDataStoreList.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.lciDataStoreList.Size = new System.Drawing.Size(205, 713);
            this.lciDataStoreList.TextLocation = DevExpress.Utils.Locations.Left;
            this.lciDataStoreList.TextVisible = false;
            // 
            // layoutControlItem28
            // 
            this.layoutControlItem28.Control = this.treeListMedicalStore;
            this.layoutControlItem28.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem28.Name = "layoutControlItem28";
            this.layoutControlItem28.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem28.Size = new System.Drawing.Size(180, 708);
            this.layoutControlItem28.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem28.TextVisible = false;
            // 
            // layoutControlItem17
            // 
            this.layoutControlItem17.Control = this.dtStoreTime_To;
            this.layoutControlItem17.Location = new System.Drawing.Point(385, 21);
            this.layoutControlItem17.Name = "layoutControlItem17";
            this.layoutControlItem17.Size = new System.Drawing.Size(85, 24);
            this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem17.TextVisible = false;
            // 
            // lciStoreTimeFilter
            // 
            this.lciStoreTimeFilter.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciStoreTimeFilter.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciStoreTimeFilter.Control = this.dtStoreTime_From;
            this.lciStoreTimeFilter.Location = new System.Drawing.Point(205, 21);
            this.lciStoreTimeFilter.Name = "lciStoreTimeFilter";
            this.lciStoreTimeFilter.Size = new System.Drawing.Size(180, 24);
            this.lciStoreTimeFilter.Text = "Thời gian lưu:";
            this.lciStoreTimeFilter.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciStoreTimeFilter.TextSize = new System.Drawing.Size(90, 20);
            this.lciStoreTimeFilter.TextToControlDistance = 5;
            // 
            // lciTreatmentCodeMediRecord
            // 
            this.lciTreatmentCodeMediRecord.Control = this.txtTreatmentCodeMediRecord;
            this.lciTreatmentCodeMediRecord.Location = new System.Drawing.Point(385, 69);
            this.lciTreatmentCodeMediRecord.Name = "lciTreatmentCodeMediRecord";
            this.lciTreatmentCodeMediRecord.Size = new System.Drawing.Size(85, 24);
            this.lciTreatmentCodeMediRecord.TextSize = new System.Drawing.Size(0, 0);
            this.lciTreatmentCodeMediRecord.TextVisible = false;
            // 
            // lciStoreCodeMediRecord
            // 
            this.lciStoreCodeMediRecord.Control = this.txtStoreCodeMediRecord;
            this.lciStoreCodeMediRecord.Location = new System.Drawing.Point(205, 69);
            this.lciStoreCodeMediRecord.Name = "lciStoreCodeMediRecord";
            this.lciStoreCodeMediRecord.Size = new System.Drawing.Size(93, 24);
            this.lciStoreCodeMediRecord.TextSize = new System.Drawing.Size(0, 0);
            this.lciStoreCodeMediRecord.TextVisible = false;
            // 
            // lciProgramFilter
            // 
            this.lciProgramFilter.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciProgramFilter.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciProgramFilter.Control = this.cboProgram;
            this.lciProgramFilter.Location = new System.Drawing.Point(205, 45);
            this.lciProgramFilter.Name = "lciProgramFilter";
            this.lciProgramFilter.Size = new System.Drawing.Size(265, 24);
            this.lciProgramFilter.Text = "Chương trình:";
            this.lciProgramFilter.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciProgramFilter.TextSize = new System.Drawing.Size(90, 20);
            this.lciProgramFilter.TextToControlDistance = 5;
            // 
            // lciFilterMediRecordType
            // 
            this.lciFilterMediRecordType.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciFilterMediRecordType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciFilterMediRecordType.Control = this.cboMediRecordType;
            this.lciFilterMediRecordType.Location = new System.Drawing.Point(470, 45);
            this.lciFilterMediRecordType.Name = "lciFilterMediRecordType";
            this.lciFilterMediRecordType.Size = new System.Drawing.Size(291, 24);
            this.lciFilterMediRecordType.Text = "Loại bệnh án:";
            this.lciFilterMediRecordType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciFilterMediRecordType.TextSize = new System.Drawing.Size(90, 20);
            this.lciFilterMediRecordType.TextToControlDistance = 5;
            // 
            // lciPatientCodeMedRecord
            // 
            this.lciPatientCodeMedRecord.Control = this.txtPatientCodeMediRecord;
            this.lciPatientCodeMedRecord.Location = new System.Drawing.Point(298, 69);
            this.lciPatientCodeMedRecord.Name = "lciPatientCodeMedRecord";
            this.lciPatientCodeMedRecord.Size = new System.Drawing.Size(87, 24);
            this.lciPatientCodeMedRecord.TextSize = new System.Drawing.Size(0, 0);
            this.lciPatientCodeMedRecord.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem1.Control = this.rdoStatus__All;
            this.layoutControlItem1.Location = new System.Drawing.Point(470, 21);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(103, 24);
            this.layoutControlItem1.Text = "Tất cả";
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(65, 20);
            this.layoutControlItem1.TextToControlDistance = 5;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem3.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem3.Control = this.rdoStatus__UnStored;
            this.layoutControlItem3.Location = new System.Drawing.Point(573, 21);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(92, 24);
            this.layoutControlItem3.Text = "Chưa lưu";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(60, 20);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem4.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem4.Control = this.rdoStatus__Stored;
            this.layoutControlItem4.Location = new System.Drawing.Point(665, 21);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(96, 24);
            this.layoutControlItem4.Text = "Đã lưu";
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(50, 20);
            this.layoutControlItem4.TextToControlDistance = 5;
            // 
            // layoutControlItem23
            // 
            this.layoutControlItem23.Control = this.btnSearchMediRecord;
            this.layoutControlItem23.Location = new System.Drawing.Point(590, 93);
            this.layoutControlItem23.Name = "layoutControlItem23";
            this.layoutControlItem23.Size = new System.Drawing.Size(91, 26);
            this.layoutControlItem23.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem23.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.btnImportStore;
            this.layoutControlItem6.Location = new System.Drawing.Point(497, 93);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(93, 26);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(470, 93);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(27, 26);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.ucPagingMediRecord;
            this.layoutControlItem5.Location = new System.Drawing.Point(205, 530);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(556, 34);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.btnExportExcel;
            this.layoutControlItem10.Location = new System.Drawing.Point(681, 93);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(80, 26);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem12
            // 
            this.layoutControlItem12.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem12.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem12.Control = this.cboLocationStore;
            this.layoutControlItem12.Location = new System.Drawing.Point(470, 69);
            this.layoutControlItem12.Name = "layoutControlItem12";
            this.layoutControlItem12.Size = new System.Drawing.Size(291, 24);
            this.layoutControlItem12.Text = "Vị trí:";
            this.layoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem12.TextSize = new System.Drawing.Size(90, 20);
            this.layoutControlItem12.TextToControlDistance = 5;
            // 
            // lciKeywordMediRecord
            // 
            this.lciKeywordMediRecord.Control = this.txtKeywordMediRecord;
            this.lciKeywordMediRecord.Location = new System.Drawing.Point(205, 93);
            this.lciKeywordMediRecord.Name = "lciKeywordMediRecord";
            this.lciKeywordMediRecord.Size = new System.Drawing.Size(265, 26);
            this.lciKeywordMediRecord.TextSize = new System.Drawing.Size(0, 0);
            this.lciKeywordMediRecord.TextVisible = false;
            // 
            // gridLookUpEdit1
            // 
            this.gridLookUpEdit1.Location = new System.Drawing.Point(625, 2);
            this.gridLookUpEdit1.Name = "gridLookUpEdit1";
            this.gridLookUpEdit1.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.gridLookUpEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridLookUpEdit1.Properties.NullText = "";
            this.gridLookUpEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.gridLookUpEdit1.Size = new System.Drawing.Size(283, 22);
            this.gridLookUpEdit1.StyleController = this.layoutControl1;
            this.gridLookUpEdit1.TabIndex = 8;
            // 
            // imageCollectionCheck
            // 
            this.imageCollectionCheck.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionCheck.ImageStream")));
            this.imageCollectionCheck.Images.SetKeyName(0, "dau tích-02.jpg");
            this.imageCollectionCheck.Images.SetKeyName(1, "dau tích-01.jpg");
            // 
            // imageCollectionRoom1
            // 
            this.imageCollectionRoom1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollectionRoom1.ImageStream")));
            this.imageCollectionRoom1.Images.SetKeyName(0, "uncheckbox.png");
            this.imageCollectionRoom1.Images.SetKeyName(1, "checkbox.png");
            // 
            // UCMedicalStoreV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UCMedicalStoreV2";
            this.Size = new System.Drawing.Size(1360, 768);
            this.Load += new System.EventHandler(this.UCMedicalStore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cboExecuteDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkJustShowTreatmentLatch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__Stored.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__UnStored.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoStatus__All.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientCodeMediRecord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboMediRecordType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListMedicalStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_CheckStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTreatmentOfMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTreatmentOfMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditDeleteDisable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditDeleteEnable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkAutoShowStore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckEnable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_ViewEmrTreatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Print_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Edit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLocationStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCustomGridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Delete_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Delete_Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_Print_Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CheckDisable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_TreatmentBorrow_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Btn_TreatmentBorrow_Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditReceiveTreatment_Enabed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditReceiveTreatment_Disabed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonEditUnImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Store__Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_Store__Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonView_Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repLocationStoreDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentCodeMediRecord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTreatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTreatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnViewTreatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_StoreCheck__Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEditInventory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_UnRejectStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit_RejectStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit_StoreCheck__Disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProgram.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboPatientType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_From.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_From.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordMediRecord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboTreatmentType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit3View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_To.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStoreTime_To.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStatusEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit2View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStoreCheckStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEditStoreCheckView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStoreCodeMediRecord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtOutTimeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTreatmentCodeSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatientCodeSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtInTimeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKeywordTreatment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndDepartment.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLocationStore.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customGridLookUpEditWithFilterMultiColumn1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAreaNotStore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStatusEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciEndDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciOutTimeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciInTimeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKeywordTreatment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentCodeSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientCodeSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciJustShowTreatmentLatch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreCheckStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciAreaStored)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciDataStoreList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem28)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreTimeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciTreatmentCodeMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciStoreCodeMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciProgramFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFilterMediRecordType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPatientCodeMedRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciKeywordMediRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollectionRoom1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		internal void PrintMediRecord(MediRecordADO mediRecordADO)
		{
			try
			{
				currentMediRecord = mediRecordADO;
				PrintProcess(PrintTypeMediRecord.IN_BARCODE_MEDI_RECORD_CODE);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void PrintProcess(PrintTypeMediRecord printType)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			try
			{
				RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.PrintTemplatePath);
				if (printType == PrintTypeMediRecord.IN_BARCODE_MEDI_RECORD_CODE)
				{
					val.RunPrintTemplate("Mps000094", new DelegateRunPrinter(DelegateRunPrinter));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool DelegateRunPrinter(string printTypeCode, string fileName)
		{
			bool result = false;
			try
			{
				if (printTypeCode == "Mps000094")
				{
					LoadBieuMauInBarCode(printTypeCode, fileName, ref result);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void LoadBieuMauInBarCode(string printTypeCode, string fileName, ref bool result)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			try
			{
				WaitingManager.Show();
                V_HIS_TREATMENT_9 val = new V_HIS_TREATMENT_9();
                DataObjectMapper.Map<V_HIS_TREATMENT_9>(val, currentMediRecord);
				Mps000094PDO data = new Mps000094PDO(val);
				PrintData printData = null;
				printData = ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.ShowDialog, "") : new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.ShowDialog, ""));
				WaitingManager.Hide();
				InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((currentMediRecord != null) ? ((V_HIS_MEDI_RECORD_1)currentMediRecord).TDL_TREATMENT_CODES : "", printTypeCode, (currentModule != null) ? currentModule.RoomId : 0);
				printData.EmrInputADO = emrInputADO;
				result = MpsPrinter.Run(printData);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}
	}
}
