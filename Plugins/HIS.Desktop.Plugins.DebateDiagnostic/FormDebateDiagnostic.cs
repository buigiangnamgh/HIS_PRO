using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACS.EFMODEL.DataModels;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Container;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using EMR.EFMODEL.DataModels;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.IsAdmin;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.DebateDiagnostic.ADO;
using HIS.Desktop.Plugins.DebateDiagnostic.Base;
using HIS.Desktop.Plugins.DebateDiagnostic.Config;
using HIS.Desktop.Plugins.DebateDiagnostic.Resources;
using HIS.Desktop.Plugins.DebateDiagnostic.UcDebateDetail;
using HIS.Desktop.Plugins.Library.EmrGenerate;
using HIS.Desktop.Print;
using HIS.Desktop.Utilities;
using HIS.Desktop.Utilities.Extensions;
using HIS.Desktop.Utility;
using HIS.Library.Telemedicine;
using HIS.UC.DateEditor.ADO;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.Resource;
using Inventec.Common.RichEditor;
using Inventec.Common.RichEditor.Base;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.DTO;
using Inventec.Common.String;
using Inventec.Common.ThreadCustom;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.CustomControl;
using Inventec.Desktop.CustomControl.CustomGrid;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;
using MPS;
using MPS.ADO;
using MPS.Processor.Mps000019.PDO;
using MPS.Processor.Mps000020.PDO;
using MPS.Processor.Mps000323.PDO;
using MPS.Processor.Mps000387.PDO;
using MPS.ProcessorBase;
using MPS.ProcessorBase.Core;
using SDA.EFMODEL.DataModels;

namespace HIS.Desktop.Plugins.DebateDiagnostic
{
	public class FormDebateDiagnostic : FormBase
	{
		public enum ModulePrintType
		{
			BIEN_BAN_HOI_CHAN,
			BIEN_BAN_HOI_CHAN_THUOC_DAU_SAO,
			SO_BIEN_BAN_HOI_CHAN,
			HOI_CHAN_PTTT
		}

		private int action = 0;

		private long treatment_id = 0L;

		private CultureInfo cultureLang;

		private List<HisDebateUserADO> lstParticipantDebate;

		private HIS_DEBATE hisDebate;

		private WorkPlaceSDO WorkPlaceSDO;

		private Module moduleData;

		private HIS_SERVICE_REQ examServiceReq;

		internal HIS_SERVICE hisService;

		private List<HIS_DEBATE> medicinePrints;

		private HIS_DEBATE_TEMP datacombox;

		private List<ACS_USER> acsUsers = new List<ACS_USER>();

		private List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();

		private bool isInitTracking;

		internal int actionType = 0;

		private long treatmentId = 0L;

		private int positionHandleControl = -1;

		internal List<long> intructionTimeSelecteds = new List<long>();

		internal List<DateTime?> intructionTimeSelected = new List<DateTime?>();

		private DateTime timeSelested;

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO__Create;

		private List<ControlStateRDO> currentControlStateRDO__Update;

		private DetailProcessor detailProcessor;

		private List<HIS_EXECUTE_ROLE> ListExecuteRole;

		private bool isNotLoadWhileChangeControlStateInFirst;

		private List<ControlStateRDO> currentControlStateRDO;

		private string moduleLink = "HIS.Desktop.Plugins.DebateDiagnostic";

		private List<AcsUserADO> lstReAcsUserADO;

		private List<HIS_DEBATE> lstDebate;

		private List<HisDebateADO> lstHisDebateADO;

		private List<HisDebateInvateUserADO> lstDebateInvateUserADO;

		private UserControl ucDetail;

		private List<InvateADO> lstInvateADO;

		private bool IsNotLoadFirst = false;

		private Timer time = new Timer();

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private GridControl gridControl;

		private GridView gridView;

		private TextEdit txtIcdTextCode;

		private CheckEdit checkEdit;

		private TextEdit txtIcdMain;

		private TextEdit txtRequestLoggin;

		private LayoutControlItem lciRequestContent;

		private LayoutControlItem lciIcdMain;

		private LayoutControlItem lciCheckEdit;

		private LayoutControlItem lciIcdSubCode1;

		private DropDownButton btnPrint;

		private SimpleButton btnSave;

		private LayoutControlItem layoutControlItem11;

		private LayoutControlItem layoutControlItem12;

		private DateEdit dtDebateTime;

		private LayoutControlItem lciDebateTime;

		private GridColumn gridColumnParticipants_Id;

		private GridColumn Gc_LoginName;

		private GridColumn Gc_UserName;

		private GridColumn Gc_President;

		private GridColumn Gc_Secretary;

		private GridColumn Gc_Titles;

		private GridColumn Gc_Add;

		private RepositoryItemCheckEdit CheckEditChuToa;

		private RepositoryItemCheckEdit CheckEditThuKy;

		private RepositoryItemCheckEdit CheckEditChuToaDisable;

		private RepositoryItemCheckEdit CheckEditThuKyDisable;

		private RepositoryItemButtonEdit ButtonAdd;

		private RepositoryItemButtonEdit ButtonDelete;

        private HIS.Desktop.Utilities.Extensions.RepositoryItemCustomGridLookUpEdit LookUpEditUserName;

		private RepositoryItemTextEdit TextEditLoginName;

		private BarManager barManager1;

		private Bar bar1;

		private BarButtonItem barButtonItemSave;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private TextEdit txtIcdTextName;

		private LayoutControlItem lciIcdSubCode;

		private BarButtonItem barButtonItem1;

		private SimpleButton btnSaveTemp;

		private LayoutControlItem layoutControlItem1;

		private TextEdit txtDebateTemp;

		private GridLookUpEdit cboDebateTemp;

		private GridView gridView1;

		private LayoutControlItem LcDebateTemp;

		private LayoutControlItem layoutControlItem4;

		private DateEdit dtInTime;

		private LayoutControlItem layoutControlItem2;

		private GridLookUpEdit cboDepartment;

		private GridView gridView2;

		private DateEdit dtOutTime;

		private LayoutControlItem layoutControlItem5;

		private LayoutControlItem layoutControlItem6;

		private GridLookUpEdit cboRequestLoggin;

		private GridView gridLookUpEdit2View;

		private LayoutControlItem layoutControlItem13;

		private TextEdit txtLocation;

		private LayoutControlItem icdLocation;

		private PanelControl panelControl1;

		private LayoutControlItem layoutControlItem3;

		private GridLookUpEdit cboIcdMain;

		private GridView gridLookUpEdit3View;

		private TextEdit icdMainText;

		private DXValidationProvider dxValidationProvider1;

		private BarButtonItem barButtonItem2;

		private GridLookUpEdit cboDebateType;

		private GridView gridLookUpEdit1View;

		private LayoutControlItem layoutControlItem15;

		private CheckEdit chkAutoSign;

		private LayoutControlItem lciAutoSign;

		private CheckEdit chkAutoCreateEmr;

		private LayoutControlItem lciAutoCreateEmr;

		private CheckEdit ChkOther;

		private CheckEdit ChkPttt;

		private CheckEdit CheckThuoc;

		private PanelControl panelDetail;

		private LayoutControlItem layoutControlItem9;

		private LayoutControlItem layoutControlItem10;

		private LayoutControlItem layoutControlItem14;

		private LayoutControlItem layoutControlItem16;

		private LayoutControlItem layoutControlItem8;

		private EmptySpaceItem emptySpaceItem3;

        private HIS.Desktop.Utilities.Extensions.RepositoryItemCustomGridLookUpEdit repositoryItemCboExecuteRole;

        private HIS.Desktop.Utilities.Extensions.CustomGridView repositoryItemCustomGridLookUpEdit1View;

		private GridLookUpEdit cboPhieuDieuTri;

		private GridView gridView3;

		internal LayoutControlItem layoutControlItem7;

		private GridLookUpEdit cboBienBanHoiChanCu;

		private GridView gridView4;

		private LayoutControlItem layoutControlItem17;

		private EmptySpaceItem emptySpaceItem2;

		private MyGridControl gridControl1;

		private MyGridView gridView5;

		private GridColumn gridColumn1;

		private RepositoryItemButtonEdit rebtnFeedBackEnable;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private GridColumn gridColumn4;

		private GridColumn gridColumn5;

		private GridColumn gridColumn6;

		private GridColumn gridColumn7;

		private GridColumn gridColumn8;

		private RepositoryItemButtonEdit rebtnFeedBackDisable;

		private LayoutControlItem layoutControlItem18;

		private RepositoryItemButtonEdit rebtnAddInvateUser;

		private RepositoryItemButtonEdit rebtnMinusInvateUser;

		private RepositoryItemCheckEdit rechkThuKy;

		private RepositoryItemCheckEdit rechkChuToa;

		private SimpleButton btnTreatmentHistory;

		private LayoutControlItem layoutControlItem19;

		private RepositoryItemComboBox recboThamGia;

		private RepositoryItemTextEdit reTxtDisable;

        private Inventec.Desktop.CustomControl.CustomGrid.RepositoryItemCustomGridLookUpEdit regluThamGia;

        private Inventec.Desktop.CustomControl.CustomGrid.CustomGridView customGridView1;

        private HIS.Desktop.Utilities.Extensions.RepositoryItemCustomGridLookUpEdit LookUpEditUserNameInvate;

        private HIS.Desktop.Utilities.Extensions.CustomGridView customGridView2;

        private HIS.Desktop.Utilities.Extensions.RepositoryItemCustomGridLookUpEdit recboExecuteRole;

        private HIS.Desktop.Utilities.Extensions.CustomGridView customGridView3;

		private GridColumn gcLoginName;

		private RepositoryItemCheckEdit rechkThuKyDisable;

		private RepositoryItemCheckEdit rechkChuToaDisable;

		private RepositoryItemButtonEdit reTxtComment;

		private PopupControlContainer popupControlContainer1;

		private LayoutControl layoutControl2;

		private SimpleButton btnCancel;

		private SimpleButton btnOk;

		private MemoEdit memContent;

		private LayoutControlGroup layoutControlGroup2;

		private LayoutControlItem layoutControlItem20;

		private LayoutControlItem layoutControlItem21;

		private LayoutControlItem layoutControlItem22;

		private EmptySpaceItem emptySpaceItem1;

		private GridColumn gridColumn9;

		private GridColumn gridColumn10;

		private GridColumn colFeedBackUnb;

		private GridColumn colIS_PARTICIPATION_strUnb;

		private GridColumn colUSERNAMEUnb;

		private GridColumn colPRESIDENTUnb;

		private GridColumn colSECRETARYUnb;

		private GridColumn colEXECUTE_ROLE_IDUnb;

		private GridColumn colCOMMENT_DOCTORUnb;

		private GridColumn colBtnDeleteInvateUserUnb;

		private SimpleButton btnSendTMP;

		private LayoutControlItem layoutControlItem23;

		private LayoutControlItem layoutControlItem24;

		private GridLookUpEdit cboDebateReason;

		private GridView gridView6;

		private GridColumn Gc_OutOfHospital;

		private RepositoryItemCheckEdit CheckEditBsNgoaiVien;

		private RepositoryItemTextEdit TextEditUserName;

		private RepositoryItemTextEdit TextEditLoginNameDis;

		private CheckEdit chkLockInfor;

		private LayoutControlItem layoutControlItem25;

		private CheckEdit chkAutoCreateTracking;

		private LayoutControlItem layoutControlItem26;

		private EmptySpaceItem emptySpaceItem4;

		private bool isCreateEmrDocument = false;

		internal HIS_DEBATE currentHisDebate { get; set; }

		internal V_HIS_TREATMENT vHisTreatment { get; set; }

		private HIS_TRACKING tracking { get; set; }

		private List<TrackingADO> trackingADOs { get; set; }

		internal long InstructionTime { get; set; }

		public List<HIS_ICD> Icds { get; private set; }

		public List<V_HIS_EMPLOYEE> Employee { get; private set; }

		public FormDebateDiagnostic(Module module)
			: base(module)
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			InitializeComponent();
			try
			{
				ResourceLanguageManager.InitResourceLanguageManager();
				cultureLang = LanguageManager.GetCulture();
				WorkPlaceSDO = new WorkPlaceSDO();
				moduleData = module;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public FormDebateDiagnostic(HIS_SERVICE_REQ exam, List<HIS_DEBATE> medicinePrints, Module module)
			: this(module)
		{
			try
			{
				((Control)(object)this).Text = module.text;
				treatment_id = exam.TREATMENT_ID;
				examServiceReq = exam;
				this.medicinePrints = medicinePrints;
				action = 1;
				moduleData = module;
				btnPrint.Enabled = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public FormDebateDiagnostic(HIS_SERVICE _hisService, List<HIS_DEBATE> medicinePrints, Module module, long treatmentId)
			: this(module)
		{
			try
			{
				((Control)(object)this).Text = module.text;
				treatment_id = treatmentId;
				hisService = _hisService;
				this.medicinePrints = medicinePrints;
				action = 1;
				moduleData = module;
				btnPrint.Enabled = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public FormDebateDiagnostic(TreatmentLogADO _treatmentAdo, Module module, List<HIS_DEBATE> medicinePrints)
			: this(module)
		{
			try
			{
				((Control)(object)this).Text = module.text;
				treatment_id = _treatmentAdo.TreatmentId;
				this.medicinePrints = medicinePrints;
				action = 1;
				moduleData = module;
				btnPrint.Enabled = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public FormDebateDiagnostic(HIS_DEBATE debate, Module module, List<HIS_DEBATE> medicinePrints)
			: this(module)
		{
			try
			{
				((Control)(object)this).Text = module.text;
				currentHisDebate = debate;
				treatment_id = debate.TREATMENT_ID;
				hisDebate = debate;
				this.medicinePrints = medicinePrints;
				action = 2;
				moduleData = module;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadAcsUser()
		{
			try
			{
				acsUsers = BackendDataWorker.Get<ACS_USER>();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadEmployee()
		{
			try
			{
				Employee = BackendDataWorker.Get<V_HIS_EMPLOYEE>();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDepartment()
		{
			try
			{
				listDepartment = BackendDataWorker.Get<HIS_DEPARTMENT>();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadExecuteRole()
		{
			try
			{
				ListExecuteRole = BackendDataWorker.Get<HIS_EXECUTE_ROLE>();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadIcd()
		{
			try
			{
				Icds = GlobalStore.HisIcds;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FormDebateDiagnostic_Load(object sender, EventArgs e)
		{
			try
			{
				IsNotLoadFirst = true;
				LogSystem.Debug(LogUtil.TraceData("currentHisDebate___:", (object)currentHisDebate));
				WaitingManager.Show();
				SetIcon();
				HisConfigCFG.LoadConfig();
				lciAutoSign.Visibility = ((!HisConfigCFG.IsUseSignEmr) ? LayoutVisibility.Never : LayoutVisibility.Always);
				lciAutoCreateEmr.Visibility = ((!HisConfigCFG.IsUseSignEmr) ? LayoutVisibility.Never : LayoutVisibility.Always);
				WorkPlaceSDO = WorkPlace.WorkPlaceSDO.Where((WorkPlaceSDO o) => o.RoomId == moduleData.RoomId && o.RoomTypeId == moduleData.RoomTypeId).FirstOrDefault();
				List<Action> list = new List<Action>();
				list.Add(LoadAcsUser);
				list.Add(LoadEmployee);
				list.Add(LoadDepartment);
				list.Add(LoadExecuteRole);
				list.Add(LoadIcd);
				ThreadCustomManager.MultipleThreadWithJoin(list);
				InitComboDebateTemp();
				InitControlState();
				LoadKeysFromlanguage();
				SetDefaultValueControl();
				FillDataToParticipants();
				FillDataToInvateUser();
				DataToComboChuanDoan();
				LoadDataToGridParticipants();
				LoadDataCombocboRequestLoggin();
				LoadDataCombocboDebateType();
				InitComboDebateReason();
				LoadDataComboDepartment();
				ProcessLoadExecuteRole();
				if (currentHisDebate != null && currentHisDebate.CONTENT_TYPE.HasValue)
				{
					if (currentHisDebate.CONTENT_TYPE.Value == 1)
					{
						ChkOther.Checked = true;
					}
					else if (currentHisDebate.CONTENT_TYPE.Value == 3)
					{
						ChkPttt.Checked = true;
					}
					else
					{
						CheckThuoc.Checked = true;
					}
				}
				else if (medicinePrints != null)
				{
					CheckThuoc.Checked = true;
				}
				else
				{
					ChkOther.Checked = true;
				}
				LoadComboThamGia();
				LoadData(treatment_id);
				LoadDataTracking();
				if (HisConfigCFG.DebateDiagnostic_IsDefaultTracking)
				{
				}
				if ((action == 2 || action == 3) && currentHisDebate != null)
				{
					LoadDataDebateDiagnostic(currentHisDebate);
					LoadDataInviteDebate(currentHisDebate);
					if (detailProcessor != null)
					{
						detailProcessor.SetData(GetTypeDetail(), currentHisDebate);
					}
				}
				else if (examServiceReq != null && examServiceReq.SERVICE_REQ_TYPE_ID == 1)
				{
					LoadDataDebateDiagnostic(examServiceReq);
					if (detailProcessor != null)
					{
						detailProcessor.SetData(GetTypeDetail(), examServiceReq);
					}
				}
				time.Tick += Time_Tick;
				time.Interval = 100;
				LoadDataComboPhieuChuanDoanCu();
				if (hisService != null)
				{
					ChkOther.Checked = true;
				}
				time.Start();
				ICDValidationRule();
				WaitingManager.Hide();
				IsNotLoadFirst = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				WaitingManager.Hide();
			}
		}

		private void Time_Tick(object sender, EventArgs e)
		{
			ValidationControl();
			FillDataToButtonPrint();
			DisableControlItem();
			EnableColumnAddDelete();
		}

		private async Task InitComboDebateReason()
		{
			try
			{
				ControlEditorADO controlEditorADO = new ControlEditorADO("DEBATE_REASON_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("DEBATE_REASON_CODE", "", 100, 1),
					new ColumnInfo("DEBATE_REASON_NAME", "", 250, 2)
				}, false, 350);
				ControlEditorLoader.Load((object)cboDebateReason, (object)(from o in BackendDataWorker.Get<HIS_DEBATE_REASON>()
					where o.IS_ACTIVE == 1
					select o).ToList(), controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void InitControlState()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			try
			{
				controlStateWorker = new ControlStateWorker();
				currentControlStateRDO__Create = controlStateWorker.GetData("HIS.Desktop.Plugins.Debate");
				currentControlStateRDO__Update = controlStateWorker.GetData("HIS.Desktop.Plugins.DebateDiagnostic");
				currentControlStateRDO = controlStateWorker.GetData(moduleLink);
				if (currentControlStateRDO__Create != null && currentControlStateRDO__Create.Count > 0)
				{
					foreach (ControlStateRDO item in currentControlStateRDO__Create)
					{
						if (item.KEY == "chkAutoSign")
						{
							chkAutoSign.Checked = item.VALUE == "1";
						}
						if (item.KEY == "chkAutoCreateEmr")
						{
							chkAutoCreateEmr.Checked = item.VALUE == "1";
						}
					}
				}
				if (currentControlStateRDO__Update != null && currentControlStateRDO__Update.Count > 0 && !chkAutoSign.Checked)
				{
					foreach (ControlStateRDO item2 in currentControlStateRDO__Update)
					{
						if (item2.KEY == "chkAutoSign")
						{
							chkAutoSign.Checked = item2.VALUE == "1";
						}
					}
				}
				if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
				{
					return;
				}
				foreach (ControlStateRDO item3 in currentControlStateRDO)
				{
					if (item3.KEY == chkLockInfor.Name)
					{
						chkLockInfor.Checked = item3.VALUE == "1";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationControl()
		{
			ValidationControlMaxLength(icdMainText, 500, false);
			ValidationControlMaxLength(txtIcdTextName, 4000, false);
			ValidControlTime();
		}

		private void ValidControlTime()
		{
			try
			{
				TimeValidationRule timeValidationRule = new TimeValidationRule();
				timeValidationRule.DateEdit1 = dtInTime;
				timeValidationRule.DateEdit2 = dtOutTime;
				dxValidationProvider1.SetValidationRule(dtOutTime, timeValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationControlMaxLength(BaseEdit control, int? maxLength, bool IsRequired)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				ControlMaxLengthValidationRule val = new ControlMaxLengthValidationRule();
				val.editor = control;
				val.maxLength = maxLength;
				val.IsRequired = IsRequired;
				int? num = maxLength;
				((ValidationRuleBase)(object)val).ErrorText = "Nhập quá kí tự cho phép [" + num + "]";
				((ValidationRuleBase)(object)val).ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, (ValidationRuleBase)(object)val);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ICDValidationRule()
		{
			try
			{
				ICDValidationRule iCDValidationRule = new ICDValidationRule();
				iCDValidationRule.editor = txtIcdMain;
				iCDValidationRule.checkEdit = checkEdit;
				iCDValidationRule.cboICD = cboIcdMain;
				iCDValidationRule.maxLength = 20;
				iCDValidationRule.IsRequired = false;
				iCDValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtIcdMain, iCDValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task LoadDataComboPhieuChuanDoanCu()
		{
			try
			{
				lstHisDebateADO = new List<HisDebateADO>();
				Action myaction = delegate
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Expected O, but got Unknown
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000d: Expected O, but got Unknown
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					CommonParam val = new CommonParam();
					HisDebateFilter val2 = new HisDebateFilter();
					val2.TREATMENT_ID = treatment_id;
					lstDebate = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_DEBATE>>("api/HisDebate/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				if (lstDebate == null)
				{
					return;
				}
				lstDebate = lstDebate.OrderByDescending((HIS_DEBATE o) => o.DEBATE_TIME).ToList();
				foreach (HIS_DEBATE item in lstDebate)
				{
					HisDebateADO ADO = new HisDebateADO(item);
					lstHisDebateADO.Add(ADO);
				}
				ControlEditorADO controlEditorADO = new ControlEditorADO("ContentTypeName", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("REQUEST_LOGINNAME", "Người tạo", 100, 1),
					new ColumnInfo("HisDebateTimeString", "Thời gian", 200, 2),
					new ColumnInfo("LOCATION", "Khoa tạo", 150, 3),
					new ColumnInfo("ContentTypeName", "Loại hội chẩn", 150, 4)
				}, true, 400);
				ControlEditorLoader.Load((object)cboBienBanHoiChanCu, (object)lstHisDebateADO, controlEditorADO);
				cboBienBanHoiChanCu.Properties.ImmediatePopup = true;
				cboBienBanHoiChanCu.Properties.Buttons[1].Visible = false;
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task LoadDataComboDepartment()
		{
			try
			{
				new CommonParam();
				ControlEditorADO controlEditorADO = new ControlEditorADO("DEPARTMENT_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("DEPARTMENT_CODE", "Mã", 100, 1),
					new ColumnInfo("DEPARTMENT_NAME", "Tên", 250, 2)
				}, true, 350);
				ControlEditorLoader.Load((object)cboDepartment, (object)listDepartment, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private async Task LoadDataCombocboRequestLoggin()
		{
			ControlEditorADO controlEditorADO2 = new ControlEditorADO("USERNAME", "LOGINNAME", new List<ColumnInfo>
			{
				new ColumnInfo("LOGINNAME", "LOGINNAME", 50, 1),
				new ColumnInfo("USERNAME", "USERNAME", 150, 2)
			}, true, 200);
			ControlEditorLoader.Load((object)cboRequestLoggin, (object)acsUsers, controlEditorADO2);
		}

		private List<HIS_DEBATE_TYPE> GetDebateTypeDb()
		{
			List<HIS_DEBATE_TYPE> list = null;
			try
			{
				list = BackendDataWorker.Get<HIS_DEBATE_TYPE>();
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Warn(ex);
			}
			return list;
		}

		private async Task LoadDataCombocboDebateType()
		{
			List<HIS_DEBATE_TYPE> Data = null;
			Action myaction = delegate
			{
				Data = BackendDataWorker.Get<HIS_DEBATE_TYPE>();
			};
			Task task = new Task(myaction);
			task.Start();
			await task;
			ControlEditorADO controlEditorADO2 = new ControlEditorADO("DEBATE_TYPE_NAME", "ID", new List<ColumnInfo>
			{
				new ColumnInfo("DEBATE_TYPE_CODE", "Mã", 50, 1),
				new ColumnInfo("DEBATE_TYPE_NAME", "Tên", 150, 2)
			}, true, 200);
			ControlEditorLoader.Load((object)cboDebateType, (object)Data, controlEditorADO2);
		}

		private void SetIcon()
		{
			try
			{
				((Form)this).Icon = Icon.ExtractAssociatedIcon(Path.Combine(ApplicationStoreLocation.ApplicationDirectory, ConfigurationSettings.AppSettings["Inventec.Desktop.Icon"]));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadKeysFromlanguage()
		{
			try
			{
				btnPrint.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__BTN_PRINT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				btnSave.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__BTN_SAVE", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				btnSaveTemp.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__BTN_SAVE_TEMP", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				Gc_LoginName.Caption = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__GC_LOGIN_NAME", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				Gc_President.Caption = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__GC_PRESIDENT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				Gc_Secretary.Caption = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__GC_SECRETARY", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				Gc_Titles.Caption = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__GC_TITLES", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				Gc_UserName.Caption = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__GC_USER_NAME", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciCheckEdit.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_CHECK_EDIT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciRequestContent.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_REQUEST_CONTENT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciDebateTime.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_DEBATE_TIME", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciIcdMain.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_ICD_MAIN", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciIcdSubCode.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_ICD_TEXT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				lciIcdSubCode1.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__LCI_ICD_TEXT", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				txtIcdTextName.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__TXT_SECONDARY_ICD__NULL_VALUE", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				icdLocation.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__LOCALTION", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				layoutControlItem2.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__InTime", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__OutTime", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
				layoutControlItem6.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FacultyDepartment", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetDefaultValueControl()
		{
			try
			{
				btnSendTMP.Enabled = false;
				dtDebateTime.EditValue = DateTime.Now;
				string text = DateTime.Now.ToString("dd/MM");
				txtDebateTemp.Focus();
				txtDebateTemp.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void FillDataToParticipants()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			try
			{
				if (action == 1 || action == 2)
				{
					lstParticipantDebate = new List<HisDebateUserADO>();
					CommonParam val = new CommonParam();
					HisDebateUserADO hisDebateUserADO = new HisDebateUserADO();
					hisDebateUserADO.Action = 1;
					lstParticipantDebate.Add(hisDebateUserADO);
					gridControl.DataSource = lstParticipantDebate;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataToInvateUser()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			try
			{
				if (action == 1 || action == 2)
				{
					lstDebateInvateUserADO = new List<HisDebateInvateUserADO>();
					CommonParam val = new CommonParam();
					HisDebateInvateUserADO hisDebateInvateUserADO = new HisDebateInvateUserADO();
					hisDebateInvateUserADO.Action = 1;
					lstDebateInvateUserADO.Add(hisDebateInvateUserADO);
					((GridControl)(object)gridControl1).DataSource = lstDebateInvateUserADO;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task LoadData(long treatmentId)
		{
			try
			{
				CommonParam param = new CommonParam();
				HisTreatmentViewFilter hisTreatmentFilter = new HisTreatmentViewFilter();
				((FilterBase)hisTreatmentFilter).ID = treatmentId;
				List<V_HIS_TREATMENT> hisTreatmentList = ((AdapterBase)new BackendAdapter(param)).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)hisTreatmentFilter, param);
				if (hisTreatmentList == null || hisTreatmentList.Count <= 0)
				{
					return;
				}
				vHisTreatment = hisTreatmentList.FirstOrDefault();
				LoadcboIcdsValue();
				dtInTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(vHisTreatment.IN_TIME);
				dtOutTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(vHisTreatment.OUT_TIME.GetValueOrDefault());
				long departmentID = vHisTreatment.LAST_DEPARTMENT_ID.GetValueOrDefault();
				if (departmentID > 0)
				{
					cboDepartment.EditValue = departmentID;
					cboDepartment.Enabled = false;
					List<HIS_DEPARTMENT> departments = listDepartment.Where((HIS_DEPARTMENT dp) => dp.ID == departmentID).ToList();
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<HIS_DEPARTMENT>>((Expression<Func<List<HIS_DEPARTMENT>>>)(() => departments)), (object)departments));
					if (departments != null && departments.Count > 0)
					{
						txtLocation.EditValue = departments.FirstOrDefault().DEPARTMENT_NAME;
					}
				}
				string loginName = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
				if (!string.IsNullOrEmpty(loginName))
				{
					txtRequestLoggin.EditValue = loginName;
					cboRequestLoggin.EditValue = loginName;
					txtRequestLoggin.Enabled = false;
					cboRequestLoggin.Enabled = false;
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<HIS_DEBATE>>((Expression<Func<List<HIS_DEBATE>>>)(() => medicinePrints)), (object)medicinePrints));
				if (medicinePrints != null && medicinePrints.Count > 0)
				{
					LogSystem.Debug("LoadData.1");
					if (detailProcessor != null)
					{
						LogSystem.Debug("LoadData.2");
						detailProcessor.SetDataMedicine(medicinePrints[0]);
						LogSystem.Debug("LoadData.3");
					}
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Error(ex2);
			}
		}

		private void LoadcboIcdsValue()
		{
			try
			{
				txtIcdMain.Text = vHisTreatment.ICD_CODE;
				if (!string.IsNullOrEmpty(vHisTreatment.ICD_NAME))
				{
					checkEdit.Checked = true;
					icdMainText.Text = vHisTreatment.ICD_NAME;
					cboIcdMain.EditValue = vHisTreatment.ICD_CODE;
				}
				else
				{
					cboIcdMain.EditValue = vHisTreatment.ICD_CODE;
				}
				txtIcdTextName.Text = vHisTreatment.ICD_TEXT;
				txtIcdTextCode.Text = vHisTreatment.ICD_SUB_CODE;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private async Task DataToComboChuanDoan()
		{
			cboIcdMain.Properties.DataSource = Icds;
			cboIcdMain.Properties.DisplayMember = "ICD_NAME";
			cboIcdMain.Properties.ValueMember = "ICD_CODE";
			cboIcdMain.Properties.TextEditStyle = TextEditStyles.Standard;
			cboIcdMain.Properties.PopupFilterMode = PopupFilterMode.Contains;
			cboIcdMain.Properties.ImmediatePopup = true;
			cboIcdMain.ForceInitialize();
			cboIcdMain.Properties.View.Columns.Clear();
			GridColumn aColumnCode = cboIcdMain.Properties.View.Columns.AddField("ICD_CODE");
			aColumnCode.Caption = "Mã";
			aColumnCode.Visible = true;
			aColumnCode.VisibleIndex = 1;
			aColumnCode.Width = 100;
			GridColumn aColumnName = cboIcdMain.Properties.View.Columns.AddField("ICD_NAME");
			aColumnName.Caption = "Tên";
			aColumnName.Visible = true;
			aColumnName.VisibleIndex = 2;
			aColumnName.Width = 200;
		}

		private async Task LoadDataToGridParticipants()
		{
			try
			{
				lstReAcsUserADO = new List<AcsUserADO>();
				Action myaction = delegate
				{
					foreach (ACS_USER item in acsUsers)
					{
						if (!string.IsNullOrEmpty(item.USERNAME) && item.IS_ACTIVE == 1)
						{
							AcsUserADO acsUserADO = new AcsUserADO(item);
							V_HIS_EMPLOYEE val = Employee.FirstOrDefault((V_HIS_EMPLOYEE o) => o.LOGINNAME == item.LOGINNAME);
							if (val != null)
							{
								acsUserADO.DOB = val.DOB;
								acsUserADO.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(val.DOB.GetValueOrDefault());
								acsUserADO.DIPLOMA = val.DIPLOMA;
								acsUserADO.DEPARTMENT_CODE = val.DEPARTMENT_CODE;
								acsUserADO.DEPARTMENT_ID = val.DEPARTMENT_ID;
								acsUserADO.DEPARTMENT_NAME = val.DEPARTMENT_NAME;
							}
							lstReAcsUserADO.Add(acsUserADO);
						}
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("USERNAME", "LOGINNAME", new List<ColumnInfo>
				{
					new ColumnInfo("LOGINNAME", "Tên đăng nhập", 150, 1),
					new ColumnInfo("USERNAME", "Họ tên", 250, 2),
					new ColumnInfo("DOB_STR", "Ngày sinh", 150, 3),
					new ColumnInfo("DIPLOMA", "CCHN", 150, 4),
					new ColumnInfo("DEPARTMENT_NAME", "Khoa phòng", 200, 5)
				}, true, 800);
				ControlEditorLoader.Load((object)LookUpEditUserName, (object)lstReAcsUserADO.Where((AcsUserADO o) => ((ACS_USER)o).IS_ACTIVE == 1).ToList(), controlEditorADO);
				LoadDataToGridUserInvate();
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void LoadDataToGridUserInvate()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			try
			{
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("LOGINNAME", "Tên đăng nhập", 150, 1));
				list.Add(new ColumnInfo("USERNAME", "Họ tên", 250, 2));
				list.Add(new ColumnInfo("DOB_STR", "Ngày sinh", 150, 3));
				list.Add(new ColumnInfo("DIPLOMA", "CCHN", 150, 4));
				list.Add(new ColumnInfo("DEPARTMENT_NAME", "Khoa phòng", 200, 5));
				ControlEditorADO val = new ControlEditorADO("USERNAME", "LOGINNAME", list, true, 800);
				ControlEditorLoader.Load((object)LookUpEditUserNameInvate, (object)lstReAcsUserADO.Where((AcsUserADO o) => ((ACS_USER)o).IS_ACTIVE == 1).ToList(), val);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataDebateDiagnostic(HIS_DEBATE hisDebate)
		{
			try
			{
				dtInTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(hisDebate.TREATMENT_FROM_TIME.GetValueOrDefault());
				dtOutTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(hisDebate.TREATMENT_TO_TIME.GetValueOrDefault());
				cboDepartment.EditValue = hisDebate.DEPARTMENT_ID;
				cboRequestLoggin.EditValue = hisDebate.REQUEST_LOGINNAME;
				txtRequestLoggin.EditValue = hisDebate.REQUEST_LOGINNAME;
				txtLocation.EditValue = hisDebate.LOCATION;
				checkEdit.Checked = false;
				dtDebateTime.EditValue = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(hisDebate.DEBATE_TIME.GetValueOrDefault());
				if (hisDebate.TRACKING_ID.HasValue)
				{
					cboPhieuDieuTri.EditValue = hisDebate.TRACKING_ID;
				}
				cboDebateType.EditValue = hisDebate.DEBATE_TYPE_ID;
				LogSystem.Debug("DEBATE_REASON_ID___2" + hisDebate.DEBATE_REASON_ID);
				cboDebateReason.EditValue = hisDebate.DEBATE_REASON_ID;
				txtIcdTextName.Text = hisDebate.ICD_TEXT;
				txtIcdTextCode.Text = hisDebate.ICD_SUB_CODE;
				if (!string.IsNullOrEmpty(hisDebate.ICD_CODE))
				{
					HIS_ICD val = GlobalStore.HisIcds.Where((HIS_ICD o) => o.ICD_CODE == hisDebate.ICD_CODE).FirstOrDefault();
					if (val != null)
					{
						cboIcdMain.EditValue = val.ICD_CODE;
						txtIcdMain.Text = val.ICD_CODE;
					}
				}
				if (!string.IsNullOrEmpty(hisDebate.ICD_NAME))
				{
					checkEdit.Checked = true;
					icdMainText.Text = hisDebate.ICD_NAME;
				}
				LoadDebateUser(hisDebate);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task LoadDebateUser(HIS_DEBATE hisDebate)
		{
			try
			{
				List<HisDebateUserADO> lstHisDebateUserADO = new List<HisDebateUserADO>();
				Action myaction = delegate
				{
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Expected O, but got Unknown
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0049: Expected O, but got Unknown
					//IL_0044: Unknown result type (might be due to invalid IL or missing references)
					//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b3: Expected O, but got Unknown
					//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
					//IL_01da: Expected O, but got Unknown
					//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
					if (hisDebate.ID > 0)
					{
						HisDebateUserFilter val = new HisDebateUserFilter();
						val.DEBATE_ID = hisDebate.ID;
						List<HIS_DEBATE_USER> list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
						if (list != null && list.Count > 0)
						{
							foreach (HIS_DEBATE_USER item in list)
							{
								HisDebateUserADO hisDebateUserADO = new HisDebateUserADO();
								DataObjectMapper.Map<HisDebateUserADO>((object)hisDebateUserADO, (object)item);
								if (item.IS_PRESIDENT == 1)
								{
									hisDebateUserADO.PRESIDENT = true;
								}
								if (item.IS_SECRETARY == 1)
								{
									hisDebateUserADO.SECRETARY = true;
								}
								if (item.IS_OUT_OF_HOSPITAL == 1)
								{
									hisDebateUserADO.OUT_OF_HOSPITAL = true;
								}
								hisDebateUserADO.Action = 2;
								lstHisDebateUserADO.Add(hisDebateUserADO);
							}
							return;
						}
						HisDebateInviteUserFilter val2 = new HisDebateInviteUserFilter();
						val2.DEBATE_ID = hisDebate.ID;
						List<HIS_DEBATE_INVITE_USER> list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_INVITE_USER>>("api/HisDebateInviteUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
						if (list2 != null && list2.Count > 0)
						{
							foreach (HIS_DEBATE_INVITE_USER item2 in list2)
							{
								if (item2.IS_PARTICIPATION == 1)
								{
									HisDebateUserADO hisDebateUserADO2 = new HisDebateUserADO();
									DataObjectMapper.Map<HisDebateUserADO>((object)hisDebateUserADO2, (object)item2);
									if (item2.IS_PRESIDENT == 1)
									{
										hisDebateUserADO2.PRESIDENT = true;
									}
									if (item2.IS_SECRETARY == 1)
									{
										hisDebateUserADO2.SECRETARY = true;
									}
									hisDebateUserADO2.Action = 2;
									lstHisDebateUserADO.Add(hisDebateUserADO2);
								}
							}
						}
					}
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				if (lstHisDebateUserADO != null && lstHisDebateUserADO.Count > 0)
				{
					lstHisDebateUserADO[0].Action = 1;
					gridControl.DataSource = lstHisDebateUserADO;
					lstParticipantDebate = lstHisDebateUserADO;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Error(ex2);
			}
		}

		private async Task LoadDataInviteDebate(HIS_DEBATE hisDebate)
		{
			try
			{
				List<HisDebateInvateUserADO> lstHisDebateInviteUserADO = new List<HisDebateInvateUserADO>();
				HisDebateInviteUserFilter hisDebateInviteUserFilter = new HisDebateInviteUserFilter();
				hisDebateInviteUserFilter.DEBATE_ID = hisDebate.ID;
				List<HIS_DEBATE_INVITE_USER> lstHisDebateInviteUser = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_INVITE_USER>>("api/HisDebateInviteUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)hisDebateInviteUserFilter, (CommonParam)null);
				if (lstHisDebateInviteUser != null && lstHisDebateInviteUser.Count > 0)
				{
					foreach (HIS_DEBATE_INVITE_USER item_DebateUser in lstHisDebateInviteUser)
					{
						HisDebateInvateUserADO hisDebateUserADO = new HisDebateInvateUserADO();
						DataObjectMapper.Map<HisDebateInvateUserADO>((object)hisDebateUserADO, (object)item_DebateUser);
						if (item_DebateUser.IS_PRESIDENT == 1)
						{
							hisDebateUserADO.PRESIDENT = true;
						}
						if (item_DebateUser.IS_SECRETARY == 1)
						{
							hisDebateUserADO.SECRETARY = true;
						}
						hisDebateUserADO.Action = 2;
						if (!(item_DebateUser.LOGINNAME == ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
						{
							if (item_DebateUser.IS_PARTICIPATION.HasValue)
							{
								hisDebateUserADO.IS_PARTICIPATION_str = lstInvateADO.First((InvateADO o) => o.ID == item_DebateUser.IS_PARTICIPATION).NAME;
							}
						}
						else if (item_DebateUser.IS_PARTICIPATION.HasValue)
						{
							hisDebateUserADO.IS_PARTICIPATION_str = item_DebateUser.IS_PARTICIPATION.ToString();
						}
						lstHisDebateInviteUserADO.Add(hisDebateUserADO);
					}
				}
				if (lstHisDebateInviteUserADO != null && lstHisDebateInviteUserADO.Count > 0)
				{
					lstHisDebateInviteUserADO[0].Action = 1;
					((BaseView)(object)gridView5).GridControl.DataSource = lstHisDebateInviteUserADO;
					lstDebateInvateUserADO = lstHisDebateInviteUserADO;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void LoadDataDebateDiagnostic(HIS_SERVICE_REQ hisDebate)
		{
			try
			{
				if (!string.IsNullOrEmpty(hisDebate.ICD_NAME))
				{
					checkEdit.Checked = true;
					icdMainText.Text = hisDebate.ICD_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void DisableControlItem()
		{
			try
			{
				if (action == 3 || vHisTreatment.IS_PAUSE == 1)
				{
					dtDebateTime.ReadOnly = true;
					txtRequestLoggin.ReadOnly = true;
					gridColumnParticipants_Id.OptionsColumn.AllowEdit = false;
					Gc_LoginName.OptionsColumn.AllowEdit = false;
					Gc_UserName.OptionsColumn.AllowEdit = false;
					Gc_President.OptionsColumn.AllowEdit = false;
					Gc_Secretary.OptionsColumn.AllowEdit = false;
					Gc_Titles.OptionsColumn.AllowEdit = false;
					Gc_Add.OptionsColumn.AllowEdit = false;
					btnSave.Enabled = false;
					if (detailProcessor != null)
					{
						detailProcessor.DisableControlItem(GetTypeDetail());
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void loadIcdDebate()
		{
			try
			{
				if (string.IsNullOrEmpty(vHisTreatment.ICD_NAME))
				{
					txtIcdMain.Text = vHisTreatment.ICD_CODE;
					cboIcdMain.EditValue = vHisTreatment.ICD_CODE;
					txtIcdTextCode.Text = vHisTreatment.ICD_SUB_CODE;
					txtIcdTextName.Text = vHisTreatment.ICD_TEXT;
				}
				else
				{
					checkEdit.Checked = true;
					txtIcdMain.Text = vHisTreatment.ICD_CODE;
					cboIcdMain.EditValue = vHisTreatment.ICD_CODE;
					txtIcdTextName.Text = vHisTreatment.ICD_TEXT;
					txtIcdTextCode.Text = vHisTreatment.ICD_SUB_CODE;
					icdMainText.Text = vHisTreatment.ICD_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				if (e.RowHandle < 0)
				{
					return;
				}
				bool flag = Parse.ToBoolean((this.gridView.GetRowCellValue(e.RowHandle, "OUT_OF_HOSPITAL") ?? "").ToString());
				if (e.Column.FieldName == "BtnDelete")
				{
					switch (Parse.ToInt32((this.gridView.GetRowCellValue(e.RowHandle, "Action") ?? "").ToString()))
					{
					case 1:
						e.RepositoryItem = ButtonAdd;
						break;
					case 2:
						e.RepositoryItem = ButtonDelete;
						break;
					}
				}
				else if (e.Column.FieldName == "USERNAME")
				{
					if (flag)
					{
						e.RepositoryItem = TextEditUserName;
						return;
					}
					e.RepositoryItem = (RepositoryItem)(object)LookUpEditUserName;
					string value = (this.gridView.GetRowCellValue(e.RowHandle, "LOGINNAME") ?? "").ToString();
					this.gridView.SetRowCellValue(e.RowHandle, Gc_UserName, value);
				}
				else if (e.Column.FieldName == "LOGINNAME")
				{
					if (flag)
					{
						e.RepositoryItem = TextEditLoginNameDis;
					}
					else
					{
						e.RepositoryItem = TextEditLoginName;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData)
				{
					return;
				}
				GridView gridView = sender as GridView;
				if (!(e.Column.FieldName == "USERNAME"))
				{
					return;
				}
				try
				{
					string status = (gridView.GetRowCellValue(e.ListSourceRowIndex, "USERNAME") ?? "").ToString();
					ACS_USER val = GlobalStore.HisAcsUser.SingleOrDefault((ACS_USER o) => o.LOGINNAME == status);
					e.Value = val.USERNAME;
				}
				catch (Exception ex)
				{
					LogSystem.Warn("Loi hien thi gia tri cot USERNAME", ex);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void gridView_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			try
			{
				if (e.FocusedColumn.FieldName == "USERNAME")
				{
					gridView.ShowEditor();
                    ((PopupBaseEdit)(HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit)gridView.ActiveEditor).ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDatatoControlByHisDebateTemp(HIS_DEBATE_TEMP data)
		{
			//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a55: Expected O, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0884: Unknown result type (might be due to invalid IL or missing references)
			//IL_088a: Expected O, but got Unknown
			//IL_089c: Unknown result type (might be due to invalid IL or missing references)
			//IL_08a6: Expected O, but got Unknown
			//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			try
			{
				if (data != null)
				{
					datacombox = data;
					HIS_DEBATE val = new HIS_DEBATE();
					DataObjectMapper.Map<HIS_DEBATE>((object)val, (object)data);
					val.DISCUSSION = data.DISCUSSION;
					if (detailProcessor != null)
					{
						HIS_DEBATE saveData = new HIS_DEBATE();
						detailProcessor.GetData(GetTypeDetail(), ref saveData);
						switch (GetTypeDetail())
						{
						case DetailEnum.Khac:
							saveData.SUBCLINICAL_PROCESSES = (IsCheckLockInfor(saveData.SUBCLINICAL_PROCESSES) ? val.SUBCLINICAL_PROCESSES : saveData.SUBCLINICAL_PROCESSES);
							saveData.MEDICINE_TYPE_IDS = (IsCheckLockInfor(saveData.MEDICINE_TYPE_IDS) ? val.MEDICINE_TYPE_IDS : saveData.MEDICINE_TYPE_IDS);
							saveData.ACTIVE_INGREDIENT_IDS = (IsCheckLockInfor(saveData.ACTIVE_INGREDIENT_IDS) ? val.ACTIVE_INGREDIENT_IDS : saveData.ACTIVE_INGREDIENT_IDS);
							saveData.MEDICINE_TYPE_NAME = (IsCheckLockInfor(saveData.MEDICINE_TYPE_NAME) ? val.MEDICINE_TYPE_NAME : saveData.MEDICINE_TYPE_NAME);
							saveData.MEDICINE_CONCENTRA = (IsCheckLockInfor(saveData.MEDICINE_CONCENTRA) ? val.MEDICINE_CONCENTRA : saveData.MEDICINE_CONCENTRA);
							saveData.MEDICINE_USE_FORM_NAME = (IsCheckLockInfor(saveData.MEDICINE_USE_FORM_NAME) ? val.MEDICINE_USE_FORM_NAME : saveData.MEDICINE_USE_FORM_NAME);
							saveData.MEDICINE_TUTORIAL = (IsCheckLockInfor(saveData.MEDICINE_TUTORIAL) ? val.MEDICINE_TUTORIAL : saveData.MEDICINE_TUTORIAL);
							saveData.DISCUSSION = (IsCheckLockInfor(saveData.DISCUSSION) ? val.DISCUSSION : saveData.DISCUSSION);
							saveData.BEFORE_DIAGNOSTIC = (IsCheckLockInfor(saveData.BEFORE_DIAGNOSTIC) ? val.BEFORE_DIAGNOSTIC : saveData.BEFORE_DIAGNOSTIC);
							saveData.CARE_METHOD = (IsCheckLockInfor(saveData.CARE_METHOD) ? val.CARE_METHOD : saveData.CARE_METHOD);
							saveData.CONCLUSION = (IsCheckLockInfor(saveData.CONCLUSION) ? val.CONCLUSION : saveData.CONCLUSION);
							saveData.DIAGNOSTIC = (IsCheckLockInfor(saveData.DIAGNOSTIC) ? val.DIAGNOSTIC : saveData.DIAGNOSTIC);
							saveData.HOSPITALIZATION_STATE = (IsCheckLockInfor(saveData.HOSPITALIZATION_STATE) ? val.HOSPITALIZATION_STATE : saveData.HOSPITALIZATION_STATE);
							saveData.PATHOLOGICAL_HISTORY = (IsCheckLockInfor(saveData.PATHOLOGICAL_HISTORY) ? val.PATHOLOGICAL_HISTORY : saveData.PATHOLOGICAL_HISTORY);
							saveData.REQUEST_CONTENT = (IsCheckLockInfor(saveData.REQUEST_CONTENT) ? val.REQUEST_CONTENT : saveData.REQUEST_CONTENT);
							saveData.TREATMENT_METHOD = (IsCheckLockInfor(saveData.TREATMENT_METHOD) ? val.TREATMENT_METHOD : saveData.TREATMENT_METHOD);
							saveData.TREATMENT_TRACKING = (IsCheckLockInfor(saveData.TREATMENT_TRACKING) ? val.TREATMENT_TRACKING : saveData.TREATMENT_TRACKING);
							saveData.MEDICINE_USE_TIME = (IsCheckLockInfor(saveData.MEDICINE_USE_TIME.ToString()) ? val.MEDICINE_USE_TIME : saveData.MEDICINE_USE_TIME);
							break;
						case DetailEnum.Thuoc:
							saveData.SUBCLINICAL_PROCESSES = (IsCheckLockInfor(saveData.SUBCLINICAL_PROCESSES) ? val.SUBCLINICAL_PROCESSES : saveData.SUBCLINICAL_PROCESSES);
							saveData.MEDICINE_TYPE_IDS = (IsCheckLockInfor(saveData.MEDICINE_TYPE_IDS) ? val.MEDICINE_TYPE_IDS : saveData.MEDICINE_TYPE_IDS);
							saveData.ACTIVE_INGREDIENT_IDS = (IsCheckLockInfor(saveData.ACTIVE_INGREDIENT_IDS) ? val.ACTIVE_INGREDIENT_IDS : saveData.ACTIVE_INGREDIENT_IDS);
							saveData.MEDICINE_TYPE_NAME = (IsCheckLockInfor(saveData.MEDICINE_TYPE_NAME) ? val.MEDICINE_TYPE_NAME : saveData.MEDICINE_TYPE_NAME);
							saveData.MEDICINE_CONCENTRA = (IsCheckLockInfor(saveData.MEDICINE_CONCENTRA) ? val.MEDICINE_CONCENTRA : saveData.MEDICINE_CONCENTRA);
							saveData.MEDICINE_USE_FORM_NAME = (IsCheckLockInfor(saveData.MEDICINE_USE_FORM_NAME) ? val.MEDICINE_USE_FORM_NAME : saveData.MEDICINE_USE_FORM_NAME);
							saveData.MEDICINE_TUTORIAL = (IsCheckLockInfor(saveData.MEDICINE_TUTORIAL) ? val.MEDICINE_TUTORIAL : saveData.MEDICINE_TUTORIAL);
							saveData.DISCUSSION = (IsCheckLockInfor(saveData.DISCUSSION) ? val.DISCUSSION : saveData.DISCUSSION);
							saveData.BEFORE_DIAGNOSTIC = (IsCheckLockInfor(saveData.BEFORE_DIAGNOSTIC) ? val.BEFORE_DIAGNOSTIC : saveData.BEFORE_DIAGNOSTIC);
							saveData.CARE_METHOD = (IsCheckLockInfor(saveData.CARE_METHOD) ? val.CARE_METHOD : saveData.CARE_METHOD);
							saveData.CONCLUSION = (IsCheckLockInfor(saveData.CONCLUSION) ? val.CONCLUSION : saveData.CONCLUSION);
							saveData.DIAGNOSTIC = (IsCheckLockInfor(saveData.DIAGNOSTIC) ? val.DIAGNOSTIC : saveData.DIAGNOSTIC);
							saveData.HOSPITALIZATION_STATE = (IsCheckLockInfor(saveData.HOSPITALIZATION_STATE) ? val.HOSPITALIZATION_STATE : saveData.HOSPITALIZATION_STATE);
							saveData.PATHOLOGICAL_HISTORY = (IsCheckLockInfor(saveData.PATHOLOGICAL_HISTORY) ? val.PATHOLOGICAL_HISTORY : saveData.PATHOLOGICAL_HISTORY);
							saveData.REQUEST_CONTENT = (IsCheckLockInfor(saveData.REQUEST_CONTENT) ? val.REQUEST_CONTENT : saveData.REQUEST_CONTENT);
							saveData.TREATMENT_METHOD = (IsCheckLockInfor(saveData.TREATMENT_METHOD) ? val.TREATMENT_METHOD : saveData.TREATMENT_METHOD);
							saveData.TREATMENT_TRACKING = (IsCheckLockInfor(saveData.TREATMENT_TRACKING) ? val.TREATMENT_TRACKING : saveData.TREATMENT_TRACKING);
							saveData.MEDICINE_USE_TIME = (IsCheckLockInfor(saveData.MEDICINE_USE_TIME.ToString()) ? val.MEDICINE_USE_TIME : saveData.MEDICINE_USE_TIME);
							break;
						case DetailEnum.Pttt:
							saveData.SUBCLINICAL_PROCESSES = (IsCheckLockInfor(saveData.SUBCLINICAL_PROCESSES) ? val.SUBCLINICAL_PROCESSES : saveData.SUBCLINICAL_PROCESSES);
							saveData.INTERNAL_MEDICINE_STATE = (IsCheckLockInfor(saveData.INTERNAL_MEDICINE_STATE) ? val.INTERNAL_MEDICINE_STATE : saveData.INTERNAL_MEDICINE_STATE);
							saveData.TREATMENT_TRACKING = (IsCheckLockInfor(saveData.TREATMENT_TRACKING) ? val.TREATMENT_TRACKING : saveData.TREATMENT_TRACKING);
							saveData.TREATMENT_METHOD = (IsCheckLockInfor(saveData.TREATMENT_METHOD) ? val.TREATMENT_METHOD : saveData.TREATMENT_METHOD);
							saveData.PTTT_METHOD_ID = (IsCheckLockInfor(saveData.PTTT_METHOD_ID.ToString()) ? val.PTTT_METHOD_ID : saveData.PTTT_METHOD_ID);
							saveData.PTTT_METHOD_NAME = (IsCheckLockInfor(saveData.PTTT_METHOD_NAME) ? val.PTTT_METHOD_NAME : saveData.PTTT_METHOD_NAME);
							saveData.EMOTIONLESS_METHOD_ID = (IsCheckLockInfor(saveData.EMOTIONLESS_METHOD_ID.ToString()) ? val.EMOTIONLESS_METHOD_ID : saveData.EMOTIONLESS_METHOD_ID);
							saveData.SURGERY_TIME = (IsCheckLockInfor(saveData.SURGERY_TIME.ToString()) ? val.SURGERY_TIME : saveData.SURGERY_TIME);
							saveData.PROGNOSIS = (IsCheckLockInfor(saveData.PROGNOSIS) ? val.PROGNOSIS : saveData.PROGNOSIS);
							saveData.CONCLUSION = (IsCheckLockInfor(saveData.CONCLUSION) ? val.CONCLUSION : saveData.CONCLUSION);
							saveData.PATHOLOGICAL_HISTORY = (IsCheckLockInfor(saveData.PATHOLOGICAL_HISTORY) ? val.PATHOLOGICAL_HISTORY : saveData.PATHOLOGICAL_HISTORY);
							saveData.HOSPITALIZATION_STATE = (IsCheckLockInfor(saveData.HOSPITALIZATION_STATE) ? val.HOSPITALIZATION_STATE : saveData.HOSPITALIZATION_STATE);
							saveData.BEFORE_DIAGNOSTIC = (IsCheckLockInfor(saveData.BEFORE_DIAGNOSTIC) ? val.BEFORE_DIAGNOSTIC : saveData.BEFORE_DIAGNOSTIC);
							saveData.DISCUSSION = (IsCheckLockInfor(saveData.DISCUSSION) ? val.DISCUSSION : saveData.DISCUSSION);
							saveData.CARE_METHOD = (IsCheckLockInfor(saveData.CARE_METHOD) ? val.CARE_METHOD : saveData.CARE_METHOD);
							break;
						}
						detailProcessor.SetData(GetTypeDetail(), saveData);
					}
					HisDebateUserFilter val2 = new HisDebateUserFilter();
					val2.DEBATE_TEMP_ID = data.ID;
					List<HIS_DEBATE_USER> list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
					List<HisDebateUserADO> list2 = new List<HisDebateUserADO>();
					foreach (HIS_DEBATE_USER item in list)
					{
						HisDebateUserADO hisDebateUserADO = new HisDebateUserADO();
						DataObjectMapper.Map<HisDebateUserADO>((object)hisDebateUserADO, (object)item);
						if (item.IS_PRESIDENT == 1)
						{
							hisDebateUserADO.PRESIDENT = true;
						}
						if (item.IS_SECRETARY == 1)
						{
							hisDebateUserADO.SECRETARY = true;
						}
						if (item.IS_OUT_OF_HOSPITAL == 1)
						{
							hisDebateUserADO.OUT_OF_HOSPITAL = true;
						}
						hisDebateUserADO.Action = 2;
						list2.Add(hisDebateUserADO);
					}
					if (list2 != null && list2.Count > 0)
					{
						list2[0].Action = 1;
					}
					else
					{
						HisDebateUserADO hisDebateUserADO2 = new HisDebateUserADO();
						hisDebateUserADO2.Action = 1;
						list2.Add(hisDebateUserADO2);
					}
					gridControl.DataSource = list2;
					lstParticipantDebate = list2;
				}
				else
				{
					HIS_DEBATE data2 = new HIS_DEBATE();
					if (detailProcessor != null)
					{
						detailProcessor.SetData(GetTypeDetail(), data2);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async Task InitComboDebateTemp()
		{
			try
			{
				List<HIS_DEBATE_TEMP> data = null;
				Action myaction = delegate
				{
					string loginName = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
					data = (from p in BackendDataWorker.Get<HIS_DEBATE_TEMP>()
						where p.IS_ACTIVE == 1 && (p.IS_PUBLIC == 1 || p.DEPARTMENT_ID == WorkPlaceSDO.DepartmentId || p.CREATOR == loginName)
						select p).ToList();
				};
				Task task = new Task(myaction);
				task.Start();
				await task;
				ControlEditorADO controlEditorADO = new ControlEditorADO("DEBATE_TEMP_NAME", "ID", new List<ColumnInfo>
				{
					new ColumnInfo("DEBATE_TEMP_CODE", "", 100, 1),
					new ColumnInfo("DEBATE_TEMP_NAME", "", 250, 2)
				}, false, 350);
				ControlEditorLoader.Load((object)cboDebateTemp, (object)data, controlEditorADO);
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Warn(ex2);
			}
		}

		private void LoadDebateTempCombo(string _Code)
		{
			try
			{
				List<HIS_DEBATE_TEMP> list = new List<HIS_DEBATE_TEMP>();
				list = (from o in BackendDataWorker.Get<HIS_DEBATE_TEMP>()
					where o.DEBATE_TEMP_CODE != null && o.IS_ACTIVE == 1 && o.DEBATE_TEMP_CODE.StartsWith(_Code)
					select o).ToList();
				if (list.Count == 1)
				{
					cboDebateTemp.EditValue = list[0].ID;
					txtDebateTemp.Text = list[0].DEBATE_TEMP_CODE;
					dtDebateTime.Focus();
				}
				else if (list.Count > 1)
				{
					cboDebateTemp.EditValue = null;
					cboDebateTemp.Focus();
					cboDebateTemp.ShowPopup();
				}
				else
				{
					cboDebateTemp.EditValue = null;
					cboDebateTemp.Focus();
					cboDebateTemp.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void barButtonItemSave_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (btnSave.Enabled)
				{
					btnSave.Focus();
					btnSave_Click(null, null);
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
				if (baseEditViewInfo == null)
				{
					return;
				}
				if (positionHandleControl == -1)
				{
					positionHandleControl = baseEdit.TabIndex;
					if (baseEdit.Visible)
					{
						baseEdit.SelectAll();
						baseEdit.Focus();
					}
				}
				if (positionHandleControl > baseEdit.TabIndex)
				{
					positionHandleControl = baseEdit.TabIndex;
					if (baseEdit.Visible)
					{
						baseEdit.Focus();
						baseEdit.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkAutoSign_CheckedChanged(object sender, EventArgs e)
		{
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			try
			{
				WaitingManager.Show();
				ControlStateRDO csAdd = ((currentControlStateRDO__Create != null && currentControlStateRDO__Create.Count > 0) ? currentControlStateRDO__Create.Where((ControlStateRDO o) => o.KEY == "chkAutoSign" && o.MODULE_LINK == "HIS.Desktop.Plugins.Debate").FirstOrDefault() : null);
				ControlStateRDO val = ((currentControlStateRDO__Update != null && currentControlStateRDO__Update.Count > 0) ? currentControlStateRDO__Update.Where((ControlStateRDO o) => o.KEY == "chkAutoSign" && o.MODULE_LINK == "HIS.Desktop.Plugins.DebateDiagnostic").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<ControlStateRDO>((Expression<Func<ControlStateRDO>>)(() => csAdd)), (object)csAdd));
				if (csAdd != null)
				{
					csAdd.VALUE = (chkAutoSign.Checked ? "1" : "");
				}
				else if (val != null)
				{
					val.VALUE = (chkAutoSign.Checked ? "1" : "");
				}
				else
				{
					csAdd = new ControlStateRDO();
					csAdd.KEY = "chkAutoSign";
					csAdd.VALUE = (chkAutoSign.Checked ? "1" : "");
					csAdd.MODULE_LINK = "HIS.Desktop.Plugins.Debate";
					if (currentControlStateRDO__Create == null)
					{
						currentControlStateRDO__Create = new List<ControlStateRDO>();
					}
					currentControlStateRDO__Create.Add(csAdd);
					val = new ControlStateRDO();
					val.KEY = "chkAutoSign";
					val.VALUE = (chkAutoSign.Checked ? "1" : "");
					val.MODULE_LINK = "HIS.Desktop.Plugins.DebateDiagnostic";
					if (currentControlStateRDO__Update == null)
					{
						currentControlStateRDO__Update = new List<ControlStateRDO>();
					}
					currentControlStateRDO__Update.Add(val);
				}
				controlStateWorker.SetData(currentControlStateRDO__Create);
				controlStateWorker.SetData(currentControlStateRDO__Update);
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkAutoCreateEmr_CheckedChanged(object sender, EventArgs e)
		{
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Expected O, but got Unknown
			try
			{
				WaitingManager.Show();
				ControlStateRDO csAdd = ((currentControlStateRDO__Create != null && currentControlStateRDO__Create.Count > 0) ? currentControlStateRDO__Create.Where((ControlStateRDO o) => o.KEY == "chkAutoCreateEmr" && o.MODULE_LINK == "HIS.Desktop.Plugins.Debate").FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<ControlStateRDO>((Expression<Func<ControlStateRDO>>)(() => csAdd)), (object)csAdd));
				if (csAdd != null)
				{
					csAdd.VALUE = (chkAutoCreateEmr.Checked ? "1" : "");
				}
				else
				{
					csAdd = new ControlStateRDO();
					csAdd.KEY = "chkAutoCreateEmr";
					csAdd.VALUE = (chkAutoCreateEmr.Checked ? "1" : "");
					csAdd.MODULE_LINK = "HIS.Desktop.Plugins.Debate";
					if (currentControlStateRDO__Create == null)
					{
						currentControlStateRDO__Create = new List<ControlStateRDO>();
					}
					currentControlStateRDO__Create.Add(csAdd);
				}
				controlStateWorker.SetData(currentControlStateRDO__Create);
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void CheckThuoc_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (CheckThuoc.Checked)
				{
					ChkPttt.Checked = false;
					ChkOther.Checked = false;
					LoadDataControlDetail();
					HIS_DEBATE_TEMP data = BackendDataWorker.Get<HIS_DEBATE_TEMP>().FirstOrDefault((HIS_DEBATE_TEMP p) => p.ID == long.Parse(cboDebateTemp.EditValue.ToString()));
					FillDatatoControlByHisDebateTemp(data);
					if (detailProcessor != null)
					{
						detailProcessor.SetData(GetTypeDetail(), currentHisDebate);
					}
				}
				else if (!ChkPttt.Checked && !ChkOther.Checked)
				{
					CheckThuoc.Checked = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ChkPttt_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (ChkPttt.Checked)
				{
					CheckThuoc.Checked = false;
					ChkOther.Checked = false;
					LoadDataControlDetail();
					HIS_DEBATE_TEMP data = BackendDataWorker.Get<HIS_DEBATE_TEMP>().FirstOrDefault((HIS_DEBATE_TEMP p) => p.ID == long.Parse(cboDebateTemp.EditValue.ToString()));
					FillDatatoControlByHisDebateTemp(data);
					if (action == 1)
					{
						if (examServiceReq == null)
						{
							examServiceReq = ProcessGetExamServiceReq();
						}
						if (detailProcessor != null && examServiceReq != null)
						{
							FillDatatoControlByHisDebateTemp(data);
						}
					}
				}
				else if (!CheckThuoc.Checked && !ChkOther.Checked)
				{
					ChkPttt.Checked = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private HIS_SERVICE_REQ ProcessGetExamServiceReq()
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			HIS_SERVICE_REQ result = null;
			try
			{
				HisServiceReqFilter val = new HisServiceReqFilter();
				val.TREATMENT_ID = treatment_id;
				val.SERVICE_REQ_TYPE_ID = 1L;
				val.IS_NO_EXECUTE = false;
				List<HIS_SERVICE_REQ> list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
				if (list != null && list.Count > 0)
				{
					result = (from o in list
						orderby o.IS_MAIN_EXAM ?? 9999, o.INTRUCTION_TIME
						select o).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				result = null;
				LogSystem.Error(ex);
			}
			return result;
		}

		private void ChkOther_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (ChkOther.Checked)
				{
					ChkPttt.Checked = false;
					CheckThuoc.Checked = false;
					LoadDataControlDetail();
					HIS_DEBATE_TEMP val = null;
					val = BackendDataWorker.Get<HIS_DEBATE_TEMP>().FirstOrDefault(delegate(HIS_DEBATE_TEMP p)
					{
						long iD = p.ID;
						object editValue = cboDebateTemp.EditValue;
						return iD == long.Parse((editValue != null) ? editValue.ToString() : null);
					});
					FillDatatoControlByHisDebateTemp(val);
				}
				else if (!ChkPttt.Checked && !CheckThuoc.Checked)
				{
					ChkOther.Checked = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataControlDetail()
		{
			try
			{
				panelDetail.Controls.Clear();
				if (detailProcessor == null)
				{
					if (hisService != null)
					{
						detailProcessor = new DetailProcessor(treatment_id, moduleData.RoomId, moduleData.RoomTypeId, hisService, moduleData);
					}
					else
					{
						detailProcessor = new DetailProcessor(treatment_id, moduleData.RoomId, moduleData.RoomTypeId, moduleData);
					}
				}
				detailProcessor.DepartmentList = listDepartment;
				detailProcessor.UserList = acsUsers;
				detailProcessor.ExecuteRoleList = ListExecuteRole;
				detailProcessor.EmployeeList = Employee;
				ucDetail = detailProcessor.GetControl(GetTypeDetail());
				panelDetail.Controls.Add(ucDetail);
				ucDetail.Dock = DockStyle.Fill;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void FillDataControlFirstCheck(object data)
		{
			try
			{
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private DetailEnum GetTypeDetail()
		{
			DetailEnum result = DetailEnum.Thuoc;
			try
			{
				if (ChkPttt.Checked)
				{
					result = DetailEnum.Pttt;
				}
				else if (ChkOther.Checked)
				{
					result = DetailEnum.Khac;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void repositoryItemCboExecuteRole_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Plus)
				{
					List<object> list = new List<object>();
					list.Add(true);
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.HisExecuteRole", moduleData.RoomId, moduleData.RoomTypeId, list);
					ProcessLoadExecuteRole();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadExecuteList()
		{
			ProcessLoadExecuteRole();
		}

		private void ProcessLoadExecuteRole()
		{
			try
			{
				LoadDataToExecuteRole((RepositoryItemGridLookUpEdit)(object)repositoryItemCboExecuteRole, ListExecuteRole);
				LoadDataToExecuteRole((RepositoryItemGridLookUpEdit)(object)recboExecuteRole, ListExecuteRole);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView_ShownEditor(object sender, EventArgs e)
		{
			try
			{
				GridView gridView = sender as GridView;
				if (gridView.FocusedColumn.FieldName == Gc_Titles.FieldName && gridView.ActiveEditor is GridLookUpEdit)
				{
					GridLookUpEdit gridLookUpEdit = gridView.ActiveEditor as GridLookUpEdit;
					if (gridLookUpEdit != null)
					{
						FillDataIntoExecuteRoleCombo(gridLookUpEdit);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void FillDataIntoExecuteRoleCombo(GridLookUpEdit editor)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			try
			{
				List<HIS_EXECUTE_ROLE> list = ListExecuteRole.Where((HIS_EXECUTE_ROLE o) => o.IS_POSITION == 1 || o.IS_TITLE == 1).ToList();
				List<ColumnInfo> list2 = new List<ColumnInfo>();
				list2.Add(new ColumnInfo("EXECUTE_ROLE_CODE", "", 150, 1));
				list2.Add(new ColumnInfo("EXECUTE_ROLE_NAME", "", 250, 2));
				ControlEditorADO val = new ControlEditorADO("EXECUTE_ROLE_NAME", "ID", list2, false, 250);
				ControlEditorLoader.Load((object)editor, (object)list, val);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataToExecuteRole(RepositoryItemGridLookUpEdit cbo, List<HIS_EXECUTE_ROLE> data)
		{
			try
			{
				cbo.DataSource = data;
				cbo.DisplayMember = "EXECUTE_ROLE_NAME";
				cbo.ValueMember = "ID";
				cbo.TextEditStyle = TextEditStyles.Standard;
				cbo.PopupFilterMode = PopupFilterMode.Contains;
				cbo.ImmediatePopup = true;
				cbo.View.Columns.Clear();
				GridColumn gridColumn = cbo.View.Columns.AddField("EXECUTE_ROLE_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 1;
				gridColumn.Width = 50;
				GridColumn gridColumn2 = cbo.View.Columns.AddField("EXECUTE_ROLE_NAME");
				gridColumn2.Caption = "Tên";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 2;
				gridColumn2.Width = 100;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			try
			{
				HisDebateUserADO ado = (HisDebateUserADO)gridView.GetFocusedRow();
				if (ado != null && e.Column.FieldName == Gc_Titles.FieldName)
				{
					List<HIS_EXECUTE_ROLE> list = ListExecuteRole.Where((HIS_EXECUTE_ROLE o) => o.ID == ((HIS_DEBATE_USER)ado).EXECUTE_ROLE_ID).ToList();
					if (list != null && list.Count > 0)
					{
						((HIS_DEBATE_USER)ado).DESCRIPTION = list.First().EXECUTE_ROLE_NAME;
					}
					gridControl.RefreshDataSource();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void repositoryItemCboExecuteRole_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				GridLookUpEdit edit = sender as GridLookUpEdit;
				if (edit != null && edit.EditValue != null && (edit.EditValue ?? ((object)0)).ToString() != (edit.OldEditValue ?? ((object)0)).ToString())
				{
					List<HIS_EXECUTE_ROLE> list = ListExecuteRole.Where((HIS_EXECUTE_ROLE o) => o.ID == Parse.ToInt64(edit.EditValue.ToString())).ToList();
					if (list != null && list.Count > 0)
					{
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_Titles, list.First().ID);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboTracking(GridLookUpEdit cbo)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			try
			{
				if (trackingADOs != null)
				{
					if (cbo.EditValue == null && actionType == 2)
					{
						cbo.EditValue = null;
					}
					List<ColumnInfo> list = new List<ColumnInfo>();
					list.Add(new ColumnInfo("TrackingTimeStr", "", 250, 1));
					ControlEditorADO val = new ControlEditorADO("TrackingTimeStr", "ID", list, false, 250);
					ControlEditorLoader.Load((object)cbo, (object)trackingADOs, val);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPhieuDieuTri_EditValueChanged(object sender, EventArgs e)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			try
			{
				if (isInitTracking)
				{
					return;
				}
				if (cboPhieuDieuTri.EditValue != null)
				{
					TrackingADO trackingADO = ((trackingADOs != null) ? trackingADOs.FirstOrDefault((TrackingADO o) => ((HIS_TRACKING)o).ID == (long)cboPhieuDieuTri.EditValue) : null);
					if (trackingADO != null)
					{
						tracking = new HIS_TRACKING();
						DataObjectMapper.Map<HIS_TRACKING>((object)tracking, (object)trackingADO);
					}
					else
					{
						tracking = null;
					}
				}
				else
				{
					tracking = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPhieuDieuTri_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPhieuDieuTri.Properties.Buttons[1].Visible = false;
					cboPhieuDieuTri.EditValue = null;
				}
				else
				{
					if (e.Button.Kind != ButtonPredefines.Plus)
					{
						return;
					}
					Module val = GlobalVariables.currentModuleRaws.Where((Module o) => o.ModuleLink == "HIS.Desktop.Plugins.TrackingCreate").FirstOrDefault();
					if (val == null)
					{
						LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.TrackingCreate");
					}
					if (val.IsPlugin && val.ExtensionInfo != null)
					{
						List<object> list = new List<object>();
						list.Add(vHisTreatment.ID);
						list.Add(new Action<HIS_TRACKING>(ProcessAfterChangeTrackingTime));
						list.Add(PluginInstance.GetModuleWithWorkingRoom(val, moduleData.RoomId, moduleData.RoomTypeId));
						object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(val, moduleData.RoomId, moduleData.RoomTypeId), list);
						if (pluginInstance == null)
						{
							throw new ArgumentNullException("moduleData is null");
						}
						((Form)pluginInstance).ShowDialog();
						LoadDataTracking();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPhieuDieuTri_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && cboPhieuDieuTri.EditValue != null)
				{
					cboPhieuDieuTri.Properties.Buttons[1].Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessAfterChangeTrackingTime(HIS_TRACKING tracking)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			try
			{
				LogSystem.Debug("ProcessAfterChangeTrackingTime.1__" + LogUtil.TraceData(LogUtil.GetMemberName<HIS_TRACKING>((Expression<Func<HIS_TRACKING>>)(() => tracking)), (object)tracking));
				DateInputADO val = new DateInputADO();
				val.Time = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(tracking.TRACKING_TIME).Value;
				val.Dates = new List<DateTime?>();
				val.Dates.Add(val.Time);
				LogSystem.Debug("ProcessAfterChangeTrackingTime.2");
				LogSystem.Debug("ProcessAfterChangeTrackingTime.5");
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Co loi khi delegate ProcessAfterChangeTrackingTime duoc goi tu chuc nang tao/sua to dieu tri", ex);
			}
		}

		private void LoadDataTracking(bool isChangeData = true)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				LogSystem.Debug("LoadDataTracking => 1");
				CommonParam val = new CommonParam();
				HisTrackingFilter val2 = new HisTrackingFilter();
				val2.TREATMENT_ID = treatment_id;
				val2.DEPARTMENT_ID = WorkPlaceSDO.DepartmentId;
				List<HIS_TRACKING> list = ((AdapterBase)new BackendAdapter(val)).Get<List<HIS_TRACKING>>("api/HisTracking/Get", Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				if (list == null || list.Count == 0)
				{
					isInitTracking = false;
					return;
				}
				list = list.OrderByDescending((HIS_TRACKING o) => o.TRACKING_TIME).ToList();
				trackingADOs = new List<TrackingADO>();
				foreach (HIS_TRACKING item2 in list)
				{
					TrackingADO trackingADO = new TrackingADO();
					DataObjectMapper.Map<TrackingADO>((object)trackingADO, (object)item2);
					trackingADO.TrackingTimeStr = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(((HIS_TRACKING)trackingADO).TRACKING_TIME);
					trackingADOs.Add(trackingADO);
				}
				trackingADOs = trackingADOs.OrderByDescending((TrackingADO o) => ((HIS_TRACKING)o).TRACKING_TIME).ToList();
				InitComboTracking(cboPhieuDieuTri);
				if (!HisConfigCFG.DebateDiagnostic_IsDefaultTracking || (action != 1 && currentHisDebate != null))
				{
					return;
				}
				ChangeIntructionTime(dtDebateTime.DateTime);
				List<string> intructionDateSelectedProcess = new List<string>();
				foreach (long intructionTimeSelected in intructionTimeSelecteds)
				{
					string item = intructionTimeSelected.ToString().Substring(0, 8);
					intructionDateSelectedProcess.Add(item);
				}
				List<HIS_TRACKING> list2 = (from o in list
					where intructionDateSelectedProcess.Contains(o.TRACKING_TIME.ToString().Substring(0, 8)) && o.DEPARTMENT_ID == WorkPlaceSDO.DepartmentId
					orderby o.TRACKING_TIME descending
					select o).ToList();
				if (list2 != null && list2.Count > 0)
				{
					cboPhieuDieuTri.EditValue = list2.First().ID;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtDebateTime_EditValueChanged(object sender, EventArgs e)
		{
			if (!IsNotLoadFirst && HisConfigCFG.DebateDiagnostic_IsDefaultTracking)
			{
				LoadDataTracking();
			}
		}

		private void ChangeIntructionTime(DateTime intructTime)
		{
			try
			{
				intructionTimeSelected = new List<DateTime?>();
				intructionTimeSelected.Add(intructTime);
				intructionTimeSelecteds = (from o in intructionTimeSelected
					select Parse.ToInt64(o.Value.ToString("yyyyMMdd") + timeSelested.ToString("HHmm") + "00") into o
					orderby o descending
					select o).ToList();
				InstructionTime = intructionTimeSelecteds.OrderByDescending((long o) => o).First();
				cboPhieuDieuTri.EditValue = null;
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<List<long>>((Expression<Func<List<long>>>)(() => intructionTimeSelecteds)), (object)intructionTimeSelecteds));
				LogSystem.Debug("ChangeIntructionTime. 2");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboBienBanHoiChanCu_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboBienBanHoiChanCu.Properties.Buttons[1].Visible = false;
					cboBienBanHoiChanCu.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboBienBanHoiChanCu_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal || cboBienBanHoiChanCu.EditValue == null)
				{
					return;
				}
				cboBienBanHoiChanCu.Properties.Buttons[1].Visible = true;
				HIS_DEBATE val = lstDebate.Where((HIS_DEBATE o) => o.ID == int.Parse(cboBienBanHoiChanCu.EditValue.ToString())).FirstOrDefault();
				if (val != null && val.CONTENT_TYPE.HasValue)
				{
					if (val.CONTENT_TYPE.Value == 1)
					{
						ChkOther.Checked = true;
					}
					else if (val.CONTENT_TYPE.Value == 3)
					{
						ChkPttt.Checked = true;
					}
					else
					{
						CheckThuoc.Checked = true;
					}
				}
				if (detailProcessor != null)
				{
					detailProcessor.SetData(GetTypeDetail(), val);
				}
				SetDataHisDebate(val);
				SetDataDebateInvateUser(val);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void EnableColumnAddDelete()
		{
			try
			{
				if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
				{
					gridColumn8.OptionsColumn.AllowEdit = true;
				}
				else
				{
					gridColumn8.OptionsColumn.AllowEdit = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadComboThamGia()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			try
			{
				lstInvateADO = new List<InvateADO>();
				lstInvateADO.Add(new InvateADO(1L, "Có"));
				lstInvateADO.Add(new InvateADO(0L, "Không"));
				List<ColumnInfo> list = new List<ColumnInfo>();
				list.Add(new ColumnInfo("NAME", "", 250, 1));
				ControlEditorADO val = new ControlEditorADO("NAME", "ID", list, false, 250);
				ControlEditorLoader.Load((object)regluThamGia, (object)lstInvateADO, val);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnTreatmentHistory_Click(object sender, EventArgs e)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			try
			{
				WaitingManager.Show();
				List<object> list = new List<object>();
				TreatmentHistoryADO val = new TreatmentHistoryADO();
				val.treatmentId = vHisTreatment.ID;
				val.treatment_code = vHisTreatment.TREATMENT_CODE;
				list.Add(val);
				WaitingManager.Hide();
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.TreatmentHistory", moduleData.RoomId, moduleData.RoomTypeId, list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView5_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			try
			{
				if (e.RowHandle < 0)
				{
					return;
				}
				if (e.Column.FieldName == "FeedBack")
				{
					if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
					{
						e.RepositoryItem = rebtnFeedBackEnable;
					}
					else
					{
						e.RepositoryItem = rebtnFeedBackDisable;
					}
				}
				else if (e.Column.FieldName == "BtnDeleteInvateUser")
				{
					switch (Parse.ToInt32((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "Action") ?? "").ToString()))
					{
					case 1:
						e.RepositoryItem = rebtnAddInvateUser;
						break;
					case 2:
						e.RepositoryItem = rebtnMinusInvateUser;
						break;
					}
				}
				else if (e.Column.FieldName == "IS_PARTICIPATION_str")
				{
					if ((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "LOGINNAME") ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() && long.Parse((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "ID") ?? "").ToString()) > 0)
					{
						if (!string.IsNullOrEmpty((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString()) && ((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString() == "Có" || (((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString() == "Không"))
						{
							long? iD = lstInvateADO.First((InvateADO o) => o.NAME == (((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString()).ID;
							((ColumnView)(object)gridView5).SetRowCellValue(e.RowHandle, gridColumn2, (object)iD);
						}
						e.RepositoryItem = (RepositoryItem)(object)regluThamGia;
						return;
					}
					if (!string.IsNullOrEmpty((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString()) && ((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString() == "1" || (((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString() == "0"))
					{
						string nAME = lstInvateADO.First((InvateADO o) => o.ID == short.Parse((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION_str") ?? "").ToString())).NAME;
						((ColumnView)(object)gridView5).SetRowCellValue(e.RowHandle, gridColumn2, (object)nAME);
					}
					e.RepositoryItem = reTxtDisable;
				}
				else if (e.Column.FieldName == "USERNAME")
				{
					if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
					{
						e.RepositoryItem = (RepositoryItem)(object)LookUpEditUserNameInvate;
						string value = (((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "LOGINNAME") ?? "").ToString();
						((ColumnView)(object)gridView5).SetRowCellValue(e.RowHandle, gridColumn3, (object)value);
					}
					else
					{
						e.RepositoryItem = reTxtDisable;
					}
				}
				else if (e.Column.FieldName == "EXECUTE_ROLE_ID")
				{
					if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
					{
						e.RepositoryItem = (RepositoryItem)(object)recboExecuteRole;
					}
					else
					{
						e.RepositoryItem = reTxtDisable;
					}
				}
				else if (e.Column.FieldName == "PRESIDENT")
				{
					if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
					{
						e.RepositoryItem = rechkChuToa;
					}
					else
					{
						e.RepositoryItem = rechkChuToaDisable;
					}
				}
				else if (e.Column.FieldName == "SECRETARY")
				{
					if ((cboRequestLoggin.EditValue ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() || CheckLoginAdmin.IsAdmin(ClientTokenManagerStore.ClientTokenManager.GetLoginName()))
					{
						e.RepositoryItem = rechkThuKy;
					}
					else
					{
						e.RepositoryItem = rechkThuKyDisable;
					}
				}
				else if (e.Column.FieldName == "COMMENT_DOCTOR")
				{
					if ((((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "LOGINNAME") ?? "").ToString() == ClientTokenManagerStore.ClientTokenManager.GetLoginName() && (((ColumnView)(object)gridView5).GetRowCellValue(e.RowHandle, "IS_PARTICIPATION") ?? "").ToString() == "1")
					{
						e.RepositoryItem = reTxtComment;
					}
					else
					{
						e.RepositoryItem = reTxtDisable;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void rebtnAddInvateUser_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				HisDebateInvateUserADO hisDebateInvateUserADO = new HisDebateInvateUserADO();
				lstDebateInvateUserADO = ((GridControl)(object)gridControl1).DataSource as List<HisDebateInvateUserADO>;
				if (lstDebateInvateUserADO != null && lstDebateInvateUserADO.Count > 0)
				{
					HisDebateInvateUserADO item = new HisDebateInvateUserADO();
					lstDebateInvateUserADO.Add(item);
					lstDebateInvateUserADO.ForEach(delegate(HisDebateInvateUserADO o)
					{
						o.Action = 2;
					});
					lstDebateInvateUserADO.LastOrDefault().Action = 1;
					((GridControl)(object)gridControl1).DataSource = null;
					((GridControl)(object)gridControl1).DataSource = lstDebateInvateUserADO;
				}
				else
				{
					HisDebateInvateUserADO item2 = new HisDebateInvateUserADO();
					lstDebateInvateUserADO.Add(item2);
					lstDebateInvateUserADO.LastOrDefault().Action = 1;
					((GridControl)(object)gridControl1).DataSource = null;
					((GridControl)(object)gridControl1).DataSource = lstDebateInvateUserADO;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rebtnMinusInvateUser_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				CommonParam val = new CommonParam();
				HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (hisDebateInvateUserADO != null)
				{
					lstDebateInvateUserADO.Remove(hisDebateInvateUserADO);
					lstDebateInvateUserADO.ForEach(delegate(HisDebateInvateUserADO o)
					{
						o.Action = 2;
					});
					lstDebateInvateUserADO.LastOrDefault().Action = 1;
					((GridControl)(object)gridControl1).DataSource = null;
					((GridControl)(object)gridControl1).DataSource = lstDebateInvateUserADO;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rebtnFeedBackEnable_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (hisDebateInvateUserADO != null)
				{
					detailProcessor.SetDataDiscussion(GetTypeDetail(), ((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView5_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData)
				{
					return;
				}
				GridView gridView = sender as GridView;
				if (!(e.Column.FieldName == "USERNAME"))
				{
					return;
				}
				try
				{
					string status = (gridView.GetRowCellValue(e.ListSourceRowIndex, "USERNAME") ?? "").ToString();
					ACS_USER val = GlobalStore.HisAcsUser.SingleOrDefault((ACS_USER o) => o.LOGINNAME == status);
					e.Value = val.USERNAME;
				}
				catch (Exception ex)
				{
					LogSystem.Warn("Loi hien thi gia tri cot USERNAME", ex);
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void regluThamGia_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					GridLookUpEdit gridLookUpEdit = sender as GridLookUpEdit;
					gridLookUpEdit.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void regluThamGia_Closed(object sender, ClosedEventArgs e)
		{
		}

		private void recboExecuteRole_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Plus)
				{
					List<object> list = new List<object>();
					list.Add(true);
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.HisExecuteRole", moduleData.RoomId, moduleData.RoomTypeId, list);
					ProcessLoadExecuteRole();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LookUpEditUserNameInvate_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
                HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit edit = (HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit)((sender is HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit) ? sender : null);
				if (edit != null && ((BaseEdit)(object)edit).EditValue != null && (((BaseEdit)(object)edit).EditValue ?? ((object)0)).ToString() != (((BaseEdit)(object)edit).OldEditValue ?? ((object)0)).ToString())
				{
					ACS_USER val = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((BaseEdit)(object)edit).EditValue.ToString());
					if (val != null)
					{
						((ColumnView)(object)gridView5).SetRowCellValue(((ColumnView)(object)gridView5).FocusedRowHandle, gcLoginName, (object)val.LOGINNAME);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void reTxtComment_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
					if (!string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR))
					{
						memContent.Text = ((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR;
					}
					else
					{
						memContent.Text = "";
					}
					ButtonEdit buttonEdit = sender as ButtonEdit;
					Rectangle rectangle = new Rectangle(buttonEdit.Bounds.X, buttonEdit.Bounds.Y, buttonEdit.Bounds.Width, buttonEdit.Bounds.Height);
					string s = Screen.PrimaryScreen.Bounds.Width.ToString();
					string text = Screen.PrimaryScreen.Bounds.Height.ToString();
					if (long.Parse(s) > 1600)
					{
						popupControlContainer1.Height = 200;
						popupControlContainer1.Width = 500;
					}
					else
					{
						popupControlContainer1.Height = 200;
						popupControlContainer1.Width = 350;
					}
					popupControlContainer1.ShowPopup(new Point(rectangle.X + 650, rectangle.Bottom + 170));
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			try
			{
				HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (!string.IsNullOrEmpty(memContent.Text))
				{
					((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR = memContent.Text;
				}
				else
				{
					((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR = null;
				}
				((ColumnView)(object)gridView5).SetRowCellValue(((ColumnView)(object)gridView5).FocusedRowHandle, gridColumn7, (object)((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR);
				popupControlContainer1.HidePopup();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				memContent.Text = "";
				popupControlContainer1.HidePopup();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void reTxtComment_Leave(object sender, EventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Expected O, but got Unknown
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				bool value = false;
				CommonParam val = new CommonParam();
				HisDebateInvateUserADO participant = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (participant != null && !string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)participant).COMMENT_DOCTOR))
				{
					string text = "";
					if (CountVi.Count(((HIS_DEBATE_INVITE_USER)participant).COMMENT_DOCTOR) > 1000)
					{
						text = ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME + " - " + GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME).USERNAME + " có \"Nhận xét\" vượt quá 1000 ký tự.\r\n";
					}
					if (!string.IsNullOrEmpty(text))
					{
						XtraMessageBox.Show(text, "Thông báo danh sách mời tham gia", MessageBoxButtons.OK);
						return;
					}
				}
				WaitingManager.Show();
				HIS_DEBATE_INVITE_USER val2 = new HIS_DEBATE_INVITE_USER();
				DataObjectMapper.Map<HIS_DEBATE_INVITE_USER>((object)val2, (object)participant);
				ACS_USER val3 = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME);
				if (val3 != null)
				{
					val2.LOGINNAME = val3.LOGINNAME;
					if (!string.IsNullOrEmpty(val3.USERNAME))
					{
						val2.USERNAME = val3.USERNAME;
					}
				}
				val2.ID = 0L;
				if (((HIS_DEBATE_INVITE_USER)participant).ID > 0)
				{
					val2.ID = ((HIS_DEBATE_INVITE_USER)participant).ID;
				}
				if (((HIS_DEBATE_INVITE_USER)participant).EXECUTE_ROLE_ID > 0)
				{
					val2.DESCRIPTION = ListExecuteRole.FirstOrDefault((HIS_EXECUTE_ROLE o) => o.ID == ((HIS_DEBATE_INVITE_USER)participant).EXECUTE_ROLE_ID).EXECUTE_ROLE_NAME;
				}
				if (participant.PRESIDENT)
				{
					val2.IS_PRESIDENT = (short)1;
				}
				else
				{
					val2.IS_PRESIDENT = null;
				}
				if (participant.SECRETARY)
				{
					val2.IS_SECRETARY = (short)1;
				}
				else
				{
					val2.IS_SECRETARY = null;
				}
				HIS_DEBATE_INVITE_USER val4 = ((AdapterBase)new BackendAdapter(val)).Post<HIS_DEBATE_INVITE_USER>("api/HisDebateInviteUser/Update", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				if (val4 != null)
				{
					value = true;
				}
				WaitingManager.Hide();
				MessageManager.Show((Form)(object)this, val, (bool?)value);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void regluThamGia_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void reTxtComment_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					reTxtComment_Leave(null, null);
					((ColumnView)(object)gridView5).FocusedColumn = ((ColumnView)(object)gridView5).VisibleColumns[7];
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView5_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			try
			{
				if (e.FocusedColumn.FieldName == "USERNAME")
				{
					((BaseView)(object)gridView5).ShowEditor();
                    ((PopupBaseEdit)(HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit)((BaseView)(object)gridView5).ActiveEditor).ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void reTxtComment_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				ButtonEdit buttonEdit = sender as ButtonEdit;
				if (!string.IsNullOrEmpty(buttonEdit.Text))
				{
					((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR = buttonEdit.Text.Trim();
				}
				else
				{
					((HIS_DEBATE_INVITE_USER)hisDebateInvateUserADO).COMMENT_DOCTOR = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void regluThamGia_EditValueChanged(object sender, EventArgs e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Expected O, but got Unknown
			//IL_0303: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Expected O, but got Unknown
			//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				GridLookUpEdit grd = sender as GridLookUpEdit;
				if (grd.EditValue == null)
				{
					return;
				}
				CommonParam val = new CommonParam();
				bool value = false;
				HisDebateInvateUserADO participant = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (participant != null && !string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)participant).COMMENT_DOCTOR))
				{
					string text = "";
					if (CountVi.Count(((HIS_DEBATE_INVITE_USER)participant).COMMENT_DOCTOR) > 1000)
					{
						text = ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME + " - " + GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME).USERNAME + " có \"Nhận xét\" vượt quá 1000 ký tự.\r\n";
					}
					if (!string.IsNullOrEmpty(text))
					{
						XtraMessageBox.Show(text, "Thông báo danh sách mời tham gia", MessageBoxButtons.OK);
						return;
					}
				}
				WaitingManager.Show();
				string text2 = "";
				text2 = lstInvateADO.First((InvateADO o) => o.ID == short.Parse(grd.EditValue.ToString())).NAME;
				((ColumnView)(object)gridView5).SetRowCellValue(((ColumnView)(object)gridView5).FocusedRowHandle, gridColumn2, grd.EditValue);
				((ColumnView)(object)gridView5).SetRowCellValue(((ColumnView)(object)gridView5).FocusedRowHandle, gridColumn9, grd.EditValue);
				((HIS_DEBATE_INVITE_USER)participant).IS_PARTICIPATION = short.Parse(grd.EditValue.ToString());
				HIS_DEBATE_INVITE_USER val2 = new HIS_DEBATE_INVITE_USER();
				DataObjectMapper.Map<HIS_DEBATE_INVITE_USER>((object)val2, (object)participant);
				ACS_USER val3 = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)participant).LOGINNAME);
				if (val3 != null)
				{
					val2.LOGINNAME = val3.LOGINNAME;
					if (!string.IsNullOrEmpty(val3.USERNAME))
					{
						val2.USERNAME = val3.USERNAME;
					}
				}
				val2.ID = 0L;
				if (((HIS_DEBATE_INVITE_USER)participant).ID > 0)
				{
					val2.ID = ((HIS_DEBATE_INVITE_USER)participant).ID;
				}
				if (((HIS_DEBATE_INVITE_USER)participant).EXECUTE_ROLE_ID > 0)
				{
					val2.DESCRIPTION = ListExecuteRole.FirstOrDefault((HIS_EXECUTE_ROLE o) => o.ID == ((HIS_DEBATE_INVITE_USER)participant).EXECUTE_ROLE_ID).EXECUTE_ROLE_NAME;
				}
				if (participant.PRESIDENT)
				{
					val2.IS_PRESIDENT = (short)1;
				}
				else
				{
					val2.IS_PRESIDENT = null;
				}
				if (participant.SECRETARY)
				{
					val2.IS_SECRETARY = (short)1;
				}
				else
				{
					val2.IS_SECRETARY = null;
				}
				HIS_DEBATE_INVITE_USER val4 = ((AdapterBase)new BackendAdapter(val)).Post<HIS_DEBATE_INVITE_USER>("api/HisDebateInviteUser/Update", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				if (val4 != null)
				{
					value = true;
					SDA_NOTIFY updateDTO = new SDA_NOTIFY();
					updateDTO.CONTENT = string.Format("Tài khoản {0} - {1}  xác nhận {2} tham gia hội chẩn bệnh nhân {3} – {4}, {5}. Mời bạn vào chức năng “Tạo biên bản hội chẩn” để xem chi tiết", ClientTokenManagerStore.ClientTokenManager.GetLoginName(), ClientTokenManagerStore.ClientTokenManager.GetUserName(), text2, vHisTreatment.TREATMENT_CODE, vHisTreatment.TDL_PATIENT_NAME, listDepartment.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == long.Parse((cboDepartment.EditValue ?? "").ToString())).DEPARTMENT_NAME);
					updateDTO.TITLE = "Xác nhận mời tham gia hội chẩn";
					updateDTO.FROM_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now).GetValueOrDefault();
					updateDTO.TO_TIME = Parse.ToInt64(System.Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd") + "235959");
					updateDTO.RECEIVER_LOGINNAME = ((cboRequestLoggin.EditValue != null) ? cboRequestLoggin.EditValue.ToString() : "");
					LogSystem.Debug("updateDTO___SDA" + LogUtil.TraceData(LogUtil.GetMemberName<SDA_NOTIFY>((Expression<Func<SDA_NOTIFY>>)(() => updateDTO)), (object)updateDTO));
					SDA_NOTIFY val5 = ((AdapterBase)new BackendAdapter(val)).Post<SDA_NOTIFY>("api/SdaNotify/Create", HIS.Desktop.ApiConsumer.ApiConsumers.SdaConsumer, (object)updateDTO, val);
					if (val5 == null)
					{
					}
				}
				WaitingManager.Hide();
				MessageManager.Show((Form)(object)this, val, (bool?)value);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView5_CustomRowColumnError(object sender, RowColumnErrorEventArgs e)
		{
			try
			{
				if (e.ColumnName == "COMMENT_DOCTOR")
				{
					gridView_CustomRowError(sender, e);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridView_CustomRowError(object sender, RowColumnErrorEventArgs e)
		{
			try
			{
				int dataSourceRowIndex = ((ColumnView)(object)gridView5).GetDataSourceRowIndex(((RowErrorEventArgs)e).RowHandle);
				if (dataSourceRowIndex < 0)
				{
					((RowErrorEventArgs)e).Info.ErrorType = ErrorType.None;
					((RowErrorEventArgs)e).Info.ErrorText = "";
					return;
				}
				List<HisDebateInvateUserADO> list = ((GridControl)(object)gridControl1).DataSource as List<HisDebateInvateUserADO>;
				HisDebateInvateUserADO hisDebateInvateUserADO = list[dataSourceRowIndex];
				if (e.ColumnName == "COMMENT_DOCTOR")
				{
					if (hisDebateInvateUserADO.ErrorTypeCommentDoctor == ErrorType.Warning)
					{
						((RowErrorEventArgs)e).Info.ErrorType = hisDebateInvateUserADO.ErrorTypeCommentDoctor;
						((RowErrorEventArgs)e).Info.ErrorText = hisDebateInvateUserADO.ErrorMessageCommentDoctor;
					}
					else
					{
						((RowErrorEventArgs)e).Info.ErrorType = ErrorType.None;
						((RowErrorEventArgs)e).Info.ErrorText = "";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidMicrobiProcessing(HisDebateInvateUserADO ado)
		{
			try
			{
				if (ado != null)
				{
					if (!string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)ado).COMMENT_DOCTOR) && CountVi.Count(((HIS_DEBATE_INVITE_USER)ado).COMMENT_DOCTOR) > 1000)
					{
						ado.ErrorMessageCommentDoctor = "Vượt quá độ dài cho phép 1000 ký tự";
						ado.ErrorTypeCommentDoctor = ErrorType.Warning;
					}
					else
					{
						ado.ErrorMessageCommentDoctor = "";
						ado.ErrorTypeCommentDoctor = ErrorType.None;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridView5_CellValueChanged(object sender, CellValueChangedEventArgs e)
		{
			try
			{
				HisDebateInvateUserADO hisDebateInvateUserADO = (HisDebateInvateUserADO)((ColumnView)(object)gridView5).GetFocusedRow();
				if (hisDebateInvateUserADO != null && !(e.Column.FieldName == "COMMENT_DOCTOR"))
				{
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSendTMP_Click(object sender, EventArgs e)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected O, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Expected O, but got Unknown
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string text = HisConfigs.Get<string>("HIS.Desktop.Plugins.Library.Telemedicine.ConnectionInfo");
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split('|');
					LogSystem.Debug(LogUtil.TraceData("infoArr___", (object)array));
					TelemedicineProcessor val = new TelemedicineProcessor(array[0], array[1], array[2]);
					if (vHisTreatment == null || vHisTreatment.ID == 0)
					{
						CommonParam val2 = new CommonParam();
						HisTreatmentViewFilter val3 = new HisTreatmentViewFilter();
						((FilterBase)val3).ID = currentHisDebate.TREATMENT_ID;
						List<V_HIS_TREATMENT> source = ((AdapterBase)new BackendAdapter(val2)).Get<List<V_HIS_TREATMENT>>("api/HisTreatment/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val3, val2);
						vHisTreatment = source.FirstOrDefault();
					}
					List<V_HIS_SERVICE_REQ> list = new List<V_HIS_SERVICE_REQ>();
					List<HIS_SERE_SERV> list2 = new List<HIS_SERE_SERV>();
					List<V_HIS_SERE_SERV_TEIN> list3 = new List<V_HIS_SERE_SERV_TEIN>();
					List<V_HIS_TREATMENT_BED_ROOM> list4 = new List<V_HIS_TREATMENT_BED_ROOM>();
					V_HIS_PATIENT val4 = new V_HIS_PATIENT();
					V_HIS_DEBATE val5 = new V_HIS_DEBATE();
					if (cboPhieuDieuTri.EditValue != null)
					{
						list = GetServiceReqs__Connect(Parse.ToInt64(cboPhieuDieuTri.EditValue.ToString()));
						list2 = GetSereServs__Connect(vHisTreatment.ID);
						list3 = GetSereServTein__Connect(vHisTreatment.ID);
					}
					else
					{
						list = GetServiceReqsExecute__Connect(vHisTreatment.ID);
						list2 = GetSereServs__Connect(vHisTreatment.ID);
						list3 = GetSereServTein__Connect(vHisTreatment.ID);
					}
					list4 = GetTreatmentBedRoom__Connect(vHisTreatment.ID);
					val4 = GetPatient__Connect(vHisTreatment.PATIENT_ID);
					val5 = GetDebate__Connect(currentHisDebate.ID);
					TelemedicineResultADO val6 = val.SendToTmp(val4, vHisTreatment, val5, list, list2, list3, list4, BranchDataWorker.Branch);
					if (!val6.Success)
					{
						XtraMessageBox.Show(val6.Message, "Thông báo", MessageBoxButtons.OK);
						return;
					}
					CommonParam val7 = new CommonParam();
					DebateTelemedicineSDO val8 = new DebateTelemedicineSDO();
					val8.TmpId = val6.TmpId;
					val8.DebateId = val5.ID;
					bool value = ((AdapterBase)new BackendAdapter(val7)).Post<bool>("api/HisDebate/UpdateTelemedicineInfo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val8, val7);
					MessageManager.Show((Form)(object)this, val7, (bool?)value);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private List<V_HIS_SERVICE_REQ> GetServiceReqsExecute__Connect(long treatmentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			List<V_HIS_SERVICE_REQ> list = new List<V_HIS_SERVICE_REQ>();
			try
			{
				HisServiceReqViewFilter val = new HisServiceReqViewFilter();
				val.TREATMENT_ID = treatmentId;
				list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
				if (list.Count() > 0)
				{
					list = list.Where((V_HIS_SERVICE_REQ o) => o.SERVICE_REQ_TYPE_ID == 1).ToList();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return list;
		}

		private V_HIS_DEBATE GetDebate__Connect(long debateId)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			V_HIS_DEBATE result = new V_HIS_DEBATE();
			try
			{
				HisDebateViewFilter val = new HisDebateViewFilter();
				((FilterBase)val).ID = debateId;
				CommonParam val2 = new CommonParam();
				result = ((AdapterBase)new BackendAdapter(val2)).Get<List<V_HIS_DEBATE>>("api/HisDebate/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, val2).FirstOrDefault();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private V_HIS_PATIENT GetPatient__Connect(long patientId)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			V_HIS_PATIENT result = new V_HIS_PATIENT();
			try
			{
				HisPatientViewFilter val = new HisPatientViewFilter();
				((FilterBase)val).ID = patientId;
				result = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_PATIENT>>("api/HisPatient/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null).First();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private List<V_HIS_TREATMENT_BED_ROOM> GetTreatmentBedRoom__Connect(long treatmentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			List<V_HIS_TREATMENT_BED_ROOM> result = new List<V_HIS_TREATMENT_BED_ROOM>();
			try
			{
				HisTreatmentBedRoomViewFilter val = new HisTreatmentBedRoomViewFilter();
				val.TREATMENT_ID = treatmentId;
				result = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_TREATMENT_BED_ROOM>>("api/HisTreatmentBedRoom/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private List<V_HIS_SERE_SERV_TEIN> GetSereServTein__Connect(long treatmentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			List<V_HIS_SERE_SERV_TEIN> result = new List<V_HIS_SERE_SERV_TEIN>();
			try
			{
				HisSereServTeinViewFilter val = new HisSereServTeinViewFilter();
				val.TDL_TREATMENT_ID = treatmentId;
				result = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_SERE_SERV_TEIN>>("api/HisSereServTein/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private List<HIS_SERE_SERV> GetSereServs__Connect(long treatmentId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
			try
			{
				HisSereServFilter val = new HisSereServFilter();
				val.TREATMENT_ID = treatmentId;
				result = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_SERE_SERV>>("api/HisSereServ/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private List<V_HIS_SERVICE_REQ> GetServiceReqs__Connect(long trackingId)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			List<V_HIS_SERVICE_REQ> result = new List<V_HIS_SERVICE_REQ>();
			try
			{
				HisServiceReqViewFilter val = new HisServiceReqViewFilter();
				val.TRACKING_ID = trackingId;
				result = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_SERVICE_REQ>>("api/HisServiceReq/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void cboDebateReason_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboDebateReason.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CheckEditBsNgoaiVien_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				CheckEdit checkEdit = sender as CheckEdit;
				if (checkEdit != null)
				{
					if (checkEdit.Checked)
					{
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_OutOfHospital, true);
					}
					else
					{
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_OutOfHospital, false);
					}
					gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_LoginName, null);
					gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_UserName, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkLockInfor_CheckedChanged(object sender, EventArgs e)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			try
			{
				if (isNotLoadWhileChangeControlStateInFirst)
				{
					return;
				}
				WaitingManager.Show();
				ControlStateRDO val = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkLockInfor.Name && o.MODULE_LINK == moduleLink).FirstOrDefault() : null);
				if (val != null)
				{
					val.VALUE = (chkLockInfor.Checked ? "1" : "");
				}
				else
				{
					val = new ControlStateRDO();
					val.KEY = chkLockInfor.Name;
					val.VALUE = (chkLockInfor.Checked ? "1" : "");
					val.MODULE_LINK = moduleLink;
					if (currentControlStateRDO == null)
					{
						currentControlStateRDO = new List<ControlStateRDO>();
					}
					currentControlStateRDO.Add(val);
				}
				controlStateWorker.SetData(currentControlStateRDO);
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool IsCheckLockInfor(string input)
		{
			return !chkLockInfor.Checked || (chkLockInfor.Checked && string.IsNullOrEmpty(input));
		}

		private bool IsCheckLockInforList(List<HIS_DEBATE_EKIP_USER> lstData)
		{
			return !chkLockInfor.Checked || (chkLockInfor.Checked && (lstData == null || lstData.Count == 0));
		}

		private void chkAutoCreateTracking_CheckedChanged(object sender, EventArgs e)
		{
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
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Expected O, but got Unknown
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Expected O, but got Unknown
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Expected O, but got Unknown
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Expected O, but got Unknown
			//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Expected O, but got Unknown
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Expected O, but got Unknown
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Expected O, but got Unknown
			ComponentResourceManager resources = new ComponentResourceManager(typeof(FormDebateDiagnostic));
			SerializableAppearanceObject appearance = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceHovered = new SerializableAppearanceObject();
			SerializableAppearanceObject appearancePressed = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceDisabled = new SerializableAppearanceObject();
			SerializableAppearanceObject appearance2 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceHovered2 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearancePressed2 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceDisabled2 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearance3 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceHovered3 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearancePressed3 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceDisabled3 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearance4 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceHovered4 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearancePressed4 = new SerializableAppearanceObject();
			SerializableAppearanceObject appearanceDisabled4 = new SerializableAppearanceObject();
			layoutControl1 = new LayoutControl();
			chkAutoCreateTracking = new CheckEdit();
			barManager1 = new BarManager();
			bar1 = new Bar();
			barButtonItemSave = new BarButtonItem();
			barButtonItem1 = new BarButtonItem();
			barButtonItem2 = new BarButtonItem();
			barDockControlTop = new BarDockControl();
			barDockControlBottom = new BarDockControl();
			barDockControlLeft = new BarDockControl();
			barDockControlRight = new BarDockControl();
			chkLockInfor = new CheckEdit();
			btnSendTMP = new SimpleButton();
			popupControlContainer1 = new PopupControlContainer();
			layoutControl2 = new LayoutControl();
			btnCancel = new SimpleButton();
			btnOk = new SimpleButton();
			memContent = new MemoEdit();
			layoutControlGroup2 = new LayoutControlGroup();
			layoutControlItem20 = new LayoutControlItem();
			layoutControlItem21 = new LayoutControlItem();
			layoutControlItem22 = new LayoutControlItem();
			emptySpaceItem1 = new EmptySpaceItem();
			btnTreatmentHistory = new SimpleButton();
			gridControl1 = new MyGridControl();
			gridView5 = new MyGridView();
			gridColumn1 = new GridColumn();
			rebtnFeedBackEnable = new RepositoryItemButtonEdit();
			gridColumn2 = new GridColumn();
            regluThamGia = new Inventec.Desktop.CustomControl.CustomGrid.RepositoryItemCustomGridLookUpEdit();
            customGridView1 = new Inventec.Desktop.CustomControl.CustomGrid.CustomGridView();
			gridColumn3 = new GridColumn();
            LookUpEditUserNameInvate = new Utilities.Extensions.RepositoryItemCustomGridLookUpEdit();
            customGridView2 = new Utilities.Extensions.CustomGridView();
			gridColumn4 = new GridColumn();
			rechkThuKy = new RepositoryItemCheckEdit();
			gridColumn5 = new GridColumn();
			rechkChuToa = new RepositoryItemCheckEdit();
			gridColumn6 = new GridColumn();
            recboExecuteRole = new Utilities.Extensions.RepositoryItemCustomGridLookUpEdit();
            customGridView3 = new Utilities.Extensions.CustomGridView();
			gridColumn7 = new GridColumn();
			reTxtComment = new RepositoryItemButtonEdit();
			gridColumn8 = new GridColumn();
			rebtnAddInvateUser = new RepositoryItemButtonEdit();
			gcLoginName = new GridColumn();
			gridColumn9 = new GridColumn();
			gridColumn10 = new GridColumn();
			colFeedBackUnb = new GridColumn();
			colIS_PARTICIPATION_strUnb = new GridColumn();
			colUSERNAMEUnb = new GridColumn();
			colPRESIDENTUnb = new GridColumn();
			colSECRETARYUnb = new GridColumn();
			colEXECUTE_ROLE_IDUnb = new GridColumn();
			colCOMMENT_DOCTORUnb = new GridColumn();
			colBtnDeleteInvateUserUnb = new GridColumn();
			rebtnFeedBackDisable = new RepositoryItemButtonEdit();
			rebtnMinusInvateUser = new RepositoryItemButtonEdit();
			recboThamGia = new RepositoryItemComboBox();
			reTxtDisable = new RepositoryItemTextEdit();
			rechkThuKyDisable = new RepositoryItemCheckEdit();
			rechkChuToaDisable = new RepositoryItemCheckEdit();
			cboBienBanHoiChanCu = new GridLookUpEdit();
			gridView4 = new GridView();
			cboPhieuDieuTri = new GridLookUpEdit();
			gridView3 = new GridView();
			gridControl = new GridControl();
			gridView = new GridView();
			gridColumnParticipants_Id = new GridColumn();
			Gc_LoginName = new GridColumn();
			TextEditLoginName = new RepositoryItemTextEdit();
			Gc_UserName = new GridColumn();
			Gc_President = new GridColumn();
			CheckEditChuToa = new RepositoryItemCheckEdit();
			Gc_Secretary = new GridColumn();
			CheckEditThuKy = new RepositoryItemCheckEdit();
			Gc_OutOfHospital = new GridColumn();
			CheckEditBsNgoaiVien = new RepositoryItemCheckEdit();
			Gc_Titles = new GridColumn();
            repositoryItemCboExecuteRole = new Utilities.Extensions.RepositoryItemCustomGridLookUpEdit();
            repositoryItemCustomGridLookUpEdit1View = new Utilities.Extensions.CustomGridView();
			Gc_Add = new GridColumn();
			ButtonAdd = new RepositoryItemButtonEdit();
			CheckEditChuToaDisable = new RepositoryItemCheckEdit();
			CheckEditThuKyDisable = new RepositoryItemCheckEdit();
			ButtonDelete = new RepositoryItemButtonEdit();
            LookUpEditUserName = new HIS.Desktop.Utilities.Extensions.RepositoryItemCustomGridLookUpEdit();
			TextEditUserName = new RepositoryItemTextEdit();
			TextEditLoginNameDis = new RepositoryItemTextEdit();
			ChkOther = new CheckEdit();
			ChkPttt = new CheckEdit();
			CheckThuoc = new CheckEdit();
			panelDetail = new PanelControl();
			chkAutoCreateEmr = new CheckEdit();
			chkAutoSign = new CheckEdit();
			cboDebateType = new GridLookUpEdit();
			gridLookUpEdit1View = new GridView();
			panelControl1 = new PanelControl();
			cboIcdMain = new GridLookUpEdit();
			gridLookUpEdit3View = new GridView();
			icdMainText = new TextEdit();
			txtLocation = new TextEdit();
			cboRequestLoggin = new GridLookUpEdit();
			gridLookUpEdit2View = new GridView();
			cboDepartment = new GridLookUpEdit();
			gridView2 = new GridView();
			dtOutTime = new DateEdit();
			dtInTime = new DateEdit();
			txtDebateTemp = new TextEdit();
			btnSaveTemp = new SimpleButton();
			txtIcdTextName = new TextEdit();
			dtDebateTime = new DateEdit();
			btnPrint = new DropDownButton();
			btnSave = new SimpleButton();
			txtIcdTextCode = new TextEdit();
			checkEdit = new CheckEdit();
			txtIcdMain = new TextEdit();
			txtRequestLoggin = new TextEdit();
			cboDebateTemp = new GridLookUpEdit();
			gridView1 = new GridView();
			cboDebateReason = new GridLookUpEdit();
			gridView6 = new GridView();
			layoutControlGroup1 = new LayoutControlGroup();
			lciRequestContent = new LayoutControlItem();
			layoutControlItem11 = new LayoutControlItem();
			layoutControlItem12 = new LayoutControlItem();
			lciDebateTime = new LayoutControlItem();
			LcDebateTemp = new LayoutControlItem();
			layoutControlItem4 = new LayoutControlItem();
			lciIcdMain = new LayoutControlItem();
			lciCheckEdit = new LayoutControlItem();
			lciIcdSubCode1 = new LayoutControlItem();
			lciIcdSubCode = new LayoutControlItem();
			layoutControlItem2 = new LayoutControlItem();
			layoutControlItem5 = new LayoutControlItem();
			layoutControlItem6 = new LayoutControlItem();
			layoutControlItem13 = new LayoutControlItem();
			icdLocation = new LayoutControlItem();
			layoutControlItem3 = new LayoutControlItem();
			layoutControlItem15 = new LayoutControlItem();
			layoutControlItem1 = new LayoutControlItem();
			layoutControlItem9 = new LayoutControlItem();
			layoutControlItem10 = new LayoutControlItem();
			layoutControlItem14 = new LayoutControlItem();
			layoutControlItem16 = new LayoutControlItem();
			lciAutoCreateEmr = new LayoutControlItem();
			layoutControlItem8 = new LayoutControlItem();
			emptySpaceItem3 = new EmptySpaceItem();
			layoutControlItem7 = new LayoutControlItem();
			layoutControlItem17 = new LayoutControlItem();
			emptySpaceItem2 = new EmptySpaceItem();
			lciAutoSign = new LayoutControlItem();
			layoutControlItem18 = new LayoutControlItem();
			layoutControlItem19 = new LayoutControlItem();
			layoutControlItem23 = new LayoutControlItem();
			layoutControlItem24 = new LayoutControlItem();
			layoutControlItem25 = new LayoutControlItem();
			layoutControlItem26 = new LayoutControlItem();
			emptySpaceItem4 = new EmptySpaceItem();
			dxValidationProvider1 = new DXValidationProvider();
			((ISupportInitialize)layoutControl1).BeginInit();
			layoutControl1.SuspendLayout();
			((ISupportInitialize)chkAutoCreateTracking.Properties).BeginInit();
			((ISupportInitialize)barManager1).BeginInit();
			((ISupportInitialize)chkLockInfor.Properties).BeginInit();
			((ISupportInitialize)popupControlContainer1).BeginInit();
			popupControlContainer1.SuspendLayout();
			((ISupportInitialize)layoutControl2).BeginInit();
			layoutControl2.SuspendLayout();
			((ISupportInitialize)memContent.Properties).BeginInit();
			((ISupportInitialize)layoutControlGroup2).BeginInit();
			((ISupportInitialize)layoutControlItem20).BeginInit();
			((ISupportInitialize)layoutControlItem21).BeginInit();
			((ISupportInitialize)layoutControlItem22).BeginInit();
			((ISupportInitialize)emptySpaceItem1).BeginInit();
			((ISupportInitialize)gridControl1).BeginInit();
			((ISupportInitialize)gridView5).BeginInit();
			((ISupportInitialize)rebtnFeedBackEnable).BeginInit();
			((ISupportInitialize)regluThamGia).BeginInit();
			((ISupportInitialize)customGridView1).BeginInit();
			((ISupportInitialize)LookUpEditUserNameInvate).BeginInit();
			((ISupportInitialize)customGridView2).BeginInit();
			((ISupportInitialize)rechkThuKy).BeginInit();
			((ISupportInitialize)rechkChuToa).BeginInit();
			((ISupportInitialize)recboExecuteRole).BeginInit();
			((ISupportInitialize)customGridView3).BeginInit();
			((ISupportInitialize)reTxtComment).BeginInit();
			((ISupportInitialize)rebtnAddInvateUser).BeginInit();
			((ISupportInitialize)rebtnFeedBackDisable).BeginInit();
			((ISupportInitialize)rebtnMinusInvateUser).BeginInit();
			((ISupportInitialize)recboThamGia).BeginInit();
			((ISupportInitialize)reTxtDisable).BeginInit();
			((ISupportInitialize)rechkThuKyDisable).BeginInit();
			((ISupportInitialize)rechkChuToaDisable).BeginInit();
			((ISupportInitialize)cboBienBanHoiChanCu.Properties).BeginInit();
			((ISupportInitialize)gridView4).BeginInit();
			((ISupportInitialize)cboPhieuDieuTri.Properties).BeginInit();
			((ISupportInitialize)gridView3).BeginInit();
			((ISupportInitialize)gridControl).BeginInit();
			((ISupportInitialize)gridView).BeginInit();
			((ISupportInitialize)TextEditLoginName).BeginInit();
			((ISupportInitialize)CheckEditChuToa).BeginInit();
			((ISupportInitialize)CheckEditThuKy).BeginInit();
			((ISupportInitialize)CheckEditBsNgoaiVien).BeginInit();
			((ISupportInitialize)repositoryItemCboExecuteRole).BeginInit();
			((ISupportInitialize)repositoryItemCustomGridLookUpEdit1View).BeginInit();
			((ISupportInitialize)ButtonAdd).BeginInit();
			((ISupportInitialize)CheckEditChuToaDisable).BeginInit();
			((ISupportInitialize)CheckEditThuKyDisable).BeginInit();
			((ISupportInitialize)ButtonDelete).BeginInit();
			((ISupportInitialize)LookUpEditUserName).BeginInit();
			((ISupportInitialize)TextEditUserName).BeginInit();
			((ISupportInitialize)TextEditLoginNameDis).BeginInit();
			((ISupportInitialize)ChkOther.Properties).BeginInit();
			((ISupportInitialize)ChkPttt.Properties).BeginInit();
			((ISupportInitialize)CheckThuoc.Properties).BeginInit();
			((ISupportInitialize)panelDetail).BeginInit();
			((ISupportInitialize)chkAutoCreateEmr.Properties).BeginInit();
			((ISupportInitialize)chkAutoSign.Properties).BeginInit();
			((ISupportInitialize)cboDebateType.Properties).BeginInit();
			((ISupportInitialize)gridLookUpEdit1View).BeginInit();
			((ISupportInitialize)panelControl1).BeginInit();
			panelControl1.SuspendLayout();
			((ISupportInitialize)cboIcdMain.Properties).BeginInit();
			((ISupportInitialize)gridLookUpEdit3View).BeginInit();
			((ISupportInitialize)icdMainText.Properties).BeginInit();
			((ISupportInitialize)txtLocation.Properties).BeginInit();
			((ISupportInitialize)cboRequestLoggin.Properties).BeginInit();
			((ISupportInitialize)gridLookUpEdit2View).BeginInit();
			((ISupportInitialize)cboDepartment.Properties).BeginInit();
			((ISupportInitialize)gridView2).BeginInit();
			((ISupportInitialize)dtOutTime.Properties.CalendarTimeProperties).BeginInit();
			((ISupportInitialize)dtOutTime.Properties).BeginInit();
			((ISupportInitialize)dtInTime.Properties.CalendarTimeProperties).BeginInit();
			((ISupportInitialize)dtInTime.Properties).BeginInit();
			((ISupportInitialize)txtDebateTemp.Properties).BeginInit();
			((ISupportInitialize)txtIcdTextName.Properties).BeginInit();
			((ISupportInitialize)dtDebateTime.Properties.CalendarTimeProperties).BeginInit();
			((ISupportInitialize)dtDebateTime.Properties).BeginInit();
			((ISupportInitialize)txtIcdTextCode.Properties).BeginInit();
			((ISupportInitialize)checkEdit.Properties).BeginInit();
			((ISupportInitialize)txtIcdMain.Properties).BeginInit();
			((ISupportInitialize)txtRequestLoggin.Properties).BeginInit();
			((ISupportInitialize)cboDebateTemp.Properties).BeginInit();
			((ISupportInitialize)gridView1).BeginInit();
			((ISupportInitialize)cboDebateReason.Properties).BeginInit();
			((ISupportInitialize)gridView6).BeginInit();
			((ISupportInitialize)layoutControlGroup1).BeginInit();
			((ISupportInitialize)lciRequestContent).BeginInit();
			((ISupportInitialize)layoutControlItem11).BeginInit();
			((ISupportInitialize)layoutControlItem12).BeginInit();
			((ISupportInitialize)lciDebateTime).BeginInit();
			((ISupportInitialize)LcDebateTemp).BeginInit();
			((ISupportInitialize)layoutControlItem4).BeginInit();
			((ISupportInitialize)lciIcdMain).BeginInit();
			((ISupportInitialize)lciCheckEdit).BeginInit();
			((ISupportInitialize)lciIcdSubCode1).BeginInit();
			((ISupportInitialize)lciIcdSubCode).BeginInit();
			((ISupportInitialize)layoutControlItem2).BeginInit();
			((ISupportInitialize)layoutControlItem5).BeginInit();
			((ISupportInitialize)layoutControlItem6).BeginInit();
			((ISupportInitialize)layoutControlItem13).BeginInit();
			((ISupportInitialize)icdLocation).BeginInit();
			((ISupportInitialize)layoutControlItem3).BeginInit();
			((ISupportInitialize)layoutControlItem15).BeginInit();
			((ISupportInitialize)layoutControlItem1).BeginInit();
			((ISupportInitialize)layoutControlItem9).BeginInit();
			((ISupportInitialize)layoutControlItem10).BeginInit();
			((ISupportInitialize)layoutControlItem14).BeginInit();
			((ISupportInitialize)layoutControlItem16).BeginInit();
			((ISupportInitialize)lciAutoCreateEmr).BeginInit();
			((ISupportInitialize)layoutControlItem8).BeginInit();
			((ISupportInitialize)emptySpaceItem3).BeginInit();
			((ISupportInitialize)layoutControlItem7).BeginInit();
			((ISupportInitialize)layoutControlItem17).BeginInit();
			((ISupportInitialize)emptySpaceItem2).BeginInit();
			((ISupportInitialize)lciAutoSign).BeginInit();
			((ISupportInitialize)layoutControlItem18).BeginInit();
			((ISupportInitialize)layoutControlItem19).BeginInit();
			((ISupportInitialize)layoutControlItem23).BeginInit();
			((ISupportInitialize)layoutControlItem24).BeginInit();
			((ISupportInitialize)layoutControlItem25).BeginInit();
			((ISupportInitialize)layoutControlItem26).BeginInit();
			((ISupportInitialize)emptySpaceItem4).BeginInit();
			((ISupportInitialize)dxValidationProvider1).BeginInit();
			((Control)this).SuspendLayout();
			layoutControl1.Controls.Add(chkAutoCreateTracking);
			layoutControl1.Controls.Add(chkLockInfor);
			layoutControl1.Controls.Add(btnSendTMP);
			layoutControl1.Controls.Add(popupControlContainer1);
			layoutControl1.Controls.Add(btnTreatmentHistory);
			layoutControl1.Controls.Add((Control)(object)gridControl1);
			layoutControl1.Controls.Add(cboBienBanHoiChanCu);
			layoutControl1.Controls.Add(cboPhieuDieuTri);
			layoutControl1.Controls.Add(gridControl);
			layoutControl1.Controls.Add(ChkOther);
			layoutControl1.Controls.Add(ChkPttt);
			layoutControl1.Controls.Add(CheckThuoc);
			layoutControl1.Controls.Add(panelDetail);
			layoutControl1.Controls.Add(chkAutoCreateEmr);
			layoutControl1.Controls.Add(chkAutoSign);
			layoutControl1.Controls.Add(cboDebateType);
			layoutControl1.Controls.Add(panelControl1);
			layoutControl1.Controls.Add(txtLocation);
			layoutControl1.Controls.Add(cboRequestLoggin);
			layoutControl1.Controls.Add(cboDepartment);
			layoutControl1.Controls.Add(dtOutTime);
			layoutControl1.Controls.Add(dtInTime);
			layoutControl1.Controls.Add(txtDebateTemp);
			layoutControl1.Controls.Add(btnSaveTemp);
			layoutControl1.Controls.Add(txtIcdTextName);
			layoutControl1.Controls.Add(dtDebateTime);
			layoutControl1.Controls.Add(btnPrint);
			layoutControl1.Controls.Add(btnSave);
			layoutControl1.Controls.Add(txtIcdTextCode);
			layoutControl1.Controls.Add(checkEdit);
			layoutControl1.Controls.Add(txtIcdMain);
			layoutControl1.Controls.Add(txtRequestLoggin);
			layoutControl1.Controls.Add(cboDebateTemp);
			layoutControl1.Controls.Add(cboDebateReason);
			layoutControl1.Dock = DockStyle.Fill;
			layoutControl1.Location = new Point(0, 29);
			layoutControl1.Name = "layoutControl1";
			layoutControl1.Root = layoutControlGroup1;
			layoutControl1.Size = new Size(1362, 720);
			layoutControl1.TabIndex = 0;
			layoutControl1.Text = "layoutControl1";
			chkAutoCreateTracking.Location = new Point(229, 696);
			chkAutoCreateTracking.MenuManager = barManager1;
			chkAutoCreateTracking.Name = "chkAutoCreateTracking";
			chkAutoCreateTracking.Properties.Caption = "";
			chkAutoCreateTracking.Size = new Size(19, 19);
			chkAutoCreateTracking.StyleController = layoutControl1;
			chkAutoCreateTracking.TabIndex = 44;
			chkAutoCreateTracking.CheckedChanged += chkAutoCreateTracking_CheckedChanged;
			barManager1.Bars.AddRange(new Bar[1] { bar1 });
			barManager1.DockControls.Add(barDockControlTop);
			barManager1.DockControls.Add(barDockControlBottom);
			barManager1.DockControls.Add(barDockControlLeft);
			barManager1.DockControls.Add(barDockControlRight);
			barManager1.Form = (Control)(object)this;
			barManager1.Items.AddRange(new BarItem[3] { barButtonItemSave, barButtonItem1, barButtonItem2 });
			barManager1.MaxItemId = 3;
			bar1.BarName = "Tools";
			bar1.DockCol = 0;
			bar1.DockRow = 0;
			bar1.DockStyle = BarDockStyle.Top;
			bar1.LinksPersistInfo.AddRange(new LinkPersistInfo[2]
			{
				new LinkPersistInfo(barButtonItemSave),
				new LinkPersistInfo(barButtonItem1)
			});
			bar1.Text = "Tools";
			bar1.Visible = false;
			barButtonItemSave.Caption = "Ctrl S";
			barButtonItemSave.Id = 0;
			barButtonItemSave.ItemShortcut = new BarShortcut(Keys.S | Keys.Control);
			barButtonItemSave.Name = "barButtonItemSave";
			barButtonItemSave.ItemClick += barButtonItemSave_ItemClick;
			barButtonItem1.Caption = "Lưu mẫu (Ctrl T)";
			barButtonItem1.Id = 1;
			barButtonItem1.ItemShortcut = new BarShortcut(Keys.T | Keys.Control);
			barButtonItem1.Name = "barButtonItem1";
			barButtonItem1.ItemClick += barButtonItem1_ItemClick;
			barButtonItem2.Caption = "barButtonItem2";
			barButtonItem2.Id = 2;
			barButtonItem2.Name = "barButtonItem2";
			barDockControlTop.CausesValidation = false;
			barDockControlTop.Dock = DockStyle.Top;
			barDockControlTop.Location = new Point(0, 0);
			barDockControlTop.Size = new Size(1362, 29);
			barDockControlBottom.CausesValidation = false;
			barDockControlBottom.Dock = DockStyle.Bottom;
			barDockControlBottom.Location = new Point(0, 749);
			barDockControlBottom.Size = new Size(1362, 0);
			barDockControlLeft.CausesValidation = false;
			barDockControlLeft.Dock = DockStyle.Left;
			barDockControlLeft.Location = new Point(0, 29);
			barDockControlLeft.Size = new Size(0, 720);
			barDockControlRight.CausesValidation = false;
			barDockControlRight.Dock = DockStyle.Right;
			barDockControlRight.Location = new Point(1362, 29);
			barDockControlRight.Size = new Size(0, 720);
			chkLockInfor.Location = new Point(469, 2);
			chkLockInfor.MenuManager = barManager1;
			chkLockInfor.Name = "chkLockInfor";
			chkLockInfor.Properties.Caption = "Giữ thông tin";
			chkLockInfor.Size = new Size(84, 19);
			chkLockInfor.StyleController = layoutControl1;
			chkLockInfor.TabIndex = 43;
			chkLockInfor.ToolTip = "Không hiển thị dữ liệu mẫu vào các ô đã có dữ liệu";
			chkLockInfor.CheckedChanged += chkLockInfor_CheckedChanged;
			btnSendTMP.Location = new Point(802, 696);
			btnSendTMP.MaximumSize = new Size(106, 22);
			btnSendTMP.MinimumSize = new Size(106, 22);
			btnSendTMP.Name = "btnSendTMP";
			btnSendTMP.Size = new Size(106, 22);
			btnSendTMP.StyleController = layoutControl1;
			btnSendTMP.TabIndex = 41;
			btnSendTMP.Text = "Gửi TMP";
			btnSendTMP.ToolTip = "Gửi thông tin đến hệ thống y tế từ xa - TMP";
			btnSendTMP.Click += btnSendTMP_Click;
			popupControlContainer1.BorderStyle = BorderStyles.NoBorder;
			popupControlContainer1.Controls.Add(layoutControl2);
			popupControlContainer1.Location = new Point(790, 232);
			popupControlContainer1.Manager = barManager1;
			popupControlContainer1.Name = "popupControlContainer1";
			popupControlContainer1.Size = new Size(250, 130);
			popupControlContainer1.TabIndex = 40;
			popupControlContainer1.Visible = false;
			layoutControl2.Controls.Add(btnCancel);
			layoutControl2.Controls.Add(btnOk);
			layoutControl2.Controls.Add(memContent);
			layoutControl2.Dock = DockStyle.Fill;
			layoutControl2.Location = new Point(0, 0);
			layoutControl2.Name = "layoutControl2";
			layoutControl2.Root = layoutControlGroup2;
			layoutControl2.Size = new Size(250, 130);
			layoutControl2.TabIndex = 0;
			layoutControl2.Text = "layoutControl2";
			btnCancel.Location = new Point(188, 106);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(60, 22);
			btnCancel.StyleController = layoutControl2;
			btnCancel.TabIndex = 6;
			btnCancel.Text = "Bỏ qua";
			btnCancel.Click += btnCancel_Click;
			btnOk.Location = new Point(123, 106);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(61, 22);
			btnOk.StyleController = layoutControl2;
			btnOk.TabIndex = 5;
			btnOk.Text = "Đồng ý";
			btnOk.Click += btnOk_Click;
			memContent.Location = new Point(2, 2);
			memContent.MenuManager = barManager1;
			memContent.Name = "memContent";
			memContent.Size = new Size(246, 100);
			memContent.StyleController = layoutControl2;
			memContent.TabIndex = 4;
			layoutControlGroup2.EnableIndentsWithoutBorders = DefaultBoolean.False;
			layoutControlGroup2.GroupBordersVisible = false;
			layoutControlGroup2.Items.AddRange(new BaseLayoutItem[4] { layoutControlItem20, layoutControlItem21, layoutControlItem22, emptySpaceItem1 });
			layoutControlGroup2.Location = new Point(0, 0);
			layoutControlGroup2.Name = "layoutControlGroup2";
			layoutControlGroup2.Size = new Size(250, 130);
			layoutControlGroup2.TextVisible = false;
			layoutControlItem20.Control = memContent;
			layoutControlItem20.Location = new Point(0, 0);
			layoutControlItem20.Name = "layoutControlItem20";
			layoutControlItem20.Size = new Size(250, 104);
			layoutControlItem20.TextSize = new Size(0, 0);
			layoutControlItem20.TextVisible = false;
			layoutControlItem21.Control = btnOk;
			layoutControlItem21.Location = new Point(121, 104);
			layoutControlItem21.Name = "layoutControlItem21";
			layoutControlItem21.Size = new Size(65, 26);
			layoutControlItem21.TextSize = new Size(0, 0);
			layoutControlItem21.TextVisible = false;
			layoutControlItem22.Control = btnCancel;
			layoutControlItem22.Location = new Point(186, 104);
			layoutControlItem22.Name = "layoutControlItem22";
			layoutControlItem22.Size = new Size(64, 26);
			layoutControlItem22.TextSize = new Size(0, 0);
			layoutControlItem22.TextVisible = false;
			emptySpaceItem1.AllowHotTrack = false;
			emptySpaceItem1.Location = new Point(0, 104);
			emptySpaceItem1.Name = "emptySpaceItem1";
			emptySpaceItem1.Size = new Size(121, 26);
			emptySpaceItem1.TextSize = new Size(0, 0);
			btnTreatmentHistory.Location = new Point(912, 696);
			btnTreatmentHistory.MaximumSize = new Size(118, 22);
			btnTreatmentHistory.MinimumSize = new Size(118, 22);
			btnTreatmentHistory.Name = "btnTreatmentHistory";
			btnTreatmentHistory.Size = new Size(118, 22);
			btnTreatmentHistory.StyleController = layoutControl1;
			btnTreatmentHistory.TabIndex = 39;
			btnTreatmentHistory.Text = "Lịch sử điều trị";
			btnTreatmentHistory.Click += btnTreatmentHistory_Click;
			((Control)(object)gridControl1).Location = new Point(792, 146);
			((GridControl)(object)gridControl1).MainView = (BaseView)(object)gridView5;
			((EditorContainer)(object)gridControl1).MenuManager = barManager1;
			((Control)(object)gridControl1).Name = "gridControl1";
			((EditorContainer)(object)gridControl1).RepositoryItems.AddRange(new RepositoryItem[14]
			{
				rebtnFeedBackEnable,
				rebtnFeedBackDisable,
				rebtnAddInvateUser,
				rebtnMinusInvateUser,
				rechkThuKy,
				rechkChuToa,
				recboThamGia,
				reTxtDisable,
				(RepositoryItem)(object)regluThamGia,
				(RepositoryItem)(object)LookUpEditUserNameInvate,
				(RepositoryItem)(object)recboExecuteRole,
				rechkThuKyDisable,
				rechkChuToaDisable,
				reTxtComment
			});
			((Control)(object)gridControl1).Size = new Size(568, 129);
			((Control)(object)gridControl1).TabIndex = 38;
			((GridControl)(object)gridControl1).ViewCollection.AddRange(new BaseView[1] { (BaseView)(object)gridView5 });
			((ColumnView)(object)gridView5).Columns.AddRange(new GridColumn[19]
			{
				gridColumn1, gridColumn2, gridColumn3, gridColumn4, gridColumn5, gridColumn6, gridColumn7, gridColumn8, gcLoginName, gridColumn9,
				gridColumn10, colFeedBackUnb, colIS_PARTICIPATION_strUnb, colUSERNAMEUnb, colPRESIDENTUnb, colSECRETARYUnb, colEXECUTE_ROLE_IDUnb, colCOMMENT_DOCTORUnb, colBtnDeleteInvateUserUnb
			});
			((BaseView)(object)gridView5).GridControl = (GridControl)(object)gridControl1;
			((BaseView)(object)gridView5).Name = "gridView5";
			((GridView)(object)gridView5).OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			((GridView)(object)gridView5).OptionsView.ShowDetailButtons = false;
			((GridView)(object)gridView5).OptionsView.ShowGroupPanel = false;
			((GridView)(object)gridView5).OptionsView.ShowIndicator = false;
			gridView5.CustomRowColumnError += gridView5_CustomRowColumnError;
			((GridView)(object)gridView5).CustomRowCellEdit += gridView5_CustomRowCellEdit;
			((ColumnView)(object)gridView5).FocusedColumnChanged += gridView5_FocusedColumnChanged;
			((ColumnView)(object)gridView5).CellValueChanged += gridView5_CellValueChanged;
			((ColumnView)(object)gridView5).CustomUnboundColumnData += gridView5_CustomUnboundColumnData;
			gridColumn1.Caption = "gridColumn1";
			gridColumn1.ColumnEdit = rebtnFeedBackEnable;
			gridColumn1.FieldName = "FeedBack";
			gridColumn1.FieldNameSortGroup = "FeedBackUnb";
			gridColumn1.MaxWidth = 25;
			gridColumn1.MinWidth = 25;
			gridColumn1.Name = "gridColumn1";
			gridColumn1.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn1.OptionsColumn.ShowCaption = false;
			gridColumn1.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn1.Visible = true;
			gridColumn1.VisibleIndex = 0;
			gridColumn1.Width = 25;
			rebtnFeedBackEnable.AutoHeight = false;
			rebtnFeedBackEnable.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Glyph, "", -1, true, true, false, ImageLocation.MiddleCenter, (Image)resources.GetObject("rebtnFeedBackEnable.Buttons"), new KeyShortcut(Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "Lấy ý kiến nhận xét của thành viên", null, null, true)
			});
			rebtnFeedBackEnable.Name = "rebtnFeedBackEnable";
			rebtnFeedBackEnable.TextEditStyle = TextEditStyles.HideTextEditor;
			rebtnFeedBackEnable.ButtonClick += rebtnFeedBackEnable_ButtonClick;
			gridColumn2.Caption = "Tham gia";
			gridColumn2.ColumnEdit = (RepositoryItem)(object)regluThamGia;
			gridColumn2.FieldName = "IS_PARTICIPATION_str";
			gridColumn2.FieldNameSortGroup = "IS_PARTICIPATION_strUnb";
			gridColumn2.MaxWidth = 70;
			gridColumn2.MinWidth = 70;
			gridColumn2.Name = "gridColumn2";
			gridColumn2.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn2.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn2.Visible = true;
			gridColumn2.VisibleIndex = 1;
			gridColumn2.Width = 70;
			((RepositoryItemGridLookUpEdit)(object)regluThamGia).AutoComplete = false;
			((RepositoryItem)(object)regluThamGia).AutoHeight = false;
			((RepositoryItemButtonEdit)(object)regluThamGia).Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete, "", -1, true, false, false, ImageLocation.MiddleCenter, null, new KeyShortcut(Keys.None), appearance2, appearanceHovered2, appearancePressed2, appearanceDisabled2, "", null, null, true)
			});
			((RepositoryItem)(object)regluThamGia).Name = "regluThamGia";
			((RepositoryItem)(object)regluThamGia).NullText = "";
			((RepositoryItemButtonEdit)(object)regluThamGia).TextEditStyle = TextEditStyles.Standard;
			((RepositoryItemGridLookUpEditBase)(object)regluThamGia).View = (GridView)(object)customGridView1;
			((RepositoryItemPopupBase)(object)regluThamGia).Closed += regluThamGia_Closed;
			((RepositoryItemButtonEdit)(object)regluThamGia).ButtonClick += regluThamGia_ButtonClick;
			((RepositoryItem)(object)regluThamGia).EditValueChanged += regluThamGia_EditValueChanged;
			((RepositoryItem)(object)regluThamGia).KeyDown += regluThamGia_KeyDown;
			((GridView)(object)customGridView1).FocusRectStyle = DrawFocusRectStyle.RowFocus;
			((BaseView)(object)customGridView1).Name = "customGridView1";
			((GridView)(object)customGridView1).OptionsSelection.EnableAppearanceFocusedCell = false;
			((GridView)(object)customGridView1).OptionsView.ShowGroupPanel = false;
			gridColumn3.Caption = "Họ và tên";
			gridColumn3.ColumnEdit = (RepositoryItem)(object)LookUpEditUserNameInvate;
			gridColumn3.FieldName = "USERNAME";
			gridColumn3.FieldNameSortGroup = "USERNAMEUnb";
			gridColumn3.Name = "gridColumn3";
			gridColumn3.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn3.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn3.Visible = true;
			gridColumn3.VisibleIndex = 2;
			gridColumn3.Width = 95;
			((RepositoryItemGridLookUpEdit)(object)LookUpEditUserNameInvate).AutoComplete = false;
			((RepositoryItem)(object)LookUpEditUserNameInvate).AutoHeight = false;
			((RepositoryItemButtonEdit)(object)LookUpEditUserNameInvate).Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			((RepositoryItem)(object)LookUpEditUserNameInvate).Name = "LookUpEditUserNameInvate";
			((RepositoryItem)(object)LookUpEditUserNameInvate).NullText = "";
			((RepositoryItemButtonEdit)(object)LookUpEditUserNameInvate).TextEditStyle = TextEditStyles.Standard;
			((RepositoryItemGridLookUpEditBase)(object)LookUpEditUserNameInvate).View = (GridView)(object)customGridView2;
			((RepositoryItem)(object)LookUpEditUserNameInvate).EditValueChanged += LookUpEditUserNameInvate_EditValueChanged;
			((GridView)(object)customGridView2).FocusRectStyle = DrawFocusRectStyle.RowFocus;
			((BaseView)(object)customGridView2).Name = "customGridView2";
			((GridView)(object)customGridView2).OptionsSelection.EnableAppearanceFocusedCell = false;
			((GridView)(object)customGridView2).OptionsView.ShowGroupPanel = false;
			gridColumn4.Caption = "Chủ tọa";
			gridColumn4.ColumnEdit = rechkThuKy;
			gridColumn4.FieldName = "PRESIDENT";
			gridColumn4.FieldNameSortGroup = "PRESIDENTUnb";
			gridColumn4.MaxWidth = 50;
			gridColumn4.MinWidth = 50;
			gridColumn4.Name = "gridColumn4";
			gridColumn4.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn4.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn4.Visible = true;
			gridColumn4.VisibleIndex = 3;
			gridColumn4.Width = 50;
			rechkThuKy.AutoHeight = false;
			rechkThuKy.Name = "rechkThuKy";
			rechkThuKy.NullStyle = StyleIndeterminate.Unchecked;
			gridColumn5.Caption = "Thư ký";
			gridColumn5.ColumnEdit = rechkChuToa;
			gridColumn5.FieldName = "SECRETARY";
			gridColumn5.FieldNameSortGroup = "SECRETARYUnb";
			gridColumn5.MaxWidth = 50;
			gridColumn5.MinWidth = 50;
			gridColumn5.Name = "gridColumn5";
			gridColumn5.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn5.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn5.Visible = true;
			gridColumn5.VisibleIndex = 4;
			gridColumn5.Width = 50;
			rechkChuToa.AutoHeight = false;
			rechkChuToa.Name = "rechkChuToa";
			rechkChuToa.NullStyle = StyleIndeterminate.Unchecked;
			gridColumn6.Caption = "Chức danh";
			gridColumn6.ColumnEdit = (RepositoryItem)(object)recboExecuteRole;
			gridColumn6.FieldName = "EXECUTE_ROLE_ID";
			gridColumn6.FieldNameSortGroup = "EXECUTE_ROLE_IDUnb";
			gridColumn6.Name = "gridColumn6";
			gridColumn6.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn6.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn6.Visible = true;
			gridColumn6.VisibleIndex = 5;
			gridColumn6.Width = 144;
			((RepositoryItemGridLookUpEdit)(object)recboExecuteRole).AutoComplete = false;
			((RepositoryItem)(object)recboExecuteRole).AutoHeight = false;
			((RepositoryItemButtonEdit)(object)recboExecuteRole).Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Plus)
			});
			((RepositoryItem)(object)recboExecuteRole).Name = "recboExecuteRole";
			((RepositoryItem)(object)recboExecuteRole).NullText = "";
			((RepositoryItemButtonEdit)(object)recboExecuteRole).TextEditStyle = TextEditStyles.Standard;
			((RepositoryItemGridLookUpEditBase)(object)recboExecuteRole).View = (GridView)(object)customGridView3;
			((RepositoryItemButtonEdit)(object)recboExecuteRole).ButtonClick += recboExecuteRole_ButtonClick;
			((GridView)(object)customGridView3).FocusRectStyle = DrawFocusRectStyle.RowFocus;
			((BaseView)(object)customGridView3).Name = "customGridView3";
			((GridView)(object)customGridView3).OptionsSelection.EnableAppearanceFocusedCell = false;
			((GridView)(object)customGridView3).OptionsView.ShowGroupPanel = false;
			gridColumn7.Caption = "Nhận xét";
			gridColumn7.ColumnEdit = reTxtComment;
			gridColumn7.FieldName = "COMMENT_DOCTOR";
			gridColumn7.FieldNameSortGroup = "COMMENT_DOCTORUnb";
			gridColumn7.Name = "gridColumn7";
			gridColumn7.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn7.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn7.Visible = true;
			gridColumn7.VisibleIndex = 6;
			gridColumn7.Width = 107;
			reTxtComment.AutoHeight = false;
			reTxtComment.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Down)
			});
			reTxtComment.Name = "reTxtComment";
			reTxtComment.ButtonClick += reTxtComment_ButtonClick;
			reTxtComment.EditValueChanged += reTxtComment_EditValueChanged;
			reTxtComment.KeyDown += reTxtComment_KeyDown;
			reTxtComment.Leave += reTxtComment_Leave;
			gridColumn8.Caption = "gridColumn8";
			gridColumn8.ColumnEdit = rebtnAddInvateUser;
			gridColumn8.FieldName = "BtnDeleteInvateUser";
			gridColumn8.FieldNameSortGroup = "BtnDeleteInvateUserUnb";
			gridColumn8.MaxWidth = 25;
			gridColumn8.MinWidth = 25;
			gridColumn8.Name = "gridColumn8";
			gridColumn8.OptionsColumn.AllowSort = DefaultBoolean.False;
			gridColumn8.OptionsColumn.ShowCaption = false;
			gridColumn8.OptionsFilter.FilterBySortField = DefaultBoolean.True;
			gridColumn8.Visible = true;
			gridColumn8.VisibleIndex = 7;
			gridColumn8.Width = 25;
			rebtnAddInvateUser.AutoHeight = false;
			rebtnAddInvateUser.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Plus)
			});
			rebtnAddInvateUser.Name = "rebtnAddInvateUser";
			rebtnAddInvateUser.TextEditStyle = TextEditStyles.HideTextEditor;
			rebtnAddInvateUser.ButtonClick += rebtnAddInvateUser_ButtonClick;
			gcLoginName.Caption = "gridColumn9";
			gcLoginName.FieldName = "LOGINNAME";
			gcLoginName.Name = "gcLoginName";
			gridColumn9.Caption = "gridColumn9";
			gridColumn9.FieldName = "IS_PARTICIPATION";
			gridColumn9.Name = "gridColumn9";
			gridColumn10.Caption = "gridColumn10";
			gridColumn10.FieldName = "ID";
			gridColumn10.Name = "gridColumn10";
			colFeedBackUnb.FieldName = "FeedBackUnb";
			colFeedBackUnb.Name = "colFeedBackUnb";
			colFeedBackUnb.UnboundType = UnboundColumnType.String;
			colIS_PARTICIPATION_strUnb.FieldName = "IS_PARTICIPATION_strUnb";
			colIS_PARTICIPATION_strUnb.Name = "colIS_PARTICIPATION_strUnb";
			colIS_PARTICIPATION_strUnb.UnboundType = UnboundColumnType.String;
			colUSERNAMEUnb.FieldName = "USERNAMEUnb";
			colUSERNAMEUnb.Name = "colUSERNAMEUnb";
			colUSERNAMEUnb.UnboundType = UnboundColumnType.String;
			colPRESIDENTUnb.FieldName = "PRESIDENTUnb";
			colPRESIDENTUnb.Name = "colPRESIDENTUnb";
			colPRESIDENTUnb.UnboundType = UnboundColumnType.String;
			colSECRETARYUnb.FieldName = "SECRETARYUnb";
			colSECRETARYUnb.Name = "colSECRETARYUnb";
			colSECRETARYUnb.UnboundType = UnboundColumnType.String;
			colEXECUTE_ROLE_IDUnb.FieldName = "EXECUTE_ROLE_IDUnb";
			colEXECUTE_ROLE_IDUnb.Name = "colEXECUTE_ROLE_IDUnb";
			colEXECUTE_ROLE_IDUnb.UnboundType = UnboundColumnType.String;
			colCOMMENT_DOCTORUnb.FieldName = "COMMENT_DOCTORUnb";
			colCOMMENT_DOCTORUnb.Name = "colCOMMENT_DOCTORUnb";
			colCOMMENT_DOCTORUnb.UnboundType = UnboundColumnType.String;
			colBtnDeleteInvateUserUnb.FieldName = "BtnDeleteInvateUserUnb";
			colBtnDeleteInvateUserUnb.Name = "colBtnDeleteInvateUserUnb";
			colBtnDeleteInvateUserUnb.UnboundType = UnboundColumnType.String;
			rebtnFeedBackDisable.AutoHeight = false;
			rebtnFeedBackDisable.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Glyph, "", -1, true, true, false, ImageLocation.MiddleCenter, (Image)resources.GetObject("rebtnFeedBackDisable.Buttons"), new KeyShortcut(Keys.None), appearance3, appearanceHovered3, appearancePressed3, appearanceDisabled3, "Lấy ý kiến nhận xét của thành viên", null, null, true)
			});
			rebtnFeedBackDisable.Name = "rebtnFeedBackDisable";
			rebtnFeedBackDisable.TextEditStyle = TextEditStyles.HideTextEditor;
			rebtnMinusInvateUser.AutoHeight = false;
			rebtnMinusInvateUser.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Minus)
			});
			rebtnMinusInvateUser.Name = "rebtnMinusInvateUser";
			rebtnMinusInvateUser.TextEditStyle = TextEditStyles.HideTextEditor;
			rebtnMinusInvateUser.ButtonClick += rebtnMinusInvateUser_ButtonClick;
			recboThamGia.AllowNullInput = DefaultBoolean.True;
			recboThamGia.AutoHeight = false;
			recboThamGia.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			recboThamGia.Items.AddRange(new object[2] { "Có", "Không" });
			recboThamGia.Name = "recboThamGia";
			reTxtDisable.AutoHeight = false;
			reTxtDisable.Name = "reTxtDisable";
			reTxtDisable.ReadOnly = true;
			rechkThuKyDisable.AutoHeight = false;
			rechkThuKyDisable.Name = "rechkThuKyDisable";
			rechkThuKyDisable.ReadOnly = true;
			rechkChuToaDisable.AutoHeight = false;
			rechkChuToaDisable.Name = "rechkChuToaDisable";
			rechkChuToaDisable.ReadOnly = true;
			cboBienBanHoiChanCu.Location = new Point(923, 2);
			cboBienBanHoiChanCu.MenuManager = barManager1;
			cboBienBanHoiChanCu.Name = "cboBienBanHoiChanCu";
			cboBienBanHoiChanCu.Properties.AllowNullInput = DefaultBoolean.True;
			cboBienBanHoiChanCu.Properties.Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete)
			});
			cboBienBanHoiChanCu.Properties.NullText = "";
			cboBienBanHoiChanCu.Properties.View = gridView4;
			cboBienBanHoiChanCu.Size = new Size(437, 20);
			cboBienBanHoiChanCu.StyleController = layoutControl1;
			cboBienBanHoiChanCu.TabIndex = 37;
			cboBienBanHoiChanCu.Closed += cboBienBanHoiChanCu_Closed;
			cboBienBanHoiChanCu.ButtonClick += cboBienBanHoiChanCu_ButtonClick;
			gridView4.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridView4.Name = "gridView4";
			gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridView4.OptionsView.ShowGroupPanel = false;
			dxValidationProvider1.SetIconAlignment(cboPhieuDieuTri, ErrorIconAlignment.MiddleRight);
			cboPhieuDieuTri.Location = new Point(652, 2);
			cboPhieuDieuTri.MenuManager = barManager1;
			cboPhieuDieuTri.Name = "cboPhieuDieuTri";
			cboPhieuDieuTri.Properties.AllowNullInput = DefaultBoolean.True;
			cboPhieuDieuTri.Properties.Buttons.AddRange(new EditorButton[3]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete),
				new EditorButton(ButtonPredefines.Plus)
			});
			cboPhieuDieuTri.Properties.NullText = "";
			cboPhieuDieuTri.Properties.View = gridView3;
			cboPhieuDieuTri.Size = new Size(132, 20);
			cboPhieuDieuTri.StyleController = layoutControl1;
			cboPhieuDieuTri.TabIndex = 36;
			cboPhieuDieuTri.Closed += cboPhieuDieuTri_Closed;
			cboPhieuDieuTri.ButtonClick += cboPhieuDieuTri_ButtonClick;
			cboPhieuDieuTri.EditValueChanged += cboPhieuDieuTri_EditValueChanged;
			gridView3.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridView3.Name = "gridView3";
			gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridView3.OptionsView.ShowGroupPanel = false;
			gridControl.Location = new Point(142, 146);
			gridControl.MainView = gridView;
			gridControl.Name = "gridControl";
			gridControl.RepositoryItems.AddRange(new RepositoryItem[12]
			{
				CheckEditChuToa,
				CheckEditThuKy,
				CheckEditChuToaDisable,
				CheckEditThuKyDisable,
				ButtonAdd,
				ButtonDelete,
				(RepositoryItem)(object)LookUpEditUserName,
				TextEditLoginName,
				(RepositoryItem)(object)repositoryItemCboExecuteRole,
				CheckEditBsNgoaiVien,
				TextEditUserName,
				TextEditLoginNameDis
			});
			gridControl.Size = new Size(551, 129);
			gridControl.TabIndex = 4;
			gridControl.ViewCollection.AddRange(new BaseView[1] { gridView });
			gridView.Columns.AddRange(new GridColumn[8] { gridColumnParticipants_Id, Gc_LoginName, Gc_UserName, Gc_President, Gc_Secretary, Gc_OutOfHospital, Gc_Titles, Gc_Add });
			gridView.GridControl = gridControl;
			gridView.Name = "gridView";
			gridView.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			gridView.OptionsView.ShowDetailButtons = false;
			gridView.OptionsView.ShowGroupPanel = false;
			gridView.OptionsView.ShowIndicator = false;
			gridView.CustomRowCellEdit += gridView_CustomRowCellEdit;
			gridView.ShownEditor += gridView_ShownEditor;
			gridView.FocusedColumnChanged += gridView_FocusedColumnChanged;
			gridView.CellValueChanged += gridView_CellValueChanged;
			gridView.CustomUnboundColumnData += gridView_CustomUnboundColumnData;
			gridColumnParticipants_Id.Caption = "gridColumn1";
			gridColumnParticipants_Id.FieldName = "ID";
			gridColumnParticipants_Id.Name = "gridColumnParticipants_Id";
			Gc_LoginName.Caption = "Tài khoản";
			Gc_LoginName.ColumnEdit = TextEditLoginName;
			Gc_LoginName.FieldName = "LOGINNAME";
			Gc_LoginName.Name = "Gc_LoginName";
			Gc_LoginName.OptionsColumn.ShowCaption = false;
			Gc_LoginName.ToolTip = "Người dùng không có trong danh mục hãy nhập vào đây";
			Gc_LoginName.Visible = true;
			Gc_LoginName.VisibleIndex = 0;
			Gc_LoginName.Width = 39;
			TextEditLoginName.AutoHeight = false;
			TextEditLoginName.Name = "TextEditLoginName";
			TextEditLoginName.KeyDown += TextEditLoginName_KeyDown;
			Gc_UserName.Caption = "Họ và tên";
			Gc_UserName.FieldName = "USERNAME";
			Gc_UserName.Name = "Gc_UserName";
			Gc_UserName.Visible = true;
			Gc_UserName.VisibleIndex = 1;
			Gc_UserName.Width = 158;
			Gc_President.AppearanceCell.Options.UseTextOptions = true;
			Gc_President.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			Gc_President.AppearanceHeader.Options.UseTextOptions = true;
			Gc_President.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			Gc_President.Caption = "Chủ tọa";
			Gc_President.ColumnEdit = CheckEditChuToa;
			Gc_President.FieldName = "PRESIDENT";
			Gc_President.Name = "Gc_President";
			Gc_President.Visible = true;
			Gc_President.VisibleIndex = 2;
			Gc_President.Width = 56;
			CheckEditChuToa.AutoHeight = false;
			CheckEditChuToa.Name = "CheckEditChuToa";
			CheckEditChuToa.NullStyle = StyleIndeterminate.Unchecked;
			Gc_Secretary.Caption = "Thư ký";
			Gc_Secretary.ColumnEdit = CheckEditThuKy;
			Gc_Secretary.FieldName = "SECRETARY";
			Gc_Secretary.Name = "Gc_Secretary";
			Gc_Secretary.Visible = true;
			Gc_Secretary.VisibleIndex = 3;
			Gc_Secretary.Width = 46;
			CheckEditThuKy.AutoHeight = false;
			CheckEditThuKy.Name = "CheckEditThuKy";
			CheckEditThuKy.NullStyle = StyleIndeterminate.Unchecked;
			Gc_OutOfHospital.Caption = "BS ngoại viện";
			Gc_OutOfHospital.ColumnEdit = CheckEditBsNgoaiVien;
			Gc_OutOfHospital.FieldName = "OUT_OF_HOSPITAL";
			Gc_OutOfHospital.Name = "Gc_OutOfHospital";
			Gc_OutOfHospital.ToolTip = "Bác sĩ ngoại viện";
			Gc_OutOfHospital.Visible = true;
			Gc_OutOfHospital.VisibleIndex = 4;
			Gc_OutOfHospital.Width = 80;
			CheckEditBsNgoaiVien.AutoHeight = false;
			CheckEditBsNgoaiVien.Name = "CheckEditBsNgoaiVien";
			CheckEditBsNgoaiVien.NullStyle = StyleIndeterminate.Unchecked;
			CheckEditBsNgoaiVien.CheckedChanged += CheckEditBsNgoaiVien_CheckedChanged;
			Gc_Titles.Caption = "Chức danh - Chức vụ";
			Gc_Titles.ColumnEdit = (RepositoryItem)(object)repositoryItemCboExecuteRole;
			Gc_Titles.FieldName = "EXECUTE_ROLE_ID";
			Gc_Titles.Name = "Gc_Titles";
			Gc_Titles.Visible = true;
			Gc_Titles.VisibleIndex = 5;
			Gc_Titles.Width = 150;
			((RepositoryItemGridLookUpEdit)(object)repositoryItemCboExecuteRole).AutoComplete = false;
			((RepositoryItem)(object)repositoryItemCboExecuteRole).AutoHeight = false;
			((RepositoryItemButtonEdit)(object)repositoryItemCboExecuteRole).Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Plus)
			});
			((RepositoryItem)(object)repositoryItemCboExecuteRole).Name = "repositoryItemCboExecuteRole";
			((RepositoryItem)(object)repositoryItemCboExecuteRole).NullText = "";
			((RepositoryItemButtonEdit)(object)repositoryItemCboExecuteRole).TextEditStyle = TextEditStyles.Standard;
			((RepositoryItemGridLookUpEditBase)(object)repositoryItemCboExecuteRole).View = (GridView)(object)repositoryItemCustomGridLookUpEdit1View;
			((RepositoryItemButtonEdit)(object)repositoryItemCboExecuteRole).ButtonClick += repositoryItemCboExecuteRole_ButtonClick;
			((RepositoryItem)(object)repositoryItemCboExecuteRole).EditValueChanged += repositoryItemCboExecuteRole_EditValueChanged;
			((GridView)(object)repositoryItemCustomGridLookUpEdit1View).FocusRectStyle = DrawFocusRectStyle.RowFocus;
			((BaseView)(object)repositoryItemCustomGridLookUpEdit1View).Name = "repositoryItemCustomGridLookUpEdit1View";
			((GridView)(object)repositoryItemCustomGridLookUpEdit1View).OptionsSelection.EnableAppearanceFocusedCell = false;
			((GridView)(object)repositoryItemCustomGridLookUpEdit1View).OptionsView.ShowGroupPanel = false;
			Gc_Add.AppearanceCell.Options.UseTextOptions = true;
			Gc_Add.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			Gc_Add.Caption = "Thêm";
			Gc_Add.ColumnEdit = ButtonAdd;
			Gc_Add.FieldName = "BtnDelete";
			Gc_Add.MaxWidth = 25;
			Gc_Add.MinWidth = 25;
			Gc_Add.Name = "Gc_Add";
			Gc_Add.OptionsColumn.ShowCaption = false;
			Gc_Add.Visible = true;
			Gc_Add.VisibleIndex = 6;
			Gc_Add.Width = 25;
			ButtonAdd.AutoHeight = false;
			ButtonAdd.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Plus)
			});
			ButtonAdd.Name = "ButtonAdd";
			ButtonAdd.TextEditStyle = TextEditStyles.HideTextEditor;
			ButtonAdd.ButtonClick += ButtonAdd_ButtonClick;
			CheckEditChuToaDisable.AutoHeight = false;
			CheckEditChuToaDisable.Name = "CheckEditChuToaDisable";
			CheckEditThuKyDisable.AutoHeight = false;
			CheckEditThuKyDisable.Name = "CheckEditThuKyDisable";
			ButtonDelete.AutoHeight = false;
			ButtonDelete.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Minus)
			});
			ButtonDelete.Name = "ButtonDelete";
			ButtonDelete.TextEditStyle = TextEditStyles.HideTextEditor;
			ButtonDelete.ButtonClick += ButtonDelete_ButtonClick;
			((RepositoryItemGridLookUpEdit)(object)LookUpEditUserName).AutoComplete = false;
			((RepositoryItem)(object)LookUpEditUserName).AutoHeight = false;
			((RepositoryItemButtonEdit)(object)LookUpEditUserName).Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Clear, "", -1, true, false, false, ImageLocation.MiddleCenter, null, new KeyShortcut(Keys.None), appearance4, appearanceHovered4, appearancePressed4, appearanceDisabled4, "", null, null, true)
			});
			((RepositoryItem)(object)LookUpEditUserName).Name = "LookUpEditUserName";
			((RepositoryItem)(object)LookUpEditUserName).NullText = "";
			((RepositoryItemButtonEdit)(object)LookUpEditUserName).TextEditStyle = TextEditStyles.Standard;
			((RepositoryItem)(object)LookUpEditUserName).EditValueChanged += LookUpEditUserName_EditValueChanged;
			TextEditUserName.AutoHeight = false;
			TextEditUserName.Name = "TextEditUserName";
			TextEditLoginNameDis.AutoHeight = false;
			TextEditLoginNameDis.Name = "TextEditLoginNameDis";
			TextEditLoginNameDis.ReadOnly = true;
			ChkOther.Location = new Point(452, 279);
			ChkOther.MenuManager = barManager1;
			ChkOther.Name = "ChkOther";
			ChkOther.Properties.Caption = "Khác";
			ChkOther.Properties.CheckStyle = CheckStyles.Radio;
			ChkOther.Size = new Size(106, 19);
			ChkOther.StyleController = layoutControl1;
			ChkOther.TabIndex = 35;
			ChkOther.CheckedChanged += ChkOther_CheckedChanged;
			ChkPttt.Location = new Point(272, 279);
			ChkPttt.MenuManager = barManager1;
			ChkPttt.Name = "ChkPttt";
			ChkPttt.Properties.Caption = "Hội chẩn trước phẫu thuật";
			ChkPttt.Properties.CheckStyle = CheckStyles.Radio;
			ChkPttt.Size = new Size(176, 19);
			ChkPttt.StyleController = layoutControl1;
			ChkPttt.TabIndex = 34;
			ChkPttt.CheckedChanged += ChkPttt_CheckedChanged;
			CheckThuoc.Location = new Point(142, 279);
			CheckThuoc.MenuManager = barManager1;
			CheckThuoc.Name = "CheckThuoc";
			CheckThuoc.Properties.Caption = "Hội chẩn thuốc";
			CheckThuoc.Properties.CheckStyle = CheckStyles.Radio;
			CheckThuoc.Size = new Size(126, 19);
			CheckThuoc.StyleController = layoutControl1;
			CheckThuoc.TabIndex = 33;
			CheckThuoc.CheckedChanged += CheckThuoc_CheckedChanged;
			panelDetail.BorderStyle = BorderStyles.NoBorder;
			panelDetail.Location = new Point(2, 303);
			panelDetail.Name = "panelDetail";
			panelDetail.Size = new Size(1358, 389);
			panelDetail.TabIndex = 32;
			chkAutoCreateEmr.Location = new Point(477, 696);
			chkAutoCreateEmr.MenuManager = barManager1;
			chkAutoCreateEmr.Name = "chkAutoCreateEmr";
			chkAutoCreateEmr.Properties.Caption = "";
			chkAutoCreateEmr.Properties.FullFocusRect = true;
			chkAutoCreateEmr.Size = new Size(21, 19);
			chkAutoCreateEmr.StyleController = layoutControl1;
			chkAutoCreateEmr.TabIndex = 31;
			chkAutoCreateEmr.CheckedChanged += chkAutoCreateEmr_CheckedChanged;
			chkAutoSign.Location = new Point(777, 696);
			chkAutoSign.MenuManager = barManager1;
			chkAutoSign.Name = "chkAutoSign";
			chkAutoSign.Properties.Caption = "";
			chkAutoSign.Size = new Size(21, 19);
			chkAutoSign.StyleController = layoutControl1;
			chkAutoSign.TabIndex = 30;
			chkAutoSign.CheckedChanged += chkAutoSign_CheckedChanged;
			cboDebateType.Location = new Point(142, 122);
			cboDebateType.MenuManager = barManager1;
			cboDebateType.Name = "cboDebateType";
			cboDebateType.Properties.AllowNullInput = DefaultBoolean.True;
			cboDebateType.Properties.Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete)
			});
			cboDebateType.Properties.NullText = "";
			cboDebateType.Properties.View = gridLookUpEdit1View;
			cboDebateType.Size = new Size(232, 20);
			cboDebateType.StyleController = layoutControl1;
			cboDebateType.TabIndex = 29;
			cboDebateType.Closed += cboDebateType_Closed;
			cboDebateType.ButtonClick += cboDebateType_ButtonClick;
			cboDebateType.PreviewKeyDown += cboDebateType_PreviewKeyDown;
			gridLookUpEdit1View.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			panelControl1.BorderStyle = BorderStyles.NoBorder;
			panelControl1.Controls.Add(cboIcdMain);
			panelControl1.Controls.Add(icdMainText);
			panelControl1.Location = new Point(222, 26);
			panelControl1.Name = "panelControl1";
			panelControl1.Size = new Size(832, 20);
			panelControl1.TabIndex = 28;
			cboIcdMain.Dock = DockStyle.Fill;
			cboIcdMain.Location = new Point(0, 0);
			cboIcdMain.MenuManager = barManager1;
			cboIcdMain.Name = "cboIcdMain";
			cboIcdMain.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			cboIcdMain.Properties.NullText = "";
			cboIcdMain.Properties.View = gridLookUpEdit3View;
			cboIcdMain.Size = new Size(832, 20);
			cboIcdMain.TabIndex = 1;
			cboIcdMain.Closed += cboIcdMain_Closed;
			gridLookUpEdit3View.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridLookUpEdit3View.Name = "gridLookUpEdit3View";
			gridLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridLookUpEdit3View.OptionsView.ShowGroupPanel = false;
			icdMainText.Dock = DockStyle.Fill;
			icdMainText.Location = new Point(0, 0);
			icdMainText.MenuManager = barManager1;
			icdMainText.Name = "icdMainText";
			icdMainText.Size = new Size(832, 20);
			icdMainText.TabIndex = 0;
			icdMainText.PreviewKeyDown += icdMainText_PreviewKeyDown;
			txtLocation.Location = new Point(846, 122);
			txtLocation.MenuManager = barManager1;
			txtLocation.Name = "txtLocation";
			txtLocation.Size = new Size(514, 20);
			txtLocation.StyleController = layoutControl1;
			txtLocation.TabIndex = 27;
			txtLocation.PreviewKeyDown += txtLocation_PreviewKeyDown;
			cboRequestLoggin.Location = new Point(651, 98);
			cboRequestLoggin.MenuManager = barManager1;
			cboRequestLoggin.Name = "cboRequestLoggin";
			cboRequestLoggin.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			cboRequestLoggin.Properties.NullText = "";
			cboRequestLoggin.Properties.View = gridLookUpEdit2View;
			cboRequestLoggin.Size = new Size(709, 20);
			cboRequestLoggin.StyleController = layoutControl1;
			cboRequestLoggin.TabIndex = 25;
			cboRequestLoggin.Closed += cboRequestLoggin_Closed;
			gridLookUpEdit2View.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridLookUpEdit2View.Name = "gridLookUpEdit2View";
			gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
			cboDepartment.Location = new Point(846, 74);
			cboDepartment.MenuManager = barManager1;
			cboDepartment.Name = "cboDepartment";
			cboDepartment.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			cboDepartment.Properties.NullText = "";
			cboDepartment.Properties.View = gridView2;
			cboDepartment.Size = new Size(514, 20);
			cboDepartment.StyleController = layoutControl1;
			cboDepartment.TabIndex = 24;
			cboDepartment.Closed += cboDepartment_Closed;
			gridView2.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridView2.Name = "gridView2";
			gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridView2.OptionsView.ShowGroupPanel = false;
			dtOutTime.EditValue = null;
			dtOutTime.Location = new Point(498, 74);
			dtOutTime.MenuManager = barManager1;
			dtOutTime.Name = "dtOutTime";
			dtOutTime.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtOutTime.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtOutTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtOutTime.Properties.DisplayFormat.FormatType = FormatType.Custom;
			dtOutTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtOutTime.Properties.EditFormat.FormatType = FormatType.Custom;
			dtOutTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
			dtOutTime.Size = new Size(224, 20);
			dtOutTime.StyleController = layoutControl1;
			dtOutTime.TabIndex = 23;
			dtOutTime.PreviewKeyDown += dtOutTime_PreviewKeyDown;
			dtInTime.EditValue = null;
			dtInTime.Location = new Point(142, 74);
			dtInTime.MenuManager = barManager1;
			dtInTime.Name = "dtInTime";
			dtInTime.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtInTime.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtInTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtInTime.Properties.DisplayFormat.FormatType = FormatType.Custom;
			dtInTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtInTime.Properties.EditFormat.FormatType = FormatType.Custom;
			dtInTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
			dtInTime.Size = new Size(232, 20);
			dtInTime.StyleController = layoutControl1;
			dtInTime.TabIndex = 22;
			dtInTime.PreviewKeyDown += dtInTime_PreviewKeyDown;
			txtDebateTemp.Location = new Point(142, 2);
			txtDebateTemp.MenuManager = barManager1;
			txtDebateTemp.Name = "txtDebateTemp";
			txtDebateTemp.Size = new Size(80, 20);
			txtDebateTemp.StyleController = layoutControl1;
			txtDebateTemp.TabIndex = 20;
			txtDebateTemp.KeyDown += txtDebateTemp_KeyDown;
			btnSaveTemp.Location = new Point(1254, 696);
			btnSaveTemp.Name = "btnSaveTemp";
			btnSaveTemp.Size = new Size(106, 22);
			btnSaveTemp.StyleController = layoutControl1;
			btnSaveTemp.TabIndex = 19;
			btnSaveTemp.Text = "Lưu mẫu (Ctrl T)";
			btnSaveTemp.Click += btnSaveTemp_Click;
			txtIcdTextName.Location = new Point(376, 50);
			txtIcdTextName.MenuManager = barManager1;
			txtIcdTextName.Name = "txtIcdTextName";
			txtIcdTextName.Properties.NullValuePrompt = "Nhấn F1 để chọn bệnh";
			txtIcdTextName.Properties.NullValuePromptShowForEmptyValue = true;
			txtIcdTextName.Properties.ShowNullValuePromptWhenFocused = true;
			txtIcdTextName.Size = new Size(984, 20);
			txtIcdTextName.StyleController = layoutControl1;
			txtIcdTextName.TabIndex = 18;
			txtIcdTextName.PreviewKeyDown += lciIcdTextName_PreviewKeyDown;
			dtDebateTime.EditValue = null;
			dtDebateTime.Location = new Point(142, 98);
			dtDebateTime.Name = "dtDebateTime";
			dtDebateTime.Properties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtDebateTime.Properties.CalendarTimeProperties.Buttons.AddRange(new EditorButton[1]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			dtDebateTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtDebateTime.Properties.DisplayFormat.FormatType = FormatType.Custom;
			dtDebateTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			dtDebateTime.Properties.EditFormat.FormatType = FormatType.Custom;
			dtDebateTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
			dtDebateTime.Size = new Size(232, 20);
			dtDebateTime.StyleController = layoutControl1;
			dtDebateTime.TabIndex = 15;
			dtDebateTime.EditValueChanged += dtDebateTime_EditValueChanged;
			dtDebateTime.PreviewKeyDown += dtDebateTime_PreviewKeyDown;
			btnPrint.Location = new Point(1144, 696);
			btnPrint.Name = "btnPrint";
			btnPrint.Size = new Size(106, 22);
			btnPrint.StyleController = layoutControl1;
			btnPrint.TabIndex = 14;
			btnPrint.Text = "In ấn";
			btnSave.Location = new Point(1034, 696);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(106, 22);
			btnSave.StyleController = layoutControl1;
			btnSave.TabIndex = 13;
			btnSave.Text = "Lưu (Ctrl S)";
			btnSave.Click += btnSave_Click;
			txtIcdTextCode.Location = new Point(142, 50);
			txtIcdTextCode.Name = "txtIcdTextCode";
			txtIcdTextCode.Properties.CharacterCasing = CharacterCasing.Upper;
			txtIcdTextCode.Size = new Size(234, 20);
			txtIcdTextCode.StyleController = layoutControl1;
			txtIcdTextCode.TabIndex = 9;
			txtIcdTextCode.PreviewKeyDown += txtIcdText_PreviewKeyDown;
			txtIcdTextCode.Validating += txtIcdTextCode_Validating;
			checkEdit.Location = new Point(1093, 26);
			checkEdit.Name = "checkEdit";
			checkEdit.Properties.Caption = "";
			checkEdit.Size = new Size(19, 19);
			checkEdit.StyleController = layoutControl1;
			checkEdit.TabIndex = 8;
			checkEdit.CheckedChanged += checkEdit_CheckedChanged;
			checkEdit.PreviewKeyDown += checkEdit_PreviewKeyDown;
			txtIcdMain.Location = new Point(142, 26);
			txtIcdMain.Name = "txtIcdMain";
			txtIcdMain.Size = new Size(80, 20);
			txtIcdMain.StyleController = layoutControl1;
			txtIcdMain.TabIndex = 6;
			txtIcdMain.PreviewKeyDown += txtIcdMain_PreviewKeyDown;
			txtRequestLoggin.Location = new Point(498, 98);
			txtRequestLoggin.Name = "txtRequestLoggin";
			txtRequestLoggin.Properties.MaxLength = 1000;
			txtRequestLoggin.Size = new Size(153, 20);
			txtRequestLoggin.StyleController = layoutControl1;
			txtRequestLoggin.TabIndex = 5;
			txtRequestLoggin.PreviewKeyDown += txtRequestContent_PreviewKeyDown;
			cboDebateTemp.Location = new Point(222, 2);
			cboDebateTemp.MenuManager = barManager1;
			cboDebateTemp.Name = "cboDebateTemp";
			cboDebateTemp.Properties.AllowNullInput = DefaultBoolean.True;
			cboDebateTemp.Properties.Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete)
			});
			cboDebateTemp.Properties.NullText = "";
			cboDebateTemp.Properties.View = gridView1;
			cboDebateTemp.Size = new Size(243, 20);
			cboDebateTemp.StyleController = layoutControl1;
			cboDebateTemp.TabIndex = 21;
			cboDebateTemp.Closed += cboDebateTemp_Closed;
			cboDebateTemp.ButtonClick += cboDebateTemp_ButtonClick;
			gridView1.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridView1.Name = "gridView1";
			gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridView1.OptionsView.ShowGroupPanel = false;
			cboDebateReason.Location = new Point(498, 122);
			cboDebateReason.MenuManager = barManager1;
			cboDebateReason.Name = "cboDebateReason";
			cboDebateReason.Properties.AllowNullInput = DefaultBoolean.True;
			cboDebateReason.Properties.Buttons.AddRange(new EditorButton[2]
			{
				new EditorButton(ButtonPredefines.Combo),
				new EditorButton(ButtonPredefines.Delete)
			});
			cboDebateReason.Properties.NullText = "";
			cboDebateReason.Properties.PopupSizeable = false;
			cboDebateReason.Properties.TextEditStyle = TextEditStyles.Standard;
			cboDebateReason.Properties.View = gridView6;
			cboDebateReason.Size = new Size(224, 20);
			cboDebateReason.StyleController = layoutControl1;
			cboDebateReason.TabIndex = 42;
			cboDebateReason.ButtonClick += cboDebateReason_ButtonClick;
			gridView6.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			gridView6.Name = "gridView6";
			gridView6.OptionsSelection.EnableAppearanceFocusedCell = false;
			gridView6.OptionsView.ShowGroupPanel = false;
			layoutControlGroup1.EnableIndentsWithoutBorders = DefaultBoolean.True;
			layoutControlGroup1.GroupBordersVisible = false;
			layoutControlGroup1.Items.AddRange(new BaseLayoutItem[36]
			{
				lciRequestContent, layoutControlItem11, layoutControlItem12, lciDebateTime, LcDebateTemp, layoutControlItem4, lciIcdMain, lciCheckEdit, lciIcdSubCode1, lciIcdSubCode,
				layoutControlItem2, layoutControlItem5, layoutControlItem6, layoutControlItem13, icdLocation, layoutControlItem3, layoutControlItem15, layoutControlItem1, layoutControlItem9, layoutControlItem10,
				layoutControlItem14, layoutControlItem16, lciAutoCreateEmr, layoutControlItem8, emptySpaceItem3, layoutControlItem7, layoutControlItem17, emptySpaceItem2, lciAutoSign, layoutControlItem18,
				layoutControlItem19, layoutControlItem23, layoutControlItem24, layoutControlItem25, layoutControlItem26, emptySpaceItem4
			});
			layoutControlGroup1.Location = new Point(0, 0);
			layoutControlGroup1.Name = "layoutControlGroup1";
			layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			layoutControlGroup1.Size = new Size(1362, 720);
			layoutControlGroup1.TextVisible = false;
			lciRequestContent.AppearanceItemCaption.Options.UseTextOptions = true;
			lciRequestContent.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciRequestContent.Control = txtRequestLoggin;
			lciRequestContent.Location = new Point(376, 96);
			lciRequestContent.Name = "lciRequestContent";
			lciRequestContent.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			lciRequestContent.Size = new Size(275, 24);
			lciRequestContent.Text = "Người yêu cầu: ";
			lciRequestContent.TextAlignMode = TextAlignModeItem.CustomSize;
			lciRequestContent.TextSize = new Size(115, 20);
			lciRequestContent.TextToControlDistance = 5;
			layoutControlItem11.Control = btnSave;
			layoutControlItem11.Location = new Point(1032, 694);
			layoutControlItem11.MaxSize = new Size(110, 26);
			layoutControlItem11.MinSize = new Size(110, 26);
			layoutControlItem11.Name = "layoutControlItem11";
			layoutControlItem11.Size = new Size(110, 26);
			layoutControlItem11.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem11.TextSize = new Size(0, 0);
			layoutControlItem11.TextVisible = false;
			layoutControlItem12.Control = btnPrint;
			layoutControlItem12.Location = new Point(1142, 694);
			layoutControlItem12.MaxSize = new Size(110, 26);
			layoutControlItem12.MinSize = new Size(110, 26);
			layoutControlItem12.Name = "layoutControlItem12";
			layoutControlItem12.Size = new Size(110, 26);
			layoutControlItem12.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem12.TextSize = new Size(0, 0);
			layoutControlItem12.TextVisible = false;
			lciDebateTime.AppearanceItemCaption.Options.UseTextOptions = true;
			lciDebateTime.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciDebateTime.Control = dtDebateTime;
			lciDebateTime.Location = new Point(0, 96);
			lciDebateTime.Name = "lciDebateTime";
			lciDebateTime.Size = new Size(376, 24);
			lciDebateTime.Text = "Thời gian hội chẩn:";
			lciDebateTime.TextAlignMode = TextAlignModeItem.CustomSize;
			lciDebateTime.TextSize = new Size(135, 13);
			lciDebateTime.TextToControlDistance = 5;
			LcDebateTemp.AppearanceItemCaption.Options.UseTextOptions = true;
			LcDebateTemp.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			LcDebateTemp.Control = txtDebateTemp;
			LcDebateTemp.Location = new Point(0, 0);
			LcDebateTemp.Name = "LcDebateTemp";
			LcDebateTemp.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			LcDebateTemp.Size = new Size(222, 24);
			LcDebateTemp.Text = "Mẫu:";
			LcDebateTemp.TextAlignMode = TextAlignModeItem.CustomSize;
			LcDebateTemp.TextSize = new Size(135, 13);
			LcDebateTemp.TextToControlDistance = 5;
			layoutControlItem4.Control = cboDebateTemp;
			layoutControlItem4.Location = new Point(222, 0);
			layoutControlItem4.Name = "layoutControlItem4";
			layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			layoutControlItem4.Size = new Size(245, 24);
			layoutControlItem4.TextSize = new Size(0, 0);
			layoutControlItem4.TextVisible = false;
			lciIcdMain.AppearanceItemCaption.Options.UseTextOptions = true;
			lciIcdMain.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciIcdMain.Control = txtIcdMain;
			lciIcdMain.Location = new Point(0, 24);
			lciIcdMain.Name = "lciIcdMain";
			lciIcdMain.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			lciIcdMain.Size = new Size(222, 24);
			lciIcdMain.Text = "Chẩn đoán chính:";
			lciIcdMain.TextAlignMode = TextAlignModeItem.CustomSize;
			lciIcdMain.TextSize = new Size(135, 20);
			lciIcdMain.TextToControlDistance = 5;
			lciCheckEdit.AppearanceItemCaption.Options.UseTextOptions = true;
			lciCheckEdit.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciCheckEdit.Control = checkEdit;
			lciCheckEdit.Location = new Point(1056, 24);
			lciCheckEdit.Name = "lciCheckEdit";
			lciCheckEdit.Size = new Size(58, 24);
			lciCheckEdit.Text = "Sửa:";
			lciCheckEdit.TextAlignMode = TextAlignModeItem.CustomSize;
			lciCheckEdit.TextSize = new Size(30, 20);
			lciCheckEdit.TextToControlDistance = 5;
			lciIcdSubCode1.AppearanceItemCaption.Options.UseTextOptions = true;
			lciIcdSubCode1.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciIcdSubCode1.Control = txtIcdTextCode;
			lciIcdSubCode1.Location = new Point(0, 48);
			lciIcdSubCode1.Name = "lciIcdSubCode1";
			lciIcdSubCode1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			lciIcdSubCode1.Size = new Size(376, 24);
			lciIcdSubCode1.Text = "Chẩn đoán phụ:";
			lciIcdSubCode1.TextAlignMode = TextAlignModeItem.CustomSize;
			lciIcdSubCode1.TextSize = new Size(135, 20);
			lciIcdSubCode1.TextToControlDistance = 5;
			lciIcdSubCode.AppearanceItemCaption.Options.UseTextOptions = true;
			lciIcdSubCode.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciIcdSubCode.Control = txtIcdTextName;
			lciIcdSubCode.Location = new Point(376, 48);
			lciIcdSubCode.Name = "lciIcdSubCode";
			lciIcdSubCode.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			lciIcdSubCode.Size = new Size(986, 24);
			lciIcdSubCode.Text = "Chẩn đoán phụ:";
			lciIcdSubCode.TextAlignMode = TextAlignModeItem.CustomSize;
			lciIcdSubCode.TextSize = new Size(0, 0);
			lciIcdSubCode.TextToControlDistance = 0;
			lciIcdSubCode.TextVisible = false;
			layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem2.Control = dtInTime;
			layoutControlItem2.Location = new Point(0, 72);
			layoutControlItem2.Name = "layoutControlItem2";
			layoutControlItem2.Size = new Size(376, 24);
			layoutControlItem2.Text = "Thời gian điều trị:  Từ: ";
			layoutControlItem2.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem2.TextSize = new Size(135, 20);
			layoutControlItem2.TextToControlDistance = 5;
			layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem5.Control = dtOutTime;
			layoutControlItem5.Location = new Point(376, 72);
			layoutControlItem5.Name = "layoutControlItem5";
			layoutControlItem5.Size = new Size(348, 24);
			layoutControlItem5.Text = "đến: ";
			layoutControlItem5.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem5.TextSize = new Size(115, 20);
			layoutControlItem5.TextToControlDistance = 5;
			layoutControlItem6.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem6.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem6.Control = cboDepartment;
			layoutControlItem6.Location = new Point(724, 72);
			layoutControlItem6.Name = "layoutControlItem6";
			layoutControlItem6.Size = new Size(638, 24);
			layoutControlItem6.Text = "Khoa điều trị: ";
			layoutControlItem6.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem6.TextSize = new Size(115, 20);
			layoutControlItem6.TextToControlDistance = 5;
			layoutControlItem13.Control = cboRequestLoggin;
			layoutControlItem13.Location = new Point(651, 96);
			layoutControlItem13.Name = "layoutControlItem13";
			layoutControlItem13.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			layoutControlItem13.Size = new Size(711, 24);
			layoutControlItem13.TextSize = new Size(0, 0);
			layoutControlItem13.TextVisible = false;
			icdLocation.AppearanceItemCaption.Options.UseTextOptions = true;
			icdLocation.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			icdLocation.Control = txtLocation;
			icdLocation.Location = new Point(724, 120);
			icdLocation.Name = "icdLocation";
			icdLocation.Size = new Size(638, 24);
			icdLocation.Text = "Địa điểm hội chẩn: ";
			icdLocation.TextAlignMode = TextAlignModeItem.CustomSize;
			icdLocation.TextSize = new Size(115, 20);
			icdLocation.TextToControlDistance = 5;
			layoutControlItem3.Control = panelControl1;
			layoutControlItem3.Location = new Point(222, 24);
			layoutControlItem3.Name = "layoutControlItem3";
			layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			layoutControlItem3.Size = new Size(834, 24);
			layoutControlItem3.TextSize = new Size(0, 0);
			layoutControlItem3.TextVisible = false;
			layoutControlItem15.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem15.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem15.Control = cboDebateType;
			layoutControlItem15.Location = new Point(0, 120);
			layoutControlItem15.Name = "layoutControlItem15";
			layoutControlItem15.Size = new Size(376, 24);
			layoutControlItem15.Text = "Hình thức hội chẩn:";
			layoutControlItem15.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem15.TextSize = new Size(135, 20);
			layoutControlItem15.TextToControlDistance = 5;
			layoutControlItem1.Control = btnSaveTemp;
			layoutControlItem1.Location = new Point(1252, 694);
			layoutControlItem1.MaxSize = new Size(110, 26);
			layoutControlItem1.MinSize = new Size(110, 26);
			layoutControlItem1.Name = "layoutControlItem1";
			layoutControlItem1.Size = new Size(110, 26);
			layoutControlItem1.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem1.TextSize = new Size(0, 0);
			layoutControlItem1.TextVisible = false;
			layoutControlItem9.Control = panelDetail;
			layoutControlItem9.Location = new Point(0, 301);
			layoutControlItem9.Name = "layoutControlItem9";
			layoutControlItem9.Size = new Size(1362, 393);
			layoutControlItem9.TextSize = new Size(0, 0);
			layoutControlItem9.TextVisible = false;
			layoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem10.Control = CheckThuoc;
			layoutControlItem10.Location = new Point(0, 277);
			layoutControlItem10.MaxSize = new Size(350, 24);
			layoutControlItem10.MinSize = new Size(270, 24);
			layoutControlItem10.Name = "layoutControlItem10";
			layoutControlItem10.Size = new Size(270, 24);
			layoutControlItem10.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem10.Text = "Loại hội chẩn:";
			layoutControlItem10.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem10.TextSize = new Size(135, 20);
			layoutControlItem10.TextToControlDistance = 5;
			layoutControlItem14.Control = ChkPttt;
			layoutControlItem14.Location = new Point(270, 277);
			layoutControlItem14.MaxSize = new Size(250, 24);
			layoutControlItem14.MinSize = new Size(180, 24);
			layoutControlItem14.Name = "layoutControlItem14";
			layoutControlItem14.Size = new Size(180, 24);
			layoutControlItem14.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem14.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem14.TextSize = new Size(0, 0);
			layoutControlItem14.TextToControlDistance = 0;
			layoutControlItem14.TextVisible = false;
			layoutControlItem16.Control = ChkOther;
			layoutControlItem16.Location = new Point(450, 277);
			layoutControlItem16.MaxSize = new Size(110, 24);
			layoutControlItem16.MinSize = new Size(110, 24);
			layoutControlItem16.Name = "layoutControlItem16";
			layoutControlItem16.Size = new Size(110, 24);
			layoutControlItem16.SizeConstraintsType = SizeConstraintsType.Custom;
			layoutControlItem16.TextSize = new Size(0, 0);
			layoutControlItem16.TextVisible = false;
			lciAutoCreateEmr.AppearanceItemCaption.Options.UseTextOptions = true;
			lciAutoCreateEmr.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciAutoCreateEmr.Control = chkAutoCreateEmr;
			lciAutoCreateEmr.Location = new Point(250, 694);
			lciAutoCreateEmr.MinSize = new Size(250, 24);
			lciAutoCreateEmr.Name = "lciAutoCreateEmr";
			lciAutoCreateEmr.OptionsToolTip.ToolTip = "Tự động tạo văn bản điện tử sau khi lưu thành công";
			lciAutoCreateEmr.Size = new Size(250, 26);
			lciAutoCreateEmr.SizeConstraintsType = SizeConstraintsType.Custom;
			lciAutoCreateEmr.Text = "Tự động tạo văn bản điện tử:";
			lciAutoCreateEmr.TextAlignMode = TextAlignModeItem.CustomSize;
			lciAutoCreateEmr.TextSize = new Size(220, 20);
			lciAutoCreateEmr.TextToControlDistance = 5;
			layoutControlItem8.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem8.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem8.AppearanceItemCaption.TextOptions.VAlignment = VertAlignment.Top;
			layoutControlItem8.Control = gridControl;
			layoutControlItem8.Location = new Point(0, 144);
			layoutControlItem8.Name = "layoutControlItem8";
			layoutControlItem8.Size = new Size(695, 133);
			layoutControlItem8.Text = "Thành phần tham gia:";
			layoutControlItem8.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem8.TextSize = new Size(135, 20);
			layoutControlItem8.TextToControlDistance = 5;
			emptySpaceItem3.AllowHotTrack = false;
			emptySpaceItem3.Location = new Point(560, 277);
			emptySpaceItem3.Name = "emptySpaceItem3";
			emptySpaceItem3.Size = new Size(802, 24);
			emptySpaceItem3.TextSize = new Size(0, 0);
			layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem7.Control = cboPhieuDieuTri;
			layoutControlItem7.Location = new Point(555, 0);
			layoutControlItem7.Name = "layoutControlItem7";
			layoutControlItem7.Size = new Size(231, 24);
			layoutControlItem7.Text = "Tờ điều trị:";
			layoutControlItem7.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem7.TextSize = new Size(90, 20);
			layoutControlItem7.TextToControlDistance = 5;
			layoutControlItem17.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem17.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem17.Control = cboBienBanHoiChanCu;
			layoutControlItem17.Location = new Point(786, 0);
			layoutControlItem17.Name = "layoutControlItem17";
			layoutControlItem17.Size = new Size(576, 24);
			layoutControlItem17.Text = "Biên bản hội chẩn cũ:";
			layoutControlItem17.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem17.TextSize = new Size(130, 20);
			layoutControlItem17.TextToControlDistance = 5;
			emptySpaceItem2.AllowHotTrack = false;
			emptySpaceItem2.Location = new Point(0, 694);
			emptySpaceItem2.Name = "emptySpaceItem2";
			emptySpaceItem2.Size = new Size(112, 26);
			emptySpaceItem2.TextSize = new Size(0, 0);
			lciAutoSign.AppearanceItemCaption.Options.UseTextOptions = true;
			lciAutoSign.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			lciAutoSign.Control = chkAutoSign;
			lciAutoSign.Location = new Point(500, 694);
			lciAutoSign.MinSize = new Size(300, 24);
			lciAutoSign.Name = "lciAutoSign";
			lciAutoSign.OptionsToolTip.ToolTip = "Tự động thiết lập ký theo thành phần tham gia";
			lciAutoSign.Size = new Size(300, 26);
			lciAutoSign.SizeConstraintsType = SizeConstraintsType.Custom;
			lciAutoSign.Text = "Tự động thiết lập ký theo thành phần tham gia:";
			lciAutoSign.TextAlignMode = TextAlignModeItem.CustomSize;
			lciAutoSign.TextSize = new Size(270, 20);
			lciAutoSign.TextToControlDistance = 5;
			layoutControlItem18.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem18.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem18.AppearanceItemCaption.TextOptions.VAlignment = VertAlignment.Top;
			layoutControlItem18.Control = (Control)(object)gridControl1;
			layoutControlItem18.Location = new Point(695, 144);
			layoutControlItem18.Name = "layoutControlItem18";
			layoutControlItem18.OptionsToolTip.ToolTip = resources.GetString("resource.ToolTip");
			layoutControlItem18.Size = new Size(667, 133);
			layoutControlItem18.Text = "Mời tham gia:";
			layoutControlItem18.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem18.TextSize = new Size(90, 20);
			layoutControlItem18.TextToControlDistance = 5;
			layoutControlItem19.Control = btnTreatmentHistory;
			layoutControlItem19.Location = new Point(910, 694);
			layoutControlItem19.Name = "layoutControlItem19";
			layoutControlItem19.Size = new Size(122, 26);
			layoutControlItem19.TextSize = new Size(0, 0);
			layoutControlItem19.TextVisible = false;
			layoutControlItem23.Control = btnSendTMP;
			layoutControlItem23.Location = new Point(800, 694);
			layoutControlItem23.Name = "layoutControlItem23";
			layoutControlItem23.Size = new Size(110, 26);
			layoutControlItem23.TextSize = new Size(0, 0);
			layoutControlItem23.TextVisible = false;
			layoutControlItem24.AppearanceItemCaption.Options.UseTextOptions = true;
			layoutControlItem24.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
			layoutControlItem24.Control = cboDebateReason;
			layoutControlItem24.Location = new Point(376, 120);
			layoutControlItem24.Name = "layoutControlItem24";
			layoutControlItem24.Size = new Size(348, 24);
			layoutControlItem24.Text = "Lý do hội chẩn:";
			layoutControlItem24.TextAlignMode = TextAlignModeItem.CustomSize;
			layoutControlItem24.TextSize = new Size(115, 20);
			layoutControlItem24.TextToControlDistance = 5;
			layoutControlItem25.Control = chkLockInfor;
			layoutControlItem25.Location = new Point(467, 0);
			layoutControlItem25.Name = "layoutControlItem25";
			layoutControlItem25.Size = new Size(88, 24);
			layoutControlItem25.TextSize = new Size(0, 0);
			layoutControlItem25.TextVisible = false;
			layoutControlItem26.Control = chkAutoCreateTracking;
			layoutControlItem26.Location = new Point(112, 694);
			layoutControlItem26.Name = "layoutControlItem26";
			layoutControlItem26.Size = new Size(138, 26);
			layoutControlItem26.Text = "Tự động tạo tờ điều trị:";
			layoutControlItem26.TextSize = new Size(112, 13);
			emptySpaceItem4.AllowHotTrack = false;
			emptySpaceItem4.Location = new Point(1114, 24);
			emptySpaceItem4.Name = "emptySpaceItem4";
			emptySpaceItem4.Size = new Size(248, 24);
			emptySpaceItem4.TextSize = new Size(0, 0);
			dxValidationProvider1.ValidationFailed += dxValidationProvider1_ValidationFailed;
			this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoSize = true;
			((Form)this).ClientSize = new Size(1362, 749);
			((Control)this).Controls.Add(layoutControl1);
			((Control)this).Controls.Add(barDockControlLeft);
			((Control)this).Controls.Add(barDockControlRight);
			((Control)this).Controls.Add(barDockControlBottom);
			((Control)this).Controls.Add(barDockControlTop);
			((Control)this).Name = "FormDebateDiagnostic";
			((Form)this).StartPosition = FormStartPosition.CenterScreen;
			((Control)(object)this).Text = "Biên bản hội chẩn";
			((Form)this).WindowState = FormWindowState.Maximized;
			((Form)this).Load += FormDebateDiagnostic_Load;
			((Control)this).Controls.SetChildIndex(barDockControlTop, 0);
			((Control)this).Controls.SetChildIndex(barDockControlBottom, 0);
			((Control)this).Controls.SetChildIndex(barDockControlRight, 0);
			((Control)this).Controls.SetChildIndex(barDockControlLeft, 0);
			((Control)this).Controls.SetChildIndex(layoutControl1, 0);
			((ISupportInitialize)layoutControl1).EndInit();
			layoutControl1.ResumeLayout(false);
			((ISupportInitialize)chkAutoCreateTracking.Properties).EndInit();
			((ISupportInitialize)barManager1).EndInit();
			((ISupportInitialize)chkLockInfor.Properties).EndInit();
			((ISupportInitialize)popupControlContainer1).EndInit();
			popupControlContainer1.ResumeLayout(false);
			((ISupportInitialize)layoutControl2).EndInit();
			layoutControl2.ResumeLayout(false);
			((ISupportInitialize)memContent.Properties).EndInit();
			((ISupportInitialize)layoutControlGroup2).EndInit();
			((ISupportInitialize)layoutControlItem20).EndInit();
			((ISupportInitialize)layoutControlItem21).EndInit();
			((ISupportInitialize)layoutControlItem22).EndInit();
			((ISupportInitialize)emptySpaceItem1).EndInit();
			((ISupportInitialize)gridControl1).EndInit();
			((ISupportInitialize)gridView5).EndInit();
			((ISupportInitialize)rebtnFeedBackEnable).EndInit();
			((ISupportInitialize)regluThamGia).EndInit();
			((ISupportInitialize)customGridView1).EndInit();
			((ISupportInitialize)LookUpEditUserNameInvate).EndInit();
			((ISupportInitialize)customGridView2).EndInit();
			((ISupportInitialize)rechkThuKy).EndInit();
			((ISupportInitialize)rechkChuToa).EndInit();
			((ISupportInitialize)recboExecuteRole).EndInit();
			((ISupportInitialize)customGridView3).EndInit();
			((ISupportInitialize)reTxtComment).EndInit();
			((ISupportInitialize)rebtnAddInvateUser).EndInit();
			((ISupportInitialize)rebtnFeedBackDisable).EndInit();
			((ISupportInitialize)rebtnMinusInvateUser).EndInit();
			((ISupportInitialize)recboThamGia).EndInit();
			((ISupportInitialize)reTxtDisable).EndInit();
			((ISupportInitialize)rechkThuKyDisable).EndInit();
			((ISupportInitialize)rechkChuToaDisable).EndInit();
			((ISupportInitialize)cboBienBanHoiChanCu.Properties).EndInit();
			((ISupportInitialize)gridView4).EndInit();
			((ISupportInitialize)cboPhieuDieuTri.Properties).EndInit();
			((ISupportInitialize)gridView3).EndInit();
			((ISupportInitialize)gridControl).EndInit();
			((ISupportInitialize)gridView).EndInit();
			((ISupportInitialize)TextEditLoginName).EndInit();
			((ISupportInitialize)CheckEditChuToa).EndInit();
			((ISupportInitialize)CheckEditThuKy).EndInit();
			((ISupportInitialize)CheckEditBsNgoaiVien).EndInit();
			((ISupportInitialize)repositoryItemCboExecuteRole).EndInit();
			((ISupportInitialize)repositoryItemCustomGridLookUpEdit1View).EndInit();
			((ISupportInitialize)ButtonAdd).EndInit();
			((ISupportInitialize)CheckEditChuToaDisable).EndInit();
			((ISupportInitialize)CheckEditThuKyDisable).EndInit();
			((ISupportInitialize)ButtonDelete).EndInit();
			((ISupportInitialize)LookUpEditUserName).EndInit();
			((ISupportInitialize)TextEditUserName).EndInit();
			((ISupportInitialize)TextEditLoginNameDis).EndInit();
			((ISupportInitialize)ChkOther.Properties).EndInit();
			((ISupportInitialize)ChkPttt.Properties).EndInit();
			((ISupportInitialize)CheckThuoc.Properties).EndInit();
			((ISupportInitialize)panelDetail).EndInit();
			((ISupportInitialize)chkAutoCreateEmr.Properties).EndInit();
			((ISupportInitialize)chkAutoSign.Properties).EndInit();
			((ISupportInitialize)cboDebateType.Properties).EndInit();
			((ISupportInitialize)gridLookUpEdit1View).EndInit();
			((ISupportInitialize)panelControl1).EndInit();
			panelControl1.ResumeLayout(false);
			((ISupportInitialize)cboIcdMain.Properties).EndInit();
			((ISupportInitialize)gridLookUpEdit3View).EndInit();
			((ISupportInitialize)icdMainText.Properties).EndInit();
			((ISupportInitialize)txtLocation.Properties).EndInit();
			((ISupportInitialize)cboRequestLoggin.Properties).EndInit();
			((ISupportInitialize)gridLookUpEdit2View).EndInit();
			((ISupportInitialize)cboDepartment.Properties).EndInit();
			((ISupportInitialize)gridView2).EndInit();
			((ISupportInitialize)dtOutTime.Properties.CalendarTimeProperties).EndInit();
			((ISupportInitialize)dtOutTime.Properties).EndInit();
			((ISupportInitialize)dtInTime.Properties.CalendarTimeProperties).EndInit();
			((ISupportInitialize)dtInTime.Properties).EndInit();
			((ISupportInitialize)txtDebateTemp.Properties).EndInit();
			((ISupportInitialize)txtIcdTextName.Properties).EndInit();
			((ISupportInitialize)dtDebateTime.Properties.CalendarTimeProperties).EndInit();
			((ISupportInitialize)dtDebateTime.Properties).EndInit();
			((ISupportInitialize)txtIcdTextCode.Properties).EndInit();
			((ISupportInitialize)checkEdit.Properties).EndInit();
			((ISupportInitialize)txtIcdMain.Properties).EndInit();
			((ISupportInitialize)txtRequestLoggin.Properties).EndInit();
			((ISupportInitialize)cboDebateTemp.Properties).EndInit();
			((ISupportInitialize)gridView1).EndInit();
			((ISupportInitialize)cboDebateReason.Properties).EndInit();
			((ISupportInitialize)gridView6).EndInit();
			((ISupportInitialize)layoutControlGroup1).EndInit();
			((ISupportInitialize)lciRequestContent).EndInit();
			((ISupportInitialize)layoutControlItem11).EndInit();
			((ISupportInitialize)layoutControlItem12).EndInit();
			((ISupportInitialize)lciDebateTime).EndInit();
			((ISupportInitialize)LcDebateTemp).EndInit();
			((ISupportInitialize)layoutControlItem4).EndInit();
			((ISupportInitialize)lciIcdMain).EndInit();
			((ISupportInitialize)lciCheckEdit).EndInit();
			((ISupportInitialize)lciIcdSubCode1).EndInit();
			((ISupportInitialize)lciIcdSubCode).EndInit();
			((ISupportInitialize)layoutControlItem2).EndInit();
			((ISupportInitialize)layoutControlItem5).EndInit();
			((ISupportInitialize)layoutControlItem6).EndInit();
			((ISupportInitialize)layoutControlItem13).EndInit();
			((ISupportInitialize)icdLocation).EndInit();
			((ISupportInitialize)layoutControlItem3).EndInit();
			((ISupportInitialize)layoutControlItem15).EndInit();
			((ISupportInitialize)layoutControlItem1).EndInit();
			((ISupportInitialize)layoutControlItem9).EndInit();
			((ISupportInitialize)layoutControlItem10).EndInit();
			((ISupportInitialize)layoutControlItem14).EndInit();
			((ISupportInitialize)layoutControlItem16).EndInit();
			((ISupportInitialize)lciAutoCreateEmr).EndInit();
			((ISupportInitialize)layoutControlItem8).EndInit();
			((ISupportInitialize)emptySpaceItem3).EndInit();
			((ISupportInitialize)layoutControlItem7).EndInit();
			((ISupportInitialize)layoutControlItem17).EndInit();
			((ISupportInitialize)emptySpaceItem2).EndInit();
			((ISupportInitialize)lciAutoSign).EndInit();
			((ISupportInitialize)layoutControlItem18).EndInit();
			((ISupportInitialize)layoutControlItem19).EndInit();
			((ISupportInitialize)layoutControlItem23).EndInit();
			((ISupportInitialize)layoutControlItem24).EndInit();
			((ISupportInitialize)layoutControlItem25).EndInit();
			((ISupportInitialize)layoutControlItem26).EndInit();
			((ISupportInitialize)emptySpaceItem4).EndInit();
			((ISupportInitialize)dxValidationProvider1).EndInit();
			((Control)this).ResumeLayout(false);
			((Control)this).PerformLayout();
		}

		private void ButtonAdd_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				HisDebateUserADO hisDebateUserADO = new HisDebateUserADO();
				lstParticipantDebate = gridControl.DataSource as List<HisDebateUserADO>;
				if (lstParticipantDebate != null && lstParticipantDebate.Count > 0)
				{
					HisDebateUserADO item = new HisDebateUserADO();
					lstParticipantDebate.Add(item);
					lstParticipantDebate.ForEach(delegate(HisDebateUserADO o)
					{
						o.Action = 2;
					});
					lstParticipantDebate.LastOrDefault().Action = 1;
					gridControl.DataSource = null;
					gridControl.DataSource = lstParticipantDebate;
				}
				else
				{
					HisDebateUserADO item2 = new HisDebateUserADO();
					lstParticipantDebate.Add(item2);
					lstParticipantDebate.LastOrDefault().Action = 1;
					gridControl.DataSource = null;
					gridControl.DataSource = lstParticipantDebate;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void TextEditLoginName_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				BaseEdit edit = sender as BaseEdit;
				if (edit == null)
				{
					return;
				}
				List<ACS_USER> list = GlobalStore.HisAcsUser.Where((ACS_USER o) => o.LOGINNAME.ToLower().Contains(edit.EditValue.ToString().ToLower())).ToList();
				if (list != null && list.Count > 0)
				{
					if (list.Count == 1)
					{
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_LoginName, list[0].LOGINNAME);
						gridView.SetRowCellValue(gridView.FocusedRowHandle, gridColumnParticipants_Id, list[0].ID);
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_UserName, list[0].ID);
					}
					else
					{
						gridView.FocusedColumn = Gc_UserName;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LookUpEditUserName_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
                HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit edit = (HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit)((sender is HIS.Desktop.Utilities.Extensions.CustomGridLookUpEdit) ? sender : null);
				if (edit != null && ((BaseEdit)(object)edit).EditValue != null && (((BaseEdit)(object)edit).EditValue ?? ((object)0)).ToString() != (((BaseEdit)(object)edit).OldEditValue ?? ((object)0)).ToString())
				{
					ACS_USER val = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((BaseEdit)(object)edit).EditValue.ToString());
					if (val != null)
					{
						gridView.SetRowCellValue(gridView.FocusedRowHandle, Gc_LoginName, val.LOGINNAME);
						gridView.SetRowCellValue(gridView.FocusedRowHandle, gridColumnParticipants_Id, val.ID);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ButtonDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				CommonParam val = new CommonParam();
				HisDebateUserADO hisDebateUserADO = (HisDebateUserADO)gridView.GetFocusedRow();
				if (hisDebateUserADO != null)
				{
					lstParticipantDebate.Remove(hisDebateUserADO);
					lstParticipantDebate.ForEach(delegate(HisDebateUserADO o)
					{
						o.Action = 2;
					});
					lstParticipantDebate.LastOrDefault().Action = 1;
					gridControl.DataSource = null;
					gridControl.DataSource = lstParticipantDebate;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			try
			{
				if (treatment_id == 0)
				{
					return;
				}
				positionHandleControl = -1;
				if (!dxValidationProvider1.Validate() || (detailProcessor != null && detailProcessor.ValidateControl(GetTypeDetail())))
				{
					return;
				}
				WaitingManager.Show();
				HIS_DEBATE val = new HIS_DEBATE();
				if (hisService != null)
				{
					val.SERVICE_ID = hisService.ID;
				}
				if (action == 1)
				{
					ProcessHisDebate(val);
					val.TREATMENT_ID = treatment_id;
					if (!string.IsNullOrEmpty(cboRequestLoggin.EditValue.ToString()))
					{
						val.REQUEST_LOGINNAME = acsUsers.FirstOrDefault((ACS_USER o) => o.LOGINNAME == cboRequestLoggin.EditValue.ToString()).LOGINNAME;
						val.REQUEST_USERNAME = acsUsers.FirstOrDefault((ACS_USER o) => o.LOGINNAME == cboRequestLoggin.EditValue.ToString()).USERNAME;
					}
					if (cboDepartment.EditValue != null)
					{
						val.DEPARTMENT_ID = System.Convert.ToInt64(cboDepartment.EditValue);
					}
				}
				else if (action == 2 && currentHisDebate != null)
				{
					ProcessHisDebate(currentHisDebate);
					val = currentHisDebate;
				}
				if (cboPhieuDieuTri.EditValue != null)
				{
					val.TRACKING_ID = (long)cboPhieuDieuTri.EditValue;
				}
				else
				{
					val.TRACKING_ID = null;
				}
				ProcessHisDebateUser(val);
				ProcessHisDebateInvateUser(val);
				if (lciAutoCreateEmr.Visibility == LayoutVisibility.Always && chkAutoCreateEmr.Checked && chkAutoSign.Checked && val.HIS_DEBATE_USER != null && val.HIS_DEBATE_USER.Count > 0)
				{
					List<EMR_SIGNER> emrSigner = BackendDataWorker.Get<EMR_SIGNER>();
					List<HIS_DEBATE_USER> list = val.HIS_DEBATE_USER.Where((HIS_DEBATE_USER o) => emrSigner == null || !emrSigner.Select((EMR_SIGNER p) => p.LOGINNAME).Contains(o.LOGINNAME)).ToList();
					if (list != null && list.Count > 0)
					{
						string arg = string.Join(",", list.Select((HIS_DEBATE_USER o) => o.LOGINNAME));
						MessageBox.Show(string.Format("Tài khoản {0} chưa được tạo thông tin người ký", arg), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				if (val != null)
				{
					if (val.HIS_DEBATE_USER != null && val.HIS_DEBATE_USER.Count > 0)
					{
						List<string> list2 = new List<string>();
						foreach (HIS_DEBATE_USER item in val.HIS_DEBATE_USER)
						{
							int num = val.HIS_DEBATE_USER.Where((HIS_DEBATE_USER o) => o.LOGINNAME == item.LOGINNAME).Count();
							if (num >= 2)
							{
								list2.Add(item.LOGINNAME);
							}
						}
						if (list2 != null && list2.Count > 0)
						{
							string arg2 = string.Join(",", list2.Distinct().ToList());
							WaitingManager.Hide();
							MessageBox.Show(string.Format(ResourceMessage.TaiKhoanBilap, arg2), ResourceMessage.ThongBao, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						if (chkAutoSign.Checked && (!val.HIS_DEBATE_USER.ToList().Exists((HIS_DEBATE_USER o) => o.IS_PRESIDENT == 1) || !val.HIS_DEBATE_USER.ToList().Exists((HIS_DEBATE_USER o) => o.IS_SECRETARY == 1)))
						{
							WaitingManager.Hide();
							MessageBox.Show("Bắt buộc có thành phần tham gia là Chủ tọa/Thư ký", ResourceMessage.ThongBao, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
					}
					else if (chkAutoSign.Checked)
					{
						WaitingManager.Hide();
						MessageBox.Show("Bắt buộc có thành phần tham gia là Chủ tọa/Thư ký", ResourceMessage.ThongBao, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				if (val != null && val.HIS_DEBATE_INVITE_USER != null && val.HIS_DEBATE_INVITE_USER.Count > 0)
				{
					List<string> list3 = new List<string>();
					foreach (HIS_DEBATE_INVITE_USER item2 in val.HIS_DEBATE_INVITE_USER)
					{
						int num2 = val.HIS_DEBATE_INVITE_USER.Where((HIS_DEBATE_INVITE_USER o) => o.LOGINNAME == item2.LOGINNAME).Count();
						if (num2 >= 2)
						{
							list3.Add(item2.LOGINNAME);
						}
					}
					if (list3 != null && list3.Count > 0)
					{
						string arg3 = string.Join(",", list3.Distinct().ToList());
						WaitingManager.Hide();
						MessageBox.Show(string.Format(ResourceMessage.TaiKhoanBilap, arg3), ResourceMessage.ThongBao, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				if (CheckValidation(val))
				{
					SaveHisDebate(val);
					WaitingManager.Hide();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				WaitingManager.Hide();
			}
		}

		private bool CheckValidation(HIS_DEBATE hisDebate)
		{
			try
			{
				if (hisDebate == null)
				{
					return false;
				}
				if (hisDebate.DEBATE_TIME.HasValue && hisDebate.DEBATE_TIME < vHisTreatment.IN_TIME)
				{
					MessageBox.Show(string.Format(ResourceMessage.ThoiGianHoiChanKhongDuocNhoHonThoiGianVao, Inventec.Common.DateTime.Convert.TimeNumberToTimeString(vHisTreatment.IN_TIME)));
					return false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return false;
			}
			return true;
		}

		private void cboDebateTemp_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					txtDebateTemp.Text = "";
					cboDebateTemp.EditValue = null;
					FillDatatoControlByHisDebateTemp(null);
					txtDebateTemp.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDebateTemp_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboDebateTemp.EditValue != null && cboDebateTemp.EditValue != cboDebateTemp.OldEditValue)
				{
					HIS_DEBATE_TEMP val = BackendDataWorker.Get<HIS_DEBATE_TEMP>().FirstOrDefault((HIS_DEBATE_TEMP o) => o.ID == Parse.ToInt64((cboDebateTemp.EditValue ?? "").ToString()));
					if (val != null)
					{
						FillDatatoControlByHisDebateTemp(val);
						txtDebateTemp.Text = val.DEBATE_TEMP_CODE;
						txtIcdMain.Focus();
					}
				}
				else
				{
					txtIcdMain.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboRequestLoggin_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && cboRequestLoggin.EditValue != null)
				{
					ACS_USER val = acsUsers.FirstOrDefault((ACS_USER o) => o.LOGINNAME == (cboRequestLoggin.EditValue ?? "").ToString());
					if (val != null)
					{
						txtRequestLoggin.Text = val.LOGINNAME;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtDebateTemp_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string text = (sender as TextEdit).Text;
					LoadDebateTempCombo(text);
					if (cboDebateTemp.EditValue != null)
					{
						HIS_DEBATE_TEMP data = BackendDataWorker.Get<HIS_DEBATE_TEMP>().FirstOrDefault((HIS_DEBATE_TEMP p) => p.ID == long.Parse(cboDebateTemp.EditValue.ToString()));
						FillDatatoControlByHisDebateTemp(data);
					}
				}
				e.Handled = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSaveTemp_Click(object sender, EventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			try
			{
				HIS_DEBATE_TEMP val = new HIS_DEBATE_TEMP();
				ProcessHisDebateTemp(val);
				List<object> list = new List<object>();
				list.Add(val);
				if (moduleData != null)
				{
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.HisDebateTemp", moduleData.RoomId, moduleData.RoomTypeId, list);
				}
				else
				{
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.HisDebateTemp", 0L, 0L, list);
				}
				InitComboDebateTemp();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (btnSaveTemp.Enabled)
				{
					btnSaveTemp_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtDebateTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (txtRequestLoggin.Enabled)
				{
					txtRequestLoggin.Focus();
					txtRequestLoggin.SelectAll();
					return;
				}
				cboDebateType.Focus();
				if (cboDebateType.EditValue == null)
				{
					cboDebateType.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtRequestContent_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				string searchCode = (sender as TextEdit).Text.ToUpper();
				if (string.IsNullOrEmpty(searchCode))
				{
					cboRequestLoggin.EditValue = acsUsers[0].LOGINNAME;
					return;
				}
				List<ACS_USER> list = (from o in BackendDataWorker.Get<ACS_USER>()
					where o.LOGINNAME.ToUpper().Contains(searchCode.ToUpper())
					select o).ToList();
				List<ACS_USER> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((ACS_USER o) => o.LOGINNAME.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					cboRequestLoggin.EditValue = list2[0].LOGINNAME;
					txtRequestLoggin.Text = list2[0].LOGINNAME;
				}
				else
				{
					cboRequestLoggin.EditValue = acsUsers[0].LOGINNAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void icdMainText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Tab)
				{
					checkEdit.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtIcdMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				bool flag = true;
				if (!string.IsNullOrEmpty(txtIcdMain.Text.Trim()))
				{
					string code = txtIcdMain.Text.Trim().ToUpper();
					List<HIS_ICD> list = GlobalStore.HisIcds.Where((HIS_ICD o) => o.ICD_CODE.ToUpper().Equals(code)).ToList();
					if (list != null && list.Count == 1)
					{
						flag = false;
						txtIcdMain.Text = list.First().ICD_CODE;
						cboIcdMain.EditValue = list.First().ICD_CODE;
						icdMainText.Text = list.First().ICD_NAME;
						checkEdit.Focus();
						if (checkEdit.Checked)
						{
							icdMainText.Focus();
							icdMainText.SelectAll();
						}
						else
						{
							cboIcdMain.Focus();
							cboIcdMain.SelectAll();
						}
					}
				}
				if (flag)
				{
					cboIcdMain.Focus();
					cboIcdMain.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboIcdMain_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && cboIcdMain.Text != null)
				{
					HIS_ICD val = GlobalStore.HisIcds.FirstOrDefault((HIS_ICD o) => o.ICD_CODE == (cboIcdMain.EditValue ?? "").ToString());
					if (val != null)
					{
						txtIcdMain.Text = val.ICD_CODE;
						checkEdit.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboIcdMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboIcdMain.Text != null)
				{
					HIS_ICD val = GlobalStore.HisIcds.FirstOrDefault(delegate(HIS_ICD o)
					{
						string iCD_CODE = o.ICD_CODE;
						object editValue = cboIcdMain.EditValue;
						return iCD_CODE == ((editValue != null) ? editValue.ToString() : null);
					});
					if (val != null)
					{
						txtIcdMain.Text = val.ICD_CODE;
						checkEdit.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void checkEdit_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (checkEdit.Checked)
				{
					cboIcdMain.Visible = false;
					icdMainText.Visible = true;
					icdMainText.Text = cboIcdMain.Text;
					icdMainText.Focus();
					icdMainText.SelectAll();
				}
				else if (!checkEdit.Checked)
				{
					icdMainText.Visible = false;
					cboIcdMain.Visible = true;
					icdMainText.Text = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void checkEdit_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtIcdTextCode.Focus();
					txtIcdTextCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtIcdText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtIcdTextName.Focus();
					txtIcdTextName.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtIcdTextCode_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				string text = (sender as TextEdit).Text;
				text = text.Trim();
				string text2 = "";
				if (!string.IsNullOrEmpty(text))
				{
					string text3 = ";";
					string text4 = "";
					string[] separator = new string[1] { text3 };
					List<string> list = new List<string>();
					string[] array = txtIcdTextCode.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);
					if (array != null && array.Count() > 0)
					{
						string[] array2 = array;
						foreach (string itemCode in array2)
						{
							HIS_ICD val = GlobalStore.HisIcds.FirstOrDefault((HIS_ICD o) => o.ICD_CODE.ToLower() == itemCode.ToLower());
							if (val != null && val.ID > 0)
							{
								text2 = text2 + text3 + val.ICD_NAME;
								continue;
							}
							list.Add(itemCode);
							text4 = text4 + text3 + itemCode;
						}
						text2 += text3;
						if (!string.IsNullOrEmpty(text4))
						{
							MessageManager.Show(string.Format(ResourceMessage.KhongTimThayIcdTuongUngVoiCacMaSau, text4));
							int start = 0;
							int length = txtIcdTextCode.Text.Length - 1;
							if (list != null && list.Count > 0)
							{
								start = txtIcdTextCode.Text.IndexOf(list[0]);
								length = list[0].Length;
							}
							txtIcdTextCode.Focus();
							txtIcdTextCode.Select(start, length);
						}
					}
				}
				SetCheckedIcdsToControl(txtIcdTextCode.Text, text2);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetCheckedIcdsToControl(string icdCodes, string icdNames)
		{
			try
			{
				string oldIcdNames = ((txtIcdTextName.Text == txtIcdTextName.Properties.NullValuePrompt) ? "" : txtIcdTextName.Text);
				txtIcdTextName.Text = processIcdNameChanged(oldIcdNames, icdNames);
				if (icdNames.Equals(";"))
				{
					txtIcdTextName.Text = "";
				}
				if (icdCodes.Equals(";"))
				{
					txtIcdTextCode.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private string processIcdNameChanged(string oldIcdNames, string newIcdNames)
		{
			string text = "";
			try
			{
				text = newIcdNames;
				if (!string.IsNullOrEmpty(oldIcdNames))
				{
					string[] array = oldIcdNames.Split(new string[1] { ";" }, StringSplitOptions.RemoveEmptyEntries);
					if (array != null && array.Length != 0)
					{
						string[] array2 = array;
						foreach (string item in array2)
						{
							if (!string.IsNullOrEmpty(item) && !newIcdNames.Contains(IcdUtil.AddSeperateToKey(item)))
							{
								HIS_ICD val = GlobalStore.HisIcds.Where((HIS_ICD o) => IcdUtil.AddSeperateToKey(item).Equals(IcdUtil.AddSeperateToKey(o.ICD_NAME))).FirstOrDefault();
								if (val == null || val.ID == 0)
								{
									text = text + item + ";";
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return text;
		}

		private void lciIcdTextName_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Expected O, but got Unknown
			try
			{
				if (e.KeyCode == Keys.F1)
				{
					Module val = GlobalVariables.currentModuleRaws.Where((Module o) => o.ModuleLink == "HIS.Desktop.Plugins.SecondaryIcd").FirstOrDefault();
					if (val == null)
					{
						LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.SecondaryIcd");
						MessageManager.Show(ResourceMessage.TaiKhoanKhongCoQuyenThucHienChucNang);
					}
					if (val.IsPlugin && val.ExtensionInfo != null)
					{
						SecondaryIcdADO item = new SecondaryIcdADO(new DelegateRefeshIcdChandoanphu(stringIcds), txtIcdTextCode.Text, txtIcdTextName.Text);
						List<object> list = new List<object>();
						list.Add(item);
						object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(val, moduleData.RoomId, moduleData.RoomTypeId), list);
						if (pluginInstance == null)
						{
							throw new ArgumentNullException("moduleData is null");
						}
						((Form)pluginInstance).ShowDialog();
					}
					else
					{
						MessageManager.Show(ResourceMessage.TaiKhoanKhongCoQuyenThucHienChucNang);
					}
				}
				if (e.KeyCode == Keys.Return)
				{
					dtInTime.Focus();
					dtInTime.SelectAll();
					dtInTime.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void stringIcds(string _icdCode, string _icdName)
		{
			try
			{
				if (!string.IsNullOrEmpty(_icdCode))
				{
					txtIcdTextCode.Text = _icdCode;
				}
				if (!string.IsNullOrEmpty(_icdName))
				{
					txtIcdTextName.Text = _icdName;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtInTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtOutTime.Focus();
					dtOutTime.SelectAll();
					dtOutTime.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtOutTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (cboDepartment.Enabled)
					{
						cboDepartment.SelectAll();
						cboDepartment.ShowPopup();
					}
					else
					{
						dtDebateTime.SelectAll();
						dtDebateTime.ShowPopup();
						dtDebateTime.Focus();
					}
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
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtDebateTime.SelectAll();
					dtDebateTime.ShowPopup();
					dtDebateTime.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtLocation_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetDataHisDebate(HIS_DEBATE hisDebate)
		{
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Expected O, but got Unknown
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				txtIcdTextName.Text = hisDebate.ICD_TEXT;
				txtIcdTextCode.Text = hisDebate.ICD_SUB_CODE;
				if (!string.IsNullOrEmpty(hisDebate.ICD_CODE))
				{
					HIS_ICD val = GlobalStore.HisIcds.Where((HIS_ICD o) => o.ICD_CODE == hisDebate.ICD_CODE).FirstOrDefault();
					cboIcdMain.EditValue = val.ICD_CODE;
					txtIcdMain.Text = val.ICD_CODE;
				}
				if (!string.IsNullOrEmpty(hisDebate.ICD_NAME))
				{
					checkEdit.Checked = true;
					icdMainText.Text = hisDebate.ICD_NAME;
				}
				if (hisDebate.DEBATE_TYPE_ID.HasValue)
				{
					cboDebateType.EditValue = hisDebate.DEBATE_TYPE_ID;
				}
				else
				{
					cboDebateType.EditValue = null;
				}
				cboDebateReason.EditValue = hisDebate.DEBATE_REASON_ID;
				txtLocation.Text = hisDebate.LOCATION;
				List<HisDebateUserADO> list = new List<HisDebateUserADO>();
				if (hisDebate.ID > 0)
				{
					HisDebateUserFilter val2 = new HisDebateUserFilter();
					val2.DEBATE_ID = hisDebate.ID;
					List<HIS_DEBATE_USER> list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
					if (list2 != null && list2.Count > 0)
					{
						foreach (HIS_DEBATE_USER item in list2)
						{
							HisDebateUserADO hisDebateUserADO = new HisDebateUserADO();
							DataObjectMapper.Map<HisDebateUserADO>((object)hisDebateUserADO, (object)item);
							if (item.IS_PRESIDENT == 1)
							{
								hisDebateUserADO.PRESIDENT = true;
							}
							if (item.IS_SECRETARY == 1)
							{
								hisDebateUserADO.SECRETARY = true;
							}
							if (item.IS_OUT_OF_HOSPITAL == 1)
							{
								hisDebateUserADO.OUT_OF_HOSPITAL = true;
							}
							hisDebateUserADO.Action = 2;
							list.Add(hisDebateUserADO);
						}
					}
				}
				if (list != null && list.Count > 0)
				{
					list[0].Action = 1;
					gridControl.DataSource = list;
					lstParticipantDebate = list;
				}
				else
				{
					FillDataToParticipants();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetDataDebateInvateUser(HIS_DEBATE hisDebate)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (hisDebate.ID > 0)
				{
					HisDebateInviteUserFilter val = new HisDebateInviteUserFilter();
					val.DEBATE_ID = hisDebate.ID;
					List<HIS_DEBATE_INVITE_USER> list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_INVITE_USER>>("apiHisDebateInvateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
					if (list != null && list.Count > 0)
					{
						foreach (HIS_DEBATE_INVITE_USER item in list)
						{
							HisDebateInvateUserADO hisDebateInvateUserADO = new HisDebateInvateUserADO();
							DataObjectMapper.Map<HisDebateInvateUserADO>((object)hisDebateInvateUserADO, (object)item);
							if (item.IS_PRESIDENT == 1)
							{
								hisDebateInvateUserADO.PRESIDENT = true;
							}
							if (item.IS_SECRETARY == 1)
							{
								hisDebateInvateUserADO.SECRETARY = true;
							}
							hisDebateInvateUserADO.Action = 2;
							lstDebateInvateUserADO.Add(hisDebateInvateUserADO);
						}
					}
				}
				if (lstDebateInvateUserADO != null && lstDebateInvateUserADO.Count > 0)
				{
					lstDebateInvateUserADO[0].Action = 1;
					((GridControl)(object)gridControl1).DataSource = lstDebateInvateUserADO;
				}
				else
				{
					FillDataToInvateUser();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ProcessHisDebate(HIS_DEBATE hisDebateSave)
		{
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Expected O, but got Unknown
			try
			{
				if (string.IsNullOrEmpty(icdMainText.Text))
				{
					if (cboIcdMain != null)
					{
						hisDebateSave.ICD_NAME = cboIcdMain.Text;
						hisDebateSave.ICD_CODE = txtIcdMain.Text;
					}
				}
				else
				{
					hisDebateSave.ICD_NAME = icdMainText.Text;
					if (cboIcdMain != null)
					{
						hisDebateSave.ICD_CODE = txtIcdMain.Text;
					}
				}
				if (!string.IsNullOrEmpty(txtIcdTextName.Text))
				{
					hisDebateSave.ICD_TEXT = txtIcdTextName.Text;
					hisDebateSave.ICD_SUB_CODE = txtIcdTextCode.Text;
				}
				else
				{
					hisDebateSave.ICD_TEXT = null;
					hisDebateSave.ICD_SUB_CODE = null;
				}
				if (dtOutTime.EditValue != null && dtOutTime.DateTime != DateTime.MinValue)
				{
					hisDebateSave.TREATMENT_TO_TIME = Parse.ToInt64(System.Convert.ToDateTime((dtOutTime.EditValue ?? "").ToString()).ToString("yyyyMMddHHmm") + "00");
				}
				else
				{
					hisDebateSave.TREATMENT_TO_TIME = null;
				}
				if (dtInTime.EditValue != null && dtInTime.DateTime != DateTime.MinValue)
				{
					hisDebateSave.TREATMENT_FROM_TIME = Parse.ToInt64(System.Convert.ToDateTime((dtInTime.EditValue ?? "").ToString()).ToString("yyyyMMddHHmm") + "00");
				}
				else
				{
					hisDebateSave.TREATMENT_FROM_TIME = null;
				}
				if (dtDebateTime.EditValue != null && dtDebateTime.DateTime != DateTime.MinValue)
				{
					hisDebateSave.DEBATE_TIME = Parse.ToInt64(System.Convert.ToDateTime((dtDebateTime.EditValue ?? "").ToString()).ToString("yyyyMMddHHmm") + "00");
				}
				else
				{
					hisDebateSave.DEBATE_TIME = null;
				}
				if (cboDebateType.EditValue != null)
				{
					hisDebateSave.DEBATE_TYPE_ID = (long)cboDebateType.EditValue;
				}
				else
				{
					hisDebateSave.DEBATE_TYPE_ID = null;
				}
				hisDebateSave.DEBATE_REASON_ID = ((cboDebateReason.EditValue != null) ? ((long?)cboDebateReason.EditValue) : ((long?)null));
				hisDebateSave.LOCATION = txtLocation.Text;
				if (detailProcessor != null)
				{
					HIS_DEBATE saveData = new HIS_DEBATE();
					detailProcessor.GetData(GetTypeDetail(), ref saveData);
					switch (GetTypeDetail())
					{
					case DetailEnum.Thuoc:
						hisDebateSave.SUBCLINICAL_PROCESSES = saveData.SUBCLINICAL_PROCESSES;
						hisDebateSave.CONTENT_TYPE = 2L;
						hisDebateSave.MEDICINE_TYPE_IDS = saveData.MEDICINE_TYPE_IDS;
						hisDebateSave.ACTIVE_INGREDIENT_IDS = saveData.ACTIVE_INGREDIENT_IDS;
						hisDebateSave.MEDICINE_TYPE_NAME = saveData.MEDICINE_TYPE_NAME;
						hisDebateSave.MEDICINE_CONCENTRA = saveData.MEDICINE_CONCENTRA;
						hisDebateSave.MEDICINE_USE_FORM_NAME = saveData.MEDICINE_USE_FORM_NAME;
						hisDebateSave.MEDICINE_TUTORIAL = saveData.MEDICINE_TUTORIAL;
						hisDebateSave.DISCUSSION = saveData.DISCUSSION;
						hisDebateSave.BEFORE_DIAGNOSTIC = saveData.BEFORE_DIAGNOSTIC;
						hisDebateSave.TREATMENT_METHOD = saveData.TREATMENT_METHOD;
						hisDebateSave.CARE_METHOD = saveData.CARE_METHOD;
						hisDebateSave.CONCLUSION = saveData.CONCLUSION;
						hisDebateSave.DIAGNOSTIC = saveData.DIAGNOSTIC;
						hisDebateSave.HOSPITALIZATION_STATE = saveData.HOSPITALIZATION_STATE;
						hisDebateSave.PATHOLOGICAL_HISTORY = saveData.PATHOLOGICAL_HISTORY;
						hisDebateSave.TREATMENT_TRACKING = saveData.TREATMENT_TRACKING;
						hisDebateSave.MEDICINE_USE_TIME = saveData.MEDICINE_USE_TIME;
						hisDebateSave.REQUEST_CONTENT = "";
						hisDebateSave.EMOTIONLESS_METHOD_ID = null;
						hisDebateSave.INTERNAL_MEDICINE_STATE = "";
						hisDebateSave.PROGNOSIS = "";
						hisDebateSave.SURGERY_SERVICE_ID = null;
						hisDebateSave.SURGERY_TIME = null;
						hisDebateSave.HIS_DEBATE_EKIP_USER = null;
						hisDebateSave.PTTT_METHOD_ID = null;
						hisDebateSave.PTTT_METHOD_NAME = null;
						break;
					case DetailEnum.Pttt:
						hisDebateSave.CONTENT_TYPE = 3L;
						hisDebateSave.EMOTIONLESS_METHOD_ID = saveData.EMOTIONLESS_METHOD_ID;
						hisDebateSave.PTTT_METHOD_ID = saveData.PTTT_METHOD_ID;
						hisDebateSave.PTTT_METHOD_NAME = saveData.PTTT_METHOD_NAME;
						hisDebateSave.INTERNAL_MEDICINE_STATE = saveData.INTERNAL_MEDICINE_STATE;
						hisDebateSave.PROGNOSIS = saveData.PROGNOSIS;
						hisDebateSave.TREATMENT_METHOD = saveData.TREATMENT_METHOD;
						hisDebateSave.CONCLUSION = saveData.CONCLUSION;
						hisDebateSave.SUBCLINICAL_PROCESSES = saveData.SUBCLINICAL_PROCESSES;
						hisDebateSave.SURGERY_SERVICE_ID = saveData.SURGERY_SERVICE_ID;
						hisDebateSave.SURGERY_TIME = saveData.SURGERY_TIME;
						hisDebateSave.HIS_DEBATE_EKIP_USER = saveData.HIS_DEBATE_EKIP_USER;
						hisDebateSave.TREATMENT_TRACKING = saveData.TREATMENT_TRACKING;
						hisDebateSave.MEDICINE_TYPE_IDS = "";
						hisDebateSave.ACTIVE_INGREDIENT_IDS = "";
						hisDebateSave.MEDICINE_TYPE_NAME = "";
						hisDebateSave.MEDICINE_CONCENTRA = "";
						hisDebateSave.MEDICINE_USE_FORM_NAME = "";
						hisDebateSave.MEDICINE_USE_TIME = null;
						hisDebateSave.MEDICINE_TUTORIAL = "";
						hisDebateSave.DISCUSSION = saveData.DISCUSSION;
						hisDebateSave.BEFORE_DIAGNOSTIC = saveData.BEFORE_DIAGNOSTIC;
						hisDebateSave.CARE_METHOD = saveData.CARE_METHOD;
						hisDebateSave.CONCLUSION = saveData.CONCLUSION;
						hisDebateSave.DIAGNOSTIC = saveData.DIAGNOSTIC;
						hisDebateSave.HOSPITALIZATION_STATE = saveData.HOSPITALIZATION_STATE;
						hisDebateSave.PATHOLOGICAL_HISTORY = saveData.PATHOLOGICAL_HISTORY;
						hisDebateSave.REQUEST_CONTENT = "";
						break;
					case DetailEnum.Khac:
						hisDebateSave.SUBCLINICAL_PROCESSES = saveData.SUBCLINICAL_PROCESSES;
						hisDebateSave.CONTENT_TYPE = 1L;
						hisDebateSave.DISCUSSION = saveData.DISCUSSION;
						hisDebateSave.BEFORE_DIAGNOSTIC = saveData.BEFORE_DIAGNOSTIC;
						hisDebateSave.CARE_METHOD = saveData.CARE_METHOD;
						hisDebateSave.CONCLUSION = saveData.CONCLUSION;
						hisDebateSave.DIAGNOSTIC = saveData.DIAGNOSTIC;
						hisDebateSave.TREATMENT_METHOD = saveData.TREATMENT_METHOD;
						hisDebateSave.HOSPITALIZATION_STATE = saveData.HOSPITALIZATION_STATE;
						hisDebateSave.PATHOLOGICAL_HISTORY = saveData.PATHOLOGICAL_HISTORY;
						hisDebateSave.TREATMENT_TRACKING = saveData.TREATMENT_TRACKING;
						hisDebateSave.REQUEST_CONTENT = saveData.REQUEST_CONTENT;
						hisDebateSave.MEDICINE_TYPE_IDS = "";
						hisDebateSave.ACTIVE_INGREDIENT_IDS = "";
						hisDebateSave.MEDICINE_TYPE_NAME = "";
						hisDebateSave.MEDICINE_CONCENTRA = "";
						hisDebateSave.MEDICINE_USE_FORM_NAME = "";
						hisDebateSave.MEDICINE_USE_TIME = null;
						hisDebateSave.MEDICINE_TUTORIAL = "";
						hisDebateSave.EMOTIONLESS_METHOD_ID = null;
						hisDebateSave.INTERNAL_MEDICINE_STATE = "";
						hisDebateSave.PROGNOSIS = "";
						hisDebateSave.SURGERY_SERVICE_ID = null;
						hisDebateSave.SURGERY_TIME = null;
						hisDebateSave.HIS_DEBATE_EKIP_USER = null;
						hisDebateSave.PTTT_METHOD_ID = null;
						hisDebateSave.PTTT_METHOD_NAME = null;
						break;
					}
				}
				else
				{
					hisDebateSave.MEDICINE_TYPE_IDS = "";
					hisDebateSave.ACTIVE_INGREDIENT_IDS = "";
					hisDebateSave.MEDICINE_TYPE_NAME = "";
					hisDebateSave.MEDICINE_CONCENTRA = "";
					hisDebateSave.MEDICINE_USE_FORM_NAME = "";
					hisDebateSave.MEDICINE_USE_TIME = null;
					hisDebateSave.MEDICINE_TUTORIAL = "";
					hisDebateSave.DISCUSSION = "";
					hisDebateSave.BEFORE_DIAGNOSTIC = "";
					hisDebateSave.CARE_METHOD = "";
					hisDebateSave.CONCLUSION = "";
					hisDebateSave.DIAGNOSTIC = "";
					hisDebateSave.TREATMENT_METHOD = "";
					hisDebateSave.HOSPITALIZATION_STATE = "";
					hisDebateSave.PATHOLOGICAL_HISTORY = "";
					hisDebateSave.REQUEST_CONTENT = "";
					hisDebateSave.CONTENT_TYPE = null;
					hisDebateSave.EMOTIONLESS_METHOD_ID = null;
					hisDebateSave.INTERNAL_MEDICINE_STATE = "";
					hisDebateSave.PROGNOSIS = "";
					hisDebateSave.SUBCLINICAL_PROCESSES = "";
					hisDebateSave.SURGERY_SERVICE_ID = null;
					hisDebateSave.SURGERY_TIME = null;
					hisDebateSave.TREATMENT_TRACKING = "";
					hisDebateSave.HIS_DEBATE_EKIP_USER = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessHisDebateTemp(HIS_DEBATE_TEMP hisDebatetemp)
		{
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			try
			{
				if (detailProcessor != null)
				{
					HIS_DEBATE saveData = new HIS_DEBATE();
					detailProcessor.GetData(GetTypeDetail(), ref saveData);
					ProcessHisDebateUser(saveData);
					hisDebatetemp.BEFORE_DIAGNOSTIC = saveData.BEFORE_DIAGNOSTIC;
					hisDebatetemp.CARE_METHOD = saveData.CARE_METHOD;
					hisDebatetemp.CONCLUSION = saveData.CONCLUSION;
					hisDebatetemp.DIAGNOSTIC = saveData.DIAGNOSTIC;
					hisDebatetemp.TREATMENT_METHOD = saveData.TREATMENT_METHOD;
					hisDebatetemp.HOSPITALIZATION_STATE = saveData.HOSPITALIZATION_STATE;
					hisDebatetemp.PATHOLOGICAL_HISTORY = saveData.PATHOLOGICAL_HISTORY;
					hisDebatetemp.REQUEST_CONTENT = saveData.REQUEST_CONTENT;
					hisDebatetemp.TREATMENT_TRACKING = saveData.TREATMENT_TRACKING;
					hisDebatetemp.HIS_DEBATE_USER = saveData.HIS_DEBATE_USER;
					hisDebatetemp.DEPARTMENT_ID = WorkPlaceSDO.DepartmentId;
					hisDebatetemp.DISCUSSION = saveData.DISCUSSION;
				}
				else
				{
					HIS_DEBATE val = new HIS_DEBATE();
					ProcessHisDebateUser(val);
					hisDebatetemp.BEFORE_DIAGNOSTIC = "";
					hisDebatetemp.CARE_METHOD = "";
					hisDebatetemp.CONCLUSION = "";
					hisDebatetemp.DIAGNOSTIC = "";
					hisDebatetemp.HOSPITALIZATION_STATE = "";
					hisDebatetemp.PATHOLOGICAL_HISTORY = "";
					hisDebatetemp.REQUEST_CONTENT = "";
					hisDebatetemp.TREATMENT_METHOD = "";
					hisDebatetemp.TREATMENT_TRACKING = "";
					hisDebatetemp.HIS_DEBATE_USER = val.HIS_DEBATE_USER;
					hisDebatetemp.DEPARTMENT_ID = WorkPlaceSDO.DepartmentId;
					hisDebatetemp.DISCUSSION = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessHisDebateUser(HIS_DEBATE hisDebateSave)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Expected O, but got Unknown
			try
			{
				List<HIS_DEBATE_USER> list = new List<HIS_DEBATE_USER>();
				List<HisDebateUserADO> list2 = gridControl.DataSource as List<HisDebateUserADO>;
				list2 = ((list2 != null) ? list2.Where((HisDebateUserADO o) => !string.IsNullOrWhiteSpace(((HIS_DEBATE_USER)o).LOGINNAME) || (!string.IsNullOrEmpty(((HIS_DEBATE_USER)o).USERNAME) && o.OUT_OF_HOSPITAL)).ToList() : null);
				if (list2 != null && list2.Count > 0)
				{
					foreach (HisDebateUserADO item_DebateUser in list2)
					{
						if (string.IsNullOrWhiteSpace(((HIS_DEBATE_USER)item_DebateUser).LOGINNAME) && !item_DebateUser.OUT_OF_HOSPITAL)
						{
							continue;
						}
						HIS_DEBATE_USER val = new HIS_DEBATE_USER();
						DataObjectMapper.Map<HIS_DEBATE_USER>((object)val, (object)item_DebateUser);
						if (item_DebateUser.OUT_OF_HOSPITAL)
						{
							val.USERNAME = val.USERNAME;
							val.IS_OUT_OF_HOSPITAL = (short)1;
							val.DEBATE_TEMP_ID = null;
							val.ID = 0L;
							if (item_DebateUser.PRESIDENT)
							{
								val.IS_PRESIDENT = (short)1;
							}
							else
							{
								val.IS_PRESIDENT = null;
							}
							if (item_DebateUser.SECRETARY)
							{
								val.IS_SECRETARY = (short)1;
							}
							else
							{
								val.IS_SECRETARY = null;
							}
							list.Add(val);
							continue;
						}
						ACS_USER val2 = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_USER)item_DebateUser).LOGINNAME);
						if (val2 != null)
						{
							val.LOGINNAME = val2.LOGINNAME;
							val.DEBATE_TEMP_ID = null;
							if (!string.IsNullOrEmpty(val2.USERNAME))
							{
								val.USERNAME = val2.USERNAME;
							}
							val.ID = 0L;
							if (item_DebateUser.PRESIDENT)
							{
								val.IS_PRESIDENT = (short)1;
							}
							else
							{
								val.IS_PRESIDENT = null;
							}
							if (item_DebateUser.SECRETARY)
							{
								val.IS_SECRETARY = (short)1;
							}
							else
							{
								val.IS_SECRETARY = null;
							}
							val.IS_OUT_OF_HOSPITAL = null;
							list.Add(val);
						}
					}
					if (list != null && list.Count > 0)
					{
						hisDebateSave.HIS_DEBATE_USER = list;
					}
					else
					{
						hisDebateSave.HIS_DEBATE_USER = null;
					}
				}
				else
				{
					hisDebateSave.HIS_DEBATE_USER = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessHisDebateInvateUser(HIS_DEBATE hisDebateSave)
		{
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Expected O, but got Unknown
			try
			{
				List<HIS_DEBATE_INVITE_USER> list = new List<HIS_DEBATE_INVITE_USER>();
				List<HisDebateInvateUserADO> list2 = ((GridControl)(object)gridControl1).DataSource as List<HisDebateInvateUserADO>;
				list2 = ((list2 != null) ? list2.Where((HisDebateInvateUserADO o) => !string.IsNullOrWhiteSpace(((HIS_DEBATE_INVITE_USER)o).LOGINNAME)).ToList() : null);
				if (list2 != null && list2.Count > 0)
				{
					if (list2 == null || list2.Count <= 0 || list2.Where((HisDebateInvateUserADO o) => o.PRESIDENT).ToList() == null || list2.Where((HisDebateInvateUserADO o) => o.PRESIDENT).ToList().Count <= 1)
					{
						if (list2 != null && list2.Count > 0 && list2.Where((HisDebateInvateUserADO o) => !string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)o).COMMENT_DOCTOR)).ToList() != null && list2.Where((HisDebateInvateUserADO o) => !string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)o).COMMENT_DOCTOR)).ToList().Count > 1)
						{
							string text = "";
							List<HisDebateInvateUserADO> list3 = list2.Where((HisDebateInvateUserADO o) => !string.IsNullOrEmpty(((HIS_DEBATE_INVITE_USER)o).COMMENT_DOCTOR)).ToList();
							foreach (HisDebateInvateUserADO item in list3)
							{
								if (CountVi.Count(((HIS_DEBATE_INVITE_USER)item).COMMENT_DOCTOR) > 1000)
								{
									text = ((HIS_DEBATE_INVITE_USER)item).LOGINNAME + " - " + GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)item).LOGINNAME).USERNAME + " có \"Nhận xét\" vượt quá 1000 ký tự.\r\n";
								}
							}
							if (!string.IsNullOrEmpty(text))
							{
								XtraMessageBox.Show(text, "Thông báo danh sách mời tham gia", MessageBoxButtons.OK);
								return;
							}
						}
						{
							foreach (HisDebateInvateUserADO item_DebateUser in list2)
							{
								if (string.IsNullOrWhiteSpace(((HIS_DEBATE_INVITE_USER)item_DebateUser).LOGINNAME))
								{
									continue;
								}
								HIS_DEBATE_INVITE_USER val = new HIS_DEBATE_INVITE_USER();
								DataObjectMapper.Map<HIS_DEBATE_INVITE_USER>((object)val, (object)item_DebateUser);
								ACS_USER val2 = GlobalStore.HisAcsUser.FirstOrDefault((ACS_USER o) => o.LOGINNAME == ((HIS_DEBATE_INVITE_USER)item_DebateUser).LOGINNAME);
								if (val2 != null)
								{
									val.LOGINNAME = val2.LOGINNAME;
									if (!string.IsNullOrEmpty(val2.USERNAME))
									{
										val.USERNAME = val2.USERNAME;
									}
									val.ID = 0L;
									if (((HIS_DEBATE_INVITE_USER)item_DebateUser).ID > 0)
									{
										val.ID = ((HIS_DEBATE_INVITE_USER)item_DebateUser).ID;
									}
									if (((HIS_DEBATE_INVITE_USER)item_DebateUser).EXECUTE_ROLE_ID > 0)
									{
										val.DESCRIPTION = ListExecuteRole.FirstOrDefault((HIS_EXECUTE_ROLE o) => o.ID == ((HIS_DEBATE_INVITE_USER)item_DebateUser).EXECUTE_ROLE_ID).EXECUTE_ROLE_NAME;
									}
									if (item_DebateUser.PRESIDENT)
									{
										val.IS_PRESIDENT = (short)1;
									}
									else
									{
										val.IS_PRESIDENT = null;
									}
									if (item_DebateUser.SECRETARY)
									{
										val.IS_SECRETARY = (short)1;
									}
									else
									{
										val.IS_SECRETARY = null;
									}
									list.Add(val);
									hisDebateSave.HIS_DEBATE_INVITE_USER = list;
								}
								else
								{
									hisDebateSave.HIS_DEBATE_INVITE_USER = null;
								}
							}
							return;
						}
					}
					XtraMessageBox.Show("Không được có hơn một chủ tọa", "Thông báo danh sách mời tham gia", MessageBoxButtons.OK);
				}
				else
				{
					hisDebateSave.HIS_DEBATE_INVITE_USER = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SaveHisDebate(HIS_DEBATE hisDebateSave)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Expected O, but got Unknown
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			CommonParam val = new CommonParam();
			bool flag = false;
			try
			{
				WaitingManager.Show();
				HIS_DEBATE hisDebateResult = new HIS_DEBATE();
				LogSystem.Debug(LogUtil.TraceData("hisDebateSave__:", (object)hisDebateSave));
				if (action == 1)
				{
					HisDebateCreateAutoTrackingSDO val2 = new HisDebateCreateAutoTrackingSDO();
					val2.HisDebate = hisDebateSave;
					if (chkAutoCreateTracking.Checked)
					{
						val2.IsAutoCreateTracking = (short)1;
					}
					hisDebateResult = ((AdapterBase)new BackendAdapter(val)).Post<HIS_DEBATE>("api/HisDebate/CreateAutoTracking", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val);
				}
				else if (action == 2 && currentHisDebate != null)
				{
					hisDebateResult = ((AdapterBase)new BackendAdapter(val)).Post<HIS_DEBATE>("/api/HisDebate/Update", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)hisDebateSave, val);
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<HIS_DEBATE>((Expression<Func<HIS_DEBATE>>)(() => hisDebateResult)), (object)hisDebateResult));
				if (hisDebateResult != null)
				{
					LoadDataInviteDebate(hisDebateResult);
					flag = true;
					btnPrint.Enabled = true;
					btnSendTMP.Enabled = true;
					currentHisDebate = hisDebateResult;
					action = 2;
					if (lciAutoCreateEmr.Visibility == LayoutVisibility.Always && chkAutoCreateEmr.Checked)
					{
						isCreateEmrDocument = true;
						RichEditorStore val3 = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.ROOT_PATH);
						val3.RunPrintTemplate("Mps000019", new DelegateRunPrinter(DelegateRunPrinter));
						val3.RunPrintTemplate("Mps000020", new DelegateRunPrinter(DelegateRunPrinter));
					}
				}
				WaitingManager.Hide();
				MessageManager.Show((Form)(object)this, val, (bool?)flag);
				if (flag && hisDebateSave.HIS_DEBATE_INVITE_USER != null && hisDebateSave.HIS_DEBATE_INVITE_USER.Count > 0)
				{
					List<string> list = new List<string>();
					foreach (HIS_DEBATE_INVITE_USER item in hisDebateSave.HIS_DEBATE_INVITE_USER)
					{
						if (!item.IS_PARTICIPATION.HasValue)
						{
							list.Add(item.LOGINNAME);
						}
					}
					if (list == null || list.Count == 0)
					{
						return;
					}
					SDA_NOTIFY updateDTO = new SDA_NOTIFY();
					updateDTO.CONTENT = string.Format("Bạn có lời mời tham gia hội chẩn cho bệnh nhân {0} – {1}, {2}. Mời bạn vào chức năng “Biên bản hội chẩn” để xem chi tiết", vHisTreatment.TREATMENT_CODE, vHisTreatment.TDL_PATIENT_NAME, listDepartment.FirstOrDefault((HIS_DEPARTMENT o) => o.ID == long.Parse((cboDepartment.EditValue ?? "").ToString())).DEPARTMENT_NAME);
					updateDTO.TITLE = "Thông báo mời tham gia hội chẩn";
					updateDTO.FROM_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((DateTime?)DateTime.Now).GetValueOrDefault();
					updateDTO.TO_TIME = Parse.ToInt64(System.Convert.ToDateTime(dtDebateTime.DateTime).ToString("yyyyMMdd") + "235959");
					foreach (string item2 in list)
					{
						updateDTO.RECEIVER_LOGINNAME = item2;
						LogSystem.Debug("updateDTO___SDA" + LogUtil.TraceData(LogUtil.GetMemberName<SDA_NOTIFY>((Expression<Func<SDA_NOTIFY>>)(() => updateDTO)), (object)updateDTO));
						SDA_NOTIFY val4 = ((AdapterBase)new BackendAdapter(val)).Post<SDA_NOTIFY>("api/SdaNotify/Create", HIS.Desktop.ApiConsumer.ApiConsumers.SdaConsumer, (object)updateDTO, val);
						if (val4 == null)
						{
						}
					}
				}
				SessionManager.ProcessTokenLost(val);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void cboDebateType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboDebateType.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDebateType_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtLocation.Focus();
					txtLocation.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDebateType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					txtLocation.Focus();
					txtLocation.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataToButtonPrint()
		{
			try
			{
				DXPopupMenu dXPopupMenu = new DXPopupMenu();
				DXMenuItem dXMenuItem = new DXMenuItem(Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__PRINT_DEBATE", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang));
				dXMenuItem.Tag = ModulePrintType.BIEN_BAN_HOI_CHAN;
				dXMenuItem.Click += onClickDebatePrint;
				dXPopupMenu.Items.Add(dXMenuItem);
				DXMenuItem dXMenuItem2 = new DXMenuItem(Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__PRINT_BIEN_BAN_HOI_CHAN_THUOC_DAU_SAO", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang));
				dXMenuItem2.Tag = ModulePrintType.BIEN_BAN_HOI_CHAN_THUOC_DAU_SAO;
				dXMenuItem2.Click += onClickDebatePrint;
				dXPopupMenu.Items.Add(dXMenuItem2);
				DXMenuItem dXMenuItem3 = new DXMenuItem(Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY__FORM_DEBATE_DIAGNOSTIC__PRINT_SO_HOI_CHAN", ResourceLanguageManager.LanguageFormDebateDiagnostic, cultureLang));
				dXMenuItem3.Tag = ModulePrintType.SO_BIEN_BAN_HOI_CHAN;
				dXMenuItem3.Click += onClickDebatePrint;
				dXPopupMenu.Items.Add(dXMenuItem3);
				DXMenuItem dXMenuItem4 = new DXMenuItem("Biên bản hội chẩn trước phẫu thuật");
				dXMenuItem4.Tag = ModulePrintType.HOI_CHAN_PTTT;
				dXMenuItem4.Click += onClickDebatePrint;
				dXPopupMenu.Items.Add(dXMenuItem4);
				btnPrint.DropDownControl = dXPopupMenu;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void onClickDebatePrint(object sender, EventArgs e)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			try
			{
				isCreateEmrDocument = false;
				RichEditorStore val = new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, LanguageManager.GetLanguage(), PrintStoreLocation.ROOT_PATH);
				DXMenuItem dXMenuItem = sender as DXMenuItem;
				switch ((ModulePrintType)dXMenuItem.Tag)
				{
				case ModulePrintType.BIEN_BAN_HOI_CHAN:
					val.RunPrintTemplate("Mps000019", new DelegateRunPrinter(DelegateRunPrinter));
					break;
				case ModulePrintType.BIEN_BAN_HOI_CHAN_THUOC_DAU_SAO:
					val.RunPrintTemplate("Mps000323", new DelegateRunPrinter(DelegateRunPrinter));
					break;
				case ModulePrintType.SO_BIEN_BAN_HOI_CHAN:
					val.RunPrintTemplate("Mps000020", new DelegateRunPrinter(DelegateRunPrinter));
					break;
				case ModulePrintType.HOI_CHAN_PTTT:
					val.RunPrintTemplate("Mps000387", new DelegateRunPrinter(DelegateRunPrinter));
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool DelegateRunPrinter(string printTypeCode, string fileName)
		{
			bool result = false;
			try
			{
				switch (printTypeCode)
				{
				case "Mps000019":
					InTrichBienBanHoiChanProcess(printTypeCode, fileName, ref result);
					break;
				case "Mps000323":
					InTrichBienBanHoiChanThuocDauSaoProcess(printTypeCode, fileName, ref result);
					break;
				case "Mps000020":
					InSoBienBanHoiChanProcess(printTypeCode, fileName, ref result);
					break;
				case "Mps000387":
					InHoiChanPtttProcess(printTypeCode, fileName, ref result);
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private List<SignerConfigDTO> ProcessSigner(List<HIS_DEBATE_USER> debateUserList)
		{
			List<SignerConfigDTO> list = new List<SignerConfigDTO>();
			try
			{
				if (debateUserList == null || debateUserList.Count == 0)
				{
					return null;
				}
				string loginName = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
				debateUserList = debateUserList.OrderBy((HIS_DEBATE_USER o) => o.ID).ToList();
				int num = 3;
				foreach (HIS_DEBATE_USER debateUser in debateUserList)
				{
					SignerConfigDTO signerConfigDTO = new SignerConfigDTO();
					if (debateUser.LOGINNAME == loginName)
					{
						if (debateUser.IS_SECRETARY == 1)
						{
							signerConfigDTO.Loginname = loginName;
							signerConfigDTO.NumOrder = 2L;
						}
						else if (!debateUser.IS_PRESIDENT.HasValue && !debateUser.IS_SECRETARY.HasValue)
						{
							signerConfigDTO.Loginname = loginName;
							signerConfigDTO.NumOrder = 1L;
						}
						else if (debateUser.IS_PRESIDENT == 1)
						{
							signerConfigDTO.Loginname = debateUser.LOGINNAME;
							signerConfigDTO.NumOrder = 100L;
						}
					}
					else if (debateUser.IS_PRESIDENT == 1)
					{
						signerConfigDTO.Loginname = debateUser.LOGINNAME;
						signerConfigDTO.NumOrder = 100L;
					}
					else if (debateUser.IS_SECRETARY == 1)
					{
						signerConfigDTO.Loginname = debateUser.LOGINNAME;
						signerConfigDTO.NumOrder = 2L;
					}
					else
					{
						signerConfigDTO.Loginname = debateUser.LOGINNAME;
						signerConfigDTO.NumOrder = num++;
					}
					list.Add(signerConfigDTO);
				}
			}
			catch (Exception ex)
			{
				list = null;
				LogSystem.Warn(ex);
			}
			return list;
		}

		private void InTrichBienBanHoiChanProcess(string printTypeCode, string fileName, ref bool result)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Expected O, but got Unknown
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037a: Expected O, but got Unknown
			//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Expected O, but got Unknown
			//IL_0414: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Expected O, but got Unknown
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Expected O, but got Unknown
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Expected O, but got Unknown
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				WaitingManager.Show();
				var patient = HIS.Desktop.Print.PrintGlobalStore.getPatient(treatment_id);
				HisDebateViewFilter val = new HisDebateViewFilter();
				((FilterBase)val).ID = currentHisDebate.ID;
				CommonParam val2 = new CommonParam();
				V_HIS_DEBATE val3 = ((AdapterBase)new BackendAdapter(val2)).Get<List<V_HIS_DEBATE>>("api/HisDebate/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, val2).FirstOrDefault();
				V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(treatment_id);
				string roomName = WorkPlaceSDO.RoomName;
				string departmentName = WorkPlaceSDO.DepartmentName;
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<HIS_DEBATE>((Expression<Func<HIS_DEBATE>>)(() => currentHisDebate)), (object)currentHisDebate));
				List<HIS_DEBATE_USER> list = null;
				if (currentHisDebate.HIS_DEBATE_USER == null || currentHisDebate.HIS_DEBATE_USER.Count == 0)
				{
					if (hisDebate != null && hisDebate.ID > 0)
					{
						HisDebateUserFilter val4 = new HisDebateUserFilter();
						val4.DEBATE_ID = hisDebate.ID;
						((FilterBase)val4).ORDER_DIRECTION = "ASC";
						((FilterBase)val4).ORDER_FIELD = "ID";
						list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val4, (CommonParam)null);
						currentHisDebate.HIS_DEBATE_USER = list;
					}
					else
					{
						currentHisDebate.HIS_DEBATE_USER = new List<HIS_DEBATE_USER>();
					}
				}
				else
				{
					list = currentHisDebate.HIS_DEBATE_USER.ToList();
				}
				HisTreatmentBedRoomViewFilter val5 = new HisTreatmentBedRoomViewFilter();
				val5.TREATMENT_ID = currentHisDebate.TREATMENT_ID;
				List<V_HIS_TREATMENT_BED_ROOM> list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_TREATMENT_BED_ROOM>>("/api/HisTreatmentBedRoom/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val5, (CommonParam)null);
				MPS.Processor.Mps000019.PDO.Mps000019PDO.Mps000019SingleKey val6 = new MPS.Processor.Mps000019.PDO.Mps000019PDO.Mps000019SingleKey();
				val6.bebRoomName = roomName;
				val6.departmentName = departmentName;
				val6.genderCode__Male = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.ID == 2).GENDER_CODE;
				val6.genderCode__FeMale = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.ID == 1).GENDER_CODE;
				if (list2 != null && list2.Count > 0)
				{
					V_HIS_TREATMENT_BED_ROOM bed = (from o in list2
						where o.BED_ID.HasValue
						orderby o.OUT_TIME.HasValue descending
						select o).FirstOrDefault();
					if (bed != null)
					{
						V_HIS_BED val7 = BackendDataWorker.Get<V_HIS_BED>().FirstOrDefault((V_HIS_BED o) => o.ID == bed.BED_ID);
						val6.BED_CODE = ((val7 != null) ? val7.BED_CODE : "");
						val6.BED_NAME = ((val7 != null) ? val7.BED_NAME : "");
					}
				}
				if (vHisTreatment != null)
				{
					val6.IN_CODE = vHisTreatment.IN_CODE;
				}
				long valueOrDefault = Inventec.Common.DateTime.Get.Now().GetValueOrDefault();
				V_HIS_PATIENT_TYPE_ALTER val8 = new V_HIS_PATIENT_TYPE_ALTER();
				PrintGlobalStore.LoadCurrentPatientTypeAlter(treatment_id, valueOrDefault, ref val8);
				WaitingManager.Hide();
				if (list == null)
				{
					list = new List<HIS_DEBATE_USER>();
				}
				Mps000019PDO val9 = new Mps000019PDO((V_HIS_PATIENT)(object)patient, val3, departmentTran, val6, list, vHisTreatment, val8);
				PrintData val10 = null;
				string text = "";
				if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
				{
					text = GlobalVariables.dicPrinter[printTypeCode];
				}
				val10 = ((isCreateEmrDocument && lciAutoCreateEmr.Visibility == LayoutVisibility.Always && chkAutoCreateEmr.Checked) ? new PrintData(printTypeCode, fileName, (object)val9, (MPS.ProcessorBase.PrintConfig.PreviewType)7, text) : ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, (object)val9, (MPS.ProcessorBase.PrintConfig.PreviewType)1, text) : new PrintData(printTypeCode, fileName, (object)val9, (MPS.ProcessorBase.PrintConfig.PreviewType)2, text)));
				InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((vHisTreatment != null) ? vHisTreatment.TREATMENT_CODE : "", printTypeCode, (long?)base.currentModuleBase.RoomId);
				if (lciAutoSign.Visibility == LayoutVisibility.Always && chkAutoSign.Checked)
				{
					List<SignerConfigDTO> signerConfigs = ProcessSigner(list);
					inputADO.SignerConfigs = signerConfigs;
				}
				val10.EmrInputADO = inputADO;
				LogSystem.Info(LogUtil.TraceData("MPS.Processor.Mps000019.PDO.Mps000019PDO inputADO: ", (object)inputADO));
				result = MpsPrinter.Run(val10);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void InTrichBienBanHoiChanThuocDauSaoProcess(string printTypeCode, string fileName, ref bool result)
		{
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Expected O, but got Unknown
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Expected O, but got Unknown
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Expected O, but got Unknown
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Expected O, but got Unknown
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				WaitingManager.Show();
                var patient = HIS.Desktop.Print.PrintGlobalStore.getPatient(treatment_id);
				V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(treatment_id);
				string roomName = WorkPlaceSDO.RoomName;
				string departmentName = WorkPlaceSDO.DepartmentName;
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName<HIS_DEBATE>((Expression<Func<HIS_DEBATE>>)(() => currentHisDebate)), (object)currentHisDebate));
				List<HIS_DEBATE_USER> list = null;
				if (currentHisDebate.HIS_DEBATE_USER == null || currentHisDebate.HIS_DEBATE_USER.Count == 0)
				{
					if (hisDebate != null && hisDebate.ID > 0)
					{
						HisDebateUserFilter val = new HisDebateUserFilter();
						val.DEBATE_ID = hisDebate.ID;
						((FilterBase)val).ORDER_DIRECTION = "ASC";
						((FilterBase)val).ORDER_FIELD = "ID";
						list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val, (CommonParam)null);
						currentHisDebate.HIS_DEBATE_USER = list;
					}
					else
					{
						currentHisDebate.HIS_DEBATE_USER = new List<HIS_DEBATE_USER>();
					}
				}
				else
				{
					list = currentHisDebate.HIS_DEBATE_USER.ToList();
				}
				HisTreatmentBedRoomViewFilter val2 = new HisTreatmentBedRoomViewFilter();
				val2.TREATMENT_ID = currentHisDebate.TREATMENT_ID;
				List<V_HIS_TREATMENT_BED_ROOM> list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_TREATMENT_BED_ROOM>>("/api/HisTreatmentBedRoom/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
				MPS.Processor.Mps000323.PDO.Mps000323PDO.Mps000323SingleKey val3 = new MPS.Processor.Mps000323.PDO.Mps000323PDO.Mps000323SingleKey();
				val3.bebRoomName = roomName;
				val3.departmentName = departmentName;
				val3.genderCode__Male = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.ID == 2).GENDER_CODE;
				val3.genderCode__FeMale = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.ID == 1).GENDER_CODE;
				if (list2 != null && list2.Count > 0)
				{
					V_HIS_TREATMENT_BED_ROOM bed = (from o in list2
						where o.BED_ID.HasValue
						orderby o.OUT_TIME.HasValue descending
						select o).FirstOrDefault();
					if (bed != null)
					{
						V_HIS_BED val4 = BackendDataWorker.Get<V_HIS_BED>().FirstOrDefault((V_HIS_BED o) => o.ID == bed.BED_ID);
						val3.BED_CODE = ((val4 != null) ? val4.BED_CODE : "");
						val3.BED_NAME = ((val4 != null) ? val4.BED_NAME : "");
					}
				}
				WaitingManager.Hide();
				Mps000323PDO val5 = new Mps000323PDO((V_HIS_PATIENT)(object)patient, currentHisDebate, departmentTran, val3, vHisTreatment);
				PrintData val6 = null;
				string text = "";
				if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
				{
					text = GlobalVariables.dicPrinter[printTypeCode];
				}
				val6 = ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, (object)val5, (MPS.ProcessorBase.PrintConfig.PreviewType)1, text) : new PrintData(printTypeCode, fileName, (object)val5, (MPS.ProcessorBase.PrintConfig.PreviewType)2, text));
				InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((vHisTreatment != null) ? vHisTreatment.TREATMENT_CODE : "", printTypeCode, (long?)base.currentModuleBase.RoomId);
				if (lciAutoSign.Visibility == LayoutVisibility.Always && chkAutoSign.Checked)
				{
					List<SignerConfigDTO> signerConfigs = ProcessSigner(list);
					inputADO.SignerConfigs = signerConfigs;
				}
				val6.EmrInputADO = inputADO;
				result = MpsPrinter.Run(val6);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void InSoBienBanHoiChanProcess(string printTypeCode, string fileName, ref bool result)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Expected O, but got Unknown
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Expected O, but got Unknown
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Expected O, but got Unknown
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				WaitingManager.Show();
				var patient = HIS.Desktop.Print.PrintGlobalStore.getPatient(treatment_id);
				V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(treatment_id);
				string roomName = WorkPlaceSDO.RoomName;
				string departmentName = WorkPlaceSDO.DepartmentName;
				string currentDateSeparateFullTime = GlobalReportQuery.GetCurrentDateSeparateFullTime();
				long valueOrDefault = Inventec.Common.DateTime.Get.Now().GetValueOrDefault();
				V_HIS_PATIENT_TYPE_ALTER val = new V_HIS_PATIENT_TYPE_ALTER();
				PrintGlobalStore.LoadCurrentPatientTypeAlter(treatment_id, valueOrDefault, ref val);
				List<HIS_DEBATE_USER> list = null;
				if (currentHisDebate.HIS_DEBATE_USER == null || currentHisDebate.HIS_DEBATE_USER.Count == 0)
				{
					if (hisDebate != null && hisDebate.ID > 0)
					{
						HisDebateUserFilter val2 = new HisDebateUserFilter();
						val2.DEBATE_ID = hisDebate.ID;
						((FilterBase)val2).ORDER_DIRECTION = "ASC";
						((FilterBase)val2).ORDER_FIELD = "ID";
						list = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, (CommonParam)null);
						currentHisDebate.HIS_DEBATE_USER = list;
					}
					else
					{
						currentHisDebate.HIS_DEBATE_USER = new List<HIS_DEBATE_USER>();
					}
				}
				else
				{
					list = currentHisDebate.HIS_DEBATE_USER.ToList();
				}
				HisDebateViewFilter val3 = new HisDebateViewFilter();
				((FilterBase)val3).ID = currentHisDebate.ID;
				List<V_HIS_DEBATE> list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_DEBATE>>("api/HisDebate/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val3, (CommonParam)null);
				WaitingManager.Hide();
				Mps000020PDO val4 = new Mps000020PDO((V_HIS_PATIENT)(object)patient, roomName, departmentName, currentHisDebate, departmentTran, currentDateSeparateFullTime, val, vHisTreatment, list2[0]);
				PrintData val5 = null;
				string text = "";
				if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
				{
					text = GlobalVariables.dicPrinter[printTypeCode];
				}
				val5 = ((isCreateEmrDocument && lciAutoCreateEmr.Visibility == LayoutVisibility.Always && chkAutoCreateEmr.Checked) ? new PrintData(printTypeCode, fileName, (object)val4, (MPS.ProcessorBase.PrintConfig.PreviewType)7, text) : ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, (object)val4, (MPS.ProcessorBase.PrintConfig.PreviewType)1, text) : new PrintData(printTypeCode, fileName, (object)val4, (MPS.ProcessorBase.PrintConfig.PreviewType)2, text)));
				InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((vHisTreatment != null) ? vHisTreatment.TREATMENT_CODE : "", printTypeCode, (long?)base.currentModuleBase.RoomId);
				if (lciAutoSign.Visibility == LayoutVisibility.Always && chkAutoSign.Checked)
				{
					List<SignerConfigDTO> signerConfigs = ProcessSigner(list);
					inputADO.SignerConfigs = signerConfigs;
				}
				val5.EmrInputADO = inputADO;
				result = MpsPrinter.Run(val5);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void InHoiChanPtttProcess(string printTypeCode, string fileName, ref bool result)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Expected O, but got Unknown
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Expected O, but got Unknown
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Expected O, but got Unknown
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Expected O, but got Unknown
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Expected O, but got Unknown
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Expected O, but got Unknown
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Expected O, but got Unknown
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				WaitingManager.Show();
				V_HIS_DEBATE val = new V_HIS_DEBATE();
				HisDebateViewFilter val2 = new HisDebateViewFilter();
				((FilterBase)val2).ID = currentHisDebate.ID;
				CommonParam val3 = new CommonParam();
				List<V_HIS_DEBATE> list = ((AdapterBase)new BackendAdapter(val3)).Get<List<V_HIS_DEBATE>>("api/HisDebate/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val2, val3);
				if (list != null && list.Count > 0)
				{
					val = list.FirstOrDefault();
				}
				V_HIS_DEPARTMENT_TRAN departmentTran = PrintGlobalStore.getDepartmentTran(treatment_id);
				List<HIS_DEBATE_USER> list2 = new List<HIS_DEBATE_USER>();
				if (currentHisDebate.HIS_DEBATE_USER == null || currentHisDebate.HIS_DEBATE_USER.Count == 0)
				{
					if (val != null && val.ID > 0)
					{
						HisDebateUserFilter val4 = new HisDebateUserFilter();
						val4.DEBATE_ID = val.ID;
						list2 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<HIS_DEBATE_USER>>("api/HisDebateUser/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val4, (CommonParam)null);
					}
				}
				else
				{
					list2 = currentHisDebate.HIS_DEBATE_USER.ToList();
				}
				HisTreatmentBedRoomViewFilter val5 = new HisTreatmentBedRoomViewFilter();
				val5.TREATMENT_ID = val.TREATMENT_ID;
				List<V_HIS_TREATMENT_BED_ROOM> list3 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_TREATMENT_BED_ROOM>>("/api/HisTreatmentBedRoom/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val5, (CommonParam)null);
				MPS.Processor.Mps000387.PDO.Mps000387PDO.Mps000387SingleKey val6 = new MPS.Processor.Mps000387.PDO.Mps000387PDO.Mps000387SingleKey();
				if (WorkPlaceSDO != null)
				{
					val6.BEB_ROOM_NAME = WorkPlaceSDO.RoomName;
					val6.DEPARTMENT_NAME = WorkPlaceSDO.DepartmentName;
				}
				if (list3 != null && list3.Count > 0)
				{
					V_HIS_TREATMENT_BED_ROOM bed = (from o in list3
						where o.BED_ID.HasValue
						orderby o.OUT_TIME.HasValue descending
						select o).FirstOrDefault();
					if (bed != null)
					{
						V_HIS_BED val7 = BackendDataWorker.Get<V_HIS_BED>().FirstOrDefault((V_HIS_BED o) => o.ID == bed.BED_ID);
						val6.BED_CODE = ((val7 != null) ? val7.BED_CODE : "");
						val6.BED_NAME = ((val7 != null) ? val7.BED_NAME : "");
					}
				}
				List<V_HIS_DEBATE_EKIP_USER> list4 = null;
				HisDebateEkipUserViewFilter val8 = new HisDebateEkipUserViewFilter();
				val8.DEBATE_ID = val.ID;
				list4 = ((AdapterBase)new BackendAdapter(new CommonParam())).Get<List<V_HIS_DEBATE_EKIP_USER>>("api/HisDebateEkipUser/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, (object)val8, (CommonParam)null);
				long valueOrDefault = Inventec.Common.DateTime.Get.Now().GetValueOrDefault();
				V_HIS_PATIENT_TYPE_ALTER val9 = new V_HIS_PATIENT_TYPE_ALTER();
				PrintGlobalStore.LoadCurrentPatientTypeAlter(treatment_id, valueOrDefault, ref val9);
				WaitingManager.Hide();
				Mps000387PDO val10 = new Mps000387PDO(vHisTreatment, val, departmentTran, val9, list2, list4, val6);
				PrintData val11 = null;
				string text = "";
				if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
				{
					text = GlobalVariables.dicPrinter[printTypeCode];
				}
				val11 = ((ConfigApplications.CheDoInChoCacChucNangTrongPhanMem != 2) ? new PrintData(printTypeCode, fileName, (object)val10, (MPS.ProcessorBase.PrintConfig.PreviewType)1, text) : new PrintData(printTypeCode, fileName, (object)val10, (MPS.ProcessorBase.PrintConfig.PreviewType)2, text));
				InputADO inputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode((vHisTreatment != null) ? vHisTreatment.TREATMENT_CODE : "", printTypeCode, (long?)base.currentModuleBase.RoomId);
				if (lciAutoSign.Visibility == LayoutVisibility.Always && chkAutoSign.Checked)
				{
					List<SignerConfigDTO> signerConfigs = ProcessSigner(list2);
					inputADO.SignerConfigs = signerConfigs;
				}
				val11.EmrInputADO = inputADO;
				result = MpsPrinter.Run(val11);
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}
	}
}
