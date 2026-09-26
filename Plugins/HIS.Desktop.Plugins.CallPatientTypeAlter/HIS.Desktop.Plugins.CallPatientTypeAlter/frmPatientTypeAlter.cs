using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.Controls.Session;
using HIS.Desktop.Library.CacheClient;
using HIS.Desktop.LibraryMessage;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.ConfigApplication;
using HIS.Desktop.LocalStorage.ConfigSystem;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Config;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Loader;
using HIS.Desktop.Plugins.CallPatientTypeAlter.Resources;
using HIS.Desktop.Plugins.CallPatientTypeAlter.ValidationRule;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using HIS.Desktop.Plugins.Library.EmrGenerate;
using HIS.Desktop.Plugins.Library.RegisterConfig;
using HIS.Desktop.Plugins.Library.ServiceDefaultPaty;
using HIS.Desktop.Utility;
using His.UC.UCHein;
using His.UC.UCHein.Base;
using His.UC.UCHein.Data;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.QrCodeBHYT;
using Inventec.Common.QrCodeCCCD;
using Inventec.Common.Resource;
using Inventec.Common.RichEditor;
using Inventec.Common.RichEditor.Base;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.String;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.LibraryMessage;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.Fss.Client;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein.Bhyt;
using MOS.LibraryHein.Bhyt.HeinLiveArea;
using MOS.LibraryHein.Bhyt.HeinRightRouteType;
using MOS.SDO;
using MPS;
using MPS.Processor.Mps000473.PDO;
using MPS.ProcessorBase;
using MPS.ProcessorBase.Core;
using SDA.EFMODEL.DataModels;
using SDA.Filter;

namespace HIS.Desktop.Plugins.CallPatientTypeAlter
{
	public class frmPatientTypeAlter : FormBase
	{
		private bool IsRuning = true;

		private List<HisPatientSDO> listResult = new List<HisPatientSDO>();

		public PatientTypeDepartmentADO currentTreatmentLogSDO;

		internal Module module;

		private V_HIS_TREATMENT_4 currentHisTreatment = new V_HIS_TREATMENT_4();

		private RefeshReference RefeshReference;

		private long treatmentId;

		private MainHisHeinBhyt uCMainHein;

		private UserControl ucHein__BHYT = new UserControl();

		private int ActionType;

		public Action<long> dlgSendTreatmentId;

		private int positionHandleControl = -1;

		private string provindcode;

		private HIS_PATIENT_TYPE_ALTER resultApi;

		private HisPatientTypeAlterAndTranPatiSDO resultPatientTypeAlter;

		public const int ActionAdd = 1;

		public const int ActionEdit = 2;

		public const int ActionView = 3;

		public const int ActionViewForEdit = 4;

		private List<PatientTypeDepartmentADO> lstTreatmentLog;

		private List<V_HIS_SERE_SERV_4> lstSereServResult;

		public PatientTypeDepartmentADO currentTreatmentSave;

		private UC_KskContract ucKskContract;

		private UC_ImageBHYT ucImageBHYT;

		private long patientId;

		private long keyIsSetPrimaryPatientType;

		private string HeinPatientCode;

		private long treatmentTypeId;

		private List<HIS_POSITION> dataPosition;

		private List<HIS_WORK_PLACE> dataWorkPlace;

		private List<HIS_MILITARY_RANK> dataMilitaryRank;

		private List<HIS_PATIENT_CLASSIFY> dataClassify;

		private HIS_PATIENT currenPatient;

		private HIS_PATIENT_TYPE patientType;

		private bool resultSuccess;

		private bool isEdit;

		private bool IsLoadForm;

		internal MainHisHeinBhyt mainHeinProcessor;

		private List<SDA_HIDE_CONTROL> currentHideControls;

		private string APP_CODE__EXACT = "HIS";

		private List<string> currentNameControl = new List<string>();

		private HIS_BRANCH branch;

		private string baseNameControl = "";

		private bool IsVisibleClassify;

		private List<HIS_PATIENT_TYPE> primaryPatientTypes = new List<HIS_PATIENT_TYPE>();

		private bool IsHasEmergency;

		private ControlStateWorker controlStateWorker;

		private List<ControlStateRDO> currentControlStateRDO;

		private string ModuleLinkName = "HIS.Desktop.Plugins.CallPatientTypeAlter";

		private Dictionary<long, List<V_HIS_SERVICE_PATY>> dicSevicepatyAllows = new Dictionary<long, List<V_HIS_SERVICE_PATY>>();

		private string MesError;

		private List<V_HIS_SERE_SERV_4> newLstSerSev = new List<V_HIS_SERE_SERV_4>();

		private List<HIS_SERE_SERV> lstSereServ = new List<HIS_SERE_SERV>();

		private V_HIS_ROOM currentWorkingRoom;

		private bool IsFirstLoadClassify;

		private ServiceDefaultPatyWorker serviceDefaultPatyWorker;

		private IContainer components;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private XtraScrollableControl xclHeinCardInformation;

		private SimpleButton btnSave;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private EmptySpaceItem emptySpaceItem1;

		private TextEdit txtPatientType;

		private TextEdit txtTreatmentTypeCode;

		private LayoutControlItem layoutControlItem1;

		private LayoutControlItem layoutControlItem5;

		private LookUpEdit cboPatientType;

		private LookUpEdit cboTreatmentType;

		private LayoutControlItem layoutControlItem6;

		private LayoutControlItem layoutControlItem7;

		private DXValidationProvider dxValidationProvider1;

		private DXErrorProvider dxErrorProvider1;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlTop;

		private BarManager barManager1;

		private Bar bar2;

		private BarButtonItem barButtonItem1;

		private DateEdit dtLogTime;

		private LayoutControlItem layoutControlItem2;

		private PanelControl panelControlImageBHYT;

		private LayoutControlItem layoutControlItem8;

		private TextEdit txtQrcode;

		private LayoutControlItem lciQrcode;

		private GridLookUpEdit cboPrimaryPatientType;

		private GridView gridLookUpEdit1View;

		private TextEdit txtPrimaryPatientTypeCode;

		private LayoutControlItem lciPrimaryPatientType;

		private LayoutControlItem lciComboPrimaryPatientType;

		private EmptySpaceItem emptySpaceItem3;

		private TextEdit txtWorkplace;

		private GridLookUpEdit cboWorkPlace;

		private GridView gridLookUpEdit4View;

		private GridLookUpEdit cboPosition;

		private GridView gridLookUpEdit3View;

		private GridLookUpEdit cboMilitaryRank;

		private GridView gridLookUpEdit2View;

		private GridLookUpEdit cboClassify;

		private GridView gridView1;

		private LayoutControlItem layoutControlItem9;

		private LayoutControlItem layoutControlItem10;

		private LayoutControlItem layoutControlItem11;

		private LayoutControlItem layoutControlItem12;

		private LayoutControlItem layoutControlItem13;

		private EmptySpaceItem emptySpaceItem2;

		private SimpleButton btnPrint;

		private CheckEdit chkAutoUpdateType;

		private LayoutControlItem layoutControlItem14;

		private LayoutControlItem layoutControlItem15;

		private Timer timerWaitOpenForm;

		private BarButtonItem barButtonItem2;

		private HIS_TREATMENT _HisTreatment { get; set; }

		private ResultDataADO ResultDataADO { get; set; }

		private bool? IsView { get; set; }

		private bool _IsShowLsKcb { get; set; }

		public long currentLogTime { get; set; }

		public long currentTreatmentType { get; set; }

		internal async Task CheckThongTuyen(HeinCardData dataHein)
		{
			try
			{
				LogSystem.Debug("CheckThongTuyen__" + LogUtil.TraceData(LogUtil.GetMemberName(() => dataHein), dataHein));
				if (HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.CheDoTuDongCheckThongTinTheBHYT > 0 && IsRuning)
				{
					if (string.IsNullOrEmpty(dataHein.PatientName) || string.IsNullOrEmpty(dataHein.Dob) || string.IsNullOrEmpty(dataHein.HeinCardNumber))
					{
						LogSystem.Info("Khong goi cong BHXH check thong tin the do du lieu truyen vao chua du du lieu bat buoc___" + LogUtil.TraceData(LogUtil.GetMemberName(() => dataHein), dataHein));
						return;
					}
					IsRuning = false;
					LogSystem.Debug("_Bat dau check thong tuyen_");
					ResultDataADO = await new HeinGOVManager(ResourceMessage.GoiSangCongBHXHTraVeMaLoi).Check(dataHein, null, true, (dataHein.Address != null) ? dataHein.Address : "", dtLogTime.DateTime, true);
					LogUtil.TraceData("_ResultADO_____", ResultDataADO);
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName(() => dataHein), dataHein));
					uCMainHein.SetResultDataADOBhyt(ucHein__BHYT, ResultDataADO);
					if (ResultDataADO != null && ResultDataADO.ResultHistoryLDO != null)
					{
						bool flag = true;
						string maKQ = ResultDataADO.ResultHistoryLDO.maKetQua;
						string text = "";
						if (maKQ == "000" && currenPatient != null)
						{
							HIS_PATIENT currentpatient = GetCurrentpatient(currenPatient);
							if (string.IsNullOrEmpty(currentpatient.COMMUNE_CODE) || string.IsNullOrEmpty(currentpatient.PROVINCE_CODE))
							{
								text = "Bệnh nhân thiếu thông tin địa chỉ ";
							}
							else
							{
								flag = false;
							}
						}
						string thongBao = "";
						if (maKQ == "060" || maKQ == "061" || maKQ == "070" || maKQ == "051" || maKQ == "052" || maKQ == "053" || maKQ == "050" || flag)
						{
							DateTime value = DateTimeHelper.ConvertDateStringToSystemDate(dataHein.FromDate).Value;
							DateTime dateTime = DateTimeHelper.ConvertDateStringToSystemDate(dataHein.ToDate) ?? DateTime.MinValue;
							if (maKQ == "004" && value.Date <= dtLogTime.DateTime && dateTime.Date >= dtLogTime.DateTime)
							{
								dataHein.HeinCardNumber = ResultDataADO.ResultHistoryLDO.maThe;
								dataHein.Address = ResultDataADO.ResultHistoryLDO.diaChi ?? dataHein.Address;
								dataHein.FineYearMonthDate = ResultDataADO.ResultHistoryLDO.ngayDu5Nam;
								dataHein.FromDate = ResultDataADO.ResultHistoryLDO.gtTheTu ?? dataHein.FromDate;
								dataHein.ToDate = ResultDataADO.ResultHistoryLDO.gtTheDen ?? dataHein.ToDate;
								dataHein.LiveAreaCode = ResultDataADO.ResultHistoryLDO.maKV ?? dataHein.LiveAreaCode;
								ResultDataADO.IsThongTinNguoiDungThayDoiSoVoiCong__Choose = true;
								CheckTTProcessResultData(dataHein, ResultDataADO);
							}
							if (string.IsNullOrEmpty(text))
							{
								text = ResultDataADO.ResultHistoryLDO.message;
							}
							thongBao = text + ". Bạn có muốn sửa thông tin bệnh nhân?";
							if (XtraMessageBox.Show(thongBao, "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, DefaultBoolean.True) == DialogResult.OK)
							{
								List<object> list = new List<object>();
								list.Add(_HisTreatment.PATIENT_ID);
								list.Add(treatmentId);
								list.Add(new RefeshReference(RefeshTreatment));
								PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.PatientUpdate", module.RoomId, module.RoomTypeId, list);
							}
							LogSystem.Info("CheckThongTuyen____" + LogUtil.TraceData(LogUtil.GetMemberName(() => maKQ), maKQ) + LogUtil.TraceData(LogUtil.GetMemberName(() => thongBao), thongBao));
						}
						else
						{
							if (ResultDataADO.IsShowQuestionWhileChangeHeinTime__Choose || ResultDataADO.IsThongTinNguoiDungThayDoiSoVoiCong__Choose)
							{
								dataHein.HeinCardNumber = ResultDataADO.ResultHistoryLDO.maTheMoi ?? dataHein.HeinCardNumber;
							}
							dataHein.Address = ResultDataADO.ResultHistoryLDO.diaChi ?? dataHein.Address;
							dataHein.FineYearMonthDate = ResultDataADO.ResultHistoryLDO.ngayDu5Nam;
							dataHein.FromDate = ResultDataADO.ResultHistoryLDO.gtTheTu ?? dataHein.FromDate;
							dataHein.ToDate = ResultDataADO.ResultHistoryLDO.gtTheDen ?? dataHein.ToDate;
							dataHein.LiveAreaCode = ResultDataADO.ResultHistoryLDO.maKV ?? dataHein.LiveAreaCode;
							ResultDataADO.IsThongTinNguoiDungThayDoiSoVoiCong__Choose = true;
							CheckTTProcessResultData(dataHein, ResultDataADO);
						}
					}
				}
				IsRuning = true;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private HIS_PATIENT GetCurrentpatient(HIS_PATIENT currenPatient)
		{
			HIS_PATIENT result = new HIS_PATIENT();
			try
			{
				long iD = currenPatient.ID;
				HisPatientFilter hisPatientFilter = new HisPatientFilter();
				hisPatientFilter.ID = iD;
				List<HIS_PATIENT> list = new BackendAdapter(new CommonParam()).Get<List<HIS_PATIENT>>("/api/HisPatient/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientFilter, new CommonParam());
				if (list != null)
				{
					result = list.FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void RefeshTreatment()
		{
			try
			{
				_HisTreatment = GetSDO(treatmentId).SingleOrDefault();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private V_HIS_PATIENT GetPatient(long _treatmentId)
		{
			V_HIS_PATIENT result = null;
			try
			{
				if (_treatmentId > 0)
				{
					CommonParam commonParam = new CommonParam();
					HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
					hisTreatmentFilter.ID = _treatmentId;
					List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
					if (list != null && list.Count > 0)
					{
						HisPatientViewFilter hisPatientViewFilter = new HisPatientViewFilter();
						hisPatientViewFilter.ID = list.FirstOrDefault().PATIENT_ID;
						List<V_HIS_PATIENT> list2 = new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT>>("api/HisPatient/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientViewFilter, commonParam);
						if (list2 != null && list2.Count > 0)
						{
							result = list2.FirstOrDefault();
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
			return result;
		}

		private V_HIS_PATIENT GetPatientById(long _patientId)
		{
			V_HIS_PATIENT result = null;
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientViewFilter hisPatientViewFilter = new HisPatientViewFilter();
				hisPatientViewFilter.ID = _patientId;
				List<V_HIS_PATIENT> list = new BackendAdapter(commonParam).Get<List<V_HIS_PATIENT>>("api/HisPatient/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientViewFilter, commonParam);
				if (list != null && list.Count > 0)
				{
					result = list.FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
			return result;
		}

		private List<HIS_TREATMENT> GetSDO(long treatmentId)
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
				hisTreatmentFilter.ID = treatmentId;
				return new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("/api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return null;
		}

		private async void CheckTT(HeinCardData dataHein)
		{
			try
			{
				if (_HisTreatment == null)
				{
					_HisTreatment = GetSDO(treatmentId).SingleOrDefault();
				}
				if (_HisTreatment != null)
				{
					V_HIS_PATIENT patientById = GetPatientById(_HisTreatment.PATIENT_ID);
					dataHein.Gender = ((patientById != null) ? HisToHein(patientById.GENDER_ID.ToString()) : "2");
					dataHein.PatientName = patientById.VIR_PATIENT_NAME;
					if (patientById.IS_HAS_NOT_DAY_DOB == 1)
					{
						dataHein.Dob = patientById.DOB.ToString().Substring(0, 4);
					}
					else
					{
						dataHein.Dob = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(patientById.DOB);
						string[] array = dataHein.Dob.Split(new string[1] { " " }, StringSplitOptions.None);
						dataHein.Dob = array[0];
					}
				}
				if (currentTreatmentLogSDO == null || currentTreatmentLogSDO.patientTypeAlter == null || !(currentTreatmentLogSDO.patientTypeAlter.HAS_BIRTH_CERTIFICATE == "C"))
				{
					await CheckThongTuyen(dataHein);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn("HIS.Desktop.Plugins.CallPatientTypeAlter/CheckTT:\n" + ((ex != null) ? ex.ToString() : null));
			}
		}

		internal void ChoiceTemplateHeinCard(string patientTypeCode, bool focusMoveOut)
		{
			try
			{
				ResouceManager.ResourceLanguageManager();
				LogSystem.Info("t3.1: begin process ChoiceTemplateHeinCard");
				xclHeinCardInformation.Controls.Clear();
				xclHeinCardInformation.Update();
				xclHeinCardInformation.Enabled = true;
				ucHein__BHYT = new UserControl();
				uCMainHein = new MainHisHeinBhyt();
				if (patientTypeCode == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeCode__BHYT || patientTypeCode == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeCode__QN)
				{
					emptySpaceItem3.Visibility = LayoutVisibility.Always;
					layoutControlItem8.Visibility = LayoutVisibility.Always;
					btnSave.Size = new Size(110, btnSave.Height);
					base.Size = new Size(base.Width, 410);
					panelControlImageBHYT.Controls.Clear();
					panelControlImageBHYT.Update();
					ucImageBHYT = new UC_ImageBHYT();
					ucImageBHYT.Dock = DockStyle.Fill;
					panelControlImageBHYT.Controls.Add(ucImageBHYT);
					ucImageBHYT.pictureEditImageBHYT.Tag = "NoImage";
					LogSystem.Info("t3.1.1: set default data to control hein");
					DataInitHeinBhyt dataInitHeinBhyt = new DataInitHeinBhyt();
					dataInitHeinBhyt.Template = MainHisHeinBhyt.TEMPLATE__BHYT1;
					if (currentHisTreatment.TDL_PATIENT_DOB > 0)
					{
						dataInitHeinBhyt.IsChild = BhytPatientTypeData.IsChild(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(currentHisTreatment.TDL_PATIENT_DOB).Value);
					}
					dataInitHeinBhyt.HEIN_LEVEL_CODE__CURRENT = HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT;
					dataInitHeinBhyt.HeinRightRouteTypes = HeinRightRouteTypeStore.Get();
					dataInitHeinBhyt.Icds = BackendDataWorker.Get<HIS_ICD>();
					dataInitHeinBhyt.Genders = BackendDataWorker.Get<HIS_GENDER>();
					dataInitHeinBhyt.BhytBlackLists = BackendDataWorker.Get<HIS_BHYT_BLACKLIST>();
					dataInitHeinBhyt.BhytWhiteLists = BackendDataWorker.Get<HIS_BHYT_WHITELIST>();
					dataInitHeinBhyt.LiveAreas = BackendDataWorker.Get<HeinLiveAreaData>();
					dataInitHeinBhyt.MEDI_ORG_CODE__CURRENT = HisMediOrgCFG.MEDI_ORG_VALUE__CURRENT;
					dataInitHeinBhyt.SYS_MEDI_ORG_CODE = HisMediOrgCFG.SYS_MEDI_ORG_CODE;
					dataInitHeinBhyt.MEDI_ORG_CODES__ACCEPTs = HisMediOrgCFG.MEDI_ORG_CODES__ACCEPT;
					dataInitHeinBhyt.MediOrgs = BackendDataWorker.Get<HIS_MEDI_ORG>();
					dataInitHeinBhyt.PATIENT_TYPE_ID__BHYT = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT;
					dataInitHeinBhyt.PatientTypes = BackendDataWorker.Get<HIS_PATIENT_TYPE>();
					dataInitHeinBhyt.TranPatiForms = BackendDataWorker.Get<HIS_TRAN_PATI_FORM>();
					dataInitHeinBhyt.TranPatiReasons = BackendDataWorker.Get<HIS_TRAN_PATI_REASON>();
					dataInitHeinBhyt.TREATMENT_TYPE_ID__EXAM = 1L;
					dataInitHeinBhyt.TreatmentTypes = BackendDataWorker.Get<HIS_TREATMENT_TYPE>();
					dataInitHeinBhyt.PatientTypeId = (long)(cboPatientType.EditValue ?? ((object)0));
					dataInitHeinBhyt.HeinPatientCode = HeinPatientCode;
					dataInitHeinBhyt.treatmentTypeId = treatmentTypeId;
					dataInitHeinBhyt.ActionType = ActionType;
					dataInitHeinBhyt.isVisibleControl = ConfigApplicationWorker.Get<long>("CONFIG_KEY__TIEP_DON_HIEN_THI_THONG_TIN_THEM");
					dataInitHeinBhyt.IsDefaultRightRouteType = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsDefaultRightRouteType == "1";
					dataInitHeinBhyt.IsShowCheckKhongKTHSD = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsShowCheckExpired;
					dataInitHeinBhyt.IsNotRequiredRightTypeInCaseOfHavingAreaCode = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsNotRequiredRightTypeInCaseOfHavingAreaCode;
					dataInitHeinBhyt.AutoCheckIcd = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.AutoCheckIcd;
					dataInitHeinBhyt.IsEdit = isEdit;
					dataInitHeinBhyt.CheckExamHistory = new CheckExamHistoryByHeinCardNumber(CheckTT);
					dataInitHeinBhyt.ExceedDayAllow = HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT");
					dataInitHeinBhyt.HisTreatment = currentHisTreatment;
					WorkPlaceSDO workPlaceSDO = WorkPlace.WorkPlaceSDO.Where((WorkPlaceSDO o) => o.RoomId == module.RoomId).FirstOrDefault();
					dataInitHeinBhyt.IsSampleDepartment = currentHisTreatment != null && currentHisTreatment.LAST_DEPARTMENT_ID == ((workPlaceSDO != null) ? workPlaceSDO.DepartmentId : (-1));
					if (patientTypeCode != HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeCode__BHYT && !string.IsNullOrEmpty(HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.CheckTempQN))
					{
						dataInitHeinBhyt.IsTempQN = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.CheckTempQN.Contains(patientTypeCode);
					}
					dataInitHeinBhyt.SetFocusMoveOut = new SetFocusMoveOut(FocusDelegate);
					dataInitHeinBhyt.IsObligatoryTranferMediOrg = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsObligatoryTranferMediOrg;
					dataInitHeinBhyt.ObligatoryTranferMediOrg = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.ObligatoryTranferMediOrg;
					dataInitHeinBhyt.IsAutoSelectEmergency = (currentHisTreatment != null && currentHisTreatment.IS_EMERGENCY == 1) || IsHasEmergency;
					dataInitHeinBhyt.AutoCheckCC = new DelegateAutoCheckCC(CheckCCToEnablePrint);
					dataInitHeinBhyt.DeleteTreatmentTypeId = new DeleteTreatmentTypeId(DelegateTreatmentType);
					dataInitHeinBhyt.currentModule = module;
					dataInitHeinBhyt.IsInitFromCallPatientTypeAlter = true;
					dataInitHeinBhyt.IsHideIcdDeathCauseOnly = true;
					dataInitHeinBhyt.IsWarningIcdNotRecommendMainWhenEdit = true;
					dataInitHeinBhyt.PatientId = ((currentHisTreatment != null) ? currentHisTreatment.PATIENT_ID : 0);
					dataInitHeinBhyt.SetShortcutKeyDown = new SetShortcutKeyDown(ShortcutDelegate);
					LogSystem.Info("t3.1.2: uCMainHein init");
					ucHein__BHYT = uCMainHein.InitUC(dataInitHeinBhyt, HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, null);
					ucHein__BHYT.Dock = DockStyle.Fill;
					xclHeinCardInformation.Controls.Add(ucHein__BHYT);
					LogSystem.Info("t3.1.3: set delegate");
					uCMainHein.DefaultFocusUserControl(ucHein__BHYT);
					LogSystem.Info("t3.1.4: end");
				}
				else
				{
					ucImageBHYT = null;
					emptySpaceItem3.Visibility = LayoutVisibility.Never;
					layoutControlItem8.Visibility = LayoutVisibility.Never;
					btnSave.Size = new Size(110, btnSave.Height);
					base.Size = new Size(base.Width, 100);
					xclHeinCardInformation.Controls.Clear();
					xclHeinCardInformation.Update();
					xclHeinCardInformation.Enabled = false;
					if (focusMoveOut)
					{
						btnSave.Focus();
					}
					ucHein__BHYT = null;
				}
				if (uCMainHein != null && ucHein__BHYT != null)
				{
					uCMainHein.ResetValidationControl(ucHein__BHYT);
				}
				LogSystem.Info("t3.2: end process ChoiceTemplateHeinCard");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void ReceiveTreatmentTypeId(long treatmentTypeId)
		{
			try
			{
				this.treatmentTypeId = treatmentTypeId;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void DelegateTreatmentType(long value)
		{
			try
			{
				cboTreatmentType.EditValue = value;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CheckCCToEnablePrint(bool value)
		{
			try
			{
				if (value || currentHisTreatment.IS_EMERGENCY == 1)
				{
					btnPrint.Enabled = true;
				}
				else
				{
					btnPrint.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FocusDelegate()
		{
			try
			{
				btnSave.Focus();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ShortcutDelegate(Keys key)
		{
			try
			{
				if (key == Keys.S)
				{
					btnSave.PerformClick();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataAfterFindQrCodeNoExistsCard(HeinCardData dataHein)
		{
			try
			{
				if (uCMainHein != null && ucHein__BHYT != null)
				{
					uCMainHein.FillDataAfterFindQrCode(ucHein__BHYT, dataHein);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetPatyAlterBhyt(V_HIS_PATIENT_TYPE_ALTER patyAlterBhyt, HisPatientSDO patientSdo)
		{
			try
			{
				if (patyAlterBhyt == null)
				{
					throw new ArgumentNullException("patyAlterBhyt");
				}
				if (patientSdo == null)
				{
					throw new ArgumentNullException("patientSdo");
				}
				patyAlterBhyt.ADDRESS = patientSdo.HeinAddress;
				patyAlterBhyt.HEIN_CARD_FROM_TIME = patientSdo.HeinCardFromTime.GetValueOrDefault();
				patyAlterBhyt.HEIN_CARD_TO_TIME = patientSdo.HeinCardToTime.GetValueOrDefault();
				patyAlterBhyt.HEIN_CARD_NUMBER = patientSdo.HeinCardNumber;
				patyAlterBhyt.HEIN_MEDI_ORG_CODE = patientSdo.HeinMediOrgCode;
				patyAlterBhyt.HEIN_MEDI_ORG_NAME = patientSdo.HeinMediOrgName;
				patyAlterBhyt.JOIN_5_YEAR = patientSdo.Join5Year;
				patyAlterBhyt.LIVE_AREA_CODE = patientSdo.LiveAreaCode;
				patyAlterBhyt.PAID_6_MONTH = patientSdo.Paid6Month;
				patyAlterBhyt.RIGHT_ROUTE_CODE = patientSdo.RightRouteCode;
				patyAlterBhyt.RIGHT_ROUTE_TYPE_CODE = patientSdo.RightRouteTypeCode;
				patyAlterBhyt.TDL_PATIENT_ID = patientSdo.ID;
				patyAlterBhyt.PATIENT_TYPE_ID = HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT;
				patyAlterBhyt.IS_NO_CHECK_EXPIRE = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidControl()
		{
			try
			{
				ValidTreatmentType();
				ValidationSingleControl(dtLogTime);
				layoutControlItem9.Visibility = LayoutVisibility.Always;
				ValidationSingleControl(cboClassify);
				if (currentNameControl != null && currentNameControl.Count > 0)
				{
					string item = layoutControl1.Name + ".Root." + layoutControlItem9.Name;
					if (currentNameControl.Contains(item))
					{
						IsVisibleClassify = true;
						layoutControlItem9.Visibility = LayoutVisibility.Never;
						dxValidationProvider1.SetValidationRule(cboClassify, null);
						emptySpaceItem2.Visibility = LayoutVisibility.Never;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidationSingleControl(BaseEdit control)
		{
			try
			{
				ControlEditValidationRule controlEditValidationRule = new ControlEditValidationRule();
				controlEditValidationRule.editor = control;
				controlEditValidationRule.ErrorText = string.Format(HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc));
				controlEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(control, controlEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidTreatmentType()
		{
			try
			{
				TreatmentTypeValidationRule treatmentTypeValidationRule = new TreatmentTypeValidationRule();
				treatmentTypeValidationRule.txtTreatmentTypeCode = txtTreatmentTypeCode;
				treatmentTypeValidationRule.cboTreatmentType = cboTreatmentType;
				treatmentTypeValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				treatmentTypeValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtTreatmentTypeCode, treatmentTypeValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidWorkPlace()
		{
			try
			{
				WorkPlaceValidationRule workPlaceValidationRule = new WorkPlaceValidationRule();
				workPlaceValidationRule.txt = txtWorkplace;
				workPlaceValidationRule.cbo = cboWorkPlace;
				workPlaceValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				workPlaceValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(cboWorkPlace, workPlaceValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidPrimaryPatientTypeCode()
		{
			try
			{
				PrimaryPatientTypeCodeValidate primaryPatientTypeCodeValidate = new PrimaryPatientTypeCodeValidate();
				primaryPatientTypeCodeValidate.txtPrimaryPatientTypeCode = txtPrimaryPatientTypeCode;
				primaryPatientTypeCodeValidate.cboPrimaryPatientTypeCode = cboPrimaryPatientType;
				primaryPatientTypeCodeValidate.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				primaryPatientTypeCodeValidate.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtPrimaryPatientTypeCode, primaryPatientTypeCodeValidate);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidDoiTuong()
		{
			try
			{
				PatientTypeValidationRule patientTypeValidationRule = new PatientTypeValidationRule();
				patientTypeValidationRule.txtMaDoiTuong = txtPatientType;
				patientTypeValidationRule.cboDoiTuong = cboPatientType;
				patientTypeValidationRule.ErrorText = HIS.Desktop.LibraryMessage.MessageUtil.GetMessage(HIS.Desktop.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				patientTypeValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtPatientType, patientTypeValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dxValidationProvider1_ValidationFailed(object sender, ValidationFailedEventArgs e)
		{
			try
			{
				BaseEdit baseEdit = e.InvalidControl as BaseEdit;
				if (baseEdit == null || !(baseEdit.GetViewInfo() is BaseEditViewInfo))
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

		public frmPatientTypeAlter(Module _module, PatientTypeDepartmentADO _HisTreatmentLogSDO, bool? _isView, List<PatientTypeDepartmentADO> _lstTreatmentLog, RefeshReference _RefeshReference)
			: base(_module)
		{
			InitializeComponent();
			module = _module;
			isEdit = true;
			currentTreatmentLogSDO = _HisTreatmentLogSDO;
			IsView = _isView;
			lstTreatmentLog = _lstTreatmentLog;
			RefeshReference = _RefeshReference;
			if (currentTreatmentLogSDO != null)
			{
				treatmentId = currentTreatmentLogSDO.patientTypeAlter.TREATMENT_ID;
			}
		}

		public frmPatientTypeAlter(Module _module, long _treatmentId, bool? _isView, List<PatientTypeDepartmentADO> _lstTreatmentLog, RefeshReference _RefeshReference)
			: base(_module)
		{
			InitializeComponent();
			module = _module;
			treatmentId = _treatmentId;
			IsView = _isView;
			lstTreatmentLog = _lstTreatmentLog;
			RefeshReference = _RefeshReference;
		}

		private void frmPatientTypeAlter_Load(object sender, EventArgs e)
		{
			try
			{
				keyIsSetPrimaryPatientType = System.Convert.ToInt16(HisConfigs.Get<string>("MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE"));
				if (module != null)
				{
					currentWorkingRoom = BackendDataWorker.Get<V_HIS_ROOM>().FirstOrDefault((V_HIS_ROOM o) => o.ID == module.RoomId);
				}
				IsLoadForm = true;
				IsFirstLoadClassify = true;
				SetIcon();
				btnSave.Enabled = IsView ?? true;
				txtPatientType.Enabled = IsView ?? true;
				cboPatientType.Enabled = IsView ?? true;
				txtTreatmentTypeCode.Enabled = IsView ?? true;
				cboTreatmentType.Enabled = IsView ?? true;
				dtLogTime.Enabled = IsView ?? true;
				WaitingManager.Show();
				GetDataVisibleControlFromPatientClassify();
				ValidControl();
				InitCombo();
				VisibleControl();
				dtLogTime.Properties.VistaDisplayMode = DefaultBoolean.True;
				dtLogTime.Properties.VistaEditTime = DefaultBoolean.True;
				HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.LoadConfig();
				HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.LoadConfig();
				VisiblePrimaryPatientType();
				AppConfigs.LoadConfig();
				LoadCurrentHisTreatment();
				SetCaptionByLanguageKey();
				if (currentTreatmentLogSDO != null)
				{
					ActionType = 2;
					FillDataPatientTypeAlterIntoForm(ref patientType);
				}
				else
				{
					ActionType = 1;
					dtLogTime.DateTime = DateTime.Now;
					dtLogTime.Update();
					txtTreatmentTypeCode.Text = "";
					cboTreatmentType.EditValue = null;
					GetPatientTypeDefault(ref patientType);
				}
				WaitingManager.Hide();
				InitPatientTypeInfo(patientType);
				cboTreatmentType_EditValueChanged(null, null);
				InitDataCombo();
				currentTreatmentSave = currentTreatmentLogSDO;
				dtLogTime.Focus();
				dtLogTime.SelectAll();
				_IsShowLsKcb = false;
				HIS.Desktop.Plugins.CallPatientTypeAlter.Config.BHXHLoginCFG.LoadConfig();
				HIS.Desktop.Plugins.Library.RegisterConfig.BHXHLoginCFG.LoadConfig();
				LoadCurrentPatient();
				InitControlState();
				LoadServiceReq();
				EnableBtnPrint();
				ResultDataADO = new ResultDataADO();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void SetCaptionByLanguageKey()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Lang", typeof(frmPatientTypeAlter).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControl1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				btnPrint.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.btnPrint.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				btnPrint.ToolTip = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.btnPrint.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkAutoUpdateType.Properties.Caption = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.chkAutoUpdateType.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkAutoUpdateType.ToolTip = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.chkAutoUpdateType.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				bar2.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.bar2.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				barButtonItem1.Caption = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.barButtonItem1.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				txtWorkplace.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.txtWorkplace.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboWorkPlace.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboWorkPlace.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboPosition.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboPosition.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboMilitaryRank.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboMilitaryRank.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboClassify.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboClassify.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboPrimaryPatientType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboPrimaryPatientType.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboPatientType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboPatientType.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboTreatmentType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboTreatmentType.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.btnSave.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				txtQrcode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.txtQrcode.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem2.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem2.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciQrcode.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.lciQrcode.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciPrimaryPatientType.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.lciPrimaryPatientType.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem9.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem9.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem9.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem9.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem10.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem10.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem11.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem11.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem12.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem12.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void GetDataVisibleControlFromPatientClassify()
		{
			try
			{
				List<HIS_BRANCH> list = BackendDataWorker.Get<HIS_BRANCH>();
				branch = ((list != null && list.Count > 0) ? list.FirstOrDefault((HIS_BRANCH o) => o.ID == BranchWorker.GetCurrentBranchId()) : null);
				CommonParam commonParam = new CommonParam();
				SdaHideControlFilter sdaHideControlFilter = new SdaHideControlFilter();
				sdaHideControlFilter.MODULE_LINK__EXACT = ModuleLinkName;
				sdaHideControlFilter.APP_CODE__EXACT = APP_CODE__EXACT;
				currentHideControls = new BackendAdapter(commonParam).Get<List<SDA_HIDE_CONTROL>>("api/sdaHideControl/Get", HIS.Desktop.ApiConsumer.ApiConsumers.SdaConsumer, sdaHideControlFilter, commonParam);
				currentHideControls = ((currentHideControls != null && currentHideControls.Count > 0) ? currentHideControls.Where((SDA_HIDE_CONTROL o) => o.BRANCH_CODE == null || (branch != null && o.BRANCH_CODE == branch.BRANCH_CODE)).ToList() : null);
				if (currentHideControls != null && currentHideControls.Count > 0)
				{
					currentNameControl = currentHideControls.Select((SDA_HIDE_CONTROL o) => o.CONTROL_PATH).ToList();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadCurrentPatient()
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientFilter hisPatientFilter = new HisPatientFilter();
				hisPatientFilter.PATIENT_CODE = currentHisTreatment.TDL_PATIENT_CODE;
				currenPatient = new BackendAdapter(commonParam).Get<List<HIS_PATIENT>>("api/HisPatient/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientFilter, commonParam).ToList().First();
				if (currentHisTreatment != null && !IsVisibleClassify && dataClassify.FirstOrDefault((HIS_PATIENT_CLASSIFY o) => o.ID == currenPatient.PATIENT_CLASSIFY_ID) != null)
				{
					cboClassify.EditValue = currenPatient.PATIENT_CLASSIFY_ID;
					if (layoutControlItem10.Visibility == LayoutVisibility.Always)
					{
						cboMilitaryRank.EditValue = currenPatient.MILITARY_RANK_ID;
						cboPosition.EditValue = currenPatient.POSITION_ID;
						cboWorkPlace.EditValue = currenPatient.WORK_PLACE_ID;
						txtWorkplace.Text = currenPatient.WORK_PLACE;
					}
				}
				ReSizeForm();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadServiceReq()
		{
			try
			{
				if (currentHisTreatment == null)
				{
					return;
				}
				CommonParam commonParam = new CommonParam();
				HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
				hisServiceReqFilter.TREATMENT_ID = currentHisTreatment.ID;
				List<HIS_SERVICE_REQ> list = new BackendAdapter(commonParam).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, commonParam);
				if (list != null && list.Count > 0)
				{
					IsHasEmergency = list.Where((HIS_SERVICE_REQ o) => o.IS_EMERGENCY == 1).ToList() != null && list.Where((HIS_SERVICE_REQ o) => o.IS_EMERGENCY == 1).ToList().Count > 0;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ReSizeForm()
		{
			try
			{
				if (emptySpaceItem3.Visibility == LayoutVisibility.Never)
				{
					base.Size = new Size(1300, 120);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void VisibleControl()
		{
			try
			{
				if (IsVisibleClassify)
				{
					layoutControlItem10.Visibility = LayoutVisibility.Never;
					layoutControlItem11.Visibility = LayoutVisibility.Never;
					layoutControlItem12.Visibility = LayoutVisibility.Never;
					layoutControlItem13.Visibility = LayoutVisibility.Never;
					emptySpaceItem2.Visibility = LayoutVisibility.Never;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void EnableControlCombo(bool IsEnable)
		{
			try
			{
				layoutControlItem10.Visibility = ((!IsEnable) ? LayoutVisibility.Never : LayoutVisibility.Always);
				layoutControlItem11.Visibility = ((!IsEnable) ? LayoutVisibility.Never : LayoutVisibility.Always);
				layoutControlItem12.Visibility = ((!IsEnable) ? LayoutVisibility.Never : LayoutVisibility.Always);
				layoutControlItem13.Visibility = ((!IsEnable) ? LayoutVisibility.Never : LayoutVisibility.Always);
				emptySpaceItem2.Visibility = (IsEnable ? LayoutVisibility.Never : LayoutVisibility.Always);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitDataCombo()
		{
			try
			{
				LoadPatientClassify();
				LoadMilitaryRank();
				LoadPosition();
				LoadWorkPlace();
				InitComboCommon(cboClassify, dataClassify, "ID", "PATIENT_CLASSIFY_NAME", "PATIENT_CLASSIFY_CODE");
				InitComboCommon(cboMilitaryRank, dataMilitaryRank, "ID", "MILITARY_RANK_NAME", "MILITARY_RANK_CODE");
				InitComboCommon(cboPosition, dataPosition, "ID", "POSITION_NAME", "POSITION_CODE");
				InitComboCommon(cboWorkPlace, dataWorkPlace, "ID", "WORK_PLACE_NAME", "WORK_PLACE_CODE");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadPatientClassify()
		{
			try
			{
				if (BackendDataWorker.IsExistsKey<HIS_PATIENT_CLASSIFY>())
				{
					LogSystem.Error("1__________________");
					dataClassify = BackendDataWorker.Get<HIS_PATIENT_CLASSIFY>();
				}
				else
				{
					LogSystem.Error("2__________________");
					CommonParam commonParam = new CommonParam();
					dynamic val = new ExpandoObject();
					dataClassify = new BackendAdapter(commonParam).Get<List<HIS_PATIENT_CLASSIFY>>("api/HisPatientClassify/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, val, commonParam);
					if (dataClassify != null)
					{
						BackendDataWorker.UpdateToRam(typeof(HIS_PATIENT_CLASSIFY), dataClassify, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
					}
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => dataClassify), dataClassify));
				if (dataClassify != null && dataClassify.Count > 0)
				{
					if (cboPatientType.EditValue != null)
					{
						dataClassify = dataClassify.Where((HIS_PATIENT_CLASSIFY o) => o.IS_ACTIVE == 1 && (!o.PATIENT_TYPE_ID.HasValue || o.PATIENT_TYPE_ID == long.Parse(cboPatientType.EditValue.ToString()))).ToList();
					}
					else
					{
						dataClassify = dataClassify.Where((HIS_PATIENT_CLASSIFY o) => o.IS_ACTIVE == 1 && !o.PATIENT_TYPE_ID.HasValue).ToList();
					}
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => dataClassify), dataClassify));
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadMilitaryRank()
		{
			try
			{
				if (BackendDataWorker.IsExistsKey<HIS_PATIENT_CLASSIFY>())
				{
					dataMilitaryRank = BackendDataWorker.Get<HIS_MILITARY_RANK>();
				}
				else
				{
					CommonParam commonParam = new CommonParam();
					dynamic val = new ExpandoObject();
					dataMilitaryRank = new BackendAdapter(commonParam).Get<List<HIS_MILITARY_RANK>>("api/HisMilitaryRank/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, val, commonParam);
					if (dataMilitaryRank != null)
					{
						BackendDataWorker.UpdateToRam(typeof(HIS_MILITARY_RANK), dataMilitaryRank, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
					}
				}
				if (dataMilitaryRank != null && dataMilitaryRank.Count > 0)
				{
					dataMilitaryRank = dataMilitaryRank.Where((HIS_MILITARY_RANK o) => o.IS_ACTIVE == 1).ToList();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadPosition()
		{
			try
			{
				if (BackendDataWorker.IsExistsKey<HIS_POSITION>())
				{
					dataPosition = BackendDataWorker.Get<HIS_POSITION>();
				}
				else
				{
					CommonParam commonParam = new CommonParam();
					dynamic val = new ExpandoObject();
					dataPosition = new BackendAdapter(commonParam).Get<List<HIS_POSITION>>("api/HisPosition/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, val, commonParam);
					if (dataPosition != null)
					{
						BackendDataWorker.UpdateToRam(typeof(HIS_POSITION), dataPosition, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
					}
				}
				if (dataPosition != null && dataPosition.Count > 0)
				{
					dataPosition = dataPosition.Where((HIS_POSITION o) => o.IS_ACTIVE == 1).ToList();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadWorkPlace()
		{
			try
			{
				if (BackendDataWorker.IsExistsKey<HIS_WORK_PLACE>())
				{
					dataWorkPlace = BackendDataWorker.Get<HIS_WORK_PLACE>();
				}
				else
				{
					CommonParam commonParam = new CommonParam();
					dynamic val = new ExpandoObject();
					dataWorkPlace = new BackendAdapter(commonParam).Get<List<HIS_WORK_PLACE>>("api/HisWorkPlace/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, val, commonParam);
					if (dataWorkPlace != null)
					{
						BackendDataWorker.UpdateToRam(typeof(HIS_WORK_PLACE), dataWorkPlace, long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")));
					}
				}
				if (dataWorkPlace != null && dataWorkPlace.Count > 0)
				{
					dataWorkPlace = dataWorkPlace.Where((HIS_WORK_PLACE o) => o.IS_ACTIVE == 1).ToList();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal static void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, string displayMemberCode)
		{
			try
			{
				InitComboCommon(cboEditor, data, valueMember, displayMember, 0, displayMemberCode, 0);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal static void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, int displayMemberWidth, string displayMemberCode, int displayMemberCodeWidth)
		{
			try
			{
				int num = 0;
				List<ColumnInfo> list = new List<ColumnInfo>();
				if (!string.IsNullOrEmpty(displayMemberCode))
				{
					list.Add(new ColumnInfo(displayMemberCode, "", (displayMemberCodeWidth > 0) ? displayMemberCodeWidth : 100, 1));
					num += ((displayMemberCodeWidth > 0) ? displayMemberCodeWidth : 100);
				}
				if (!string.IsNullOrEmpty(displayMember))
				{
					list.Add(new ColumnInfo(displayMember, "", (displayMemberWidth > 0) ? displayMemberWidth : 250, 2));
					num += ((displayMemberWidth > 0) ? displayMemberWidth : 250);
				}
				ControlEditorADO controlEditorADO = new ControlEditorADO(displayMember, valueMember, list, false, num);
				ControlEditorLoader.Load(cboEditor, data, controlEditorADO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void VisiblePrimaryPatientType()
		{
			try
			{
				if (HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsSetPrimaryPatientType == "2")
				{
					lciPrimaryPatientType.Visibility = LayoutVisibility.Always;
					lciComboPrimaryPatientType.Visibility = LayoutVisibility.Always;
				}
				else
				{
					lciPrimaryPatientType.Visibility = LayoutVisibility.Never;
					lciComboPrimaryPatientType.Visibility = LayoutVisibility.Never;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void InitCombo()
		{
			try
			{
				PatientTypeLoader.LoadDataToCombo(cboPatientType, (from p in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
					where p.IS_ACTIVE == 1 && p.IS_NOT_USE_FOR_PATIENT != 1
					select p).ToList());
				LoadComboTreatmentType(null);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadComboTreatmentType(long? keepTreatmentTypeId)
		{
			try
			{
				List<HIS_TREATMENT_TYPE> list = BackendDataWorker.Get<HIS_TREATMENT_TYPE>();
				if (list == null)
				{
					return;
				}
				List<HIS_TREATMENT_TYPE> list2 = list.Where((HIS_TREATMENT_TYPE o) => o.IS_ACTIVE == 1).ToList();
				if (keepTreatmentTypeId.HasValue && !list2.Any((HIS_TREATMENT_TYPE o) => o.ID == keepTreatmentTypeId.Value))
				{
					HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = list.FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == keepTreatmentTypeId.Value);
					if (hIS_TREATMENT_TYPE != null)
					{
						list2.Add(hIS_TREATMENT_TYPE);
					}
				}
				TreatmentTypeLoader.LoadDataToComboTreatmentType(cboTreatmentType, list2);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitPatientTypeInfo(HIS_PATIENT_TYPE patientType)
		{
			try
			{
				if (patientType == null)
				{
					return;
				}
				lciQrcode.Enabled = false;
				cboPatientType.EditValue = patientType.ID;
				txtPatientType.Text = patientType.PATIENT_TYPE_CODE;
				if (patientType.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__KSK)
				{
					xclHeinCardInformation.Controls.Clear();
					xclHeinCardInformation.Update();
					xclHeinCardInformation.Enabled = true;
					emptySpaceItem3.Visibility = LayoutVisibility.Never;
					layoutControlItem8.Visibility = LayoutVisibility.Never;
					btnSave.Size = new Size(110, btnSave.Height);
					base.Size = new Size(base.Width, 130);
					ucKskContract = new UC_KskContract();
					ucKskContract.cboContract.EditValue = currentTreatmentLogSDO.patientTypeAlter.KSK_CONTRACT_ID;
					ucKskContract.Dock = DockStyle.Fill;
					xclHeinCardInformation.Controls.Add(ucKskContract);
					if (ucKskContract != null && ucKskContract.cboContract.EditValue != null)
					{
						V_HIS_KSK_CONTRACT v_HIS_KSK_CONTRACT = ucKskContract.listKskContract.FirstOrDefault((V_HIS_KSK_CONTRACT o) => o.ID == Parse.ToInt64(ucKskContract.cboContract.EditValue.ToString()));
						if (v_HIS_KSK_CONTRACT != null)
						{
							ucKskContract.lblNgayHetHan.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(v_HIS_KSK_CONTRACT.EXPIRY_DATE.GetValueOrDefault());
							ucKskContract.lblNgayHieuLuc.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(v_HIS_KSK_CONTRACT.EFFECT_DATE.GetValueOrDefault());
							ucKskContract.lblTenCongTy.Text = v_HIS_KSK_CONTRACT.WORK_PLACE_NAME;
							ucKskContract.lblTyLeThanhToan.Text = System.Convert.ToInt64(v_HIS_KSK_CONTRACT.PAYMENT_RATIO * 100m) + "%";
						}
					}
				}
				else
				{
					ChoiceTemplateHeinCard(patientType.PATIENT_TYPE_CODE, false);
					if (patientType.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
					{
						lciQrcode.Enabled = true;
					}
				}
				if (ActionType == 2)
				{
					HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = new HIS_PATIENT_TYPE_ALTER();
					hIS_PATIENT_TYPE_ALTER.PATIENT_TYPE_ID = patientType.ID;
					hIS_PATIENT_TYPE_ALTER.ID = currentTreatmentLogSDO.patientTypeAlter.ID;
					hIS_PATIENT_TYPE_ALTER.TDL_PATIENT_ID = currentTreatmentLogSDO.patientTypeAlter.TDL_PATIENT_ID;
					Mapper.CreateMap<V_HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>();
					hIS_PATIENT_TYPE_ALTER = Mapper.Map<V_HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>(currentTreatmentLogSDO.patientTypeAlter);
					if (ucHein__BHYT != null && uCMainHein != null)
					{
						uCMainHein.FillDataHeinInsuranceInfoByPatientTypeAlter(ucHein__BHYT, hIS_PATIENT_TYPE_ALTER);
						uCMainHein.InitOldPatientData(ucHein__BHYT, currentTreatmentLogSDO.patientTypeAlter.TDL_PATIENT_ID, hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER);
						uCMainHein.FillDataTranPatiInForm(ucHein__BHYT, currentHisTreatment.ID);
					}
					if (currentTreatmentLogSDO.patientTypeAlter != null && !string.IsNullOrEmpty(currentTreatmentLogSDO.patientTypeAlter.BHYT_URL))
					{
						try
						{
							ucImageBHYT.pictureEditImageBHYT.Image = Image.FromStream(FileDownload.GetFile(currentTreatmentLogSDO.patientTypeAlter.BHYT_URL));
						}
						catch (Exception ex)
						{
							LogSystem.Warn(ex);
							ucImageBHYT.SetImageDefaultForPictureEdit(null);
						}
					}
				}
				LoadPrimaryPatientType();
				if (currentTreatmentLogSDO != null && currentTreatmentLogSDO.patientTypeAlter.PRIMARY_PATIENT_TYPE_ID.HasValue && currentTreatmentLogSDO.patientTypeAlter != null)
				{
					cboPrimaryPatientType.EditValue = currentTreatmentLogSDO.patientTypeAlter.PRIMARY_PATIENT_TYPE_ID;
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void GetPatientTypeDefault(ref HIS_PATIENT_TYPE patientType)
		{
			try
			{
				if (!string.IsNullOrEmpty(ConfigApplicationWorker.Get<string>("CONFIG_KEY__DEFAULT_CONFIG_PATIENT_TYPE_CODE")))
				{
					patientType = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE == ConfigApplicationWorker.Get<string>("CONFIG_KEY__DEFAULT_CONFIG_PATIENT_TYPE_CODE"));
					if (patientType == null)
					{
						LogSystem.Warn("Phan mem RAE da duoc cau hinh doi tuong benh nhan mac dinh, tuy nhien ma doi tuong cau hinh khong ton tai trong danh muc doi tuong benh nhan, he thong tu dong lay doi tuong mac dinh la doi tuong BHYT. " + LogUtil.TraceData(LogUtil.GetMemberName(() => ConfigApplicationWorker.Get<string>("CONFIG_KEY__DEFAULT_CONFIG_PATIENT_TYPE_CODE")), "CONFIG_KEY__DEFAULT_CONFIG_PATIENT_TYPE_CODE"));
						patientType = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT);
					}
				}
				else
				{
					patientType = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadKeyLanguage()
		{
			try
			{
				ResourceLanguageManager.LanguageResource = new ResourceManager("HIS.Desktop.Plugins.CallPatientTypeAlter.Resources.Lang", typeof(frmPatientTypeAlter).Assembly);
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControl1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboPatientType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboPatientType.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboTreatmentType.Properties.NullText = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.cboTreatmentType.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				btnSave.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.btnSave.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem1.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControlItem5.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.layoutControlItem5.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				bar2.Text = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.bar2.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				barButtonItem1.Caption = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.barButtonItem1.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				txtQrcode.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("frmPatientTypeAlter.txtQrcode.NullValuePrompt", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				if (module != null)
				{
					Text = module.text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadCurrentHisTreatment()
		{
			try
			{
				if (module != null)
				{
					CommonParam commonParam = new CommonParam();
					HisTreatmentView4Filter hisTreatmentView4Filter = new HisTreatmentView4Filter();
					hisTreatmentView4Filter.ID = treatmentId;
					currentHisTreatment = new BackendAdapter(commonParam).Get<List<V_HIS_TREATMENT_4>>("api/HisTreatment/GetView4", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentView4Filter, commonParam).ToList().First();
					if (currentHisTreatment != null)
					{
						HeinPatientCode = ((currentHisTreatment.HEIN_PATIENT_TYPE_CODE != null) ? currentHisTreatment.HEIN_PATIENT_TYPE_CODE : null);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataPatientTypeAlterIntoForm(ref HIS_PATIENT_TYPE patientType)
		{
			try
			{
				dtLogTime.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(currentTreatmentLogSDO.LOG_TIME) ?? DateTime.Now;
				dtLogTime.Update();
				dtLogTime.Visible = false;
				if (currentTreatmentLogSDO.patientTypeAlter == null)
				{
					return;
				}
				patientType = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == currentTreatmentLogSDO.patientTypeAlter.PATIENT_TYPE_ID);
				if (patientType == null)
				{
					LogSystem.Debug("Khong lay duoc doi tuong benh nhan theo PATIENT_TYPE_ID. " + LogUtil.TraceData(LogUtil.GetMemberName(() => currentTreatmentLogSDO.patientTypeAlter.PATIENT_TYPE_ID), currentTreatmentLogSDO.patientTypeAlter.PATIENT_TYPE_ID));
				}
				txtTreatmentTypeCode.Text = currentTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_CODE;
				LoadComboTreatmentType(currentTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID);
				cboTreatmentType.EditValue = currentTreatmentLogSDO.patientTypeAlter.TREATMENT_TYPE_ID;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTreatmentType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					txtPatientType.Focus();
					txtPatientType.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadDataToGridSereServ(PatientTypeDepartmentADO data, List<PatientTypeDepartmentADO> listTL)
		{
			try
			{
				WaitingManager.Show();
				if (data != null && data.type == 1 && listTL != null && listTL.Count > 0)
				{
					listTL = listTL.OrderBy((PatientTypeDepartmentADO o) => o.LOG_TIME).ToList();
					PatientTypeDepartmentADO logtime = new PatientTypeDepartmentADO();
					for (int num = 0; num < listTL.Count; num++)
					{
						if (listTL[num].type == 1 && listTL[num].LOG_TIME != data.LOG_TIME && listTL[num].LOG_TIME > data.LOG_TIME)
						{
							logtime = listTL[num];
							break;
						}
					}
					CommonParam commonParam = new CommonParam();
					HisSereServBillFilter hisSereServBillFilter = new HisSereServBillFilter();
					hisSereServBillFilter.TDL_TREATMENT_ID = data.TREATMENT_ID;
					hisSereServBillFilter.IS_NOT_CANCEL = true;
					List<HIS_SERE_SERV_BILL> lstSereServBill = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_BILL>>("api/HisSereServBill/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServBillFilter, commonParam);
					HisSereServView4Filter hisSereServView4Filter = new HisSereServView4Filter();
					hisSereServView4Filter.TREATMENT_ID = data.TREATMENT_ID;
					List<V_HIS_SERE_SERV_4> list = new List<V_HIS_SERE_SERV_4>();
					list = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_4>>("api/HisSereServ/GetView4", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView4Filter, commonParam);
					if (list != null && list.Count > 0)
					{
						if (lstSereServBill != null && lstSereServBill.Count > 0)
						{
							list = list.Where((V_HIS_SERE_SERV_4 o) => !lstSereServBill.Select((HIS_SERE_SERV_BILL p) => p.SERE_SERV_ID).Contains(o.ID)).ToList();
						}
						lstSereServResult = new List<V_HIS_SERE_SERV_4>();
						if (list != null && list.Count > 0)
						{
							if (logtime != null)
							{
								if (logtime.LOG_TIME > 0)
								{
									lstSereServResult = list.Where((V_HIS_SERE_SERV_4 o) => o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && data.LOG_TIME <= o.TDL_INTRUCTION_TIME && o.TDL_INTRUCTION_TIME <= logtime.LOG_TIME).ToList();
								}
								else
								{
									lstSereServResult = list.Where((V_HIS_SERE_SERV_4 o) => o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && data.LOG_TIME <= o.TDL_INTRUCTION_TIME).ToList();
								}
							}
							else
							{
								lstSereServResult = list.Where((V_HIS_SERE_SERV_4 o) => o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && data.LOG_TIME <= o.TDL_INTRUCTION_TIME).ToList();
							}
						}
						if (lstSereServResult.Count > 0)
						{
							data.patientTypeAlter.TREATMENT_TYPE_ID = Parse.ToInt64((cboTreatmentType.EditValue ?? "0").ToString());
							data.patientTypeAlter.TREATMENT_TYPE_CODE = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == data.patientTypeAlter.TREATMENT_TYPE_ID).TREATMENT_TYPE_CODE;
							data.patientTypeAlter.TREATMENT_TYPE_NAME = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == data.patientTypeAlter.TREATMENT_TYPE_ID).TREATMENT_TYPE_NAME;
							HisPatientProfileSDO hisPatientProfileSDO = new HisPatientProfileSDO();
							if (uCMainHein != null && ucHein__BHYT != null)
							{
								uCMainHein.UpdateDataFormIntoPatientTypeAlter(ucHein__BHYT, hisPatientProfileSDO);
								data.patientTypeAlter.HEIN_CARD_FROM_TIME = hisPatientProfileSDO.HisPatientTypeAlter.HEIN_CARD_FROM_TIME;
								data.patientTypeAlter.HEIN_CARD_TO_TIME = hisPatientProfileSDO.HisPatientTypeAlter.HEIN_CARD_TO_TIME;
							}
							long? patient_primary_patient_type_id = null;
							if (cboPrimaryPatientType.EditValue != null && !string.IsNullOrEmpty(cboPrimaryPatientType.EditValue.ToString()))
							{
								patient_primary_patient_type_id = Parse.ToInt64(cboPrimaryPatientType.EditValue.ToString());
							}
							long? patient_classify_id = null;
							if (cboClassify.EditValue != null && !string.IsNullOrEmpty(cboClassify.EditValue.ToString()))
							{
								patient_classify_id = Parse.ToInt64(cboClassify.EditValue.ToString());
							}
							new frmSwapPatientTypeAlter(module, Parse.ToInt64((cboPatientType.EditValue ?? "").ToString()), patient_primary_patient_type_id, patient_classify_id, data, listTL, lstSereServResult, new DelegateReturnSuccess(DelegateSuccess)).ShowDialog();
						}
					}
				}
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		public void DelegateSuccess(bool success)
		{
			try
			{
				if (success)
				{
					resultSuccess = success;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					lciQrcode.Enabled = false;
					if (Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
					{
						uCMainHein.DefaultFocusUserControl(ucHein__BHYT);
						lciQrcode.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void PrepareForm(bool? isView)
		{
			try
			{
				if (isView.HasValue)
				{
					TextEdit textEdit = txtTreatmentTypeCode;
					LookUpEdit lookUpEdit = cboTreatmentType;
					TextEdit textEdit2 = txtPatientType;
					LookUpEdit lookUpEdit2 = cboPatientType;
					bool flag = (xclHeinCardInformation.Enabled = !isView.Value);
					bool flag3 = (lookUpEdit2.Enabled = flag);
					bool flag5 = (textEdit2.Enabled = flag3);
					bool enabled = (lookUpEdit.Enabled = flag5);
					textEdit.Enabled = enabled;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			CommonParam param = new CommonParam();
			bool check = false;
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			try
			{
				positionHandleControl = -1;
				if (!dxValidationProvider1.Validate())
				{
					IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
					for (int num = invalidControls.Count - 1; num >= 0; num--)
					{
						LogSystem.Debug(((num == 0) ? "InvalidControls:" : "") + invalidControls[num].Name + ",");
					}
					flag3 = false;
				}
				MesError = null;
				newLstSerSev = new List<V_HIS_SERE_SERV_4>();
				if (currentTreatmentLogSDO != null)
				{
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == currentTreatmentLogSDO.patientTypeAlter.PATIENT_TYPE_ID);
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE2 = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()));
					if (hIS_PATIENT_TYPE != null && hIS_PATIENT_TYPE2 != null && hIS_PATIENT_TYPE.IS_COPAYMENT != 1 && hIS_PATIENT_TYPE2.IS_COPAYMENT == 1)
					{
						check = true;
					}
				}
				if (uCMainHein != null && ucHein__BHYT != null && (Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT || Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__QN))
				{
					flag = uCMainHein.GetInvalidControls(ucHein__BHYT);
				}
				flag = flag && AlertExpriedTimeHeinCardBhyt();
				flag4 = BlockingInvalidBhyt();
				if (flag2 && flag && flag3 && flag4)
				{
					SaveProcess(param, check);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Fatal(ex);
			}
		}

		private bool BlockingInvalidBhyt()
		{
			try
			{
				HisPatientProfileSDO dataPatientProfile = new HisPatientProfileSDO();
				dataPatientProfile.HisPatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				if (uCMainHein != null)
				{
					uCMainHein.UpdateDataFormIntoPatientTypeAlter(ucHein__BHYT, dataPatientProfile);
				}
				if (cboPatientType.EditValue != null && Parse.ToInt64((cboPatientType.EditValue ?? "0").ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
				{
					if (dataPatientProfile != null && !string.IsNullOrEmpty(dataPatientProfile.HisPatientTypeAlter.HEIN_MEDI_ORG_CODE))
					{
						HIS_MEDI_ORG hIS_MEDI_ORG = BackendDataWorker.Get<HIS_MEDI_ORG>().FirstOrDefault((HIS_MEDI_ORG o) => o.MEDI_ORG_CODE == dataPatientProfile.HisPatientTypeAlter.HEIN_MEDI_ORG_CODE);
						if (!string.IsNullOrEmpty(BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE) && hIS_MEDI_ORG != null && (";" + BranchDataWorker.Branch.DO_NOT_ALLOW_HEIN_LEVEL_CODE + ";").Contains(";" + hIS_MEDI_ORG.LEVEL_CODE + ";"))
						{
							XtraMessageBox.Show(string.Format("Nơi đăng ký khám chữa bệnh ban đầu thuộc tuyến {0}, không được hưởng BHYT", (hIS_MEDI_ORG.LEVEL_CODE == "1") ? "trung ương" : ((hIS_MEDI_ORG.LEVEL_CODE == "2") ? "Tỉnh" : ((hIS_MEDI_ORG.LEVEL_CODE == "3") ? "Huyện" : "Xã"))), ResourceMessage.ThongBao, MessageBoxButtons.OK);
							return false;
						}
					}
					if (ResultDataADO != null && ResultDataADO.ResultHistoryLDO != null && HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.WarningInvalidCheckHistoryHeinCard && ResultDataADO.ResultHistoryLDO.message == "Thẻ BHYT có thông tin kiểm tra thẻ chưa ra viện." && XtraMessageBox.Show(ResultDataADO.ResultHistoryLDO.message + " Bạn có muốn tiếp tục?", ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return false;
					}
					if (dataPatientProfile != null && dataPatientProfile.HisPatientTypeAlter.HAS_BIRTH_CERTIFICATE != "C" && ResultDataADO != null && ResultDataADO.ResultHistoryLDO != null)
					{
						if (HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.IsBlockingInvalidBhyt == 2.ToString() && HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.MaKetQuaBlockings.Contains(ResultDataADO.ResultHistoryLDO.maKetQua))
						{
							LogSystem.Info("maKetQua: " + ResultDataADO.ResultHistoryLDO.maKetQua);
							XtraMessageBox.Show(ResourceMessage.TheBhytKhongHopLeKhongChoPhepDangKy);
							return false;
						}
						if (HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.IsBlockingInvalidBhyt != 2.ToString() && HIS.Desktop.Plugins.Library.RegisterConfig.HisConfigCFG.MaKetQuaBlockings.Contains(ResultDataADO.ResultHistoryLDO.maKetQua))
						{
							LogSystem.Info("maKetQua: " + ResultDataADO.ResultHistoryLDO.maKetQua);
							if (XtraMessageBox.Show(ResourceMessage.TheBhytKhongHopLeBanCoMuonSuDung, ResourceMessage.ThongBao, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, DefaultBoolean.True) == DialogResult.OK)
							{
								return true;
							}
							return false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return false;
			}
			return true;
		}

		private bool AlertExpriedTimeHeinCardBhyt()
		{
			bool result = false;
			long resultDayAlert = -1L;
			try
			{
				if (cboPatientType.EditValue != null && Parse.ToInt64((cboPatientType.EditValue ?? "0").ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
				{
					if (uCMainHein != null && ucHein__BHYT != null)
					{
						resultDayAlert = uCMainHein.AlertExpriedTimeHeinCardBhyt(ucHein__BHYT, ConfigApplicationWorker.Get<long>("CONFIG_KEY__ALERT_EXPRIED_TIME_HEIN_CARD_BHYT"), ref resultDayAlert);
					}
					result = resultDayAlert <= -1 || MessageBox.Show(string.Format(ResourceMessage.TheSapHetHan, resultDayAlert), Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaThongBao), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
				}
				else
				{
					result = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public byte[] ImageToByteArray(Image imageIn)
		{
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				new Bitmap(imageIn).Save(memoryStream, ImageFormat.Jpeg);
				return memoryStream.ToArray();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

		private void SaveProcess(CommonParam param, bool check)
		{
			bool flag = false;
			patientId = 0L;
			resultApi = null;
			resultPatientTypeAlter = null;
			HisPatientTypeAlterAndTranPatiSDO patientTypeAlterSDO = new HisPatientTypeAlterAndTranPatiSDO();
			patientTypeAlterSDO.PatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
			try
			{
				WaitingManager.Show();
				if (ActionType == 1)
				{
					currentTreatmentLogSDO = new PatientTypeDepartmentADO();
					currentTreatmentLogSDO.patientTypeAlter = new V_HIS_PATIENT_TYPE_ALTER();
					currentTreatmentLogSDO.patientTypeAlter.LOG_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now).GetValueOrDefault();
					currentTreatmentLogSDO.patientTypeAlter.TREATMENT_ID = currentHisTreatment.ID;
				}
				currentTreatmentLogSDO.patientTypeAlter.TDL_PATIENT_ID = currentHisTreatment.PATIENT_ID;
				DataObjectMapper.Map<HIS_PATIENT_TYPE_ALTER>(patientTypeAlterSDO.PatientTypeAlter, currentTreatmentLogSDO.patientTypeAlter);
				UpdatePatientTypeAlterFromDataForm(ref patientTypeAlterSDO);
				if (ucKskContract != null && patientTypeAlterSDO.PatientTypeAlter != null && patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__KSK)
				{
					if (ucKskContract.cboContract.EditValue == null)
					{
						WaitingManager.Hide();
						XtraMessageBox.Show(ResourceMessage.DoiTuongKhamSucKhoe, ResourceMessage.ThongBao);
						return;
					}
					patientTypeAlterSDO.PatientTypeAlter.KSK_CONTRACT_ID = Parse.ToInt64(ucKskContract.cboContract.EditValue.ToString());
				}
				if (patientTypeAlterSDO.PatientTypeAlter != null && patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT && (IsChild() || patientTypeAlterSDO.PatientTypeAlter.HAS_BIRTH_CERTIFICATE == "C") && string.IsNullOrEmpty(currentHisTreatment.TDL_PATIENT_PROVINCE_CODE))
				{
					WaitingManager.Hide();
					XtraMessageBox.Show(ResourceMessage.BNTreEmCanNhapDuTT, ResourceMessage.ThongBao);
					return;
				}
				if (patientTypeAlterSDO.PatientTypeAlter != null && patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
				{
					bool flag2 = true;
					HisPatientTypeAlterFilter hisPatientTypeAlterFilter = new HisPatientTypeAlterFilter();
					hisPatientTypeAlterFilter.TREATMENT_ID = patientTypeAlterSDO.PatientTypeAlter.TREATMENT_ID;
					hisPatientTypeAlterFilter.PATIENT_TYPE_ID = patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID;
					hisPatientTypeAlterFilter.HEIN_CARD_NUMBER__EXACT = patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_NUMBER;
					hisPatientTypeAlterFilter.ID__NOT_EQUAL = patientTypeAlterSDO.PatientTypeAlter.ID;
					List<HIS_PATIENT_TYPE_ALTER> list = new BackendAdapter(param).Get<List<HIS_PATIENT_TYPE_ALTER>>("api/HisPatientTypeAlter/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientTypeAlterFilter, param);
					if (list != null && list.Count() > 0)
					{
						foreach (HIS_PATIENT_TYPE_ALTER item in list)
						{
							flag2 = flag2 && (patientTypeAlterSDO.PatientTypeAlter.RIGHT_ROUTE_CODE ?? "") == (item.RIGHT_ROUTE_CODE ?? "") && (patientTypeAlterSDO.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE ?? "") == (item.RIGHT_ROUTE_TYPE_CODE ?? "") && (patientTypeAlterSDO.PatientTypeAlter.LIVE_AREA_CODE ?? "") == (item.LIVE_AREA_CODE ?? "") && (patientTypeAlterSDO.PatientTypeAlter.HEIN_MEDI_ORG_CODE ?? "") == (item.HEIN_MEDI_ORG_CODE ?? "") && (patientTypeAlterSDO.PatientTypeAlter.JOIN_5_YEAR ?? "") == (item.JOIN_5_YEAR ?? "") && (patientTypeAlterSDO.PatientTypeAlter.PAID_6_MONTH ?? "") == (item.PAID_6_MONTH ?? "");
						}
					}
					if (!flag2)
					{
						WaitingManager.Hide();
						if (XtraMessageBox.Show(ResourceMessage.BNCoTTDienDoiTuong, ResourceMessage.ThongBao, MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
					}
				}
				if (ucImageBHYT != null && ucImageBHYT.pictureEditImageBHYT.Tag != "NoImage" && ucImageBHYT.pictureEditImageBHYT.Image != null)
				{
					patientTypeAlterSDO.ImgBhytData = ImageToByteArray(ucImageBHYT.pictureEditImageBHYT.Image);
				}
				if (patientTypeAlterSDO.ImgBhytData == null || patientTypeAlterSDO.ImgBhytData.Count() == 0)
				{
					patientTypeAlterSDO.PatientTypeAlter.BHYT_URL = null;
				}
				if (!IsUnusedHeinCardNumberByAnother(patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_NUMBER, patientTypeAlterSDO.PatientTypeAlter.TDL_PATIENT_ID))
				{
					return;
				}
				if (patientId > 0)
				{
					patientTypeAlterSDO.PatientTypeAlter.TDL_PATIENT_ID = patientId;
				}
				LogSystem.Debug(LogUtil.TraceData("patientTypeAlterAndTranPati__:", patientTypeAlterSDO));
				if (ActionType == 1)
				{
					resultPatientTypeAlter = new BackendAdapter(param).Post<HisPatientTypeAlterAndTranPatiSDO>("api/HisPatientTypeAlter/Create", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, patientTypeAlterSDO, param);
					if (resultPatientTypeAlter != null)
					{
						flag = true;
						if (!UpdatePatientClassify())
						{
							flag = false;
							return;
						}
						V_HIS_PATIENT_TYPE_ALTER v_HIS_PATIENT_TYPE_ALTER = new V_HIS_PATIENT_TYPE_ALTER();
						if (resultPatientTypeAlter.PatientTypeAlter != null)
						{
							DataObjectMapper.Map<V_HIS_PATIENT_TYPE_ALTER>(v_HIS_PATIENT_TYPE_ALTER, resultPatientTypeAlter.PatientTypeAlter);
						}
						PatientTypeDepartmentADO patientTypeDepartmentADO = new PatientTypeDepartmentADO();
						patientTypeDepartmentADO.patientTypeAlter = v_HIS_PATIENT_TYPE_ALTER;
						patientTypeDepartmentADO.LOG_TIME = resultPatientTypeAlter.PatientTypeAlter.LOG_TIME;
						patientTypeDepartmentADO.TREATMENT_ID = resultPatientTypeAlter.PatientTypeAlter.TREATMENT_ID;
						patientTypeDepartmentADO.type = 1L;
						List<PatientTypeDepartmentADO> list2 = lstTreatmentLog;
						list2.Add(patientTypeDepartmentADO);
						if (!chkAutoUpdateType.Checked)
						{
							LoadDataToGridSereServ(patientTypeDepartmentADO, list2);
						}
						else
						{
							AutoUpdateSereServ(patientTypeDepartmentADO, list2);
						}
					}
				}
				else if (ActionType == 2)
				{
					long? hanThe = patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_TO_TIME;
					if (patientTypeAlterSDO.PatientTypeAlter != null && hanThe.HasValue && hanThe > 0)
					{
						double value = 0.0;
						if (patientTypeAlterSDO.PatientTypeAlter.TREATMENT_TYPE_ID == 3 || patientTypeAlterSDO.PatientTypeAlter.TREATMENT_TYPE_ID == 4)
						{
							value = HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT");
						}
						hanThe = Calculation.Add(hanThe.GetValueOrDefault(), value, Calculation.UnitDifferenceTime.DAY);
						HisDepartmentTranFilter hisDepartmentTranFilter = new HisDepartmentTranFilter();
						hisDepartmentTranFilter.ID = patientTypeAlterSDO.PatientTypeAlter.DEPARTMENT_TRAN_ID;
						List<HIS_DEPARTMENT_TRAN> list3 = new BackendAdapter(param).Get<List<HIS_DEPARTMENT_TRAN>>("api/HisDepartmentTran/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisDepartmentTranFilter, param);
						if (list3 != null && list3.Count > 0)
						{
							HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
							hisServiceReqFilter.TREATMENT_ID = currentHisTreatment.ID;
							hisServiceReqFilter.REQUEST_DEPARTMENT_ID = list3.FirstOrDefault().DEPARTMENT_ID;
							List<HIS_SERVICE_REQ> list4 = new BackendAdapter(param).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, param);
							if (list4 != null && list4.Count > 0)
							{
								List<HIS_SERVICE_REQ> list5 = list4.Where((HIS_SERVICE_REQ o) => o.INTRUCTION_DATE > hanThe).ToList();
								if (list5 != null && list5.Count > 0)
								{
									WaitingManager.Hide();
									HisSereServFilter hisSereServFilter = new HisSereServFilter();
									hisSereServFilter.SERVICE_REQ_IDs = list5.Select((HIS_SERVICE_REQ o) => o.ID).ToList();
									List<HIS_SERE_SERV> sereServs = new BackendAdapter(param).Get<List<HIS_SERE_SERV>>("api/HisSereServ/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServFilter, param);
									if (sereServs != null && sereServs.Count > 0)
									{
										sereServs = sereServs.Where((HIS_SERE_SERV o) => o.PATIENT_TYPE_ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT).ToList();
										if (sereServs != null && sereServs.Count > 0)
										{
											list5 = list5.Where((HIS_SERVICE_REQ o) => sereServs.Select((HIS_SERE_SERV p) => p.SERVICE_REQ_ID).ToList().Contains(o.ID)).ToList();
											string arg = string.Join(",", list5.Select((HIS_SERVICE_REQ o) => o.SERVICE_REQ_CODE).ToList());
											XtraMessageBox.Show(string.Format(ResourceMessage.DaCoThongTinDoiTuongBHYT, arg), ResourceMessage.ThongBao);
											return;
										}
									}
								}
							}
						}
					}
					patientTypeAlterSDO.PatientTypeAlter.LOG_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dtLogTime.DateTime).GetValueOrDefault();
					if (patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID != HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT && patientTypeAlterSDO.PatientTypeAlter.PATIENT_TYPE_ID != HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__QN)
					{
						patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_FROM_TIME = null;
						patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_NUMBER = null;
						patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_TO_TIME = null;
						patientTypeAlterSDO.PatientTypeAlter.HEIN_MEDI_ORG_CODE = null;
						patientTypeAlterSDO.PatientTypeAlter.HEIN_MEDI_ORG_NAME = null;
						patientTypeAlterSDO.PatientTypeAlter.JOIN_5_YEAR = null;
						patientTypeAlterSDO.PatientTypeAlter.LEVEL_CODE = null;
						patientTypeAlterSDO.PatientTypeAlter.LIVE_AREA_CODE = null;
						patientTypeAlterSDO.PatientTypeAlter.PAID_6_MONTH = null;
						patientTypeAlterSDO.PatientTypeAlter.RIGHT_ROUTE_CODE = null;
						patientTypeAlterSDO.PatientTypeAlter.RIGHT_ROUTE_TYPE_CODE = null;
						patientTypeAlterSDO.PatientTypeAlter.HNCODE = null;
						patientTypeAlterSDO.PatientTypeAlter.HAS_BIRTH_CERTIFICATE = null;
						patientTypeAlterSDO.PatientTypeAlter.IS_TEMP_QN = null;
					}
					LogSystem.Debug(LogUtil.TraceData("patientTypeAlterAndTranPati__:", patientTypeAlterSDO));
					HIS_PATIENT_TYPE obj = (HIS_PATIENT_TYPE)cboPatientType.Properties.GetDataSourceRowByKeyValue(cboPatientType.EditValue);
					bool flag3 = false;
					if (obj.IS_COPAYMENT != 1 && chkAutoUpdateType.Checked)
					{
						if (!currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME.HasValue)
						{
							currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME = patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_FROM_TIME;
							currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME = patientTypeAlterSDO.PatientTypeAlter.HEIN_CARD_TO_TIME;
						}
						flag3 = true;
						AutoUpdateSereServ(currentTreatmentLogSDO, lstTreatmentLog);
					}
					resultPatientTypeAlter = new BackendAdapter(param).Post<HisPatientTypeAlterAndTranPatiSDO>("api/HisPatientTypeAlter/Update", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, patientTypeAlterSDO, null);
					LogSystem.Debug(LogUtil.TraceData("resultPatientTypeAlter__:", resultPatientTypeAlter));
					if (resultPatientTypeAlter != null)
					{
						flag = true;
						if (resultPatientTypeAlter.PatientTypeAlter != null && currentTreatmentLogSDO != null && currentTreatmentLogSDO.patientTypeAlter != null)
						{
							DataObjectMapper.Map<V_HIS_PATIENT_TYPE_ALTER>(currentTreatmentLogSDO.patientTypeAlter, resultPatientTypeAlter.PatientTypeAlter);
						}
						if (!UpdatePatientClassify())
						{
							flag = false;
							return;
						}
						if (check)
						{
							if (!chkAutoUpdateType.Checked)
							{
								LoadDataToGridSereServ(currentTreatmentLogSDO, lstTreatmentLog);
							}
							else if (!flag3)
							{
								if (!currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME.HasValue)
								{
									currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME = resultPatientTypeAlter.PatientTypeAlter.HEIN_CARD_FROM_TIME;
									currentTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME = resultPatientTypeAlter.PatientTypeAlter.HEIN_CARD_TO_TIME;
								}
								AutoUpdateSereServ(currentTreatmentLogSDO, lstTreatmentLog);
							}
						}
					}
				}
				WaitingManager.Hide();
				if (!string.IsNullOrEmpty(MesError))
				{
					XtraMessageBox.Show(string.Format(ResourceMessage.DVKhongTheChuyenDoi, MesError), ResourceMessage.ThongBao);
				}
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Fatal(ex);
			}
			MessageManager.Show(this, param, flag);
			SessionManager.ProcessTokenLost(param);
			if (flag)
			{
				if (RefeshReference != null)
				{
					RefeshReference();
				}
				Close();
			}
		}

		private bool UpdatePatientClassify()
		{
			bool result = true;
			try
			{
				if (cboClassify.EditValue != null && resultPatientTypeAlter != null && resultPatientTypeAlter.PatientTypeAlter != null)
				{
					V_HIS_PATIENT_TYPE_ALTER v_HIS_PATIENT_TYPE_ALTER = new BackendAdapter(new CommonParam()).Get<V_HIS_PATIENT_TYPE_ALTER>("api/HisPatientTypeAlter/GetLastByTreatmentId", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, currentHisTreatment.ID, null);
					if (v_HIS_PATIENT_TYPE_ALTER == null || (v_HIS_PATIENT_TYPE_ALTER != null && resultPatientTypeAlter.PatientTypeAlter.ID == v_HIS_PATIENT_TYPE_ALTER.ID && resultPatientTypeAlter.PatientTypeAlter.LOG_TIME == v_HIS_PATIENT_TYPE_ALTER.LOG_TIME))
					{
						CommonParam commonParam = new CommonParam();
						HisPatientUpdateSDO patientUpdateSdo = new HisPatientUpdateSDO();
						patientUpdateSdo.HisPatient = new HIS_PATIENT();
						patientUpdateSdo.HisPatient = ((patientId > 0) ? GetPatientByIds(new List<long> { patientId }).FirstOrDefault() : GetPatientByIds(new List<long> { currenPatient.ID }).FirstOrDefault());
						if (cboClassify.EditValue != null)
						{
							patientUpdateSdo.HisPatient.PATIENT_CLASSIFY_ID = long.Parse(cboClassify.EditValue.ToString());
						}
						if (cboMilitaryRank.EditValue != null)
						{
							patientUpdateSdo.HisPatient.MILITARY_RANK_ID = long.Parse(cboMilitaryRank.EditValue.ToString());
						}
						if (cboPosition.EditValue != null)
						{
							patientUpdateSdo.HisPatient.POSITION_ID = long.Parse(cboPosition.EditValue.ToString());
						}
						if (cboWorkPlace.EditValue != null)
						{
							patientUpdateSdo.HisPatient.WORK_PLACE_ID = long.Parse(cboWorkPlace.EditValue.ToString());
						}
						patientUpdateSdo.HisPatient.WORK_PLACE = txtWorkplace.Text;
						patientUpdateSdo.TreatmentId = currentHisTreatment.ID;
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => patientUpdateSdo), patientUpdateSdo));
						if (new BackendAdapter(commonParam).Post<HIS_PATIENT>("api/HisPatient/UpdateSdo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, patientUpdateSdo, commonParam) != null)
						{
							result = true;
							WaitingManager.Hide();
						}
						else
						{
							WaitingManager.Hide();
							result = false;
							string text = commonParam.GetMessage();
							if (text.Contains(ResourceMessage.XuLyThatBai))
							{
								text = text.Replace(ResourceMessage.XuLyThatBai, "");
							}
							XtraMessageBox.Show(string.Format(ResourceMessage.CapNhatTTDTTB, text), ResourceMessage.ThongBao);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
			return result;
		}

		private long NumberMin(long n1, long n2)
		{
			long result = 0L;
			try
			{
				if (n1 > n2)
				{
					result = n2;
				}
				if (n1 < n2)
				{
					result = n1;
				}
				if (n1 == n2)
				{
					result = n1;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private long NumberMax(long n1, long n2)
		{
			long result = 0L;
			try
			{
				if (n1 > n2)
				{
					result = n1;
				}
				if (n1 < n2)
				{
					result = n2;
				}
				if (n1 == n2)
				{
					result = n1;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void AutoUpdateSereServ(PatientTypeDepartmentADO data, List<PatientTypeDepartmentADO> listTL)
		{
			try
			{
				WaitingManager.Show();
				LogSystem.Error(LogUtil.TraceData("PatientTypeDepartmentADO", data));
				LogSystem.Error(LogUtil.TraceData("List<PatientTypeDepartmentADO>", listTL));
				if (data != null && data.type == 1)
				{
					data.patientTypeAlter.HEIN_CARD_TO_TIME = (data.patientTypeAlter.HEIN_CARD_TO_TIME.HasValue ? long.Parse(data.patientTypeAlter.HEIN_CARD_TO_TIME.ToString().Substring(0, 8) + "235959") : 0);
					listTL = listTL.OrderBy((PatientTypeDepartmentADO o) => o.LOG_TIME).ToList();
					PatientTypeDepartmentADO logtime = new PatientTypeDepartmentADO();
					for (int num = 0; num < listTL.Count; num++)
					{
						if (listTL[num].type == 1 && listTL[num].LOG_TIME != data.LOG_TIME && listTL[num].LOG_TIME > data.LOG_TIME)
						{
							logtime = listTL[num];
							break;
						}
					}
					CommonParam commonParam = new CommonParam();
					HisSereServBillFilter hisSereServBillFilter = new HisSereServBillFilter();
					hisSereServBillFilter.TDL_TREATMENT_ID = data.TREATMENT_ID;
					hisSereServBillFilter.IS_NOT_CANCEL = true;
					List<HIS_SERE_SERV_BILL> lstSereServBill = new BackendAdapter(commonParam).Get<List<HIS_SERE_SERV_BILL>>("api/HisSereServBill/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServBillFilter, commonParam);
					HisSereServView4Filter hisSereServView4Filter = new HisSereServView4Filter();
					hisSereServView4Filter.TREATMENT_ID = data.TREATMENT_ID;
					List<V_HIS_SERE_SERV_4> lstHisSereServWithTreatment = new List<V_HIS_SERE_SERV_4>();
					lstHisSereServWithTreatment = new BackendAdapter(commonParam).Get<List<V_HIS_SERE_SERV_4>>("api/HisSereServ/GetView4", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisSereServView4Filter, commonParam);
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstHisSereServWithTreatment.Select((V_HIS_SERE_SERV_4 o) => new { o.PATIENT_TYPE_ID, o.TDL_INTRUCTION_TIME })), lstHisSereServWithTreatment.Select((V_HIS_SERE_SERV_4 o) => new { o.PATIENT_TYPE_ID, o.TDL_INTRUCTION_TIME })));
					ProcessDataForUpdatePaty();
					if (lstHisSereServWithTreatment != null && lstHisSereServWithTreatment.Count > 0)
					{
						if (lstSereServBill != null && lstSereServBill.Count > 0)
						{
							lstHisSereServWithTreatment = lstHisSereServWithTreatment.Where((V_HIS_SERE_SERV_4 o) => !lstSereServBill.Select((HIS_SERE_SERV_BILL p) => p.SERE_SERV_ID).Contains(o.ID)).ToList();
						}
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstHisSereServWithTreatment.Select((V_HIS_SERE_SERV_4 o) => new { o.PATIENT_TYPE_ID, o.TDL_INTRUCTION_TIME })), lstHisSereServWithTreatment.Select((V_HIS_SERE_SERV_4 o) => new { o.PATIENT_TYPE_ID, o.TDL_INTRUCTION_TIME })));
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => data.LOG_TIME), data.LOG_TIME));
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => data.patientTypeAlter.HEIN_CARD_FROM_TIME), data.patientTypeAlter.HEIN_CARD_FROM_TIME));
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => data.patientTypeAlter.TREATMENT_TYPE_ID), data.patientTypeAlter.TREATMENT_TYPE_ID));
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => data.patientTypeAlter.HEIN_CARD_TO_TIME), data.patientTypeAlter.HEIN_CARD_TO_TIME));
						lstSereServResult = new List<V_HIS_SERE_SERV_4>();
						if (lstHisSereServWithTreatment != null && lstHisSereServWithTreatment.Count > 0)
						{
							if (logtime != null)
							{
								if (logtime.LOG_TIME > 0)
								{
									LogSystem.Error("CASE1");
									lstSereServResult = lstHisSereServWithTreatment.Where((V_HIS_SERE_SERV_4 o) => o.IS_NOT_USE_BHYT != 1 && o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && NumberMax(data.LOG_TIME, data.patientTypeAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault()) <= o.TDL_INTRUCTION_TIME && o.TDL_INTRUCTION_TIME <= ((data.patientTypeAlter.TREATMENT_TYPE_ID != 3 && data.patientTypeAlter.TREATMENT_TYPE_ID != 4) ? ((data.patientTypeAlter.HEIN_CARD_TO_TIME > 0) ? NumberMin(logtime.LOG_TIME, data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) : logtime.LOG_TIME) : ((data.patientTypeAlter.HEIN_CARD_TO_TIME > 0) ? NumberMin(logtime.LOG_TIME, Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.Now).AddDays(HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"))).GetValueOrDefault()) : logtime.LOG_TIME))).ToList();
								}
								else
								{
									LogSystem.Error("CASE2");
									lstSereServResult = lstHisSereServWithTreatment.Where((V_HIS_SERE_SERV_4 o) => o.IS_NOT_USE_BHYT != 1 && o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && NumberMax(data.LOG_TIME, data.patientTypeAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault()) <= o.TDL_INTRUCTION_TIME).ToList();
									if (data.patientTypeAlter.HEIN_CARD_TO_TIME > 0)
									{
										lstSereServResult = lstSereServResult.Where((V_HIS_SERE_SERV_4 o) => o.TDL_INTRUCTION_TIME <= ((data.patientTypeAlter.TREATMENT_TYPE_ID == 3 || data.patientTypeAlter.TREATMENT_TYPE_ID == 4) ? Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.Now).AddDays(HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"))).GetValueOrDefault() : data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault())).ToList();
									}
								}
							}
							else
							{
								LogSystem.Error("CASE3");
								lstSereServResult = lstHisSereServWithTreatment.Where((V_HIS_SERE_SERV_4 o) => o.IS_NOT_USE_BHYT != 1 && o.PATIENT_TYPE_ID != Parse.ToInt64(cboPatientType.EditValue.ToString()) && NumberMax(data.LOG_TIME, data.patientTypeAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault()) <= o.TDL_INTRUCTION_TIME).ToList();
								if (data.patientTypeAlter.HEIN_CARD_TO_TIME > 0)
								{
									lstSereServResult = lstHisSereServWithTreatment.Where((V_HIS_SERE_SERV_4 o) => o.TDL_INTRUCTION_TIME <= ((data.patientTypeAlter.TREATMENT_TYPE_ID == 3 || data.patientTypeAlter.TREATMENT_TYPE_ID == 4) ? Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber((Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault()) ?? DateTime.Now).AddDays(HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT"))).GetValueOrDefault() : data.patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault())).ToList();
								}
							}
						}
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => lstSereServResult.Select((V_HIS_SERE_SERV_4 o) => new { o.SERVICE_ID, o.SERVICE_REQ_ID, o.SERVICE_TYPE_CODE })), lstSereServResult.Select((V_HIS_SERE_SERV_4 o) => new { o.SERVICE_ID, o.SERVICE_REQ_ID, o.SERVICE_TYPE_CODE })));
						if (lstSereServResult.Count > 0)
						{
							data.patientTypeAlter.TREATMENT_TYPE_ID = Parse.ToInt64((cboTreatmentType.EditValue ?? "0").ToString());
							data.patientTypeAlter.TREATMENT_TYPE_CODE = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == data.patientTypeAlter.TREATMENT_TYPE_ID).TREATMENT_TYPE_CODE;
							data.patientTypeAlter.TREATMENT_TYPE_NAME = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == data.patientTypeAlter.TREATMENT_TYPE_ID).TREATMENT_TYPE_NAME;
							long? num2 = null;
							long num3 = 0L;
							if (cboPrimaryPatientType.EditValue != null && !string.IsNullOrEmpty(cboPrimaryPatientType.EditValue.ToString()))
							{
								num2 = Parse.ToInt64(cboPrimaryPatientType.EditValue.ToString());
							}
							if (cboPatientType.EditValue != null)
							{
								num3 = Parse.ToInt64(cboPatientType.EditValue.ToString());
							}
							if (System.Convert.ToInt16(HisConfigs.Get<string>("MOS.HIS_SERE_SERV.IS_SET_PRIMARY_PATIENT_TYPE")) == 2 && num2 == num3)
							{
								num2 = null;
							}
							SwapPatientTypeAlter(num3, num2, data, listTL, lstSereServResult);
						}
					}
				}
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Warn(ex);
			}
		}

		private void ProcessDataForUpdatePaty()
		{
			try
			{
				List<V_HIS_SERVICE_PATY> source = BackendDataWorker.Get<V_HIS_SERVICE_PATY>();
				List<string> patienttpecodeAllows = (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
					select o.PATIENT_TYPE_CODE).ToList();
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

		private void SwapPatientTypeAlter(long patient_type_id, long? patient_primary_patient_type_id, PatientTypeDepartmentADO HisTreatmentLogSDO, List<PatientTypeDepartmentADO> _lstTreatmentLog, List<V_HIS_SERE_SERV_4> _lstSereServ)
		{
			try
			{
				List<HIS_MEDICINE> list = new List<HIS_MEDICINE>();
				List<HIS_MATERIAL> list2 = new List<HIS_MATERIAL>();
				List<long> list3 = (from p in _lstSereServ
					where p.MEDICINE_ID.HasValue && p.MEDICINE_ID.Value > 0
					select p.MEDICINE_ID.Value).Distinct().ToList();
				List<long> list4 = (from p in _lstSereServ
					where p.MATERIAL_ID.HasValue && p.MATERIAL_ID.Value > 0
					select p.MATERIAL_ID.Value).Distinct().ToList();
				if (list3 != null && list3.Count > 0)
				{
					HisMedicineFilter hisMedicineFilter = new HisMedicineFilter();
					hisMedicineFilter.IDs = list3;
					list = new BackendAdapter(new CommonParam()).Get<List<HIS_MEDICINE>>("api/HisMedicine/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMedicineFilter, null);
				}
				if (list4 != null && list4.Count > 0)
				{
					HisMaterialFilter hisMaterialFilter = new HisMaterialFilter();
					hisMaterialFilter.IDs = list4;
					list2 = new BackendAdapter(new CommonParam()).Get<List<HIS_MATERIAL>>("api/HisMaterial/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisMaterialFilter, null);
				}
				HisServiceFilter hisServiceFilter = new HisServiceFilter();
				hisServiceFilter.IDs = _lstSereServ.Select((V_HIS_SERE_SERV_4 o) => o.SERVICE_ID).ToList();
				List<HIS_SERVICE> source = new BackendAdapter(new CommonParam()).Get<List<HIS_SERVICE>>("api/HisService/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceFilter, null);
				foreach (V_HIS_SERE_SERV_4 item in _lstSereServ)
				{
					long oldPatientTypeId = item.PATIENT_TYPE_ID;
					HIS_SERVICE hIS_SERVICE = source.Where((HIS_SERVICE o) => o.ID == item.SERVICE_ID).First();
					if (item.MEDICINE_ID.HasValue && item.MEDICINE_ID.Value > 0 && list != null && list.Count > 0)
					{
						if (patient_type_id == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
						{
							HIS_MEDICINE checkMedicine = list.FirstOrDefault((HIS_MEDICINE o) => o.ID == item.MEDICINE_ID.Value);
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
					else if (item.MATERIAL_ID.HasValue && item.MATERIAL_ID.Value > 0 && list2 != null && list2.Count > 0)
					{
						if (patient_type_id == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
						{
							HIS_MATERIAL checkMaterial = list2.FirstOrDefault((HIS_MATERIAL o) => o.ID == item.MATERIAL_ID.Value);
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
						if (patient_type_id == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
						{
							if (!string.IsNullOrEmpty(item.TDL_HEIN_SERVICE_BHYT_CODE) && !string.IsNullOrEmpty(item.TDL_HEIN_SERVICE_BHYT_NAME))
							{
								List<V_HIS_SERVICE_PATY> list5 = dicSevicepatyAllows[item.SERVICE_ID];
								if (list5 != null && list5.Count > 0 && list5.FirstOrDefault((V_HIS_SERVICE_PATY o) => o.PATIENT_TYPE_ID == patient_type_id) != null)
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
									else if (HisTreatmentLogSDO != null && item.IS_NOT_USE_BHYT != 1 && HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_FROM_TIME <= item.TDL_INTRUCTION_TIME && item.TDL_INTRUCTION_TIME <= HisTreatmentLogSDO.patientTypeAlter.HEIN_CARD_TO_TIME)
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
						MesError = MesError + item.TDL_SERVICE_CODE + ", ";
					}
					if (item.PATIENT_TYPE_ID != oldPatientTypeId && !IsAllowEditPatientTypeByServiceConfig(item))
					{
						item.PATIENT_TYPE_ID = oldPatientTypeId;
						HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == oldPatientTypeId);
						item.PATIENT_TYPE_NAME = ((hIS_PATIENT_TYPE != null) ? hIS_PATIENT_TYPE.PATIENT_TYPE_NAME : item.PATIENT_TYPE_NAME);
					}
					if (item.PATIENT_TYPE_ID != HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
					{
						item.SERVICE_CONDITION_ID = null;
					}
					else if (hIS_SERVICE.DO_NOT_USE_BHYT == 1)
					{
						item.PATIENT_TYPE_ID = oldPatientTypeId;
						item.PATIENT_TYPE_NAME = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == oldPatientTypeId).PATIENT_TYPE_NAME;
					}
					newLstSerSev.Add(item);
				}
				UpdateHisSereServ(HisTreatmentLogSDO);
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
					long? num = ((cboPrimaryPatientType.EditValue != null) ? new long?(Parse.ToInt64(cboPrimaryPatientType.EditValue.ToString())) : ((long?)null));
					long? num2 = ((cboPatientType.EditValue != null) ? new long?(Parse.ToInt64(cboPatientType.EditValue.ToString())) : ((long?)null));
					result = ((num.HasValue && num != num2) ? num : ((long?)null));
					return result;
				}
				if (keyIsSetPrimaryPatientType == 1)
				{
					long? num3 = null;
					if (cboClassify.EditValue != null && !string.IsNullOrEmpty(cboClassify.EditValue.ToString()))
					{
						num3 = Parse.ToInt64(cboClassify.EditValue.ToString());
					}
					List<V_HIS_SERVICE_PATY> list = dicSevicepatyAllows[item.SERVICE_ID];
					if (list != null && list.Count > 0 && service != null && service.BILL_PATIENT_TYPE_ID.HasValue && list.FirstOrDefault((V_HIS_SERVICE_PATY o) => o.PATIENT_TYPE_ID == service.BILL_PATIENT_TYPE_ID) != null && item.PATIENT_TYPE_ID != service.BILL_PATIENT_TYPE_ID && (service.APPLIED_PATIENT_TYPE_IDS == null || service.APPLIED_PATIENT_TYPE_IDS.Split(',').ToList().Contains(item.PATIENT_TYPE_ID.ToString())) && (service.APPLIED_PATIENT_CLASSIFY_IDS == null || (num3.HasValue && service.APPLIED_PATIENT_CLASSIFY_IDS.Split(',').ToList().Contains(num3.ToString()))))
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

		private void UpdateHisSereServ(PatientTypeDepartmentADO HisTreatmentLogSDO)
		{
			try
			{
				lstSereServ = new List<HIS_SERE_SERV>();
				CommonParam commonParam = new CommonParam();
				HisSereServPayslipSDO sdo = new HisSereServPayslipSDO();
				if (newLstSerSev != null && newLstSerSev.Count > 0)
				{
					Mapper.CreateMap<V_HIS_SERE_SERV_4, HIS_SERE_SERV>();
					lstSereServ = Mapper.Map<List<HIS_SERE_SERV>>(newLstSerSev);
				}
				sdo.SereServs = lstSereServ;
				sdo.Field = UpdateField.PATIENT_TYPE_ID;
				sdo.TreatmentId = HisTreatmentLogSDO.TREATMENT_ID;
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => sdo), sdo));
				List<HIS_SERE_SERV> updateSs = new BackendAdapter(commonParam).Post<List<HIS_SERE_SERV>>("api/HisSereServ/UpdateEveryPayslipInfo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, sdo, commonParam);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => updateSs), updateSs));
				if (updateSs != null)
				{
					commonParam.Messages = new List<string>();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool IsUnusedHeinCardNumberByAnother(string heinCardNumber, long patientId)
		{
			bool result = true;
			try
			{
				if (!string.IsNullOrEmpty(heinCardNumber))
				{
					CommonParam commonParam = new CommonParam();
					HisPatientTypeAlterFilter hisPatientTypeAlterFilter = new HisPatientTypeAlterFilter();
					hisPatientTypeAlterFilter.HEIN_CARD_NUMBER__EXACT = heinCardNumber;
					hisPatientTypeAlterFilter.TDL_PATIENT_ID__NOT_EQUAL = patientId;
					List<HIS_PATIENT_TYPE_ALTER> list = new BackendAdapter(commonParam).Get<List<HIS_PATIENT_TYPE_ALTER>>("api/HisPatientTypeAlter/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientTypeAlterFilter, commonParam);
					if (list != null && list.Count > 0)
					{
						List<long> ids = list.Select((HIS_PATIENT_TYPE_ALTER o) => o.TDL_PATIENT_ID).ToList();
						List<HIS_PATIENT> patientByIds = GetPatientByIds(ids);
						List<string> list2 = ((patientByIds != null) ? patientByIds.Select((HIS_PATIENT o) => o.PATIENT_CODE).ToList() : null);
						if (list2 != null && list2.Count > 0)
						{
							string arg = string.Join(", ", list2);
							WaitingManager.Hide();
							if (XtraMessageBox.Show(string.Format(ResourceMessage.TheNayDaDuocSD, arg), ResourceMessage.ThongBao, MessageBoxButtons.YesNo) != DialogResult.Yes)
							{
								return false;
							}
							List<object> list3 = new List<object>();
							list3.Add(treatmentId);
							list3.Add(list2);
							list3.Add(new DelegateSelectData(DelegateSuccess));
							new CallModule("HIS.Desktop.Plugins.TreatmentPatientUpdate", module.RoomId, module.RoomTypeId, list3);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				result = false;
			}
			return result;
		}

		private void DelegateSuccess(object data)
		{
			try
			{
				if (data is long)
				{
					patientId = (long)data;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private List<HIS_PATIENT> GetPatientByIds(List<long> ids)
		{
			List<HIS_PATIENT> list = null;
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientFilter hisPatientFilter = new HisPatientFilter();
				hisPatientFilter.IDs = ids;
				return new BackendAdapter(commonParam).Get<List<HIS_PATIENT>>("api/HisPatient/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientFilter, commonParam);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

		private bool IsChild()
		{
			bool flag = false;
			try
			{
				flag = BhytPatientTypeData.IsChild(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(currentHisTreatment.TDL_PATIENT_DOB) ?? DateTime.Now);
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		private void UpdateTranpati(HisPatientTypeAlterAndTranPatiSDO tranpati, HisPatientProfileSDO patientProfileSDO)
		{
			try
			{
				if (patientProfileSDO != null && patientProfileSDO.HisTreatment != null)
				{
					tranpati.TransferInFormId = patientProfileSDO.HisTreatment.TRANSFER_IN_FORM_ID;
					tranpati.TransferInIcdName = patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_NAME;
					tranpati.TransferInMediOrgCode = patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_CODE;
					tranpati.TransferInMediOrgName = patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_NAME;
					tranpati.TransferInReasonId = patientProfileSDO.HisTreatment.TRANSFER_IN_REASON_ID;
					tranpati.TransferInCmkt = patientProfileSDO.HisTreatment.TRANSFER_IN_CMKT;
					tranpati.TransferInCode = patientProfileSDO.HisTreatment.TRANSFER_IN_CODE;
					tranpati.TransferInIcdCode = patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_CODE;
					tranpati.TransferInTimeFrom = patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_FROM;
					tranpati.TransferInTimeTo = patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_TO;
					tranpati.HeinPatientTypeCode = patientProfileSDO.HisTreatment.HEIN_PATIENT_TYPE_CODE;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UpdatePatientTypeAlterFromDataForm(ref HisPatientTypeAlterAndTranPatiSDO patientTypeAlterSDO)
		{
			try
			{
				if (patientTypeAlterSDO == null)
				{
					patientTypeAlterSDO = new HisPatientTypeAlterAndTranPatiSDO();
				}
				if (patientTypeAlterSDO.PatientTypeAlter == null)
				{
					patientTypeAlterSDO.PatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				}
				Mapper.CreateMap<HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>();
				Mapper.CreateMap<HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>();
				HisPatientProfileSDO hisPatientProfileSDO = new HisPatientProfileSDO();
				hisPatientProfileSDO.HisPatientTypeAlter = Mapper.Map<HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>(patientTypeAlterSDO.PatientTypeAlter);
				UpdatePatientTypeAlterFromDataForm(hisPatientProfileSDO);
				patientTypeAlterSDO.PatientTypeAlter = Mapper.Map<HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>(hisPatientProfileSDO.HisPatientTypeAlter);
				UpdateTranpati(patientTypeAlterSDO, hisPatientProfileSDO);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UpdatePatientTypeAlterFromDataForm(HisPatientProfileSDO ServiceReqData)
		{
			try
			{
				if (ServiceReqData.HisPatientTypeAlter == null)
				{
					ServiceReqData.HisPatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				}
				if (ServiceReqData.HisPatientTypeAlter == null)
				{
					ServiceReqData.HisPatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				}
				long patientTypeId = Parse.ToInt64((cboPatientType.EditValue ?? "").ToString());
				HIS_PATIENT_TYPE hIS_PATIENT_TYPE = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == patientTypeId);
				if (hIS_PATIENT_TYPE != null)
				{
					if (hIS_PATIENT_TYPE.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT || hIS_PATIENT_TYPE.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__QN)
					{
						UpdateHeinCardBHYTDTOFromDataForm(ServiceReqData);
					}
					ServiceReqData.HisPatientTypeAlter.PATIENT_TYPE_ID = hIS_PATIENT_TYPE.ID;
					if (dtLogTime.EditValue != null && dtLogTime.DateTime != DateTime.MinValue)
					{
						ServiceReqData.HisPatientTypeAlter.LOG_TIME = Parse.ToInt64(dtLogTime.DateTime.ToString("yyyyMMddHHmm") + "00");
					}
					if (cboTreatmentType.EditValue != null)
					{
						ServiceReqData.HisPatientTypeAlter.TREATMENT_TYPE_ID = Parse.ToInt64((cboTreatmentType.EditValue ?? "0").ToString());
					}
					if (cboPrimaryPatientType.EditValue != null)
					{
						ServiceReqData.HisPatientTypeAlter.PRIMARY_PATIENT_TYPE_ID = System.Convert.ToInt64(cboPrimaryPatientType.EditValue);
					}
					else
					{
						ServiceReqData.HisPatientTypeAlter.PRIMARY_PATIENT_TYPE_ID = null;
					}
				}
				else
				{
					LogSystem.Debug("Lay doi tuong benh nhan theo gia tri cua combo doi tuong man hinh dang ky tiep don khong thanh cong. patientTypeId= " + patientTypeId);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UpdateHeinCardBHYTDTOFromDataForm(HisPatientProfileSDO HisPatientTypeAlter)
		{
			try
			{
				new CommonParam();
				if (HisPatientTypeAlter.HisPatientTypeAlter == null)
				{
					HisPatientTypeAlter.HisPatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				}
				if (HisPatientTypeAlter.HisTreatment == null)
				{
					HisPatientTypeAlter.HisTreatment = new HIS_TREATMENT();
				}
				uCMainHein.UpdateDataFormIntoPatientTypeAlter(ucHein__BHYT, HisPatientTypeAlter);
				uCMainHein.UpdateDataFormIntoPatientProfile(ucHein__BHYT, HisPatientTypeAlter);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetIcon()
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
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtLogTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtTreatmentTypeCode.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtTreatmentTypeCode_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadTreatmentType((sender as TextEdit).Text, false, cboTreatmentType, txtTreatmentTypeCode, txtPatientType);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtPatientType_KeyDown_1(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string searchCode = (sender as TextEdit).Text;
					LoadPatientType(searchCode, false, cboPatientType, txtPatientType);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void GetProvinceCode()
		{
			try
			{
				CommonParam commonParam = new CommonParam();
				HisPatientFilter hisPatientFilter = new HisPatientFilter();
				hisPatientFilter.ID = currentHisTreatment.PATIENT_ID;
				List<HIS_PATIENT> list = new BackendAdapter(commonParam).Get<List<HIS_PATIENT>>("api/HisPatient/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientFilter, commonParam);
				if (list != null && list.Count > 0)
				{
					provindcode = list.FirstOrDefault().PROVINCE_CODE;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboTreatmentType_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (uCMainHein != null && ucHein__BHYT != null)
				{
					long num = ((cboTreatmentType.EditValue != null) ? long.Parse(cboTreatmentType.EditValue.ToString()) : 0);
					if (currentTreatmentType == 0L || currentTreatmentType != num)
					{
						uCMainHein.SetValueTreatmentType(ucHein__BHYT, num);
					}
					currentTreatmentType = num;
					treatmentTypeId = (long)(cboTreatmentType.EditValue ?? ((object)0));
				}
				if (cboTreatmentType.EditValue != null)
				{
					HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = BackendDataWorker.Get<HIS_TREATMENT_TYPE>().SingleOrDefault((HIS_TREATMENT_TYPE o) => o.ID == long.Parse((cboTreatmentType.EditValue ?? "").ToString()));
					if (hIS_TREATMENT_TYPE != null)
					{
						txtTreatmentTypeCode.Text = hIS_TREATMENT_TYPE.TREATMENT_TYPE_CODE;
					}
					else
					{
						txtTreatmentTypeCode.Text = "";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static void LoadTreatmentType(string searchCode, bool isExpand, LookUpEdit _cboTreatmentType, TextEdit _txtTreatmentType, TextEdit focusControl)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					_cboTreatmentType.EditValue = null;
					_cboTreatmentType.Focus();
					_cboTreatmentType.ShowPopup();
					return;
				}
				List<HIS_TREATMENT_TYPE> list = (from o in BackendDataWorker.Get<HIS_TREATMENT_TYPE>()
					where o.TREATMENT_TYPE_CODE.Equals(searchCode) && o.IS_ACTIVE == 1
					select o).ToList();
				if (list != null)
				{
					if (list.Count == 1)
					{
						_cboTreatmentType.EditValue = list[0].ID;
						_txtTreatmentType.Text = list[0].TREATMENT_TYPE_CODE;
						focusControl.Focus();
						focusControl.SelectAll();
					}
					else
					{
						_cboTreatmentType.EditValue = null;
						_cboTreatmentType.Focus();
						_cboTreatmentType.ShowPopup();
					}
				}
				else
				{
					_cboTreatmentType.EditValue = null;
					_cboTreatmentType.Focus();
					_cboTreatmentType.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public bool LoadPatientType(string searchCode, bool isExpand, LookUpEdit _cboPatientType, TextEdit _txtPatientType)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					_cboPatientType.EditValue = null;
					_cboPatientType.Focus();
					_cboPatientType.ShowPopup();
				}
				else
				{
					List<HIS_PATIENT_TYPE> list = (from o in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
						where o.PATIENT_TYPE_CODE.Equals(searchCode)
						select o).ToList();
					if (list != null)
					{
						if (list.Count == 1)
						{
							if (_cboPatientType.EditValue == null || (long)_cboPatientType.EditValue != list[0].ID)
							{
								result = true;
								_cboPatientType.EditValue = list[0].ID;
								_txtPatientType.Text = list[0].PATIENT_TYPE_CODE;
							}
						}
						else
						{
							_cboPatientType.EditValue = null;
							_cboPatientType.Focus();
							_cboPatientType.ShowPopup();
						}
					}
					else
					{
						_cboPatientType.EditValue = null;
						_cboPatientType.Focus();
						_cboPatientType.ShowPopup();
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void dtLogTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal && dtLogTime.EditValue != null)
				{
					txtTreatmentTypeCode.Focus();
					txtTreatmentTypeCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientType_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				lciQrcode.Enabled = false;
				if (cboPatientType.EditValue != null && cboPatientType.EditValue != cboPatientType.OldEditValue && !IsLoadForm)
				{
					cboClassify.EditValue = null;
					LoadPatientClassify();
					InitComboCommon(cboClassify, dataClassify, "ID", "PATIENT_CLASSIFY_NAME", "PATIENT_CLASSIFY_CODE");
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE = (HIS_PATIENT_TYPE)cboPatientType.Properties.GetDataSourceRowByKeyValue(cboPatientType.EditValue);
					if (hIS_PATIENT_TYPE != null)
					{
						txtPatientType.Text = hIS_PATIENT_TYPE.PATIENT_TYPE_CODE;
						lciPrimaryPatientType.AppearanceItemCaption.ForeColor = Color.Black;
						if (hIS_PATIENT_TYPE.IS_ADDITION_REQUIRED == 1)
						{
							lciPrimaryPatientType.Visibility = LayoutVisibility.Always;
							lciComboPrimaryPatientType.Visibility = LayoutVisibility.Always;
							lciPrimaryPatientType.AppearanceItemCaption.ForeColor = Color.Maroon;
							ValidPrimaryPatientTypeCode();
						}
						else
						{
							VisiblePrimaryPatientType();
							lciPrimaryPatientType.AppearanceItemCaption.ForeColor = Color.Black;
							dxValidationProvider1.RemoveControlError(txtPrimaryPatientTypeCode);
							dxValidationProvider1.SetValidationRule(txtPrimaryPatientTypeCode, null);
						}
						if (hIS_PATIENT_TYPE.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
						{
							ucKskContract = null;
							ChoiceTemplateHeinCard(hIS_PATIENT_TYPE.PATIENT_TYPE_CODE, true);
							lciQrcode.Enabled = false;
						}
						else if (hIS_PATIENT_TYPE.ID == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__KSK)
						{
							ucImageBHYT = null;
							emptySpaceItem3.Visibility = LayoutVisibility.Never;
							layoutControlItem8.Visibility = LayoutVisibility.Never;
							btnSave.Size = new Size(110, btnSave.Height);
							base.Size = new Size(base.Width, 130);
							xclHeinCardInformation.Controls.Clear();
							xclHeinCardInformation.Update();
							xclHeinCardInformation.Enabled = true;
							ucKskContract = new UC_KskContract();
							ucKskContract.Dock = DockStyle.Fill;
							xclHeinCardInformation.Controls.Add(ucKskContract);
						}
						else
						{
							ucKskContract = null;
							ChoiceTemplateHeinCard(hIS_PATIENT_TYPE.PATIENT_TYPE_CODE, false);
						}
						if (layoutControlItem9.Visibility == LayoutVisibility.Never && hIS_PATIENT_TYPE.IS_COPAYMENT != 1 && !chkAutoUpdateType.Checked)
						{
							LoadDataToGridSereServ(currentTreatmentLogSDO, lstTreatmentLog);
						}
					}
					IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
					for (int num = invalidControls.Count - 1; num >= 0; num--)
					{
						dxValidationProvider1.RemoveControlError(invalidControls[num]);
					}
					dxErrorProvider1.ClearErrors();
				}
				else
				{
					btnSave.Focus();
					btnSave.Enabled = true;
				}
				ReSizeForm();
				LoadPrimaryPatientType();
				IsLoadForm = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void EnableSave(bool isEnable)
		{
			try
			{
				btnSave.Enabled = isEnable;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async void txtQrcode_KeyDown(object sender, KeyEventArgs e)
		{
			int num = 1;
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				string strValue = (sender as TextEdit).Text;
				if (strValue.Length <= 0)
				{
					return;
				}
				bool flag = false;
				HeinCardData heinCardData = null;
				string[] array = strValue.Split('|');
				if (array[0].Length == 10 || array[0].Length == 15)
				{
					heinCardData = GetDataQrCodeHeinCard(strValue);
				}
				else if (array[0].Length == 12)
				{
					CccdCardData dataQrCodeCccdCard = GetDataQrCodeCccdCard(strValue);
					flag = true;
					heinCardData = new HeinCardData();
					heinCardData.HeinCardNumber = dataQrCodeCccdCard.CardData;
					heinCardData.PatientName = dataQrCodeCccdCard.PatientName;
					heinCardData.Dob = dataQrCodeCccdCard.Dob;
					heinCardData.Gender = ((dataQrCodeCccdCard.Gender == "NAM") ? "1" : "2");
					heinCardData.Address = dataQrCodeCccdCard.Address;
				}
				if (heinCardData == null)
				{
					return;
				}
				LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName(() => strValue), strValue));
				string text = "";
				string heinAddressOfPatient = "";
				if (currentHisTreatment != null)
				{
					CommonParam commonParam = new CommonParam();
					HisPatientAdvanceFilter hisPatientAdvanceFilter = new HisPatientAdvanceFilter();
					hisPatientAdvanceFilter.PATIENT_CODE__EXACT = string.Format("{0:0000000000}", System.Convert.ToInt64(currentHisTreatment.TDL_PATIENT_CODE));
					HisPatientSDO patientSDO = new BackendAdapter(commonParam).Get<List<HisPatientSDO>>("api/HisPatient/GetSdoAdvance", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisPatientAdvanceFilter, new Action(SessionManager.ActionLostToken), commonParam).FirstOrDefault();
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName(() => patientSDO), patientSDO));
					if (patientSDO != null)
					{
						string text2 = ((patientSDO.IS_HAS_NOT_DAY_DOB != 1) ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientSDO.DOB) : patientSDO.DOB.ToString().Substring(0, 4));
						if ((!flag && patientSDO.VIR_PATIENT_NAME.ToLower() != Inventec.Common.String.Convert.HexToUTF8Fix(heinCardData.PatientName).ToLower()) || (flag && patientSDO.VIR_PATIENT_NAME.ToLower() != heinCardData.PatientName.ToLower()))
						{
							heinCardData = null;
							text = ResourceMessage.TheSaiHTenGov060;
						}
						else if (text2 != heinCardData.Dob)
						{
							heinCardData = null;
							text = ResourceMessage.TheSaiNgaySinhGov070;
						}
						else if (string.IsNullOrEmpty(patientSDO.COMMUNE_CODE) || string.IsNullOrEmpty(patientSDO.PROVINCE_CODE))
						{
							heinCardData = null;
							text = "Bệnh nhân thiếu thông tin địa chỉ";
						}
						else
						{
							heinAddressOfPatient = patientSDO.HeinAddress;
							string text3 = Inventec.Common.String.Convert.HexToUTF8Fix(heinCardData.PatientName);
							if (!string.IsNullOrEmpty(text3))
							{
								heinCardData.PatientName = text3;
							}
							string text4 = Inventec.Common.String.Convert.HexToUTF8Fix(heinCardData.Address);
							if (!string.IsNullOrEmpty(text4))
							{
								heinCardData.Address = text4;
							}
						}
					}
				}
				if (heinCardData != null && string.IsNullOrEmpty(text))
				{
					if (!flag)
					{
						ProcessQrCodeData(heinCardData);
					}
					HeinGOVManager heinGOVManager = new HeinGOVManager(ResourceMessage.GoiSangCongBHXHTraVeMaLoi);
					if (!flag)
					{
						ResultDataADO = await heinGOVManager.Check(heinCardData, null, false, heinAddressOfPatient, dtLogTime.DateTime, true);
					}
					else
					{
						ResultDataADO = await heinGOVManager.CheckCccdQrCode(heinCardData, null, dtLogTime.DateTime);
					}
					LogSystem.Info(LogUtil.TraceData(LogUtil.GetMemberName(() => heinCardData), heinCardData) + "____" + LogUtil.TraceData(LogUtil.GetMemberName(() => ResultDataADO), ResultDataADO));
					uCMainHein.SetResultDataADOBhyt(ucHein__BHYT, ResultDataADO);
					if (ResultDataADO != null)
					{
						heinCardData.HeinCardNumber = ResultDataADO.ResultHistoryLDO.maThe ?? heinCardData.HeinCardNumber;
						if (ResultDataADO.IsShowQuestionWhileChangeHeinTime__Choose)
						{
							heinCardData.HeinCardNumber = ResultDataADO.ResultHistoryLDO.maTheMoi;
						}
						ProcessQrCodeData(heinCardData);
						CheckTTProcessResultData(heinCardData, ResultDataADO);
					}
				}
				else if (!string.IsNullOrEmpty(text) && XtraMessageBox.Show(text + ResourceMessage.SuaTTBN, "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, DefaultBoolean.True) == DialogResult.OK)
				{
					if (_HisTreatment == null)
					{
						_HisTreatment = GetSDO(treatmentId).SingleOrDefault();
					}
					List<object> list = new List<object>();
					list.Add(_HisTreatment.PATIENT_ID);
					list.Add(treatmentId);
					list.Add(new RefeshReference(RefeshTreatment));
					PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.PatientUpdate", module.RoomId, module.RoomTypeId, list);
				}
				txtQrcode.Text = "";
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private string HisToHein(string ge)
		{
			if (!(ge == "1"))
			{
				return "1";
			}
			return "2";
		}

		private void ProcessQrCodeData(HeinCardData dataHein)
		{
			try
			{
				string currentHeincardNumber = dataHein.HeinCardNumber;
				if (dataHein == null)
				{
					throw new ArgumentNullException("ProcessQrCodeData => dataHein is null");
				}
				if (!string.IsNullOrEmpty(dataHein.HeinCardNumber))
				{
					if (dataHein.HeinCardNumber.Length > 17)
					{
						dataHein.HeinCardNumber = dataHein.HeinCardNumber.Substring(0, 17);
					}
					else if (dataHein.HeinCardNumber.Length != 15 && dataHein.HeinCardNumber.Length != 17)
					{
						LogSystem.Info("Do dai so the bhyt cua benh nhan khong hop le. " + LogUtil.TraceData(LogUtil.GetMemberName(() => currentHeincardNumber), currentHeincardNumber));
					}
				}
				FillDataAfterFindQrCodeNoExistsCard(dataHein);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CheckTTProcessResultData(HeinCardData dataHein, ResultDataADO ResultDataADO)
		{
			try
			{
				if (ResultDataADO == null || ResultDataADO.ResultHistoryLDO == null)
				{
					return;
				}
				if (ResultDataADO.ResultHistoryLDO.maKetQua == "000" && ResultDataADO.ResultHistoryLDO.gioiTinh.ToLower() != currentHisTreatment.TDL_PATIENT_GENDER_NAME.ToLower())
				{
					CommonParam commonParam = new CommonParam();
					HisPatientUpdateSDO patientUpdateSdo = new HisPatientUpdateSDO();
					patientUpdateSdo.HisPatient = new HIS_PATIENT();
					patientUpdateSdo.HisPatient = ((patientId > 0) ? GetPatientByIds(new List<long> { patientId }).FirstOrDefault() : GetPatientByIds(new List<long> { currenPatient.ID }).FirstOrDefault());
					HIS_GENDER hIS_GENDER = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER o) => o.GENDER_NAME.ToLower() == ResultDataADO.ResultHistoryLDO.gioiTinh.ToLower());
					if (hIS_GENDER != null)
					{
						patientUpdateSdo.HisPatient.GENDER_ID = hIS_GENDER.ID;
						patientUpdateSdo.TreatmentId = currentHisTreatment.ID;
						LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => patientUpdateSdo), patientUpdateSdo));
						currenPatient = new BackendAdapter(commonParam).Post<HIS_PATIENT>("api/HisPatient/UpdateSdo", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, patientUpdateSdo, commonParam);
						LoadCurrentHisTreatment();
						if (string.IsNullOrEmpty(currenPatient.COMMUNE_CODE) && string.IsNullOrEmpty(currenPatient.PROVINCE_CODE))
						{
							XtraMessageBox.Show("Bệnh nhân thiếu thông tin địa chỉ", "Thông báo!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, DefaultBoolean.True);
						}
					}
				}
				dataHein.FineYearMonthDate = this.ResultDataADO.ResultHistoryLDO.ngayDu5Nam;
				LogSystem.Info("CheckTTProcessResultData => 1");
				if (ResultDataADO.IsShowQuestionWhileChangeHeinTime__Choose)
				{
					LogSystem.Info("CheckTTProcessResultData => 2");
					if (uCMainHein != null && ucHein__BHYT != null)
					{
						uCMainHein.FillDataAfterCheckBHYT(ucHein__BHYT, dataHein);
					}
				}
				if (ResultDataADO.IsToDate)
				{
					if (uCMainHein != null && ucHein__BHYT != null)
					{
						uCMainHein.FillDataAfterCheckBHYT(ucHein__BHYT, ResultDataADO.HeinCardData);
					}
					LogSystem.Debug("Ket thuc gan du lieu cho benh nhan khi doc the va khong co han den");
				}
				if (ResultDataADO.IsThongTinNguoiDungThayDoiSoVoiCong__Choose)
				{
					LogSystem.Info("CheckTTProcessResultData => 3");
					if (uCMainHein != null && ucHein__BHYT != null)
					{
						uCMainHein.FillDataAfterCheckBHYT(ucHein__BHYT, dataHein);
					}
				}
				if (HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.IsCheckExamHistory && (ResultDataADO.IsShowQuestionWhileChangeHeinTime__Choose || ResultDataADO.SuccessWithoutMessage))
				{
					LogSystem.Info("Mo form lich su voi data rsIns");
					new frmCheckHeinCardGOV(ResultDataADO.ResultHistoryLDO).ShowDialog();
				}
				LogSystem.Debug("CheckHanSDTheBHYT => 3");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private HeinCardData GetDataQrCodeHeinCard(string qrCode)
		{
			HeinCardData result = null;
			try
			{
				result = new ReadQrCodeHeinCard().ReadDataQrCode(qrCode);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private CccdCardData GetDataQrCodeCccdCard(string qrCode)
		{
			CccdCardData result = null;
			try
			{
				result = ReadQrCodeCCCD.ReadDataQrCode(qrCode);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void LoadPrimaryPatientType()
		{
			try
			{
				primaryPatientTypes = new List<HIS_PATIENT_TYPE>();
				List<V_HIS_PATIENT_TYPE_ALLOW> patyAlows = BackendDataWorker.Get<V_HIS_PATIENT_TYPE_ALLOW>();
				if (cboPatientType.EditValue != null)
				{
					long patyId = System.Convert.ToInt64(cboPatientType.EditValue);
					primaryPatientTypes = (from p in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
						where p.IS_ACTIVE == 1 && p.ID != patyId && patyAlows != null && patyAlows.Any((V_HIS_PATIENT_TYPE_ALLOW a) => a.PATIENT_TYPE_ID == patyId && a.PATIENT_TYPE_ALLOW_ID == p.ID)
						select p).ToList();
				}
				if (cboPrimaryPatientType.EditValue != null && (cboPatientType.EditValue == null || System.Convert.ToInt64(cboPatientType.EditValue) == System.Convert.ToInt64(cboPrimaryPatientType.EditValue)))
				{
					cboPrimaryPatientType.EditValue = null;
				}
				if (cboPrimaryPatientType.EditValue != null && (primaryPatientTypes == null || !primaryPatientTypes.Any((HIS_PATIENT_TYPE a) => a.ID == System.Convert.ToInt64(cboPrimaryPatientType.EditValue))))
				{
					cboPrimaryPatientType.EditValue = null;
				}
				PatientTypeLoader.LoadDataToCombo(cboPrimaryPatientType, primaryPatientTypes);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtPrimaryPatientTypeCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				string strValue = (sender as TextEdit).Text;
				if (!string.IsNullOrWhiteSpace(strValue))
				{
					strValue = strValue.ToLower();
					List<HIS_PATIENT_TYPE> list = (from p in BackendDataWorker.Get<HIS_PATIENT_TYPE>()
						where p.IS_ACTIVE == 1 && p.IS_NOT_USE_FOR_PATIENT != 1 && p.PATIENT_TYPE_CODE.ToLower().Contains(strValue)
						select p).ToList();
					if (list != null && list.Count == 1)
					{
						cboPrimaryPatientType.EditValue = list[0].ID;
						lciQrcode.Enabled = false;
						if (Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
						{
							uCMainHein.DefaultFocusUserControl(ucHein__BHYT);
							lciQrcode.Enabled = true;
						}
					}
					else
					{
						cboPrimaryPatientType.Focus();
						cboPrimaryPatientType.ShowPopup();
					}
				}
				else
				{
					cboPrimaryPatientType.Focus();
					cboPrimaryPatientType.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPrimaryPatientType_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					lciQrcode.Enabled = false;
					if (Parse.ToInt64((cboPatientType.EditValue ?? ((object)0)).ToString()) == HIS.Desktop.Plugins.CallPatientTypeAlter.Config.HisConfigCFG.PatientTypeId__BHYT)
					{
						uCMainHein.DefaultFocusUserControl(ucHein__BHYT);
						lciQrcode.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPrimaryPatientType_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPrimaryPatientType.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPrimaryPatientType_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				txtPrimaryPatientTypeCode.Text = "";
				if (cboPrimaryPatientType.EditValue != null)
				{
					cboPrimaryPatientType.Properties.Buttons[1].Visible = true;
					HIS_PATIENT_TYPE hIS_PATIENT_TYPE = ((primaryPatientTypes != null) ? primaryPatientTypes.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == System.Convert.ToInt64(cboPrimaryPatientType.EditValue)) : null);
					if (hIS_PATIENT_TYPE != null)
					{
						txtPrimaryPatientTypeCode.Text = hIS_PATIENT_TYPE.PATIENT_TYPE_CODE;
					}
				}
				else
				{
					cboPrimaryPatientType.Properties.Buttons[1].Visible = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboClassify_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				layoutControlItem10.Visibility = LayoutVisibility.Never;
				layoutControlItem11.Visibility = LayoutVisibility.Never;
				layoutControlItem12.Visibility = LayoutVisibility.Never;
				layoutControlItem13.Visibility = LayoutVisibility.Never;
				emptySpaceItem2.Visibility = LayoutVisibility.Always;
				dxValidationProvider1.SetValidationRule(cboMilitaryRank, null);
				dxValidationProvider1.SetValidationRule(cboPosition, null);
				dxValidationProvider1.SetValidationRule(cboWorkPlace, null);
				if (cboClassify.EditValue == null)
				{
					return;
				}
				HIS_PATIENT_TYPE hIS_PATIENT_TYPE = (HIS_PATIENT_TYPE)cboPatientType.Properties.GetDataSourceRowByKeyValue(cboPatientType.EditValue);
				if (!IsFirstLoadClassify && hIS_PATIENT_TYPE.IS_COPAYMENT != 1 && !chkAutoUpdateType.Checked)
				{
					LoadDataToGridSereServ(currentTreatmentLogSDO, lstTreatmentLog);
				}
				cboClassify.Properties.Buttons[1].Visible = true;
				HIS_PATIENT_CLASSIFY val = BackendDataWorker.Get<HIS_PATIENT_CLASSIFY>().FirstOrDefault((HIS_PATIENT_CLASSIFY o) => o.ID == long.Parse(cboClassify.EditValue.ToString()));
				if (val != null && val.IS_POLICE == 1 && !IsVisibleClassify)
				{
					if (currentNameControl != null && currentNameControl.Count > 0)
					{
						string item = layoutControl1.Name + ".Root." + layoutControlItem10.Name;
						string item2 = layoutControl1.Name + ".Root." + layoutControlItem11.Name;
						string item3 = layoutControl1.Name + ".Root." + layoutControlItem12.Name;
						if (!currentNameControl.Contains(item))
						{
							layoutControlItem10.Visibility = LayoutVisibility.Always;
							ValidationSingleControl(cboMilitaryRank);
						}
						if (!currentNameControl.Contains(item2))
						{
							layoutControlItem11.Visibility = LayoutVisibility.Always;
							ValidationSingleControl(cboPosition);
						}
						if (!currentNameControl.Contains(item3))
						{
							layoutControlItem12.Visibility = LayoutVisibility.Always;
							layoutControlItem13.Visibility = LayoutVisibility.Always;
							ValidWorkPlace();
						}
					}
					else
					{
						EnableControlCombo(true);
						ValidationSingleControl(cboMilitaryRank);
						ValidationSingleControl(cboPosition);
						ValidWorkPlace();
					}
				}
				IsFirstLoadClassify = false;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboClassify_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboClassify.Properties.Buttons[1].Visible = false;
					cboClassify.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMilitaryRank_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboMilitaryRank.Properties.Buttons[1].Visible = false;
					cboMilitaryRank.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPosition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPosition.Properties.Buttons[1].Visible = false;
					cboPosition.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboWorkPlace_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboWorkPlace.Properties.Buttons[2].Visible = false;
					cboWorkPlace.EditValue = null;
				}
				else if (e.Button.Kind == ButtonPredefines.Plus)
				{
					Module module = GlobalVariables.currentModuleRaws.Where((Module o) => o.ModuleLink == "HIS.Desktop.Plugins.HisWorkPlace").FirstOrDefault();
					if (module == null)
					{
						throw new NullReferenceException("Not found module by ModuleLink = 'HIS.Desktop.Plugins.HisWorkPlace'");
					}
					if (!module.IsPlugin || module.ExtensionInfo == null)
					{
						throw new NullReferenceException("Module 'HIS.Desktop.Plugins.HisWorkPlace' is not plugins");
					}
					List<object> arrParams = new List<object>();
					object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, 0L, 0L), arrParams);
					if (pluginInstance == null)
					{
						throw new ArgumentNullException("moduleData is null");
					}
					((Form)pluginInstance).ShowDialog();
					BackendDataWorker.Reset<HIS_WORK_PLACE>();
					LoadWorkPlace();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboMilitaryRank_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboMilitaryRank.EditValue != null)
				{
					cboMilitaryRank.Properties.Buttons[1].Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPosition_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboPosition.EditValue != null)
				{
					cboPosition.Properties.Buttons[1].Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboWorkPlace_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboWorkPlace.EditValue != null)
				{
					cboWorkPlace.Properties.Buttons[2].Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void checkEdit1_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				ControlStateRDO csAddOrUpdate = ((currentControlStateRDO != null && currentControlStateRDO.Count > 0) ? currentControlStateRDO.Where((ControlStateRDO o) => o.KEY == chkAutoUpdateType.Name && o.MODULE_LINK == ModuleLinkName).FirstOrDefault() : null);
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => csAddOrUpdate), csAddOrUpdate));
				if (csAddOrUpdate != null)
				{
					csAddOrUpdate.VALUE = (chkAutoUpdateType.Checked ? "1" : "");
				}
				else
				{
					csAddOrUpdate = new ControlStateRDO();
					csAddOrUpdate.KEY = chkAutoUpdateType.Name;
					csAddOrUpdate.VALUE = (chkAutoUpdateType.Checked ? "1" : "");
					csAddOrUpdate.MODULE_LINK = ModuleLinkName;
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

		private void InitControlState()
		{
			try
			{
				controlStateWorker = new ControlStateWorker();
				currentControlStateRDO = controlStateWorker.GetData(ModuleLinkName);
				if (currentControlStateRDO == null || currentControlStateRDO.Count <= 0)
				{
					return;
				}
				foreach (ControlStateRDO item in currentControlStateRDO)
				{
					if (item.KEY == chkAutoUpdateType.Name)
					{
						chkAutoUpdateType.Checked = item.VALUE == "1";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			try
			{
				new RichEditorStore(HIS.Desktop.ApiConsumer.ApiConsumers.SarConsumer, ConfigSystems.URI_API_SAR, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetLanguage(), GlobalVariables.TemnplatePathFolder).RunPrintTemplate("Mps000473", new DelegateRunPrinter(DelegateRunPrinter));
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
				if (printTypeCode == "Mps000473")
				{
					LoadBieuMau(printTypeCode, fileName, ref result);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void LoadBieuMau(string printTypeCode, string fileName, ref bool result)
		{
			try
			{
				if (currentHisTreatment != null)
				{
					Mapper.CreateMap<V_HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>();
					HIS_PATIENT_TYPE_ALTER patientAlter = Mapper.Map<V_HIS_PATIENT_TYPE_ALTER, HIS_PATIENT_TYPE_ALTER>(currentTreatmentLogSDO.patientTypeAlter);
					CommonParam commonParam = new CommonParam();
					HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
					hisTreatmentFilter.ID = currentHisTreatment.ID;
					List<HIS_TREATMENT> source = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisTreatmentFilter, commonParam);
					HisBedLogViewFilter hisBedLogViewFilter = new HisBedLogViewFilter();
					hisBedLogViewFilter.TREATMENT_ID = currentHisTreatment.ID;
					List<V_HIS_BED_LOG> list = new BackendAdapter(commonParam).Get<List<V_HIS_BED_LOG>>("api/HisBedLog/GetView", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisBedLogViewFilter, commonParam);
					HisServiceReqFilter hisServiceReqFilter = new HisServiceReqFilter();
					hisServiceReqFilter.TREATMENT_ID = currentHisTreatment.ID;
					hisServiceReqFilter.IS_MAIN_EXAM = true;
					List<HIS_SERVICE_REQ> list2 = new BackendAdapter(commonParam).Get<List<HIS_SERVICE_REQ>>("api/HisServiceReq/Get", HIS.Desktop.ApiConsumer.ApiConsumers.MosConsumer, hisServiceReqFilter, commonParam);
					Mps000473PDO data = new Mps000473PDO(currenPatient, patientAlter, source.First(), (list != null && list.Count > 0) ? list.OrderByDescending((V_HIS_BED_LOG o) => o.ID).First() : null, (list2 != null) ? list2.First() : null, ClientTokenManagerStore.ClientTokenManager.GetLoginName());
					string printerName = "";
					if (GlobalVariables.dicPrinter.ContainsKey(printTypeCode))
					{
						printerName = GlobalVariables.dicPrinter[printTypeCode];
					}
					InputADO emrInputADO = new EmrGenerateProcessor().GenerateInputADOWithPrintTypeCode(currentHisTreatment.TREATMENT_CODE, printTypeCode, (module != null) ? module.RoomId : 0);
					if (ConfigApplicationWorker.Get<string>("CONFIG_KEY__HIS_DESKTOP__ASSIGN_PRESCRIPTION__IS_PRINT_NOW") == "1")
					{
						result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
						{
							EmrInputADO = emrInputADO
						});
					}
					else if (ConfigApplications.CheDoInChoCacChucNangTrongPhanMem == 2)
					{
						result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.PrintNow, printerName)
						{
							EmrInputADO = emrInputADO
						});
					}
					else
					{
						result = MpsPrinter.Run(new PrintData(printTypeCode, fileName, data, MPS.ProcessorBase.PrintConfig.PreviewType.Show, printerName)
						{
							EmrInputADO = emrInputADO
						});
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				WaitingManager.Hide();
			}
		}

		private void EnableBtnPrint()
		{
			try
			{
				if (currentHisTreatment.IS_EMERGENCY == 1)
				{
					btnPrint.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public override void ProcessDisposeModuleDataAfterClose()
		{
			try
			{
				if (uCMainHein != null)
				{
					uCMainHein.DisposeControl(ucHein__BHYT);
				}
				if (ucImageBHYT != null)
				{
					ucImageBHYT.DisposeControl();
				}
				if (ucKskContract != null)
				{
					ucKskContract.DisposeControl();
				}
				lstSereServ = null;
				newLstSerSev = null;
				MesError = null;
				dicSevicepatyAllows = null;
				ModuleLinkName = null;
				currentControlStateRDO = null;
				controlStateWorker = null;
				IsHasEmergency = false;
				primaryPatientTypes = null;
				IsVisibleClassify = false;
				baseNameControl = null;
				branch = null;
				currentNameControl = null;
				APP_CODE__EXACT = null;
				ModuleLinkName = null;
				currentHideControls = null;
				IsLoadForm = false;
				IsFirstLoadClassify = false;
				isEdit = false;
				resultSuccess = false;
				_IsShowLsKcb = false;
				currenPatient = null;
				dataClassify = null;
				dataMilitaryRank = null;
				dataWorkPlace = null;
				dataPosition = null;
				keyIsSetPrimaryPatientType = 0L;
				patientId = 0L;
				ucImageBHYT = null;
				ucKskContract = null;
				currentTreatmentSave = null;
				lstSereServResult = null;
				lstTreatmentLog = null;
				resultPatientTypeAlter = null;
				resultApi = null;
				provindcode = null;
				positionHandleControl = 0;
				IsView = null;
				ActionType = 0;
				ucHein__BHYT = null;
				uCMainHein = null;
				treatmentId = 0L;
				RefeshReference = null;
				currentHisTreatment = null;
				module = null;
				currentTreatmentLogSDO = null;
				IsRuning = false;
				_HisTreatment = null;
				listResult = null;
				btnPrint.Click -= new EventHandler(btnPrint_Click);
				chkAutoUpdateType.CheckedChanged -= new EventHandler(checkEdit1_CheckedChanged);
				barButtonItem1.ItemClick -= new ItemClickEventHandler(barButtonItem1_ItemClick);
				cboWorkPlace.ButtonClick -= new ButtonPressedEventHandler(cboWorkPlace_ButtonClick);
				cboWorkPlace.EditValueChanged -= new EventHandler(cboWorkPlace_EditValueChanged);
				cboPosition.ButtonClick -= new ButtonPressedEventHandler(cboPosition_ButtonClick);
				cboPosition.EditValueChanged -= new EventHandler(cboPosition_EditValueChanged);
				cboMilitaryRank.ButtonClick -= new ButtonPressedEventHandler(cboMilitaryRank_ButtonClick);
				cboMilitaryRank.EditValueChanged -= new EventHandler(cboMilitaryRank_EditValueChanged);
				cboClassify.ButtonClick -= new ButtonPressedEventHandler(cboClassify_ButtonClick);
				cboClassify.EditValueChanged -= new EventHandler(cboClassify_EditValueChanged);
				cboPrimaryPatientType.Closed -= new ClosedEventHandler(cboPrimaryPatientType_Closed);
				cboPrimaryPatientType.ButtonClick -= new ButtonPressedEventHandler(cboPrimaryPatientType_ButtonClick);
				cboPrimaryPatientType.EditValueChanged -= new EventHandler(cboPrimaryPatientType_EditValueChanged);
				txtPrimaryPatientTypeCode.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtPrimaryPatientTypeCode_PreviewKeyDown);
				dtLogTime.Closed -= new ClosedEventHandler(dtLogTime_Closed);
				cboPatientType.Closed -= new ClosedEventHandler(cboPatientType_Closed);
				cboPatientType.EditValueChanged -= new EventHandler(cboPatientType_EditValueChanged);
				cboTreatmentType.Closed -= new ClosedEventHandler(cboTreatmentType_Closed);
				cboTreatmentType.EditValueChanged -= new EventHandler(cboTreatmentType_EditValueChanged);
				txtPatientType.KeyDown -= new KeyEventHandler(txtPatientType_KeyDown_1);
				txtTreatmentTypeCode.KeyDown -= new KeyEventHandler(txtTreatmentTypeCode_KeyDown);
				btnSave.Click -= new EventHandler(btnSave_Click);
				txtQrcode.KeyDown -= new KeyEventHandler(txtQrcode_KeyDown);
				base.Load -= new EventHandler(frmPatientTypeAlter_Load);
				gridView1.GridControl.DataSource = null;
				cboClassify.Properties.DataSource = null;
				gridLookUpEdit2View.GridControl.DataSource = null;
				cboMilitaryRank.Properties.DataSource = null;
				gridLookUpEdit3View.GridControl.DataSource = null;
				cboPosition.Properties.DataSource = null;
				gridLookUpEdit4View.GridControl.DataSource = null;
				cboWorkPlace.Properties.DataSource = null;
				gridLookUpEdit1View.GridControl.DataSource = null;
				cboPrimaryPatientType.Properties.DataSource = null;
				cboTreatmentType.Properties.DataSource = null;
				cboPatientType.Properties.DataSource = null;
				layoutControlItem15 = null;
				layoutControlItem14 = null;
				chkAutoUpdateType = null;
				btnPrint = null;
				emptySpaceItem2 = null;
				layoutControlItem13 = null;
				layoutControlItem12 = null;
				layoutControlItem11 = null;
				layoutControlItem10 = null;
				layoutControlItem9 = null;
				gridView1 = null;
				cboClassify = null;
				gridLookUpEdit2View = null;
				cboMilitaryRank = null;
				gridLookUpEdit3View = null;
				cboPosition = null;
				gridLookUpEdit4View = null;
				cboWorkPlace = null;
				txtWorkplace = null;
				emptySpaceItem3 = null;
				lciComboPrimaryPatientType = null;
				lciPrimaryPatientType = null;
				txtPrimaryPatientTypeCode = null;
				gridLookUpEdit1View = null;
				cboPrimaryPatientType = null;
				lciQrcode = null;
				txtQrcode = null;
				layoutControlItem8 = null;
				panelControlImageBHYT = null;
				layoutControlItem2 = null;
				dtLogTime = null;
				barButtonItem1 = null;
				bar2 = null;
				barManager1 = null;
				barDockControlTop = null;
				barDockControlBottom = null;
				barDockControlRight = null;
				barDockControlLeft = null;
				dxErrorProvider1 = null;
				dxValidationProvider1 = null;
				layoutControlItem7 = null;
				layoutControlItem6 = null;
				cboTreatmentType = null;
				cboPatientType = null;
				layoutControlItem5 = null;
				layoutControlItem1 = null;
				txtTreatmentTypeCode = null;
				txtPatientType = null;
				emptySpaceItem1 = null;
				layoutControlItem4 = null;
				layoutControlItem3 = null;
				btnSave = null;
				xclHeinCardInformation = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtLogTime_Leave(object sender, EventArgs e)
		{
			try
			{
				if (uCMainHein != null && ucHein__BHYT != null)
				{
					long num = ((dtLogTime.EditValue != null) ? long.Parse(dtLogTime.DateTime.ToString("yyyyMMdd0000")) : 0);
					if (currentLogTime == 0L || currentLogTime != num)
					{
						uCMainHein.SetValueLogTime(ucHein__BHYT, num);
					}
					currentLogTime = num;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
		{
			try
			{
				if (uCMainHein != null && ucHein__BHYT != null)
				{
					uCMainHein.ShowComboSoThe(ucHein__BHYT);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private ServiceDefaultPatyWorker GetServiceDefaultPatyWorker()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			try
			{
				if (serviceDefaultPatyWorker == null)
				{
					serviceDefaultPatyWorker = new ServiceDefaultPatyWorker();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return serviceDefaultPatyWorker;
		}

		private bool IsAllowEditPatientTypeByServiceConfig(V_HIS_SERE_SERV_4 item)
		{
			bool flag = true;
			try
			{
				if (item == null)
				{
					return true;
				}
				ServiceDefaultPatyWorker val = GetServiceDefaultPatyWorker();
				if (val == null || val.IsEmpty)
				{
					return true;
				}
				return val.IsAllowEditPatientType(item.SERVICE_ID, item.TDL_REQUEST_LOGINNAME);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return true;
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
			DevExpress.Utils.SerializableAppearanceObject appearance2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearance3 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered3 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed3 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled3 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearance4 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered4 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed4 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled4 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearance5 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered5 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed5 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled5 = new DevExpress.Utils.SerializableAppearanceObject();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
			this.chkAutoUpdateType = new DevExpress.XtraEditors.CheckEdit();
			this.barManager1 = new DevExpress.XtraBars.BarManager();
			this.bar2 = new DevExpress.XtraBars.Bar();
			this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
			this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
			this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
			this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
			this.txtWorkplace = new DevExpress.XtraEditors.TextEdit();
			this.cboWorkPlace = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit4View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.cboPosition = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit3View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.cboMilitaryRank = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit2View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.cboClassify = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.cboPrimaryPatientType = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.txtPrimaryPatientTypeCode = new DevExpress.XtraEditors.TextEdit();
			this.panelControlImageBHYT = new DevExpress.XtraEditors.PanelControl();
			this.dtLogTime = new DevExpress.XtraEditors.DateEdit();
			this.cboPatientType = new DevExpress.XtraEditors.LookUpEdit();
			this.cboTreatmentType = new DevExpress.XtraEditors.LookUpEdit();
			this.txtPatientType = new DevExpress.XtraEditors.TextEdit();
			this.txtTreatmentTypeCode = new DevExpress.XtraEditors.TextEdit();
			this.xclHeinCardInformation = new DevExpress.XtraEditors.XtraScrollableControl();
			this.btnSave = new DevExpress.XtraEditors.SimpleButton();
			this.txtQrcode = new DevExpress.XtraEditors.TextEdit();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciQrcode = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciPrimaryPatientType = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciComboPrimaryPatientType = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem12 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
			this.timerWaitOpenForm = new System.Windows.Forms.Timer();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.chkAutoUpdateType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtWorkplace.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboWorkPlace.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit4View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboPosition.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit3View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboMilitaryRank.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboClassify.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboPrimaryPatientType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtPrimaryPatientTypeCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.panelControlImageBHYT).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtLogTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtLogTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboPatientType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboTreatmentType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtPatientType.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtTreatmentTypeCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtQrcode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciQrcode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciPrimaryPatientType).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciComboPrimaryPatientType).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxErrorProvider1).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnPrint);
			this.layoutControl1.Controls.Add(this.chkAutoUpdateType);
			this.layoutControl1.Controls.Add(this.txtWorkplace);
			this.layoutControl1.Controls.Add(this.cboWorkPlace);
			this.layoutControl1.Controls.Add(this.cboPosition);
			this.layoutControl1.Controls.Add(this.cboMilitaryRank);
			this.layoutControl1.Controls.Add(this.cboClassify);
			this.layoutControl1.Controls.Add(this.cboPrimaryPatientType);
			this.layoutControl1.Controls.Add(this.txtPrimaryPatientTypeCode);
			this.layoutControl1.Controls.Add(this.panelControlImageBHYT);
			this.layoutControl1.Controls.Add(this.dtLogTime);
			this.layoutControl1.Controls.Add(this.cboPatientType);
			this.layoutControl1.Controls.Add(this.cboTreatmentType);
			this.layoutControl1.Controls.Add(this.txtPatientType);
			this.layoutControl1.Controls.Add(this.txtTreatmentTypeCode);
			this.layoutControl1.Controls.Add(this.xclHeinCardInformation);
			this.layoutControl1.Controls.Add(this.btnSave);
			this.layoutControl1.Controls.Add(this.txtQrcode);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 22);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(82, 38, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1371, 262);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnPrint.Enabled = false;
			this.btnPrint.Location = new System.Drawing.Point(926, 238);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(151, 22);
			this.btnPrint.StyleController = this.layoutControl1;
			this.btnPrint.TabIndex = 22;
			this.btnPrint.Text = "In giấy xác nhận BN CC";
			this.btnPrint.ToolTip = "In giấy xác nhận bệnh nhân cấp cứu";
			this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
			this.chkAutoUpdateType.Location = new System.Drawing.Point(769, 238);
			this.chkAutoUpdateType.MenuManager = this.barManager1;
			this.chkAutoUpdateType.Name = "chkAutoUpdateType";
			this.chkAutoUpdateType.Properties.Caption = "Tự động cập nhật ĐTTT";
			this.chkAutoUpdateType.Size = new System.Drawing.Size(153, 19);
			this.chkAutoUpdateType.StyleController = this.layoutControl1;
			this.chkAutoUpdateType.TabIndex = 21;
			this.chkAutoUpdateType.ToolTip = "Tự động cập nhật Đối tượng thanh toán, Đối tượng phụ thu";
			this.chkAutoUpdateType.CheckedChanged += new System.EventHandler(checkEdit1_CheckedChanged);
			this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[1] { this.bar2 });
			this.barManager1.DockControls.Add(this.barDockControlTop);
			this.barManager1.DockControls.Add(this.barDockControlBottom);
			this.barManager1.DockControls.Add(this.barDockControlLeft);
			this.barManager1.DockControls.Add(this.barDockControlRight);
			this.barManager1.Form = this;
			this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[2] { this.barButtonItem1, this.barButtonItem2 });
			this.barManager1.MainMenu = this.bar2;
			this.barManager1.MaxItemId = 2;
			this.bar2.BarName = "Main menu";
			this.bar2.DockCol = 0;
			this.bar2.DockRow = 0;
			this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
			this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[2]
			{
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
				new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)
			});
			this.bar2.OptionsBar.MultiLine = true;
			this.bar2.OptionsBar.UseWholeRow = true;
			this.bar2.Text = "Main menu";
			this.bar2.Visible = false;
			this.barButtonItem1.Caption = "Lưu(CtrlS)";
			this.barButtonItem1.Id = 0;
			this.barButtonItem1.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control);
			this.barButtonItem1.Name = "barButtonItem1";
			this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem1_ItemClick);
			this.barButtonItem2.Caption = "ShowPopup";
			this.barButtonItem2.Id = 1;
			this.barButtonItem2.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control);
			this.barButtonItem2.Name = "barButtonItem2";
			this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem2_ItemClick);
			this.barDockControlTop.CausesValidation = false;
			this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
			this.barDockControlTop.Size = new System.Drawing.Size(1371, 22);
			this.barDockControlBottom.CausesValidation = false;
			this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.barDockControlBottom.Location = new System.Drawing.Point(0, 284);
			this.barDockControlBottom.Size = new System.Drawing.Size(1371, 0);
			this.barDockControlLeft.CausesValidation = false;
			this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.barDockControlLeft.Location = new System.Drawing.Point(0, 22);
			this.barDockControlLeft.Size = new System.Drawing.Size(0, 262);
			this.barDockControlRight.CausesValidation = false;
			this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.barDockControlRight.Location = new System.Drawing.Point(1371, 22);
			this.barDockControlRight.Size = new System.Drawing.Size(0, 262);
			this.txtWorkplace.Location = new System.Drawing.Point(1151, 26);
			this.txtWorkplace.MenuManager = this.barManager1;
			this.txtWorkplace.Name = "txtWorkplace";
			this.txtWorkplace.Properties.NullValuePrompt = "Nơi làm việc (thông tin khác)";
			this.txtWorkplace.Properties.NullValuePromptShowForEmptyValue = true;
			this.txtWorkplace.Properties.ShowNullValuePromptWhenFocused = true;
			this.txtWorkplace.Size = new System.Drawing.Size(106, 20);
			this.txtWorkplace.StyleController = this.layoutControl1;
			this.txtWorkplace.TabIndex = 20;
			this.cboWorkPlace.Location = new System.Drawing.Point(942, 26);
			this.cboWorkPlace.MenuManager = this.barManager1;
			this.cboWorkPlace.Name = "cboWorkPlace";
			this.cboWorkPlace.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboWorkPlace.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[3]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "", null, null, true)
			});
			this.cboWorkPlace.Properties.NullText = "";
			this.cboWorkPlace.Properties.View = this.gridLookUpEdit4View;
			this.cboWorkPlace.Size = new System.Drawing.Size(205, 20);
			this.cboWorkPlace.StyleController = this.layoutControl1;
			this.cboWorkPlace.TabIndex = 19;
			this.cboWorkPlace.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboWorkPlace_ButtonClick);
			this.cboWorkPlace.EditValueChanged += new System.EventHandler(cboWorkPlace_EditValueChanged);
			this.gridLookUpEdit4View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit4View.Name = "gridLookUpEdit4View";
			this.gridLookUpEdit4View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit4View.OptionsView.ShowGroupPanel = false;
			this.cboPosition.Location = new System.Drawing.Point(639, 26);
			this.cboPosition.MenuManager = this.barManager1;
			this.cboPosition.Name = "cboPosition";
			this.cboPosition.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboPosition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance2, appearanceHovered2, appearancePressed2, appearanceDisabled2, "", null, null, true)
			});
			this.cboPosition.Properties.NullText = "";
			this.cboPosition.Properties.View = this.gridLookUpEdit3View;
			this.cboPosition.Size = new System.Drawing.Size(204, 20);
			this.cboPosition.StyleController = this.layoutControl1;
			this.cboPosition.TabIndex = 18;
			this.cboPosition.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboPosition_ButtonClick);
			this.cboPosition.EditValueChanged += new System.EventHandler(cboPosition_EditValueChanged);
			this.gridLookUpEdit3View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit3View.Name = "gridLookUpEdit3View";
			this.gridLookUpEdit3View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit3View.OptionsView.ShowGroupPanel = false;
			this.cboMilitaryRank.Location = new System.Drawing.Point(345, 26);
			this.cboMilitaryRank.MenuManager = this.barManager1;
			this.cboMilitaryRank.Name = "cboMilitaryRank";
			this.cboMilitaryRank.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboMilitaryRank.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance3, appearanceHovered3, appearancePressed3, appearanceDisabled3, "", null, null, true)
			});
			this.cboMilitaryRank.Properties.NullText = "";
			this.cboMilitaryRank.Properties.View = this.gridLookUpEdit2View;
			this.cboMilitaryRank.Size = new System.Drawing.Size(195, 20);
			this.cboMilitaryRank.StyleController = this.layoutControl1;
			this.cboMilitaryRank.TabIndex = 17;
			this.cboMilitaryRank.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboMilitaryRank_ButtonClick);
			this.cboMilitaryRank.EditValueChanged += new System.EventHandler(cboMilitaryRank_EditValueChanged);
			this.gridLookUpEdit2View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit2View.Name = "gridLookUpEdit2View";
			this.gridLookUpEdit2View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit2View.OptionsView.ShowGroupPanel = false;
			this.cboClassify.Location = new System.Drawing.Point(97, 26);
			this.cboClassify.MaximumSize = new System.Drawing.Size(149, 20);
			this.cboClassify.MenuManager = this.barManager1;
			this.cboClassify.Name = "cboClassify";
			this.cboClassify.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboClassify.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance4, appearanceHovered4, appearancePressed4, appearanceDisabled4, "", null, null, true)
			});
			this.cboClassify.Properties.NullText = "";
			this.cboClassify.Properties.View = this.gridView1;
			this.cboClassify.Size = new System.Drawing.Size(149, 20);
			this.cboClassify.StyleController = this.layoutControl1;
			this.cboClassify.TabIndex = 16;
			this.cboClassify.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboClassify_ButtonClick);
			this.cboClassify.EditValueChanged += new System.EventHandler(cboClassify_EditValueChanged);
			this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.cboPrimaryPatientType.Location = new System.Drawing.Point(1016, 2);
			this.cboPrimaryPatientType.MenuManager = this.barManager1;
			this.cboPrimaryPatientType.Name = "cboPrimaryPatientType";
			this.cboPrimaryPatientType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboPrimaryPatientType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance5, appearanceHovered5, appearancePressed5, appearanceDisabled5, "", null, null, true)
			});
			this.cboPrimaryPatientType.Properties.NullText = "";
			this.cboPrimaryPatientType.Properties.View = this.gridLookUpEdit1View;
			this.cboPrimaryPatientType.Size = new System.Drawing.Size(132, 20);
			this.cboPrimaryPatientType.StyleController = this.layoutControl1;
			this.cboPrimaryPatientType.TabIndex = 15;
			this.cboPrimaryPatientType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboPrimaryPatientType_Closed);
			this.cboPrimaryPatientType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboPrimaryPatientType_ButtonClick);
			this.cboPrimaryPatientType.EditValueChanged += new System.EventHandler(cboPrimaryPatientType_EditValueChanged);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.txtPrimaryPatientTypeCode.Location = new System.Drawing.Point(945, 2);
			this.txtPrimaryPatientTypeCode.MenuManager = this.barManager1;
			this.txtPrimaryPatientTypeCode.Name = "txtPrimaryPatientTypeCode";
			this.txtPrimaryPatientTypeCode.Size = new System.Drawing.Size(71, 20);
			this.txtPrimaryPatientTypeCode.StyleController = this.layoutControl1;
			this.txtPrimaryPatientTypeCode.TabIndex = 14;
			this.txtPrimaryPatientTypeCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtPrimaryPatientTypeCode_PreviewKeyDown);
			this.panelControlImageBHYT.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControlImageBHYT.Location = new System.Drawing.Point(1151, 135);
			this.panelControlImageBHYT.Name = "panelControlImageBHYT";
			this.panelControlImageBHYT.Size = new System.Drawing.Size(220, 127);
			this.panelControlImageBHYT.TabIndex = 13;
			this.dtLogTime.EditValue = null;
			this.dtLogTime.Location = new System.Drawing.Point(97, 2);
			this.dtLogTime.MenuManager = this.barManager1;
			this.dtLogTime.Name = "dtLogTime";
			this.dtLogTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtLogTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtLogTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.dtLogTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.dtLogTime.Properties.EditFormat.FormatString = "dd/MM/yyyy HH:mm";
			this.dtLogTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
			this.dtLogTime.Properties.Mask.EditMask = "dd/MM/yyyy HH:mm";
			this.dtLogTime.Size = new System.Drawing.Size(166, 20);
			this.dtLogTime.StyleController = this.layoutControl1;
			this.dtLogTime.TabIndex = 12;
			this.dtLogTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtLogTime_Closed);
			this.dtLogTime.Leave += new System.EventHandler(dtLogTime_Leave);
			this.cboPatientType.Location = new System.Drawing.Point(714, 2);
			this.cboPatientType.Name = "cboPatientType";
			this.cboPatientType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboPatientType.Properties.NullText = "";
			this.cboPatientType.Size = new System.Drawing.Size(132, 20);
			this.cboPatientType.StyleController = this.layoutControl1;
			this.cboPatientType.TabIndex = 11;
			this.cboPatientType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboPatientType_Closed);
			this.cboPatientType.EditValueChanged += new System.EventHandler(cboPatientType_EditValueChanged);
			this.cboTreatmentType.Location = new System.Drawing.Point(412, 2);
			this.cboTreatmentType.Name = "cboTreatmentType";
			this.cboTreatmentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboTreatmentType.Properties.NullText = "";
			this.cboTreatmentType.Size = new System.Drawing.Size(132, 20);
			this.cboTreatmentType.StyleController = this.layoutControl1;
			this.cboTreatmentType.TabIndex = 10;
			this.cboTreatmentType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboTreatmentType_Closed);
			this.cboTreatmentType.EditValueChanged += new System.EventHandler(cboTreatmentType_EditValueChanged);
			this.txtPatientType.Location = new System.Drawing.Point(643, 2);
			this.txtPatientType.Name = "txtPatientType";
			this.txtPatientType.Size = new System.Drawing.Size(71, 20);
			this.txtPatientType.StyleController = this.layoutControl1;
			this.txtPatientType.TabIndex = 9;
			this.txtPatientType.KeyDown += new System.Windows.Forms.KeyEventHandler(txtPatientType_KeyDown_1);
			this.txtTreatmentTypeCode.Location = new System.Drawing.Point(362, 2);
			this.txtTreatmentTypeCode.MaximumSize = new System.Drawing.Size(50, 0);
			this.txtTreatmentTypeCode.Name = "txtTreatmentTypeCode";
			this.txtTreatmentTypeCode.Size = new System.Drawing.Size(50, 20);
			this.txtTreatmentTypeCode.StyleController = this.layoutControl1;
			this.txtTreatmentTypeCode.TabIndex = 8;
			this.txtTreatmentTypeCode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtTreatmentTypeCode_KeyDown);
			this.xclHeinCardInformation.Location = new System.Drawing.Point(2, 50);
			this.xclHeinCardInformation.Name = "xclHeinCardInformation";
			this.xclHeinCardInformation.Size = new System.Drawing.Size(1367, 83);
			this.xclHeinCardInformation.TabIndex = 7;
			this.btnSave.Location = new System.Drawing.Point(1081, 238);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(68, 22);
			this.btnSave.StyleController = this.layoutControl1;
			this.btnSave.TabIndex = 6;
			this.btnSave.Text = "Lưu (Ctrl S)";
			this.btnSave.Click += new System.EventHandler(btnSave_Click);
			this.txtQrcode.EditValue = "";
			this.txtQrcode.Location = new System.Drawing.Point(1245, 2);
			this.txtQrcode.MenuManager = this.barManager1;
			this.txtQrcode.Name = "txtQrcode";
			this.txtQrcode.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
			this.txtQrcode.Properties.NullValuePrompt = "Quẹt mã QR (trên thẻ BHYT hoặc CCCD) để lấy thông tin BHYT";
			this.txtQrcode.Properties.NullValuePromptShowForEmptyValue = true;
			this.txtQrcode.Size = new System.Drawing.Size(124, 20);
			this.txtQrcode.StyleController = this.layoutControl1;
			this.txtQrcode.TabIndex = 12;
			this.txtQrcode.KeyDown += new System.Windows.Forms.KeyEventHandler(txtQrcode_KeyDown);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[21]
			{
				this.layoutControlItem4, this.emptySpaceItem1, this.layoutControlItem1, this.layoutControlItem5, this.layoutControlItem6, this.layoutControlItem7, this.layoutControlItem2, this.layoutControlItem3, this.emptySpaceItem3, this.layoutControlItem8,
				this.lciQrcode, this.lciPrimaryPatientType, this.lciComboPrimaryPatientType, this.layoutControlItem9, this.layoutControlItem10, this.layoutControlItem11, this.layoutControlItem12, this.layoutControlItem13, this.emptySpaceItem2, this.layoutControlItem14,
				this.layoutControlItem15
			});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1371, 262);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem4.Control = this.xclHeinCardInformation;
			this.layoutControlItem4.Location = new System.Drawing.Point(0, 48);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(1371, 87);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.emptySpaceItem1.AllowHotTrack = false;
			this.emptySpaceItem1.Location = new System.Drawing.Point(0, 236);
			this.emptySpaceItem1.Name = "emptySpaceItem1";
			this.emptySpaceItem1.Size = new System.Drawing.Size(767, 26);
			this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem1.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem1.Control = this.txtTreatmentTypeCode;
			this.layoutControlItem1.Location = new System.Drawing.Point(265, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem1.Size = new System.Drawing.Size(147, 24);
			this.layoutControlItem1.Text = "Loại diện ĐT:";
			this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem1.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem1.TextToControlDistance = 5;
			this.layoutControlItem5.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem5.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem5.Control = this.txtPatientType;
			this.layoutControlItem5.Location = new System.Drawing.Point(546, 0);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.layoutControlItem5.Size = new System.Drawing.Size(168, 24);
			this.layoutControlItem5.Text = "Đối tượng:";
			this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem5.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem5.TextToControlDistance = 5;
			this.layoutControlItem6.Control = this.cboTreatmentType;
			this.layoutControlItem6.Location = new System.Drawing.Point(412, 0);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem6.Size = new System.Drawing.Size(134, 24);
			this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem6.TextVisible = false;
			this.layoutControlItem7.Control = this.cboPatientType;
			this.layoutControlItem7.Location = new System.Drawing.Point(714, 0);
			this.layoutControlItem7.Name = "layoutControlItem7";
			this.layoutControlItem7.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.layoutControlItem7.Size = new System.Drawing.Size(134, 24);
			this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem7.TextVisible = false;
			this.layoutControlItem2.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem2.Control = this.dtLogTime;
			this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(265, 24);
			this.layoutControlItem2.Text = "Thời gian:";
			this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem2.TextToControlDistance = 5;
			this.layoutControlItem3.Control = this.btnSave;
			this.layoutControlItem3.Location = new System.Drawing.Point(1079, 236);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(72, 26);
			this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem3.TextVisible = false;
			this.emptySpaceItem3.AllowHotTrack = false;
			this.emptySpaceItem3.Location = new System.Drawing.Point(0, 135);
			this.emptySpaceItem3.Name = "emptySpaceItem3";
			this.emptySpaceItem3.Size = new System.Drawing.Size(1151, 101);
			this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem8.Control = this.panelControlImageBHYT;
			this.layoutControlItem8.Location = new System.Drawing.Point(1151, 135);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlItem8.Size = new System.Drawing.Size(220, 127);
			this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem8.TextVisible = false;
			this.lciQrcode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciQrcode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciQrcode.Control = this.txtQrcode;
			this.lciQrcode.Location = new System.Drawing.Point(1150, 0);
			this.lciQrcode.Name = "lciQrcode";
			this.lciQrcode.Size = new System.Drawing.Size(221, 24);
			this.lciQrcode.Text = "Mã QR:";
			this.lciQrcode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciQrcode.TextSize = new System.Drawing.Size(88, 20);
			this.lciQrcode.TextToControlDistance = 5;
			this.lciPrimaryPatientType.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciPrimaryPatientType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciPrimaryPatientType.Control = this.txtPrimaryPatientTypeCode;
			this.lciPrimaryPatientType.Location = new System.Drawing.Point(848, 0);
			this.lciPrimaryPatientType.Name = "lciPrimaryPatientType";
			this.lciPrimaryPatientType.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lciPrimaryPatientType.Size = new System.Drawing.Size(168, 24);
			this.lciPrimaryPatientType.Text = "ĐT phụ thu:";
			this.lciPrimaryPatientType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciPrimaryPatientType.TextSize = new System.Drawing.Size(90, 20);
			this.lciPrimaryPatientType.TextToControlDistance = 5;
			this.lciPrimaryPatientType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.lciComboPrimaryPatientType.Control = this.cboPrimaryPatientType;
			this.lciComboPrimaryPatientType.Location = new System.Drawing.Point(1016, 0);
			this.lciComboPrimaryPatientType.Name = "lciComboPrimaryPatientType";
			this.lciComboPrimaryPatientType.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciComboPrimaryPatientType.Size = new System.Drawing.Size(134, 24);
			this.lciComboPrimaryPatientType.TextSize = new System.Drawing.Size(0, 0);
			this.lciComboPrimaryPatientType.TextVisible = false;
			this.lciComboPrimaryPatientType.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.layoutControlItem9.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem9.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem9.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem9.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem9.Control = this.cboClassify;
			this.layoutControlItem9.Location = new System.Drawing.Point(0, 24);
			this.layoutControlItem9.Name = "layoutControlItem9";
			this.layoutControlItem9.OptionsToolTip.ToolTip = "Đối tượng chi tiết";
			this.layoutControlItem9.Size = new System.Drawing.Size(248, 24);
			this.layoutControlItem9.Text = "ĐT chi tiết:";
			this.layoutControlItem9.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem9.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem9.TextToControlDistance = 5;
			this.layoutControlItem10.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem10.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem10.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem10.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem10.Control = this.cboMilitaryRank;
			this.layoutControlItem10.Location = new System.Drawing.Point(248, 24);
			this.layoutControlItem10.Name = "layoutControlItem10";
			this.layoutControlItem10.Size = new System.Drawing.Size(294, 24);
			this.layoutControlItem10.Text = "Quân hàm:";
			this.layoutControlItem10.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem10.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem10.TextToControlDistance = 5;
			this.layoutControlItem10.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.layoutControlItem11.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem11.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem11.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem11.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem11.Control = this.cboPosition;
			this.layoutControlItem11.Location = new System.Drawing.Point(542, 24);
			this.layoutControlItem11.Name = "layoutControlItem11";
			this.layoutControlItem11.Size = new System.Drawing.Size(303, 24);
			this.layoutControlItem11.Text = "Chức vụ:";
			this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem11.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem11.TextToControlDistance = 5;
			this.layoutControlItem11.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.layoutControlItem12.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.layoutControlItem12.AppearanceItemCaption.Options.UseForeColor = true;
			this.layoutControlItem12.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem12.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem12.Control = this.cboWorkPlace;
			this.layoutControlItem12.Location = new System.Drawing.Point(845, 24);
			this.layoutControlItem12.Name = "layoutControlItem12";
			this.layoutControlItem12.Size = new System.Drawing.Size(304, 24);
			this.layoutControlItem12.Text = "Nơi làm việc:";
			this.layoutControlItem12.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem12.TextSize = new System.Drawing.Size(90, 20);
			this.layoutControlItem12.TextToControlDistance = 5;
			this.layoutControlItem12.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.layoutControlItem13.Control = this.txtWorkplace;
			this.layoutControlItem13.Location = new System.Drawing.Point(1149, 24);
			this.layoutControlItem13.Name = "layoutControlItem13";
			this.layoutControlItem13.Size = new System.Drawing.Size(110, 24);
			this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem13.TextVisible = false;
			this.layoutControlItem13.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(1259, 24);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(112, 24);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem14.Control = this.chkAutoUpdateType;
			this.layoutControlItem14.Location = new System.Drawing.Point(767, 236);
			this.layoutControlItem14.Name = "layoutControlItem14";
			this.layoutControlItem14.Size = new System.Drawing.Size(157, 26);
			this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem14.TextVisible = false;
			this.layoutControlItem15.Control = this.btnPrint;
			this.layoutControlItem15.Location = new System.Drawing.Point(924, 236);
			this.layoutControlItem15.Name = "layoutControlItem15";
			this.layoutControlItem15.Size = new System.Drawing.Size(155, 26);
			this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem15.TextVisible = false;
			this.dxErrorProvider1.ContainerControl = this;
			this.timerWaitOpenForm.Interval = 2000;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1371, 284);
			base.Controls.Add(this.layoutControl1);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			base.Name = "frmPatientTypeAlter";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Đối tượng điều trị";
			base.Load += new System.EventHandler(frmPatientTypeAlter_Load);
			base.Controls.SetChildIndex(this.barDockControlTop, 0);
			base.Controls.SetChildIndex(this.barDockControlBottom, 0);
			base.Controls.SetChildIndex(this.barDockControlRight, 0);
			base.Controls.SetChildIndex(this.barDockControlLeft, 0);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.chkAutoUpdateType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.barManager1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtWorkplace.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboWorkPlace.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit4View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboPosition.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit3View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboMilitaryRank.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit2View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboClassify.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboPrimaryPatientType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtPrimaryPatientTypeCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.panelControlImageBHYT).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtLogTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtLogTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboPatientType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboTreatmentType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtPatientType.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtTreatmentTypeCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtQrcode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciQrcode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciPrimaryPatientType).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciComboPrimaryPatientType).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem9).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem10).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem11).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem12).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxErrorProvider1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
