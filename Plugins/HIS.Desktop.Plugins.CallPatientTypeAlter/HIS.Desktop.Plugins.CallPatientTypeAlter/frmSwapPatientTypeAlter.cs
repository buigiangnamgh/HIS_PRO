using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using AutoMapper;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.IsAdmin;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.Plugins.CallPatientTypeAlter.ADO;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Config;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Resources;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Resource;
using Inventec.Core;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.CustomControl;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public class frmSwapPatientTypeAlter : FormBase
	{
		internal Module module;

		internal List<V_HIS_SERE_SERV_4> lstHisSereServWithTreatmentOld = new List<V_HIS_SERE_SERV_4>();

		internal List<HIS_PATIENT_TYPE> currentPatientTypeWithPatientTypeAlter;

		internal List<HIS_SERVICE> lstServiceBySereServ = new List<HIS_SERVICE>();

		private long patient_type_id;

		private long? patient_primary_patient_type_id;

		private long? patient_classify_id;

		private PatientTypeDepartmentADO HisTreatmentLogSDO;

		private List<PatientTypeDepartmentADO> lstTreatmentLog;

		private List<HIS_PATIENT_TYPE> dataCombo;

		private DelegateReturnSuccess success;

		private Dictionary<long, List<V_HIS_SERVICE_PATY>> dicSevicepatyAllows = new Dictionary<long, List<V_HIS_SERVICE_PATY>>();

		private long keyIsSetPrimaryPatientType = System.Convert.ToInt16(HisConfigs.Get<string>("MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE"));

		private List<ServiceConditionADO> lstADO;

		private V_HIS_ROOM currentWorkingRoom;

		private IContainer components;

		private LayoutControl layoutControl1;

		private GridControl gridControlSereServ;

		private GridView gridViewSereServ;

		private LayoutControlGroup layoutControlGroup1;

		private LayoutControlItem layoutControlItem1;

		private GridColumn gridColSTT;

		private GridColumn gridColServiceCode;

		private GridColumn gridColServiceName;

		private GridColumn gridColServiceUnitName;

		private GridColumn gridColAmount;

		private GridColumn gridColPatientTypeName;

		private LayoutControl layoutControl2;

		private LayoutControlGroup Root;

		private SimpleButton btnChooSereServ;

		private LayoutControlItem layoutControlItem2;

		private LayoutControlItem layoutControlItem3;

		private SimpleButton btnUpdatePatientType;

		private LabelControl lblNote;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit;

		private GridView repositoryItemGridLookUpEdit1View;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItem1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private LabelControl lblUpdatePatientType;

		private LayoutControlItem layoutControlItem6;

		private GridColumn gridColAdditionRequire;

		private RepositoryItemGridLookUpEdit repositoryItemGridLookUpEditSurcharge;

		private GridView gridView1;

		private RepositoryItemGridLookUpEdit repositoryItemGridLookUpEditSurchargeDis;

		private GridView gridView2;

		private GridColumn grdColumnServiceCondition;

		private RepositoryItemCustomGridLookUpEdit repServiceCondition;

		private CustomGridViewWithFilterMultiColumn repositoryItemCustomGridLookUpEdit1View;

		private RepositoryItemMemoEdit repConditionName;

		private RepositoryItemMemoEdit repConditionCode;

		private RepositoryItemGridLookUpEdit repItemServiceCondition;

		private GridView gridView3;

		internal List<HIS_SERE_SERV> lstSereServ { get; set; }

		internal List<V_HIS_SERE_SERV_4> lstHisSereServWithTreatment { get; set; }

		public frmSwapPatientTypeAlter(Module module, long patient_type_id, long? patient_primary_patient_type_id, long? patient_classify_id, PatientTypeDepartmentADO HisTreatmentLogSDO, List<PatientTypeDepartmentADO> _lstTreatmentLog, List<V_HIS_SERE_SERV_4> _lstSereServ, DelegateReturnSuccess success)
		{
			InitializeComponent();
			this.module = module;
			this.patient_type_id = patient_type_id;
			this.patient_primary_patient_type_id = patient_primary_patient_type_id;
			this.patient_classify_id = patient_classify_id;
			this.HisTreatmentLogSDO = HisTreatmentLogSDO;
			lstTreatmentLog = _lstTreatmentLog;
			lstHisSereServWithTreatment = _lstSereServ;
			this.success = success;
		}

		private void frmSwapPatientTypeAlter_Load(object sender, EventArgs e)
		{
			try
			{
				try
				{
					string filePath = Path.Combine(ApplicationStoreLocation.ApplicationStartupPath, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]);
					base.Icon = Icon.ExtractAssociatedIcon(filePath);
				}
				catch (Exception ex)
				{
					LogSystem.Warn(ex);
				}
				if (module != null)
				{
					currentWorkingRoom = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == module.RoomId);
				}
				List<HIS_SERVICE_CONDITION> list = (from o in BackendDataWorker.Get<HIS_SERVICE_CONDITION>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				lstADO = new List<ServiceConditionADO>();
				foreach (HIS_SERVICE_CONDITION item2 in list)
				{
					ServiceConditionADO item = new ServiceConditionADO(item2);
					lstADO.Add(item);
				}
				LoadServiceConditionDefault();
				HisConfigCFG.InitWarningOverCeiling();
				SetCaptionByLanguageKey();
				GetServiceBySereServ();
				LoadDataToGridSereServ();
				LoadDataToPatientTypeRepositoryItemCombo(repositoryItemGridLookUpEdit, BackendDataWorker.Get<HIS_PATIENT_TYPE>());
				LoadDataToPatientTypeRepositoryItemComboSurcharge();
				lblUpdatePatientType.Text = string.Format("({0})", BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME);
				currentPatientTypeWithPatientTypeAlter = PatientTypeWithPatientTypeAlter();
				InitComboRespositoryPatientType(currentPatientTypeWithPatientTypeAlter);
				ProcessDataForUpdatePaty();
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Lang", typeof(frmSwapPatientTypeAlter).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.layoutControl1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lblUpdatePatientType.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.lblUpdatePatientType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnUpdatePatientType.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.btnUpdatePatientType.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				lblNote.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.lblNote.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControl2.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.layoutControl2.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				btnChooSereServ.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.btnChooSereServ.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColSTT.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColSTT.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColServiceCode.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColServiceCode.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColServiceName.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColServiceName.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColServiceUnitName.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColServiceUnitName.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColAmount.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColAmount.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColPatientTypeName.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColPatientTypeName.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				gridColAdditionRequire.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.gridColAdditionRequire.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				grdColumnServiceCondition.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.grdColumnServiceCondition.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				grdColumnServiceCondition.ToolTip = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.grdColumnServiceCondition.ToolTip", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				repServiceCondition.NullText = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.repServiceCondition.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				repositoryItemGridLookUpEdit.NullText = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.repositoryItemGridLookUpEdit.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				repositoryItemGridLookUpEditSurcharge.NullText = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.repositoryItemGridLookUpEditSurcharge.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				repositoryItemGridLookUpEditSurchargeDis.NullText = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.repositoryItemGridLookUpEditSurchargeDis.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				repItemServiceCondition.NullText = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.repItemServiceCondition.NullText", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				layoutControlItem4.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.layoutControlItem4.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				bar1.Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.bar1.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				barButtonItem1.Caption = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.barButtonItem1.Caption", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmSwapPatientTypeAlter.Text", ResourceLanguageManager.LanguageResource, LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void GetServiceBySereServ()
		{
			try
			{
				HisServiceFilter hisServiceFilter = new HisServiceFilter();
				hisServiceFilter.IDs = lstHisSereServWithTreatment.Select((V_HIS_SERE_SERV_4 o) => o.SERVICE_ID).ToList();
				lstServiceBySereServ = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE>>("api/HisService/Get", ApiConsumers.MosConsumer, hisServiceFilter, null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataToPatientTypeRepositoryItemComboSurcharge()
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("PATIENT_TYPE_CODE", "", 100, 1));
				list.Add(new ColumnInfo("PATIENT_TYPE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("PATIENT_TYPE_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(repositoryItemGridLookUpEditSurcharge, (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
					where o.IS_ADDITION == 1
					select o).ToList(), controlEditorADO);
				ControlEditorLoader.Load(repositoryItemGridLookUpEditSurchargeDis, (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
					where o.IS_ADDITION == 1
					select o).ToList(), controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void LoadDataToPatientTypeRepositoryItemCombo(RepositoryItemGridLookUpEdit repositoryItemcboPatientType, object data)
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("PATIENT_TYPE_CODE", "", 100, 1));
				list.Add(new ColumnInfo("PATIENT_TYPE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("PATIENT_TYPE_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(repositoryItemcboPatientType, data, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataToGridSereServ()
		{
			try
			{
				WaitingManager.Show();
				if (lstTreatmentLog != null)
				{
					if (lstHisSereServWithTreatmentOld == null || lstHisSereServWithTreatmentOld.Count() == 0)
					{
						foreach (V_HIS_SERE_SERV_4 item in lstHisSereServWithTreatment)
						{
							V_HIS_SERE_SERV_4 v_HIS_SERE_SERV_ = new V_HIS_SERE_SERV_4();
							DataObjectMapper.Map<V_HIS_SERE_SERV_4>(v_HIS_SERE_SERV_, item);
							lstHisSereServWithTreatmentOld.Add(v_HIS_SERE_SERV_);
						}
					}
					gridControlSereServ.DataSource = null;
					gridControlSereServ.DataSource = lstHisSereServWithTreatment;
				}
				else
				{
					Close();
				}
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void gridViewSereServ_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (e.IsGetData && e.Column.UnboundType != UnboundColumnType.Bound && (V_HIS_SERE_SERV_4)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex] != null && e.Column.FieldName == "STT")
				{
					e.Value = e.ListSourceRowIndex + 1;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnChooSereServ_Click(object sender, EventArgs e)
		{
			try
			{
				UpdateHisSereServ();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UpdateHisSereServ()
		{
			try
			{
				gridViewSereServ.PostEditor();
				lstSereServ = new List<HIS_SERE_SERV>();
				List<V_HIS_SERE_SERV_4> list = (List<V_HIS_SERE_SERV_4>)gridControlSereServ.DataSource;
				decimal? num = null;
				string arg = "";
				string text = "";
				string arg2 = "";
				bool flag = false;
				List<V_HIS_SERE_SERV_4> list2 = list.Where((V_HIS_SERE_SERV_4 o) => o.PATIENT_TYPE_ID == HisConfigCFG.PatientTypeId__BHYT).ToList();
				if (list2 != null && list2.Count > 0)
				{
					if (list2.Exists((V_HIS_SERE_SERV_4 o) => lstADO.Exists((ServiceConditionADO p) => ((HIS_SERVICE_CONDITION)p).SERVICE_ID == o.SERVICE_ID) && !o.SERVICE_CONDITION_ID.HasValue))
					{
						XtraMessageBox.Show(string.Format(ResourceMessage.DichVuBatBuocChonDieuKien, string.Join(",", from o in list2
							where lstADO.Exists((ServiceConditionADO p) => ((HIS_SERVICE_CONDITION)p).SERVICE_ID == o.SERVICE_ID) && !o.SERVICE_CONDITION_ID.HasValue
							select o.TDL_SERVICE_NAME)), ResourceMessage.ThongBao, MessageBoxButtons.OK);
						gridViewSereServ.FocusedRowHandle = list.IndexOf(list2.FirstOrDefault((V_HIS_SERE_SERV_4 o) => lstADO.Exists((ServiceConditionADO p) => ((HIS_SERVICE_CONDITION)p).SERVICE_ID == o.SERVICE_ID) && !o.SERVICE_CONDITION_ID.HasValue));
						gridViewSereServ.FocusedColumn = grdColumnServiceCondition;
						return;
					}
					num = list2.Sum((V_HIS_SERE_SERV_4 o) => o.VIR_PRICE);
					if (HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 1)
					{
						if (HisConfigCFG.WarningOverCeiling__Exam > 0m)
						{
							decimal? num2 = num;
							decimal warningOverCeiling__Exam = HisConfigCFG.WarningOverCeiling__Exam;
							if ((num2.GetValueOrDefault() > warningOverCeiling__Exam) & num2.HasValue)
							{
								arg2 = HisConfigCFG.WarningOverCeiling__Exam.ToString();
								flag = true;
							}
						}
					}
					else if (HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 3)
					{
						if (HisConfigCFG.WarningOverCeiling__In > 0m)
						{
							decimal? num2 = num;
							decimal warningOverCeiling__Exam = HisConfigCFG.WarningOverCeiling__In;
							if ((num2.GetValueOrDefault() > warningOverCeiling__Exam) & num2.HasValue)
							{
								arg2 = HisConfigCFG.WarningOverCeiling__In.ToString();
								flag = true;
							}
						}
					}
					else if (HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 2 && HisConfigCFG.WarningOverCeiling__Out > 0m)
					{
						decimal? num2 = num;
						decimal warningOverCeiling__Exam = HisConfigCFG.WarningOverCeiling__Out;
						if ((num2.GetValueOrDefault() > warningOverCeiling__Exam) & num2.HasValue)
						{
							arg2 = HisConfigCFG.WarningOverCeiling__Out.ToString();
							flag = true;
						}
					}
				}
				if (flag)
				{
					if (num.HasValue)
					{
						arg = num.ToString();
					}
					text = HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_NAME;
					if (XtraMessageBox.Show(string.Format(ResourceMessage.TongSoTienCuaCacDichVu, text, arg, arg2), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
				}
				bool value = false;
				CommonParam commonParam = new CommonParam();
				HisSereServPayslipSDO sdo = new HisSereServPayslipSDO();
				if (list != null && list.Count > 0)
				{
					Mapper.CreateMap<V_HIS_SERE_SERV_4, HIS_SERE_SERV>();
					lstSereServ = Mapper.Map<List<HIS_SERE_SERV>>(list);
				}
				sdo.SereServs = lstSereServ;
				sdo.Field = UpdateField.PATIENT_TYPE_ID;
				sdo.TreatmentId = HisTreatmentLogSDO.TREATMENT_ID;
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => sdo), sdo));
				if (new BackendAdapter(commonParam).Post<List<HIS_SERE_SERV>>("api/HisSereServ/UpdatePayslipInfo", ApiConsumers.MosConsumer, sdo, commonParam) != null)
				{
					value = true;
					commonParam.Messages = new List<string>();
					success(true);
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

		private bool CheckVuotQuaTran(List<V_HIS_SERE_SERV_4> data, ref decimal? price)
		{
			return false;
		}

		private void gridViewSereServ_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				int focusedRowHandle = gridViewSereServ.FocusedRowHandle;
				if (gridViewSereServ.SelectedRowsCount > 0)
				{
					btnChooSereServ.Enabled = true;
				}
				else
				{
					btnChooSereServ.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessDataForUpdatePaty()
		{
			try
			{
				List<V_HIS_SERVICE_PATY> source = BackendDataWorker.Get<V_HIS_SERVICE_PATY>();
				List<string> patienttpecodeAllows = currentPatientTypeWithPatientTypeAlter.Select((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE).ToList();
				List<IGrouping<long, V_HIS_SERVICE_PATY>> list = (from o in source.Where((V_HIS_SERVICE_PATY o) => patienttpecodeAllows != null && patienttpecodeAllows.Contains(o.PATIENT_TYPE_CODE)).ToList()
					group o by o.SERVICE_ID).ToList();
				if (list == null || list.Count <= 0)
				{
					return;
				}
				foreach (IGrouping<long, V_HIS_SERVICE_PATY> item in list)
				{
					dicSevicepatyAllows.Add(item.Key, item.ToList());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnUpdatePatientType_Click(object sender, EventArgs e)
		{
			try
			{
				List<V_HIS_SERE_SERV_4> list = new List<V_HIS_SERE_SERV_4>();
				string text = "";
				List<HIS_MEDICINE> list2 = new List<HIS_MEDICINE>();
				List<HIS_MATERIAL> list3 = new List<HIS_MATERIAL>();
				List<long> list4 = (from p in lstHisSereServWithTreatment
					where p.MEDICINE_ID.HasValue && p.MEDICINE_ID.Value > 0
					select p.MEDICINE_ID.Value).Distinct().ToList();
				List<long> list5 = (from p in lstHisSereServWithTreatment
					where p.MATERIAL_ID.HasValue && p.MATERIAL_ID.Value > 0
					select p.MATERIAL_ID.Value).Distinct().ToList();
				if (list4 != null && list4.Count > 0)
				{
					HisMedicineFilter hisMedicineFilter = new HisMedicineFilter();
					hisMedicineFilter.IDs = list4;
					list2 = new BackendAdapter(new CommonParam()).Get<List<HIS_MEDICINE>>("api/HisMedicine/Get", ApiConsumers.MosConsumer, hisMedicineFilter, null);
				}
				if (list5 != null && list5.Count > 0)
				{
					HisMaterialFilter hisMaterialFilter = new HisMaterialFilter();
					hisMaterialFilter.IDs = list5;
					list3 = new BackendAdapter(new CommonParam()).Get<List<HIS_MATERIAL>>("api/HisMaterial/Get", ApiConsumers.MosConsumer, hisMaterialFilter, null);
				}
				foreach (V_HIS_SERE_SERV_4 item2 in lstHisSereServWithTreatment)
				{
					item2.TDL_INTRUCTION_TIME = long.Parse(item2.TDL_INTRUCTION_TIME.ToString().Substring(0, 8) + "000000");
				}
				foreach (V_HIS_SERE_SERV_4 item in lstHisSereServWithTreatment)
				{
					long oldPatientTypeId = item.PATIENT_TYPE_ID;
					HIS_SERVICE hIS_SERVICE = lstServiceBySereServ.Where((HIS_SERVICE o) => o.ID == item.SERVICE_ID).First();
					if (item.MEDICINE_ID.HasValue && item.MEDICINE_ID.Value > 0 && list2 != null && list2.Count > 0)
					{
						if (patient_type_id == HisConfigCFG.PatientTypeId__BHYT)
						{
							HIS_MEDICINE checkMedicine = list2.FirstOrDefault((HIS_MEDICINE o) => o.ID == item.MEDICINE_ID.Value);
							V_HIS_MEDICINE_TYPE v_HIS_MEDICINE_TYPE = ((checkMedicine != null) ? BackendDataWorker.Get<V_HIS_MEDICINE_TYPE>().FirstOrDefault((V_HIS_MEDICINE_TYPE o) => o.ID == checkMedicine.MEDICINE_TYPE_ID) : null);
							if (v_HIS_MEDICINE_TYPE != null && !string.IsNullOrWhiteSpace(v_HIS_MEDICINE_TYPE.ACTIVE_INGR_BHYT_CODE) && (v_HIS_MEDICINE_TYPE.HEIN_SERVICE_TYPE_ID == 10 || v_HIS_MEDICINE_TYPE.HEIN_SERVICE_TYPE_ID == 11 || v_HIS_MEDICINE_TYPE.HEIN_SERVICE_TYPE_ID == 12))
							{
								if ((HisTreatmentLogSDO != null && HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 3) || HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 4)
								{
									DateTime value = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.MinValue;
									long num = System.Convert.ToInt16(HisConfigs.Get<string>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"));
									if (num > 0)
									{
										value = value.AddDays(num);
									}
									if (item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(value))
									{
										item.PATIENT_TYPE_ID = patient_type_id;
										item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
									}
								}
								else if (HisTreatmentLogSDO != null && item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME)
								{
									item.PATIENT_TYPE_ID = patient_type_id;
									item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
								}
							}
						}
						else
						{
							item.PATIENT_TYPE_ID = patient_type_id;
							item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
						}
					}
					else if (item.MATERIAL_ID.HasValue && item.MATERIAL_ID.Value > 0 && list3 != null && list3.Count > 0)
					{
						if (patient_type_id == HisConfigCFG.PatientTypeId__BHYT)
						{
							HIS_MATERIAL checkMaterial = list3.FirstOrDefault((HIS_MATERIAL o) => o.ID == item.MATERIAL_ID.Value);
							V_HIS_MATERIAL_TYPE v_HIS_MATERIAL_TYPE = ((checkMaterial != null) ? BackendDataWorker.Get<V_HIS_MATERIAL_TYPE>().FirstOrDefault((V_HIS_MATERIAL_TYPE o) => o.ID == checkMaterial.MATERIAL_TYPE_ID) : null);
							if (v_HIS_MATERIAL_TYPE != null && !string.IsNullOrWhiteSpace(v_HIS_MATERIAL_TYPE.HEIN_SERVICE_BHYT_CODE) && !string.IsNullOrWhiteSpace(v_HIS_MATERIAL_TYPE.HEIN_SERVICE_BHYT_NAME) && (v_HIS_MATERIAL_TYPE.HEIN_SERVICE_TYPE_ID == 14 || v_HIS_MATERIAL_TYPE.HEIN_SERVICE_TYPE_ID == 16 || v_HIS_MATERIAL_TYPE.HEIN_SERVICE_TYPE_ID == 17))
							{
								if ((HisTreatmentLogSDO != null && HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 3) || HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 4)
								{
									DateTime value2 = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.MinValue;
									long num2 = System.Convert.ToInt16(HisConfigs.Get<string>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"));
									if (num2 > 0)
									{
										value2 = value2.AddDays(num2);
									}
									if (item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(value2))
									{
										item.PATIENT_TYPE_ID = patient_type_id;
										item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
									}
								}
								else if (HisTreatmentLogSDO != null && item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME)
								{
									item.PATIENT_TYPE_ID = patient_type_id;
									item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
								}
							}
						}
						else
						{
							item.PATIENT_TYPE_ID = patient_type_id;
							item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
						}
					}
					else if (dicSevicepatyAllows.ContainsKey(item.SERVICE_ID))
					{
						if (patient_type_id == HisConfigCFG.PatientTypeId__BHYT)
						{
							if (!string.IsNullOrEmpty(item.TDL_HEIN_SERVICE_BHYT_CODE) && !string.IsNullOrEmpty(item.TDL_HEIN_SERVICE_BHYT_NAME))
							{
								List<V_HIS_SERVICE_PATY> list6 = dicSevicepatyAllows[item.SERVICE_ID];
								if (list6 != null && list6.Count > 0 && list6.FirstOrDefault((V_HIS_SERVICE_PATY o) => o.PATIENT_TYPE_ID == patient_type_id) != null)
								{
									if ((HisTreatmentLogSDO != null && HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 3) || HisTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID == 4)
									{
										DateTime value3 = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.MinValue;
										long num3 = System.Convert.ToInt16(HisConfigs.Get<string>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"));
										if (num3 > 0)
										{
											value3 = value3.AddDays(num3);
										}
										if (item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(value3))
										{
											item.PATIENT_TYPE_ID = patient_type_id;
											item.PRIMARY_PATIENT_TYPE_ID = ProcessPrimaryPatientTypeId(item, hIS_SERVICE);
											item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
										}
									}
									else if (HisTreatmentLogSDO != null && item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && (!HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME.HasValue || item.TDL_INTRUCTION_TIME <= HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME))
									{
										item.PATIENT_TYPE_ID = patient_type_id;
										item.PRIMARY_PATIENT_TYPE_ID = ProcessPrimaryPatientTypeId(item, hIS_SERVICE);
										item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
									}
								}
							}
						}
						else
						{
							item.PATIENT_TYPE_ID = patient_type_id;
							item.PRIMARY_PATIENT_TYPE_ID = ProcessPrimaryPatientTypeId(item, hIS_SERVICE);
							item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patient_type_id).PATIENT_TYPE_NAME;
						}
					}
					else
					{
						text = text + item.TDL_SERVICE_CODE + ", ";
					}
					if (item.PATIENT_TYPE_ID != HisConfigCFG.PatientTypeId__BHYT)
					{
						item.SERVICE_CONDITION_ID = null;
					}
					else if (hIS_SERVICE.DO_NOT_USE_BHYT == 1)
					{
						item.PATIENT_TYPE_ID = oldPatientTypeId;
						item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == oldPatientTypeId).PATIENT_TYPE_NAME;
					}
					list.Add(item);
				}
				if (!string.IsNullOrEmpty(text))
				{
					XtraMessageBox.Show(string.Format(ResourceMessage.DVKhongTheChuyenDoi, text), ResourceMessage.ThongBao);
				}
				gridControlSereServ.DataSource = null;
				gridControlSereServ.DataSource = list;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private long? ProcessPrimaryPatientTypeId(V_HIS_SERE_SERV_4 item, HIS_SERVICE service)
		{
			long? result = null;
			try
			{
				if (keyIsSetPrimaryPatientType == 2)
				{
					result = ((patient_primary_patient_type_id != patient_type_id) ? patient_primary_patient_type_id : ((long?)null));
					return result;
				}
				if (keyIsSetPrimaryPatientType == 1)
				{
					List<V_HIS_SERVICE_PATY> list = dicSevicepatyAllows[item.SERVICE_ID];
					if (list != null && list.Count > 0 && service != null && service.BILL_PATIENT_TYPE_ID.HasValue && list.FirstOrDefault((V_HIS_SERVICE_PATY o) => o.PATIENT_TYPE_ID == service.BILL_PATIENT_TYPE_ID) != null && item.PATIENT_TYPE_ID != service.BILL_PATIENT_TYPE_ID && (service.APPLIED_PATIENT_TYPE_IDS == null || service.APPLIED_PATIENT_TYPE_IDS.Split(',').ToList().Contains(item.PATIENT_TYPE_ID.ToString())) && (service.APPLIED_PATIENT_CLASSIFY_IDS == null || (patient_classify_id.HasValue && service.APPLIED_PATIENT_CLASSIFY_IDS.Split(',').ToList().Contains(patient_classify_id.ToString()))))
					{
						result = service.BILL_PATIENT_TYPE_ID;
						return result;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				btnChooSereServ_Click(null, null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewSereServ_ShownEditor(object sender, EventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				V_HIS_SERE_SERV_4 v_HIS_SERE_SERV_ = gridView.GetFocusedRow() as V_HIS_SERE_SERV_4;
				if (gridView.FocusedColumn.FieldName == "PATIENT_TYPE_ID" && gridView.ActiveEditor is GridLookUpEdit)
				{
					GridLookUpEdit gridLookUpEdit = gridView.ActiveEditor as GridLookUpEdit;
					FillDataIntoPatientTypeCombo(v_HIS_SERE_SERV_, gridLookUpEdit);
					gridLookUpEdit.EditValue = ((v_HIS_SERE_SERV_ != null) ? v_HIS_SERE_SERV_.PATIENT_TYPE_ID : 0);
				}
				else if (gridView.FocusedColumn.FieldName == "SERVICE_CONDITION_ID" && gridView.ActiveEditor is GridLookUpEdit)
				{
					GridLookUpEdit gridLookUpEdit2 = gridView.ActiveEditor as GridLookUpEdit;
					LoadServiceConditionShow(v_HIS_SERE_SERV_, gridLookUpEdit2);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataIntoPatientTypeCombo(V_HIS_SERE_SERV_4 data, GridLookUpEdit patientTypeCombo)
		{
			try
			{
				List<V_HIS_SERVICE_PATY> list = BackendDataWorker.Get<V_HIS_SERVICE_PATY>();
				if (list == null || list.Count <= 0)
				{
					return;
				}
				List<string> arrPatientTypeCode = (from o in list
					where data != null && o.SERVICE_ID == data.SERVICE_ID
					select o.PATIENT_TYPE_CODE).ToList();
				if (arrPatientTypeCode == null || arrPatientTypeCode.Count <= 0)
				{
					return;
				}
				dataCombo = currentPatientTypeWithPatientTypeAlter.Where((HIS_PATIENT_TYPE o) => arrPatientTypeCode.Contains(o.PATIENT_TYPE_CODE)).ToList();
				if (string.IsNullOrEmpty(data.TDL_HEIN_SERVICE_BHYT_CODE) || (string.IsNullOrEmpty(data.TDL_HEIN_SERVICE_BHYT_NAME) && dataCombo != null && dataCombo.Count > 0))
				{
					dataCombo = dataCombo.Where((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE != HisConfigCFG.PatientTypeCode__BHYT).ToList();
				}
				HIS_SERVICE hIS_SERVICE = lstServiceBySereServ.FirstOrDefault((HIS_SERVICE o) => o.ID == data.SERVICE_ID);
				if (hIS_SERVICE != null && dataCombo != null && dataCombo.Count > 0 && hIS_SERVICE.DO_NOT_USE_BHYT == 1 && !CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
				{
					dataCombo = dataCombo.Where((HIS_PATIENT_TYPE o) => o.ID != HisConfigCFG.PatientTypeId__BHYT).ToList();
				}
				List<ColumnInfo> list2 = new List<ColumnInfo>();
				list2.Add(new ColumnInfo("PATIENT_TYPE_CODE", "", 100, 1));
				list2.Add(new ColumnInfo("PATIENT_TYPE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("PATIENT_TYPE_NAME", "ID", list2, false, 250);
				ControlEditorLoader.Load(patientTypeCombo, dataCombo, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private List<HIS_PATIENT_TYPE> PatientTypeWithPatientTypeAlter()
		{
			List<HIS_PATIENT_TYPE> list = null;
			try
			{
				return BackendDataWorker.Get<HIS_PATIENT_TYPE>();
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Warn(ex);
				return list;
			}
		}

		private void InitComboRespositoryPatientType(List<HIS_PATIENT_TYPE> currentPatientTypeWithPatientTypeAlter)
		{
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("PATIENT_TYPE_CODE", "", 100, 2));
				list.Add(new ColumnInfo("PATIENT_TYPE_NAME", "", 250, 2));
				ControlEditorADO controlEditorADO = new ControlEditorADO("PATIENT_TYPE_NAME", "ID", list, false, 250);
				ControlEditorLoader.Load(repositoryItemGridLookUpEdit, currentPatientTypeWithPatientTypeAlter, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewSereServ_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				V_HIS_SERE_SERV_4 data = null;
				if (e.RowHandle > -1)
				{
					data = (V_HIS_SERE_SERV_4)((IList)((BaseView)sender).DataSource)[e.RowHandle];
				}
				if (e.RowHandle < 0 || data == null)
				{
					return;
				}
				if (e.Column.FieldName == "PATIENT_TYPE_ID")
				{
					e.RepositoryItem = repositoryItemGridLookUpEdit;
				}
				if (e.Column.FieldName == "PRIMARY_PATIENT_TYPE_ID")
				{
					HIS_SERVICE hIS_SERVICE = lstServiceBySereServ.Where((HIS_SERVICE o) => o.ID == data.SERVICE_ID).FirstOrDefault();
					e.RepositoryItem = repositoryItemGridLookUpEditSurcharge;
					e.RepositoryItem = ((keyIsSetPrimaryPatientType == 2 || (hIS_SERVICE != null && hIS_SERVICE.IS_NOT_CHANGE_BILL_PATY == 1)) ? repositoryItemGridLookUpEditSurchargeDis : repositoryItemGridLookUpEditSurcharge);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void repositoryItemGridLookUpEdit_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				V_HIS_SERE_SERV_4 row = (V_HIS_SERE_SERV_4)gridViewSereServ.GetFocusedRow();
				long num = System.Convert.ToInt64((sender as GridLookUpEdit).EditValue);
				long? num2 = null;
				if (!string.IsNullOrEmpty((gridViewSereServ.GetRowCellValue(gridViewSereServ.FocusedRowHandle, "PRIMARY_PATIENT_TYPE_ID") ?? "").ToString()))
				{
					num2 = System.Convert.ToInt64((gridViewSereServ.GetRowCellValue(gridViewSereServ.FocusedRowHandle, "PRIMARY_PATIENT_TYPE_ID") ?? "").ToString());
				}
				long? pRIMARY_PATIENT_TYPE_ID = lstHisSereServWithTreatmentOld.FirstOrDefault((V_HIS_SERE_SERV_4 o) => o.ID == row.ID).PRIMARY_PATIENT_TYPE_ID;
				HIS_SERVICE hIS_SERVICE = lstServiceBySereServ.Where((HIS_SERVICE o) => o.ID == row.SERVICE_ID).First();
				LogSystem.Info("PatientTypeId___" + num);
				long? num3 = num2;
				LogSystem.Info("PrimaryPatientTypeId___" + num3);
				num3 = pRIMARY_PATIENT_TYPE_ID;
				LogSystem.Info("oldPrimaryTypeId___" + num3);
				if (num == num2 || num == pRIMARY_PATIENT_TYPE_ID)
				{
					gridViewSereServ.SetRowCellValue(gridViewSereServ.FocusedRowHandle, gridColAdditionRequire, null);
				}
				else if (pRIMARY_PATIENT_TYPE_ID.HasValue && string.IsNullOrEmpty((gridViewSereServ.GetRowCellValue(gridViewSereServ.FocusedRowHandle, "PRIMARY_PATIENT_TYPE_ID") ?? "").ToString()))
				{
					gridViewSereServ.SetRowCellValue(gridViewSereServ.FocusedRowHandle, gridColAdditionRequire, pRIMARY_PATIENT_TYPE_ID);
				}
				if (keyIsSetPrimaryPatientType == 2)
				{
					gridViewSereServ.SetRowCellValue(gridViewSereServ.FocusedRowHandle, "PRIMARY_PATIENT_TYPE_ID", (num2 != num) ? num2 : ((long?)null));
				}
				else if (keyIsSetPrimaryPatientType == 1)
				{
					if (hIS_SERVICE == null || !hIS_SERVICE.BILL_PATIENT_TYPE_ID.HasValue || !HasServicePaty(row.SERVICE_ID, hIS_SERVICE.BILL_PATIENT_TYPE_ID) || !patient_classify_id.HasValue || num == hIS_SERVICE.BILL_PATIENT_TYPE_ID || (hIS_SERVICE.APPLIED_PATIENT_TYPE_IDS != null && !hIS_SERVICE.APPLIED_PATIENT_TYPE_IDS.Split(',').ToList().Contains(num.ToString())) || (hIS_SERVICE.APPLIED_PATIENT_CLASSIFY_IDS != null && !hIS_SERVICE.APPLIED_PATIENT_CLASSIFY_IDS.Split(',').ToList().Contains(patient_classify_id.ToString())))
					{
						gridViewSereServ.SetRowCellValue(gridViewSereServ.FocusedRowHandle, gridColAdditionRequire, null);
					}
					else
					{
						gridViewSereServ.SetRowCellValue(gridViewSereServ.FocusedRowHandle, gridColAdditionRequire, hIS_SERVICE.BILL_PATIENT_TYPE_ID);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool HasServicePaty(long serviceId, long? billPatientTypeId)
		{
			bool result = false;
			try
			{
				List<V_HIS_SERVICE_PATY> list = dicSevicepatyAllows[serviceId];
				if (list != null && list.Count > 0)
				{
					result = list.FirstOrDefault((V_HIS_SERVICE_PATY o) => o.PATIENT_TYPE_ID == billPatientTypeId) != null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public override void ProcessDisposeModuleDataAfterClose()
		{
			try
			{
				keyIsSetPrimaryPatientType = 0L;
				dicSevicepatyAllows = null;
				success = null;
				dataCombo = null;
				lstTreatmentLog = null;
				HisTreatmentLogSDO = null;
				patient_primary_patient_type_id = null;
				patient_type_id = 0L;
				lstServiceBySereServ = null;
				currentPatientTypeWithPatientTypeAlter = null;
				lstHisSereServWithTreatmentOld = null;
				lstHisSereServWithTreatment = null;
				lstSereServ = null;
				module = null;
				btnUpdatePatientType.Click -= new EventHandler(btnUpdatePatientType_Click);
				btnChooSereServ.Click -= new EventHandler(btnChooSereServ_Click);
				gridViewSereServ.CustomRowCellEdit -= new CustomRowCellEditEventHandler(gridViewSereServ_CustomRowCellEdit);
				gridViewSereServ.SelectionChanged -= new SelectionChangedEventHandler(gridViewSereServ_SelectionChanged);
				gridViewSereServ.ShownEditor -= new EventHandler(gridViewSereServ_ShownEditor);
				gridViewSereServ.CustomUnboundColumnData -= new CustomColumnDataEventHandler(gridViewSereServ_CustomUnboundColumnData);
				repositoryItemGridLookUpEdit.EditValueChanged -= new EventHandler(repositoryItemGridLookUpEdit_EditValueChanged);
				barButtonItem1.ItemClick -= new ItemClickEventHandler(barButtonItem1_ItemClick);
				base.Load -= new EventHandler(frmSwapPatientTypeAlter_Load);
				gridView2.GridControl.DataSource = null;
				repositoryItemGridLookUpEditSurchargeDis.DataSource = null;
				gridView1.GridControl.DataSource = null;
				repositoryItemGridLookUpEditSurcharge.DataSource = null;
				repositoryItemGridLookUpEdit1View.GridControl.DataSource = null;
				repositoryItemGridLookUpEdit.DataSource = null;
				gridViewSereServ.GridControl.DataSource = null;
				gridControlSereServ.DataSource = null;
				gridView2 = null;
				repositoryItemGridLookUpEditSurchargeDis = null;
				gridView1 = null;
				repositoryItemGridLookUpEditSurcharge = null;
				gridColAdditionRequire = null;
				layoutControlItem6 = null;
				lblUpdatePatientType = null;
				barDockControlRight = null;
				barDockControlLeft = null;
				barDockControlBottom = null;
				barDockControlTop = null;
				barButtonItem1 = null;
				bar1 = null;
				barManager1 = null;
				repositoryItemGridLookUpEdit1View = null;
				repositoryItemGridLookUpEdit = null;
				layoutControlItem5 = null;
				layoutControlItem4 = null;
				lblNote = null;
				btnUpdatePatientType = null;
				layoutControlItem3 = null;
				layoutControlItem2 = null;
				btnChooSereServ = null;
				Root = null;
				layoutControl2 = null;
				gridColPatientTypeName = null;
				gridColAmount = null;
				gridColServiceUnitName = null;
				gridColServiceName = null;
				gridColServiceCode = null;
				gridColSTT = null;
				layoutControlItem1 = null;
				layoutControlGroup1 = null;
				gridViewSereServ = null;
				gridControlSereServ = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadServiceConditionShow(V_HIS_SERE_SERV_4 row, GridLookUpEdit repServiceCondition)
		{
			try
			{
				repServiceCondition.Properties.DataSource = lstADO.Where((ServiceConditionADO o) => ((HIS_SERVICE_CONDITION)o).SERVICE_ID == row.SERVICE_ID).ToList();
				repServiceCondition.Properties.DisplayMember = "SERVICE_CONDITION_NAME";
				repServiceCondition.Properties.ValueMember = "ID";
				repServiceCondition.Properties.TextEditStyle = TextEditStyles.Standard;
				repServiceCondition.Properties.PopupFilterMode = PopupFilterMode.Contains;
				repServiceCondition.Properties.ImmediatePopup = true;
				repServiceCondition.Properties.PopupFormSize = new Size(470, repServiceCondition.Properties.PopupFormSize.Height);
				repServiceCondition.Properties.View.Columns.Clear();
				repServiceCondition.Properties.View.OptionsView.RowAutoHeight = true;
				GridColumn gridColumn = repServiceCondition.Properties.View.Columns.AddField("SERVICE_CONDITION_CODE");
				gridColumn.Caption = ResourceMessage.Ma;
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 100;
				gridColumn.ColumnEdit = repConditionCode;
				gridColumn.AppearanceCell.TextOptions.Trimming = Trimming.Word;
				gridColumn.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
				GridColumn gridColumn2 = repServiceCondition.Properties.View.Columns.AddField("SERVICE_CONDITION_NAME");
				gridColumn2.Caption = ResourceMessage.Ten;
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.ColumnEdit = repConditionName;
				gridColumn2.AppearanceCell.TextOptions.Trimming = Trimming.Word;
				gridColumn2.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
				gridColumn2.Width = 250;
				GridColumn gridColumn3 = repServiceCondition.Properties.View.Columns.AddField("HEIN_RATIO");
				gridColumn3.Caption = ResourceMessage.TLTT;
				gridColumn3.DisplayFormat.FormatString = "#,##0";
				gridColumn3.DisplayFormat.FormatType = FormatType.Custom;
				gridColumn3.Visible = true;
				gridColumn3.VisibleIndex = 3;
				gridColumn3.Width = 120;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadServiceConditionDefault()
		{
			try
			{
				repServiceCondition.DataSource = lstADO;
				repServiceCondition.DisplayMember = "SERVICE_CONDITION_NAME";
				repServiceCondition.ValueMember = "ID";
				repServiceCondition.TextEditStyle = TextEditStyles.Standard;
				repServiceCondition.PopupFilterMode = PopupFilterMode.Contains;
				repServiceCondition.ImmediatePopup = true;
				repServiceCondition.PopupFormSize = new Size(470, repServiceCondition.PopupFormSize.Height);
				repServiceCondition.View.Columns.Clear();
				repServiceCondition.View.OptionsView.RowAutoHeight = true;
				GridColumn gridColumn = repServiceCondition.View.Columns.AddField("SERVICE_CONDITION_CODE");
				gridColumn.Caption = ResourceMessage.Ma;
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 100;
				gridColumn.ColumnEdit = repConditionCode;
				gridColumn.AppearanceCell.TextOptions.Trimming = Trimming.Word;
				gridColumn.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
				GridColumn gridColumn2 = repServiceCondition.View.Columns.AddField("SERVICE_CONDITION_NAME");
				gridColumn2.Caption = ResourceMessage.Ten;
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.ColumnEdit = repConditionName;
				gridColumn2.AppearanceCell.TextOptions.Trimming = Trimming.Word;
				gridColumn2.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
				gridColumn2.Width = 250;
				GridColumn gridColumn3 = repServiceCondition.View.Columns.AddField("HEIN_RATIO");
				gridColumn3.Caption = ResourceMessage.TLTT;
				gridColumn3.DisplayFormat.FormatString = "#,##0";
				gridColumn3.DisplayFormat.FormatType = FormatType.Custom;
				gridColumn3.Visible = true;
				gridColumn3.VisibleIndex = 3;
				gridColumn3.Width = 120;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewSereServ_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			try
			{
				V_HIS_SERE_SERV_4 v_HIS_SERE_SERV_ = (V_HIS_SERE_SERV_4)gridViewSereServ.GetFocusedRow();
				if (e.Column.FieldName == "PATIENT_TYPE_ID" && v_HIS_SERE_SERV_.PATIENT_TYPE_ID != HisConfigCFG.PatientTypeId__BHYT)
				{
					v_HIS_SERE_SERV_.SERVICE_CONDITION_ID = null;
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
			DevExpress.Utils.SerializableAppearanceObject appearance = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled = new DevExpress.Utils.SerializableAppearanceObject();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HIS.Desktop.Plugins.CallPatientTypeAlter.frmSwapPatientTypeAlter));
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.lblUpdatePatientType = new DevExpress.XtraEditors.LabelControl();
			this.btnUpdatePatientType = new DevExpress.XtraEditors.SimpleButton();
			this.lblNote = new DevExpress.XtraEditors.LabelControl();
			this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
			this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
			this.btnChooSereServ = new DevExpress.XtraEditors.SimpleButton();
			this.gridControlSereServ = new DevExpress.XtraGrid.GridControl();
			this.gridViewSereServ = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColSTT = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColServiceCode = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColServiceName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColServiceUnitName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColAmount = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColPatientTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColAdditionRequire = new DevExpress.XtraGrid.Columns.GridColumn();
			this.grdColumnServiceCondition = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repServiceCondition = new Inventec.Desktop.CustomControl.RepositoryItemCustomGridLookUpEdit();
			this.repositoryItemCustomGridLookUpEdit1View = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
			this.repositoryItemGridLookUpEdit = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
			this.repositoryItemGridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.repositoryItemGridLookUpEditSurcharge = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.repositoryItemGridLookUpEditSurchargeDis = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
			this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.repConditionName = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
			this.repConditionCode = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
			this.repItemServiceCondition = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
			this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar1 = new DevExpress.XtraBars.Bar();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.layoutControl2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.Root).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridControlSereServ).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewSereServ).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repServiceCondition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCustomGridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEdit).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEditSurcharge).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEditSurchargeDis).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repConditionName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repConditionCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repItemServiceCondition).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.lblUpdatePatientType);
			this.layoutControl1.Controls.Add(this.btnUpdatePatientType);
			this.layoutControl1.Controls.Add(this.lblNote);
			this.layoutControl1.Controls.Add(this.layoutControl2);
			this.layoutControl1.Controls.Add(this.btnChooSereServ);
			this.layoutControl1.Controls.Add(this.gridControlSereServ);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 29);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1090, 299);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.lblUpdatePatientType.Appearance.Font = new System.Drawing.Font("Tahoma", 9f);
			this.lblUpdatePatientType.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblUpdatePatientType.Location = new System.Drawing.Point(132, 29);
			this.lblUpdatePatientType.Name = "lblUpdatePatientType";
			this.lblUpdatePatientType.Padding = new System.Windows.Forms.Padding(3, 5, 5, 5);
			this.lblUpdatePatientType.Size = new System.Drawing.Size(956, 24);
			this.lblUpdatePatientType.StyleController = this.layoutControl1;
			this.lblUpdatePatientType.TabIndex = 9;
			this.lblUpdatePatientType.Text = "_______";
			this.btnUpdatePatientType.Location = new System.Drawing.Point(5, 29);
			this.btnUpdatePatientType.Name = "btnUpdatePatientType";
			this.btnUpdatePatientType.Size = new System.Drawing.Size(125, 22);
			this.btnUpdatePatientType.StyleController = this.layoutControl1;
			this.btnUpdatePatientType.TabIndex = 8;
			this.btnUpdatePatientType.Text = "Chuyển toàn bộ dịch vụ";
			this.btnUpdatePatientType.Click += new System.EventHandler(btnUpdatePatientType_Click);
			this.lblNote.Appearance.Font = new System.Drawing.Font("Tahoma", 10f);
			this.lblNote.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblNote.Location = new System.Drawing.Point(5, 5);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(1080, 17);
			this.lblNote.StyleController = this.layoutControl1;
			this.lblNote.TabIndex = 7;
			this.lblNote.Text = "Các chỉ định dịch vụ sau đang sử dụng đối tượng thanh toán khác. Bạn có muốn cập nhật?";
			this.layoutControl2.Location = new System.Drawing.Point(2, 275);
			this.layoutControl2.Name = "layoutControl2";
			this.layoutControl2.Root = this.Root;
			this.layoutControl2.Size = new System.Drawing.Size(985, 22);
			this.layoutControl2.TabIndex = 6;
			this.layoutControl2.Text = "layoutControl2";
			this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.Root.GroupBordersVisible = false;
			this.Root.Location = new System.Drawing.Point(0, 0);
			this.Root.Name = "Root";
			this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.Root.Size = new System.Drawing.Size(985, 22);
			this.Root.TextVisible = false;
			this.btnChooSereServ.Location = new System.Drawing.Point(991, 275);
			this.btnChooSereServ.Name = "btnChooSereServ";
			this.btnChooSereServ.Size = new System.Drawing.Size(97, 22);
			this.btnChooSereServ.StyleController = this.layoutControl1;
			this.btnChooSereServ.TabIndex = 5;
			this.btnChooSereServ.Text = "Lưu (Ctrl S)";
			this.btnChooSereServ.Click += new System.EventHandler(btnChooSereServ_Click);
			this.gridControlSereServ.Location = new System.Drawing.Point(2, 57);
			this.gridControlSereServ.MainView = this.gridViewSereServ;
			this.gridControlSereServ.Name = "gridControlSereServ";
			this.gridControlSereServ.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[7] { this.repositoryItemGridLookUpEdit, this.repositoryItemGridLookUpEditSurcharge, this.repositoryItemGridLookUpEditSurchargeDis, this.repServiceCondition, this.repConditionName, this.repConditionCode, this.repItemServiceCondition });
			this.gridControlSereServ.Size = new System.Drawing.Size(1086, 214);
			this.gridControlSereServ.TabIndex = 4;
			this.gridControlSereServ.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewSereServ });
			this.gridViewSereServ.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[8] { this.gridColSTT, this.gridColServiceCode, this.gridColServiceName, this.gridColServiceUnitName, this.gridColAmount, this.gridColPatientTypeName, this.gridColAdditionRequire, this.grdColumnServiceCondition });
			this.gridViewSereServ.GridControl = this.gridControlSereServ;
			this.gridViewSereServ.Name = "gridViewSereServ";
			this.gridViewSereServ.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
			this.gridViewSereServ.OptionsView.ShowGroupPanel = false;
			this.gridViewSereServ.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(gridViewSereServ_CustomRowCellEdit);
			this.gridViewSereServ.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(gridViewSereServ_SelectionChanged);
			this.gridViewSereServ.ShownEditor += new System.EventHandler(gridViewSereServ_ShownEditor);
			this.gridViewSereServ.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridViewSereServ_CellValueChanged);
			this.gridViewSereServ.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridViewSereServ_CustomUnboundColumnData);
			this.gridColSTT.Caption = "STT";
			this.gridColSTT.FieldName = "STT";
			this.gridColSTT.Name = "gridColSTT";
			this.gridColSTT.OptionsColumn.AllowEdit = false;
			this.gridColSTT.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColSTT.Visible = true;
			this.gridColSTT.VisibleIndex = 0;
			this.gridColSTT.Width = 36;
			this.gridColServiceCode.Caption = "Mã dịch vụ";
			this.gridColServiceCode.FieldName = "TDL_SERVICE_CODE";
			this.gridColServiceCode.Name = "gridColServiceCode";
			this.gridColServiceCode.OptionsColumn.AllowEdit = false;
			this.gridColServiceCode.Visible = true;
			this.gridColServiceCode.VisibleIndex = 1;
			this.gridColServiceCode.Width = 146;
			this.gridColServiceName.AppearanceCell.Options.UseTextOptions = true;
			this.gridColServiceName.AppearanceCell.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.gridColServiceName.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.gridColServiceName.Caption = "Tên dịch vụ";
			this.gridColServiceName.FieldName = "TDL_SERVICE_NAME";
			this.gridColServiceName.Name = "gridColServiceName";
			this.gridColServiceName.OptionsColumn.AllowEdit = false;
			this.gridColServiceName.Visible = true;
			this.gridColServiceName.VisibleIndex = 2;
			this.gridColServiceName.Width = 182;
			this.gridColServiceUnitName.Caption = "Đơn vị tính";
			this.gridColServiceUnitName.FieldName = "SERVICE_UNIT_NAME";
			this.gridColServiceUnitName.Name = "gridColServiceUnitName";
			this.gridColServiceUnitName.OptionsColumn.AllowEdit = false;
			this.gridColServiceUnitName.Visible = true;
			this.gridColServiceUnitName.VisibleIndex = 3;
			this.gridColServiceUnitName.Width = 133;
			this.gridColAmount.Caption = "Số lượng";
			this.gridColAmount.FieldName = "AMOUNT";
			this.gridColAmount.Name = "gridColAmount";
			this.gridColAmount.OptionsColumn.AllowEdit = false;
			this.gridColAmount.Visible = true;
			this.gridColAmount.VisibleIndex = 4;
			this.gridColAmount.Width = 76;
			this.gridColPatientTypeName.Caption = "Đối tượng thanh toán";
			this.gridColPatientTypeName.FieldName = "PATIENT_TYPE_ID";
			this.gridColPatientTypeName.Name = "gridColPatientTypeName";
			this.gridColPatientTypeName.Visible = true;
			this.gridColPatientTypeName.VisibleIndex = 5;
			this.gridColPatientTypeName.Width = 132;
			this.gridColAdditionRequire.Caption = "Đối tượng phụ thu";
			this.gridColAdditionRequire.FieldName = "PRIMARY_PATIENT_TYPE_ID";
			this.gridColAdditionRequire.Name = "gridColAdditionRequire";
			this.gridColAdditionRequire.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColAdditionRequire.Visible = true;
			this.gridColAdditionRequire.VisibleIndex = 6;
			this.gridColAdditionRequire.Width = 122;
			this.grdColumnServiceCondition.Caption = "Điều kiện";
			this.grdColumnServiceCondition.ColumnEdit = this.repServiceCondition;
			this.grdColumnServiceCondition.FieldName = "SERVICE_CONDITION_ID";
			this.grdColumnServiceCondition.Name = "grdColumnServiceCondition";
			this.grdColumnServiceCondition.ToolTip = "Điều kiện thanh toán";
			this.grdColumnServiceCondition.Visible = true;
			this.grdColumnServiceCondition.VisibleIndex = 7;
			this.grdColumnServiceCondition.Width = 241;
			this.repServiceCondition.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.repServiceCondition.Appearance.Options.UseTextOptions = true;
			this.repServiceCondition.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.repServiceCondition.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.repServiceCondition.AutoComplete = false;
			this.repServiceCondition.AutoHeight = false;
			this.repServiceCondition.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.repServiceCondition.Name = "repServiceCondition";
			this.repServiceCondition.NullText = "";
			this.repServiceCondition.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.repServiceCondition.View = this.repositoryItemCustomGridLookUpEdit1View;
			this.repositoryItemCustomGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.repositoryItemCustomGridLookUpEdit1View.Name = "repositoryItemCustomGridLookUpEdit1View";
			this.repositoryItemCustomGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.repositoryItemCustomGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.repositoryItemGridLookUpEdit.AutoHeight = false;
			this.repositoryItemGridLookUpEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.repositoryItemGridLookUpEdit.Name = "repositoryItemGridLookUpEdit";
			this.repositoryItemGridLookUpEdit.NullText = "";
			this.repositoryItemGridLookUpEdit.View = this.repositoryItemGridLookUpEdit1View;
			this.repositoryItemGridLookUpEdit.EditValueChanged += new System.EventHandler(repositoryItemGridLookUpEdit_EditValueChanged);
			this.repositoryItemGridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.repositoryItemGridLookUpEdit1View.Name = "repositoryItemGridLookUpEdit1View";
			this.repositoryItemGridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.repositoryItemGridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.repositoryItemGridLookUpEditSurcharge.AutoHeight = false;
			this.repositoryItemGridLookUpEditSurcharge.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.repositoryItemGridLookUpEditSurcharge.Name = "repositoryItemGridLookUpEditSurcharge";
			this.repositoryItemGridLookUpEditSurcharge.NullText = "";
			this.repositoryItemGridLookUpEditSurcharge.View = this.gridView1;
			this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.repositoryItemGridLookUpEditSurchargeDis.AutoHeight = false;
			this.repositoryItemGridLookUpEditSurchargeDis.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, false, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "", null, null, true)
			});
			this.repositoryItemGridLookUpEditSurchargeDis.Name = "repositoryItemGridLookUpEditSurchargeDis";
			this.repositoryItemGridLookUpEditSurchargeDis.NullText = "";
			this.repositoryItemGridLookUpEditSurchargeDis.View = this.gridView2;
			this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView2.OptionsView.ShowGroupPanel = false;
			this.repConditionName.Appearance.Options.UseTextOptions = true;
			this.repConditionName.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.repConditionName.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.repConditionName.Name = "repConditionName";
			this.repConditionCode.Appearance.Options.UseTextOptions = true;
			this.repConditionCode.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.repConditionCode.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.repConditionCode.Name = "repConditionCode";
			this.repItemServiceCondition.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.repItemServiceCondition.AutoHeight = false;
			this.repItemServiceCondition.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.repItemServiceCondition.Name = "repItemServiceCondition";
			this.repItemServiceCondition.NullText = "";
			this.repItemServiceCondition.View = this.gridView3;
			this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView3.Name = "gridView3";
			this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView3.OptionsView.ShowGroupPanel = false;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[6] { this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.layoutControlItem6 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "layoutControlGroup1";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1090, 299);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.gridControlSereServ;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 55);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1090, 218);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.layoutControlItem2.Control = this.btnChooSereServ;
			this.layoutControlItem2.Location = new System.Drawing.Point(989, 273);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(101, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.layoutControlItem3.Control = this.layoutControl2;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 273);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(989, 26);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.layoutControlItem4.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 15f);
			this.layoutControlItem4.AppearanceItemCaption.Options.UseFont = true;
			this.layoutControlItem4.Control = this.lblNote;
			this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
			this.layoutControlItem4.Size = new System.Drawing.Size(1090, 27);
			this.layoutControlItem4.Text = "\"Các chỉ định dịch vụ sau đang sử dụng đối tượng thanh toán khác. Bạn có muốn cập nhật thông tin không?\"";
			this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextToControlDistance = 0;
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.Control = this.btnUpdatePatientType;
			this.layoutControlItem5.Location = new System.Drawing.Point(0, 27);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2);
			this.layoutControlItem5.Size = new System.Drawing.Size(132, 28);
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextVisible = false;
			this.layoutControlItem6.Control = this.lblUpdatePatientType;
			this.layoutControlItem6.Location = new System.Drawing.Point(132, 27);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem6.Size = new System.Drawing.Size(958, 28);
			this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem6.TextVisible = false;
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
			this.barButtonItem1.Caption = "Lưu (Ctrl S)";
			this.barButtonItem1.Id = 0;
			this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem1.Name = "barButtonItem1";
			this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem1_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(1090, 29);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 328);
			this.barDockControlBottom.Size = new System.Drawing.Size(1090, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 299);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(1090, 29);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 299);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1090, 328);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "frmSwapPatientTypeAlter";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Các dịch vụ";
			base.Load += new System.EventHandler(frmSwapPatientTypeAlter_Load);
			base.Controls.SetChildIndex(this.barDockControlTop, 0);
			base.Controls.SetChildIndex(this.barDockControlBottom, 0);
			base.Controls.SetChildIndex(this.barDockControlRight, 0);
			base.Controls.SetChildIndex(this.barDockControlLeft, 0);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.layoutControl2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.Root).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridControlSereServ).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewSereServ).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repServiceCondition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemCustomGridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEdit).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEditSurcharge).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemGridLookUpEditSurchargeDis).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repConditionName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repConditionCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repItemServiceCondition).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
