using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using His.Bhyt.InsuranceExpertise;
using His.Bhyt.InsuranceExpertise.LDO;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.Common;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.Location;
using HIS.Desktop.ModuleExt;
using HIS.Desktop.Plugins.CheckInfoBHYT.ADO;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using HIS.Desktop.Plugins.Library.RegisterConfig;
using HIS.Desktop.Utility;
using Inventec.Common.Adapter;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Message;
using Inventec.Desktop.Common.Modules;
using Inventec.UC.Login.Base;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.SDO;

namespace HIS.Desktop.Plugins.CheckInfoBHYT
{
	public class frmCheckInfoBHYT : FormBase
	{
		public class GenderConvert
		{
			public static string TextToNumber(string ge)
			{
				return (ge == "Nữ") ? "2" : "1";
			}

			public static string HisToHein(string ge)
			{
				return (ge == "1") ? "2" : "1";
			}

			public static long HeinToHisNumber(string ge)
			{
				return (ge == "1") ? 2 : 1;
			}
		}

		private Module currentModule = null;

		private DelegateRefreshData _dlg = null;

		private CheckInfoBhytADO checkInfoBhytADO = null;

		private long _TreatmentId;

		private HIS_EMPLOYEE currentEmployee = null;

		private Dictionary<string, HIS_MEDI_ORG> dicMediOrg;

		private string CONFIG_KEY__PATIENT_TYPE_CODE__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";

		private List<string> connectInfors = new List<string>();

		private string api = "";

		private string nameCb = "";

		private string cccdCb = "";

		private string apiv2 = "";

		private IContainer components = null;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private GridControl gridControlBHYT;

		private GridView gridViewBHYT;

		private GridColumn gridColumn1;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private GridColumn gridColumn4;

		private GridColumn gridColumn5;

		private GridColumn gridColumn6;

		private LayoutControlItem layoutControlItem1;

		private GridControl gridControlHistoryExam;

		private GridView gridViewHistoryExam;

		private GridColumn gridColumn7;

		private GridColumn gridColumn8;

		private GridColumn gridColumn9;

		private GridColumn gridColumn10;

		private LabelControl lblMessenger;

		private SimpleButton btnUpdateBHYT;

		private SimpleButton btnUpdatePatient;

		private MemoEdit txtViewInfoCheck;

		private GridColumn gridColumn12;

		private GridColumn gridColumn13;

		private GridColumn gridColumn14;

		private LayoutControlItem layoutControlItem13;

		private LayoutControlItem layoutControlItem14;

		private LayoutControlItem layoutControlItem15;

		private LayoutControlItem layoutControlItem16;

		private LayoutControlItem layoutControlItem17;

		private GridColumn gridColumn18;

		private GridColumn gridColumn15;

		private GridColumn gridColumn16;

		private GridColumn gridColumn20;

		private GridColumn maHoSo;

		private GridColumn gridColumn19;

		private GridColumn gridColumn17;

		private SimpleButton btnPrintScreen;

		private LayoutControlItem layoutControlItem2;

		private SaveFileDialog saveFileDialog1;

		private HIS_PATIENT _HisPatient { get; set; }

		private HIS_TREATMENT _HisTreatment { get; set; }

		private ResultDataADO rsDataBHYT { get; set; }

		private ResultHistoryLDO _ResultHistoryLDO { get; set; }

		private HIS_PATIENT_TYPE_ALTER _PatientTypeAlter { get; set; }

		private async Task CheckTTFull(V_HIS_PATIENT_TYPE_ALTER _patientTypeAlter, string nameCb, string cccdCb, string api, string newapi)
		{
			rsDataBHYT = new ResultDataADO();
			try
			{
				LogSystem.Debug(string.Format("Tên cán bộ:{0}", nameCb));
				LogSystem.Debug(string.Format("CCCD cán bộ:{0}", cccdCb));
				LogSystem.Debug(string.Format("Tên api:{0}", api));
				LogSystem.Debug(string.Format("Tên api 2:{0}", newapi));
				ApiInsuranceExpertise apiInsuranceExpertise = new ApiInsuranceExpertise
				{
					ApiEgw = api
				};
				CheckHistoryLDO checkHistoryLDO = new CheckHistoryLDO
				{
					maThe = _patientTypeAlter.HEIN_CARD_NUMBER,
					ngaySinh = Inventec.Common.DateTime.Convert.TimeNumberToDateString(_HisPatient.DOB),
					hoTen = _HisPatient.VIR_PATIENT_NAME,
					cccdCb = cccdCb,
					hoTenCb = nameCb
				};
				if (!string.IsNullOrEmpty(BHXHLoginCFG.USERNAME) || !string.IsNullOrEmpty(BHXHLoginCFG.PASSWORD) || !string.IsNullOrEmpty(BHXHLoginCFG.ADDRESS))
				{
					ResultDataADO resultDataADO = rsDataBHYT;
					resultDataADO.ResultHistoryLDO = await apiInsuranceExpertise.CheckHistory(BHXHLoginCFG.USERNAME, BHXHLoginCFG.PASSWORD, BHXHLoginCFG.ADDRESS, checkHistoryLDO, BHXHLoginCFG.ADDRESS_OPTION);
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => rsDataBHYT.ResultHistoryLDO), rsDataBHYT.ResultHistoryLDO));
					if (HisConfigCFG.WarningInvalidCheckHistoryHeinCard && rsDataBHYT.ResultHistoryLDO.dsLichSuKT2018 != null && rsDataBHYT.ResultHistoryLDO.dsLichSuKT2018.Count > 0)
					{
						long maxNgayRa = 0L;
						if (rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018 != null)
						{
							foreach (ExamHistoryLDO p in rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018)
							{
								long ngayRa;
								if (long.TryParse(p.ngayRa, out ngayRa) && ngayRa > maxNgayRa)
								{
									maxNgayRa = ngayRa;
								}
							}
						}
						List<object> otherChecks = new List<object>();
						foreach (dsLichSuKT2018 o in rsDataBHYT.ResultHistoryLDO.dsLichSuKT2018)
						{
							long thoiGianKT;
							if (long.TryParse(o.thoiGianKT, out thoiGianKT))
							{
								string userKT = o.userKT ?? "";
								string currentMediOrgCode = BranchDataWorker.Branch.HEIN_MEDI_ORG_CODE;
								if (thoiGianKT > maxNgayRa && !string.IsNullOrEmpty(userKT) && userKT.IndexOf(currentMediOrgCode) < 0 && (o.maLoi == "000" || o.maLoi == "001" || o.maLoi == "002" || o.maLoi == "003"))
								{
									otherChecks.Add(o);
								}
							}
						}
						Dictionary<string, dsLichSuKT2018> grouped = new Dictionary<string, dsLichSuKT2018>();
						long thoiGianKT2 = default(long);
						foreach (dynamic check in otherChecks)
						{
							string userKT2 = check.userKT ?? "";
							if (!((!long.TryParse(check.thoiGianKT, out thoiGianKT2)) ? true : false) && (!grouped.ContainsKey(userKT2) || long.Parse(grouped[userKT2].thoiGianKT) < thoiGianKT2))
							{
								grouped[userKT2] = check;
							}
						}
						List<dsLichSuKT2018> filteredChecks = new List<dsLichSuKT2018>(grouped.Values);
						if (filteredChecks.Count > 0)
						{
							List<HIS_MEDI_ORG> mediOrgs = BackendDataWorker.Get<HIS_MEDI_ORG>().ToList();
							List<string> errorDetails = new List<string>();
							foreach (dsLichSuKT2018 check2 in filteredChecks)
							{
								string userKT3 = check2.userKT ?? "";
								string mediOrgCode = ((userKT3.Length >= 5) ? userKT3.Substring(0, 5) : userKT3);
								HIS_MEDI_ORG mediOrg = mediOrgs.FirstOrDefault((HIS_MEDI_ORG m) => m.MEDI_ORG_CODE == mediOrgCode);
								long thoiGianKT3;
								long.TryParse(check2.thoiGianKT, out thoiGianKT3);
								string timeStrRaw = thoiGianKT3.ToString();
								string timeStr = timeStrRaw;
								if (timeStrRaw.Length == 12)
								{
									timeStr = DateTime.ParseExact(timeStrRaw, "yyyyMMddHHmm", null).ToString("dd/MM/yyyy HH:mm");
								}
								else if (timeStrRaw.Length == 14)
								{
									timeStr = DateTime.ParseExact(timeStrRaw, "yyyyMMddHHmmss", null).ToString("dd/MM/yyyy HH:mm");
								}
								string detail = ((mediOrg == null || string.IsNullOrEmpty(mediOrg.MEDI_ORG_NAME)) ? string.Format("Tài khoản {0} [{1}]", userKT3, timeStr) : string.Format("{0} ({1}) [{2}]", mediOrg.MEDI_ORG_NAME, mediOrg.MEDI_ORG_CODE, timeStr));
								errorDetails.Add(detail);
							}
							List<ExamHistoryLDO> mappedList = (from x in filteredChecks.Select(delegate(dsLichSuKT2018 dsLichSuKT2019)
								{
									string maCSKCB = ((dsLichSuKT2019.userKT != null && dsLichSuKT2019.userKT.Length >= 5) ? dsLichSuKT2019.userKT.Substring(0, 5) : null);
									return new ExamHistoryLDO
									{
										maCSKCB = maCSKCB,
										ngayVao = dsLichSuKT2019.thoiGianKT,
										ngayRa = null,
										tinhTrang = "5"
									};
								})
								orderby x.ngayVao descending
								select x).ToList();
							List<ExamHistoryLDO> oldList = rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018 ?? new List<ExamHistoryLDO>();
							List<ExamHistoryLDO> newList = mappedList.Concat(oldList).ToList();
							rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018 = newList;
							rsDataBHYT.ResultHistoryLDO.message = "Thẻ BHYT có thông tin kiểm tra thẻ tại " + string.Join("; ", errorDetails.ToArray());
							rsDataBHYT.ResultHistoryLDO.maKetQua = "8888";
							return;
						}
					}
					if (rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018 == null)
					{
						LogSystem.Debug("Khong co lich su KCB 2018, kiem tra voi api moi");
						ResultHistoryLDO rsIns2 = new ResultHistoryLDO();
						if (!string.IsNullOrEmpty(BHXHLoginCFG.USERNAME) || !string.IsNullOrEmpty(BHXHLoginCFG.PASSWORD) || !string.IsNullOrEmpty(BHXHLoginCFG.ADDRESS))
						{
							apiInsuranceExpertise.ApiEgw = newapi;
							rsIns2 = await apiInsuranceExpertise.CheckHistory(BHXHLoginCFG.USERNAME, BHXHLoginCFG.PASSWORD, BHXHLoginCFG.ADDRESS, checkHistoryLDO, BHXHLoginCFG.ADDRESS_OPTION);
							LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => rsIns2), rsIns2));
						}
						else
						{
							LogSystem.Error("Kiem tra lai cau hinh 'HIS.CHECK_HEIN_CARD.BHXH.LOGIN.USER_PASS'  -- 'HIS.CHECK_HEIN_CARD.BHXH__ADDRESS' ==>BHYT");
							rsIns2 = null;
						}
						if (rsIns2 != null && rsIns2.dsLichSuKCB2025 != null && rsIns2.dsLichSuKCB2025.Count > 0 && rsIns2.success && string.IsNullOrEmpty(rsIns2.message))
						{
							rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018 = new List<ExamHistoryLDO>();
							foreach (ExamHistoryLDO item in rsIns2.dsLichSuKCB2025)
							{
								rsDataBHYT.ResultHistoryLDO.dsLichSuKCB2018.Add(item);
							}
						}
					}
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => rsDataBHYT.ResultHistoryLDO), rsDataBHYT.ResultHistoryLDO));
				}
				else
				{
					LogSystem.Error("Kiem tra lai cau hinh 'HIS.CHECK_HEIN_CARD.BHXH.LOGIN.USER_PASS'  -- 'HIS.CHECK_HEIN_CARD.BHXH__ADDRESS' ==>BHYT");
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				rsDataBHYT = null;
				LogSystem.Warn(ex2);
			}
		}

		public frmCheckInfoBHYT(Module module)
			: base(module)
		{
			InitializeComponent();
			try
			{
				SetIcon();
				currentModule = module;
				if (currentModule != null)
				{
					Text = currentModule.text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public frmCheckInfoBHYT(Module module, long _treatmentId)
			: base(module)
		{
			InitializeComponent();
			try
			{
				SetIcon();
				currentModule = module;
				_TreatmentId = _treatmentId;
				if (currentModule != null)
				{
					Text = currentModule.text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public frmCheckInfoBHYT(Module module, CheckInfoBhytADO _ado)
			: base(module)
		{
			InitializeComponent();
			try
			{
				SetIcon();
				currentModule = module;
				checkInfoBhytADO = _ado;
				if (currentModule != null)
				{
					Text = currentModule.text;
				}
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

		private void frmCheckInfoBHYT_Load(object sender, EventArgs e)
		{
			try
			{
				WaitingManager.Show();
				BHXHLoginCFG.LoadConfig();
				checkConfig();
				CheckEmploy();
				dicMediOrg = BackendDataWorker.Get<HIS_MEDI_ORG>().ToDictionary((HIS_MEDI_ORG o) => o.MEDI_ORG_CODE, (HIS_MEDI_ORG o) => o);
				LoadHisTreatment();
				LoadHisPatient();
				LoadPatientTypeAlter();
				SetEnableControl();
				WaitingManager.Hide();
			}
			catch (Exception ex)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex);
			}
		}

		private void SetEnableControl()
		{
			try
			{
				long treatmentId = _TreatmentId;
				if (_TreatmentId == 0)
				{
					btnUpdatePatient.Enabled = false;
					btnUpdateBHYT.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadHisPatient()
		{
			try
			{
				if (_HisTreatment != null)
				{
					_HisPatient = new HIS_PATIENT();
					HisPatientFilter hisPatientFilter = new HisPatientFilter();
					hisPatientFilter.ID = _HisTreatment.PATIENT_ID;
					List<HIS_PATIENT> list = new BackendAdapter(null).Get<List<HIS_PATIENT>>("api/HisPatient/Get", ApiConsumers.MosConsumer, hisPatientFilter, null);
					if (list != null && list.Count > 0)
					{
						_HisPatient = list.FirstOrDefault();
					}
				}
				else if (checkInfoBhytADO != null)
				{
					_HisPatient = new HIS_PATIENT();
					_HisPatient.DOB = checkInfoBhytADO.TDL_DOB;
					_HisPatient.VIR_PATIENT_NAME = checkInfoBhytADO.TDL_PATIENT_NAME;
					_HisPatient.GENDER_ID = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER p) => p.GENDER_NAME.Equals(checkInfoBhytADO.TDL_GENDER_NAME)).ID;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadHisTreatment()
		{
			try
			{
				long treatmentId = _TreatmentId;
				if (_TreatmentId > 0)
				{
					_HisTreatment = new HIS_TREATMENT();
					HisTreatmentFilter hisTreatmentFilter = new HisTreatmentFilter();
					hisTreatmentFilter.ID = _TreatmentId;
					List<HIS_TREATMENT> list = new BackendAdapter(null).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", ApiConsumers.MosConsumer, hisTreatmentFilter, null);
					if (list != null && list.Count > 0)
					{
						_HisTreatment = list.FirstOrDefault();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void checkConfig()
		{
			try
			{
				HisConfigCHECKHEINCARD.LoadConfig();
				string cHECK_HEIN_CARD_BHXH__API = HisConfigCHECKHEINCARD.CHECK_HEIN_CARD_BHXH__API;
				if (!string.IsNullOrEmpty(cHECK_HEIN_CARD_BHXH__API))
				{
					connectInfors = cHECK_HEIN_CARD_BHXH__API.Split('|').ToList();
					api = connectInfors[0];
					nameCb = connectInfors[1];
					cccdCb = connectInfors[2];
					if (connectInfors.Count > 3)
					{
						apiv2 = connectInfors[3];
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		public void CheckEmploy()
		{
			try
			{
				if (currentEmployee == null)
				{
					CommonParam commonParam = new CommonParam();
					HisEmployeeFilter hisEmployeeFilter = new HisEmployeeFilter();
					hisEmployeeFilter.LOGINNAME__EXACT = ClientTokenManagerStore.ClientTokenManager.GetLoginName();
					currentEmployee = new BackendAdapter(commonParam).Get<List<HIS_EMPLOYEE>>("api/HisEmployee/Get", ApiConsumers.MosConsumer, hisEmployeeFilter, commonParam).FirstOrDefault();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private async void LoadPatientTypeAlter()
		{
			try
			{
				string name = ((!string.IsNullOrEmpty(nameCb)) ? nameCb : currentEmployee.TDL_USERNAME);
				string cccd = ((!string.IsNullOrEmpty(cccdCb)) ? cccdCb : currentEmployee.IDENTIFICATION_NUMBER);
				long treatmentId = _TreatmentId;
				if (_TreatmentId > 0)
				{
					gridControlBHYT.DataSource = null;
					List<PatientTypeAlterADO> _PatientTypeAlterADOs = new List<PatientTypeAlterADO>();
					HisPatientTypeAlterViewFilter filter = new HisPatientTypeAlterViewFilter
					{
						TREATMENT_ID = _TreatmentId
					};
					string key = HisConfigs.Get<string>(CONFIG_KEY__PATIENT_TYPE_CODE__BHYT);
					HIS_PATIENT_TYPE patientType = BackendDataWorker.Get<HIS_PATIENT_TYPE>().FirstOrDefault((HIS_PATIENT_TYPE o) => o.PATIENT_TYPE_CODE == key.Trim());
					if (patientType != null)
					{
						filter.PATIENT_TYPE_ID = patientType.ID;
						filter.ORDER_DIRECTION = "ASC";
						filter.ORDER_FIELD = "LOG_TIME";
						List<V_HIS_PATIENT_TYPE_ALTER> datas = new BackendAdapter(null).Get<List<V_HIS_PATIENT_TYPE_ALTER>>("api/HisPatientTypeAlter/GetView", ApiConsumers.MosConsumer, filter, null);
						if (datas != null && datas.Count > 0)
						{
							foreach (V_HIS_PATIENT_TYPE_ALTER item in datas)
							{
								PatientTypeAlterADO ado = new PatientTypeAlterADO(item);
								await CheckTTFull(item, name, cccd, api, apiv2);
								if (rsDataBHYT != null)
								{
									ado.ResultDataADO = rsDataBHYT;
								}
								_PatientTypeAlterADOs.Add(ado);
							}
						}
						gridControlBHYT.BeginUpdate();
						gridControlBHYT.DataSource = _PatientTypeAlterADOs;
						gridControlBHYT.EndUpdate();
						Process(_PatientTypeAlterADOs.FirstOrDefault());
					}
					else
					{
						LogSystem.Error("MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT :   null");
					}
				}
				else if (checkInfoBhytADO != null)
				{
					V_HIS_PATIENT_TYPE_ALTER patientTypeAlert = new V_HIS_PATIENT_TYPE_ALTER
					{
						HEIN_CARD_NUMBER = checkInfoBhytADO.TDL_HEIN_CARD_NUMBER
					};
					PatientTypeAlterADO ado2 = new PatientTypeAlterADO(patientTypeAlert);
					await CheckTTFull(patientTypeAlert, name, cccd, api, apiv2);
					if (rsDataBHYT != null)
					{
						ado2.ResultDataADO = rsDataBHYT;
					}
					gridControlBHYT.BeginUpdate();
					gridControlBHYT.EndUpdate();
					Process(ado2);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				LogSystem.Error(ex2);
			}
		}

		private void gridViewBHYT_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				PatientTypeAlterADO patientTypeAlterADO = (PatientTypeAlterADO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (patientTypeAlterADO == null)
				{
					return;
				}
				if (e.Column.FieldName == "STT")
				{
					e.Value = e.ListSourceRowIndex + 1;
				}
				else if (e.Column.FieldName == "HEIN_CARD_FROM_TIME_STR")
				{
					e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlterADO.HEIN_CARD_FROM_TIME.GetValueOrDefault());
				}
				else if (e.Column.FieldName == "HEIN_CARD_TO_TIME_STR")
				{
					e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlterADO.HEIN_CARD_TO_TIME.GetValueOrDefault());
				}
				else if (e.Column.FieldName == "JOIN_5_YEAR_STR")
				{
					if (!string.IsNullOrEmpty(patientTypeAlterADO.JOIN_5_YEAR) && patientTypeAlterADO.JOIN_5_YEAR == "C")
					{
						e.Value = "X";
					}
					else
					{
						e.Value = "";
					}
				}
				else if (e.Column.FieldName == "PAID_6_MONTH_STR")
				{
					if (!string.IsNullOrEmpty(patientTypeAlterADO.PAID_6_MONTH) && patientTypeAlterADO.PAID_6_MONTH == "C")
					{
						e.Value = "X";
					}
					else
					{
						e.Value = "";
					}
				}
				else if (e.Column.FieldName == "JOIN_5_YEAR_TIME_STR")
				{
					if (patientTypeAlterADO.JOIN_5_YEAR_TIME.HasValue)
					{
						e.Value = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlterADO.JOIN_5_YEAR_TIME.GetValueOrDefault());
					}
					else
					{
						e.Value = "";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void gridViewBHYT_RowCellClick(object sender, RowCellClickEventArgs e)
		{
			try
			{
				PatientTypeAlterADO patientTypeAlterADO = (PatientTypeAlterADO)gridViewBHYT.GetFocusedRow();
				if (patientTypeAlterADO != null)
				{
					Process(patientTypeAlterADO);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void Process(PatientTypeAlterADO data)
		{
			try
			{
				_ResultHistoryLDO = null;
				_PatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				if (data.ResultDataADO == null || data.ResultDataADO.ResultHistoryLDO == null)
				{
					return;
				}
				if (data.ResultDataADO.ResultHistoryLDO.success)
				{
					bool flag = true;
					bool flag2 = true;
					flag = flag && _HisPatient.VIR_PATIENT_NAME.ToLower().Trim() == data.ResultDataADO.ResultHistoryLDO.hoTen.ToLower().Trim();
					flag = ((data.ResultDataADO.ResultHistoryLDO.ngaySinh.Length != 4) ? (flag && Inventec.Common.DateTime.Convert.TimeNumberToDateString(_HisPatient.DOB) == data.ResultDataADO.ResultHistoryLDO.ngaySinh) : (flag && _HisPatient.DOB.ToString().Substring(0, 4) == data.ResultDataADO.ResultHistoryLDO.ngaySinh));
					HIS_GENDER hIS_GENDER = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER p) => p.ID == _HisPatient.GENDER_ID);
					flag = flag && hIS_GENDER != null && hIS_GENDER.GENDER_NAME == data.ResultDataADO.ResultHistoryLDO.gioiTinh;
					flag2 = flag2 && Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.HEIN_CARD_FROM_TIME.GetValueOrDefault()) == data.ResultDataADO.ResultHistoryLDO.gtTheTu && Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.HEIN_CARD_TO_TIME.GetValueOrDefault()) == data.ResultDataADO.ResultHistoryLDO.gtTheDen && data.HEIN_MEDI_ORG_CODE == data.ResultDataADO.ResultHistoryLDO.maDKBD && data.ADDRESS == data.ResultDataADO.ResultHistoryLDO.diaChi && data.HEIN_CARD_NUMBER == data.ResultDataADO.ResultHistoryLDO.maThe && (string.IsNullOrEmpty(data.ResultDataADO.ResultHistoryLDO.ngayDu5Nam) || Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.JOIN_5_YEAR_TIME.GetValueOrDefault()) == data.ResultDataADO.ResultHistoryLDO.ngayDu5Nam);
					btnUpdateBHYT.Enabled = !flag2;
					btnUpdatePatient.Enabled = !flag;
					if (!flag2 || !flag)
					{
						lblMessenger.Text = "Có sự sai khác thông tin thẻ và thông tin bệnh nhân trên cổng Bảo hiểm y tế. Nhấn nút bên cạnh để cập nhập lại thông tin.";
					}
					else
					{
						lblMessenger.Text = "";
					}
					long treatmentId = _TreatmentId;
					if (_TreatmentId == 0)
					{
						btnUpdateBHYT.Enabled = false;
						btnUpdatePatient.Enabled = false;
						lblMessenger.Text = "";
					}
					_ResultHistoryLDO = data.ResultDataADO.ResultHistoryLDO;
					DataObjectMapper.Map<HIS_PATIENT_TYPE_ALTER>(_PatientTypeAlter, data);
				}
				string ghiChu = data.ResultDataADO.ResultHistoryLDO.ghiChu;
				string text = "";
				string[] array = ghiChu.Split(new char[4] { ')', '(', '.', '!' }, StringSplitOptions.RemoveEmptyEntries);
				List<ExamHistoryLDO> dsLichSuKCB = data.ResultDataADO.ResultHistoryLDO.dsLichSuKCB2018;
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text2.Equals(" "))
					{
						text = text + text2 + " !\r\n";
					}
				}
				if (data.ResultDataADO.ResultHistoryLDO.maKetQua == "101")
				{
					text = string.Format("{0} - {1}", data.ResultDataADO.ResultHistoryLDO.message, text);
				}
				txtViewInfoCheck.Text = text;
				LoadDataGridControl(data.ResultDataADO.ResultHistoryLDO);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void LoadDataGridControl(ResultHistoryLDO _resultHistoryLDO)
		{
			try
			{
				gridControlHistoryExam.DataSource = null;
				if (_resultHistoryLDO.dsLichSuKCB2018 != null)
				{
					List<ExamHistoryLDO> dataSource = _resultHistoryLDO.dsLichSuKCB2018.OrderByDescending((ExamHistoryLDO o) => o.ngayRa).ToList();
					gridControlHistoryExam.DataSource = dataSource;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void gridViewHistoryExam_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			try
			{
				if (!e.IsGetData || e.Column.UnboundType == UnboundColumnType.Bound)
				{
					return;
				}
				ExamHistoryLDO examHistoryLDO = (ExamHistoryLDO)((IList)((BaseView)sender).DataSource)[e.ListSourceRowIndex];
				if (examHistoryLDO == null)
				{
					return;
				}
				if (e.Column.FieldName == "STT")
				{
					e.Value = 1 + e.ListSourceRowIndex;
				}
				else if (e.Column.FieldName == "tinhTrang_str")
				{
					if (examHistoryLDO.tinhTrang == "1")
					{
						e.Value = "Ra viện";
					}
					else if (examHistoryLDO.tinhTrang == "2")
					{
						e.Value = "Chuyển viện";
					}
					else if (examHistoryLDO.tinhTrang == "3")
					{
						e.Value = "Trốn viện";
					}
					else if (examHistoryLDO.tinhTrang == "4")
					{
						e.Value = "Xin ra viện";
					}
					else if (examHistoryLDO.tinhTrang == "5")
					{
						e.Value = "Kiểm tra thẻ";
					}
				}
				else if (e.Column.FieldName == "kqDieuTri_str")
				{
					if (examHistoryLDO.kqDieuTri == "1")
					{
						e.Value = "Khỏi";
					}
					else if (examHistoryLDO.kqDieuTri == "2")
					{
						e.Value = "Đỡ";
					}
					else if (examHistoryLDO.kqDieuTri == "3")
					{
						e.Value = "Không thay đổi";
					}
					else if (examHistoryLDO.kqDieuTri == "4")
					{
						e.Value = "Nặng hơn";
					}
					else if (examHistoryLDO.kqDieuTri == "5")
					{
						e.Value = "Tử vong";
					}
				}
				else if (e.Column.FieldName == "lyDoVV_str")
				{
					if (examHistoryLDO.lyDoVV == "1")
					{
						e.Value = "Đúng tuyến";
					}
					else if (examHistoryLDO.lyDoVV == "2")
					{
						e.Value = "Cấp cứu";
					}
					else if (examHistoryLDO.lyDoVV == "3")
					{
						e.Value = "Trái tuyến";
					}
				}
				else if (e.Column.FieldName == "ngayVao_str" && !string.IsNullOrEmpty(examHistoryLDO.ngayVao))
				{
					e.Value = TimeNumberToTimeStringWithoutSecond(long.Parse(examHistoryLDO.ngayVao));
				}
				else if (e.Column.FieldName == "ngayRa_str" && !string.IsNullOrEmpty(examHistoryLDO.ngayRa))
				{
					e.Value = TimeNumberToTimeStringWithoutSecond(long.Parse(examHistoryLDO.ngayRa));
				}
				else if (e.Column.FieldName == "cskcbbd_name" && dicMediOrg != null && dicMediOrg.ContainsKey(examHistoryLDO.maCSKCB))
				{
					e.Value = dicMediOrg[examHistoryLDO.maCSKCB].MEDI_ORG_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public static string TimeNumberToTimeStringWithoutSecond(long time)
		{
			string result = null;
			try
			{
				string text = time.ToString();
				if (text != null)
				{
					if (text.Length >= 12)
					{
						return new StringBuilder().Append(text.Substring(6, 2)).Append("/").Append(text.Substring(4, 2))
							.Append("/")
							.Append(text.Substring(0, 4))
							.Append(" ")
							.Append(text.Substring(8, 2))
							.Append(":")
							.Append(text.Substring(10, 2))
							.ToString();
					}
					return result;
				}
				return result;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private void btnUpdatePatient_Click(object sender, EventArgs e)
		{
			CommonParam commonParam = new CommonParam();
			try
			{
				if (_HisPatient == null || _ResultHistoryLDO == null)
				{
					return;
				}
				HisPatientUpdateSDO hisPatientUpdateSDO = new HisPatientUpdateSDO();
				HIS_PATIENT hisPatient = _HisPatient;
				try
				{
					int num = _ResultHistoryLDO.hoTen.Trim().LastIndexOf(" ");
					if (num > -1)
					{
						hisPatient.FIRST_NAME = _ResultHistoryLDO.hoTen.Trim().Substring(num).Trim();
						hisPatient.LAST_NAME = _ResultHistoryLDO.hoTen.Trim().Substring(0, num).Trim();
					}
					else
					{
						hisPatient.FIRST_NAME = _ResultHistoryLDO.hoTen.Trim();
						hisPatient.LAST_NAME = "";
					}
				}
				catch (Exception ex)
				{
					LogSystem.Warn("Loi xu ly cat chuoi ho ten benh nhan: ", ex);
				}
				HIS_GENDER hIS_GENDER = BackendDataWorker.Get<HIS_GENDER>().FirstOrDefault((HIS_GENDER p) => p.GENDER_NAME == _ResultHistoryLDO.gioiTinh.Trim());
				if (hIS_GENDER != null)
				{
					hisPatient.GENDER_ID = hIS_GENDER.ID;
				}
				try
				{
					string[] array = _ResultHistoryLDO.ngaySinh.Trim().Split('/');
					hisPatient.DOB = Parse.ToInt64(array[2] + array[1] + array[0] + "000000");
				}
				catch (Exception ex2)
				{
					LogSystem.Warn("Loi xu ly cat chuoi ngay sinh BN: ", ex2);
				}
				List<object> list = new List<object>();
				V_HIS_PATIENT v_HIS_PATIENT = new V_HIS_PATIENT();
				DataObjectMapper.Map<V_HIS_PATIENT>(v_HIS_PATIENT, _HisPatient);
				list.Add(v_HIS_PATIENT);
				list.Add(new RefeshReference(RefeshTreatment));
				PluginInstanceBehavior.ShowModule("HIS.Desktop.Plugins.PatientUpdate", currentModule.RoomId, currentModule.RoomTypeId, list);
			}
			catch (Exception ex3)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex3);
			}
		}

		private void RefeshTreatment()
		{
			try
			{
				LoadHisPatient();
				btnUpdatePatient.Enabled = false;
				lblMessenger.Text = "Cập nhật thông tin bệnh nhân thành công";
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnUpdateBHYT_Click(object sender, EventArgs e)
		{
			bool value = false;
			CommonParam commonParam = new CommonParam();
			HisPatientTypeAlterAndTranPatiSDO hisPatientTypeAlterAndTranPatiSDO = new HisPatientTypeAlterAndTranPatiSDO();
			hisPatientTypeAlterAndTranPatiSDO.PatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
			try
			{
				if (_PatientTypeAlter != null)
				{
					HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = new HIS_PATIENT_TYPE_ALTER();
					hIS_PATIENT_TYPE_ALTER = _PatientTypeAlter;
					try
					{
						string[] array = _ResultHistoryLDO.gtTheTu.Trim().Split('/');
						hIS_PATIENT_TYPE_ALTER.HEIN_CARD_FROM_TIME = Parse.ToInt64(array[2] + array[1] + array[0] + "000000");
					}
					catch (Exception ex)
					{
						LogSystem.Warn("Loi xu ly cat chuoi gtTheTuMoi: ", ex);
					}
					try
					{
						string[] array2 = _ResultHistoryLDO.gtTheDen.Trim().Split('/');
						hIS_PATIENT_TYPE_ALTER.HEIN_CARD_TO_TIME = Parse.ToInt64(array2[2] + array2[1] + array2[0] + "000000");
					}
					catch (Exception ex2)
					{
						LogSystem.Warn("Loi xu ly cat chuoi gtTheDenMoi: ", ex2);
					}
					hIS_PATIENT_TYPE_ALTER.HEIN_MEDI_ORG_CODE = _ResultHistoryLDO.maDKBD;
					hIS_PATIENT_TYPE_ALTER.ADDRESS = _ResultHistoryLDO.diaChi;
					hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER = _ResultHistoryLDO.maThe;
					if (!string.IsNullOrEmpty(_ResultHistoryLDO.ngayDu5Nam))
					{
						try
						{
							string[] array3 = _ResultHistoryLDO.ngayDu5Nam.Trim().Split('/');
							hIS_PATIENT_TYPE_ALTER.JOIN_5_YEAR_TIME = Parse.ToInt64(array3[2] + array3[1] + array3[0] + "000000");
						}
						catch (Exception ex3)
						{
							LogSystem.Warn("Loi xu ly cat chuoi ngayDu5Nam: ", ex3);
						}
					}
					long treatmentId = _TreatmentId;
					if (_TreatmentId != 0L && _HisTreatment != null)
					{
						hisPatientTypeAlterAndTranPatiSDO.TransferInFormId = _HisTreatment.TRANSFER_IN_FORM_ID;
						hisPatientTypeAlterAndTranPatiSDO.TransferInIcdName = _HisTreatment.TRANSFER_IN_ICD_NAME;
						hisPatientTypeAlterAndTranPatiSDO.TransferInIcdCode = _HisTreatment.TRANSFER_IN_ICD_CODE;
						hisPatientTypeAlterAndTranPatiSDO.TransferInMediOrgCode = _HisTreatment.TRANSFER_IN_MEDI_ORG_CODE;
						hisPatientTypeAlterAndTranPatiSDO.TransferInMediOrgName = _HisTreatment.TRANSFER_IN_MEDI_ORG_NAME;
						hisPatientTypeAlterAndTranPatiSDO.TransferInReasonId = _HisTreatment.TRANSFER_IN_REASON_ID;
						hisPatientTypeAlterAndTranPatiSDO.TransferInCmkt = _HisTreatment.TRANSFER_IN_CMKT;
						hisPatientTypeAlterAndTranPatiSDO.TransferInCode = _HisTreatment.TRANSFER_IN_CODE;
						hisPatientTypeAlterAndTranPatiSDO.TransferInTimeFrom = _HisTreatment.TRANSFER_IN_TIME_FROM;
						hisPatientTypeAlterAndTranPatiSDO.TransferInTimeTo = _HisTreatment.TRANSFER_IN_TIME_TO;
						hisPatientTypeAlterAndTranPatiSDO.HeinPatientTypeCode = _HisTreatment.HEIN_PATIENT_TYPE_CODE;
					}
					hisPatientTypeAlterAndTranPatiSDO.PatientTypeAlter = hIS_PATIENT_TYPE_ALTER;
					HisPatientTypeAlterAndTranPatiSDO hisPatientTypeAlterAndTranPatiSDO2 = new BackendAdapter(commonParam).Post<HisPatientTypeAlterAndTranPatiSDO>("api/HisPatientTypeAlter/Update", ApiConsumers.MosConsumer, hisPatientTypeAlterAndTranPatiSDO, commonParam);
					if (hisPatientTypeAlterAndTranPatiSDO2 != null)
					{
						value = true;
						btnUpdateBHYT.Enabled = false;
						lblMessenger.Text = "Cập nhật thông tin thẻ thành công";
						LoadPatientTypeAlter();
					}
				}
				WaitingManager.Hide();
			}
			catch (Exception ex4)
			{
				WaitingManager.Hide();
				LogSystem.Error(ex4);
			}
			MessageManager.Show(this, commonParam, value);
		}

		private void btnPrintScreen_Click(object sender, EventArgs e)
		{
			string tDL_HEIN_CARD_NUMBER = _HisTreatment.TDL_HEIN_CARD_NUMBER;
			string tREATMENT_CODE = _HisTreatment.TREATMENT_CODE;
			string text = (saveFileDialog1.FileName = tREATMENT_CODE + "_" + tDL_HEIN_CARD_NUMBER);
			string text3 = text;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Thread.Sleep(500);
				string directoryName = Path.GetDirectoryName(saveFileDialog1.FileName);
				FullScreenshot(saveFileDialog1.FileName, ImageFormat.Jpeg);
				MessageBox.Show("Lưu ảnh thành công !" + directoryName);
				Show();
			}
		}

		private void FullScreenshot(string filepath, ImageFormat format)
		{
			Rectangle bounds = Screen.GetBounds(Point.Empty);
			using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
			{
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
				}
				bitmap.Save(filepath, format);
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
			this.btnPrintScreen = new DevExpress.XtraEditors.SimpleButton();
			this.gridControlHistoryExam = new DevExpress.XtraGrid.GridControl();
			this.gridViewHistoryExam = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.lblMessenger = new DevExpress.XtraEditors.LabelControl();
			this.btnUpdateBHYT = new DevExpress.XtraEditors.SimpleButton();
			this.btnUpdatePatient = new DevExpress.XtraEditors.SimpleButton();
			this.txtViewInfoCheck = new DevExpress.XtraEditors.MemoEdit();
			this.gridControlBHYT = new DevExpress.XtraGrid.GridControl();
			this.gridViewBHYT = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem13 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem14 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem15 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem16 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem17 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.maHoSo = new DevExpress.XtraGrid.Columns.GridColumn();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.gridControlHistoryExam).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewHistoryExam).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtViewInfoCheck.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridControlBHYT).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewBHYT).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem16).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem17).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			base.SuspendLayout();
			this.layoutControl1.Controls.Add(this.btnPrintScreen);
			this.layoutControl1.Controls.Add(this.gridControlHistoryExam);
			this.layoutControl1.Controls.Add(this.lblMessenger);
			this.layoutControl1.Controls.Add(this.btnUpdateBHYT);
			this.layoutControl1.Controls.Add(this.btnUpdatePatient);
			this.layoutControl1.Controls.Add(this.txtViewInfoCheck);
			this.layoutControl1.Controls.Add(this.gridControlBHYT);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(486, 2, 250, 350);
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1028, 601);
			this.layoutControl1.TabIndex = 0;
			this.layoutControl1.Text = "layoutControl1";
			this.btnPrintScreen.Location = new System.Drawing.Point(925, 408);
			this.btnPrintScreen.Margin = new System.Windows.Forms.Padding(2);
			this.btnPrintScreen.Name = "btnPrintScreen";
			this.btnPrintScreen.Size = new System.Drawing.Size(101, 22);
			this.btnPrintScreen.StyleController = this.layoutControl1;
			this.btnPrintScreen.TabIndex = 11;
			this.btnPrintScreen.Text = "Chụp ảnh màn hình";
			this.btnPrintScreen.Click += new System.EventHandler(btnPrintScreen_Click);
			this.gridControlHistoryExam.Location = new System.Drawing.Point(2, 434);
			this.gridControlHistoryExam.MainView = this.gridViewHistoryExam;
			this.gridControlHistoryExam.Name = "gridControlHistoryExam";
			this.gridControlHistoryExam.Size = new System.Drawing.Size(1024, 165);
			this.gridControlHistoryExam.TabIndex = 10;
			this.gridControlHistoryExam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewHistoryExam });
			this.gridViewHistoryExam.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[10] { this.gridColumn7, this.gridColumn8, this.gridColumn9, this.gridColumn10, this.gridColumn19, this.gridColumn17, this.gridColumn16, this.gridColumn20, this.gridColumn18, this.gridColumn15 });
			this.gridViewHistoryExam.GridControl = this.gridControlHistoryExam;
			this.gridViewHistoryExam.Name = "gridViewHistoryExam";
			this.gridViewHistoryExam.OptionsView.ShowGroupPanel = false;
			this.gridViewHistoryExam.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridViewHistoryExam_CustomUnboundColumnData);
			this.gridColumn7.Caption = "STT";
			this.gridColumn7.FieldName = "STT";
			this.gridColumn7.Name = "gridColumn7";
			this.gridColumn7.OptionsColumn.AllowEdit = false;
			this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn7.Visible = true;
			this.gridColumn7.VisibleIndex = 0;
			this.gridColumn7.Width = 34;
			this.gridColumn8.Caption = "Tên cơ sở KCB";
			this.gridColumn8.FieldName = "cskcbbd_name";
			this.gridColumn8.Name = "gridColumn8";
			this.gridColumn8.OptionsColumn.AllowEdit = false;
			this.gridColumn8.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn8.Visible = true;
			this.gridColumn8.VisibleIndex = 1;
			this.gridColumn8.Width = 201;
			this.gridColumn9.Caption = "Từ ngày";
			this.gridColumn9.FieldName = "ngayVao_str";
			this.gridColumn9.Name = "gridColumn9";
			this.gridColumn9.OptionsColumn.AllowEdit = false;
			this.gridColumn9.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn9.Visible = true;
			this.gridColumn9.VisibleIndex = 2;
			this.gridColumn9.Width = 124;
			this.gridColumn10.Caption = "Đến ngày";
			this.gridColumn10.FieldName = "ngayRa_str";
			this.gridColumn10.Name = "gridColumn10";
			this.gridColumn10.OptionsColumn.AllowEdit = false;
			this.gridColumn10.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn10.Visible = true;
			this.gridColumn10.VisibleIndex = 3;
			this.gridColumn10.Width = 124;
			this.gridColumn19.Caption = "Tình trạng";
			this.gridColumn19.FieldName = "tinhTrang_str";
			this.gridColumn19.Name = "gridColumn19";
			this.gridColumn19.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn19.Visible = true;
			this.gridColumn19.VisibleIndex = 4;
			this.gridColumn17.Caption = "Mã hồ sơ";
			this.gridColumn17.FieldName = "maHoSo";
			this.gridColumn17.Name = "gridColumn17";
			this.gridColumn17.Visible = true;
			this.gridColumn17.VisibleIndex = 5;
			this.gridColumn16.Caption = "Mã cơ sở KCB";
			this.gridColumn16.FieldName = "maCSKCB";
			this.gridColumn16.Name = "gridColumn16";
			this.gridColumn16.Visible = true;
			this.gridColumn16.VisibleIndex = 6;
			this.gridColumn20.Caption = "Tên bệnh";
			this.gridColumn20.FieldName = "tenBenh";
			this.gridColumn20.Name = "gridColumn20";
			this.gridColumn20.Visible = true;
			this.gridColumn20.VisibleIndex = 7;
			this.gridColumn18.Caption = "Kết quả điều trị";
			this.gridColumn18.FieldName = "kqDieuTri_str";
			this.gridColumn18.Name = "gridColumn18";
			this.gridColumn18.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn18.Visible = true;
			this.gridColumn18.VisibleIndex = 8;
			this.gridColumn18.Width = 120;
			this.gridColumn15.Caption = "Lý do vào  viện";
			this.gridColumn15.FieldName = "lyDoVV_str";
			this.gridColumn15.Name = "gridColumn15";
			this.gridColumn15.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn15.Visible = true;
			this.gridColumn15.VisibleIndex = 9;
			this.gridColumn15.Width = 120;
			this.lblMessenger.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lblMessenger.Location = new System.Drawing.Point(17, 408);
			this.lblMessenger.Name = "lblMessenger";
			this.lblMessenger.Size = new System.Drawing.Size(663, 20);
			this.lblMessenger.StyleController = this.layoutControl1;
			this.lblMessenger.TabIndex = 9;
			this.lblMessenger.Text = "Có sự sai khác thông tin bệnh nhân và thông tin trên cổng BHYT";
			this.btnUpdateBHYT.Enabled = false;
			this.btnUpdateBHYT.Location = new System.Drawing.Point(803, 408);
			this.btnUpdateBHYT.Name = "btnUpdateBHYT";
			this.btnUpdateBHYT.Size = new System.Drawing.Size(118, 22);
			this.btnUpdateBHYT.StyleController = this.layoutControl1;
			this.btnUpdateBHYT.TabIndex = 8;
			this.btnUpdateBHYT.Text = "Cập nhật thông tin thẻ";
			this.btnUpdateBHYT.Click += new System.EventHandler(btnUpdateBHYT_Click);
			this.btnUpdatePatient.Enabled = false;
			this.btnUpdatePatient.Location = new System.Drawing.Point(684, 408);
			this.btnUpdatePatient.Name = "btnUpdatePatient";
			this.btnUpdatePatient.Size = new System.Drawing.Size(115, 22);
			this.btnUpdatePatient.StyleController = this.layoutControl1;
			this.btnUpdatePatient.TabIndex = 7;
			this.btnUpdatePatient.Text = "Cập nhật thông tin BN";
			this.btnUpdatePatient.ToolTip = "Cập nhật thông tin bệnh nhân";
			this.btnUpdatePatient.Click += new System.EventHandler(btnUpdatePatient_Click);
			this.txtViewInfoCheck.Location = new System.Drawing.Point(2, 118);
			this.txtViewInfoCheck.Name = "txtViewInfoCheck";
			this.txtViewInfoCheck.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
			this.txtViewInfoCheck.Properties.Appearance.Options.UseFont = true;
			this.txtViewInfoCheck.Size = new System.Drawing.Size(1024, 286);
			this.txtViewInfoCheck.StyleController = this.layoutControl1;
			this.txtViewInfoCheck.TabIndex = 6;
			this.gridControlBHYT.Location = new System.Drawing.Point(2, 2);
			this.gridControlBHYT.MainView = this.gridViewBHYT;
			this.gridControlBHYT.Name = "gridControlBHYT";
			this.gridControlBHYT.Size = new System.Drawing.Size(1024, 112);
			this.gridControlBHYT.TabIndex = 4;
			this.gridControlBHYT.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[1] { this.gridViewBHYT });
			this.gridViewBHYT.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[9] { this.gridColumn1, this.gridColumn2, this.gridColumn3, this.gridColumn4, this.gridColumn5, this.gridColumn6, this.gridColumn12, this.gridColumn13, this.gridColumn14 });
			this.gridViewBHYT.GridControl = this.gridControlBHYT;
			this.gridViewBHYT.Name = "gridViewBHYT";
			this.gridViewBHYT.OptionsView.ShowGroupPanel = false;
			this.gridViewBHYT.OptionsView.ShowIndicator = false;
			this.gridViewBHYT.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(gridViewBHYT_RowCellClick);
			this.gridViewBHYT.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(gridViewBHYT_CustomUnboundColumnData);
			this.gridColumn1.Caption = "Diện điều trị";
			this.gridColumn1.FieldName = "TREATMENT_TYPE_NAME";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.AllowEdit = false;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 0;
			this.gridColumn1.Width = 83;
			this.gridColumn2.Caption = "Số thẻ";
			this.gridColumn2.FieldName = "HEIN_CARD_NUMBER";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.OptionsColumn.AllowEdit = false;
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 1;
			this.gridColumn2.Width = 93;
			this.gridColumn3.Caption = "Hạn từ";
			this.gridColumn3.FieldName = "HEIN_CARD_FROM_TIME_STR";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.OptionsColumn.AllowEdit = false;
			this.gridColumn3.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 2;
			this.gridColumn3.Width = 57;
			this.gridColumn4.Caption = "Hạn đến";
			this.gridColumn4.FieldName = "HEIN_CARD_TO_TIME_STR";
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.OptionsColumn.AllowEdit = false;
			this.gridColumn4.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 3;
			this.gridColumn4.Width = 63;
			this.gridColumn5.Caption = "Nơi khám chữa bệnh ban đầu";
			this.gridColumn5.FieldName = "HEIN_MEDI_ORG_NAME";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.OptionsColumn.AllowEdit = false;
			this.gridColumn5.Visible = true;
			this.gridColumn5.VisibleIndex = 4;
			this.gridColumn5.Width = 162;
			this.gridColumn6.Caption = "Đủ 5 năm";
			this.gridColumn6.FieldName = "JOIN_5_YEAR_STR";
			this.gridColumn6.Name = "gridColumn6";
			this.gridColumn6.OptionsColumn.AllowEdit = false;
			this.gridColumn6.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn6.Visible = true;
			this.gridColumn6.VisibleIndex = 5;
			this.gridColumn6.Width = 58;
			this.gridColumn12.Caption = "6 tháng";
			this.gridColumn12.FieldName = "PAID_6_MONTH_STR";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn12.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn12.Visible = true;
			this.gridColumn12.VisibleIndex = 6;
			this.gridColumn12.Width = 53;
			this.gridColumn13.Caption = "Ngày đủ 5 năm";
			this.gridColumn13.FieldName = "JOIN_5_YEAR_TIME_STR";
			this.gridColumn13.Name = "gridColumn13";
			this.gridColumn13.UnboundType = DevExpress.Data.UnboundColumnType.Object;
			this.gridColumn13.Visible = true;
			this.gridColumn13.VisibleIndex = 7;
			this.gridColumn13.Width = 89;
			this.gridColumn14.Caption = "Khu vực";
			this.gridColumn14.FieldName = "LIVE_AREA_CODE";
			this.gridColumn14.Name = "gridColumn14";
			this.gridColumn14.Visible = true;
			this.gridColumn14.VisibleIndex = 8;
			this.gridColumn14.Width = 54;
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[7] { this.layoutControlItem1, this.layoutControlItem13, this.layoutControlItem14, this.layoutControlItem15, this.layoutControlItem16, this.layoutControlItem17, this.layoutControlItem2 });
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1028, 601);
			this.layoutControlGroup1.TextVisible = false;
			this.layoutControlItem1.Control = this.gridControlBHYT;
			this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(1028, 116);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.layoutControlItem13.Control = this.txtViewInfoCheck;
			this.layoutControlItem13.Location = new System.Drawing.Point(0, 116);
			this.layoutControlItem13.Name = "layoutControlItem13";
			this.layoutControlItem13.Size = new System.Drawing.Size(1028, 290);
			this.layoutControlItem13.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem13.TextVisible = false;
			this.layoutControlItem14.Control = this.btnUpdatePatient;
			this.layoutControlItem14.Location = new System.Drawing.Point(682, 406);
			this.layoutControlItem14.Name = "layoutControlItem14";
			this.layoutControlItem14.Size = new System.Drawing.Size(119, 26);
			this.layoutControlItem14.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem14.TextVisible = false;
			this.layoutControlItem15.Control = this.btnUpdateBHYT;
			this.layoutControlItem15.Location = new System.Drawing.Point(801, 406);
			this.layoutControlItem15.Name = "layoutControlItem15";
			this.layoutControlItem15.Size = new System.Drawing.Size(122, 26);
			this.layoutControlItem15.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem15.TextVisible = false;
			this.layoutControlItem16.Control = this.lblMessenger;
			this.layoutControlItem16.Location = new System.Drawing.Point(0, 406);
			this.layoutControlItem16.Name = "layoutControlItem16";
			this.layoutControlItem16.Size = new System.Drawing.Size(682, 26);
			this.layoutControlItem16.Text = " ";
			this.layoutControlItem16.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem16.TextSize = new System.Drawing.Size(10, 20);
			this.layoutControlItem16.TextToControlDistance = 5;
			this.layoutControlItem17.Control = this.gridControlHistoryExam;
			this.layoutControlItem17.Location = new System.Drawing.Point(0, 432);
			this.layoutControlItem17.Name = "layoutControlItem17";
			this.layoutControlItem17.Size = new System.Drawing.Size(1028, 169);
			this.layoutControlItem17.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem17.TextVisible = false;
			this.layoutControlItem2.Control = this.btnPrintScreen;
			this.layoutControlItem2.Location = new System.Drawing.Point(923, 406);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.Size = new System.Drawing.Size(105, 26);
			this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem2.TextVisible = false;
			this.maHoSo.Caption = "Mã hồ sơ";
			this.maHoSo.FieldName = "maHoSo";
			this.maHoSo.Name = "maHoSo";
			this.maHoSo.Width = 66;
			this.saveFileDialog1.DefaultExt = "bmp";
			this.saveFileDialog1.Filter = "JPeg Image|*.jpg";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1028, 601);
			base.Controls.Add(this.layoutControl1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "frmCheckInfoBHYT";
			base.ShowIcon = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Thông tin BHYT";
			base.Load += new System.EventHandler(frmCheckInfoBHYT_Load);
			base.Controls.SetChildIndex(this.layoutControl1, 0);
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.gridControlHistoryExam).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewHistoryExam).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtViewInfoCheck.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridControlBHYT).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridViewBHYT).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem13).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem14).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem15).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem16).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem17).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
