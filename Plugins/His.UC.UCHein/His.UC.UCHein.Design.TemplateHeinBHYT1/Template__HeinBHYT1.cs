using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using HIS.Desktop.ADO;
using HIS.Desktop.ApiConsumer;
using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using HIS.Desktop.LocalStorage.LocalData;
using HIS.Desktop.Plugins.Library.CheckHeinGOV;
using HIS.Desktop.Utility;
using His.UC.LibraryMessage;
using His.UC.UCHein.Base;
using His.UC.UCHein.Config;
using His.UC.UCHein.ControlProcess;
using His.UC.UCHein.Data;
using His.UC.UCHein.Design.TemplateHeinBHYT1.ValidationRule;
using His.UC.UCHein.HisPatient;
using His.UC.UCHein.HisTreatment;
using His.UC.UCHein.Resources;
using His.UC.UCHein.Utils;
using Inventec.Common.Adapter;
using Inventec.Common.Controls.EditorLoader;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.QrCodeBHYT;
using Inventec.Common.Resource;
using Inventec.Common.String;
using Inventec.Common.TypeConvert;
using Inventec.Core;
using Inventec.Desktop.Common.Controls.ValidationRule;
using Inventec.Desktop.Common.HtmlString;
using Inventec.Desktop.Common.LanguageManager;
using Inventec.Desktop.Common.LibraryMessage;
using Inventec.Desktop.Common.Modules;
using Inventec.Desktop.CustomControl;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein.Bhyt;
using MOS.LibraryHein.Bhyt.HeinLiveArea;
using MOS.LibraryHein.Bhyt.HeinRightRouteType;
using MOS.SDO;

namespace His.UC.UCHein.Design.TemplateHeinBHYT1
{
	public class Template__HeinBHYT1 : UserControl
	{
		private enum RightRouterFactory
		{
			RIGHT_ROUTER,
			WRONG_ROUTER,
			WRONG_ROUTER__CHOICE_RIGHT,
			WRONG_ROUTER__CHOICE_RIGHT_FOR_MEDI_ORG_ROUTE,
			WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC,
			WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT,
			WRONG_ROUTER__CHOICE_RIGHT__DELETE_CHOICE_TYPE,
			WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT
		}

		private SetFocusMoveOut dlgsetFocusMoveOut;

		private SetShortcutKeyDown dlgsetShortcutKeyDown;

		private ProcessFillDataCareerUnder6AgeByHeinCardNumber dlgProcessFillDataCareerUnder6AgeByHeinCardNumber;

		private DelegateAutoCheckCC dlgautoCheckCC;

		private DeleteTreatmentTypeId TreatmentTypeId1;

		private CheckExamHistoryByHeinCardNumber dlgcheckExamHistory;

		private FillDataPatientSDOToRegisterForm dlgfillDataPatientSDOToRegisterForm;

		private DelegateSetRelativeAddress _DelegateSetRelativeAddress;

		private Action actChangePatientDob;

		private int positionHandleControl = -1;

		private long PatientTypeId;

		private bool IsReset;

		private DataInitHeinBhyt entity;

		private string HeinPatientCode;

		private HisPatientSDO currentPatientSdo;

		private bool isDefaultInit;

		private long ExceedDayAllow;

		private bool firstCheck = true;

		private bool isCallByRegistor;

		private string _TextIcdName = "";

		private bool IsDungTuyenCapCuuByTime;

		private bool IsShowMessage;

		private bool IsAutoCheck;

		private bool isFillingHeinDataFromDb;

		private long logTime;

		private string TT = "TT";

		private string DT = "DT";

		private long treatmentTypeId;

		private long ActionType;

		private static List<HIS_BHYT_PARAM> listBhytParam;

		private bool isClickCboPatientTypeCode;

		private IContainer components;

		private ButtonEdit txtHeinCardToTime;

		private ButtonEdit txtHeinCardFromTime;

		private DateEdit dtHeinCardToTime;

		private DateEdit dtHeinCardFromTime;

		internal ButtonEdit txtSoThe;

		internal CheckEdit chkHasDobCertificate;

		internal TextEdit txtMucHuong;

		internal CheckEdit chkMediRecordNoRouteTransfer;

		internal CheckEdit chkMediRecordRouteTransfer;

		internal CheckEdit chkJoin5Year;

		internal GridLookUpEdit cboSoThe;

		private LayoutControl layoutControl1;

		private LayoutControlGroup layoutControlGroup1;

		private LayoutControlItem lblCaptionHasDobCertificate;

		private Panel panel3;

		private Panel panel2;

		private Panel panel1;

		private LayoutControlItem lblHeincardNumber;

		private LayoutControlItem lblHeincardToDate;

		private LayoutControlItem lblHeincardFromDate;

		private LayoutControlItem lciMediRecordRouteTransfer;

		private LayoutControlItem lciMediRecordNoRouteTransfer;

		private LayoutControlItem lcichkJoin5Year;

		private LayoutControlItem lblMediRecordBenefitSymbol;

		private DXErrorProvider dxErrorProvider1;

		private DXValidationProvider dxValidationProvider1;

		private TextEdit txtAddress;

		private LayoutControlItem lblCaptionAddress;

		internal TextEdit txtMaHinhThucChuyen;

		private LayoutControlItem lciTransPatiFormCode;

		internal LookUpEdit cboHinhThucChuyen;

		private LayoutControlItem lciTransPatiFormCbo;

		private CustomGridLookUpEditWithFilterMultiColumn cboNoiChuyenDen;

		private CustomGridViewWithFilterMultiColumn gridView1;

		internal TextEdit txtMaNoiChuyenDen;

		private LayoutControlItem lblMediRecordMediOrgForm;

		private LayoutControlItem lciNoiChuyenDenName;

		private CustomGridLookUpEditWithFilterMultiColumn cboDKKCBBD;

		private CustomGridViewWithFilterMultiColumn gridLookUpEdit1View;

		internal TextEdit txtMaDKKCBBD;

		private LayoutControlItem lblHeincardMediOrg;

		private LayoutControlItem lciDKKCBBDName;

		internal LookUpEdit cboHeinRightRoute;

		internal TextEdit txtHeinRightRouteCode;

		private LayoutControlItem lblRightRouteType;

		private LayoutControlItem lciRightRouteTypeName;

		internal LookUpEdit cboLyDoChuyen;

		internal TextEdit txtMaLyDoChuyen;

		private LayoutControlItem lciTransPatiReasonCode;

		private LayoutControlItem lciTransPatiReasoncbo;

		private Panel panel5;

		private TextEdit txtMaChanDoanTD;

		private CheckEdit chkHasDialogText;

		private LayoutControlItem lblEditIcd;

		private LayoutControlItem lciIcdMain;

		private LayoutControlItem panelICD;

		private CustomGridLookUpEditWithFilterMultiColumn cboChanDoanTD;

		private CustomGridViewWithFilterMultiColumn gridView3;

		internal TextEdit txtDialogText;

		internal CheckEdit chkPaid6Month;

		private LayoutControlItem lcichkPaid6Month;

		internal CheckEdit rdoWrongRoute;

		private LayoutControlItem lcirdoWrongRoute;

		internal CheckEdit rdoRightRoute;

		private LayoutControlItem lcirdoRightRoute;

		private CheckEdit checkKhongKTHSD;

		private LayoutControlItem lciKhongKTHSD;

		internal TextEdit txtHNCode;

		private LayoutControlItem lciHNCode;

		internal TextEdit txtCoPaidAccumulate;

		private LayoutControlItem lciCoPaidAccumulate;

		internal TextEdit txtInCode;

		private LayoutControlItem lciInCode;

		private ButtonEdit txtFreeCoPainTime;

		private PanelControl panelControl1;

		private LayoutControlItem lciFreeCoPainTime;

		private DateEdit dtFreeCoPainTime;

		private CheckEdit chkTempQN;

		private LayoutControlItem lciTempQN;

		private GridView gridView2;

		private LayoutControlItem lblMediRecordLiveArea;

		internal GridLookUpEdit cboNoiSong;

		private Panel panel4;

		private ButtonEdit txtDu5Nam;

		private DateEdit dtDu5Nam;

		private LayoutControlItem lciDu5Nam;

		private DateEdit dtTransferInTimeTo;

		private DateEdit dtTransferInTimeFrom;

		private LayoutControlItem lciFordtTransferInTimeFrom;

		private LayoutControlItem lciFordtTransferInTimeTo;

		private SimpleButton btnCheckInfoBHYT;

		private LayoutControlItem layoutControlItem1;

		private CheckEdit chkBaby;

		private LayoutControlItem layoutControlItem2;

		private CheckEdit chkTt46;

		private TextEdit txtTt46;

		private CheckEdit chkHasAbsentLetter;

		private CheckEdit chkHasWorkingLetter;

		private LayoutControlItem layoutControlItem3;

		private LayoutControlItem layoutControlItem4;

		private LayoutControlItem layoutControlItem5;

		private LayoutControlItem layoutControlItem6;

		private EmptySpaceItem emptySpaceItem2;

		private GridLookUpEdit cboPatientCode;

		private GridView gridView4;

		internal LayoutControlItem layoutControlItem7;

		private RepositoryItemMemoEdit repositoryItemMemoEdit1;

		private LayoutControlItem layoutControlItem8;

		private string TreatmentTypeCode { get; set; }

		private CultureInfo CultureInfo { get; set; }

		private ResultDataADO ResultDataADO { get; set; }

		private string SysMediOrgCode { get; set; }

		private string HeinLevelCodeCurrent { get; set; }

		private string MediOrgCodeCurrent { get; set; }

		private List<string> MediOrgCodesAccepts { get; set; }

		private long TreatmentTypeIdExam { get; set; }

		internal static long PatientTypeIdBHYT { get; set; }

		private long isVisibleControl { get; set; }

		private string isShowCheckKhongKTHSD { get; set; }

		private string autoCheckIcd { get; set; }

		private bool IsDefaultRightRouteType { get; set; }

		private bool IsNotRequiredRightTypeInCaseOfHavingAreaCode { get; set; }

		private bool IsEdit { get; set; }

		private bool IsTempQN { get; set; }

		private bool IsObligatoryTranferMediOrg { get; set; }

		private string ObligatoryTranferMediOrg { get; set; }

		public long PatientId { get; private set; }

		private HIS_PATIENT_TYPE_ALTER patientTypeAlterOld { get; set; }

		private List<HIS_PATIENT_TYPE_ALTER> patyAlters { get; set; }

		public long TreatmentTypeId { get; private set; }

		private List<PatientTypeAlterADO> lstPatientTypeAlterMap { get; set; }

		public string oldeHeinCardNumber { get; set; }

		internal void InitOldPatientData(long patientId, string heinCardNumber)
		{
			try
			{
				if (currentPatientSdo == null)
				{
					currentPatientSdo = new HisPatientSDO();
				}
				currentPatientSdo.ID = patientId;
				currentPatientSdo.HeinCardNumber = heinCardNumber;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitDataToControl()
		{
			try
			{
				MediOrgProcess.LoadDataToComboNoiDKKCBBD(cboDKKCBBD, DataStore.MediOrgs);
				MediOrgProcess.LoadDataToComboNoiDKKCBBD(cboNoiChuyenDen, DataStore.MediOrgs);
				IcdProcess.LoadDataToCombo(cboChanDoanTD, DataStore.IcdADOs);
				LiveAreaProcess.LoadDataToComboNoiSong(cboNoiSong, DataStore.LiveAreas);
				TranPatiFormProcess.LoadDataToCombo(cboHinhThucChuyen, DataStore.TranPatiForms);
				TranPatiReasonProcess.LoadDataToComboLyDoChuyen(cboLyDoChuyen, DataStore.TranPatiReasons);
				if (HisConfigCFG.NotDisplayedRouteTypeOver == "1")
				{
					DataStore.HeinRightRouteTypes = DataStore.HeinRightRouteTypes.Where((HeinRightRouteTypeData p) => p.HeinRightRouteTypeCode != "TH").ToList();
				}
				HeinRightRouterTypeProcess.FillDataToComboHeinRightRouterType(cboHeinRightRoute, DataStore.HeinRightRouteTypes);
				chkHasDobCertificate.Enabled = entity != null && entity.IsChild;
				if (chkHasDobCertificate.Enabled)
				{
					chkHasDobCertificate.Focus();
				}
				else
				{
					FocusMoveOut();
				}
				chkBaby.Enabled = entity.IsChild;
				if (!chkBaby.Enabled)
				{
					chkBaby.Checked = false;
				}
				chkTt46.Checked = false;
				txtTt46.Enabled = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitDefaultRightRouteType()
		{
			try
			{
				cboHeinRightRoute.EditValue = "GT";
				txtHeinRightRouteCode.Text = "GT";
				cboHeinRightRoute.Properties.Buttons[1].Visible = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool FillDataAfterSelectOnePatient(HisPatientSDO patientSDO)
		{
			bool flag = true;
			try
			{
				if (patientSDO == null)
				{
					throw new ArgumentNullException("Du lieu dau vao khong hop le => patientSDO is null");
				}
				currentPatientSdo = patientSDO;
				if (flag)
				{
					FillDataToHeinInsuranceInfoByOldPatient(currentPatientSdo);
				}
				if (dlgfillDataPatientSDOToRegisterForm != null)
				{
					flag = dlgfillDataPatientSDOToRegisterForm(currentPatientSdo);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return flag;
		}

		private void InitDefaultValidRightRouteType(bool isFocus, string mediOrgCode, string liveArea = "")
		{
			try
			{
				if (entity.IsAutoSelectEmergency)
				{
					AutoSelectEmergency(entity);
				}
				else if ((HasChangeValidRightRouteType(mediOrgCode, liveArea) && isFocus && cboHeinRightRoute.EditValue == null && rdoRightRoute.Checked) || IsDefaultRightRouteType)
				{
					if (IsDefaultRightRouteType)
					{
						InitDefaultRightRouteType();
					}
					else if (cboHeinRightRoute.EditValue == null && !SetDefaultRightCode())
					{
						XtraMessageBox.Show(His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.His_UCHein__MaDKKCBBDKhacVoiCuaVienNguoiDungPhaiChonTruongHop), His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), DefaultBoolean.True);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitDefaultRightRouteTypeAppointment(string mediOrgCode)
		{
			try
			{
				if (entity.IsAutoSelectEmergency)
				{
					AutoSelectEmergency(entity);
				}
				else if (currentPatientSdo != null && !string.IsNullOrEmpty(currentPatientSdo.AppointmentCode) && !MediOrgCodeCurrent.Equals(mediOrgCode))
				{
					cboHeinRightRoute.EditValue = "HK";
					txtHeinRightRouteCode.Text = "HK";
					cboHeinRightRoute.Properties.Buttons[1].Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void FillDataHeinInsuranceBySelectedPatientTypeAlter(HIS_PATIENT_TYPE_ALTER patientTypeAlter, bool isFocus)
		{
			try
			{
				isFillingHeinDataFromDb = true;
				if (patientTypeAlter == null)
				{
					throw new ArgumentNullException("patientTypeAlter is null");
				}
				txtSoThe.Text = patientTypeAlter.HEIN_CARD_NUMBER;
				txtHeinCardFromTime.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault());
				dtHeinCardFromTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
				txtHeinCardToTime.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlter.HEIN_CARD_TO_TIME.GetValueOrDefault());
				dtHeinCardToTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
				if (patientTypeAlter.JOIN_5_YEAR_TIME.HasValue)
				{
					txtDu5Nam.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlter.JOIN_5_YEAR_TIME.GetValueOrDefault());
					dtDu5Nam.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtDu5Nam.Text);
				}
				else
				{
					txtDu5Nam.Text = "";
					dtDu5Nam.EditValue = null;
				}
				if (!IsDungTuyenCapCuuByTime && !entity.IsAutoSelectEmergency && DataStore.HeinRightRouteTypes.Exists((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode == patientTypeAlter.RIGHT_ROUTE_TYPE_CODE))
				{
					cboHeinRightRoute.EditValue = patientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
					txtHeinRightRouteCode.Text = patientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
					cboHeinRightRoute.Properties.Buttons[1].Visible = !string.IsNullOrEmpty(patientTypeAlter.RIGHT_ROUTE_TYPE_CODE);
				}
				if (patientTypeAlter.HAS_BIRTH_CERTIFICATE == "C")
				{
					List<MediOrgADO> list = cboDKKCBBD.Properties.DataSource as List<MediOrgADO>;
					MediOrgADO mediOrgADO = new MediOrgADO();
					mediOrgADO.MEDI_ORG_CODE = patientTypeAlter.HEIN_MEDI_ORG_CODE;
					mediOrgADO.MEDI_ORG_NAME = patientTypeAlter.HEIN_MEDI_ORG_NAME;
					mediOrgADO.MEDI_ORG_NAME_UNSIGNED = Inventec.Common.String.Convert.UnSignVNese2(patientTypeAlter.HEIN_MEDI_ORG_NAME);
					list.Add(mediOrgADO);
					cboDKKCBBD.Properties.DataSource = list;
					chkHasDobCertificate.Enabled = false;
				}
				if (patientTypeAlter.HAS_BIRTH_CERTIFICATE == "C" && patientTypeAlter.PATIENT_TYPE_ID == PatientTypeIdBHYT && entity.IsSampleDepartment && entity.HisTreatment != null && entity.HisTreatment.IS_PAUSE != 1)
				{
					chkHasDobCertificate.Enabled = true;
				}
				txtMaDKKCBBD.Text = patientTypeAlter.HEIN_MEDI_ORG_CODE;
				cboDKKCBBD.EditValue = patientTypeAlter.HEIN_MEDI_ORG_CODE;
				cboNoiSong.EditValue = patientTypeAlter.LIVE_AREA_CODE;
				IsAutoCheck = true;
				chkJoin5Year.Checked = patientTypeAlter.JOIN_5_YEAR == "C";
				chkPaid6Month.Checked = patientTypeAlter.PAID_6_MONTH == "C";
				if (chkBaby.Enabled)
				{
					chkBaby.Checked = patientTypeAlter.IS_NEWBORN == 1;
				}
				chkHasWorkingLetter.Checked = patientTypeAlter.HAS_WORKING_LETTER == 1;
				chkHasAbsentLetter.Checked = patientTypeAlter.HAS_ABSENT_LETTER == 1;
				chkTt46.Checked = patientTypeAlter.IS_TT46 == 1;
				txtTt46.Text = patientTypeAlter.TT46_NOTE;
				IsAutoCheck = false;
				txtAddress.Text = patientTypeAlter.ADDRESS;
				txtHNCode.Text = patientTypeAlter.HNCODE;
				if (HeinPatientCode != null)
				{
					cboPatientCode.EditValue = HeinPatientCode;
				}
				if (patientTypeAlter.FREE_CO_PAID_TIME.GetValueOrDefault() > 0)
				{
					txtFreeCoPainTime.Text = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patientTypeAlter.FREE_CO_PAID_TIME.GetValueOrDefault());
					dtFreeCoPainTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtFreeCoPainTime.Text);
				}
				else
				{
					txtFreeCoPainTime.Text = "";
					dtFreeCoPainTime.EditValue = null;
				}
				if (patientTypeAlter.CO_PAID_ACCUMULATE_AMOUNT.GetValueOrDefault() > 0)
				{
					txtCoPaidAccumulate.Text = patientTypeAlter.CO_PAID_ACCUMULATE_AMOUNT.GetValueOrDefault().ToString();
				}
				else
				{
					txtCoPaidAccumulate.Text = "";
				}
				if (currentPatientSdo != null && !string.IsNullOrEmpty(currentPatientSdo.AppointmentCode) && !MediOrgCodeCurrent.Equals(patientTypeAlter.HEIN_MEDI_ORG_CODE))
				{
					rdoRightRoute.Checked = true;
					rdoWrongRoute.Checked = !rdoRightRoute.Checked;
					cboHeinRightRoute.EditValue = "HK";
					txtHeinRightRouteCode.Text = "HK";
					cboHeinRightRoute.Properties.Buttons[1].Visible = true;
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT, isFocus);
				}
				else if (patientTypeAlter.RIGHT_ROUTE_CODE == "DT")
				{
					rdoRightRoute.Checked = true;
					rdoWrongRoute.Checked = !rdoRightRoute.Checked;
					if (DataStore.HeinRightRouteTypes.Exists((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode == patientTypeAlter.RIGHT_ROUTE_TYPE_CODE))
					{
						cboHeinRightRoute.EditValue = patientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
						txtHeinRightRouteCode.Text = patientTypeAlter.RIGHT_ROUTE_TYPE_CODE;
					}
					if (IsDungTuyenCapCuuByTime)
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC, isFocus);
					}
					else if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == "GT")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT, isFocus);
						txtHeinRightRouteCode.Enabled = true;
						cboHeinRightRoute.Enabled = true;
					}
					else if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == "CC")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC, isFocus);
					}
					else if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == "HK")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT, isFocus);
						txtHeinRightRouteCode.Enabled = true;
						cboHeinRightRoute.Enabled = true;
					}
					else if (string.IsNullOrEmpty(patientTypeAlter.RIGHT_ROUTE_TYPE_CODE))
					{
						SetEnableControlHein(RightRouterFactory.RIGHT_ROUTER, isFocus);
					}
				}
				else
				{
					rdoRightRoute.Checked = false;
					rdoWrongRoute.Checked = !rdoRightRoute.Checked;
					txtHeinRightRouteCode.Enabled = false;
					cboHeinRightRoute.Enabled = false;
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER, isFocus);
				}
				chkHasDobCertificate.Checked = patientTypeAlter.HAS_BIRTH_CERTIFICATE == "C";
				HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = DataStore.TreatmentTypes.FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == patientTypeAlter.TREATMENT_TYPE_ID);
				ChangeDefaultHeinRatio((hIS_TREATMENT_TYPE != null) ? hIS_TREATMENT_TYPE.HEIN_TREATMENT_TYPE_CODE : "");
				if (entity.IsChild)
				{
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER, isFocus);
				}
				VisibleButtonDeleteHeinRightRoute();
				ValidateRightRouteType();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			finally
			{
				isFillingHeinDataFromDb = false;
			}
		}

		internal void FillDataToHeinInsuranceInfoByOldPatient(HisPatientSDO patientsdo)
		{
			try
			{
				if (patientsdo == null || patientsdo.ID == 0L)
				{
					throw new ArgumentNullException("Du lieu dau vao khong hop le => patientsdo is null");
				}
				dxErrorProvider1.ClearErrors();
				currentPatientSdo = patientsdo;
				CommonParam commonParam = new CommonParam();
				HisPatientTypeAlterFilter hisPatientTypeAlterFilter = new HisPatientTypeAlterFilter();
				hisPatientTypeAlterFilter.TDL_PATIENT_ID = patientsdo.ID;
				hisPatientTypeAlterFilter.PATIENT_TYPE_ID = PatientTypeIdBHYT;
				List<HIS_PATIENT_TYPE_ALTER> list = new BackendAdapter(commonParam).Get<List<HIS_PATIENT_TYPE_ALTER>>("/api/HisPatientTypeAlter/Get", ApiConsumerStore.MosConsumer, hisPatientTypeAlterFilter, commonParam);
				if (list == null || list.Count == 0)
				{
					throw new ArgumentNullException("Khong tim thay du lieu HIS_PATIENT_TYPE_ALTER theo benh nhan co PatientId = " + patientsdo.ID + " => patyAlters is null");
				}
				list = list.OrderByDescending((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME).ToList();
				List<HIS_PATIENT_TYPE_ALTER> list2 = (from it in list
					group it by new { it.HEIN_CARD_NUMBER, it.HEIN_CARD_FROM_TIME, it.HEIN_CARD_TO_TIME, it.HEIN_MEDI_ORG_CODE, it.JOIN_5_YEAR, it.PAID_6_MONTH, it.RIGHT_ROUTE_CODE, it.RIGHT_ROUTE_TYPE_CODE } into @group
					select @group.First()).Distinct().ToList();
				if (list2 != null)
				{
					list2 = list2.OrderByDescending((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME).ToList();
				}
				ReloadComboSoThe(cboSoThe, list2);
				if (!string.IsNullOrEmpty(patientsdo.HeinCardNumber))
				{
					list2 = list2.Where((HIS_PATIENT_TYPE_ALTER o) => o.HEIN_CARD_NUMBER == patientsdo.HeinCardNumber).ToList();
				}
				if (!string.IsNullOrEmpty(patientsdo.AppointmentCode))
				{
					list2 = list2.Where((HIS_PATIENT_TYPE_ALTER o) => o.HEIN_MEDI_ORG_CODE == patientsdo.HeinMediOrgCode && o.JOIN_5_YEAR == patientsdo.Join5Year && o.PAID_6_MONTH == patientsdo.Paid6Month && o.RIGHT_ROUTE_CODE == patientsdo.RightRouteCode && o.RIGHT_ROUTE_TYPE_CODE == patientsdo.RightRouteTypeCode).ToList();
				}
				if (list2 != null && list2.Count > 0)
				{
					if (list2.Count > 1)
					{
						list2 = list2.OrderByDescending((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME).ToList();
					}
					cboSoThe.EditValue = list2[0].ID;
					HeinCardSelectRowHandler(list2[0]);
					if (cboHeinRightRoute.EditValue != "HK")
					{
						MediOrgSelectRowChange(false, (cboNoiSong.EditValue ?? "").ToString());
					}
					LogSystem.Info("FillDataToHeinInsuranceInfoByOldPatient => Benh nhan co the bhyt, tu dong lay the gan nhat (so the: " + list2[0].HEIN_CARD_NUMBER + ")");
				}
				else
				{
					LogSystem.Info("FillDataToHeinInsuranceInfoByOldPatient => Khong tim thay thong tin the bhyt cua BN, PatientId = " + patientsdo.ID);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool ConfirmIcdNotRecommendMain(HIS_ICD icd)
		{
			try
			{
				if (icd == null)
				{
					return true;
				}
				if (entity == null || !entity.IsWarningIcdNotRecommendMainWhenEdit)
				{
					return true;
				}
				if (icd.IS_NOT_RECOMMEND_MAIN != 1)
				{
					return true;
				}
				string arg = icd.ICD_CODE + " - " + icd.ICD_NAME;
				return XtraMessageBox.Show(string.Format(ResourceMessage.BenhKhongKhuyenKhichDungLamBenhChinh, arg), Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return true;
		}

		private void ClearChanDoanTD()
		{
			try
			{
				cboChanDoanTD.EditValue = null;
				txtMaChanDoanTD.Text = null;
				txtDialogText.Text = null;
				chkHasDialogText.Enabled = false;
				cboChanDoanTD.Properties.Buttons[1].Visible = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ChangecboChanDoanTD()
		{
			try
			{
				HIS_ICD hIS_ICD = DataStore.Icds.FirstOrDefault((HIS_ICD o) => o.ID == Parse.ToInt64((cboChanDoanTD.EditValue ?? "0").ToString()));
				if (hIS_ICD == null)
				{
					return;
				}
				if (!ConfirmIcdNotRecommendMain(hIS_ICD))
				{
					ClearChanDoanTD();
					cboChanDoanTD.Focus();
					return;
				}
				cboChanDoanTD.Properties.Buttons[1].Visible = true;
				txtMaChanDoanTD.Text = hIS_ICD.ICD_CODE;
				chkHasDialogText.Enabled = true;
				if (autoCheckIcd == "1")
				{
					chkHasDialogText.Checked = true;
				}
				if (chkHasDialogText.Checked)
				{
					txtDialogText.Text = hIS_ICD.ICD_NAME;
					txtDialogText.Focus();
					txtDialogText.SelectAll();
				}
				else
				{
					txtDialogText.Text = hIS_ICD.ICD_NAME;
					chkHasDialogText.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ChangecboChanDoanTD_V2_GanICDNAME(string text)
		{
			try
			{
				chkHasDialogText.Enabled = true;
				chkHasDialogText.Checked = true;
				txtDialogText.Text = text;
				txtDialogText.Focus();
				txtDialogText.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadChuanDoanTDCombo(string searchCode)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					txtMaChanDoanTD.ErrorText = "";
					chkHasDialogText.Enabled = false;
					cboChanDoanTD.Properties.Buttons[1].Visible = false;
					ResetEditorControl.ResetAndFocus(cboChanDoanTD, false);
					return;
				}
				List<HIS_ICD> list = DataStore.Icds.Where((HIS_ICD o) => o.ICD_CODE.ToUpper().Contains(searchCode.ToUpper())).ToList();
				List<HIS_ICD> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((HIS_ICD o) => o.ICD_CODE.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					if (!ConfirmIcdNotRecommendMain(list2[0]))
					{
						ClearChanDoanTD();
						txtMaChanDoanTD.Focus();
						txtMaChanDoanTD.SelectAll();
						return;
					}
					cboChanDoanTD.Properties.Buttons[1].Visible = true;
					cboChanDoanTD.EditValue = list2[0].ID;
					txtMaChanDoanTD.Text = list2[0].ICD_CODE;
					chkHasDialogText.Enabled = true;
					chkHasDialogText.Checked = autoCheckIcd == "1";
					if (chkHasDialogText.Checked)
					{
						txtDialogText.Text = list2[0].ICD_NAME;
						txtDialogText.Focus();
						txtDialogText.SelectAll();
					}
					else
					{
						txtDialogText.Text = list2[0].ICD_NAME;
						chkHasDialogText.Focus();
					}
					txtMaChanDoanTD.ErrorText = "";
				}
				else
				{
					txtMaChanDoanTD.ErrorText = ResourceMessage.MaBenhChinhKhongHopLe;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadHeinRightRouterTypeCombo(string searchCode)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					ResetEditorControl.ResetAndFocus(cboHeinRightRoute, true);
					return;
				}
				List<HeinRightRouteTypeData> list = DataStore.HeinRightRouteTypes.Where((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode.Contains(searchCode)).ToList();
				List<HeinRightRouteTypeData> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					cboHeinRightRoute.Properties.Buttons[1].Visible = true;
					cboHeinRightRoute.EditValue = list2[0].HeinRightRouteTypeCode;
					txtHeinRightRouteCode.Text = list2[0].HeinRightRouteTypeCode;
					if (list2[0].HeinRightRouteTypeCode == "CC")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC, true);
					}
					else if (list2[0].HeinRightRouteTypeCode == "HK")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT, true);
					}
					else
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT, true);
					}
				}
				else
				{
					ResetEditorControl.ResetAndFocus(cboHeinRightRoute, true);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadTranPatiFormCombo(string searchCode)
		{
			try
			{
				cboHinhThucChuyen.Properties.Buttons[1].Visible = false;
				if (string.IsNullOrEmpty(searchCode))
				{
					ResetEditorControl.ResetAndFocus(cboHinhThucChuyen, true);
					return;
				}
				List<HIS_TRAN_PATI_FORM> list = DataStore.TranPatiForms.Where((HIS_TRAN_PATI_FORM o) => o.TRAN_PATI_FORM_CODE.Contains(searchCode)).ToList();
				List<HIS_TRAN_PATI_FORM> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((HIS_TRAN_PATI_FORM o) => o.TRAN_PATI_FORM_CODE.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					cboHinhThucChuyen.EditValue = list2[0].ID;
					txtMaHinhThucChuyen.Text = list2[0].TRAN_PATI_FORM_CODE;
					cboHinhThucChuyen.Properties.Buttons[1].Visible = true;
					txtMaLyDoChuyen.Focus();
					txtMaLyDoChuyen.SelectAll();
				}
				else
				{
					ResetEditorControl.ResetAndFocus(cboHinhThucChuyen, true);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadTranPatiReasonCombo(string searchCode)
		{
			try
			{
				cboLyDoChuyen.Properties.Buttons[1].Visible = false;
				if (string.IsNullOrEmpty(searchCode))
				{
					ResetEditorControl.ResetAndFocus(cboLyDoChuyen, true);
					return;
				}
				List<HIS_TRAN_PATI_REASON> list = DataStore.TranPatiReasons.Where((HIS_TRAN_PATI_REASON o) => o.TRAN_PATI_REASON_CODE.ToLower().Contains(searchCode.ToLower())).ToList();
				List<HIS_TRAN_PATI_REASON> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((HIS_TRAN_PATI_REASON o) => o.TRAN_PATI_REASON_CODE.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					cboLyDoChuyen.EditValue = list2[0].ID;
					txtMaLyDoChuyen.Text = list2[0].TRAN_PATI_REASON_CODE;
					cboLyDoChuyen.Properties.Buttons[1].Visible = true;
					FocusMoveOut();
				}
				else
				{
					ResetEditorControl.ResetAndFocus(cboLyDoChuyen, true);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadNoiDKKCBBDCombo(string searchCode)
		{
			try
			{
				if (string.IsNullOrEmpty(searchCode))
				{
					ResetEditorControl.ResetAndFocus(cboDKKCBBD, false, true);
					return;
				}
				List<MediOrgADO> list = DataStore.MediOrgs.Where((MediOrgADO o) => o.MEDI_ORG_CODE.Contains(searchCode)).ToList();
				if (list != null)
				{
					HIS_MEDI_ORG hIS_MEDI_ORG = null;
					hIS_MEDI_ORG = ((list.Count == 1) ? list[0] : DataStore.MediOrgs.FirstOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE.Equals(searchCode)));
					if (hIS_MEDI_ORG != null)
					{
						cboDKKCBBD.EditValue = hIS_MEDI_ORG.MEDI_ORG_CODE;
						txtMaDKKCBBD.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
						MediOrgSelectRowChange(true, (cboNoiSong.EditValue ?? "").ToString());
						string text = txtSoThe.Text;
						text = text.Replace(" ", "").ToUpper().Trim();
						text = HeinUtils.TrimHeinCardNumber(text);
						CheckExamHistoryFromBHXHApi(text);
					}
					else
					{
						ResetEditorControl.ResetAndFocus(cboDKKCBBD, false, true);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LoadNoiChuyenDenCombo(string searchCode)
		{
			try
			{
				cboNoiChuyenDen.Properties.Buttons[1].Visible = false;
				if (string.IsNullOrEmpty(searchCode))
				{
					ResetEditorControl.ResetAndFocus(cboNoiChuyenDen, true);
					return;
				}
				List<MediOrgADO> list = DataStore.MediOrgs.Where((MediOrgADO o) => o.MEDI_ORG_CODE.Contains(searchCode)).ToList();
				List<MediOrgADO> list2 = ((list == null || list.Count <= 0) ? null : ((list.Count == 1) ? list : list.Where((MediOrgADO o) => o.MEDI_ORG_CODE.ToUpper() == searchCode.ToUpper()).ToList()));
				if (list2 != null && list2.Count == 1)
				{
					cboNoiChuyenDen.EditValue = list2[0].MEDI_ORG_CODE;
					txtMaNoiChuyenDen.Text = list2[0].MEDI_ORG_CODE;
					cboNoiChuyenDen.Properties.Buttons[1].Visible = true;
					ProcessLevelOfMediOrg();
					txtMaChanDoanTD.Focus();
					txtMaChanDoanTD.SelectAll();
				}
				else
				{
					ResetEditorControl.ResetAndFocus(cboNoiChuyenDen, true);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FillDataPatientOldYnder6(bool IsChild)
		{
			try
			{
				chkBaby.Enabled = IsChild;
				if (!chkBaby.Enabled)
				{
					chkBaby.Checked = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void UpdateDataFormIntoPatientProfile(HisPatientProfileSDO patientProfileSDO)
		{
			try
			{
				if (patientProfileSDO != null && patientProfileSDO.HisPatientTypeAlter != null && patientProfileSDO.HisPatientTypeAlter.RIGHT_ROUTE_TYPE_CODE != "CC")
				{
					if ((cboChanDoanTD.Enabled && txtMaChanDoanTD.Enabled) || entity.IsInitFromCallPatientTypeAlter)
					{
						if (patientProfileSDO.HisTreatment == null)
						{
							patientProfileSDO.HisTreatment = new HIS_TREATMENT();
						}
						patientProfileSDO.HisTreatment.IS_TRANSFER_IN = (short)1;
						patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_CODE = txtMaChanDoanTD.Text.Trim();
						object editValue = cboPatientCode.EditValue;
						if (!string.IsNullOrEmpty((editValue != null) ? editValue.ToString() : null))
						{
							patientProfileSDO.HisTreatment.HEIN_PATIENT_TYPE_CODE = cboPatientCode.EditValue.ToString();
						}
						else
						{
							patientProfileSDO.HisTreatment.HEIN_PATIENT_TYPE_CODE = "";
						}
						if (chkHasDialogText.Checked)
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_NAME = txtDialogText.Text.Trim();
						}
						else
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_NAME = cboChanDoanTD.Text;
						}
						patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_CODE = (txtMaNoiChuyenDen.EditValue ?? "").ToString();
						patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_NAME = cboNoiChuyenDen.Text;
						if (txtMaHinhThucChuyen.Enabled || entity.IsInitFromCallPatientTypeAlter)
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_CMKT = GetInCMKT();
							if (cboHinhThucChuyen.EditValue != null)
							{
								patientProfileSDO.HisTreatment.TRANSFER_IN_FORM_ID = Parse.ToInt64((cboHinhThucChuyen.EditValue ?? "0").ToString());
							}
							if (cboLyDoChuyen.EditValue != null)
							{
								patientProfileSDO.HisTreatment.TRANSFER_IN_REASON_ID = Parse.ToInt64((cboLyDoChuyen.EditValue ?? "0").ToString());
							}
						}
						else
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_CMKT = null;
							patientProfileSDO.HisTreatment.TRANSFER_IN_FORM_ID = null;
							patientProfileSDO.HisTreatment.TRANSFER_IN_REASON_ID = null;
						}
						if ((dtTransferInTimeFrom.Enabled || entity.IsInitFromCallPatientTypeAlter) && dtTransferInTimeFrom.EditValue != null && dtTransferInTimeFrom.DateTime != DateTime.MinValue)
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_FROM = Parse.ToInt64(System.Convert.ToDateTime(dtTransferInTimeFrom.EditValue).ToString("yyyyMMdd") + "000000");
						}
						else
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_FROM = null;
						}
						if ((dtTransferInTimeTo.Enabled || entity.IsInitFromCallPatientTypeAlter) && dtTransferInTimeTo.EditValue != null && dtTransferInTimeTo.DateTime != DateTime.MinValue)
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_TO = Parse.ToInt64(System.Convert.ToDateTime(dtTransferInTimeTo.EditValue).ToString("yyyyMMdd") + "235959");
						}
						else
						{
							patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_TO = null;
						}
						patientProfileSDO.HisTreatment.TRANSFER_IN_CODE = txtInCode.Text.Trim();
					}
					if (cboHeinRightRoute.EditValue != null && cboHeinRightRoute.EditValue.ToString() == "HK")
					{
						patientProfileSDO.HisTreatment.IS_TRANSFER_IN = (short)1;
						patientProfileSDO.HisTreatment.TRANSFER_IN_CODE = txtInCode.Text.Trim();
					}
				}
				else if (patientProfileSDO.HisTreatment != null)
				{
					patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_CODE = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_ICD_NAME = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_CODE = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_MEDI_ORG_NAME = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_CMKT = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_FORM_ID = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_REASON_ID = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_FROM = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_TIME_TO = null;
					patientProfileSDO.HisTreatment.TRANSFER_IN_CODE = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void UpdateDataFormIntoPatientTypeAlter(HisPatientProfileSDO patientProfileSDO)
		{
			try
			{
				if (patientProfileSDO.HisTreatment == null)
				{
					patientProfileSDO.HisTreatment = new HIS_TREATMENT();
				}
				if (patientProfileSDO.HisPatientTypeAlter == null)
				{
					patientProfileSDO.HisPatientTypeAlter = new HIS_PATIENT_TYPE_ALTER();
				}
				if (chkHasDobCertificate.Checked)
				{
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_NUMBER = "";
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_FROM_TIME = null;
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_TO_TIME = null;
					patientProfileSDO.HisPatientTypeAlter.HAS_BIRTH_CERTIFICATE = "C";
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.HAS_BIRTH_CERTIFICATE = "K";
				}
				object editValue = cboPatientCode.EditValue;
				if (!string.IsNullOrEmpty((editValue != null) ? editValue.ToString() : null))
				{
					patientProfileSDO.HisTreatment.HEIN_PATIENT_TYPE_CODE = cboPatientCode.EditValue.ToString();
				}
				else
				{
					patientProfileSDO.HisTreatment.HEIN_PATIENT_TYPE_CODE = "";
				}
				patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_NUMBER = HeinUtils.TrimHeinCardNumber(txtSoThe.Text);
				patientProfileSDO.HisPatientTypeAlter.RIGHT_ROUTE_CODE = "DT";
				patientProfileSDO.HisPatientTypeAlter.RIGHT_ROUTE_TYPE_CODE = (cboHeinRightRoute.EditValue ?? "").ToString();
				patientProfileSDO.HisPatientTypeAlter.JOIN_5_YEAR = (chkJoin5Year.Checked ? "C" : "K");
				patientProfileSDO.HisPatientTypeAlter.PAID_6_MONTH = (chkPaid6Month.Checked ? "C" : "K");
				patientProfileSDO.HisPatientTypeAlter.LIVE_AREA_CODE = (cboNoiSong.EditValue ?? "").ToString();
				patientProfileSDO.HisPatientTypeAlter.HEIN_MEDI_ORG_CODE = (cboDKKCBBD.EditValue ?? "").ToString();
				patientProfileSDO.HisPatientTypeAlter.HEIN_MEDI_ORG_NAME = cboDKKCBBD.Text;
				patientProfileSDO.HisPatientTypeAlter.LEVEL_CODE = HeinLevelCodeCurrent;
				patientProfileSDO.HisPatientTypeAlter.TREATMENT_TYPE_ID = TreatmentTypeIdExam;
				if (chkTempQN.Checked)
				{
					patientProfileSDO.HisPatientTypeAlter.IS_TEMP_QN = (short)1;
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.IS_TEMP_QN = null;
				}
				if (!rdoRightRoute.Checked && !rdoWrongRoute.Checked)
				{
					rdoRightRoute.Checked = true;
					rdoWrongRoute.Checked = !rdoRightRoute.Checked;
					LogSystem.Info("Tiep don BN bhyt, khong xac dinh duoc dung tuyen - trai tuyen, mac dinh lay dung tuyen. Du lieu dau vao: HEIN_CARD_NUMBER = " + txtSoThe.Text + " | HEIN_MEDI_ORG_CODE = " + (cboDKKCBBD.EditValue ?? "").ToString());
				}
				if (rdoRightRoute.Checked)
				{
					patientProfileSDO.HisPatientTypeAlter.RIGHT_ROUTE_CODE = "DT";
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.RIGHT_ROUTE_CODE = "TT";
				}
				dtHeinCardFromTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
				if (dtHeinCardFromTime.EditValue != null)
				{
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_FROM_TIME = Parse.ToInt64(dtHeinCardFromTime.DateTime.ToString("yyyyMMdd") + "000000");
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_FROM_TIME = null;
				}
				dtHeinCardToTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
				if (dtHeinCardToTime.EditValue != null)
				{
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_TO_TIME = Parse.ToInt64(dtHeinCardToTime.DateTime.ToString("yyyyMMdd") + "000000");
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.HEIN_CARD_TO_TIME = null;
				}
				if (dtDu5Nam.EditValue != null || !string.IsNullOrEmpty(txtDu5Nam.Text))
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtDu5Nam.Text);
					if (dtDu5Nam.DateTime != dateTime)
					{
						dtDu5Nam.EditValue = dateTime;
					}
					patientProfileSDO.HisPatientTypeAlter.JOIN_5_YEAR_TIME = Parse.ToInt64(dtDu5Nam.DateTime.ToString("yyyyMMdd") + "000000");
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.JOIN_5_YEAR_TIME = null;
				}
				string text = txtFreeCoPainTime.Text.Trim();
				if (text.Length == 8)
				{
					text = text.Substring(0, 2) + "/" + text.Substring(2, 2) + "/" + text.Substring(4, 4);
				}
				dtFreeCoPainTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(text);
				if (!string.IsNullOrEmpty(text) && dtFreeCoPainTime.EditValue != null)
				{
					txtFreeCoPainTime.Text = text;
					patientProfileSDO.HisPatientTypeAlter.FREE_CO_PAID_TIME = Parse.ToInt64(dtFreeCoPainTime.DateTime.ToString("yyyyMMdd"));
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.FREE_CO_PAID_TIME = null;
				}
				patientProfileSDO.HisPatientTypeAlter.ADDRESS = txtAddress.Text.Trim();
				patientProfileSDO.HisPatientTypeAlter.HNCODE = txtHNCode.Text.Trim();
				if (lciKhongKTHSD.Visibility == LayoutVisibility.Always && checkKhongKTHSD.Checked)
				{
					patientProfileSDO.HisPatientTypeAlter.IS_NO_CHECK_EXPIRE = 1;
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.IS_NO_CHECK_EXPIRE = null;
				}
				patientProfileSDO.HisPatientTypeAlter.IS_NEWBORN = (chkBaby.Checked ? new short?(1) : ((short?)null));
				patientProfileSDO.HisPatientTypeAlter.HAS_WORKING_LETTER = (chkHasWorkingLetter.Checked ? new short?(1) : ((short?)null));
				patientProfileSDO.HisPatientTypeAlter.HAS_ABSENT_LETTER = (chkHasAbsentLetter.Checked ? new short?(1) : ((short?)null));
				patientProfileSDO.HisPatientTypeAlter.IS_TT46 = (chkTt46.Checked ? new short?(1) : ((short?)null));
				patientProfileSDO.HisPatientTypeAlter.TT46_NOTE = txtTt46.Text.Trim();
				string text2 = new string((txtCoPaidAccumulate.Text ?? "").Where(new Func<char, bool>(char.IsDigit)).ToArray());
				if (!string.IsNullOrEmpty(text2))
				{
					patientProfileSDO.HisPatientTypeAlter.CO_PAID_ACCUMULATE_AMOUNT = Parse.ToInt64(text2);
				}
				else
				{
					patientProfileSDO.HisPatientTypeAlter.CO_PAID_ACCUMULATE_AMOUNT = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboHeinRightRoute()
		{
			try
			{
				cboHeinRightRoute.Properties.DataSource = DataStore.HeinRightRouteTypes;
				cboHeinRightRoute.Properties.DisplayMember = "RIGHT_ROUTE_TYPE_NAME";
				cboHeinRightRoute.Properties.ValueMember = "ID";
				cboHeinRightRoute.Properties.ForceInitialize();
				cboHeinRightRoute.Properties.Columns.Clear();
				cboHeinRightRoute.Properties.Columns.Add(new LookUpColumnInfo("RIGHT_ROUTE_TYPE_CODE", "", 100));
				cboHeinRightRoute.Properties.Columns.Add(new LookUpColumnInfo("RIGHT_ROUTE_TYPE_NAME", "", 200));
				cboHeinRightRoute.Properties.ShowHeader = false;
				cboHeinRightRoute.Properties.ImmediatePopup = true;
				cboHeinRightRoute.Properties.DropDownRows = 20;
				cboHeinRightRoute.Properties.PopupWidth = 300;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public Template__HeinBHYT1()
			: this(null)
		{
		}

		public Template__HeinBHYT1(DataInitHeinBhyt data)
		{
			try
			{
				InitializeComponent();
				if (data == null)
				{
					return;
				}
				entity = data;
				IsReset = data.IsReset;
				isDefaultInit = true;
				HeinPatientCode = data.HeinPatientCode;
				DataStore.MediOrgs = data.MediOrgs.Select((HIS_MEDI_ORG m) => new MediOrgADO(m)).ToList();
				List<HIS_ICD> source = data.Icds ?? new List<HIS_ICD>();
				if (data.IsHideIcdDeathCauseOnly)
				{
					source = source.Where((HIS_ICD o) => o.IS_DEATH_CAUSE_ONLY != 1).ToList();
				}
				DataStore.IcdADOs = source.Select((HIS_ICD m) => new IcdADO(m)).ToList();
				DataStore.LiveAreas = data.LiveAreas;
				DataStore.Icds = source.Where((HIS_ICD p) => p.IS_ACTIVE == 1).ToList();
				DataStore.TranPatiForms = data.TranPatiForms;
				DataStore.TranPatiReasons = data.TranPatiReasons;
				DataStore.HeinRightRouteTypes = data.HeinRightRouteTypes;
				DataStore.TreatmentTypes = data.TreatmentTypes;
				DataStore.Genders = data.Genders;
				DataStore.PatientTypes = data.PatientTypes;
				MediOrgCodeCurrent = data.MEDI_ORG_CODE__CURRENT;
				MediOrgCodesAccepts = data.MEDI_ORG_CODES__ACCEPTs;
				HeinLevelCodeCurrent = data.HEIN_LEVEL_CODE__CURRENT;
				TreatmentTypeIdExam = data.TREATMENT_TYPE_ID__EXAM;
				SysMediOrgCode = data.SYS_MEDI_ORG_CODE;
				PatientTypeIdBHYT = data.PATIENT_TYPE_ID__BHYT;
				PatientTypeId = data.PatientTypeId;
				lblEditIcd.Enabled = false;
				treatmentTypeId = data.treatmentTypeId;
				isVisibleControl = data.isVisibleControl;
				isShowCheckKhongKTHSD = data.IsShowCheckKhongKTHSD;
				IsNotRequiredRightTypeInCaseOfHavingAreaCode = data.IsNotRequiredRightTypeInCaseOfHavingAreaCode;
				autoCheckIcd = data.AutoCheckIcd;
				IsDefaultRightRouteType = data.IsDefaultRightRouteType;
				IsEdit = data.IsEdit;
				IsTempQN = data.IsTempQN;
				IsObligatoryTranferMediOrg = data.IsObligatoryTranferMediOrg;
				ObligatoryTranferMediOrg = data.ObligatoryTranferMediOrg;
				IsDungTuyenCapCuuByTime = data.IsDungTuyenCapCuuByTime;
				dlgProcessFillDataCareerUnder6AgeByHeinCardNumber = data.ProcessFillDataCareerUnder6AgeByHeinCardNumber;
				dlgfillDataPatientSDOToRegisterForm = data.FillDataPatientSDOToRegisterForm;
				dlgautoCheckCC = data.AutoCheckCC;
				TreatmentTypeId1 = data.DeleteTreatmentTypeId;
				dlgcheckExamHistory = data.CheckExamHistory;
				dlgsetFocusMoveOut = data.SetFocusMoveOut;
				dlgsetShortcutKeyDown = data.SetShortcutKeyDown;
				_DelegateSetRelativeAddress = data.SetRelativeAddress;
				actChangePatientDob = data.ActChangePatientDob;
				ExceedDayAllow = data.ExceedDayAllow;
				PatientId = data.PatientId;
				ActionType = data.ActionType;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Template__HeinBHYT1_Load(object sender, EventArgs e)
		{
			try
			{
				LogSystem.Debug("Template__HeinBHYT1_Load()");
				SetCaptionByLanguageKeyNew();
				HisConfigCFG.LoadConfig();
				SetColorForHeinPatientType();
				ResetPatientCode();
				InitComboPatientCode();
				if (isDefaultInit)
				{
					InitData(entity);
				}
				if (IsReset)
				{
					cboPatientCode.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void LogChonMaDoiTuongKcb(List<HIS_HEIN_PATIENT_TYPE> heinList, string dkbdCode, HIS_HEIN_PATIENT_TYPE matched, string ghiChu)
		{
			try
			{
				int num = heinList.Count((HIS_HEIN_PATIENT_TYPE o) => !string.IsNullOrWhiteSpace(o.HEIN_MEDI_ORG_CODES));
				LogSystem.Debug("ChonMaDoiTuongKCB: maDKKCBBanDau=" + (dkbdCode ?? "(trong)") + "___soBanGhiSauLoc=" + heinList.Count + "___soBanGhiDaCauHinhMaCoSo=" + num + "___maChon=" + ((matched != null) ? matched.HEIN_PATIENT_TYPE_CODE : "(trong)") + "___ghiChu=" + ghiChu);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ResetHeinPatientCodeForRecalc()
		{
			try
			{
				if (!isFillingHeinDataFromDb && !isClickCboPatientTypeCode)
				{
					HeinPatientCode = null;
					firstCheck = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool IsMediOrgCodeListSpecific(string codes)
		{
			if (!string.IsNullOrWhiteSpace(codes))
			{
				return codes.Trim() != "*";
			}
			return false;
		}

		private HIS_HEIN_PATIENT_TYPE GetHeinPatientTypeByMediOrgPriority(List<HIS_HEIN_PATIENT_TYPE> heinList, string dkbdCode)
		{
			try
			{
				List<HIS_HEIN_PATIENT_TYPE> list = new List<HIS_HEIN_PATIENT_TYPE>();
				if (!string.IsNullOrEmpty(dkbdCode))
				{
					list = heinList.Where((HIS_HEIN_PATIENT_TYPE o) => IsMediOrgCodeListSpecific(o.HEIN_MEDI_ORG_CODES) && o.HEIN_MEDI_ORG_CODES.Split(new char[2] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Any((string c) => c.Trim() == dkbdCode)).ToList();
				}
				List<HIS_HEIN_PATIENT_TYPE> list2 = heinList.Where((HIS_HEIN_PATIENT_TYPE o) => !IsMediOrgCodeListSpecific(o.HEIN_MEDI_ORG_CODES)).ToList();
				List<HIS_HEIN_PATIENT_TYPE> list3 = ((list.Count > 0) ? list : list2);
				if (list3.Count == 0)
				{
					LogChonMaDoiTuongKcb(heinList, dkbdCode, null, "khong ban ghi nao ap dung -> de trong");
					return null;
				}
				HIS_HEIN_PATIENT_TYPE val = (from o in list3
					orderby (!o.NUM_ORDER.HasValue) ? 1 : 0, o.NUM_ORDER, o.ID descending
					select o).FirstOrDefault();
				LogChonMaDoiTuongKcb(heinList, dkbdCode, val, (list.Count > 0) ? "khop ma co so cu the" : "ap dung moi co so (de trong hoac *)");
				return val;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				return null;
			}
		}

		private void InitComboPatientCode()
		{
			try
			{
				List<HIS_HEIN_PATIENT_TYPE> list = (from o in BackendDataWorker.Get<HIS_HEIN_PATIENT_TYPE>()
					where o.IS_ACTIVE == 1
					select o).ToList();
				List<HIS_HEIN_PATIENT_TYPE> source = new List<HIS_HEIN_PATIENT_TYPE>();
				long actionType = ActionType;
				if (!firstCheck && !isClickCboPatientTypeCode)
				{
					string rightRouteCode = null;
					if (rdoRightRoute.Checked)
					{
						rightRouteCode = "DT";
					}
					else if (rdoWrongRoute.Checked)
					{
						rightRouteCode = "TT";
					}
					if (!string.IsNullOrEmpty(rightRouteCode))
					{
						source = list.Where((HIS_HEIN_PATIENT_TYPE o) => o.RIGHT_ROUTE_CODE == rightRouteCode).ToList();
					}
					string rightRouteTypeCode = null;
					if (cboHeinRightRoute.EditValue != null)
					{
						string text = cboHeinRightRoute.EditValue.ToString();
						switch (text)
						{
						case "CC":
						case "GT":
						case "HK":
						case "TH":
							rightRouteTypeCode = text;
							break;
						}
					}
					source = source.Where((HIS_HEIN_PATIENT_TYPE o) => o.RIGHT_ROUTE_TYPE_CODE == rightRouteTypeCode).ToList();
					if (TreatmentTypeId > 0)
					{
						source = source.Where((HIS_HEIN_PATIENT_TYPE o) => string.IsNullOrEmpty(o.TREATMENT_TYPE_IDS) || (from s in o.TREATMENT_TYPE_IDS.Split(',')
							select s.Trim()).Any((string id) => id == TreatmentTypeId.ToString())).ToList();
					}
					if (source.Count > 0 && HeinPatientCode == null)
					{
						HIS_HEIN_PATIENT_TYPE heinPatientTypeByMediOrgPriority = GetHeinPatientTypeByMediOrgPriority(source, (txtMaDKKCBBD.Text ?? "").Trim());
						cboPatientCode.EditValue = ((heinPatientTypeByMediOrgPriority != null) ? heinPatientTypeByMediOrgPriority.HEIN_PATIENT_TYPE_CODE : null);
					}
					else
					{
						cboPatientCode.EditValue = HeinPatientCode;
					}
				}
				cboPatientCode.Properties.DataSource = list;
				cboPatientCode.Properties.DisplayMember = "HEIN_PATIENT_TYPE_CODE";
				cboPatientCode.Properties.ValueMember = "HEIN_PATIENT_TYPE_CODE";
				cboPatientCode.Properties.TextEditStyle = TextEditStyles.Standard;
				cboPatientCode.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboPatientCode.ForceInitialize();
				cboPatientCode.Properties.View.Columns.Clear();
				cboPatientCode.Properties.ImmediatePopup = true;
				cboPatientCode.Properties.AutoComplete = false;
				cboPatientCode.Properties.NullText = null;
				cboPatientCode.Properties.AllowNullInput = DefaultBoolean.True;
				GridColumn gridColumn = cboPatientCode.Properties.View.Columns.AddField("HEIN_PATIENT_TYPE_CODE");
				gridColumn.Caption = "Mã";
				gridColumn.Visible = true;
				gridColumn.VisibleIndex = 0;
				gridColumn.Width = 50;
				gridColumn.MinWidth = 50;
				GridColumn gridColumn2 = cboPatientCode.Properties.View.Columns.AddField("DESCRIPTION");
				gridColumn2.Caption = "Mô tả";
				gridColumn2.Visible = true;
				gridColumn2.VisibleIndex = 1;
				gridColumn2.ColumnEdit = repositoryItemMemoEdit1;
				gridColumn2.AppearanceCell.TextOptions.Trimming = Trimming.Word;
				gridColumn2.AppearanceCell.TextOptions.WordWrap = WordWrap.Wrap;
				gridColumn2.Width = 400;
				cboPatientCode.Properties.View.OptionsView.RowAutoHeight = true;
				cboPatientCode.Properties.PopupFormMinSize = new Size(400, 200);
				cboPatientCode.Properties.View.ActiveFilter.Clear();
				cboPatientCode.EditValueChanged += new EventHandler(cboPatientCode_EditValueChanged);
				cboPatientCode.ButtonClick += new ButtonPressedEventHandler(cboPatientCode_ButtonClick);
				cboPatientCode.TextChanged += new EventHandler(cboPatientCode_TextChanged_1);
				cboPatientCode.Properties.ImmediatePopup = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetSize()
		{
			try
			{
				cboPatientCode.Properties.View.Columns.AddField("HEIN_PATIENT_TYPE_CODE").Width = 20;
				cboPatientCode.Properties.View.Columns.AddField("DESCRIPTION").Width = 400;
				cboPatientCode.Properties.PopupFormMinSize = new Size(400, 200);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ResetPatientCode()
		{
			try
			{
				cboPatientCode.Text = "";
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitData(DataInitHeinBhyt data)
		{
			try
			{
				InitDataToControl();
				VisibleControl(isVisibleControl);
				DisableControlWhenPatientTypeQN(IsTempQN, false);
				CheckTempQN();
				CheckHSDAndTECard();
				ValidControl();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void AutoSelectEmergency(DataInitHeinBhyt data)
		{
			try
			{
				if (data.IsAutoSelectEmergency)
				{
					cboHeinRightRoute.EditValue = "CC";
					txtHeinRightRouteCode.Text = "CC";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CheckTempQN()
		{
			try
			{
				if (!IsTempQN)
				{
					lblHeincardNumber.Size = new Size(lciTempQN.Size.Width + lblHeincardNumber.Size.Width - layoutControlItem1.Size.Width, lblHeincardNumber.Size.Height);
				}
				lciTempQN.Visibility = ((!IsTempQN) ? LayoutVisibility.Never : LayoutVisibility.Always);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetLanguageKey()
		{
			try
			{
				lciFreeCoPainTime.Text = Inventec.Common.Resource.Get.Value("lciFreeCoPainTime.Text", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				txtFreeCoPainTime.ToolTip = Inventec.Common.Resource.Get.Value("txtFreeCoPainTime.Tooltip", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblCaptionHasDobCertificate.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_CAPTION_HAS_DOB_CERTIFICATE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblHeincardNumber.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_HEINCARD_NUMBER", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblHeincardFromDate.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_HEINCARD_FROM_DATE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblHeincardToDate.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_HEINCARD_TO_DATE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblCaptionAddress.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_CAPTION_ADDRESS", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblHeincardMediOrg.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_HEINCARD_MEDIORG", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciInCode.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT__LCI_IN_CODE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciHNCode.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT__LCI_HNCODE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblRightRouteType.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_RIGHT_ROUTE_TYPE", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblMediRecordMediOrgForm.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_MEDI_RECORD_MEDI_ORG_FORM", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciMediRecordRouteTransfer.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LAYOUT_CONTROL_ITEM12", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciMediRecordNoRouteTransfer.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LAYOUT_CONTROL_ITEM13", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciTransPatiFormCode.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_MEDI_RECORD_TRANS_PATI_FORM", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciTransPatiReasonCode.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_MEDI_RECORD_TRANS_PATI_REASON", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblMediRecordLiveArea.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_MEDI_RECORD_LIVE_AREA", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lcichkJoin5Year.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LCI_JOIN_5_YEAR", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lcichkPaid6Month.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LCI_PAID_6_MONTH", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblMediRecordBenefitSymbol.Text = Inventec.Common.Resource.Get.Value("IVT_LANGUAGE_KEY_UCHEIN_BHYT_LBL_MEDI_RECORD_BENEFIT_SYMBOL", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lciIcdMain.Text = Inventec.Common.Resource.Get.Value("lciIcdMain.Text", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lcirdoWrongRoute.Text = Inventec.Common.Resource.Get.Value("lcirdoWrongRoute.Text", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lcirdoRightRoute.Text = Inventec.Common.Resource.Get.Value("lcirdoRightRoute.Text", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				lblEditIcd.Text = Inventec.Common.Resource.Get.Value("lblEditIcd.Text", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
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
				ResourceLanguageManager.LanguageResource = new ResourceManager("His.UC.UCHein.Resources.Lang", typeof(Template__HeinBHYT1).Assembly);
				dtHeinCardToTime.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.dtHeinCardToTime.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				dtHeinCardFromTime.Properties.NullValuePrompt = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.dtHeinCardFromTime.Properties.NullValuePrompt", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				layoutControl1.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.layoutControl1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				btnCheckInfoBHYT.ToolTip = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.btnCheckInfoBHYT.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				rdoWrongRoute.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.rdoWrongRoute.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboNoiSong.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboNoiSong.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkTempQN.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkTempQN.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				txtFreeCoPainTime.ToolTip = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.txtFreeCoPainTime.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				checkKhongKTHSD.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.checkKhongKTHSD.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				rdoRightRoute.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.rdoRightRoute.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkPaid6Month.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkPaid6Month.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkPaid6Month.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem1.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboChanDoanTD.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboChanDoanTD.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkHasDialogText.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkHasDialogText.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboLyDoChuyen.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboLyDoChuyen.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboHeinRightRoute.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboHeinRightRoute.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboDKKCBBD.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboDKKCBBD.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboNoiChuyenDen.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboNoiChuyenDen.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboHinhThucChuyen.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboHinhThucChuyen.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkJoin5Year.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkJoin5Year.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkJoin5Year.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem2.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkMediRecordNoRouteTransfer.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkMediRecordNoRouteTransfer.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkMediRecordNoRouteTransfer.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem3.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkMediRecordRouteTransfer.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkMediRecordRouteTransfer.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkMediRecordRouteTransfer.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem4.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboSoThe.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem5.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				cboSoThe.Properties.NullText = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.cboSoThe.Properties.NullText", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkHasDobCertificate.Properties.Caption = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.chkHasDobCertificate.Properties.Caption", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				chkHasDobCertificate.ToolTip = Inventec.Common.Resource.Get.Value("toolTipItem6.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblCaptionHasDobCertificate.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblCaptionHasDobCertificate.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblHeincardNumber.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblHeincardNumber.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblHeincardToDate.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblHeincardToDate.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblHeincardFromDate.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblHeincardFromDate.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblCaptionAddress.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblCaptionAddress.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblMediRecordMediOrgForm.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblMediRecordMediOrgForm.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblHeincardMediOrg.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblHeincardMediOrg.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblRightRouteType.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblRightRouteType.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciTransPatiReasonCode.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciTransPatiReasonCode.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblMediRecordBenefitSymbol.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblMediRecordBenefitSymbol.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciIcdMain.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciIcdMain.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblEditIcd.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblEditIcd.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciMediRecordRouteTransfer.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciMediRecordRouteTransfer.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciMediRecordNoRouteTransfer.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciMediRecordNoRouteTransfer.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciTransPatiFormCode.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciTransPatiFormCode.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lcirdoWrongRoute.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lcirdoWrongRoute.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lcirdoRightRoute.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lcirdoRightRoute.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciKhongKTHSD.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciKhongKTHSD.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciHNCode.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciHNCode.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciCoPaidAccumulate.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciCoPaidAccumulate.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lcichkJoin5Year.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lcichkJoin5Year.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lcichkPaid6Month.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lcichkPaid6Month.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciTempQN.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciTempQN.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lblMediRecordLiveArea.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lblMediRecordLiveArea.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciDu5Nam.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciDu5Nam.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciDu5Nam.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciDu5Nam.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciFordtTransferInTimeFrom.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciFordtTransferInTimeFrom.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciFordtTransferInTimeFrom.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciFordtTransferInTimeFrom.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciFordtTransferInTimeTo.OptionsToolTip.ToolTip = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciFordtTransferInTimeTo.OptionsToolTip.ToolTip", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
				lciFordtTransferInTimeTo.Text = Inventec.Common.Resource.Get.Value("Template__HeinBHYT1.lciFordtTransferInTimeTo.Text", ResourceLanguageManager.LanguageResource, Inventec.Desktop.Common.LanguageManager.LanguageManager.GetCulture());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetValidate(string heinMediOrgCode)
		{
			try
			{
				if (("1" == HeinLevelCodeCurrent || "2" == HeinLevelCodeCurrent) && !string.IsNullOrEmpty(heinMediOrgCode) && MediOrgCodeCurrent != heinMediOrgCode && (string.IsNullOrWhiteSpace(SysMediOrgCode) || !SysMediOrgCode.Contains(heinMediOrgCode)))
				{
					if (IsTempQN && chkTempQN.Checked)
					{
						lblRightRouteType.AppearanceItemCaption.ForeColor = default(Color);
					}
					else
					{
						lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Maroon;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void CheckHSDAndTECard()
		{
			try
			{
				lciKhongKTHSD.Visibility = LayoutVisibility.Never;
				checkKhongKTHSD.Checked = false;
				if (isShowCheckKhongKTHSD == "1" && dtHeinCardToTime.DateTime != DateTime.MinValue)
				{
					string text = txtSoThe.Text;
					text = text.Replace(" ", "").ToUpper().Trim();
					text = HeinUtils.TrimHeinCardNumber(text);
					if (!string.IsNullOrEmpty(text) && text.StartsWith("TE") && dtHeinCardToTime.DateTime.Date < DateTime.Now.Date)
					{
						lciKhongKTHSD.Visibility = LayoutVisibility.Always;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void VisibleControl(long isVisibleControl)
		{
			try
			{
				if (isVisibleControl == 1)
				{
					lciMediRecordRouteTransfer.Visibility = LayoutVisibility.Never;
					lciMediRecordNoRouteTransfer.Visibility = LayoutVisibility.Never;
					lciTransPatiFormCode.Visibility = LayoutVisibility.Never;
					lciTransPatiFormCbo.Visibility = LayoutVisibility.Never;
					lciTransPatiReasonCode.Visibility = LayoutVisibility.Never;
					lciTransPatiReasoncbo.Visibility = LayoutVisibility.Never;
					base.Height = 73;
				}
				else
				{
					base.Height = 97;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void DisableControlWhenPatientTypeQN(bool isTempQN, bool checkedCardTemp)
		{
			try
			{
				lblHeincardNumber.Enabled = !isTempQN;
				lblHeincardFromDate.Enabled = !isTempQN;
				lblHeincardToDate.Enabled = !isTempQN;
				if (isTempQN)
				{
					dxValidationProvider1.SetValidationRule(txtSoThe, null);
					dxValidationProvider1.SetValidationRule(txtHeinCardToTime, null);
					dxValidationProvider1.SetValidationRule(txtHeinCardFromTime, null);
				}
				if (checkedCardTemp)
				{
					lblCaptionAddress.Enabled = checkedCardTemp;
					lblHeincardMediOrg.Enabled = checkedCardTemp;
					lciDKKCBBDName.Enabled = checkedCardTemp;
					lciRightRouteTypeName.Enabled = checkedCardTemp;
					rdoRightRoute.Enabled = checkedCardTemp;
					rdoWrongRoute.Enabled = checkedCardTemp;
					lblRightRouteType.Enabled = checkedCardTemp;
					lcichkJoin5Year.Enabled = checkedCardTemp;
					lcichkPaid6Month.Enabled = checkedCardTemp;
					lciFreeCoPainTime.Enabled = checkedCardTemp;
					lblMediRecordBenefitSymbol.Enabled = checkedCardTemp;
					lblMediRecordLiveArea.Enabled = checkedCardTemp;
					lblMediRecordMediOrgForm.Enabled = checkedCardTemp;
					lblRightRouteType.Enabled = checkedCardTemp;
					lciNoiChuyenDenName.Enabled = checkedCardTemp;
					lciIcdMain.Enabled = checkedCardTemp;
					panelICD.Enabled = checkedCardTemp;
					lblEditIcd.Enabled = checkedCardTemp;
					lciInCode.Enabled = checkedCardTemp;
					lciHNCode.Enabled = checkedCardTemp;
					lciMediRecordRouteTransfer.Enabled = checkedCardTemp;
					lciMediRecordNoRouteTransfer.Enabled = checkedCardTemp;
					lciTransPatiFormCode.Enabled = checkedCardTemp;
					lciTransPatiFormCbo.Enabled = checkedCardTemp;
					lciTransPatiReasonCode.Enabled = checkedCardTemp;
					lciTransPatiReasoncbo.Enabled = checkedCardTemp;
				}
				else
				{
					lblCaptionAddress.Enabled = !isTempQN;
					lblHeincardMediOrg.Enabled = !isTempQN;
					lciDKKCBBDName.Enabled = !isTempQN;
					lciRightRouteTypeName.Enabled = !isTempQN;
					rdoRightRoute.Enabled = !isTempQN;
					rdoWrongRoute.Enabled = !isTempQN;
					lblRightRouteType.Enabled = !isTempQN;
					lcichkJoin5Year.Enabled = !isTempQN;
					lcichkPaid6Month.Enabled = !isTempQN;
					lciFreeCoPainTime.Enabled = !isTempQN;
					lblMediRecordBenefitSymbol.Enabled = !isTempQN;
					lblMediRecordLiveArea.Enabled = !isTempQN;
					lblMediRecordMediOrgForm.Enabled = !isTempQN;
					lblRightRouteType.Enabled = !isTempQN;
					lciNoiChuyenDenName.Enabled = !isTempQN;
					lciIcdMain.Enabled = !isTempQN;
					panelICD.Enabled = !isTempQN;
					lblEditIcd.Enabled = !isTempQN;
					lciInCode.Enabled = !isTempQN;
					lciHNCode.Enabled = !isTempQN;
					lciMediRecordRouteTransfer.Enabled = !isTempQN;
					lciMediRecordNoRouteTransfer.Enabled = !isTempQN;
					lciTransPatiFormCode.Enabled = !isTempQN;
					lciTransPatiFormCbo.Enabled = !isTempQN;
					lciTransPatiReasonCode.Enabled = !isTempQN;
					lciTransPatiReasoncbo.Enabled = !isTempQN;
				}
				if (IsTempQN && !checkedCardTemp)
				{
					lblHeincardNumber.AppearanceItemCaption.ForeColor = default(Color);
					lblHeincardToDate.AppearanceItemCaption.ForeColor = default(Color);
					lblHeincardFromDate.AppearanceItemCaption.ForeColor = default(Color);
					lblRightRouteType.AppearanceItemCaption.ForeColor = default(Color);
					IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
					for (int num = invalidControls.Count - 1; num >= 0; num--)
					{
						dxValidationProvider1.RemoveControlError(invalidControls[num]);
					}
					dxErrorProvider1.ClearErrors();
					dxValidationProvider1.SetValidationRule(txtSoThe, null);
					dxValidationProvider1.SetValidationRule(txtFreeCoPainTime, null);
					dxValidationProvider1.SetValidationRule(txtHeinCardToTime, null);
					dxValidationProvider1.SetValidationRule(txtHeinCardFromTime, null);
					dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, null);
					dxValidationProvider1.SetValidationRule(txtHNCode, null);
					dxValidationProvider1.SetValidationRule(txtMaNoiChuyenDen, null);
					dxValidationProvider1.SetValidationRule(txtMaChanDoanTD, null);
					dxValidationProvider1.SetValidationRule(txtMaDKKCBBD, null);
				}
				else if (IsTempQN && checkedCardTemp)
				{
					lblHeincardNumber.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblHeincardToDate.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblHeincardFromDate.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Maroon;
					txtHeinCardToTime.EditValue = null;
					dtHeinCardFromTime.EditValue = null;
					txtHeinCardFromTime.EditValue = null;
					dtHeinCardToTime.EditValue = null;
					txtSoThe.EditValue = null;
					cboSoThe.EditValue = null;
					txtAddress.Text = "";
					cboPatientCode.EditValue = null;
					IList<Control> invalidControls2 = dxValidationProvider1.GetInvalidControls();
					for (int num2 = invalidControls2.Count - 1; num2 >= 0; num2--)
					{
						dxValidationProvider1.RemoveControlError(invalidControls2[num2]);
					}
					dxErrorProvider1.ClearErrors();
					ValidFreeCoPainTime(true);
					ValidRightRouteType();
					ValidHNCode();
					ValidIcd();
					ValidNoiDKKCBBD();
				}
				else
				{
					lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblHeincardNumber.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblHeincardToDate.AppearanceItemCaption.ForeColor = Color.Maroon;
					lblHeincardFromDate.AppearanceItemCaption.ForeColor = Color.Maroon;
					txtHeinCardToTime.EditValue = null;
					dtHeinCardFromTime.EditValue = null;
					txtHeinCardFromTime.EditValue = null;
					dtHeinCardToTime.EditValue = null;
					txtSoThe.EditValue = null;
					cboSoThe.EditValue = null;
					ValidTxtSoThe();
					ValidFreeCoPainTime(true);
					ValidHeinCardToTime();
					ValidHeinCardFromTime();
					ValidRightRouteType();
					ValidHNCode();
					ValidIcd();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ShortcutKeyDown(Keys key)
		{
			try
			{
				if (dlgsetShortcutKeyDown != null)
				{
					dlgsetShortcutKeyDown(key);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private HIS_TRAN_PATI_FORM GetTranPatiFormById(long id)
		{
			HIS_TRAN_PATI_FORM hIS_TRAN_PATI_FORM = null;
			try
			{
				hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.SingleOrDefault((HIS_TRAN_PATI_FORM o) => o.ID == id);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return hIS_TRAN_PATI_FORM ?? new HIS_TRAN_PATI_FORM();
		}

		private HIS_TRAN_PATI_REASON GetTranPatiReasonById(long id)
		{
			HIS_TRAN_PATI_REASON hIS_TRAN_PATI_REASON = null;
			try
			{
				hIS_TRAN_PATI_REASON = DataStore.TranPatiReasons.SingleOrDefault((HIS_TRAN_PATI_REASON o) => o.ID == id);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return hIS_TRAN_PATI_REASON ?? new HIS_TRAN_PATI_REASON();
		}

		internal void ProcessFillDataTranPatiInForm(long treatmentId)
		{
			try
			{
				if (currentPatientSdo != null && !string.IsNullOrEmpty(currentPatientSdo.AppointmentCode))
				{
					return;
				}
				V_HIS_TREATMENT_4 treatment = HisTreatmentGet.GetById(treatmentId);
				if (treatment == null)
				{
					return;
				}
				if (treatment.TRANSFER_IN_TIME_FROM.HasValue)
				{
					dtTransferInTimeFrom.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TRANSFER_IN_TIME_FROM.GetValueOrDefault()) ?? DateTime.Now;
				}
				else
				{
					dtTransferInTimeFrom.EditValue = null;
				}
				if (treatment.TRANSFER_IN_TIME_TO.HasValue)
				{
					dtTransferInTimeTo.DateTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(treatment.TRANSFER_IN_TIME_TO.GetValueOrDefault()) ?? DateTime.Now;
				}
				else
				{
					dtTransferInTimeTo.EditValue = null;
				}
				txtMaHinhThucChuyen.Text = (treatment.TRANSFER_IN_FORM_ID.HasValue ? (GetTranPatiFormById(treatment.TRANSFER_IN_FORM_ID.Value) ?? new HIS_TRAN_PATI_FORM()).TRAN_PATI_FORM_CODE : "");
				cboHinhThucChuyen.EditValue = treatment.TRANSFER_IN_FORM_ID;
				txtMaLyDoChuyen.Text = (treatment.TRANSFER_IN_REASON_ID.HasValue ? (GetTranPatiReasonById(treatment.TRANSFER_IN_REASON_ID.Value) ?? new HIS_TRAN_PATI_REASON()).TRAN_PATI_REASON_CODE : "");
				cboLyDoChuyen.EditValue = treatment.TRANSFER_IN_REASON_ID;
				txtMaNoiChuyenDen.Text = treatment.TRANSFER_IN_MEDI_ORG_CODE;
				cboNoiChuyenDen.EditValue = treatment.TRANSFER_IN_MEDI_ORG_CODE;
				chkMediRecordRouteTransfer.Checked = treatment.TRANSFER_IN_CMKT == 1;
				chkMediRecordNoRouteTransfer.Checked = treatment.TRANSFER_IN_CMKT == 0;
				txtInCode.Text = treatment.TRANSFER_IN_CODE;
				lblEditIcd.Enabled = !string.IsNullOrEmpty(treatment.TRANSFER_IN_CODE);
				txtMaChanDoanTD.Text = treatment.TRANSFER_IN_ICD_CODE;
				if (!string.IsNullOrEmpty(treatment.TRANSFER_IN_ICD_CODE))
				{
					HIS_ICD hIS_ICD = DataStore.Icds.FirstOrDefault((HIS_ICD o) => o.ICD_CODE == treatment.TRANSFER_IN_ICD_CODE) ?? new HIS_ICD();
					cboChanDoanTD.EditValue = hIS_ICD.ID;
					if (autoCheckIcd == "1" || (!string.IsNullOrEmpty(treatment.TRANSFER_IN_ICD_NAME) && (treatment.TRANSFER_IN_ICD_NAME ?? "").Trim().ToLower() != (hIS_ICD.ICD_NAME ?? "").Trim().ToLower()))
					{
						chkHasDialogText.Checked = true;
						txtDialogText.Text = treatment.TRANSFER_IN_ICD_NAME;
					}
					else
					{
						chkHasDialogText.Checked = false;
						txtDialogText.Text = hIS_ICD.ICD_NAME;
					}
				}
				else
				{
					if (IsObligatoryTranferMediOrg)
					{
						lblEditIcd.Enabled = true;
						chkHasDialogText.Checked = true;
					}
					else
					{
						chkHasDialogText.Checked = false;
					}
					txtDialogText.Text = treatment.TRANSFER_IN_ICD_NAME;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FocusMoveOut()
		{
			try
			{
				if (dlgsetFocusMoveOut != null)
				{
					dlgsetFocusMoveOut();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal long GetExpriedTimeHeinCardBhyt(long alertExpriedTimeHeinCardBhyt, ref long resultDayAlert)
		{
			long num = -1L;
			try
			{
				if (chkHasDobCertificate.Checked)
				{
					num = -1L;
				}
				else
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue && dateTime2.HasValue && dateTime2.Value != DateTime.MinValue)
					{
						num = (long)(dateTime2.Value.Date - DateTime.Now.Date).TotalDays;
						if (num > alertExpriedTimeHeinCardBhyt)
						{
							num = -1L;
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return num;
		}

		internal void DefaultFocusUserControl()
		{
			try
			{
				if (chkHasDobCertificate.Enabled)
				{
					chkHasDobCertificate.Focus();
					return;
				}
				txtSoThe.Focus();
				txtSoThe.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetFocusUserByLiveAreaCode()
		{
			try
			{
				cboNoiSong.Focus();
				cboNoiSong.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetLogTime(long logTime)
		{
			try
			{
				this.logTime = logTime;
				ShowPatientFromHeinCardNumber();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetTreatmentType(long TreatmentTypeId)
		{
			try
			{
				this.TreatmentTypeId = TreatmentTypeId;
				ShowPatientFromHeinCardNumber();
				InitComboPatientCode();
				firstCheck = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FocusHeinCardFromTime()
		{
			try
			{
				txtHeinCardFromTime.Focus();
				txtHeinCardFromTime.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void HeinCardNumberKeyDownByRegisterForm(HeinCardData heinCardData, bool isSearchHeinCardNumber)
		{
			try
			{
				if (dlgProcessFillDataCareerUnder6AgeByHeinCardNumber != null)
				{
					dlgProcessFillDataCareerUnder6AgeByHeinCardNumber(heinCardData, isSearchHeinCardNumber);
				}
				txtHeinCardFromTime.Focus();
				txtHeinCardFromTime.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void UpdateHasDobCertificateEnable(bool hasDobCretificate)
		{
			try
			{
				chkHasDobCertificate.Enabled = hasDobCretificate;
				chkHasDobCertificate.Checked = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FillDataAfterFindQrCode(HeinCardData dataHein)
		{
			try
			{
				IsAutoCheck = true;
				txtSoThe.Text = dataHein.HeinCardNumber;
				HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.FirstOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == dataHein.MediOrgCode);
				if (hIS_MEDI_ORG != null)
				{
					txtMaDKKCBBD.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
					cboDKKCBBD.EditValue = hIS_MEDI_ORG.MEDI_ORG_CODE;
					MediOrgSelectRowChange(false, dataHein.LiveAreaCode);
				}
				if (HisConfigCFG.IsAllowedRouteTypeByDefault == "1" && hIS_MEDI_ORG != null)
				{
					TuDongChonLoaiThongTuyen(hIS_MEDI_ORG, dataHein);
				}
				else
				{
					ChonLoai_DungTuyen_HenKham(dataHein);
				}
				if (!string.IsNullOrEmpty(dataHein.FromDate))
				{
					txtHeinCardFromTime.Text = dataHein.FromDate;
					dtHeinCardFromTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
				}
				else
				{
					txtHeinCardFromTime.Text = "";
					dtHeinCardFromTime.EditValue = null;
				}
				if (!string.IsNullOrEmpty(dataHein.ToDate))
				{
					txtHeinCardToTime.Text = dataHein.ToDate;
					dtHeinCardToTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
				}
				else
				{
					txtHeinCardToTime.Text = "";
					dtHeinCardToTime.EditValue = null;
				}
				if (!string.IsNullOrEmpty(dataHein.FineYearMonthDate))
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(dataHein.FineYearMonthDate);
					txtDu5Nam.Text = dataHein.FineYearMonthDate;
					dtDu5Nam.EditValue = dateTime;
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(DateTime.Now.ToString("dd/MM/yyyy"));
					if (entity.IsInitFromCallPatientTypeAlter)
					{
						DateTime? dateTime3 = dateTime;
						DateTime? dateTime4 = dateTime2;
						if ((dateTime3.HasValue & dateTime4.HasValue) && dateTime3.GetValueOrDefault() < dateTime4.GetValueOrDefault() && !HisConfigCFG.IsNotAutoCheck5Y6M)
						{
							chkJoin5Year.Checked = true;
						}
						else
						{
							chkJoin5Year.Checked = false;
						}
					}
					else if (dateTime < dateTime2)
					{
						chkJoin5Year.Checked = true;
					}
					else
					{
						chkJoin5Year.Checked = false;
					}
				}
				else
				{
					txtDu5Nam.Text = "";
					dtDu5Nam.EditValue = null;
				}
				HeinLiveAreaData heinLiveAreaData = DataStore.LiveAreas.SingleOrDefault((HeinLiveAreaData o) => o.HeinLiveCode == dataHein.LiveAreaCode);
				cboNoiSong.EditValue = ((heinLiveAreaData != null) ? heinLiveAreaData.HeinLiveCode : null);
				CheckEdit checkEdit = chkJoin5Year;
				bool flag = (chkPaid6Month.Checked = false);
				checkEdit.Checked = flag;
				if (!string.IsNullOrEmpty(dataHein.MediOrgCode) && !string.IsNullOrEmpty(dataHein.PatientName) && !string.IsNullOrEmpty(dataHein.Dob) && !string.IsNullOrEmpty(dataHein.Gender))
				{
					string value = Inventec.Common.String.Convert.HexToUTF8Fix(dataHein.Address);
					if (string.IsNullOrEmpty(value))
					{
						txtAddress.Text = dataHein.Address;
					}
					else
					{
						txtAddress.Text = value;
					}
				}
				else
				{
					txtAddress.Text = dataHein.Address;
				}
				txtHNCode.Text = "";
				if (("1" == HeinLevelCodeCurrent || "2" == HeinLevelCodeCurrent) && !MediOrgCodeCurrent.Equals(txtMaDKKCBBD.Text) && rdoRightRoute.Checked)
				{
					if (IsDefaultRightRouteType)
					{
						InitDefaultRightRouteType();
						LogSystem.Debug("Quet the bhyt load thong tin the. this.IsDefaultRightRouteType = true");
					}
					else
					{
						LogSystem.Debug("Quet the bhyt load thong tin the. show thong bao phai chon truong hop");
						if (cboHeinRightRoute.EditValue == null)
						{
							XtraMessageBox.Show(His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.His_UCHein__MaDKKCBBDKhacVoiCuaVienNguoiDungPhaiChonTruongHop), His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), DefaultBoolean.True);
							txtHeinRightRouteCode.Focus();
							txtHeinRightRouteCode.SelectAll();
						}
					}
				}
				IsAutoCheck = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void FillDataAfterCheckBHYT(HeinCardData dataHein)
		{
			try
			{
				txtSoThe.Text = dataHein.HeinCardNumber;
				HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.FirstOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == dataHein.MediOrgCode);
				if (hIS_MEDI_ORG != null)
				{
					txtMaDKKCBBD.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
					cboDKKCBBD.EditValue = hIS_MEDI_ORG.MEDI_ORG_CODE;
					MediOrgSelectRowChange(false, dataHein.LiveAreaCode);
				}
				if (HisConfigCFG.IsAllowedRouteTypeByDefault == "1" && hIS_MEDI_ORG != null)
				{
					TuDongChonLoaiThongTuyen(hIS_MEDI_ORG, dataHein);
				}
				else
				{
					ChonLoai_DungTuyen_HenKham(dataHein);
				}
				if (!string.IsNullOrEmpty(dataHein.FromDate))
				{
					txtHeinCardFromTime.Text = dataHein.FromDate;
					dtHeinCardFromTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
				}
				else
				{
					txtHeinCardFromTime.Text = "";
					dtHeinCardFromTime.EditValue = null;
				}
				if (!string.IsNullOrEmpty(dataHein.ToDate))
				{
					txtHeinCardToTime.Text = dataHein.ToDate;
					dtHeinCardToTime.EditValue = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
				}
				else
				{
					txtHeinCardToTime.Text = "";
					dtHeinCardToTime.EditValue = null;
				}
				if (!string.IsNullOrEmpty(dataHein.FineYearMonthDate))
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(dataHein.FineYearMonthDate);
					txtDu5Nam.Text = dataHein.FineYearMonthDate;
					dtDu5Nam.EditValue = dateTime;
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(DateTime.Now.ToString("dd/MM/yyyy"));
					if (entity.IsInitFromCallPatientTypeAlter)
					{
						DateTime? dateTime3 = dateTime;
						DateTime? dateTime4 = dateTime2;
						if ((dateTime3.HasValue & dateTime4.HasValue) && dateTime3.GetValueOrDefault() < dateTime4.GetValueOrDefault() && !HisConfigCFG.IsNotAutoCheck5Y6M)
						{
							chkJoin5Year.Checked = true;
						}
						else
						{
							chkJoin5Year.Checked = false;
						}
					}
					else if (dateTime < dateTime2)
					{
						chkJoin5Year.Checked = true;
					}
					else
					{
						chkJoin5Year.Checked = false;
					}
				}
				else
				{
					txtDu5Nam.Text = "";
					dtDu5Nam.EditValue = null;
				}
				HeinLiveAreaData heinLiveAreaData = DataStore.LiveAreas.SingleOrDefault((HeinLiveAreaData o) => o.HeinLiveCode == dataHein.LiveAreaCode);
				cboNoiSong.EditValue = ((heinLiveAreaData != null) ? heinLiveAreaData.HeinLiveCode : null);
				chkPaid6Month.Checked = false;
				if (!string.IsNullOrEmpty(dataHein.MediOrgCode) && !string.IsNullOrEmpty(dataHein.PatientName) && !string.IsNullOrEmpty(dataHein.Dob) && !string.IsNullOrEmpty(dataHein.Gender))
				{
					string value = Inventec.Common.String.Convert.HexToUTF8Fix(dataHein.Address);
					if (string.IsNullOrEmpty(value))
					{
						txtAddress.Text = dataHein.Address;
					}
					else
					{
						txtAddress.Text = value;
					}
				}
				else
				{
					txtAddress.Text = dataHein.Address;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool ChonLoai_DungTuyen_HenKham(HeinCardData dataHein)
		{
			bool result = false;
			try
			{
				if (dataHein == null)
				{
					return false;
				}
				if (currentPatientSdo != null && !string.IsNullOrEmpty(currentPatientSdo.AppointmentCode) && !MediOrgCodeCurrent.Equals(dataHein.MediOrgCode))
				{
					rdoRightRoute.Checked = true;
					rdoWrongRoute.Checked = false;
					cboHeinRightRoute.EditValue = "HK";
					txtHeinRightRouteCode.Text = "HK";
					cboHeinRightRoute.Properties.Buttons[1].Visible = true;
					result = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void ChonLoai_DungTuyen_ThongTuyen()
		{
			try
			{
				List<HeinRightRouteTypeData> list = cboHeinRightRoute.Properties.DataSource as List<HeinRightRouteTypeData>;
				list = list ?? new List<HeinRightRouteTypeData>();
				if (!list.Exists((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode == "TH"))
				{
					list.Add(DataStore.HeinRightRouteTypes.FirstOrDefault((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode == "TH"));
					HeinRightRouterTypeProcess.FillDataToComboHeinRightRouterType(cboHeinRightRoute, list);
				}
				rdoRightRoute.Checked = true;
				rdoWrongRoute.Checked = false;
				cboHeinRightRoute.EditValue = "TH";
				txtHeinRightRouteCode.Text = "TH";
				cboHeinRightRoute.Properties.Buttons[1].Visible = true;
				SetEnableControlHein(RightRouterFactory.RIGHT_ROUTER, false);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void TuDongChonLoaiThongTuyen(HIS_MEDI_ORG MediOrgADO, HeinCardData dataHein)
		{
			try
			{
				if (MediOrgADO == null || string.IsNullOrEmpty(MediOrgADO.MEDI_ORG_CODE))
				{
					return;
				}
				string text = MacDinhLoaiThongTuyen(MediOrgADO);
				if (string.IsNullOrWhiteSpace(text))
				{
					return;
				}
				switch (text)
				{
				case "DT":
					if (!ChonLoai_DungTuyen_HenKham(dataHein))
					{
						rdoRightRoute.Checked = true;
						rdoWrongRoute.Checked = false;
						SetEnableControlHein(RightRouterFactory.RIGHT_ROUTER, false);
					}
					break;
				case "TH":
					ChonLoai_DungTuyen_ThongTuyen();
					break;
				case "TT":
					rdoRightRoute.Checked = false;
					rdoWrongRoute.Checked = !rdoRightRoute.Checked;
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER, false);
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private string MacDinhLoaiThongTuyen(HIS_MEDI_ORG MediOrgADO)
		{
			string text = null;
			try
			{
				LogSystem.Debug(LogUtil.TraceData("MacDinhLoaiThongTuyen() => MediOrgADO", MediOrgADO));
				HIS_BRANCH branch = BranchDataWorker.Branch;
				LogSystem.Debug(LogUtil.TraceData("MacDinhLoaiThongTuyen() => branch", branch));
				if (MediOrgADO != null && !string.IsNullOrEmpty(MediOrgADO.MEDI_ORG_CODE) && branch != null)
				{
					string text2 = (branch.HEIN_MEDI_ORG_CODE ?? "").Trim();
					if (MediOrgADO.MEDI_ORG_CODE.Trim() == text2 || ValidAcceptHeinMediOrgCode(MediOrgADO.MEDI_ORG_CODE, branch.ACCEPT_HEIN_MEDI_ORG_CODE) || ValidSysMediOrgCode(MediOrgADO.MEDI_ORG_CODE, branch.SYS_MEDI_ORG_CODE))
					{
						text = "DT";
					}
					else
					{
						string text3 = MediOrgADO.MEDI_ORG_CODE.Substring(0, 2);
						string text4 = ((text2.Length > 2) ? text2.Substring(0, 2) : null);
						if ((HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT == "3" || HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT == "4") && text3 == text4 && (MediOrgADO.LEVEL_CODE == "3" || MediOrgADO.LEVEL_CODE == "4"))
						{
							text = "TH";
						}
						else if (text3 != text4 || MediOrgADO.LEVEL_CODE == "2" || MediOrgADO.LEVEL_CODE == "1" || HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT == "2" || HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT == "1")
						{
							text = "TT";
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			LogSystem.Debug(LogUtil.TraceData("MacDinhLoaiThongTuyen() => result", text));
			return text;
		}

		private bool ValidAcceptHeinMediOrgCode(string mediOrgCode, string AcceptHeinMediOrgCode)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrWhiteSpace(mediOrgCode) || string.IsNullOrWhiteSpace(AcceptHeinMediOrgCode))
				{
					return false;
				}
				string[] array = AcceptHeinMediOrgCode.Split(',', ';');
				foreach (string text in array)
				{
					if (text != null && text.Trim() == mediOrgCode.Trim())
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private bool ValidSysMediOrgCode(string mediOrgCode, string SysMediOrgCode)
		{
			bool result = false;
			try
			{
				if (string.IsNullOrWhiteSpace(mediOrgCode) || string.IsNullOrWhiteSpace(SysMediOrgCode))
				{
					return false;
				}
				string[] array = SysMediOrgCode.Split(',', ';');
				foreach (string text in array)
				{
					if (text != null && text.Trim() == mediOrgCode.Trim())
					{
						return true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		internal void ResetValidationControl()
		{
			try
			{
				IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
				for (int num = invalidControls.Count - 1; num >= 0; num--)
				{
					dxValidationProvider1.RemoveControlError(invalidControls[num]);
				}
				dxErrorProvider1.ClearErrors();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal bool GetInvalidControls()
		{
			bool flag = true;
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
					flag = false;
				}
				flag = flag && ValidateHeinPatientTypeCode() && ValidateCoPaidAccumulate();
			}
			catch (Exception ex)
			{
				flag = false;
				LogSystem.Error(ex);
			}
			return flag;
		}

		private HIS_BHYT_PARAM GetCurrentBhytParam()
		{
			try
			{
				if (listBhytParam == null)
				{
					CommonParam commonParam = new CommonParam();
					HisBhytParamFilter hisBhytParamFilter = new HisBhytParamFilter();
					hisBhytParamFilter.IS_ACTIVE = 1;
					listBhytParam = new BackendAdapter(commonParam).Get<List<HIS_BHYT_PARAM>>("api/HisBhytParam/Get", ApiConsumerStore.MosConsumer, hisBhytParamFilter, commonParam);
				}
				if (listBhytParam == null)
				{
					return null;
				}
				return (from o in listBhytParam
					where !o.TO_TIME.HasValue
					orderby o.FROM_TIME descending
					select o).FirstOrDefault();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return null;
			}
		}

		public bool ValidateCoPaidAccumulate()
		{
			bool result = true;
			try
			{
				string text = new string((txtCoPaidAccumulate.Text ?? "").Where(new Func<char, bool>(char.IsDigit)).ToArray());
				if (string.IsNullOrEmpty(text))
				{
					return true;
				}
				long num = Parse.ToInt64(text);
				HIS_BHYT_PARAM currentBhytParam = GetCurrentBhytParam();
				if (currentBhytParam == null || currentBhytParam.BASE_SALARY <= 0m)
				{
					return true;
				}
				decimal num2 = currentBhytParam.BASE_SALARY * 6m;
				if ((decimal)num > num2 && string.IsNullOrEmpty(txtFreeCoPainTime.Text.Trim()))
				{
					XtraMessageBox.Show(ResourceMessage.SoTienLuyKeCungChiTraVuot06ThangLuongCoSo, Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					if (txtFreeCoPainTime.Enabled)
					{
						txtFreeCoPainTime.Focus();
						txtFreeCoPainTime.SelectAll();
					}
					result = false;
				}
			}
			catch (Exception ex)
			{
				result = false;
				LogSystem.Error(ex);
			}
			return result;
		}

		internal void SelectMediOrgForSearch(bool isSearch)
		{
			try
			{
				MediOrgSelectRowChange(isSearch, (cboNoiSong.EditValue ?? "").ToString());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiSong_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (txtMaNoiChuyenDen.Enabled)
					{
						txtMaNoiChuyenDen.Focus();
						txtMaNoiChuyenDen.SelectAll();
					}
					else
					{
						txtHNCode.Focus();
						txtHNCode.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void AutoCheckRightRoute(bool IsDungTuyenCapCuu)
		{
			try
			{
				IsDungTuyenCapCuuByTime = IsDungTuyenCapCuu;
				if (IsDungTuyenCapCuu)
				{
					rdoRightRoute.Checked = true;
					cboHeinRightRoute.EditValue = "CC";
					txtHeinRightRouteCode.Text = "CC";
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC, false);
					dxValidationProvider1.SetValidationRule(txtMaNoiChuyenDen, null);
				}
				else if (rdoRightRoute.Checked)
				{
					txtHeinCardToTime.Enabled = true;
					txtMaNoiChuyenDen.Enabled = true;
					chkHasDialogText.Enabled = true;
					cboNoiChuyenDen.Enabled = true;
					txtDialogText.Enabled = true;
					txtMaChanDoanTD.Enabled = true;
					cboChanDoanTD.Enabled = true;
					chkJoin5Year.Enabled = true;
					chkMediRecordRouteTransfer.Enabled = true;
					chkMediRecordNoRouteTransfer.Enabled = true;
					cboHinhThucChuyen.Enabled = true;
					txtMaHinhThucChuyen.Enabled = true;
					txtMaLyDoChuyen.Enabled = true;
					cboLyDoChuyen.Enabled = true;
					dtTransferInTimeFrom.Enabled = true;
					dtTransferInTimeTo.Enabled = true;
					HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == (cboDKKCBBD.EditValue ?? "").ToString());
					if (entity.IsAutoSelectEmergency)
					{
						AutoSelectEmergency(entity);
					}
					else if (hIS_MEDI_ORG != null && !string.IsNullOrEmpty(hIS_MEDI_ORG.MEDI_ORG_CODE) && (MediOrgCodeCurrent == hIS_MEDI_ORG.MEDI_ORG_CODE || "3" == HeinLevelCodeCurrent || "4" == HeinLevelCodeCurrent))
					{
						cboHeinRightRoute.Properties.Buttons[1].Visible = false;
						cboHeinRightRoute.EditValue = null;
						txtHeinRightRouteCode.Text = "";
					}
					else if (hIS_MEDI_ORG != null)
					{
						cboHeinRightRoute.EditValue = "GT";
						txtHeinRightRouteCode.Text = "GT";
						cboHeinRightRoute.Properties.Buttons[1].Visible = true;
					}
					else
					{
						cboHeinRightRoute.Properties.Buttons[1].Visible = false;
						cboHeinRightRoute.EditValue = null;
						txtHeinRightRouteCode.Text = "";
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void ChangeRoomNotEmergency()
		{
			try
			{
				IsDungTuyenCapCuuByTime = false;
				cboHeinRightRoute.EditValue = null;
				MediOrgSelectRowChange(true, (cboNoiSong.EditValue ?? "").ToString());
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtTransferInTimeFrom_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				dtTransferInTimeTo.Focus();
				dtTransferInTimeTo.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtTransferInTimeFrom_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtTransferInTimeTo.Focus();
					dtTransferInTimeTo.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtTransferInTimeTo_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				txtMaHinhThucChuyen.Focus();
				txtMaHinhThucChuyen.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtTransferInTimeTo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtMaHinhThucChuyen.Focus();
					txtMaHinhThucChuyen.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void btnCheckInfoBHYT_Click(object sender, EventArgs e)
		{
			try
			{
				bool flag = true;
				if (!dxValidationProvider1.Validate(txtSoThe))
				{
					flag = false;
				}
				if (!flag)
				{
					return;
				}
				Inventec.Desktop.Common.Modules.Module module = GlobalVariables.currentModuleRaws.Where((Inventec.Desktop.Common.Modules.Module o) => o.ModuleLink == "HIS.Desktop.Plugins.CheckInfoBHYT").FirstOrDefault();
				if (module == null)
				{
					LogSystem.Error("khong tim thay moduleLink = HIS.Desktop.Plugins.CheckInfoBHYT");
				}
				if (module.IsPlugin && module.ExtensionInfo != null)
				{
					List<object> listArgs = new List<object>();
					CheckInfoBhytADO checkInfoBhytADO = new CheckInfoBhytADO();
					checkInfoBhytADO.TDL_PATIENT_NAME = entity.HisTreatment.TDL_PATIENT_NAME;
					checkInfoBhytADO.TDL_DOB = entity.HisTreatment.TDL_PATIENT_DOB;
					checkInfoBhytADO.TDL_GENDER_NAME = entity.HisTreatment.TDL_PATIENT_GENDER_NAME;
					checkInfoBhytADO.TDL_HEIN_CARD_NUMBER = HeinUtils.TrimHeinCardNumber(txtSoThe.Text);
					listArgs.Add(checkInfoBhytADO);
					LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => listArgs), listArgs));
					listArgs.Add(PluginInstance.GetModuleWithWorkingRoom(module, entity.currentModule.RoomId, entity.currentModule.RoomTypeId));
					object pluginInstance = PluginInstance.GetPluginInstance(PluginInstance.GetModuleWithWorkingRoom(module, entity.currentModule.RoomId, entity.currentModule.RoomTypeId), listArgs);
					if (pluginInstance == null)
					{
						throw new ArgumentNullException("moduleData is null");
					}
					((Form)pluginInstance).ShowDialog();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void DisposeControl()
		{
			try
			{
				IsDungTuyenCapCuuByTime = false;
				_TextIcdName = null;
				patientTypeAlterOld = null;
				isCallByRegistor = false;
				ExceedDayAllow = 0L;
				isDefaultInit = false;
				currentPatientSdo = null;
				ObligatoryTranferMediOrg = null;
				IsObligatoryTranferMediOrg = false;
				IsTempQN = false;
				IsEdit = false;
				IsNotRequiredRightTypeInCaseOfHavingAreaCode = false;
				IsDefaultRightRouteType = false;
				autoCheckIcd = null;
				isShowCheckKhongKTHSD = null;
				isVisibleControl = 0L;
				PatientTypeIdBHYT = 0L;
				TreatmentTypeIdExam = 0L;
				MediOrgCodesAccepts = null;
				MediOrgCodeCurrent = null;
				HeinLevelCodeCurrent = null;
				SysMediOrgCode = null;
				entity = null;
				CultureInfo = null;
				TreatmentTypeCode = null;
				PatientTypeId = 0L;
				positionHandleControl = 0;
				actChangePatientDob = null;
				_DelegateSetRelativeAddress = null;
				dlgfillDataPatientSDOToRegisterForm = null;
				dlgcheckExamHistory = null;
				dlgautoCheckCC = null;
				TreatmentTypeId1 = null;
				dlgProcessFillDataCareerUnder6AgeByHeinCardNumber = null;
				dlgsetShortcutKeyDown = null;
				dlgsetFocusMoveOut = null;
				txtHeinCardToTime.ButtonClick -= new ButtonPressedEventHandler(txtHeinCardToTime_ButtonClick);
				txtHeinCardToTime.InvalidValue -= new InvalidValueExceptionEventHandler(txtHeinCardToTime_InvalidValue);
				txtHeinCardToTime.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtHeinCardToTime_PreviewKeyDown);
				txtHeinCardFromTime.ButtonClick -= new ButtonPressedEventHandler(txtHeinCardFromTime_ButtonClick);
				txtHeinCardFromTime.InvalidValue -= new InvalidValueExceptionEventHandler(txtHeinCardFromTime_InvalidValue);
				txtHeinCardFromTime.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtHeinCardFromTime_PreviewKeyDown);
				dtHeinCardToTime.Closed -= new ClosedEventHandler(dtHeinCardToTime_Closed);
				dtHeinCardToTime.EditValueChanged -= new EventHandler(dtHeinCardToTime_EditValueChanged);
				dtHeinCardToTime.KeyDown -= new KeyEventHandler(dtHeinCardToTime_KeyDown);
				dtHeinCardFromTime.Closed -= new ClosedEventHandler(dtHeinCardFromTime_Closed);
				dtHeinCardFromTime.EditValueChanged -= new EventHandler(dtHeinCardFromTime_EditValueChanged);
				dtHeinCardFromTime.KeyDown -= new KeyEventHandler(dtHeinCardFromTime_KeyDown);
				btnCheckInfoBHYT.Click -= new EventHandler(btnCheckInfoBHYT_Click);
				rdoWrongRoute.CheckedChanged -= new EventHandler(rdoWrongRoute_CheckedChanged);
				rdoWrongRoute.PreviewKeyDown -= new PreviewKeyDownEventHandler(rdoWrongRoute_PreviewKeyDown);
				dtTransferInTimeTo.Closed -= new ClosedEventHandler(dtTransferInTimeTo_Closed);
				dtTransferInTimeTo.PreviewKeyDown -= new PreviewKeyDownEventHandler(dtTransferInTimeTo_PreviewKeyDown);
				dtTransferInTimeFrom.Closed -= new ClosedEventHandler(dtTransferInTimeFrom_Closed);
				dtTransferInTimeFrom.PreviewKeyDown -= new PreviewKeyDownEventHandler(dtTransferInTimeFrom_PreviewKeyDown);
				txtDu5Nam.ButtonClick -= new ButtonPressedEventHandler(txtDu5Nam_ButtonClick);
				txtDu5Nam.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtDu5Nam_PreviewKeyDown);
				dtDu5Nam.Closed -= new ClosedEventHandler(dtDu5Nam_Closed);
				dtDu5Nam.KeyDown -= new KeyEventHandler(dtDu5Nam_KeyDown);
				cboNoiSong.Closed -= new ClosedEventHandler(cboNoiSong_Closed);
				cboNoiSong.ButtonClick -= new ButtonPressedEventHandler(cboNoiSong_ButtonClick);
				cboNoiSong.EditValueChanged -= new EventHandler(cboNoiSong_EditValueChanged);
				cboNoiSong.KeyUp -= new KeyEventHandler(cboNoiSong_KeyUp);
				cboNoiSong.PreviewKeyDown -= new PreviewKeyDownEventHandler(cboNoiSong_PreviewKeyDown);
				chkTempQN.CheckedChanged -= new EventHandler(chkTempQN_CheckedChanged);
				txtFreeCoPainTime.ButtonClick -= new ButtonPressedEventHandler(txtFreeCoPainTime_ButtonClick);
				txtFreeCoPainTime.InvalidValue -= new InvalidValueExceptionEventHandler(txtFreeCoPainTime_InvalidValue);
				txtFreeCoPainTime.TextChanged -= new EventHandler(txtDTMCChiTra_TextChanged);
				txtFreeCoPainTime.Click -= new EventHandler(txtFreeCoPainTime_Click);
				txtFreeCoPainTime.KeyDown -= new KeyEventHandler(txtFreeCoPainTime_KeyDown);
				txtFreeCoPainTime.KeyPress -= new KeyPressEventHandler(txtFreeCoPainTime_KeyPress);
				txtFreeCoPainTime.Validating -= new CancelEventHandler(txtFreeCoPainTime_Validating);
				dtFreeCoPainTime.Closed -= new ClosedEventHandler(dtFreeCoPainTime_Closed);
				dtFreeCoPainTime.KeyDown -= new KeyEventHandler(dtFreeCoPainTime_KeyDown);
				txtInCode.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtInCode_PreviewKeyDown);
				txtHNCode.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtHNCode_PreviewKeyDown);
				rdoRightRoute.CheckedChanged -= new EventHandler(rdoRightRoute_CheckedChanged);
				rdoRightRoute.PreviewKeyDown -= new PreviewKeyDownEventHandler(rdoRightRoute_PreviewKeyDown);
				chkPaid6Month.CheckedChanged -= new EventHandler(chkPaid6Month_CheckedChanged);
				chkPaid6Month.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkPaid6Month_PreviewKeyDown);
				cboChanDoanTD.Closed -= new ClosedEventHandler(cboChanDoanTD_Closed);
				cboChanDoanTD.ButtonClick -= new ButtonPressedEventHandler(cboChanDoanTD_ButtonClick);
				cboChanDoanTD.TextChanged -= new EventHandler(cboChanDoanTD_TextChanged);
				cboChanDoanTD.KeyUp -= new KeyEventHandler(cboChanDoanTD_KeyUp);
				txtMaChanDoanTD.InvalidValue -= new InvalidValueExceptionEventHandler(txtMaChanDoanTD_InvalidValue);
				txtMaChanDoanTD.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtMaChuanDoanTD_PreviewKeyDown);
				txtMaChanDoanTD.Validating -= new CancelEventHandler(txtMaChanDoanTD_Validating);
				chkHasDialogText.CheckedChanged -= new EventHandler(chkHasDialogText_CheckedChanged);
				chkHasDialogText.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkHasDialogText_PreviewKeyDown);
				cboLyDoChuyen.Closed -= new ClosedEventHandler(cboLyDoChuyen_Closed);
				cboLyDoChuyen.ButtonClick -= new ButtonPressedEventHandler(cboLyDoChuyen_ButtonClick);
				cboLyDoChuyen.EditValueChanged -= new EventHandler(cboLyDoChuyen_EditValueChanged);
				cboLyDoChuyen.PreviewKeyDown -= new PreviewKeyDownEventHandler(cboLyDoChuyen_PreviewKeyDown);
				txtMaLyDoChuyen.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtMaLyDoChuyen_PreviewKeyDown);
				cboHeinRightRoute.Closed -= new ClosedEventHandler(cboHeinRightRoute_Closed);
				cboHeinRightRoute.ButtonClick -= new ButtonPressedEventHandler(cboHeinRightRoute_ButtonClick);
				cboHeinRightRoute.EditValueChanged -= new EventHandler(cboHeinRightRoute_EditValueChanged);
				cboHeinRightRoute.KeyUp -= new KeyEventHandler(cboHeinRightRoute_KeyUp);
				cboHeinRightRoute.PreviewKeyDown -= new PreviewKeyDownEventHandler(cboHeinRightRoute_PreviewKeyDown);
				txtHeinRightRouteCode.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtHeinRightRouteCode_PreviewKeyDown);
				cboDKKCBBD.Closed -= new ClosedEventHandler(cboDKKCBBD_Closed);
				cboDKKCBBD.KeyUp -= new KeyEventHandler(cboDKKCBBD_KeyUp);
				txtMaDKKCBBD.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtMaDKKCBBD_PreviewKeyDown);
				cboNoiChuyenDen.Closed -= new ClosedEventHandler(cboNoiChuyenDen_Closed);
				cboNoiChuyenDen.ButtonClick -= new ButtonPressedEventHandler(cboNoiChuyenDen_ButtonClick);
				cboNoiChuyenDen.KeyUp -= new KeyEventHandler(cboNoiChuyenDen_KeyUp);
				txtMaNoiChuyenDen.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtMaNoiChuyenDen_PreviewKeyDown);
				cboHinhThucChuyen.Closed -= new ClosedEventHandler(cboHinhThucChuyen_Closed);
				cboHinhThucChuyen.ButtonClick -= new ButtonPressedEventHandler(cboHinhThucChuyen_ButtonClick);
				cboHinhThucChuyen.EditValueChanged -= new EventHandler(cboHinhThucChuyen_EditValueChanged);
				cboHinhThucChuyen.KeyUp -= new KeyEventHandler(cboHinhThucChuyen_KeyUp);
				txtMaHinhThucChuyen.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtMaHinhThucChuyen_PreviewKeyDown);
				txtAddress.PreviewKeyDown -= new PreviewKeyDownEventHandler(txtAddress_PreviewKeyDown);
				chkJoin5Year.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkJoin5Year_PreviewKeyDown);
				chkMediRecordNoRouteTransfer.CheckedChanged -= new EventHandler(chkMediRecordNoRouteTransfer_CheckedChanged);
				chkMediRecordNoRouteTransfer.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkMediRecordNoRouteTransfer_PreviewKeyDown);
				chkMediRecordRouteTransfer.CheckedChanged -= new EventHandler(chkMediRecordRouteTransfer_CheckedChanged);
				chkMediRecordRouteTransfer.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkMediRecordRouteTransfer_PreviewKeyDown);
				txtSoThe.Properties.ButtonClick -= new ButtonPressedEventHandler(txtSoThe_Properties_ButtonClick);
				txtSoThe.InvalidValue -= new InvalidValueExceptionEventHandler(txtSoThe_InvalidValue);
				txtSoThe.EditValueChanged -= new EventHandler(txtSoThe_EditValueChanged);
				txtSoThe.KeyDown -= new KeyEventHandler(txtSoThe_KeyDown);
				cboSoThe.Closed -= new ClosedEventHandler(cboSoThe_Closed);
				cboSoThe.KeyUp -= new KeyEventHandler(cboSoThe_KeyUp);
				chkHasDobCertificate.CheckedChanged -= new EventHandler(chkHasDobCertificate_CheckedChanged);
				chkHasDobCertificate.PreviewKeyDown -= new PreviewKeyDownEventHandler(chkHasDobCertificate_PreviewKeyDown);
				dxValidationProvider1.ValidationFailed -= new ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
				base.Load -= new EventHandler(Template__HeinBHYT1_Load);
				cboNoiSong.Properties.DataSource = null;
				gridView2.GridControl.DataSource = null;
				gridView3.GridControl.DataSource = null;
				cboChanDoanTD.Properties.DataSource = null;
				cboLyDoChuyen.Properties.DataSource = null;
				cboHeinRightRoute.Properties.DataSource = null;
				gridLookUpEdit1View.GridControl.DataSource = null;
				cboDKKCBBD.Properties.DataSource = null;
				gridView1.GridControl.DataSource = null;
				cboNoiChuyenDen.Properties.DataSource = null;
				cboHinhThucChuyen.Properties.DataSource = null;
				cboSoThe.Properties.DataSource = null;
				layoutControlItem1 = null;
				btnCheckInfoBHYT = null;
				lciFordtTransferInTimeTo = null;
				lciFordtTransferInTimeFrom = null;
				dtTransferInTimeFrom = null;
				dtTransferInTimeTo = null;
				lciDu5Nam = null;
				dtDu5Nam = null;
				txtDu5Nam = null;
				panel4 = null;
				cboNoiSong = null;
				lblMediRecordLiveArea = null;
				gridView2 = null;
				lciTempQN = null;
				chkTempQN = null;
				dtFreeCoPainTime = null;
				lciFreeCoPainTime = null;
				panelControl1 = null;
				txtFreeCoPainTime = null;
				lciInCode = null;
				txtInCode = null;
				lciHNCode = null;
				txtHNCode = null;
				lciKhongKTHSD = null;
				checkKhongKTHSD = null;
				lcirdoRightRoute = null;
				rdoRightRoute = null;
				lcirdoWrongRoute = null;
				rdoWrongRoute = null;
				lcichkPaid6Month = null;
				chkPaid6Month = null;
				txtDialogText = null;
				gridView3 = null;
				cboChanDoanTD = null;
				panelICD = null;
				lciIcdMain = null;
				lblEditIcd = null;
				chkHasDialogText = null;
				txtMaChanDoanTD = null;
				panel5 = null;
				lciTransPatiReasoncbo = null;
				lciTransPatiReasonCode = null;
				txtMaLyDoChuyen = null;
				cboLyDoChuyen = null;
				lciRightRouteTypeName = null;
				lblRightRouteType = null;
				txtHeinRightRouteCode = null;
				cboHeinRightRoute = null;
				lciDKKCBBDName = null;
				lblHeincardMediOrg = null;
				txtMaDKKCBBD = null;
				gridLookUpEdit1View = null;
				cboDKKCBBD = null;
				lciNoiChuyenDenName = null;
				lblMediRecordMediOrgForm = null;
				txtMaNoiChuyenDen = null;
				gridView1 = null;
				cboNoiChuyenDen = null;
				lciTransPatiFormCbo = null;
				cboHinhThucChuyen = null;
				lciTransPatiFormCode = null;
				txtMaHinhThucChuyen = null;
				lblCaptionAddress = null;
				txtAddress = null;
				dxValidationProvider1 = null;
				dxErrorProvider1 = null;
				lblMediRecordBenefitSymbol = null;
				lcichkJoin5Year = null;
				lciMediRecordNoRouteTransfer = null;
				lciMediRecordRouteTransfer = null;
				lblHeincardFromDate = null;
				lblHeincardToDate = null;
				lblHeincardNumber = null;
				panel1 = null;
				panel2 = null;
				panel3 = null;
				lblCaptionHasDobCertificate = null;
				layoutControlGroup1 = null;
				layoutControl1 = null;
				cboSoThe = null;
				chkJoin5Year = null;
				chkMediRecordRouteTransfer = null;
				chkMediRecordNoRouteTransfer = null;
				txtMucHuong = null;
				chkHasDobCertificate = null;
				txtSoThe = null;
				dtHeinCardFromTime = null;
				dtHeinCardToTime = null;
				txtHeinCardFromTime = null;
				txtHeinCardToTime = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkJoin5Year_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				ChangeDefaultHeinRatio();
				Join5YearAndPaid6MonthCheckedChanged();
				if (entity.IsInitFromCallPatientTypeAlter)
				{
					ValidateCheckBox5Y(chkJoin5Year.Checked);
					if (!chkJoin5Year.Checked && !chkPaid6Month.Checked)
					{
						IsShowMessage = false;
					}
					else if (chkJoin5Year.Checked && chkPaid6Month.Checked)
					{
						IsShowMessage = true;
					}
					if (chkJoin5Year.Checked && chkJoin5Year.OldEditValue != chkJoin5Year.EditValue)
					{
						ShowMessageNotAutoCheck5Y6M(chkJoin5Year);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void ShowPatientFromHeinCardNumber()
		{
			try
			{
				if (PatientId == 0L || TreatmentTypeId == 0L)
				{
					return;
				}
				if (patyAlters == null || patyAlters.Count == 0)
				{
					CommonParam commonParam = new CommonParam();
					HisPatientTypeAlterFilter hisPatientTypeAlterFilter = new HisPatientTypeAlterFilter();
					hisPatientTypeAlterFilter.TDL_PATIENT_ID = PatientId;
					hisPatientTypeAlterFilter.PATIENT_TYPE_ID = PatientTypeIdBHYT;
					patyAlters = new BackendAdapter(commonParam).Get<List<HIS_PATIENT_TYPE_ALTER>>("/api/HisPatientTypeAlter/Get", ApiConsumerStore.MosConsumer, hisPatientTypeAlterFilter, commonParam);
				}
				if (patyAlters != null && patyAlters.Count > 0)
				{
					long valueAdd = HisConfigs.Get<long>("MOS.BHYT.EXCEED_DAY_ALLOW_FOR_IN_PATIENT");
					lstPatientTypeAlterMap = new List<PatientTypeAlterADO>();
					foreach (HIS_PATIENT_TYPE_ALTER item2 in patyAlters.Where((HIS_PATIENT_TYPE_ALTER o) => !string.IsNullOrEmpty(o.HEIN_CARD_NUMBER)).ToList())
					{
						PatientTypeAlterADO item = new PatientTypeAlterADO(item2);
						lstPatientTypeAlterMap.Add(item);
					}
					lstPatientTypeAlterMap.ForEach(delegate(PatientTypeAlterADO o)
					{
						if (TreatmentTypeId != 3 && TreatmentTypeId != 4)
						{
							o.HEIN_CARD_TO_TIME_CAL = (o.HEIN_CARD_TO_TIME.HasValue ? long.Parse(o.HEIN_CARD_TO_TIME.ToString().Substring(0, 12)) : 0);
						}
						else
						{
							o.HEIN_CARD_TO_TIME_CAL = (o.HEIN_CARD_TO_TIME.HasValue ? long.Parse(Calculation.Add(o.HEIN_CARD_TO_TIME.GetValueOrDefault(), valueAdd, Calculation.UnitDifferenceTime.DAY).ToString().Substring(0, 12)) : 0);
						}
					});
					lstPatientTypeAlterMap = lstPatientTypeAlterMap.Where((PatientTypeAlterADO o) => o.HEIN_CARD_TO_TIME_CAL >= logTime).ToList();
					if (lstPatientTypeAlterMap != null && lstPatientTypeAlterMap.Count > 0)
					{
						lstPatientTypeAlterMap = lstPatientTypeAlterMap.Distinct(new Compare()).ToList();
					}
				}
				if (lstPatientTypeAlterMap == null)
				{
					lstPatientTypeAlterMap = new List<PatientTypeAlterADO>();
				}
				ReloadComboSoThe(cboSoThe, null, lstPatientTypeAlterMap);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ReloadComboSoThe(GridLookUpEdit cboEditor, List<HIS_PATIENT_TYPE_ALTER> patientTypeAlters, List<PatientTypeAlterADO> lstMap = null)
		{
			try
			{
				List<PatientTypeAlterADO> list = lstMap;
				if (lstMap == null)
				{
					list = new List<PatientTypeAlterADO>();
					foreach (HIS_PATIENT_TYPE_ALTER patientTypeAlter in patientTypeAlters)
					{
						PatientTypeAlterADO item = new PatientTypeAlterADO(patientTypeAlter);
						list.Add(item);
					}
				}
				cboEditor.Properties.DataSource = list;
				cboEditor.Properties.DisplayMember = "RENDERER_HEIN_CARD_NUMBER";
				cboEditor.Properties.ValueMember = "RENDERER_HEIN_CARD_NUMBER";
				cboEditor.Properties.TextEditStyle = TextEditStyles.Standard;
				cboEditor.Properties.PopupFilterMode = PopupFilterMode.Contains;
				cboEditor.Properties.ImmediatePopup = true;
				cboEditor.ForceInitialize();
				cboEditor.Properties.View.Columns.Clear();
				GridColumn gridColumn = cboEditor.Properties.View.Columns.AddField("RENDERER_HEIN_CARD_NUMBER");
				gridColumn.Caption = "Số thẻ BHYT";
				gridColumn.Width = 150;
				gridColumn.VisibleIndex = 1;
				GridColumn gridColumn2 = cboEditor.Properties.View.Columns.AddField("RENDERER_FROM_DATE_TODATE");
				gridColumn2.Caption = "Hạn thẻ";
				gridColumn2.Width = 250;
				gridColumn2.VisibleIndex = 2;
				GridColumn gridColumn3 = cboEditor.Properties.View.Columns.AddField("HEIN_MEDI_ORG_NAME");
				gridColumn3.Caption = "Nơi ĐKKCB BĐ";
				gridColumn3.ToolTip = "Nơi đăng ký khám chữa bệnh ban đầu";
				gridColumn3.Width = 250;
				gridColumn3.VisibleIndex = 3;
				cboEditor.Properties.View.OptionsView.ColumnAutoWidth = false;
				cboEditor.Properties.View.OptionsView.ShowIndicator = false;
				cboEditor.Properties.View.OptionsView.ShowGroupPanel = false;
				cboEditor.Properties.PopupFormSize = new Size(650, cboEditor.Height);
				cboEditor.Properties.View.OptionsView.ShowColumnHeaders = true;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboSoThe_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Plus)
				{
					txtSoThe_Properties_ButtonClick(null, e);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkBaby_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkBaby.Checked)
				{
					rdoRightRoute.Checked = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkTt46_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkTt46.Checked)
				{
					txtTt46.Enabled = true;
					rdoRightRoute.Checked = true;
				}
				else
				{
					txtTt46.Text = null;
					txtTt46.Enabled = false;
				}
				if (rdoRightRoute.Checked)
				{
					ValidateRightRouteType();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkHasWorkingLetter_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkHasWorkingLetter.Checked)
				{
					rdoRightRoute.Checked = true;
				}
				if (rdoRightRoute.Checked)
				{
					ValidateRightRouteType();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void chkHasAbsentLetter_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkHasAbsentLetter.Checked)
				{
					rdoRightRoute.Checked = true;
				}
				if (rdoRightRoute.Checked)
				{
					ValidateRightRouteType();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPatientCode_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (cboPatientCode != null)
				{
					if (string.IsNullOrWhiteSpace(cboPatientCode.Text))
					{
						cboPatientCode.Properties.View.ActiveFilter.Clear();
						return;
					}
					string displayMember = cboPatientCode.Properties.DisplayMember;
					cboPatientCode.Properties.View.ActiveFilter.Clear();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ClearHeinPatientCodeError()
		{
			try
			{
				object editValue = cboPatientCode.EditValue;
				if (!string.IsNullOrEmpty((editValue != null) ? editValue.ToString() : null))
				{
					dxValidationProvider1.SetValidationRule(cboPatientCode, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboPatientCode_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				isClickCboPatientTypeCode = true;
				if (isCallByRegistor || IsReset)
				{
					HIS_HEIN_PATIENT_TYPE val = (from o in BackendDataWorker.Get<HIS_HEIN_PATIENT_TYPE>()
						where o.IS_ACTIVE == 1 && o.HEIN_PATIENT_TYPE_CODE == cboPatientCode.EditValue.ToString()
						select o).FirstOrDefault();
					bool flag = false;
					if (val != null)
					{
						if (val.RIGHT_ROUTE_CODE == "TT" && !rdoWrongRoute.Checked)
						{
							flag = true;
						}
						else if (val.RIGHT_ROUTE_CODE == "DT" && !rdoRightRoute.Checked)
						{
							flag = true;
						}
						if (val.RIGHT_ROUTE_TYPE_CODE == "CC" && txtHeinRightRouteCode.Text != "CC")
						{
							flag = true;
						}
						else if (val.RIGHT_ROUTE_TYPE_CODE == "GT" && txtHeinRightRouteCode.Text != "GT")
						{
							flag = true;
						}
						else if (val.RIGHT_ROUTE_TYPE_CODE == "HK" && txtHeinRightRouteCode.Text != "HK")
						{
							flag = true;
						}
						else if (val.RIGHT_ROUTE_TYPE_CODE == "TH" && txtHeinRightRouteCode.Text != "TH")
						{
							flag = true;
						}
						else if (val.RIGHT_ROUTE_TYPE_CODE == null && cboHeinRightRoute.EditValue != null && txtHeinRightRouteCode.Text != null)
						{
							flag = true;
						}
						List<string> list = (from s in (val.TREATMENT_TYPE_IDS ?? "").Split(',')
							select s.Trim()).ToList();
						if (list.Count == 1 && list[0] == "")
						{
							list[0] = "0";
						}
						if (!list.Contains(TreatmentTypeId.ToString()))
						{
							flag = true;
						}
						if (flag && XtraMessageBox.Show(string.Format("Bạn có muốn cập nhật lại thông tin bệnh nhân theo Đối tượng khám chữa bệnh {0} không?", val.HEIN_PATIENT_TYPE_CODE), Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							if (val.RIGHT_ROUTE_CODE == "DT")
							{
								rdoRightRoute.Checked = true;
								rdoWrongRoute.Checked = false;
							}
							else if (val.RIGHT_ROUTE_CODE == "TT")
							{
								rdoWrongRoute.Checked = true;
								rdoRightRoute.Checked = false;
							}
							if (val.RIGHT_ROUTE_TYPE_CODE == "CC")
							{
								cboHeinRightRoute.EditValue = "CC";
							}
							else if (val.RIGHT_ROUTE_TYPE_CODE == "GT")
							{
								cboHeinRightRoute.EditValue = "GT";
							}
							else if (val.RIGHT_ROUTE_TYPE_CODE == "HK")
							{
								cboHeinRightRoute.EditValue = "HK";
							}
							else if (val.RIGHT_ROUTE_TYPE_CODE == "TH")
							{
								cboHeinRightRoute.EditValue = "TH";
							}
							else if (val.RIGHT_ROUTE_TYPE_CODE == null)
							{
								txtHeinRightRouteCode.Text = null;
								cboHeinRightRoute.EditValue = null;
							}
							long result;
							if (list.Count > 0 && list[0] != "0" && long.TryParse(list[0], out result))
							{
								TreatmentTypeId1(result);
							}
							if (list[0] == "0")
							{
								TreatmentTypeId1(1L);
							}
						}
					}
				}
				isClickCboPatientTypeCode = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientCode_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboPatientCode.EditValue = null;
				}
				cboPatientCode.Properties.Buttons[1].Visible = false;
				cboPatientCode.Properties.View.ActiveFilter.Clear();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboPatientCode_TextChanged_1(object sender, EventArgs e)
		{
			if (cboPatientCode != null)
			{
				string text = cboPatientCode.Text;
				if (string.IsNullOrWhiteSpace(text))
				{
					cboPatientCode.Properties.View.ActiveFilter.Clear();
					return;
				}
				cboPatientCode.Properties.View.ActiveFilter.Clear();
				cboPatientCode.Properties.View.ActiveFilterCriteria = CriteriaOperator.Parse(string.Format("[{0}] LIKE '%{1}%' OR [{2}] LIKE '%{1}%'", "HEIN_PATIENT_TYPE_CODE", text, "DESCRIPTION"));
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(His.UC.UCHein.Design.TemplateHeinBHYT1.Template__HeinBHYT1));
			DevExpress.Utils.SerializableAppearanceObject appearance2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled2 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SuperToolTip superToolTip = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem = new DevExpress.Utils.ToolTipItem();
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
			DevExpress.Utils.SerializableAppearanceObject appearance6 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered6 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed6 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled6 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearance7 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered7 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed7 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled7 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
			DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
			DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
			DevExpress.Utils.SerializableAppearanceObject appearance8 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered8 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed8 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled8 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearance9 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered9 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed9 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled9 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem5 = new DevExpress.Utils.ToolTipItem();
			DevExpress.Utils.SerializableAppearanceObject appearance10 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceHovered10 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearancePressed10 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SerializableAppearanceObject appearanceDisabled10 = new DevExpress.Utils.SerializableAppearanceObject();
			DevExpress.Utils.SuperToolTip superToolTip6 = new DevExpress.Utils.SuperToolTip();
			DevExpress.Utils.ToolTipItem toolTipItem6 = new DevExpress.Utils.ToolTipItem();
			this.txtHeinCardToTime = new DevExpress.XtraEditors.ButtonEdit();
			this.txtHeinCardFromTime = new DevExpress.XtraEditors.ButtonEdit();
			this.dtHeinCardToTime = new DevExpress.XtraEditors.DateEdit();
			this.dtHeinCardFromTime = new DevExpress.XtraEditors.DateEdit();
			this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
			this.cboPatientCode = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.chkTt46 = new DevExpress.XtraEditors.CheckEdit();
			this.txtTt46 = new DevExpress.XtraEditors.TextEdit();
			this.chkHasAbsentLetter = new DevExpress.XtraEditors.CheckEdit();
			this.chkHasWorkingLetter = new DevExpress.XtraEditors.CheckEdit();
			this.chkBaby = new DevExpress.XtraEditors.CheckEdit();
			this.btnCheckInfoBHYT = new DevExpress.XtraEditors.SimpleButton();
			this.rdoWrongRoute = new DevExpress.XtraEditors.CheckEdit();
			this.dtTransferInTimeTo = new DevExpress.XtraEditors.DateEdit();
			this.dtTransferInTimeFrom = new DevExpress.XtraEditors.DateEdit();
			this.panel4 = new System.Windows.Forms.Panel();
			this.txtDu5Nam = new DevExpress.XtraEditors.ButtonEdit();
			this.dtDu5Nam = new DevExpress.XtraEditors.DateEdit();
			this.cboNoiSong = new DevExpress.XtraEditors.GridLookUpEdit();
			this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.chkTempQN = new DevExpress.XtraEditors.CheckEdit();
			this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
			this.txtFreeCoPainTime = new DevExpress.XtraEditors.ButtonEdit();
			this.dtFreeCoPainTime = new DevExpress.XtraEditors.DateEdit();
			this.txtInCode = new DevExpress.XtraEditors.TextEdit();
			this.txtHNCode = new DevExpress.XtraEditors.TextEdit();
			this.txtCoPaidAccumulate = new DevExpress.XtraEditors.TextEdit();
			this.checkKhongKTHSD = new DevExpress.XtraEditors.CheckEdit();
			this.rdoRightRoute = new DevExpress.XtraEditors.CheckEdit();
			this.chkPaid6Month = new DevExpress.XtraEditors.CheckEdit();
			this.panel5 = new System.Windows.Forms.Panel();
			this.cboChanDoanTD = new Inventec.Desktop.CustomControl.CustomGridLookUpEditWithFilterMultiColumn();
			this.gridView3 = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
			this.txtDialogText = new DevExpress.XtraEditors.TextEdit();
			this.txtMaChanDoanTD = new DevExpress.XtraEditors.TextEdit();
			this.chkHasDialogText = new DevExpress.XtraEditors.CheckEdit();
			this.cboLyDoChuyen = new DevExpress.XtraEditors.LookUpEdit();
			this.txtMaLyDoChuyen = new DevExpress.XtraEditors.TextEdit();
			this.cboHeinRightRoute = new DevExpress.XtraEditors.LookUpEdit();
			this.txtHeinRightRouteCode = new DevExpress.XtraEditors.TextEdit();
			this.cboDKKCBBD = new Inventec.Desktop.CustomControl.CustomGridLookUpEditWithFilterMultiColumn();
			this.gridLookUpEdit1View = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
			this.txtMaDKKCBBD = new DevExpress.XtraEditors.TextEdit();
			this.cboNoiChuyenDen = new Inventec.Desktop.CustomControl.CustomGridLookUpEditWithFilterMultiColumn();
			this.gridView1 = new Inventec.Desktop.CustomControl.CustomGridViewWithFilterMultiColumn();
			this.txtMaNoiChuyenDen = new DevExpress.XtraEditors.TextEdit();
			this.cboHinhThucChuyen = new DevExpress.XtraEditors.LookUpEdit();
			this.txtMaHinhThucChuyen = new DevExpress.XtraEditors.TextEdit();
			this.txtAddress = new DevExpress.XtraEditors.TextEdit();
			this.txtMucHuong = new DevExpress.XtraEditors.TextEdit();
			this.chkJoin5Year = new DevExpress.XtraEditors.CheckEdit();
			this.chkMediRecordNoRouteTransfer = new DevExpress.XtraEditors.CheckEdit();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.chkMediRecordRouteTransfer = new DevExpress.XtraEditors.CheckEdit();
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtSoThe = new DevExpress.XtraEditors.ButtonEdit();
			this.cboSoThe = new DevExpress.XtraEditors.GridLookUpEdit();
			this.chkHasDobCertificate = new DevExpress.XtraEditors.CheckEdit();
			this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
			this.lblHeincardNumber = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblHeincardToDate = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblHeincardFromDate = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblCaptionAddress = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblMediRecordMediOrgForm = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciNoiChuyenDenName = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciDKKCBBDName = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblRightRouteType = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciRightRouteTypeName = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciTransPatiReasonCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciTransPatiReasoncbo = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblMediRecordBenefitSymbol = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciIcdMain = new DevExpress.XtraLayout.LayoutControlItem();
			this.panelICD = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblEditIcd = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciMediRecordRouteTransfer = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciMediRecordNoRouteTransfer = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciTransPatiFormCbo = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciTransPatiFormCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.lcirdoWrongRoute = new DevExpress.XtraLayout.LayoutControlItem();
			this.lcirdoRightRoute = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciKhongKTHSD = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciFreeCoPainTime = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciCoPaidAccumulate = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciInCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciHNCode = new DevExpress.XtraLayout.LayoutControlItem();
			this.lcichkJoin5Year = new DevExpress.XtraLayout.LayoutControlItem();
			this.lcichkPaid6Month = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblMediRecordLiveArea = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciDu5Nam = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciFordtTransferInTimeFrom = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciFordtTransferInTimeTo = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
			this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblHeincardMediOrg = new DevExpress.XtraLayout.LayoutControlItem();
			this.lciTempQN = new DevExpress.XtraLayout.LayoutControlItem();
			this.lblCaptionHasDobCertificate = new DevExpress.XtraLayout.LayoutControlItem();
			this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
			this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
			this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider();
			this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
			((System.ComponentModel.ISupportInitialize)this.txtHeinCardToTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtHeinCardFromTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardToTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardToTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardFromTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardFromTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).BeginInit();
			this.layoutControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.cboPatientCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkTt46.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtTt46.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasAbsentLetter.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasWorkingLetter.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkBaby.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.rdoWrongRoute.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeTo.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeTo.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeFrom.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeFrom.Properties).BeginInit();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtDu5Nam.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtDu5Nam.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtDu5Nam.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboNoiSong.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkTempQN.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.panelControl1).BeginInit();
			this.panelControl1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtFreeCoPainTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtFreeCoPainTime.Properties.CalendarTimeProperties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dtFreeCoPainTime.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtInCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtHNCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtCoPaidAccumulate.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.checkKhongKTHSD.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.rdoRightRoute.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkPaid6Month.Properties).BeginInit();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.cboChanDoanTD.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtDialogText.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaChanDoanTD.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasDialogText.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboLyDoChuyen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaLyDoChuyen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboHeinRightRoute.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtHeinRightRouteCode.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboDKKCBBD.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaDKKCBBD.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboNoiChuyenDen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaNoiChuyenDen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboHinhThucChuyen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaHinhThucChuyen.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtAddress.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.txtMucHuong.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkJoin5Year.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkMediRecordNoRouteTransfer.Properties).BeginInit();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.chkMediRecordRouteTransfer.Properties).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.txtSoThe.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.cboSoThe.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasDobCertificate.Properties).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardNumber).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardToDate).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardFromDate).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblCaptionAddress).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordMediOrgForm).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciNoiChuyenDenName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciDKKCBBDName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblRightRouteType).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciRightRouteTypeName).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiReasonCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiReasoncbo).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordBenefitSymbol).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciIcdMain).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.panelICD).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblEditIcd).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciMediRecordRouteTransfer).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciMediRecordNoRouteTransfer).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiFormCbo).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiFormCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lcirdoWrongRoute).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lcirdoRightRoute).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciKhongKTHSD).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciFreeCoPainTime).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciCoPaidAccumulate).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciInCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciHNCode).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lcichkJoin5Year).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lcichkPaid6Month).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordLiveArea).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciDu5Nam).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciFordtTransferInTimeFrom).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciFordtTransferInTimeTo).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardMediOrg).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lciTempQN).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.lblCaptionHasDobCertificate).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxErrorProvider1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemMemoEdit1).BeginInit();
			base.SuspendLayout();
			this.txtHeinCardToTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtHeinCardToTime.Location = new System.Drawing.Point(0, 0);
			this.txtHeinCardToTime.Name = "txtHeinCardToTime";
			this.txtHeinCardToTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
			});
			this.txtHeinCardToTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtHeinCardToTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtHeinCardToTime.Properties.Mask.EditMask = "\\d{2}/\\d{2}/\\d{4}";
			this.txtHeinCardToTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.txtHeinCardToTime.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.txtHeinCardToTime.Size = new System.Drawing.Size(111, 20);
			this.txtHeinCardToTime.TabIndex = 4;
			this.txtHeinCardToTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtHeinCardToTime_ButtonClick);
			this.txtHeinCardToTime.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(txtHeinCardToTime_InvalidValue);
			this.txtHeinCardToTime.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtHeinCardToTime_PreviewKeyDown);
			this.txtHeinCardFromTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtHeinCardFromTime.Location = new System.Drawing.Point(0, 0);
			this.txtHeinCardFromTime.Name = "txtHeinCardFromTime";
			this.txtHeinCardFromTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
			});
			this.txtHeinCardFromTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtHeinCardFromTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtHeinCardFromTime.Properties.Mask.EditMask = "\\d{2}/\\d{2}/\\d{4}";
			this.txtHeinCardFromTime.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.txtHeinCardFromTime.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.txtHeinCardFromTime.Size = new System.Drawing.Size(111, 20);
			this.txtHeinCardFromTime.TabIndex = 3;
			this.txtHeinCardFromTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtHeinCardFromTime_ButtonClick);
			this.txtHeinCardFromTime.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(txtHeinCardFromTime_InvalidValue);
			this.txtHeinCardFromTime.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtHeinCardFromTime_PreviewKeyDown);
			this.dtHeinCardToTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dtHeinCardToTime.EditValue = null;
			this.dtHeinCardToTime.Location = new System.Drawing.Point(0, 0);
			this.dtHeinCardToTime.Name = "dtHeinCardToTime";
			this.dtHeinCardToTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "HH:mm";
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.EditFormat.FormatString = "HH:mm";
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardToTime.Properties.CalendarTimeProperties.Mask.EditMask = "HH:mm";
			this.dtHeinCardToTime.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
			this.dtHeinCardToTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtHeinCardToTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardToTime.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtHeinCardToTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardToTime.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtHeinCardToTime.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.dtHeinCardToTime.Properties.NullValuePrompt = "dd/MM/yyyy";
			this.dtHeinCardToTime.Properties.NullValuePromptShowForEmptyValue = true;
			this.dtHeinCardToTime.Properties.ShowNullValuePromptWhenFocused = true;
			this.dtHeinCardToTime.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
			this.dtHeinCardToTime.Size = new System.Drawing.Size(111, 20);
			this.dtHeinCardToTime.TabIndex = 4;
			this.dtHeinCardToTime.Visible = false;
			this.dtHeinCardToTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtHeinCardToTime_Closed);
			this.dtHeinCardToTime.EditValueChanged += new System.EventHandler(dtHeinCardToTime_EditValueChanged);
			this.dtHeinCardToTime.KeyDown += new System.Windows.Forms.KeyEventHandler(dtHeinCardToTime_KeyDown);
			this.dtHeinCardFromTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dtHeinCardFromTime.EditValue = null;
			this.dtHeinCardFromTime.Location = new System.Drawing.Point(0, 0);
			this.dtHeinCardFromTime.Name = "dtHeinCardFromTime";
			this.dtHeinCardFromTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.DisplayFormat.FormatString = "HH:mm";
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.EditFormat.FormatString = "HH:mm";
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardFromTime.Properties.CalendarTimeProperties.Mask.EditMask = "HH:mm";
			this.dtHeinCardFromTime.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
			this.dtHeinCardFromTime.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtHeinCardFromTime.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardFromTime.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtHeinCardFromTime.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtHeinCardFromTime.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtHeinCardFromTime.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.dtHeinCardFromTime.Properties.NullValuePrompt = "dd/MM/yyyy";
			this.dtHeinCardFromTime.Properties.NullValuePromptShowForEmptyValue = true;
			this.dtHeinCardFromTime.Properties.ShowNullValuePromptWhenFocused = true;
			this.dtHeinCardFromTime.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
			this.dtHeinCardFromTime.Size = new System.Drawing.Size(111, 20);
			this.dtHeinCardFromTime.TabIndex = 3;
			this.dtHeinCardFromTime.Visible = false;
			this.dtHeinCardFromTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtHeinCardFromTime_Closed);
			this.dtHeinCardFromTime.EditValueChanged += new System.EventHandler(dtHeinCardFromTime_EditValueChanged);
			this.dtHeinCardFromTime.KeyDown += new System.Windows.Forms.KeyEventHandler(dtHeinCardFromTime_KeyDown);
			this.layoutControl1.Controls.Add(this.cboPatientCode);
			this.layoutControl1.Controls.Add(this.chkTt46);
			this.layoutControl1.Controls.Add(this.txtTt46);
			this.layoutControl1.Controls.Add(this.chkHasAbsentLetter);
			this.layoutControl1.Controls.Add(this.chkHasWorkingLetter);
			this.layoutControl1.Controls.Add(this.chkBaby);
			this.layoutControl1.Controls.Add(this.btnCheckInfoBHYT);
			this.layoutControl1.Controls.Add(this.rdoWrongRoute);
			this.layoutControl1.Controls.Add(this.dtTransferInTimeTo);
			this.layoutControl1.Controls.Add(this.dtTransferInTimeFrom);
			this.layoutControl1.Controls.Add(this.panel4);
			this.layoutControl1.Controls.Add(this.cboNoiSong);
			this.layoutControl1.Controls.Add(this.chkTempQN);
			this.layoutControl1.Controls.Add(this.panelControl1);
			this.layoutControl1.Controls.Add(this.txtInCode);
			this.layoutControl1.Controls.Add(this.txtHNCode);
			this.layoutControl1.Controls.Add(this.txtCoPaidAccumulate);
			this.layoutControl1.Controls.Add(this.checkKhongKTHSD);
			this.layoutControl1.Controls.Add(this.rdoRightRoute);
			this.layoutControl1.Controls.Add(this.chkPaid6Month);
			this.layoutControl1.Controls.Add(this.panel5);
			this.layoutControl1.Controls.Add(this.txtMaChanDoanTD);
			this.layoutControl1.Controls.Add(this.chkHasDialogText);
			this.layoutControl1.Controls.Add(this.cboLyDoChuyen);
			this.layoutControl1.Controls.Add(this.txtMaLyDoChuyen);
			this.layoutControl1.Controls.Add(this.cboHeinRightRoute);
			this.layoutControl1.Controls.Add(this.txtHeinRightRouteCode);
			this.layoutControl1.Controls.Add(this.cboDKKCBBD);
			this.layoutControl1.Controls.Add(this.txtMaDKKCBBD);
			this.layoutControl1.Controls.Add(this.cboNoiChuyenDen);
			this.layoutControl1.Controls.Add(this.txtMaNoiChuyenDen);
			this.layoutControl1.Controls.Add(this.cboHinhThucChuyen);
			this.layoutControl1.Controls.Add(this.txtMaHinhThucChuyen);
			this.layoutControl1.Controls.Add(this.txtAddress);
			this.layoutControl1.Controls.Add(this.txtMucHuong);
			this.layoutControl1.Controls.Add(this.chkJoin5Year);
			this.layoutControl1.Controls.Add(this.chkMediRecordNoRouteTransfer);
			this.layoutControl1.Controls.Add(this.panel3);
			this.layoutControl1.Controls.Add(this.panel2);
			this.layoutControl1.Controls.Add(this.chkMediRecordRouteTransfer);
			this.layoutControl1.Controls.Add(this.panel1);
			this.layoutControl1.Controls.Add(this.chkHasDobCertificate);
			this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutControl1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[1] { this.layoutControlItem8 });
			this.layoutControl1.Location = new System.Drawing.Point(0, 0);
			this.layoutControl1.Name = "layoutControl1";
			this.layoutControl1.OptionsFocus.EnableAutoTabOrder = false;
			this.layoutControl1.Root = this.layoutControlGroup1;
			this.layoutControl1.Size = new System.Drawing.Size(1320, 127);
			this.layoutControl1.TabIndex = 146;
			this.layoutControl1.Text = "layoutControl1";
			this.cboPatientCode.Location = new System.Drawing.Point(819, 28);
			this.cboPatientCode.Name = "cboPatientCode";
			this.cboPatientCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance, appearanceHovered, appearancePressed, appearanceDisabled, "", null, null, true)
			});
			this.cboPatientCode.Properties.NullText = "";
			this.cboPatientCode.Properties.View = this.gridView4;
			this.cboPatientCode.Size = new System.Drawing.Size(79, 20);
			this.cboPatientCode.StyleController = this.layoutControl1;
			this.cboPatientCode.TabIndex = 42;
			this.cboPatientCode.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboPatientCode_Closed);
			this.cboPatientCode.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboPatientCode_ButtonClick);
			this.cboPatientCode.EditValueChanged += new System.EventHandler(cboPatientCode_EditValueChanged);
			this.cboPatientCode.TextChanged += new System.EventHandler(cboPatientCode_TextChanged_1);
			this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView4.Name = "gridView4";
			this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView4.OptionsView.ShowGroupPanel = false;
			this.chkTt46.Location = new System.Drawing.Point(602, 100);
			this.chkTt46.Name = "chkTt46";
			this.chkTt46.Properties.Caption = "TT 46";
			this.chkTt46.Size = new System.Drawing.Size(49, 19);
			this.chkTt46.StyleController = this.layoutControl1;
			this.chkTt46.TabIndex = 39;
			this.chkTt46.ToolTip = "Với các bệnh nhân điều trị dài ngày có thẻ BHYT khác nơi KCB ban đầu vẫn được hưởng như đúng tuyến theo thông tư 46";
			this.chkTt46.CheckedChanged += new System.EventHandler(chkTt46_CheckedChanged);
			this.txtTt46.Location = new System.Drawing.Point(655, 100);
			this.txtTt46.Name = "txtTt46";
			this.txtTt46.Properties.NullValuePrompt = "Ghi chú";
			this.txtTt46.Properties.NullValuePromptShowForEmptyValue = true;
			this.txtTt46.Properties.ShowNullValuePromptWhenFocused = true;
			this.txtTt46.Size = new System.Drawing.Size(255, 20);
			this.txtTt46.StyleController = this.layoutControl1;
			this.txtTt46.TabIndex = 38;
			this.txtTt46.ToolTip = "Với trường hợp bệnh nhân có thẻ BHYT khác nơi KCB ban đầu nhưng có giấy đăng ký tạm trú, tạm vắng trên địa bàn sẽ được hưởng như đúng tuyến";
			this.chkHasAbsentLetter.Location = new System.Drawing.Point(424, 100);
			this.chkHasAbsentLetter.Name = "chkHasAbsentLetter";
			this.chkHasAbsentLetter.Properties.Caption = "Giấy đăng ký tạm trú, tạm vắng";
			this.chkHasAbsentLetter.Size = new System.Drawing.Size(174, 19);
			this.chkHasAbsentLetter.StyleController = this.layoutControl1;
			this.chkHasAbsentLetter.TabIndex = 37;
			this.chkHasAbsentLetter.ToolTip = "Với trường hợp bệnh nhân có thẻ BHYT khác nơi KCB ban đầu nhưng có giấy đăng ký tạm trú, tạm vắng trên địa bàn sẽ được hưởng như đúng tuyến";
			this.chkHasAbsentLetter.CheckedChanged += new System.EventHandler(chkHasAbsentLetter_CheckedChanged);
			this.chkHasWorkingLetter.Location = new System.Drawing.Point(66, 100);
			this.chkHasWorkingLetter.Name = "chkHasWorkingLetter";
			this.chkHasWorkingLetter.Properties.Caption = "Giấy đi công tác, quyết định nhập học";
			this.chkHasWorkingLetter.Size = new System.Drawing.Size(354, 19);
			this.chkHasWorkingLetter.StyleController = this.layoutControl1;
			this.chkHasWorkingLetter.TabIndex = 36;
			this.chkHasWorkingLetter.ToolTip = "Với trường hợp bệnh nhân có thẻ BHYT\u00a0khác nơi KCB ban đầu nhưng có giấy đi công tác hoặc quyết định nhập học sẽ được hưởng như đúng tuyến";
			this.chkHasWorkingLetter.CheckedChanged += new System.EventHandler(chkHasWorkingLetter_CheckedChanged);
			this.chkBaby.Enabled = false;
			this.chkBaby.Location = new System.Drawing.Point(932, 28);
			this.chkBaby.Name = "chkBaby";
			this.chkBaby.Properties.Caption = "";
			this.chkBaby.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.chkBaby.Size = new System.Drawing.Size(16, 19);
			this.chkBaby.StyleController = this.layoutControl1;
			this.chkBaby.TabIndex = 35;
			this.chkBaby.ToolTip = "Trẻ cần điều trị ngay sau khi sinh";
			this.chkBaby.CheckedChanged += new System.EventHandler(chkBaby_CheckedChanged);
			this.btnCheckInfoBHYT.Image = (System.Drawing.Image)resources.GetObject("btnCheckInfoBHYT.Image");
			this.btnCheckInfoBHYT.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnCheckInfoBHYT.Location = new System.Drawing.Point(430, 2);
			this.btnCheckInfoBHYT.Name = "btnCheckInfoBHYT";
			this.btnCheckInfoBHYT.Size = new System.Drawing.Size(24, 22);
			this.btnCheckInfoBHYT.StyleController = this.layoutControl1;
			this.btnCheckInfoBHYT.TabIndex = 34;
			this.btnCheckInfoBHYT.ToolTip = "Kiểm tra thông tin thẻ BHYT";
			this.btnCheckInfoBHYT.Click += new System.EventHandler(btnCheckInfoBHYT_Click);
			this.rdoWrongRoute.Location = new System.Drawing.Point(445, 28);
			this.rdoWrongRoute.Name = "rdoWrongRoute";
			this.rdoWrongRoute.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.rdoWrongRoute.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.rdoWrongRoute.Properties.Caption = "";
			this.rdoWrongRoute.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
			this.rdoWrongRoute.Properties.FullFocusRect = true;
			this.rdoWrongRoute.Properties.RadioGroupIndex = 10;
			this.rdoWrongRoute.Size = new System.Drawing.Size(25, 19);
			this.rdoWrongRoute.StyleController = this.layoutControl1;
			this.rdoWrongRoute.TabIndex = 10;
			this.rdoWrongRoute.TabStop = false;
			this.rdoWrongRoute.CheckedChanged += new System.EventHandler(rdoWrongRoute_CheckedChanged);
			this.rdoWrongRoute.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(rdoWrongRoute_PreviewKeyDown);
			this.dtTransferInTimeTo.EditValue = null;
			this.dtTransferInTimeTo.Location = new System.Drawing.Point(459, 76);
			this.dtTransferInTimeTo.Name = "dtTransferInTimeTo";
			this.dtTransferInTimeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtTransferInTimeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtTransferInTimeTo.Size = new System.Drawing.Size(111, 20);
			this.dtTransferInTimeTo.StyleController = this.layoutControl1;
			this.dtTransferInTimeTo.TabIndex = 33;
			this.dtTransferInTimeTo.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtTransferInTimeTo_Closed);
			this.dtTransferInTimeTo.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(dtTransferInTimeTo_PreviewKeyDown);
			this.dtTransferInTimeFrom.EditValue = null;
			this.dtTransferInTimeFrom.Location = new System.Drawing.Point(309, 76);
			this.dtTransferInTimeFrom.Name = "dtTransferInTimeFrom";
			this.dtTransferInTimeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtTransferInTimeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtTransferInTimeFrom.Size = new System.Drawing.Size(111, 20);
			this.dtTransferInTimeFrom.StyleController = this.layoutControl1;
			this.dtTransferInTimeFrom.TabIndex = 32;
			this.dtTransferInTimeFrom.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtTransferInTimeFrom_Closed);
			this.dtTransferInTimeFrom.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(dtTransferInTimeFrom_PreviewKeyDown);
			this.panel4.Controls.Add(this.txtDu5Nam);
			this.panel4.Controls.Add(this.dtDu5Nam);
			this.panel4.Location = new System.Drawing.Point(1011, 2);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(101, 20);
			this.panel4.TabIndex = 31;
			this.txtDu5Nam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDu5Nam.Location = new System.Drawing.Point(0, 0);
			this.txtDu5Nam.Name = "txtDu5Nam";
			this.txtDu5Nam.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
			});
			this.txtDu5Nam.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtDu5Nam.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.txtDu5Nam.Properties.Mask.EditMask = "\\d{2}/\\d{2}/\\d{4}";
			this.txtDu5Nam.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.txtDu5Nam.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.txtDu5Nam.Size = new System.Drawing.Size(101, 20);
			this.txtDu5Nam.TabIndex = 6;
			this.txtDu5Nam.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtDu5Nam_ButtonClick);
			this.txtDu5Nam.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtDu5Nam_PreviewKeyDown);
			this.dtDu5Nam.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dtDu5Nam.EditValue = null;
			this.dtDu5Nam.Location = new System.Drawing.Point(0, 0);
			this.dtDu5Nam.Name = "dtDu5Nam";
			this.dtDu5Nam.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtDu5Nam.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtDu5Nam.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
			this.dtDu5Nam.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtDu5Nam.Properties.EditFormat.FormatString = "dd/MM/yyyy";
			this.dtDu5Nam.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
			this.dtDu5Nam.Properties.Mask.EditMask = "dd/MM/yyyy";
			this.dtDu5Nam.Size = new System.Drawing.Size(101, 20);
			this.dtDu5Nam.TabIndex = 6;
			this.dtDu5Nam.Visible = false;
			this.dtDu5Nam.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtDu5Nam_Closed);
			this.dtDu5Nam.KeyDown += new System.Windows.Forms.KeyEventHandler(dtDu5Nam_KeyDown);
			this.cboNoiSong.Location = new System.Drawing.Point(87, 52);
			this.cboNoiSong.Name = "cboNoiSong";
			this.cboNoiSong.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboNoiSong.Properties.AutoComplete = false;
			this.cboNoiSong.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance2, appearanceHovered2, appearancePressed2, appearanceDisabled2, "", null, null, true)
			});
			this.cboNoiSong.Properties.NullText = "";
			this.cboNoiSong.Properties.View = this.gridView2;
			this.cboNoiSong.Size = new System.Drawing.Size(71, 20);
			this.cboNoiSong.StyleController = this.layoutControl1;
			this.cboNoiSong.TabIndex = 17;
			this.cboNoiSong.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboNoiSong_Closed);
			this.cboNoiSong.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboNoiSong_ButtonClick);
			this.cboNoiSong.EditValueChanged += new System.EventHandler(cboNoiSong_EditValueChanged);
			this.cboNoiSong.KeyUp += new System.Windows.Forms.KeyEventHandler(cboNoiSong_KeyUp);
			this.cboNoiSong.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cboNoiSong_PreviewKeyDown);
			this.gridView2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView2.Name = "gridView2";
			this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView2.OptionsView.ShowGroupPanel = false;
			this.chkTempQN.Location = new System.Drawing.Point(115, 2);
			this.chkTempQN.Name = "chkTempQN";
			this.chkTempQN.Properties.Caption = "";
			this.chkTempQN.Size = new System.Drawing.Size(19, 19);
			this.chkTempQN.StyleController = this.layoutControl1;
			this.chkTempQN.TabIndex = 30;
			this.chkTempQN.CheckedChanged += new System.EventHandler(chkTempQN_CheckedChanged);
			this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
			this.panelControl1.Controls.Add(this.txtFreeCoPainTime);
			this.panelControl1.Controls.Add(this.dtFreeCoPainTime);
			this.panelControl1.Location = new System.Drawing.Point(1181, 28);
			this.panelControl1.Name = "panelControl1";
			this.panelControl1.Size = new System.Drawing.Size(97, 20);
			this.panelControl1.TabIndex = 15;
			this.txtFreeCoPainTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtFreeCoPainTime.EditValue = "";
			this.txtFreeCoPainTime.Location = new System.Drawing.Point(0, 0);
			this.txtFreeCoPainTime.Name = "txtFreeCoPainTime";
			this.txtFreeCoPainTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down)
			});
			this.txtFreeCoPainTime.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtFreeCoPainTime.Properties.MaxLength = 12;
			this.txtFreeCoPainTime.Size = new System.Drawing.Size(97, 20);
			this.txtFreeCoPainTime.TabIndex = 1;
			this.txtFreeCoPainTime.ToolTip = "Thời điểm miễn cùng chi trả";
			this.txtFreeCoPainTime.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtFreeCoPainTime_ButtonClick);
			this.txtFreeCoPainTime.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(txtFreeCoPainTime_InvalidValue);
			this.txtFreeCoPainTime.TextChanged += new System.EventHandler(txtDTMCChiTra_TextChanged);
			this.txtFreeCoPainTime.Click += new System.EventHandler(txtFreeCoPainTime_Click);
			this.txtFreeCoPainTime.KeyDown += new System.Windows.Forms.KeyEventHandler(txtFreeCoPainTime_KeyDown);
			this.txtFreeCoPainTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreeCoPainTime_KeyPress);
			this.txtFreeCoPainTime.Validating += new System.ComponentModel.CancelEventHandler(txtFreeCoPainTime_Validating);
			this.dtFreeCoPainTime.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dtFreeCoPainTime.EditValue = null;
			this.dtFreeCoPainTime.Location = new System.Drawing.Point(0, 0);
			this.dtFreeCoPainTime.Name = "dtFreeCoPainTime";
			this.dtFreeCoPainTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtFreeCoPainTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.dtFreeCoPainTime.Size = new System.Drawing.Size(97, 20);
			this.dtFreeCoPainTime.TabIndex = 2;
			this.dtFreeCoPainTime.Visible = false;
			this.dtFreeCoPainTime.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(dtFreeCoPainTime_Closed);
			this.dtFreeCoPainTime.KeyDown += new System.Windows.Forms.KeyEventHandler(dtFreeCoPainTime_KeyDown);
			this.txtInCode.EditValue = "";
			this.txtInCode.Enabled = false;
			this.txtInCode.Location = new System.Drawing.Point(977, 52);
			this.txtInCode.Name = "txtInCode";
			this.txtInCode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtInCode.Properties.MaxLength = 20;
			this.txtInCode.Size = new System.Drawing.Size(50, 20);
			this.txtInCode.StyleController = this.layoutControl1;
			this.txtInCode.TabIndex = 23;
			this.txtInCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtInCode_PreviewKeyDown);
			this.txtHNCode.EditValue = "";
			this.txtHNCode.Location = new System.Drawing.Point(1116, 52);
			this.txtHNCode.Name = "txtHNCode";
			this.txtHNCode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtHNCode.Properties.MaxLength = 20;
			this.txtHNCode.Size = new System.Drawing.Size(202, 20);
			this.txtHNCode.StyleController = this.layoutControl1;
			this.txtHNCode.TabIndex = 24;
			this.txtHNCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtHNCode_PreviewKeyDown);
			this.txtCoPaidAccumulate.Location = new System.Drawing.Point(1034, 100);
			this.txtCoPaidAccumulate.Name = "txtCoPaidAccumulate";
			this.txtCoPaidAccumulate.Properties.MaxLength = 18;
			this.txtCoPaidAccumulate.Size = new System.Drawing.Size(284, 20);
			this.txtCoPaidAccumulate.StyleController = this.layoutControl1;
			this.txtCoPaidAccumulate.TabIndex = 31;
			this.txtCoPaidAccumulate.ToolTip = "Số tiền cùng chi trả lũy kế";
			this.checkKhongKTHSD.Location = new System.Drawing.Point(898, 2);
			this.checkKhongKTHSD.Name = "checkKhongKTHSD";
			this.checkKhongKTHSD.Properties.Caption = "";
			this.checkKhongKTHSD.Properties.FullFocusRect = true;
			this.checkKhongKTHSD.Size = new System.Drawing.Size(19, 19);
			this.checkKhongKTHSD.StyleController = this.layoutControl1;
			this.checkKhongKTHSD.TabIndex = 5;
			this.rdoRightRoute.EditValue = true;
			this.rdoRightRoute.Location = new System.Drawing.Point(538, 28);
			this.rdoRightRoute.Name = "rdoRightRoute";
			this.rdoRightRoute.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.rdoRightRoute.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.rdoRightRoute.Properties.Caption = "";
			this.rdoRightRoute.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
			this.rdoRightRoute.Properties.FullFocusRect = true;
			this.rdoRightRoute.Properties.RadioGroupIndex = 10;
			this.rdoRightRoute.Size = new System.Drawing.Size(22, 19);
			this.rdoRightRoute.StyleController = this.layoutControl1;
			this.rdoRightRoute.TabIndex = 11;
			this.rdoRightRoute.CheckedChanged += new System.EventHandler(rdoRightRoute_CheckedChanged);
			this.rdoRightRoute.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(rdoRightRoute_PreviewKeyDown);
			this.chkPaid6Month.Location = new System.Drawing.Point(1082, 28);
			this.chkPaid6Month.Name = "chkPaid6Month";
			this.chkPaid6Month.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkPaid6Month.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkPaid6Month.Properties.Caption = "";
			this.chkPaid6Month.Properties.FullFocusRect = true;
			this.chkPaid6Month.Size = new System.Drawing.Size(30, 19);
			this.chkPaid6Month.StyleController = this.layoutControl1;
			toolTipItem.Text = "Đồng chi trả lũy kế đủ 6 tháng lương tối thiểu";
			superToolTip.Items.Add(toolTipItem);
			this.chkPaid6Month.SuperTip = superToolTip;
			this.chkPaid6Month.TabIndex = 15;
			this.chkPaid6Month.CheckedChanged += new System.EventHandler(chkPaid6Month_CheckedChanged);
			this.chkPaid6Month.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkPaid6Month_PreviewKeyDown);
			this.panel5.Controls.Add(this.cboChanDoanTD);
			this.panel5.Controls.Add(this.txtDialogText);
			this.panel5.Location = new System.Drawing.Point(652, 52);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(168, 20);
			this.panel5.TabIndex = 21;
			this.cboChanDoanTD.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboChanDoanTD.Location = new System.Drawing.Point(0, 0);
			this.cboChanDoanTD.Name = "cboChanDoanTD";
			this.cboChanDoanTD.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboChanDoanTD.Properties.AutoComplete = false;
			this.cboChanDoanTD.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance3, appearanceHovered3, appearancePressed3, appearanceDisabled3, "", null, null, true)
			});
			this.cboChanDoanTD.Properties.NullText = "";
			this.cboChanDoanTD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboChanDoanTD.Properties.View = this.gridView3;
			this.cboChanDoanTD.Size = new System.Drawing.Size(168, 20);
			this.cboChanDoanTD.TabIndex = 1;
			this.cboChanDoanTD.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboChanDoanTD_Closed);
			this.cboChanDoanTD.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboChanDoanTD_ButtonClick);
			this.cboChanDoanTD.TextChanged += new System.EventHandler(cboChanDoanTD_TextChanged);
			this.cboChanDoanTD.KeyUp += new System.Windows.Forms.KeyEventHandler(cboChanDoanTD_KeyUp);
			this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView3.Name = "gridView3";
			this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView3.OptionsView.ShowGroupPanel = false;
			this.txtDialogText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtDialogText.EnterMoveNextControl = true;
			this.txtDialogText.Location = new System.Drawing.Point(0, 0);
			this.txtDialogText.Name = "txtDialogText";
			this.txtDialogText.Size = new System.Drawing.Size(168, 20);
			this.txtDialogText.TabIndex = 1;
			this.txtMaChanDoanTD.Location = new System.Drawing.Point(602, 52);
			this.txtMaChanDoanTD.Name = "txtMaChanDoanTD";
			this.txtMaChanDoanTD.Properties.Appearance.Options.UseTextOptions = true;
			this.txtMaChanDoanTD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtMaChanDoanTD.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtMaChanDoanTD.Size = new System.Drawing.Size(50, 20);
			this.txtMaChanDoanTD.StyleController = this.layoutControl1;
			this.txtMaChanDoanTD.TabIndex = 20;
			this.txtMaChanDoanTD.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(txtMaChanDoanTD_InvalidValue);
			this.txtMaChanDoanTD.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtMaChuanDoanTD_PreviewKeyDown);
			this.txtMaChanDoanTD.Validating += new System.ComponentModel.CancelEventHandler(txtMaChanDoanTD_Validating);
			this.chkHasDialogText.Location = new System.Drawing.Point(859, 52);
			this.chkHasDialogText.Name = "chkHasDialogText";
			this.chkHasDialogText.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkHasDialogText.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkHasDialogText.Properties.Caption = "";
			this.chkHasDialogText.Properties.FullFocusRect = true;
			this.chkHasDialogText.Size = new System.Drawing.Size(19, 19);
			this.chkHasDialogText.StyleController = this.layoutControl1;
			this.chkHasDialogText.TabIndex = 22;
			this.chkHasDialogText.CheckedChanged += new System.EventHandler(chkHasDialogText_CheckedChanged);
			this.chkHasDialogText.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkHasDialogText_PreviewKeyDown);
			this.cboLyDoChuyen.Location = new System.Drawing.Point(1069, 76);
			this.cboLyDoChuyen.Name = "cboLyDoChuyen";
			this.cboLyDoChuyen.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboLyDoChuyen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance4, appearanceHovered4, appearancePressed4, appearanceDisabled4, "", null, null, true)
			});
			this.cboLyDoChuyen.Properties.NullText = "";
			this.cboLyDoChuyen.Properties.PopupSizeable = false;
			this.cboLyDoChuyen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboLyDoChuyen.Size = new System.Drawing.Size(249, 20);
			this.cboLyDoChuyen.StyleController = this.layoutControl1;
			this.cboLyDoChuyen.TabIndex = 30;
			this.cboLyDoChuyen.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboLyDoChuyen_Closed);
			this.cboLyDoChuyen.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboLyDoChuyen_ButtonClick);
			this.cboLyDoChuyen.EditValueChanged += new System.EventHandler(cboLyDoChuyen_EditValueChanged);
			this.cboLyDoChuyen.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cboLyDoChuyen_PreviewKeyDown);
			this.txtMaLyDoChuyen.Location = new System.Drawing.Point(1041, 76);
			this.txtMaLyDoChuyen.Name = "txtMaLyDoChuyen";
			this.txtMaLyDoChuyen.Properties.Appearance.Options.UseTextOptions = true;
			this.txtMaLyDoChuyen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtMaLyDoChuyen.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtMaLyDoChuyen.Size = new System.Drawing.Size(28, 20);
			this.txtMaLyDoChuyen.StyleController = this.layoutControl1;
			this.txtMaLyDoChuyen.TabIndex = 29;
			this.txtMaLyDoChuyen.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtMaLyDoChuyen_PreviewKeyDown);
			this.cboHeinRightRoute.Enabled = false;
			this.cboHeinRightRoute.EnterMoveNextControl = true;
			this.cboHeinRightRoute.Location = new System.Drawing.Point(687, 28);
			this.cboHeinRightRoute.Name = "cboHeinRightRoute";
			this.cboHeinRightRoute.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboHeinRightRoute.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance5, appearanceHovered5, appearancePressed5, appearanceDisabled5, "", null, null, true)
			});
			this.cboHeinRightRoute.Properties.NullText = "";
			this.cboHeinRightRoute.Properties.PopupSizeable = false;
			this.cboHeinRightRoute.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboHeinRightRoute.Size = new System.Drawing.Size(98, 20);
			this.cboHeinRightRoute.StyleController = this.layoutControl1;
			this.cboHeinRightRoute.TabIndex = 13;
			this.cboHeinRightRoute.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboHeinRightRoute_Closed);
			this.cboHeinRightRoute.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboHeinRightRoute_ButtonClick);
			this.cboHeinRightRoute.EditValueChanged += new System.EventHandler(cboHeinRightRoute_EditValueChanged);
			this.cboHeinRightRoute.KeyUp += new System.Windows.Forms.KeyEventHandler(cboHeinRightRoute_KeyUp);
			this.cboHeinRightRoute.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(cboHeinRightRoute_PreviewKeyDown);
			this.txtHeinRightRouteCode.Enabled = false;
			this.txtHeinRightRouteCode.Location = new System.Drawing.Point(649, 28);
			this.txtHeinRightRouteCode.Name = "txtHeinRightRouteCode";
			this.txtHeinRightRouteCode.Properties.Appearance.Options.UseTextOptions = true;
			this.txtHeinRightRouteCode.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtHeinRightRouteCode.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtHeinRightRouteCode.Size = new System.Drawing.Size(38, 20);
			this.txtHeinRightRouteCode.StyleController = this.layoutControl1;
			this.txtHeinRightRouteCode.TabIndex = 12;
			this.txtHeinRightRouteCode.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtHeinRightRouteCode_PreviewKeyDown);
			this.cboDKKCBBD.EditValue = "";
			this.cboDKKCBBD.Location = new System.Drawing.Point(151, 28);
			this.cboDKKCBBD.Name = "cboDKKCBBD";
			this.cboDKKCBBD.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[1]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
			});
			this.cboDKKCBBD.Properties.NullText = "";
			this.cboDKKCBBD.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
			this.cboDKKCBBD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboDKKCBBD.Properties.View = this.gridLookUpEdit1View;
			this.cboDKKCBBD.Size = new System.Drawing.Size(226, 20);
			this.cboDKKCBBD.StyleController = this.layoutControl1;
			this.cboDKKCBBD.TabIndex = 9;
			this.cboDKKCBBD.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboDKKCBBD_Closed);
			this.cboDKKCBBD.KeyUp += new System.Windows.Forms.KeyEventHandler(cboDKKCBBD_KeyUp);
			this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
			this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridLookUpEdit1View.OptionsView.ShowColumnHeaders = false;
			this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
			this.gridLookUpEdit1View.OptionsView.ShowIndicator = false;
			this.txtMaDKKCBBD.Location = new System.Drawing.Point(87, 28);
			this.txtMaDKKCBBD.Name = "txtMaDKKCBBD";
			this.txtMaDKKCBBD.Properties.Appearance.Options.UseTextOptions = true;
			this.txtMaDKKCBBD.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtMaDKKCBBD.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtMaDKKCBBD.Size = new System.Drawing.Size(64, 20);
			this.txtMaDKKCBBD.StyleController = this.layoutControl1;
			this.txtMaDKKCBBD.TabIndex = 8;
			this.txtMaDKKCBBD.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtMaDKKCBBD_PreviewKeyDown);
			this.cboNoiChuyenDen.EnterMoveNextControl = true;
			this.cboNoiChuyenDen.Location = new System.Drawing.Point(330, 52);
			this.cboNoiChuyenDen.Name = "cboNoiChuyenDen";
			this.cboNoiChuyenDen.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboNoiChuyenDen.Properties.AutoComplete = false;
			this.cboNoiChuyenDen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance6, appearanceHovered6, appearancePressed6, appearanceDisabled6, "", null, null, true)
			});
			this.cboNoiChuyenDen.Properties.NullText = "";
			this.cboNoiChuyenDen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboNoiChuyenDen.Properties.View = this.gridView1;
			this.cboNoiChuyenDen.Size = new System.Drawing.Size(158, 20);
			this.cboNoiChuyenDen.StyleController = this.layoutControl1;
			this.cboNoiChuyenDen.TabIndex = 19;
			this.cboNoiChuyenDen.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboNoiChuyenDen_Closed);
			this.cboNoiChuyenDen.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboNoiChuyenDen_ButtonClick);
			this.cboNoiChuyenDen.KeyUp += new System.Windows.Forms.KeyEventHandler(cboNoiChuyenDen_KeyUp);
			this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
			this.gridView1.Name = "gridView1";
			this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
			this.gridView1.OptionsView.ShowColumnHeaders = false;
			this.gridView1.OptionsView.ShowGroupPanel = false;
			this.gridView1.OptionsView.ShowIndicator = false;
			this.txtMaNoiChuyenDen.Location = new System.Drawing.Point(267, 52);
			this.txtMaNoiChuyenDen.Name = "txtMaNoiChuyenDen";
			this.txtMaNoiChuyenDen.Properties.Appearance.Options.UseTextOptions = true;
			this.txtMaNoiChuyenDen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtMaNoiChuyenDen.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtMaNoiChuyenDen.Size = new System.Drawing.Size(63, 20);
			this.txtMaNoiChuyenDen.StyleController = this.layoutControl1;
			this.txtMaNoiChuyenDen.TabIndex = 18;
			this.txtMaNoiChuyenDen.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtMaNoiChuyenDen_PreviewKeyDown);
			this.cboHinhThucChuyen.EnterMoveNextControl = true;
			this.cboHinhThucChuyen.Location = new System.Drawing.Point(737, 76);
			this.cboHinhThucChuyen.Name = "cboHinhThucChuyen";
			this.cboHinhThucChuyen.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			this.cboHinhThucChuyen.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance7, appearanceHovered7, appearancePressed7, appearanceDisabled7, "", null, null, true)
			});
			this.cboHinhThucChuyen.Properties.NullText = "";
			this.cboHinhThucChuyen.Properties.PopupSizeable = false;
			this.cboHinhThucChuyen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			this.cboHinhThucChuyen.Size = new System.Drawing.Size(205, 20);
			this.cboHinhThucChuyen.StyleController = this.layoutControl1;
			this.cboHinhThucChuyen.TabIndex = 28;
			this.cboHinhThucChuyen.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboHinhThucChuyen_Closed);
			this.cboHinhThucChuyen.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboHinhThucChuyen_ButtonClick);
			this.cboHinhThucChuyen.EditValueChanged += new System.EventHandler(cboHinhThucChuyen_EditValueChanged);
			this.cboHinhThucChuyen.KeyUp += new System.Windows.Forms.KeyEventHandler(cboHinhThucChuyen_KeyUp);
			this.txtMaHinhThucChuyen.Location = new System.Drawing.Point(659, 76);
			this.txtMaHinhThucChuyen.Name = "txtMaHinhThucChuyen";
			this.txtMaHinhThucChuyen.Properties.Appearance.Options.UseTextOptions = true;
			this.txtMaHinhThucChuyen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.txtMaHinhThucChuyen.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtMaHinhThucChuyen.Size = new System.Drawing.Size(78, 20);
			this.txtMaHinhThucChuyen.StyleController = this.layoutControl1;
			this.txtMaHinhThucChuyen.TabIndex = 27;
			this.txtMaHinhThucChuyen.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtMaHinhThucChuyen_PreviewKeyDown);
			this.txtAddress.Location = new System.Drawing.Point(1201, 2);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(117, 20);
			this.txtAddress.StyleController = this.layoutControl1;
			this.txtAddress.TabIndex = 7;
			this.txtAddress.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(txtAddress_PreviewKeyDown);
			this.txtMucHuong.Enabled = false;
			this.txtMucHuong.Location = new System.Drawing.Point(1282, 28);
			this.txtMucHuong.Name = "txtMucHuong";
			this.txtMucHuong.Properties.ReadOnly = true;
			this.txtMucHuong.Size = new System.Drawing.Size(36, 20);
			this.txtMucHuong.StyleController = this.layoutControl1;
			this.txtMucHuong.TabIndex = 16;
			this.chkJoin5Year.Location = new System.Drawing.Point(997, 28);
			this.chkJoin5Year.Name = "chkJoin5Year";
			this.chkJoin5Year.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkJoin5Year.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkJoin5Year.Properties.Caption = "";
			this.chkJoin5Year.Properties.FullFocusRect = true;
			this.chkJoin5Year.Size = new System.Drawing.Size(30, 19);
			this.chkJoin5Year.StyleController = this.layoutControl1;
			toolTipItem2.Text = "Đóng bảo hiểm y tế đủ 5 năm liên tục";
			superToolTip2.Items.Add(toolTipItem2);
			this.chkJoin5Year.SuperTip = superToolTip2;
			this.chkJoin5Year.TabIndex = 14;
			this.chkJoin5Year.CheckedChanged += new System.EventHandler(chkJoin5Year_CheckedChanged);
			this.chkJoin5Year.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkJoin5Year_PreviewKeyDown);
			this.chkMediRecordNoRouteTransfer.Location = new System.Drawing.Point(251, 76);
			this.chkMediRecordNoRouteTransfer.Name = "chkMediRecordNoRouteTransfer";
			this.chkMediRecordNoRouteTransfer.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkMediRecordNoRouteTransfer.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkMediRecordNoRouteTransfer.Properties.AutoHeight = false;
			this.chkMediRecordNoRouteTransfer.Properties.Caption = "";
			this.chkMediRecordNoRouteTransfer.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
			this.chkMediRecordNoRouteTransfer.Properties.FullFocusRect = true;
			this.chkMediRecordNoRouteTransfer.Size = new System.Drawing.Size(19, 20);
			this.chkMediRecordNoRouteTransfer.StyleController = this.layoutControl1;
			toolTipItem3.Text = "Chuyển vượt tuyến CMKT gồm các trường hợp chuyển người bệnh không theo đúng quy định tại các khoản 1, 2, 3, 4  Điều 5 Thông tư";
			superToolTip3.Items.Add(toolTipItem3);
			this.chkMediRecordNoRouteTransfer.SuperTip = superToolTip3;
			this.chkMediRecordNoRouteTransfer.TabIndex = 26;
			this.chkMediRecordNoRouteTransfer.CheckedChanged += new System.EventHandler(chkMediRecordNoRouteTransfer_CheckedChanged);
			this.chkMediRecordNoRouteTransfer.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkMediRecordNoRouteTransfer_PreviewKeyDown);
			this.panel3.Controls.Add(this.dtHeinCardFromTime);
			this.panel3.Controls.Add(this.txtHeinCardFromTime);
			this.panel3.Location = new System.Drawing.Point(523, 2);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(111, 20);
			this.panel3.TabIndex = 3;
			this.panel2.Controls.Add(this.txtHeinCardToTime);
			this.panel2.Controls.Add(this.dtHeinCardToTime);
			this.panel2.Location = new System.Drawing.Point(703, 2);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(111, 20);
			this.panel2.TabIndex = 4;
			this.chkMediRecordRouteTransfer.Location = new System.Drawing.Point(87, 76);
			this.chkMediRecordRouteTransfer.Name = "chkMediRecordRouteTransfer";
			this.chkMediRecordRouteTransfer.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkMediRecordRouteTransfer.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkMediRecordRouteTransfer.Properties.AutoHeight = false;
			this.chkMediRecordRouteTransfer.Properties.Caption = "";
			this.chkMediRecordRouteTransfer.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
			this.chkMediRecordRouteTransfer.Properties.FullFocusRect = true;
			this.chkMediRecordRouteTransfer.Size = new System.Drawing.Size(95, 20);
			this.chkMediRecordRouteTransfer.StyleController = this.layoutControl1;
			toolTipItem4.Text = "Chuyển đúng tuyến CMKT gồm các trường hợp chuyển người bệnh theo đúng quy định tại các khoản 1, 2, 3, 4  Điều 5 Thông tư";
			superToolTip4.Items.Add(toolTipItem4);
			this.chkMediRecordRouteTransfer.SuperTip = superToolTip4;
			this.chkMediRecordRouteTransfer.TabIndex = 25;
			this.chkMediRecordRouteTransfer.CheckedChanged += new System.EventHandler(chkMediRecordRouteTransfer_CheckedChanged);
			this.chkMediRecordRouteTransfer.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkMediRecordRouteTransfer_PreviewKeyDown);
			this.panel1.Controls.Add(this.txtSoThe);
			this.panel1.Controls.Add(this.cboSoThe);
			this.panel1.Location = new System.Drawing.Point(193, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(233, 22);
			this.panel1.TabIndex = 2;
			this.txtSoThe.CausesValidation = false;
			this.txtSoThe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtSoThe.EditValue = "";
			this.txtSoThe.Location = new System.Drawing.Point(0, 0);
			this.txtSoThe.Name = "txtSoThe";
			this.txtSoThe.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
			this.txtSoThe.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
			this.txtSoThe.Properties.Appearance.Options.UseFont = true;
			this.txtSoThe.Properties.Appearance.Options.UseTextOptions = true;
			this.txtSoThe.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			this.txtSoThe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.DropDown, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F | System.Windows.Forms.Keys.Control), appearance8, appearanceHovered8, appearancePressed8, appearanceDisabled8, "", null, null, true),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance9, appearanceHovered9, appearancePressed9, appearanceDisabled9, "Nhập số thẻ mới", null, null, true)
			});
			this.txtSoThe.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.txtSoThe.Properties.Mask.EditMask = "(\\w{2}-\\d{1}-\\w{2}-\\w{2}-\\w{3}-\\w{5})|(\\w{2}-\\d{1}-\\w{2}-\\w{12})";
			this.txtSoThe.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
			this.txtSoThe.Properties.Mask.UseMaskAsDisplayFormat = true;
			this.txtSoThe.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(txtSoThe_Properties_ButtonClick);
			this.txtSoThe.Size = new System.Drawing.Size(233, 20);
			toolTipItem5.Text = "Nhấn tổ hợp phím Ctrl + F để mở danh sách thẻ của bệnh  nhân";
			superToolTip5.Items.Add(toolTipItem5);
			this.txtSoThe.SuperTip = superToolTip5;
			this.txtSoThe.TabIndex = 2;
			this.txtSoThe.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(txtSoThe_InvalidValue);
			this.txtSoThe.EditValueChanged += new System.EventHandler(txtSoThe_EditValueChanged);
			this.txtSoThe.KeyDown += new System.Windows.Forms.KeyEventHandler(txtSoThe_KeyDown);
			this.cboSoThe.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboSoThe.Location = new System.Drawing.Point(0, 0);
			this.cboSoThe.Name = "cboSoThe";
			this.cboSoThe.Properties.AutoHeight = false;
			this.cboSoThe.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[2]
			{
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
				new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), appearance10, appearanceHovered10, appearancePressed10, appearanceDisabled10, "Nhập số thẻ mới", null, null, true)
			});
			this.cboSoThe.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.cboSoThe.Properties.MaxLength = 17;
			this.cboSoThe.Properties.NullText = "";
			this.cboSoThe.Properties.PopupSizeable = false;
			this.cboSoThe.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
			this.cboSoThe.Size = new System.Drawing.Size(233, 22);
			this.cboSoThe.TabIndex = 2;
			this.cboSoThe.Visible = false;
			this.cboSoThe.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(cboSoThe_Closed);
			this.cboSoThe.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(cboSoThe_ButtonClick);
			this.cboSoThe.KeyUp += new System.Windows.Forms.KeyEventHandler(cboSoThe_KeyUp);
			this.chkHasDobCertificate.Location = new System.Drawing.Point(37, 2);
			this.chkHasDobCertificate.Name = "chkHasDobCertificate";
			this.chkHasDobCertificate.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Silver;
			this.chkHasDobCertificate.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.chkHasDobCertificate.Properties.Caption = "";
			this.chkHasDobCertificate.Properties.FullFocusRect = true;
			this.chkHasDobCertificate.Size = new System.Drawing.Size(19, 19);
			this.chkHasDobCertificate.StyleController = this.layoutControl1;
			toolTipItem6.Text = "Tự tạo thẻ BHYT cho trường hợp trẻ em dưới 6 tuổi có giấy khai sinh";
			superToolTip6.Items.Add(toolTipItem6);
			this.chkHasDobCertificate.SuperTip = superToolTip6;
			this.chkHasDobCertificate.TabIndex = 1;
			this.chkHasDobCertificate.CheckedChanged += new System.EventHandler(chkHasDobCertificate_CheckedChanged);
			this.chkHasDobCertificate.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(chkHasDobCertificate_PreviewKeyDown);
			this.layoutControlItem8.Location = new System.Drawing.Point(166, 0);
			this.layoutControlItem8.Name = "layoutControlItem8";
			this.layoutControlItem8.Size = new System.Drawing.Size(217, 35);
			this.layoutControlItem8.TextSize = new System.Drawing.Size(50, 20);
			this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
			this.layoutControlGroup1.GroupBordersVisible = false;
			this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[42]
			{
				this.lblHeincardNumber, this.lblHeincardToDate, this.lblHeincardFromDate, this.lblCaptionAddress, this.lblMediRecordMediOrgForm, this.lciNoiChuyenDenName, this.lciDKKCBBDName, this.lblRightRouteType, this.lciRightRouteTypeName, this.lciTransPatiReasonCode,
				this.lciTransPatiReasoncbo, this.lblMediRecordBenefitSymbol, this.lciIcdMain, this.panelICD, this.lblEditIcd, this.lciMediRecordRouteTransfer, this.lciMediRecordNoRouteTransfer, this.lciTransPatiFormCbo, this.lciTransPatiFormCode, this.lcirdoWrongRoute,
				this.lcirdoRightRoute, this.lciKhongKTHSD, this.lciFreeCoPainTime, this.lciCoPaidAccumulate, this.lciInCode, this.lciHNCode, this.lcichkJoin5Year, this.lcichkPaid6Month, this.lblMediRecordLiveArea, this.lciDu5Nam,
				this.lciFordtTransferInTimeFrom, this.lciFordtTransferInTimeTo, this.layoutControlItem1, this.layoutControlItem2, this.layoutControlItem3, this.layoutControlItem4, this.layoutControlItem5, this.layoutControlItem6, this.layoutControlItem7, this.lblHeincardMediOrg,
				this.lciTempQN, this.lblCaptionHasDobCertificate
			});
			this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
			this.layoutControlGroup1.Name = "Root";
			this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
			this.layoutControlGroup1.Size = new System.Drawing.Size(1320, 127);
			this.layoutControlGroup1.TextVisible = false;
			this.lblHeincardNumber.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.lblHeincardNumber.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblHeincardNumber.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblHeincardNumber.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblHeincardNumber.Control = this.panel1;
			this.lblHeincardNumber.Location = new System.Drawing.Point(136, 0);
			this.lblHeincardNumber.Name = "lblHeincardNumber";
			this.lblHeincardNumber.Size = new System.Drawing.Size(292, 26);
			this.lblHeincardNumber.Text = "Số thẻ:";
			this.lblHeincardNumber.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblHeincardNumber.TextSize = new System.Drawing.Size(50, 20);
			this.lblHeincardNumber.TextToControlDistance = 5;
			this.lblHeincardToDate.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.lblHeincardToDate.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblHeincardToDate.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblHeincardToDate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblHeincardToDate.Control = this.panel2;
			this.lblHeincardToDate.Location = new System.Drawing.Point(636, 0);
			this.lblHeincardToDate.MaxSize = new System.Drawing.Size(180, 24);
			this.lblHeincardToDate.MinSize = new System.Drawing.Size(180, 24);
			this.lblHeincardToDate.Name = "lblHeincardToDate";
			this.lblHeincardToDate.Size = new System.Drawing.Size(180, 26);
			this.lblHeincardToDate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lblHeincardToDate.Text = "Đến:";
			this.lblHeincardToDate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblHeincardToDate.TextSize = new System.Drawing.Size(60, 20);
			this.lblHeincardToDate.TextToControlDistance = 5;
			this.lblHeincardFromDate.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.lblHeincardFromDate.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblHeincardFromDate.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblHeincardFromDate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblHeincardFromDate.Control = this.panel3;
			this.lblHeincardFromDate.Location = new System.Drawing.Point(456, 0);
			this.lblHeincardFromDate.MaxSize = new System.Drawing.Size(180, 24);
			this.lblHeincardFromDate.MinSize = new System.Drawing.Size(180, 24);
			this.lblHeincardFromDate.Name = "lblHeincardFromDate";
			this.lblHeincardFromDate.Size = new System.Drawing.Size(180, 26);
			this.lblHeincardFromDate.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lblHeincardFromDate.Text = "Hạn từ:";
			this.lblHeincardFromDate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblHeincardFromDate.TextSize = new System.Drawing.Size(60, 20);
			this.lblHeincardFromDate.TextToControlDistance = 5;
			this.lblCaptionAddress.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.lblCaptionAddress.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblCaptionAddress.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblCaptionAddress.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblCaptionAddress.Control = this.txtAddress;
			this.lblCaptionAddress.Location = new System.Drawing.Point(1114, 0);
			this.lblCaptionAddress.Name = "lblCaptionAddress";
			this.lblCaptionAddress.Size = new System.Drawing.Size(206, 26);
			this.lblCaptionAddress.Text = "Địa chỉ thẻ:";
			this.lblCaptionAddress.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblCaptionAddress.TextSize = new System.Drawing.Size(80, 20);
			this.lblCaptionAddress.TextToControlDistance = 5;
			this.lblMediRecordMediOrgForm.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblMediRecordMediOrgForm.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblMediRecordMediOrgForm.Control = this.txtMaNoiChuyenDen;
			this.lblMediRecordMediOrgForm.Location = new System.Drawing.Point(160, 50);
			this.lblMediRecordMediOrgForm.Name = "lblMediRecordMediOrgForm";
			this.lblMediRecordMediOrgForm.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lblMediRecordMediOrgForm.Size = new System.Drawing.Size(170, 24);
			this.lblMediRecordMediOrgForm.Text = "Nơi chuyển đến:";
			this.lblMediRecordMediOrgForm.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblMediRecordMediOrgForm.TextSize = new System.Drawing.Size(100, 20);
			this.lblMediRecordMediOrgForm.TextToControlDistance = 5;
			this.lciNoiChuyenDenName.Control = this.cboNoiChuyenDen;
			this.lciNoiChuyenDenName.Location = new System.Drawing.Point(330, 50);
			this.lciNoiChuyenDenName.Name = "lciNoiChuyenDenName";
			this.lciNoiChuyenDenName.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciNoiChuyenDenName.Size = new System.Drawing.Size(160, 24);
			this.lciNoiChuyenDenName.TextSize = new System.Drawing.Size(0, 0);
			this.lciNoiChuyenDenName.TextVisible = false;
			this.lciDKKCBBDName.Control = this.cboDKKCBBD;
			this.lciDKKCBBDName.Location = new System.Drawing.Point(151, 26);
			this.lciDKKCBBDName.Name = "lciDKKCBBDName";
			this.lciDKKCBBDName.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciDKKCBBDName.Size = new System.Drawing.Size(228, 24);
			this.lciDKKCBBDName.TextSize = new System.Drawing.Size(0, 0);
			this.lciDKKCBBDName.TextVisible = false;
			this.lblRightRouteType.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblRightRouteType.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblRightRouteType.Control = this.txtHeinRightRouteCode;
			this.lblRightRouteType.Location = new System.Drawing.Point(562, 26);
			this.lblRightRouteType.MaxSize = new System.Drawing.Size(0, 24);
			this.lblRightRouteType.MinSize = new System.Drawing.Size(110, 24);
			this.lblRightRouteType.Name = "lblRightRouteType";
			this.lblRightRouteType.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lblRightRouteType.Size = new System.Drawing.Size(125, 24);
			this.lblRightRouteType.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lblRightRouteType.Text = "Trường hợp:";
			this.lblRightRouteType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblRightRouteType.TextSize = new System.Drawing.Size(80, 20);
			this.lblRightRouteType.TextToControlDistance = 5;
			this.lciRightRouteTypeName.Control = this.cboHeinRightRoute;
			this.lciRightRouteTypeName.Location = new System.Drawing.Point(687, 26);
			this.lciRightRouteTypeName.Name = "lciRightRouteTypeName";
			this.lciRightRouteTypeName.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciRightRouteTypeName.Size = new System.Drawing.Size(100, 24);
			this.lciRightRouteTypeName.TextSize = new System.Drawing.Size(0, 0);
			this.lciRightRouteTypeName.TextVisible = false;
			this.lciTransPatiReasonCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciTransPatiReasonCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciTransPatiReasonCode.Control = this.txtMaLyDoChuyen;
			this.lciTransPatiReasonCode.Location = new System.Drawing.Point(944, 74);
			this.lciTransPatiReasonCode.MaxSize = new System.Drawing.Size(0, 24);
			this.lciTransPatiReasonCode.MinSize = new System.Drawing.Size(110, 24);
			this.lciTransPatiReasonCode.Name = "lblMediRecordTransPatiReason";
			this.lciTransPatiReasonCode.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lciTransPatiReasonCode.Size = new System.Drawing.Size(125, 24);
			this.lciTransPatiReasonCode.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciTransPatiReasonCode.Text = "Lý do chuyển:";
			this.lciTransPatiReasonCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciTransPatiReasonCode.TextSize = new System.Drawing.Size(90, 20);
			this.lciTransPatiReasonCode.TextToControlDistance = 5;
			this.lciTransPatiReasoncbo.Control = this.cboLyDoChuyen;
			this.lciTransPatiReasoncbo.Location = new System.Drawing.Point(1069, 74);
			this.lciTransPatiReasoncbo.Name = "layoutControlItem9";
			this.lciTransPatiReasoncbo.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciTransPatiReasoncbo.Size = new System.Drawing.Size(251, 24);
			this.lciTransPatiReasoncbo.TextSize = new System.Drawing.Size(0, 0);
			this.lciTransPatiReasoncbo.TextVisible = false;
			this.lblMediRecordBenefitSymbol.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblMediRecordBenefitSymbol.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblMediRecordBenefitSymbol.Control = this.txtMucHuong;
			this.lblMediRecordBenefitSymbol.Location = new System.Drawing.Point(1280, 26);
			this.lblMediRecordBenefitSymbol.MaxSize = new System.Drawing.Size(0, 24);
			this.lblMediRecordBenefitSymbol.MinSize = new System.Drawing.Size(40, 24);
			this.lblMediRecordBenefitSymbol.Name = "lblMediRecordBenefitSymbol";
			this.lblMediRecordBenefitSymbol.Size = new System.Drawing.Size(40, 24);
			this.lblMediRecordBenefitSymbol.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lblMediRecordBenefitSymbol.Text = "Mức hưởng:";
			this.lblMediRecordBenefitSymbol.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblMediRecordBenefitSymbol.TextSize = new System.Drawing.Size(0, 0);
			this.lblMediRecordBenefitSymbol.TextToControlDistance = 0;
			this.lblMediRecordBenefitSymbol.TextVisible = false;
			this.lciIcdMain.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciIcdMain.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciIcdMain.Control = this.txtMaChanDoanTD;
			this.lciIcdMain.Location = new System.Drawing.Point(490, 50);
			this.lciIcdMain.Name = "lciIcdMain";
			this.lciIcdMain.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lciIcdMain.Size = new System.Drawing.Size(162, 24);
			this.lciIcdMain.Text = "Bệnh chính:";
			this.lciIcdMain.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciIcdMain.TextSize = new System.Drawing.Size(105, 20);
			this.lciIcdMain.TextToControlDistance = 5;
			this.panelICD.Control = this.panel5;
			this.panelICD.Location = new System.Drawing.Point(652, 50);
			this.panelICD.Name = "panelICD";
			this.panelICD.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.panelICD.Size = new System.Drawing.Size(170, 24);
			this.panelICD.TextSize = new System.Drawing.Size(0, 0);
			this.panelICD.TextVisible = false;
			this.lblEditIcd.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblEditIcd.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblEditIcd.Control = this.chkHasDialogText;
			this.lblEditIcd.Location = new System.Drawing.Point(822, 50);
			this.lblEditIcd.MaxSize = new System.Drawing.Size(0, 23);
			this.lblEditIcd.MinSize = new System.Drawing.Size(58, 23);
			this.lblEditIcd.Name = "lblEditIcd";
			this.lblEditIcd.Size = new System.Drawing.Size(58, 24);
			this.lblEditIcd.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lblEditIcd.Text = "Sửa:";
			this.lblEditIcd.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblEditIcd.TextSize = new System.Drawing.Size(30, 0);
			this.lblEditIcd.TextToControlDistance = 5;
			this.lciMediRecordRouteTransfer.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciMediRecordRouteTransfer.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciMediRecordRouteTransfer.Control = this.chkMediRecordRouteTransfer;
			this.lciMediRecordRouteTransfer.Location = new System.Drawing.Point(0, 74);
			this.lciMediRecordRouteTransfer.Name = "lciMediRecordRouteTransfer";
			this.lciMediRecordRouteTransfer.Size = new System.Drawing.Size(184, 24);
			this.lciMediRecordRouteTransfer.Text = "CĐ tuyến:";
			this.lciMediRecordRouteTransfer.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciMediRecordRouteTransfer.TextSize = new System.Drawing.Size(80, 20);
			this.lciMediRecordRouteTransfer.TextToControlDistance = 5;
			this.lciMediRecordNoRouteTransfer.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciMediRecordNoRouteTransfer.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciMediRecordNoRouteTransfer.Control = this.chkMediRecordNoRouteTransfer;
			this.lciMediRecordNoRouteTransfer.Location = new System.Drawing.Point(184, 74);
			this.lciMediRecordNoRouteTransfer.Name = "lciMediRecordNoRouteTransfer";
			this.lciMediRecordNoRouteTransfer.Size = new System.Drawing.Size(88, 24);
			this.lciMediRecordNoRouteTransfer.Text = "CV tuyến:";
			this.lciMediRecordNoRouteTransfer.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciMediRecordNoRouteTransfer.TextSize = new System.Drawing.Size(60, 20);
			this.lciMediRecordNoRouteTransfer.TextToControlDistance = 5;
			this.lciTransPatiFormCbo.Control = this.cboHinhThucChuyen;
			this.lciTransPatiFormCbo.Location = new System.Drawing.Point(737, 74);
			this.lciTransPatiFormCbo.Name = "layoutControlItem2";
			this.lciTransPatiFormCbo.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 2, 2, 2);
			this.lciTransPatiFormCbo.Size = new System.Drawing.Size(207, 24);
			this.lciTransPatiFormCbo.TextSize = new System.Drawing.Size(0, 0);
			this.lciTransPatiFormCbo.TextVisible = false;
			this.lciTransPatiFormCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciTransPatiFormCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciTransPatiFormCode.Control = this.txtMaHinhThucChuyen;
			this.lciTransPatiFormCode.Location = new System.Drawing.Point(572, 74);
			this.lciTransPatiFormCode.MaxSize = new System.Drawing.Size(0, 24);
			this.lciTransPatiFormCode.MinSize = new System.Drawing.Size(110, 24);
			this.lciTransPatiFormCode.Name = "lblMediRecordTransPatiForm";
			this.lciTransPatiFormCode.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lciTransPatiFormCode.Size = new System.Drawing.Size(165, 24);
			this.lciTransPatiFormCode.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciTransPatiFormCode.Text = "HT chuyển:";
			this.lciTransPatiFormCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciTransPatiFormCode.TextSize = new System.Drawing.Size(80, 20);
			this.lciTransPatiFormCode.TextToControlDistance = 5;
			this.lcirdoWrongRoute.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lcirdoWrongRoute.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lcirdoWrongRoute.Control = this.rdoWrongRoute;
			this.lcirdoWrongRoute.Location = new System.Drawing.Point(379, 26);
			this.lcirdoWrongRoute.Name = "lcirdoWrongRoute";
			this.lcirdoWrongRoute.Size = new System.Drawing.Size(93, 24);
			this.lcirdoWrongRoute.Text = "Trái tuyến:";
			this.lcirdoWrongRoute.TextSize = new System.Drawing.Size(61, 13);
			this.lcirdoRightRoute.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lcirdoRightRoute.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lcirdoRightRoute.Control = this.rdoRightRoute;
			this.lcirdoRightRoute.Location = new System.Drawing.Point(472, 26);
			this.lcirdoRightRoute.Name = "lcirdoRightRoute";
			this.lcirdoRightRoute.Size = new System.Drawing.Size(90, 24);
			this.lcirdoRightRoute.Text = "Đúng tuyến:";
			this.lcirdoRightRoute.TextSize = new System.Drawing.Size(61, 13);
			this.lciKhongKTHSD.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciKhongKTHSD.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciKhongKTHSD.Control = this.checkKhongKTHSD;
			this.lciKhongKTHSD.Location = new System.Drawing.Point(816, 0);
			this.lciKhongKTHSD.Name = "lciKhongKTHSD";
			this.lciKhongKTHSD.Size = new System.Drawing.Size(103, 26);
			this.lciKhongKTHSD.Text = "Không KT HSD:";
			this.lciKhongKTHSD.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciKhongKTHSD.TextSize = new System.Drawing.Size(75, 20);
			this.lciKhongKTHSD.TextToControlDistance = 5;
			this.lciKhongKTHSD.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
			this.lciFreeCoPainTime.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciFreeCoPainTime.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciFreeCoPainTime.Control = this.panelControl1;
			this.lciFreeCoPainTime.Location = new System.Drawing.Point(1114, 26);
			this.lciFreeCoPainTime.MinSize = new System.Drawing.Size(150, 24);
			this.lciFreeCoPainTime.Name = "lciFreeCoPainTime";
			this.lciFreeCoPainTime.OptionsToolTip.ToolTip = "Thời điểm miễn đồng chi trả";
			this.lciFreeCoPainTime.Size = new System.Drawing.Size(166, 24);
			this.lciFreeCoPainTime.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciFreeCoPainTime.Text = "TDMC CT:";
			this.lciFreeCoPainTime.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciFreeCoPainTime.TextSize = new System.Drawing.Size(60, 20);
			this.lciFreeCoPainTime.TextToControlDistance = 5;
			this.lciCoPaidAccumulate.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciCoPaidAccumulate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciCoPaidAccumulate.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
			this.lciCoPaidAccumulate.Control = this.txtCoPaidAccumulate;
			this.lciCoPaidAccumulate.Location = new System.Drawing.Point(912, 98);
			this.lciCoPaidAccumulate.Name = "lciCoPaidAccumulate";
			this.lciCoPaidAccumulate.OptionsToolTip.ToolTip = "Số tiền cùng chi trả lũy kế";
			this.lciCoPaidAccumulate.Size = new System.Drawing.Size(408, 29);
			this.lciCoPaidAccumulate.Text = "Cùng chi trả lũy kế:";
			this.lciCoPaidAccumulate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciCoPaidAccumulate.TextSize = new System.Drawing.Size(115, 20);
			this.lciCoPaidAccumulate.TextToControlDistance = 5;
			this.lciInCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciInCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciInCode.Control = this.txtInCode;
			this.lciInCode.Enabled = false;
			this.lciInCode.Location = new System.Drawing.Point(880, 50);
			this.lciInCode.Name = "lciInCode";
			this.lciInCode.OptionsToolTip.ToolTip = "Số giấy chuyển viện hoặc số hẹn khám";
			this.lciInCode.Size = new System.Drawing.Size(149, 24);
			this.lciInCode.Text = "GCT/Giấy hẹn:";
			this.lciInCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciInCode.TextSize = new System.Drawing.Size(90, 20);
			this.lciInCode.TextToControlDistance = 5;
			this.lciHNCode.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciHNCode.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciHNCode.Control = this.txtHNCode;
			this.lciHNCode.Location = new System.Drawing.Point(1029, 50);
			this.lciHNCode.Name = "lciHNCode";
			this.lciHNCode.Size = new System.Drawing.Size(291, 24);
			this.lciHNCode.Text = "Mã hộ nghèo:";
			this.lciHNCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciHNCode.TextSize = new System.Drawing.Size(80, 20);
			this.lciHNCode.TextToControlDistance = 5;
			this.lcichkJoin5Year.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lcichkJoin5Year.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lcichkJoin5Year.Control = this.chkJoin5Year;
			this.lcichkJoin5Year.Location = new System.Drawing.Point(950, 26);
			this.lcichkJoin5Year.MaxSize = new System.Drawing.Size(0, 24);
			this.lcichkJoin5Year.MinSize = new System.Drawing.Size(78, 24);
			this.lcichkJoin5Year.Name = "lcichkJoin5Year";
			this.lcichkJoin5Year.Size = new System.Drawing.Size(79, 24);
			this.lcichkJoin5Year.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lcichkJoin5Year.Text = "5 năm:";
			this.lcichkJoin5Year.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lcichkJoin5Year.TextSize = new System.Drawing.Size(40, 20);
			this.lcichkJoin5Year.TextToControlDistance = 5;
			this.lcichkPaid6Month.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lcichkPaid6Month.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lcichkPaid6Month.Control = this.chkPaid6Month;
			this.lcichkPaid6Month.Location = new System.Drawing.Point(1029, 26);
			this.lcichkPaid6Month.MaxSize = new System.Drawing.Size(0, 24);
			this.lcichkPaid6Month.MinSize = new System.Drawing.Size(85, 24);
			this.lcichkPaid6Month.Name = "lcichkPaid6Month";
			this.lcichkPaid6Month.Size = new System.Drawing.Size(85, 24);
			this.lcichkPaid6Month.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lcichkPaid6Month.Text = "6 tháng:";
			this.lcichkPaid6Month.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lcichkPaid6Month.TextSize = new System.Drawing.Size(46, 20);
			this.lcichkPaid6Month.TextToControlDistance = 5;
			this.lblMediRecordLiveArea.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblMediRecordLiveArea.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblMediRecordLiveArea.Control = this.cboNoiSong;
			this.lblMediRecordLiveArea.Location = new System.Drawing.Point(0, 50);
			this.lblMediRecordLiveArea.Name = "lblMediRecordLiveArea";
			this.lblMediRecordLiveArea.Size = new System.Drawing.Size(160, 24);
			this.lblMediRecordLiveArea.Text = "Khu vực:";
			this.lblMediRecordLiveArea.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblMediRecordLiveArea.TextSize = new System.Drawing.Size(80, 20);
			this.lblMediRecordLiveArea.TextToControlDistance = 5;
			this.lciDu5Nam.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciDu5Nam.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciDu5Nam.Control = this.panel4;
			this.lciDu5Nam.Location = new System.Drawing.Point(919, 0);
			this.lciDu5Nam.MaxSize = new System.Drawing.Size(195, 24);
			this.lciDu5Nam.MinSize = new System.Drawing.Size(195, 24);
			this.lciDu5Nam.Name = "lciDu5Nam";
			this.lciDu5Nam.OptionsToolTip.ToolTip = "Thời điểm đủ 5 năm liên tục";
			this.lciDu5Nam.Size = new System.Drawing.Size(195, 26);
			this.lciDu5Nam.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciDu5Nam.Text = "TĐ đủ 5 năm:";
			this.lciDu5Nam.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciDu5Nam.TextSize = new System.Drawing.Size(85, 20);
			this.lciDu5Nam.TextToControlDistance = 5;
			this.lciFordtTransferInTimeFrom.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciFordtTransferInTimeFrom.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciFordtTransferInTimeFrom.Control = this.dtTransferInTimeFrom;
			this.lciFordtTransferInTimeFrom.Location = new System.Drawing.Point(272, 74);
			this.lciFordtTransferInTimeFrom.MaxSize = new System.Drawing.Size(0, 24);
			this.lciFordtTransferInTimeFrom.MinSize = new System.Drawing.Size(150, 24);
			this.lciFordtTransferInTimeFrom.Name = "lciFordtTransferInTimeFrom";
			this.lciFordtTransferInTimeFrom.OptionsToolTip.ToolTip = "Ngày điều trị tuyến dưới từ";
			this.lciFordtTransferInTimeFrom.Size = new System.Drawing.Size(150, 24);
			this.lciFordtTransferInTimeFrom.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciFordtTransferInTimeFrom.Text = "Từ:";
			this.lciFordtTransferInTimeFrom.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciFordtTransferInTimeFrom.TextSize = new System.Drawing.Size(30, 20);
			this.lciFordtTransferInTimeFrom.TextToControlDistance = 5;
			this.lciFordtTransferInTimeTo.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciFordtTransferInTimeTo.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciFordtTransferInTimeTo.Control = this.dtTransferInTimeTo;
			this.lciFordtTransferInTimeTo.Location = new System.Drawing.Point(422, 74);
			this.lciFordtTransferInTimeTo.MaxSize = new System.Drawing.Size(0, 24);
			this.lciFordtTransferInTimeTo.MinSize = new System.Drawing.Size(150, 24);
			this.lciFordtTransferInTimeTo.Name = "lciFordtTransferInTimeTo";
			this.lciFordtTransferInTimeTo.OptionsToolTip.ToolTip = "Ngày điều trị tuyến dưới đến";
			this.lciFordtTransferInTimeTo.Size = new System.Drawing.Size(150, 24);
			this.lciFordtTransferInTimeTo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.lciFordtTransferInTimeTo.Text = "Đến:";
			this.lciFordtTransferInTimeTo.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciFordtTransferInTimeTo.TextSize = new System.Drawing.Size(30, 20);
			this.lciFordtTransferInTimeTo.TextToControlDistance = 5;
			this.layoutControlItem1.Control = this.btnCheckInfoBHYT;
			this.layoutControlItem1.Location = new System.Drawing.Point(428, 0);
			this.layoutControlItem1.Name = "layoutControlItem1";
			this.layoutControlItem1.Size = new System.Drawing.Size(28, 26);
			this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem1.TextVisible = false;
			this.layoutControlItem2.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem2.Control = this.chkBaby;
			this.layoutControlItem2.Location = new System.Drawing.Point(900, 26);
			this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 23);
			this.layoutControlItem2.MinSize = new System.Drawing.Size(50, 23);
			this.layoutControlItem2.Name = "layoutControlItem2";
			this.layoutControlItem2.OptionsToolTip.ToolTip = "Sơ sinh";
			this.layoutControlItem2.Size = new System.Drawing.Size(50, 24);
			this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
			this.layoutControlItem2.Text = "SS:";
			this.layoutControlItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem2.TextSize = new System.Drawing.Size(25, 20);
			this.layoutControlItem2.TextToControlDistance = 5;
			this.layoutControlItem3.Control = this.chkHasWorkingLetter;
			this.layoutControlItem3.Location = new System.Drawing.Point(0, 98);
			this.layoutControlItem3.Name = "layoutControlItem3";
			this.layoutControlItem3.Size = new System.Drawing.Size(422, 29);
			this.layoutControlItem3.Text = " ";
			this.layoutControlItem3.TextSize = new System.Drawing.Size(61, 13);
			this.layoutControlItem4.Control = this.chkHasAbsentLetter;
			this.layoutControlItem4.Location = new System.Drawing.Point(422, 98);
			this.layoutControlItem4.Name = "layoutControlItem4";
			this.layoutControlItem4.Size = new System.Drawing.Size(178, 29);
			this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem4.TextVisible = false;
			this.layoutControlItem5.Control = this.txtTt46;
			this.layoutControlItem5.Location = new System.Drawing.Point(653, 98);
			this.layoutControlItem5.Name = "layoutControlItem5";
			this.layoutControlItem5.Size = new System.Drawing.Size(259, 29);
			this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem5.TextVisible = false;
			this.layoutControlItem6.Control = this.chkTt46;
			this.layoutControlItem6.Location = new System.Drawing.Point(600, 98);
			this.layoutControlItem6.Name = "layoutControlItem6";
			this.layoutControlItem6.Size = new System.Drawing.Size(53, 29);
			this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
			this.layoutControlItem6.TextVisible = false;
			this.layoutControlItem7.AppearanceItemCaption.Options.UseTextOptions = true;
			this.layoutControlItem7.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.layoutControlItem7.Control = this.cboPatientCode;
			this.layoutControlItem7.Location = new System.Drawing.Point(787, 26);
			this.layoutControlItem7.Name = "layoutControlItem7";
			this.layoutControlItem7.Size = new System.Drawing.Size(113, 24);
			this.layoutControlItem7.Text = "Mã:";
			this.layoutControlItem7.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.layoutControlItem7.TextSize = new System.Drawing.Size(25, 20);
			this.layoutControlItem7.TextToControlDistance = 5;
			this.lblHeincardMediOrg.AppearanceItemCaption.ForeColor = System.Drawing.Color.Maroon;
			this.lblHeincardMediOrg.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblHeincardMediOrg.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblHeincardMediOrg.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblHeincardMediOrg.Control = this.txtMaDKKCBBD;
			this.lblHeincardMediOrg.Location = new System.Drawing.Point(0, 26);
			this.lblHeincardMediOrg.Name = "lblHeincardMediOrg";
			this.lblHeincardMediOrg.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 0, 2, 2);
			this.lblHeincardMediOrg.Size = new System.Drawing.Size(151, 24);
			this.lblHeincardMediOrg.Text = "Nơi ĐKKCB BĐ:";
			this.lblHeincardMediOrg.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblHeincardMediOrg.TextSize = new System.Drawing.Size(80, 20);
			this.lblHeincardMediOrg.TextToControlDistance = 5;
			this.lciTempQN.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lciTempQN.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lciTempQN.Control = this.chkTempQN;
			this.lciTempQN.Location = new System.Drawing.Point(58, 0);
			this.lciTempQN.Name = "lciTempQN";
			this.lciTempQN.Size = new System.Drawing.Size(78, 26);
			this.lciTempQN.Text = "Thẻ tạm:";
			this.lciTempQN.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lciTempQN.TextSize = new System.Drawing.Size(50, 20);
			this.lciTempQN.TextToControlDistance = 5;
			this.lblCaptionHasDobCertificate.AppearanceItemCaption.ForeColor = System.Drawing.Color.FromArgb(0, 0, 0, 66);
			this.lblCaptionHasDobCertificate.AppearanceItemCaption.Options.UseForeColor = true;
			this.lblCaptionHasDobCertificate.AppearanceItemCaption.Options.UseTextOptions = true;
			this.lblCaptionHasDobCertificate.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
			this.lblCaptionHasDobCertificate.Control = this.chkHasDobCertificate;
			this.lblCaptionHasDobCertificate.Location = new System.Drawing.Point(0, 0);
			this.lblCaptionHasDobCertificate.Name = "lblCaptionHasDobCertificate";
			this.lblCaptionHasDobCertificate.OptionsToolTip.ToolTip = "Khai sinh (thẻ tạm sử dụng trong trường hợp chưa được cấp thẻ BHYT)";
			this.lblCaptionHasDobCertificate.Size = new System.Drawing.Size(58, 26);
			this.lblCaptionHasDobCertificate.Text = "KS:";
			this.lblCaptionHasDobCertificate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
			this.lblCaptionHasDobCertificate.TextSize = new System.Drawing.Size(30, 20);
			this.lblCaptionHasDobCertificate.TextToControlDistance = 5;
			this.emptySpaceItem2.AllowHotTrack = false;
			this.emptySpaceItem2.Location = new System.Drawing.Point(912, 98);
			this.emptySpaceItem2.Name = "emptySpaceItem2";
			this.emptySpaceItem2.Size = new System.Drawing.Size(408, 29);
			this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
			this.dxErrorProvider1.ContainerControl = this;
			this.dxValidationProvider1.ValidateHiddenControls = false;
			this.dxValidationProvider1.ValidationFailed += new DevExpress.XtraEditors.DXErrorProvider.ValidationFailedEventHandler(dxValidationProvider1_ValidationFailed);
			this.repositoryItemMemoEdit1.Appearance.Options.UseTextOptions = true;
			this.repositoryItemMemoEdit1.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
			this.repositoryItemMemoEdit1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
			this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			base.Controls.Add(this.layoutControl1);
			base.Name = "Template__HeinBHYT1";
			base.Size = new System.Drawing.Size(1320, 127);
			base.Load += new System.EventHandler(Template__HeinBHYT1_Load);
			((System.ComponentModel.ISupportInitialize)this.txtHeinCardToTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtHeinCardFromTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardToTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardToTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardFromTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtHeinCardFromTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControl1).EndInit();
			this.layoutControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.cboPatientCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkTt46.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtTt46.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasAbsentLetter.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasWorkingLetter.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkBaby.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.rdoWrongRoute.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeTo.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeTo.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeFrom.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtTransferInTimeFrom.Properties).EndInit();
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtDu5Nam.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtDu5Nam.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtDu5Nam.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboNoiSong.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkTempQN.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.panelControl1).EndInit();
			this.panelControl1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtFreeCoPainTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtFreeCoPainTime.Properties.CalendarTimeProperties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dtFreeCoPainTime.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtInCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtHNCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtCoPaidAccumulate.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.checkKhongKTHSD.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.rdoRightRoute.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkPaid6Month.Properties).EndInit();
			this.panel5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.cboChanDoanTD.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtDialogText.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaChanDoanTD.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasDialogText.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboLyDoChuyen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaLyDoChuyen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboHeinRightRoute.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtHeinRightRouteCode.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboDKKCBBD.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridLookUpEdit1View).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaDKKCBBD.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboNoiChuyenDen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.gridView1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaNoiChuyenDen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboHinhThucChuyen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMaHinhThucChuyen.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtAddress.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.txtMucHuong.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkJoin5Year.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkMediRecordNoRouteTransfer.Properties).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.chkMediRecordRouteTransfer.Properties).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.txtSoThe.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.cboSoThe.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.chkHasDobCertificate.Properties).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem8).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlGroup1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardNumber).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardToDate).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardFromDate).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblCaptionAddress).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordMediOrgForm).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciNoiChuyenDenName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciDKKCBBDName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblRightRouteType).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciRightRouteTypeName).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiReasonCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiReasoncbo).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordBenefitSymbol).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciIcdMain).EndInit();
			((System.ComponentModel.ISupportInitialize)this.panelICD).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblEditIcd).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciMediRecordRouteTransfer).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciMediRecordNoRouteTransfer).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiFormCbo).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciTransPatiFormCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lcirdoWrongRoute).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lcirdoRightRoute).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciKhongKTHSD).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciFreeCoPainTime).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciCoPaidAccumulate).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciInCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciHNCode).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lcichkJoin5Year).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lcichkPaid6Month).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblMediRecordLiveArea).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciDu5Nam).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciFordtTransferInTimeFrom).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciFordtTransferInTimeTo).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem3).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem4).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem5).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem6).EndInit();
			((System.ComponentModel.ISupportInitialize)this.layoutControlItem7).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblHeincardMediOrg).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lciTempQN).EndInit();
			((System.ComponentModel.ISupportInitialize)this.lblCaptionHasDobCertificate).EndInit();
			((System.ComponentModel.ISupportInitialize)this.emptySpaceItem2).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxErrorProvider1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.dxValidationProvider1).EndInit();
			((System.ComponentModel.ISupportInitialize)this.repositoryItemMemoEdit1).EndInit();
			base.ResumeLayout(false);
		}

		private void cboChanDoanTD_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboChanDoanTD.EditValue = null;
					cboChanDoanTD.Properties.Buttons[1].Visible = false;
					txtMaChanDoanTD.Text = "";
					txtMaChanDoanTD.ErrorText = "";
					txtDialogText.Text = "";
					chkHasDialogText.Checked = false;
					chkHasDialogText.Enabled = false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtDTMCChiTra_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (!HisConfigCFG.IsNotAutoCheck5Y6M && !isFillingHeinDataFromDb)
				{
					chkJoin5Year.Checked = (IsShowMessage = !string.IsNullOrEmpty(txtFreeCoPainTime.Text.Trim()));
					chkPaid6Month.Checked = false;
					if (!string.IsNullOrEmpty(txtFreeCoPainTime.Text.Trim()))
					{
						IsShowMessage = chkJoin5Year.Checked;
						DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtFreeCoPainTime.Text);
						chkPaid6Month.Checked = dateTime.HasValue && dateTime.Value != DateTime.MinValue && long.Parse(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dateTime).ToString().Substring(0, 8)) < long.Parse(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(DateTime.Now).ToString().Substring(0, 8));
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					cboNoiSong.Focus();
					cboNoiSong.SelectAll();
					if (cboNoiSong.EditValue == null)
					{
						cboNoiSong.ShowPopup();
					}
				}
				else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Next)
				{
					dtFreeCoPainTime.ShowPopup();
					dtFreeCoPainTime.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtFreeCoPainTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtFreeCoPainTime.Visible = false;
					dtFreeCoPainTime.Update();
					txtFreeCoPainTime.Text = dtFreeCoPainTime.DateTime.ToString("dd/MM/yyyy");
					cboNoiSong.Focus();
					cboNoiSong.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtFreeCoPainTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtFreeCoPainTime.Visible = false;
					dtFreeCoPainTime.Update();
					if (dtFreeCoPainTime.EditValue != null && dtFreeCoPainTime.DateTime != DateTime.MinValue)
					{
						txtFreeCoPainTime.Text = dtFreeCoPainTime.DateTime.ToString("dd/MM/yyyy");
					}
					else
					{
						txtFreeCoPainTime.Text = null;
					}
					cboNoiSong.Focus();
					cboNoiSong.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtFreeCoPainTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtFreeCoPainTime.EditValue = dateTime;
						dtFreeCoPainTime.Update();
					}
					dtFreeCoPainTime.Visible = true;
					dtFreeCoPainTime.ShowPopup();
					dtFreeCoPainTime.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '/')
				{
					e.Handled = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_Click(object sender, EventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(txtFreeCoPainTime.Text))
				{
					string value = "";
					if (txtFreeCoPainTime.Text.Contains("/"))
					{
						value = HeinUtils.DateToDateRaw(txtFreeCoPainTime.Text);
					}
					if (!string.IsNullOrEmpty(value))
					{
						txtFreeCoPainTime.Text = value;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				string text = txtFreeCoPainTime.Text.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					if (text.Length == 8)
					{
						text = text.Substring(0, 2) + "/" + text.Substring(2, 2) + "/" + text.Substring(4, 4);
					}
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						txtFreeCoPainTime.Text = text;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtFreeCoPainTime_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			try
			{
				AutoValidate = AutoValidate.EnableAllowFocusChange;
				e.ExceptionMode = ExceptionMode.NoAction;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiChuyenDen_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboNoiChuyenDen.Properties.Buttons[1].Visible = !string.IsNullOrEmpty((cboNoiChuyenDen.EditValue ?? "").ToString());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiChuyenDen_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboNoiChuyenDen.EditValue = null;
					cboNoiChuyenDen.Properties.Buttons[1].Visible = false;
					txtMaNoiChuyenDen.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLyDoChuyen_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboLyDoChuyen.EditValue = null;
					cboLyDoChuyen.Properties.Buttons[1].Visible = false;
					txtMaLyDoChuyen.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboLyDoChuyen_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboLyDoChuyen.Properties.Buttons[1].Visible = !string.IsNullOrEmpty((cboLyDoChuyen.EditValue ?? "").ToString());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaChuanDoanTD_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadChuanDoanTDCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboChanDoanTD_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(cboChanDoanTD.Text))
				{
					cboChanDoanTD.EditValue = null;
					txtMaChanDoanTD.Text = "";
					chkHasDialogText.Checked = false;
				}
				else
				{
					_TextIcdName = cboChanDoanTD.Text;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaChanDoanTD_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(txtMaChanDoanTD.Text))
				{
					List<HIS_ICD> list = ((DataStore.Icds != null && DataStore.Icds.Count > 0) ? DataStore.Icds.Where((HIS_ICD o) => o.ICD_CODE.ToUpper() == txtMaChanDoanTD.Text.ToUpper()).ToList() : null);
					if (list == null || list.Count == 0)
					{
						e.Cancel = true;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaChanDoanTD_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			try
			{
				if (!string.IsNullOrEmpty(txtMaChanDoanTD.Text))
				{
					List<HIS_ICD> list = ((DataStore.Icds != null && DataStore.Icds.Count > 0) ? DataStore.Icds.Where((HIS_ICD o) => o.ICD_CODE.ToUpper() == txtMaChanDoanTD.Text.ToUpper()).ToList() : null);
					if (list == null || list.Count == 0)
					{
						e.ErrorText = ResourceMessage.MaBenhChinhKhongHopLe;
					}
				}
				else
				{
					e.ErrorText = "";
				}
				AutoValidate = AutoValidate.EnableAllowFocusChange;
				e.ExceptionMode = ExceptionMode.DisplayError;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkHasDialogText_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkHasDialogText.Checked)
				{
					cboChanDoanTD.Visible = false;
					txtDialogText.Visible = true;
					if (IsObligatoryTranferMediOrg)
					{
						txtDialogText.Text = _TextIcdName;
					}
					else
					{
						txtDialogText.Text = cboChanDoanTD.Text;
					}
					txtDialogText.Focus();
					txtDialogText.SelectAll();
				}
				else
				{
					txtDialogText.Visible = false;
					cboChanDoanTD.Visible = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkHasDialogText_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					SendKeys.Send("{TAB}");
					SendKeys.Send("^a");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinCardFromTime_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			try
			{
				e.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				AutoValidate = AutoValidate.EnableAllowFocusChange;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinCardToTime_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			try
			{
				e.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.ThieuTruongDuLieuBatBuoc);
				AutoValidate = AutoValidate.EnableAllowFocusChange;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtInCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtHNCode.Focus();
					txtHNCode.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHNCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (lciMediRecordRouteTransfer.Visibility == LayoutVisibility.Always && chkMediRecordRouteTransfer.Enabled)
					{
						chkMediRecordRouteTransfer.Focus();
					}
					else
					{
						FocusMoveOut();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkHasDobCertificate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (chkHasDobCertificate.Checked)
					{
						FocusMoveOut();
						return;
					}
					txtSoThe.Focus();
					txtSoThe.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkHasDobCertificate_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				bool flag = !chkHasDobCertificate.Checked;
				if (flag)
				{
					lblHeincardToDate.AppearanceItemCaption.ForeColor = Color.Maroon;
					ValidHeinCardToTime();
				}
				else
				{
					lblHeincardToDate.AppearanceItemCaption.ForeColor = default(Color);
					dxValidationProvider1.SetValidationRule(txtHeinCardToTime, null);
				}
				MediOrgProcess.LoadDataToComboNoiDKKCBBD(cboDKKCBBD, DataStore.MediOrgs);
				if (_DelegateSetRelativeAddress != null)
				{
					_DelegateSetRelativeAddress(flag);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtSoThe_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			try
			{
				e.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapSoTheBHYTKhongHopLe);
				AutoValidate = AutoValidate.EnableAllowFocusChange;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void SetRegisterButtonsEnabled(bool enabled)
		{
			Form form = FindForm();
			if (form == null)
			{
				return;
			}
			Control control = form.Controls.Find("UCRegister", true).FirstOrDefault();
			if (control != null)
			{
				Control control2 = control.Controls.Find("btnSave", true).FirstOrDefault();
				Control control3 = control.Controls.Find("btnSaveAndPrint", true).FirstOrDefault();
				if (control2 != null)
				{
					control2.Enabled = enabled;
				}
				if (control3 != null)
				{
					control3.Enabled = enabled;
				}
			}
		}

		private void txtSoThe_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					string text = txtSoThe.Text;
					text = HeinUtils.TrimHeinCardNumber(text.Replace(" ", "").Replace("  ", "").ToUpper()
						.Trim());
					txtHeinCardFromTime.Focus();
					txtHeinCardFromTime.SelectAll();
					ProcessorString.InsertColor(ProcessorString.InsertFontStyle(txtSoThe.Text, FontStyle.Bold), Color.Green);
					LogSystem.Debug("txtSoThe_KeyDown => So the ban dau = " + txtSoThe.Text + ", so the sau khi da xu ly chuoi = " + text);
					if (true && new BhytHeinProcessor().IsValidHeinCardNumber(text) && !string.IsNullOrEmpty(text))
					{
						dxErrorProvider1.ClearErrors();
						List<HisPatientSDO> sDO = HisPatientGet.GetSDO(text);
						if (sDO != null && sDO.Count > 0)
						{
							LogSystem.Info("txtSoThe_KeyDown => Tim thay " + sDO.Count + " BN co So the = " + text + ".");
							if (sDO.Count > 1)
							{
								new frmPatientChoice(sDO, new FillDataPatientSDOToRegisterForm(FillDataAfterSelectOnePatient), DataStore.Genders).ShowDialog();
							}
							else
							{
								currentPatientSdo = sDO[0];
								FillDataAfterSelectOnePatient(sDO[0]);
								if (entity.IsInitFromCallPatientTypeAlter)
								{
									cboDKKCBBD_KeyUp(null, e);
								}
							}
							if (HisConfigs.Get<string>("MOS.TREATMENT.ALLOW_MANY_TREATMENT_OPENING_OPTION") == "6")
							{
								SetRegisterButtonsEnabled(true);
								CommonParam commonParam = new CommonParam();
								HisTreatmentFilter filter = new HisTreatmentFilter
								{
									PATIENT_ID = sDO[0].ID,
									IS_PAUSE = false,
									TDL_TREATMENT_TYPE_IDs = new List<long> { 1L, 2L, 4L }
								};
								List<HIS_TREATMENT> list = new BackendAdapter(commonParam).Get<List<HIS_TREATMENT>>("api/HisTreatment/Get", ApiConsumers.MosConsumer, filter, commonParam).ToList();
								LogSystem.Debug("treatmentList.Count" + list.Count);
								List<HIS_TREATMENT> list2 = list.Where((HIS_TREATMENT t) => t.TDL_TREATMENT_TYPE_ID == 2 || t.TDL_TREATMENT_TYPE_ID == 4 || (t.TDL_TREATMENT_TYPE_ID == 1 && t.IS_EMERGENCY != 1)).ToList();
								LogSystem.Debug("activeTreatments.Count" + list2.Count);
								if (list2 != null && list2.Count > 0)
								{
									List<string> values = list2.Select((HIS_TREATMENT t) => t.TREATMENT_CODE).ToList();
									string text2 = string.Join(", ", values);
									if (XtraMessageBox.Show("Tồn tại hồ sơ chưa được kết thúc điều trị (Hồ sơ đang mở: " + text2 + "). Bạn có muốn mở thêm hồ sơ mới hay không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
									{
										SetRegisterButtonsEnabled(false);
										return;
									}
								}
							}
						}
						else
						{
							oldeHeinCardNumber = null;
							CheckExamHistoryFromBHXHApi(text);
							LogSystem.Info("txtSoThe_KeyDown => khong tim thay Bn cu theo so the = " + text + ", listResult.Count = 0");
						}
						if (dlgProcessFillDataCareerUnder6AgeByHeinCardNumber != null)
						{
							HeinCardData heinCardData = new HeinCardData();
							heinCardData.HeinCardNumber = text;
							dlgProcessFillDataCareerUnder6AgeByHeinCardNumber(heinCardData, true);
						}
						if (actChangePatientDob != null)
						{
							actChangePatientDob();
						}
					}
					else
					{
						dxErrorProvider1.SetError(txtSoThe, His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapSoTheBHYTKhongHopLe));
						txtSoThe.Focus();
						txtSoThe.SelectAll();
					}
					e.Handled = true;
				}
				else if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Down || e.KeyCode == Keys.Next)
				{
					GetHeinCarNumberToCheckTT(txtSoThe.Text.Trim());
					cboSoThe.ShowPopup();
					cboSoThe.SelectAll();
					PopupProcess.SelectFirstRowPopup(cboSoThe);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void GetHeinCarNumberToCheckTT(string heinCardNumber)
		{
			try
			{
				heinCardNumber = heinCardNumber.Replace(" ", "").ToUpper().Trim();
				heinCardNumber = HeinUtils.TrimHeinCardNumber(heinCardNumber);
				CheckExamHistoryFromBHXHApi(heinCardNumber);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void UpdateControlEditorTime(ButtonEdit txtEditorTime, DateEdit dtEditorTime)
		{
			try
			{
				string text = "";
				text = ((txtEditorTime.Text.Length == 2 || txtEditorTime.Text.Length == 1) ? ("01/01/" + (DateTime.Now.Year - Parse.ToInt64(txtEditorTime.Text))) : ((txtEditorTime.Text.Length == 4) ? ("01/01/" + txtEditorTime.Text) : ((txtEditorTime.Text.Length != 8) ? txtEditorTime.Text : (txtEditorTime.Text.Substring(0, 2) + "/" + txtEditorTime.Text.Substring(2, 2) + "/" + txtEditorTime.Text.Substring(4, 4)))));
				dtEditorTime.EditValue = ConvertDateStringToSystemDate(text);
				dtEditorTime.Update();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private DateTime? ConvertDateStringToSystemDate(string date)
		{
			DateTime? result = null;
			try
			{
				if (!string.IsNullOrEmpty(date))
				{
					date = date.Replace(" ", "");
					if (date.Length == 4)
					{
						int year = short.Parse(date);
						return new DateTime(year, 1, 1);
					}
					int day = short.Parse(date.Substring(0, 2));
					int month = short.Parse(date.Substring(3, 2));
					int year2 = short.Parse(date.Substring(6, 4));
					return new DateTime(year2, month, day);
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		private void CheckExamHistoryFromBHXHApi(string heinCardNumber)
		{
			try
			{
				if (dlgcheckExamHistory != null && !string.IsNullOrEmpty(heinCardNumber))
				{
					UpdateControlEditorTime(txtHeinCardFromTime, dtHeinCardFromTime);
					UpdateControlEditorTime(txtHeinCardToTime, dtHeinCardToTime);
					HeinCardData heinCardData = new HeinCardData();
					heinCardData.HeinCardNumber = heinCardNumber;
					heinCardData.MediOrgCode = txtMaDKKCBBD.Text;
					heinCardData.Address = txtAddress.Text;
					if (dtHeinCardFromTime.EditValue != null && dtHeinCardFromTime.DateTime != DateTime.MinValue)
					{
						heinCardData.FromDate = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(dtHeinCardFromTime.DateTime);
					}
					if (dtHeinCardToTime.EditValue != null && dtHeinCardToTime.DateTime != DateTime.MinValue)
					{
						heinCardData.ToDate = Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(dtHeinCardToTime.DateTime);
					}
					if (ResultDataADO == null || ResultDataADO.ResultHistoryLDO == null || (!(ResultDataADO.ResultHistoryLDO.maKetQua == "001") && !(ResultDataADO.ResultHistoryLDO.maKetQua == "002") && !(ResultDataADO.ResultHistoryLDO.maKetQua == "050")) || !(oldeHeinCardNumber == heinCardData.HeinCardNumber))
					{
						oldeHeinCardNumber = heinCardNumber;
						dlgcheckExamHistory(heinCardData);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtSoThe_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Plus)
				{
					ResetEditorControl.Reset(cboSoThe);
					ResetEditorControl.Reset(txtSoThe);
					ResetEditorControl.Reset(dtHeinCardToTime);
					ResetEditorControl.Reset(txtHeinCardToTime);
					ResetEditorControl.Reset(dtHeinCardFromTime);
					ResetEditorControl.Reset(txtHeinCardFromTime);
					ResetEditorControl.Reset(txtMaDKKCBBD);
					ResetEditorControl.Reset(cboDKKCBBD);
					ResetEditorControl.Reset(txtHeinRightRouteCode);
					ResetEditorControl.Reset(cboHeinRightRoute);
					ResetEditorControl.Reset(txtMaChanDoanTD);
					ResetEditorControl.Reset(cboChanDoanTD);
					ResetEditorControl.Reset(txtMaNoiChuyenDen);
					ResetEditorControl.Reset(cboNoiChuyenDen);
					ResetEditorControl.Reset(txtMaHinhThucChuyen);
					ResetEditorControl.Reset(dtTransferInTimeFrom);
					ResetEditorControl.Reset(dtTransferInTimeTo);
					ResetEditorControl.Reset(cboHinhThucChuyen);
					ResetEditorControl.Reset(txtMaLyDoChuyen);
					ResetEditorControl.Reset(cboLyDoChuyen);
					ResetEditorControl.Reset(cboNoiSong);
					ResetEditorControl.Reset(txtMucHuong);
					ResetEditorControl.Reset(txtAddress);
					CheckEdit checkEdit = rdoWrongRoute;
					bool flag = (rdoRightRoute.Checked = false);
					checkEdit.Checked = flag;
					TextEdit textEdit = txtHeinRightRouteCode;
					flag = (cboHeinRightRoute.Enabled = false);
					textEdit.Enabled = flag;
					txtSoThe.Focus();
				}
				else if (e.Button.Kind == ButtonPredefines.DropDown)
				{
					ShowComboSoThe();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void ShowComboSoThe()
		{
			try
			{
				cboSoThe.Visible = true;
				ResetEditorControl.ResetAndFocus(cboSoThe, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtSoThe_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				string text = txtSoThe.Text;
				text = text.Replace(" ", "").ToUpper().Trim();
				oldeHeinCardNumber = HeinUtils.TrimHeinCardNumber(text);
				CheckHSDAndTECard();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboSoThe_Properties_GetNotInListValue(object sender, GetNotInListValueEventArgs e)
		{
			try
			{
				if (e.FieldName == "RENDERER_HEIN_CARD_NUMBER")
				{
					HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = ((List<HIS_PATIENT_TYPE_ALTER>)cboSoThe.Properties.DataSource)[e.RecordIndex];
					if (hIS_PATIENT_TYPE_ALTER != null)
					{
						string text = "";
						string text2 = "";
						string text3 = "";
						string text4 = "";
						string text5 = "";
						string text6 = "";
						try
						{
							text = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(0, 2);
							text2 = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(2, 1);
							text3 = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(3, 2);
							text4 = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(5, 2);
							text5 = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(7, 3);
							text6 = hIS_PATIENT_TYPE_ALTER.HEIN_CARD_NUMBER.Substring(10);
						}
						catch (Exception ex)
						{
							LogSystem.Warn("Gan chuoi RENDERER_HEIN_CARD_NUMBER the BHYT loi", ex);
						}
						e.Value = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", text, text2, text3, text4, text5, text6);
					}
				}
				else if (e.FieldName == "RENDERER_FROM_DATE_TODATE")
				{
					HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER2 = ((List<HIS_PATIENT_TYPE_ALTER>)cboSoThe.Properties.DataSource)[e.RecordIndex];
					if (hIS_PATIENT_TYPE_ALTER2 != null && hIS_PATIENT_TYPE_ALTER2.HEIN_CARD_FROM_TIME > 0 && hIS_PATIENT_TYPE_ALTER2.HEIN_CARD_TO_TIME > 0)
					{
						string text7 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(hIS_PATIENT_TYPE_ALTER2.HEIN_CARD_FROM_TIME.GetValueOrDefault());
						string text8 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(hIS_PATIENT_TYPE_ALTER2.HEIN_CARD_TO_TIME.GetValueOrDefault());
						e.Value = text7 + " - " + text8;
					}
				}
			}
			catch (Exception ex2)
			{
				LogSystem.Warn(ex2);
			}
		}

		private void cboSoThe_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboSoThe.EditValue != null)
				{
					patientTypeAlterOld = lstPatientTypeAlterMap.FirstOrDefault((PatientTypeAlterADO o) => HeinCardProcess.ProcessHeinCardNumber(o.HEIN_CARD_NUMBER) == cboSoThe.EditValue.ToString());
					HeinCardSelectRowHandler(patientTypeAlterOld);
					oldeHeinCardNumber = null;
					CheckExamHistoryFromBHXHApi(patientTypeAlterOld.HEIN_CARD_NUMBER);
					if (actChangePatientDob != null)
					{
						actChangePatientDob();
					}
				}
				else
				{
					txtHeinCardFromTime.Focus();
					txtHeinCardFromTime.SelectAll();
				}
				cboSoThe.Visible = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboSoThe_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (cboSoThe.EditValue != null)
				{
					patientTypeAlterOld = lstPatientTypeAlterMap.FirstOrDefault((PatientTypeAlterADO o) => HeinCardProcess.ProcessHeinCardNumber(o.HEIN_CARD_NUMBER) == cboSoThe.EditValue.ToString());
					HeinCardSelectRowHandler(patientTypeAlterOld);
					oldeHeinCardNumber = null;
					CheckExamHistoryFromBHXHApi(patientTypeAlterOld.HEIN_CARD_NUMBER);
					if (actChangePatientDob != null)
					{
						actChangePatientDob();
					}
				}
				cboSoThe.Visible = false;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtHeinCardFromTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtHeinCardFromTime.Visible = false;
					dtHeinCardFromTime.Update();
					txtHeinCardFromTime.Text = dtHeinCardFromTime.DateTime.ToString("dd/MM/yyyy");
					dtHeinCardToTime.Focus();
					dtHeinCardToTime.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtHeinCardFromTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtHeinCardFromTime.Visible = false;
					dtHeinCardFromTime.Update();
					txtHeinCardFromTime.Text = dtHeinCardFromTime.DateTime.ToString("dd/MM/yyyy");
					txtHeinCardToTime.Focus();
					txtHeinCardToTime.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtHeinCardFromTime_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dtHeinCardFromTime.EditValue == null || !(dtHeinCardFromTime.DateTime != DateTime.MinValue))
				{
					return;
				}
				if (dtHeinCardFromTime.DateTime.Date > DateTime.Now.Date)
				{
					XtraMessageBox.Show(His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.His_UCHein__TheBHYTChuaDenHanSuDung), His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), DefaultBoolean.True);
					txtHeinCardFromTime.Focus();
				}
				if (entity.IsInitFromCallPatientTypeAlter)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					if (patientTypeAlterOld != null)
					{
						bool flag = patientTypeAlterOld.HEIN_CARD_FROM_TIME == Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dateTime);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void dtHeinCardToTime_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtHeinCardToTime.Visible = false;
					dtHeinCardToTime.Update();
					if (dtHeinCardToTime.EditValue != null && dtHeinCardToTime.DateTime != DateTime.MinValue)
					{
						txtHeinCardToTime.Text = dtHeinCardToTime.DateTime.ToString("dd/MM/yyyy");
					}
					else
					{
						txtHeinCardToTime.Text = null;
					}
					txtDu5Nam.Focus();
					txtDu5Nam.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtHeinCardToTime_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtHeinCardToTime.Visible = false;
					dtHeinCardToTime.Update();
					txtHeinCardToTime.Text = dtHeinCardToTime.DateTime.ToString("dd/MM/yyyy");
					SendKeys.Send("{TAB}");
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtDu5Nam_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					dtDu5Nam.Visible = false;
					dtDu5Nam.Update();
					if (dtDu5Nam.EditValue != null && dtDu5Nam.DateTime != DateTime.MinValue)
					{
						txtDu5Nam.Text = dtDu5Nam.DateTime.ToString("dd/MM/yyyy");
					}
					else
					{
						txtDu5Nam.Text = null;
					}
					txtAddress.Focus();
					txtAddress.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtDu5Nam_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtDu5Nam.Visible = false;
					dtDu5Nam.Update();
					txtDu5Nam.Text = dtDu5Nam.DateTime.ToString("dd/MM/yyyy");
					txtAddress.Focus();
					txtAddress.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtDu5Nam_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtDu5Nam.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtDu5Nam.EditValue = dateTime;
						dtDu5Nam.Update();
					}
					dtDu5Nam.Visible = true;
					dtDu5Nam.Focus();
					dtDu5Nam.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtDu5Nam_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtDu5Nam.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtDu5Nam.EditValue = dateTime;
						dtDu5Nam.Update();
					}
					txtAddress.Focus();
					txtAddress.SelectAll();
				}
				else if (e.KeyCode == Keys.Down)
				{
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(txtDu5Nam.Text);
					if (dateTime2.HasValue && dateTime2.Value != DateTime.MinValue)
					{
						dtDu5Nam.EditValue = dateTime2;
						dtDu5Nam.Update();
					}
					dtDu5Nam.Visible = true;
					dtDu5Nam.ShowPopup();
					dtDu5Nam.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void dtHeinCardToTime_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dtHeinCardToTime.EditValue == null || !(dtHeinCardToTime.DateTime != DateTime.MinValue))
				{
					return;
				}
				if (isShowCheckKhongKTHSD == "1")
				{
					CheckHSDAndTECard();
				}
				else if (dtHeinCardToTime.DateTime.Date < DateTime.Now.Date)
				{
					XtraMessageBox.Show(His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.His_UCHein__TheBHYTDaHetHanSuDung), His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), DefaultBoolean.True);
					txtHeinCardToTime.Focus();
				}
				if (entity.IsInitFromCallPatientTypeAlter)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					if (patientTypeAlterOld != null)
					{
						bool flag = patientTypeAlterOld.HEIN_CARD_TO_TIME == Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(dateTime);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtHeinCardFromTime_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtHeinCardFromTime.EditValue = dateTime;
						dtHeinCardFromTime.Update();
					}
					dtHeinCardFromTime.Visible = true;
					dtHeinCardFromTime.ShowPopup();
					dtHeinCardFromTime.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinCardFromTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtHeinCardFromTime.EditValue = dateTime;
						dtHeinCardFromTime.Update();
					}
					string text = txtSoThe.Text;
					text = HeinUtils.TrimHeinCardNumber(text.Replace(" ", "").Replace("  ", "").ToUpper()
						.Trim());
					CheckExamHistoryFromBHXHApi(text);
					txtHeinCardToTime.Focus();
					txtHeinCardToTime.SelectAll();
				}
				else if (e.KeyCode == Keys.Down)
				{
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardFromTime.Text);
					if (dateTime2.HasValue && dateTime2.Value != DateTime.MinValue)
					{
						dtHeinCardFromTime.EditValue = dateTime2;
						dtHeinCardFromTime.Update();
					}
					dtHeinCardFromTime.Visible = true;
					dtHeinCardFromTime.ShowPopup();
					dtHeinCardFromTime.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinCardToTime_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Down)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtHeinCardToTime.EditValue = dateTime;
						dtHeinCardToTime.Update();
					}
					dtHeinCardToTime.Visible = true;
					dtHeinCardToTime.Focus();
					dtHeinCardToTime.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinCardToTime_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					DateTime? dateTime = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					if (dateTime.HasValue && dateTime.Value != DateTime.MinValue)
					{
						dtHeinCardToTime.EditValue = dateTime;
						dtHeinCardToTime.Update();
					}
					string text = txtSoThe.Text;
					text = HeinUtils.TrimHeinCardNumber(text.Replace(" ", "").Replace("  ", "").ToUpper()
						.Trim());
					CheckExamHistoryFromBHXHApi(text);
					txtDu5Nam.Focus();
					txtDu5Nam.SelectAll();
				}
				else if (e.KeyCode == Keys.Down)
				{
					DateTime? dateTime2 = HeinUtils.ConvertDateStringToSystemDate(txtHeinCardToTime.Text);
					if (dateTime2.HasValue && dateTime2.Value != DateTime.MinValue)
					{
						dtHeinCardToTime.EditValue = dateTime2;
						dtHeinCardToTime.Update();
					}
					dtHeinCardToTime.Visible = true;
					dtHeinCardToTime.ShowPopup();
					dtHeinCardToTime.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaDKKCBBD_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadNoiDKKCBBDCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDKKCBBD_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					if (cboDKKCBBD.EditValue != null)
					{
						MediOrgSelectRowChange(true, (cboNoiSong.EditValue ?? "").ToString());
						string text = txtSoThe.Text;
						text = text.Replace(" ", "").ToUpper().Trim();
						text = HeinUtils.TrimHeinCardNumber(text);
						CheckExamHistoryFromBHXHApi(text);
					}
					else
					{
						rdoWrongRoute.Focus();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboDKKCBBD_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (cboDKKCBBD.EditValue != null)
					{
						MediOrgSelectRowChange(true);
						string text = txtSoThe.Text;
						text = text.Replace(" ", "").ToUpper().Trim();
						text = HeinUtils.TrimHeinCardNumber(text);
						CheckExamHistoryFromBHXHApi(text);
					}
				}
				else
				{
					cboDKKCBBD.ShowPopup();
					PopupProcess.SelectFirstRowPopup(cboDKKCBBD);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rdoWrongRoute_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (rdoWrongRoute.Checked)
				{
					ChangeDefaultHeinRatio();
					rdoRightRoute.Checked = false;
					txtHeinRightRouteCode.Text = "";
					cboHeinRightRoute.EditValue = null;
					cboHeinRightRoute.Properties.Buttons[1].Visible = false;
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER, true);
					txtInCode.Enabled = false;
					firstCheck = false;
					ResetHeinPatientCodeForRecalc();
					InitComboPatientCode();
					SetSize();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void rdoWrongRoute_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					rdoRightRoute.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void rdoRightRoute_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (rdoRightRoute.Checked && !chkHasDobCertificate.Checked)
				{
					ResetHeinPatientCodeForRecalc();
					InitComboPatientCode();
					SetSize();
					firstCheck = false;
					ChangeDefaultHeinRatio();
					rdoWrongRoute.Checked = false;
					bool flag = "3" == HeinLevelCodeCurrent || "4" == HeinLevelCodeCurrent;
					SetEnableControlHein((!flag) ? RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT : RightRouterFactory.RIGHT_ROUTER, true);
					txtInCode.Enabled = false;
				}
				ValidateRightRouteType(false);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidateRightRouteType(bool LoadDefault = true)
		{
			try
			{
				V_HIS_TREATMENT_4 treatment = ((patientTypeAlterOld != null && patientTypeAlterOld.TREATMENT_ID > 0) ? HisTreatmentGet.GetById(patientTypeAlterOld.TREATMENT_ID) : ((entity.HisTreatment != null && entity.HisTreatment.ID > 0) ? entity.HisTreatment : null));
				if (LoadDefault)
				{
					SetDefaultRightCode();
				}
				if (!isCallByRegistor && rdoRightRoute.Checked && cboDKKCBBD.EditValue != null && (string)cboDKKCBBD.EditValue != BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == treatment.BRANCH_ID).HEIN_MEDI_ORG_CODE && (!IsNotRequiredRightTypeInCaseOfHavingAreaCode || cboNoiSong.EditValue == null) && !chkBaby.Checked && !chkHasAbsentLetter.Checked && !chkHasWorkingLetter.Checked && !chkTt46.Checked && !ValidAcceptHeinMediOrgCode((string)cboDKKCBBD.EditValue, BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == treatment.BRANCH_ID).ACCEPT_HEIN_MEDI_ORG_CODE) && !ValidSysMediOrgCode((string)cboDKKCBBD.EditValue, BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == treatment.BRANCH_ID).SYS_MEDI_ORG_CODE))
				{
					ValidRightRouteType();
					return;
				}
				lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Black;
				dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, null);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private bool SetDefaultRightCode()
		{
			bool result = false;
			try
			{
				V_HIS_TREATMENT_4 treatment = ((patientTypeAlterOld != null && patientTypeAlterOld.TREATMENT_ID > 0) ? HisTreatmentGet.GetById(patientTypeAlterOld.TREATMENT_ID) : ((entity.HisTreatment != null && entity.HisTreatment.ID > 0) ? entity.HisTreatment : null));
				if (ValidAcceptHeinMediOrgCode((string)cboDKKCBBD.EditValue, BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == treatment.BRANCH_ID).ACCEPT_HEIN_MEDI_ORG_CODE) || ValidSysMediOrgCode((string)cboDKKCBBD.EditValue, BackendDataWorker.Get<HIS_BRANCH>().FirstOrDefault((HIS_BRANCH o) => o.ID == treatment.BRANCH_ID).SYS_MEDI_ORG_CODE))
				{
					result = (rdoRightRoute.Checked = true);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
			return result;
		}

		private void rdoRightRoute_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (txtHeinRightRouteCode.Enabled)
					{
						txtHeinRightRouteCode.Focus();
						txtHeinRightRouteCode.SelectAll();
					}
					else
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtHeinRightRouteCode_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadHeinRightRouterTypeCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ChangecboHeinRightRoute()
		{
			try
			{
				HeinRightRouteTypeData data = HeinRightRouteTypeStore.GetByCode((cboHeinRightRoute.EditValue ?? "").ToString());
				LogSystem.Debug("ChangecboHeinRightRoute" + LogUtil.TraceData(LogUtil.GetMemberName(() => data), data));
				if (data != null && DataStore.HeinRightRouteTypes.Exists((HeinRightRouteTypeData o) => o.HeinRightRouteTypeCode == data.HeinRightRouteTypeCode))
				{
					cboHeinRightRoute.Properties.Buttons[1].Visible = true;
					txtHeinRightRouteCode.Text = data.HeinRightRouteTypeCode;
					if (data.HeinRightRouteTypeCode == "CC")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC, false);
					}
					else if (data.HeinRightRouteTypeCode == "HK")
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT, false);
					}
					else
					{
						SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT, false);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHeinRightRoute_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboHeinRightRoute.Properties.Buttons[1].Visible = false;
					cboHeinRightRoute.EditValue = null;
					txtHeinRightRouteCode.Text = "";
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT, true);
					ResetHeinPatientCodeForRecalc();
					InitComboPatientCode();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHeinRightRoute_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				if (dlgautoCheckCC != null)
				{
					if (cboHeinRightRoute.EditValue != null && cboHeinRightRoute.EditValue.ToString() == "CC")
					{
						dlgautoCheckCC(true);
					}
					else
					{
						dlgautoCheckCC(false);
					}
				}
				cboHeinRightRoute.Properties.Buttons[1].Visible = !string.IsNullOrEmpty((cboHeinRightRoute.EditValue ?? "").ToString());
				if (cboHeinRightRoute.EditValue != null)
				{
					ChangecboHeinRightRoute();
				}
				else
				{
					SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__DELETE_CHOICE_TYPE, false);
				}
				if (cboHeinRightRoute.EditValue != null && cboHeinRightRoute.EditValue.ToString() == "HK")
				{
					txtInCode.Enabled = true;
				}
				else
				{
					txtInCode.Enabled = false;
				}
				ResetValidationRightRoute_Present();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ResetValidationRightRoute_Present()
		{
			try
			{
				if (cboHeinRightRoute.EditValue == null || (cboHeinRightRoute.EditValue != null && cboHeinRightRoute.EditValue.ToString() != "GT"))
				{
					dxValidationProvider1.SetValidationRule(txtMaNoiChuyenDen, null);
					dxValidationProvider1.SetValidationRule(txtMaChanDoanTD, null);
					dxValidationProvider1.SetValidationRule(txtInCode, null);
					dxValidationProvider1.SetValidationRule(chkMediRecordRouteTransfer, null);
					dxValidationProvider1.SetValidationRule(chkMediRecordNoRouteTransfer, null);
					dxValidationProvider1.SetValidationRule(dtTransferInTimeFrom, null);
					dxValidationProvider1.SetValidationRule(dtTransferInTimeTo, null);
					dxValidationProvider1.SetValidationRule(txtMaHinhThucChuyen, null);
					dxValidationProvider1.SetValidationRule(txtMaLyDoChuyen, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHeinRightRoute_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
		}

		private void cboHeinRightRoute_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal)
				{
					if (cboHeinRightRoute.EditValue != null)
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
						cboNoiSong.ShowPopup();
					}
					ResetHeinPatientCodeForRecalc();
					InitComboPatientCode();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHeinRightRoute_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboHeinRightRoute.EditValue == null)
				{
					cboNoiSong.Focus();
					cboNoiSong.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaNoiChuyenDen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadNoiChuyenDenCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiChuyenDen_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboNoiChuyenDen.EditValue != null)
				{
					HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE.Equals(cboNoiChuyenDen.EditValue ?? ""));
					if (hIS_MEDI_ORG != null)
					{
						txtMaNoiChuyenDen.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
						cboNoiChuyenDen.Properties.Buttons[1].Visible = true;
						ProcessLevelOfMediOrg();
					}
				}
				txtMaChanDoanTD.Focus();
				txtMaChanDoanTD.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiChuyenDen_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboNoiChuyenDen.EditValue != null)
				{
					HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE.Equals(cboNoiChuyenDen.EditValue ?? ""));
					if (hIS_MEDI_ORG != null)
					{
						txtMaNoiChuyenDen.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
						cboNoiChuyenDen.Properties.Buttons[1].Visible = true;
						ProcessLevelOfMediOrg();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkMediRecordRouteTransfer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					chkMediRecordNoRouteTransfer.Focus();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkMediRecordRouteTransfer_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkMediRecordRouteTransfer.Checked && chkMediRecordNoRouteTransfer.Checked)
				{
					chkMediRecordNoRouteTransfer.Checked = !chkMediRecordRouteTransfer.Checked;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkMediRecordNoRouteTransfer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					dtTransferInTimeFrom.Focus();
					dtTransferInTimeFrom.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkMediRecordNoRouteTransfer_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkMediRecordRouteTransfer.Checked && chkMediRecordNoRouteTransfer.Checked)
				{
					chkMediRecordRouteTransfer.Checked = !chkMediRecordNoRouteTransfer.Checked;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtMaHinhThucChuyen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadTranPatiFormCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHinhThucChuyen_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboHinhThucChuyen.EditValue != null)
				{
					HIS_TRAN_PATI_FORM hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.SingleOrDefault((HIS_TRAN_PATI_FORM o) => o.ID == Parse.ToInt64((cboHinhThucChuyen.EditValue ?? ((object)0)).ToString()));
					if (hIS_TRAN_PATI_FORM != null)
					{
						txtMaHinhThucChuyen.Text = hIS_TRAN_PATI_FORM.TRAN_PATI_FORM_CODE;
						cboHinhThucChuyen.Properties.Buttons[1].Visible = true;
					}
				}
				txtMaLyDoChuyen.Focus();
				txtMaLyDoChuyen.SelectAll();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHinhThucChuyen_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboHinhThucChuyen.EditValue != null)
				{
					HIS_TRAN_PATI_FORM hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.SingleOrDefault((HIS_TRAN_PATI_FORM o) => o.ID == Parse.ToInt64((cboHinhThucChuyen.EditValue ?? ((object)0)).ToString()));
					if (hIS_TRAN_PATI_FORM != null)
					{
						txtMaHinhThucChuyen.Text = hIS_TRAN_PATI_FORM.TRAN_PATI_FORM_CODE;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHinhThucChuyen_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboHinhThucChuyen.Properties.Buttons[1].Visible = !string.IsNullOrEmpty((cboHinhThucChuyen.EditValue ?? "").ToString());
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboHinhThucChuyen_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboHinhThucChuyen.EditValue = null;
					cboHinhThucChuyen.Properties.Buttons[1].Visible = false;
					txtMaHinhThucChuyen.Text = "";
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void txtMaLyDoChuyen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					LoadTranPatiReasonCombo((sender as TextEdit).Text);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboLyDoChuyen_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				if (cboLyDoChuyen.EditValue != null)
				{
					HIS_TRAN_PATI_REASON hIS_TRAN_PATI_REASON = DataStore.TranPatiReasons.SingleOrDefault((HIS_TRAN_PATI_REASON o) => o.ID == Parse.ToInt64((cboLyDoChuyen.EditValue ?? ((object)0)).ToString()));
					if (hIS_TRAN_PATI_REASON != null)
					{
						cboLyDoChuyen.Properties.Buttons[1].Visible = true;
						txtMaLyDoChuyen.Text = hIS_TRAN_PATI_REASON.TRAN_PATI_REASON_CODE;
					}
				}
				FocusMoveOut();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboLyDoChuyen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboLyDoChuyen.EditValue != null && DataStore.TranPatiForms.SingleOrDefault((HIS_TRAN_PATI_FORM o) => o.ID == Parse.ToInt64((cboLyDoChuyen.EditValue ?? ((object)0)).ToString())) != null)
				{
					cboLyDoChuyen.Properties.Buttons[1].Visible = true;
					FocusMoveOut();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiSong_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode != PopupCloseMode.Normal)
				{
					return;
				}
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => IsNotRequiredRightTypeInCaseOfHavingAreaCode), IsNotRequiredRightTypeInCaseOfHavingAreaCode) + LogUtil.TraceData("this.cboNoiSong.EditValue", cboNoiSong.EditValue) + LogUtil.TraceData("this.cboDKKCBBD.EditValue", cboDKKCBBD.EditValue));
				ValidateRightRouteType();
				if (IsNotRequiredRightTypeInCaseOfHavingAreaCode)
				{
					string text = (cboNoiSong.EditValue ?? "").ToString();
					MediOrgADO mediOrgADO = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == (cboDKKCBBD.EditValue ?? "").ToString());
					if (mediOrgADO != null)
					{
						HasChangeValidRightRouteType(mediOrgADO.MEDI_ORG_CODE, text);
					}
					if (!string.IsNullOrEmpty(text))
					{
						switch (text)
						{
						case "K1":
						case "K2":
						case "K3":
							lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Black;
							dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, null);
							rdoRightRoute.Checked = true;
							dxValidationProvider1.RemoveControlError(cboHeinRightRoute);
							break;
						}
					}
				}
				if (cboNoiSong.EditValue != null)
				{
					ChangeDefaultHeinRatio();
					cboNoiSong.Properties.Buttons[1].Visible = true;
					if (txtMaNoiChuyenDen.Enabled)
					{
						txtMaNoiChuyenDen.Focus();
						txtMaNoiChuyenDen.SelectAll();
					}
					else
					{
						txtHNCode.Focus();
						txtHNCode.SelectAll();
					}
				}
				else
				{
					cboNoiSong.Focus();
					cboNoiSong.SelectAll();
					cboNoiSong.ShowPopup();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiSong_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Return)
				{
					return;
				}
				if (IsNotRequiredRightTypeInCaseOfHavingAreaCode)
				{
					string text = (cboNoiSong.EditValue ?? "").ToString();
					MediOrgADO mediOrgADO = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == (cboDKKCBBD.EditValue ?? "").ToString());
					if (mediOrgADO != null)
					{
						HasChangeValidRightRouteType(mediOrgADO.MEDI_ORG_CODE, text);
					}
					if (!string.IsNullOrEmpty(text))
					{
						switch (text)
						{
						case "K1":
						case "K2":
						case "K3":
							rdoRightRoute.Checked = true;
							dxValidationProvider1.RemoveControlError(cboHeinRightRoute);
							break;
						}
					}
				}
				if (cboNoiSong.EditValue != null)
				{
					ChangeDefaultHeinRatio();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboNoiSong_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			try
			{
				if (e.Button.Kind == ButtonPredefines.Delete)
				{
					cboNoiSong.EditValue = null;
					cboNoiSong.Properties.Buttons[1].Visible = false;
					MediOrgADO mediOrgADO = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == (cboDKKCBBD.EditValue ?? "").ToString());
					if (mediOrgADO != null && IsNotRequiredRightTypeInCaseOfHavingAreaCode)
					{
						HasChangeValidRightRouteType(mediOrgADO.MEDI_ORG_CODE);
					}
					ChangeDefaultHeinRatio();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboNoiSong_EditValueChanged(object sender, EventArgs e)
		{
			try
			{
				cboNoiSong.Properties.Buttons[1].Visible = cboNoiSong.EditValue != null;
				if (cboNoiSong.EditValue != null && rdoRightRoute.Checked && IsNotRequiredRightTypeInCaseOfHavingAreaCode)
				{
					dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, null);
					lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Black;
				}
				ResetValidationRightRoute_Present();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void cboChanDoanTD_Closed(object sender, ClosedEventArgs e)
		{
			try
			{
				if (e.CloseMode == PopupCloseMode.Normal || e.CloseMode == PopupCloseMode.Immediate)
				{
					if (cboChanDoanTD.EditValue != null)
					{
						ChangecboChanDoanTD();
					}
					else if (IsObligatoryTranferMediOrg && !string.IsNullOrEmpty(_TextIcdName))
					{
						ChangecboChanDoanTD_V2_GanICDNAME(_TextIcdName);
					}
					else
					{
						SendKeys.Send("{TAB}");
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void cboChanDoanTD_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return && cboChanDoanTD.EditValue != null)
				{
					ChangecboChanDoanTD();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkJoin5Year_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (chkPaid6Month.Enabled)
					{
						chkPaid6Month.Focus();
					}
					else if (txtFreeCoPainTime.Enabled)
					{
						txtFreeCoPainTime.Focus();
						txtFreeCoPainTime.SelectAll();
					}
					else
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void Join5YearAndPaid6MonthCheckedChanged()
		{
			try
			{
				if (chkJoin5Year.Checked && chkPaid6Month.Checked)
				{
					lciFreeCoPainTime.AppearanceItemCaption.ForeColor = Color.Maroon;
					ValidFreeCoPainTime(true);
				}
				else if (!chkPaid6Month.Checked)
				{
					lciFreeCoPainTime.AppearanceItemCaption.ForeColor = Color.Black;
					ValidFreeCoPainTime(false);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkPaid6Month_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				ChangeDefaultHeinRatio();
				Join5YearAndPaid6MonthCheckedChanged();
				if (entity.IsInitFromCallPatientTypeAlter)
				{
					ValidateCheckBox6M(chkPaid6Month.Checked);
					if (!chkJoin5Year.Checked && !chkPaid6Month.Checked)
					{
						IsShowMessage = false;
					}
					else if (chkJoin5Year.Checked && chkPaid6Month.Checked)
					{
						IsShowMessage = true;
					}
					if (chkPaid6Month.Checked && chkPaid6Month.OldEditValue != chkPaid6Month.EditValue)
					{
						ShowMessageNotAutoCheck5Y6M(chkPaid6Month);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateCheckBox6M(bool IsVlid)
		{
			try
			{
				if (IsVlid)
				{
					lciFreeCoPainTime.AppearanceItemCaption.ForeColor = Color.Maroon;
					TemplateHeinBHYT1__CheckBox__ValidationRule templateHeinBHYT1__CheckBox__ValidationRule = new TemplateHeinBHYT1__CheckBox__ValidationRule();
					templateHeinBHYT1__CheckBox__ValidationRule.chk = chkPaid6Month;
					templateHeinBHYT1__CheckBox__ValidationRule.txt = txtFreeCoPainTime;
					templateHeinBHYT1__CheckBox__ValidationRule.ErrorType = ErrorType.Warning;
					dxValidationProvider1.SetValidationRule(txtFreeCoPainTime, templateHeinBHYT1__CheckBox__ValidationRule);
				}
				else
				{
					lciFreeCoPainTime.AppearanceItemCaption.ForeColor = Color.Black;
					dxValidationProvider1.SetValidationRule(txtFreeCoPainTime, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateCheckBox5Y(bool IsVlid)
		{
			try
			{
				if (IsVlid)
				{
					lciDu5Nam.AppearanceItemCaption.ForeColor = Color.Maroon;
					TemplateHeinBHYT1__CheckBox__ValidationRule templateHeinBHYT1__CheckBox__ValidationRule = new TemplateHeinBHYT1__CheckBox__ValidationRule();
					templateHeinBHYT1__CheckBox__ValidationRule.chk = chkJoin5Year;
					templateHeinBHYT1__CheckBox__ValidationRule.txt = txtDu5Nam;
					templateHeinBHYT1__CheckBox__ValidationRule.ErrorType = ErrorType.Warning;
					dxValidationProvider1.SetValidationRule(txtDu5Nam, templateHeinBHYT1__CheckBox__ValidationRule);
				}
				else
				{
					lciDu5Nam.AppearanceItemCaption.ForeColor = Color.Black;
					dxValidationProvider1.SetValidationRule(txtDu5Nam, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ShowMessageNotAutoCheck5Y6M(CheckEdit checkEdit)
		{
			try
			{
				IsShowMessage = IsShowMessage && (chkJoin5Year.Checked || chkPaid6Month.Checked);
				if (HisConfigCFG.IsNotAutoCheck5Y6M && !IsShowMessage && !IsAutoCheck && (chkJoin5Year.Checked || chkPaid6Month.Checked))
				{
					IsShowMessage = true;
					if (XtraMessageBox.Show("Bệnh nhân phải có giấy chứng nhận không cùng chi trả trong năm. Bạn có muốn tiếp tục?", His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), MessageBoxButtons.YesNo) == DialogResult.No)
					{
						checkEdit.Checked = false;
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkPaid6Month_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					if (txtFreeCoPainTime.Enabled)
					{
						txtFreeCoPainTime.Focus();
						txtFreeCoPainTime.SelectAll();
					}
					else
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void txtAddress_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Return)
				{
					txtMaDKKCBBD.Focus();
					txtMaDKKCBBD.SelectAll();
					if (patientTypeAlterOld != null && !txtAddress.Text.Equals(patientTypeAlterOld.ADDRESS))
					{
						string text = txtSoThe.Text;
						text = HeinUtils.TrimHeinCardNumber(text.Replace(" ", "").Replace("  ", "").ToUpper()
							.Trim());
						CheckExamHistoryFromBHXHApi(text);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void chkTempQN_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (IsTempQN)
				{
					if (chkTempQN.Checked)
					{
						DisableControlWhenPatientTypeQN(IsTempQN, true);
					}
					else
					{
						DisableControlWhenPatientTypeQN(IsTempQN, false);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidateTranferMediOrg()
		{
			try
			{
				_TextIcdName = "";
				bool _isPresent = (string)cboHeinRightRoute.EditValue == "GT";
				LogSystem.Debug(LogUtil.TraceData(LogUtil.GetMemberName(() => _isPresent), _isPresent));
				if (_isPresent && ObligatoryTranferMediOrg == "1")
				{
					lblMediRecordMediOrgForm.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciIcdMain.AppearanceItemCaption.ForeColor = Color.Maroon;
					ValidNoiChuyenDen();
					ValidIcdByDTGT();
				}
				else if (_isPresent && ObligatoryTranferMediOrg == "3")
				{
					lblMediRecordMediOrgForm.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciIcdMain.AppearanceItemCaption.ForeColor = Color.Maroon;
					ValidNoiChuyenDen();
					ValidIcdByDTGT();
					ValidationSingleControl(dtTransferInTimeFrom, dxValidationProvider1);
					ValidationSingleControl(dtTransferInTimeTo, dxValidationProvider1);
					ValidationSingleControl(txtInCode, dxValidationProvider1);
					ValidChuyenTuyen();
					ValidateLookupWithTextEdit(cboHinhThucChuyen, txtMaHinhThucChuyen, dxValidationProvider1);
					ValidateLookupWithTextEdit(cboLyDoChuyen, txtMaLyDoChuyen, dxValidationProvider1);
					lciFordtTransferInTimeFrom.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciFordtTransferInTimeTo.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciInCode.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciMediRecordRouteTransfer.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciTransPatiFormCode.AppearanceItemCaption.ForeColor = Color.Maroon;
					lciTransPatiReasonCode.AppearanceItemCaption.ForeColor = Color.Maroon;
				}
				else
				{
					dxValidationProvider1.SetValidationRule(txtMaChanDoanTD, null);
					lciIcdMain.AppearanceItemCaption.ForeColor = Color.Black;
					dxValidationProvider1.SetValidationRule(dtTransferInTimeFrom, null);
					dxValidationProvider1.SetValidationRule(dtTransferInTimeTo, null);
					dxValidationProvider1.SetValidationRule(txtInCode, null);
					dxValidationProvider1.SetValidationRule(chkMediRecordRouteTransfer, null);
					dxValidationProvider1.SetValidationRule(txtMaHinhThucChuyen, null);
					dxValidationProvider1.SetValidationRule(txtMaLyDoChuyen, null);
					dxValidationProvider1.SetValidationRule(cboHinhThucChuyen, null);
					dxValidationProvider1.SetValidationRule(cboLyDoChuyen, null);
					dxValidationProvider1.SetValidationRule(txtMaNoiChuyenDen, null);
					lciFordtTransferInTimeFrom.AppearanceItemCaption.ForeColor = Color.Black;
					lciFordtTransferInTimeTo.AppearanceItemCaption.ForeColor = Color.Black;
					lciInCode.AppearanceItemCaption.ForeColor = Color.Black;
					lciMediRecordRouteTransfer.AppearanceItemCaption.ForeColor = Color.Black;
					lciTransPatiFormCode.AppearanceItemCaption.ForeColor = Color.Black;
					lciTransPatiReasonCode.AppearanceItemCaption.ForeColor = Color.Black;
					lblMediRecordMediOrgForm.AppearanceItemCaption.ForeColor = Color.Black;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ProcessLevelOfMediOrg()
		{
			try
			{
				string s = FixWrongLevelCode(BranchDataWorker.Branch.HEIN_LEVEL_CODE);
				if (string.IsNullOrEmpty(txtMaNoiChuyenDen.Text) || cboNoiChuyenDen.EditValue == null)
				{
					return;
				}
				MediOrgADO mediOrgADO = DataStore.MediOrgs.Where((MediOrgADO o) => o.MEDI_ORG_CODE == txtMaNoiChuyenDen.Text).FirstOrDefault();
				if (mediOrgADO == null)
				{
					return;
				}
				string s2 = FixWrongLevelCode(mediOrgADO.LEVEL_CODE);
				int num = int.Parse(s);
				int num2 = int.Parse(s2) - num;
				HIS_TRAN_PATI_FORM hIS_TRAN_PATI_FORM = null;
				if (num2 == 1)
				{
					hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.Where((HIS_TRAN_PATI_FORM o) => o.ID == 1).FirstOrDefault();
				}
				else if (num2 > 1)
				{
					hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.Where((HIS_TRAN_PATI_FORM o) => o.ID == 2).FirstOrDefault();
				}
				else if (num2 < 0)
				{
					hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.Where((HIS_TRAN_PATI_FORM o) => o.ID == 3).FirstOrDefault();
				}
				else if (num2 == 0)
				{
					hIS_TRAN_PATI_FORM = DataStore.TranPatiForms.Where((HIS_TRAN_PATI_FORM o) => o.ID == 4).FirstOrDefault();
				}
				cboHinhThucChuyen.EditValue = ((hIS_TRAN_PATI_FORM != null) ? new long?(hIS_TRAN_PATI_FORM.ID) : ((long?)null));
				txtMaHinhThucChuyen.Text = ((hIS_TRAN_PATI_FORM != null) ? hIS_TRAN_PATI_FORM.TRAN_PATI_FORM_CODE : "");
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private string FixWrongLevelCode(string code)
		{
			string result = "";
			try
			{
				switch (code)
				{
				case "TW":
					result = "1";
					break;
				case "T":
					result = "2";
					break;
				case "H":
					result = "3";
					break;
				case "X":
					result = "4";
					break;
				default:
					result = code;
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void ProcessChangeMediOrgCombo()
		{
			try
			{
				List<HisPatientSDO> sDO = HisPatientGet.GetSDO(txtSoThe.Text);
				sDO = ((sDO != null) ? sDO.Where((HisPatientSDO o) => o.HeinMediOrgCode == txtMaDKKCBBD.Text).ToList() : null);
				if (sDO != null && sDO.Count > 0 && (currentPatientSdo == null || currentPatientSdo.ID == 0L || (currentPatientSdo != null && currentPatientSdo.ID > 0 && currentPatientSdo.HeinCardNumber != HeinUtils.TrimHeinCardNumber(txtSoThe.Text.Replace(" ", "").ToUpper().Trim()))))
				{
					if (sDO.Count > 1)
					{
						new frmPatientChoice(sDO, new FillDataPatientSDOToRegisterForm(FillDataAfterSelectOnePatient), DataStore.Genders).ShowDialog();
					}
					else
					{
						FillDataAfterSelectOnePatient(sDO[0]);
					}
				}
				else
				{
					MediOrgSelectRowChange(true, (cboNoiSong.EditValue ?? "").ToString());
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void VisibleIcdControl(bool hasDialogText)
		{
			try
			{
				txtDialogText.Text = cboChanDoanTD.Text;
				txtDialogText.Visible = hasDialogText;
				cboChanDoanTD.Visible = !hasDialogText;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void HeinCardSelectRowHandler(HIS_PATIENT_TYPE_ALTER patientTypeAlter)
		{
			try
			{
				if (patientTypeAlter != null)
				{
					ChangeDataHeinInsuranceInfoByPatientTypeAlter(patientTypeAlter);
					ProcessFillDataTranPatiInForm(patientTypeAlter.TREATMENT_ID);
					ResetTranspatiInfoWithMediOrg((patientTypeAlter != null) ? patientTypeAlter.HEIN_MEDI_ORG_CODE : "");
				}
				else
				{
					txtHeinCardFromTime.Focus();
					txtHeinCardFromTime.SelectAll();
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void MediOrgSelectRowChange(bool isFocus, string liveArea = "")
		{
			try
			{
				HIS_MEDI_ORG hIS_MEDI_ORG = DataStore.MediOrgs.SingleOrDefault((MediOrgADO o) => o.MEDI_ORG_CODE == (cboDKKCBBD.EditValue ?? "").ToString());
				if (hIS_MEDI_ORG != null)
				{
					txtMaDKKCBBD.Text = hIS_MEDI_ORG.MEDI_ORG_CODE;
					if (IsDungTuyenCapCuuByTime)
					{
						rdoRightRoute.Checked = true;
						cboHeinRightRoute.EditValue = "CC";
						txtHeinRightRouteCode.Text = "CC";
						cboHeinRightRoute.Properties.Buttons[1].Visible = true;
						chkJoin5Year.Focus();
					}
					else
					{
						bool flag = false;
						if (HisConfigCFG.IsAllowedRouteTypeByDefault == "1" && hIS_MEDI_ORG != null)
						{
							TuDongChonLoaiThongTuyen(hIS_MEDI_ORG, null);
							if (cboHeinRightRoute.EditValue != null && !string.IsNullOrEmpty(txtHeinRightRouteCode.Text))
							{
								flag = true;
							}
						}
						else
						{
							InitDefaultRightRouteTypeAppointment(hIS_MEDI_ORG.MEDI_ORG_CODE);
						}
						if (currentPatientSdo == null || (currentPatientSdo != null && string.IsNullOrEmpty(currentPatientSdo.AppointmentCode)))
						{
							InitDefaultValidRightRouteType(isFocus, hIS_MEDI_ORG.MEDI_ORG_CODE, liveArea);
						}
						if (currentPatientSdo != null && !string.IsNullOrEmpty(currentPatientSdo.AppointmentCode) && !MediOrgCodeCurrent.Equals(hIS_MEDI_ORG.MEDI_ORG_CODE))
						{
							SetEnableControlHein(RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT, isFocus);
						}
						else if (MediOrgCodeCurrent == hIS_MEDI_ORG.MEDI_ORG_CODE || "3" == HeinLevelCodeCurrent || "4" == HeinLevelCodeCurrent)
						{
							SetEnableControlHein(RightRouterFactory.RIGHT_ROUTER, isFocus);
						}
						else if (IsMediOrgRightRouteByCurrent(hIS_MEDI_ORG.MEDI_ORG_CODE))
						{
							SetEnableControlHein(IsDefaultRightRouteType ? RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT : RightRouterFactory.RIGHT_ROUTER, isFocus);
						}
						else
						{
							cboNoiSong.Focus();
							cboNoiSong.SelectAll();
						}
						if (!flag)
						{
							ReloadDataCboRightRoute(hIS_MEDI_ORG.MEDI_ORG_CODE, liveArea);
						}
						ResetTranspatiInfoWithMediOrg(hIS_MEDI_ORG.MEDI_ORG_CODE);
						ChangeDefaultHeinRatio();
						if (entity.IsAutoSelectEmergency)
						{
							AutoSelectEmergency(entity);
						}
					}
					if (isFocus)
					{
						ResetHeinPatientCodeForRecalc();
					}
					InitComboPatientCode();
				}
				ValidateRightRouteType();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ReloadDataCboRightRoute(string mediOrgCode, string liveArea = "")
		{
			try
			{
				if (entity.IsAutoSelectEmergency)
				{
					AutoSelectEmergency(entity);
					return;
				}
				List<HeinRightRouteTypeData> list = new List<HeinRightRouteTypeData>();
				list.AddRange(DataStore.HeinRightRouteTypes);
				if (MediOrgCodeCurrent == mediOrgCode || ("1" != HeinLevelCodeCurrent && "2" != HeinLevelCodeCurrent))
				{
					list = list.Where((HeinRightRouteTypeData p) => p.HeinRightRouteTypeCode != "TH").ToList();
				}
				if (!string.IsNullOrEmpty(mediOrgCode))
				{
					if (!(MediOrgCodeCurrent == mediOrgCode) && !("3" == HeinLevelCodeCurrent) && !("4" == HeinLevelCodeCurrent) && !IsMediOrgRightRouteByCurrent(mediOrgCode))
					{
						if (!IsNotRequiredRightTypeInCaseOfHavingAreaCode)
						{
							goto IL_011f;
						}
						switch (liveArea)
						{
						case "K1":
						case "K2":
						case "K3":
							break;
						default:
							goto IL_011f;
						}
					}
					InitComboCommon(cboHeinRightRoute, list, "HeinRightRouteTypeCode", "HeinRightRouteTypeName", "HeinRightRouteTypeCode");
					return;
				}
				goto IL_011f;
				IL_011f:
				list = list.Where((HeinRightRouteTypeData p) => p.HeinRightRouteTypeCode != "DT").ToList();
				InitComboCommon(cboHeinRightRoute, list, "HeinRightRouteTypeCode", "HeinRightRouteTypeName", "HeinRightRouteTypeCode");
				if ((cboHeinRightRoute.EditValue ?? "").ToString() == "DT")
				{
					cboHeinRightRoute.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, string displayMemberCode)
		{
			try
			{
				InitComboCommon(cboEditor, data, valueMember, displayMember, 0, displayMemberCode, 0);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void InitComboCommon(Control cboEditor, object data, string valueMember, string displayMember, int displayMemberWidth, string displayMemberCode, int displayMemberCodeWidth)
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
				LogSystem.Warn(ex);
			}
		}

		private void ResetTranspatiInfoWithMediOrg(string mediOrgCode)
		{
			try
			{
				if (!string.IsNullOrEmpty(mediOrgCode) && MediOrgCodeCurrent.Equals(mediOrgCode))
				{
					txtHeinRightRouteCode.EditValue = null;
					ResetEditorControl.Reset(cboHeinRightRoute);
					ResetEditorControl.Reset(cboHinhThucChuyen);
					ResetEditorControl.Reset(cboLyDoChuyen);
					chkMediRecordNoRouteTransfer.Checked = false;
					chkMediRecordRouteTransfer.Checked = false;
					txtMaHinhThucChuyen.Text = "";
					txtMaLyDoChuyen.Text = "";
					txtMaNoiChuyenDen.Text = "";
					ResetEditorControl.Reset(cboNoiChuyenDen);
					txtMaChanDoanTD.Text = "";
					txtMaChanDoanTD.ErrorText = "";
					txtDialogText.Text = "";
					chkHasDialogText.Checked = false;
					ResetEditorControl.Reset(cboChanDoanTD);
					txtInCode.Text = "";
					dtTransferInTimeFrom.EditValue = null;
					dtTransferInTimeTo.EditValue = null;
					dxValidationProvider1.SetValidationRule(txtInCode, null);
					dxValidationProvider1.SetValidationRule(chkMediRecordRouteTransfer, null);
					dxValidationProvider1.SetValidationRule(chkMediRecordNoRouteTransfer, null);
					dxValidationProvider1.SetValidationRule(dtTransferInTimeFrom, null);
					dxValidationProvider1.SetValidationRule(dtTransferInTimeTo, null);
					dxValidationProvider1.SetValidationRule(txtMaHinhThucChuyen, null);
					dxValidationProvider1.SetValidationRule(cboHinhThucChuyen, null);
					dxValidationProvider1.SetValidationRule(txtMaLyDoChuyen, null);
					dxValidationProvider1.SetValidationRule(cboLyDoChuyen, null);
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ChangeDefaultHeinRatio()
		{
			try
			{
				ChangeDefaultHeinRatio("KH");
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ChangeDefaultHeinRatio(string treatmentTypeCode)
		{
			try
			{
				string heinCardNumber = HeinUtils.TrimHeinCardNumber(txtSoThe.Text.Replace(" ", "").ToUpper());
				BhytPatientTypeData bhytPatientTypeData = new BhytPatientTypeData();
				bhytPatientTypeData.HAS_BIRTH_CERTIFICATE = (chkHasDobCertificate.Checked ? "C" : "K");
				bhytPatientTypeData.LIVE_AREA_CODE = cboNoiSong.Text;
				bhytPatientTypeData.RIGHT_ROUTE_CODE = (rdoRightRoute.Checked ? "DT" : "TT");
				bhytPatientTypeData.JOIN_5_YEAR = (chkJoin5Year.Checked ? "C" : "K");
				bhytPatientTypeData.PAID_6_MONTH = (chkPaid6Month.Checked ? "C" : "K");
				bhytPatientTypeData.RIGHT_ROUTE_TYPE_CODE = (cboHeinRightRoute.EditValue ?? "").ToString();
				bhytPatientTypeData.HEIN_MEDI_ORG_CODE = (string)cboDKKCBBD.EditValue;
				bhytPatientTypeData.HEIN_MEDI_ORG_NAME = cboDKKCBBD.Text;
				bhytPatientTypeData.LEVEL_CODE = HeinLevelCodeCurrent;
				string facilityClassCode = ((patientTypeAlterOld != null) ? patientTypeAlterOld.FACILITY_CLASS : null);
				string formerLevelCode = ((patientTypeAlterOld != null) ? patientTypeAlterOld.FORMER_LEVEL_CODE : null);
				long point = ((patientTypeAlterOld != null) ? patientTypeAlterOld.CLASSIFY_POINT.GetValueOrDefault() : 0);
				V_HIS_TREATMENT_4 v_HIS_TREATMENT_ = ((patientTypeAlterOld != null && patientTypeAlterOld.TREATMENT_ID > 0) ? HisTreatmentGet.GetById(patientTypeAlterOld.TREATMENT_ID) : ((entity.HisTreatment != null && entity.HisTreatment.ID > 0) ? entity.HisTreatment : null));
				long clinicalInTime = ((v_HIS_TREATMENT_ != null) ? v_HIS_TREATMENT_.CLINICAL_IN_TIME.GetValueOrDefault() : 0);
				txtMucHuong.Text = new ServiceRequestProcess().GetDefaultHeinRatio(bhytPatientTypeData, heinCardNumber, treatmentTypeCode, facilityClassCode, formerLevelCode, point, clinicalInTime);
				((HIS_PATIENT_TYPE_ALTER)bhytPatientTypeData).IS_NEWBORN = (chkBaby.Checked ? new short?(1) : ((short?)null));
				((HIS_PATIENT_TYPE_ALTER)bhytPatientTypeData).HAS_ABSENT_LETTER = (chkHasAbsentLetter.Checked ? new short?(1) : ((short?)null));
				((HIS_PATIENT_TYPE_ALTER)bhytPatientTypeData).HAS_WORKING_LETTER = (chkHasWorkingLetter.Checked ? new short?(1) : ((short?)null));
				((HIS_PATIENT_TYPE_ALTER)bhytPatientTypeData).IS_TT46 = (chkTt46.Checked ? new short?(1) : ((short?)null));
				((HIS_PATIENT_TYPE_ALTER)bhytPatientTypeData).TT46_NOTE = txtTt46.Text.Trim();
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void SetEnableControlHein(RightRouterFactory rightRouterType, bool isFocus)
		{
			try
			{
				switch (rightRouterType)
				{
				case RightRouterFactory.RIGHT_ROUTER:
					txtHeinRightRouteCode.Enabled = true;
					cboHeinRightRoute.Enabled = true;
					ResetValueByDTCC(false);
					if (isFocus)
					{
						if (txtHeinRightRouteCode.Enabled)
						{
							txtHeinRightRouteCode.Focus();
							txtHeinRightRouteCode.SelectAll();
						}
						else
						{
							cboNoiSong.Focus();
						}
					}
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT_FOR_MEDI_ORG_ROUTE:
					txtHeinRightRouteCode.Enabled = false;
					cboHeinRightRoute.Enabled = false;
					ResetValueByDTCC(false);
					if (isFocus)
					{
						rdoWrongRoute.Focus();
					}
					break;
				case RightRouterFactory.WRONG_ROUTER:
					txtHeinRightRouteCode.Enabled = false;
					cboHeinRightRoute.Enabled = false;
					ResetValueByDTCC(false);
					if (isFocus)
					{
						if (txtHeinRightRouteCode.Enabled)
						{
							txtHeinRightRouteCode.Focus();
							txtHeinRightRouteCode.SelectAll();
						}
						else
						{
							cboNoiSong.Focus();
						}
					}
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT:
					txtHeinRightRouteCode.Enabled = true;
					cboHeinRightRoute.Enabled = true;
					ResetValueByDTCC(false);
					if (isFocus)
					{
						if (txtHeinRightRouteCode.Enabled)
						{
							txtHeinRightRouteCode.Focus();
							txtHeinRightRouteCode.SelectAll();
						}
						else
						{
							cboNoiSong.Focus();
						}
					}
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC:
					if (isFocus)
					{
						cboNoiSong.Focus();
						cboNoiSong.ShowPopup();
					}
					txtHeinRightRouteCode.Enabled = true;
					cboHeinRightRoute.Enabled = true;
					ResetValueByDTCC(false);
					ValidateTranferMediOrg();
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_HASAPPOINTMENT:
					if (isFocus)
					{
						cboNoiSong.Focus();
						cboNoiSong.ShowPopup();
					}
					txtHeinRightRouteCode.Enabled = true;
					cboHeinRightRoute.Enabled = true;
					ResetValueByDTCC(false);
					ValidateTranferMediOrg();
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT:
					txtHeinRightRouteCode.Enabled = true;
					cboHeinRightRoute.Enabled = true;
					ResetValueByDTCC(true);
					if (cboChanDoanTD.EditValue != null)
					{
						lblEditIcd.Enabled = true;
					}
					else
					{
						lblEditIcd.Enabled = false;
					}
					if (isFocus)
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
					}
					ValidateTranferMediOrg();
					break;
				case RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__DELETE_CHOICE_TYPE:
					if (isFocus)
					{
						cboNoiSong.Focus();
						cboNoiSong.SelectAll();
					}
					break;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private RightRouterFactory GetRouteFactory(HIS_PATIENT_TYPE_ALTER patyAlterBhyt)
		{
			RightRouterFactory result = RightRouterFactory.WRONG_ROUTER;
			try
			{
				string rIGHT_ROUTE_CODE = patyAlterBhyt.RIGHT_ROUTE_CODE;
				if (!(rIGHT_ROUTE_CODE == "DT"))
				{
					if (rIGHT_ROUTE_CODE == "TT")
					{
						result = RightRouterFactory.WRONG_ROUTER;
					}
				}
				else if (!string.IsNullOrEmpty(patyAlterBhyt.RIGHT_ROUTE_TYPE_CODE))
				{
					string rIGHT_ROUTE_TYPE_CODE = patyAlterBhyt.RIGHT_ROUTE_TYPE_CODE;
					if (!(rIGHT_ROUTE_TYPE_CODE == "CC"))
					{
						if (rIGHT_ROUTE_TYPE_CODE == "GT")
						{
							result = RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT;
						}
					}
					else
					{
						result = RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTCC;
					}
				}
				else
				{
					result = RightRouterFactory.RIGHT_ROUTER;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private bool IsMediOrgRightRouteByCurrent(string checkCurrentCode)
		{
			bool flag = false;
			try
			{
				List<string> mediOrgCodesAccepts = MediOrgCodesAccepts;
				return mediOrgCodesAccepts != null && mediOrgCodesAccepts.Contains(checkCurrentCode);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return false;
			}
		}

		private void VisibleButtonDeleteHeinRightRoute()
		{
			try
			{
				cboHeinRightRoute.Properties.Buttons[1].Visible = cboHeinRightRoute.EditValue != null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private bool HasChangeValidRightRouteType(string heinMediOrgCode, string liveArea = "")
		{
			bool result = false;
			try
			{
				if (("1" == HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT || "2" == HisHeinLevelCFG.HEIN_LEVEL_CODE__CURRENT) && !HisMediOrgCFG.MEDI_ORG_VALUE__CURRENT.Equals(heinMediOrgCode))
				{
					LogSystem.Debug("HasChangeValidRightRouteType.1");
					result = true;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		internal void ChangeDataHeinInsuranceInfoByPatientTypeAlter(HIS_PATIENT_TYPE_ALTER patyAlterBhyt)
		{
			try
			{
				patientTypeAlterOld = patyAlterBhyt;
				chkTempQN.Checked = patyAlterBhyt.IS_TEMP_QN == 1;
				if (patyAlterBhyt.ID > 0)
				{
					FillDataHeinInsuranceBySelectedPatientTypeAlter(patyAlterBhyt, false);
					HasChangeValidRightRouteType(patyAlterBhyt.HEIN_MEDI_ORG_CODE, patyAlterBhyt.LIVE_AREA_CODE);
					return;
				}
				if (!string.IsNullOrEmpty(patyAlterBhyt.HEIN_MEDI_ORG_CODE))
				{
					cboDKKCBBD.EditValue = patyAlterBhyt.HEIN_MEDI_ORG_CODE;
					txtMaDKKCBBD.EditValue = patyAlterBhyt.HEIN_MEDI_ORG_CODE;
					MediOrgSelectRowChange(false, patyAlterBhyt.LIVE_AREA_CODE);
					FillDataHeinInsuranceBySelectedPatientTypeAlter(patyAlterBhyt, false);
				}
				else
				{
					cboDKKCBBD.EditValue = null;
					txtMaDKKCBBD.EditValue = null;
				}
				if (patyAlterBhyt.HAS_BIRTH_CERTIFICATE == "C")
				{
					txtFreeCoPainTime.Enabled = false;
					dtFreeCoPainTime.Visible = false;
					chkHasDobCertificate.Checked = true;
					chkHasDobCertificate.Enabled = false;
					txtHeinRightRouteCode.Enabled = false;
					cboHeinRightRoute.Enabled = false;
					chkMediRecordRouteTransfer.Enabled = false;
					chkMediRecordNoRouteTransfer.Enabled = false;
					cboHinhThucChuyen.Enabled = false;
					txtMaHinhThucChuyen.Enabled = false;
					dtTransferInTimeTo.Enabled = false;
					dtTransferInTimeFrom.Enabled = false;
					txtMaLyDoChuyen.Enabled = false;
					cboLyDoChuyen.Enabled = false;
					txtMaNoiChuyenDen.Enabled = false;
					chkHasDialogText.Enabled = false;
					cboNoiChuyenDen.Enabled = false;
					txtDialogText.Enabled = false;
					txtMaChanDoanTD.Enabled = false;
					cboChanDoanTD.Enabled = false;
					lblEditIcd.Enabled = false;
					txtAddress.Enabled = false;
					TextEdit textEdit = txtMaDKKCBBD;
					bool enabled = (cboDKKCBBD.Enabled = false);
					textEdit.Enabled = enabled;
					txtMaDKKCBBD.EditValue = patyAlterBhyt.HEIN_MEDI_ORG_CODE;
					cboDKKCBBD.EditValue = patyAlterBhyt.HEIN_MEDI_ORG_CODE;
					DataStore.MediOrgForHasDobCretidentials = new List<HIS_MEDI_ORG>();
					HIS_MEDI_ORG hIS_MEDI_ORG = new HIS_MEDI_ORG();
					hIS_MEDI_ORG.MEDI_ORG_CODE = patyAlterBhyt.HEIN_MEDI_ORG_CODE;
					hIS_MEDI_ORG.MEDI_ORG_NAME = patyAlterBhyt.HEIN_MEDI_ORG_NAME;
					DataStore.MediOrgForHasDobCretidentials.Add(hIS_MEDI_ORG);
					MediOrgProcess.LoadDataToComboNoiDKKCBBD(cboDKKCBBD, DataStore.MediOrgForHasDobCretidentials);
					if (cboNoiSong.Enabled)
					{
						cboNoiSong.Focus();
						cboNoiSong.ShowPopup();
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private short? GetInCMKT()
		{
			short? result = null;
			try
			{
				if (chkMediRecordRouteTransfer.Checked)
				{
					result = (short)1;
					return result;
				}
				if (chkMediRecordNoRouteTransfer.Checked)
				{
					result = 0;
					return result;
				}
				result = null;
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		private void ProcessCaseWrongRoute(string mediOrgCode, string liveArea = "")
		{
			try
			{
				if (string.IsNullOrEmpty(mediOrgCode))
				{
					return;
				}
				if (!(MediOrgCodeCurrent == mediOrgCode) && !("3" == HeinLevelCodeCurrent) && !("4" == HeinLevelCodeCurrent))
				{
					if (IsNotRequiredRightTypeInCaseOfHavingAreaCode)
					{
						switch (liveArea)
						{
						case "K1":
						case "K2":
						case "K3":
							goto IL_0075;
						}
					}
					if (!IsMediOrgRightRouteByCurrent(mediOrgCode))
					{
						return;
					}
				}
				goto IL_0075;
				IL_0075:
				rdoRightRoute.Checked = true;
				SetEnableControlHein(IsDefaultRightRouteType ? RightRouterFactory.WRONG_ROUTER__CHOICE_RIGHT__CHOICE_TYPE_DTGT : RightRouterFactory.RIGHT_ROUTER, false);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		internal void SetValueAddress(string heinAddress)
		{
			try
			{
				txtAddress.Text = heinAddress ?? "";
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void ResetValue()
		{
			try
			{
				chkTt46.Checked = false;
				txtTt46.Enabled = false;
				chkHasAbsentLetter.Checked = false;
				chkHasWorkingLetter.Checked = false;
				chkBaby.Checked = false;
				chkBaby.Enabled = false;
				cboSoThe.Visible = false;
				chkTempQN.Checked = false;
				cboSoThe.Properties.DataSource = null;
				chkJoin5Year.Checked = false;
				chkHasDobCertificate.Checked = false;
				chkHasDialogText.Checked = false;
				VisibleIcdControl(chkHasDialogText.Checked);
				dtFreeCoPainTime.Visible = false;
				ResetEditorControl.Reset(dtFreeCoPainTime);
				ResetEditorControl.Reset(txtHeinCardToTime);
				ResetEditorControl.Reset(txtSoThe);
				ResetEditorControl.Reset(cboSoThe);
				ResetEditorControl.Reset(dtHeinCardFromTime);
				ResetEditorControl.Reset(dtHeinCardToTime);
				ResetEditorControl.Reset(txtHeinCardFromTime);
				ResetEditorControl.Reset(txtHeinCardToTime);
				ResetEditorControl.Reset(txtMaDKKCBBD);
				ResetEditorControl.Reset(cboDKKCBBD);
				ResetEditorControl.Reset(txtHeinRightRouteCode);
				ResetEditorControl.Reset(cboHeinRightRoute);
				ResetEditorControl.Reset(txtMaChanDoanTD);
				ResetEditorControl.Reset(cboChanDoanTD);
				ResetEditorControl.Reset(txtMaNoiChuyenDen);
				ResetEditorControl.Reset(cboNoiChuyenDen);
				ResetEditorControl.Reset(dtTransferInTimeFrom);
				ResetEditorControl.Reset(dtTransferInTimeTo);
				ResetEditorControl.Reset(cboNoiSong);
				ResetEditorControl.Reset(txtMaHinhThucChuyen);
				ResetEditorControl.Reset(cboHinhThucChuyen);
				ResetEditorControl.Reset(txtMaLyDoChuyen);
				ResetEditorControl.Reset(cboLyDoChuyen);
				ResetEditorControl.Reset(txtAddress);
				ResetEditorControl.Reset(txtMucHuong);
				txtMaChanDoanTD.ErrorText = "";
				txtHeinRightRouteCode.Enabled = false;
				chkHasDobCertificate.Enabled = false;
				cboHeinRightRoute.Enabled = false;
				txtHeinCardToTime.Enabled = true;
				txtMaNoiChuyenDen.Enabled = true;
				chkHasDialogText.Enabled = true;
				cboNoiChuyenDen.Enabled = true;
				txtDialogText.Enabled = true;
				txtMaChanDoanTD.Enabled = true;
				cboChanDoanTD.Enabled = true;
				chkJoin5Year.Enabled = true;
				chkMediRecordRouteTransfer.Enabled = true;
				chkMediRecordNoRouteTransfer.Enabled = true;
				cboHinhThucChuyen.Enabled = true;
				txtMaHinhThucChuyen.Enabled = true;
				dtTransferInTimeFrom.Enabled = true;
				dtTransferInTimeTo.Enabled = true;
				txtMaLyDoChuyen.Enabled = true;
				cboLyDoChuyen.Enabled = true;
				ResetValidationControl();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ResetValueByDTCC(bool enable)
		{
			try
			{
				chkMediRecordRouteTransfer.Enabled = enable;
				chkMediRecordNoRouteTransfer.Enabled = enable;
				cboHinhThucChuyen.Enabled = enable;
				txtMaHinhThucChuyen.Enabled = enable;
				dtTransferInTimeTo.Enabled = enable;
				dtTransferInTimeFrom.Enabled = enable;
				txtMaLyDoChuyen.Enabled = enable;
				cboLyDoChuyen.Enabled = enable;
				txtMaNoiChuyenDen.Enabled = enable;
				chkHasDialogText.Enabled = enable;
				cboNoiChuyenDen.Enabled = enable;
				txtDialogText.Enabled = enable;
				txtMaChanDoanTD.Enabled = enable;
				cboChanDoanTD.Enabled = enable;
				txtInCode.Enabled = enable;
				lblEditIcd.Enabled = enable;
				if (!enable)
				{
					chkMediRecordRouteTransfer.Checked = enable;
					chkMediRecordNoRouteTransfer.Checked = enable;
					cboHinhThucChuyen.EditValue = null;
					txtMaHinhThucChuyen.EditValue = null;
					dtTransferInTimeTo.EditValue = null;
					dtTransferInTimeFrom.EditValue = null;
					txtMaLyDoChuyen.EditValue = null;
					cboLyDoChuyen.EditValue = null;
					txtMaNoiChuyenDen.EditValue = null;
					cboNoiChuyenDen.EditValue = null;
					txtDialogText.EditValue = null;
					txtMaChanDoanTD.EditValue = null;
					cboChanDoanTD.EditValue = null;
					txtInCode.EditValue = null;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		internal void CoPhaiUCDuocGoiTuModuleTiepDonHayKhong(bool isByRegistor)
		{
			try
			{
				isCallByRegistor = isByRegistor;
			}
			catch (Exception ex)
			{
				LogSystem.Warn("Template_HeinBHYT1_Process/CoPhaiUCDuocGoiTuModuleTiepDonHayKhong:\n" + ((ex != null) ? ex.ToString() : null));
			}
		}

		internal void SetRsDataADO(ResultDataADO resultDataADO)
		{
			try
			{
				ResultDataADO = resultDataADO;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
			}
		}

		private void ValidControl()
		{
			try
			{
				ValidTxtSoThe();
				ValidNoiDKKCBBD();
				ValidFreeCoPainTime(true);
				ValidHeinCardToTime();
				ValidHeinCardFromTime();
				ValidRightRouteType();
				ValidAddress();
				ValidHNCode();
				ValidIcd();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidIcd()
		{
			try
			{
				TemplateHeinBHYT1__Icd__ValidationRule templateHeinBHYT1__Icd__ValidationRule = new TemplateHeinBHYT1__Icd__ValidationRule();
				templateHeinBHYT1__Icd__ValidationRule.txtMaChanDoanTD = txtMaChanDoanTD;
				templateHeinBHYT1__Icd__ValidationRule.cboChanDoanTD = cboChanDoanTD;
				templateHeinBHYT1__Icd__ValidationRule.ErrorText = ResourceMessage.MaBenhChinhKhongHopLe;
				templateHeinBHYT1__Icd__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtMaChanDoanTD, templateHeinBHYT1__Icd__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		public void SetColorForHeinPatientType()
		{
			if (HisConfigCFG.WarningHeinPatientTypeCode == "1")
			{
				layoutControlItem7.AppearanceItemCaption.ForeColor = Color.Maroon;
				ValidPatientTypeCode();
			}
		}

		public bool ValidateHeinPatientTypeCode()
		{
			try
			{
				object editValue = cboPatientCode.EditValue;
				string value = ((editValue != null) ? editValue.ToString() : null);
				if (PatientTypeIdBHYT == System.Convert.ToInt64(HisConfigCFG.PatientTypeCode__BHYT) && string.IsNullOrEmpty(value) && HisConfigCFG.WarningHeinPatientTypeCode == "2" && XtraMessageBox.Show("Chưa nhập mã đối tượng khám chữa bệnh. Bạn có muốn tiếp tục?", Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TieuDeCuaSoThongBaoLaCanhBao), MessageBoxButtons.YesNo) == DialogResult.No)
				{
					cboPatientCode.Focus();
					cboPatientCode.ShowPopup();
					return false;
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn("ValidateHeinPatientTypeCode: \n" + ((ex != null) ? ex.ToString() : null));
				return false;
			}
			return true;
		}

		private void ValidHNCode()
		{
			try
			{
				TemplateHeinBHYT1__HNCode__ValidationRule templateHeinBHYT1__HNCode__ValidationRule = new TemplateHeinBHYT1__HNCode__ValidationRule();
				templateHeinBHYT1__HNCode__ValidationRule.txtHNCode = txtHNCode;
				templateHeinBHYT1__HNCode__ValidationRule.ErrorText = Inventec.Common.Resource.Get.Value("His.UC.UCHein.Message.MaHoNgheoKhongHopLe", ResourceLanguageManager.LanguageUCHeinBHYT, His.UC.UCHein.Base.LanguageManager.GetCulture());
				templateHeinBHYT1__HNCode__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtHNCode, templateHeinBHYT1__HNCode__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidFreeCoPainTime(bool _isValidate)
		{
			if (_isValidate)
			{
				TemplateHeinBHYT1__FreeCoPainTime__ValidationRule templateHeinBHYT1__FreeCoPainTime__ValidationRule = new TemplateHeinBHYT1__FreeCoPainTime__ValidationRule();
				templateHeinBHYT1__FreeCoPainTime__ValidationRule.txtFreeCoPainTime = txtFreeCoPainTime;
				templateHeinBHYT1__FreeCoPainTime__ValidationRule.chkJoin5Year = chkJoin5Year;
				templateHeinBHYT1__FreeCoPainTime__ValidationRule.chkPaid6Month = chkPaid6Month;
				templateHeinBHYT1__FreeCoPainTime__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapNgayKhongHopLe);
				templateHeinBHYT1__FreeCoPainTime__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtFreeCoPainTime, templateHeinBHYT1__FreeCoPainTime__ValidationRule);
			}
			else
			{
				dxValidationProvider1.SetValidationRule(txtFreeCoPainTime, null);
			}
		}

		private void ValidHeinCardToTime()
		{
			TemplateHeinBHYT1__HeinCardToTime__ValidationRule templateHeinBHYT1__HeinCardToTime__ValidationRule = new TemplateHeinBHYT1__HeinCardToTime__ValidationRule();
			templateHeinBHYT1__HeinCardToTime__ValidationRule.txtHeinCardToTime = txtHeinCardToTime;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.txtHeinCardFromTime = txtHeinCardFromTime;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.checkKhongKTHSD = checkKhongKTHSD;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.isShowCheckKhongKTHSD = isShowCheckKhongKTHSD;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.IsEdit = IsEdit;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.ExceedDayAllow = ExceedDayAllow;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.PatientTypeId = PatientTypeId;
			templateHeinBHYT1__HeinCardToTime__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapNgayKhongHopLe);
			templateHeinBHYT1__HeinCardToTime__ValidationRule.ErrorType = ErrorType.Warning;
			dxValidationProvider1.SetValidationRule(txtHeinCardToTime, templateHeinBHYT1__HeinCardToTime__ValidationRule);
		}

		private void ValidHeinCardFromTime()
		{
			TemplateHeinBHYT1__HeinCardFromTime__ValidationRule templateHeinBHYT1__HeinCardFromTime__ValidationRule = new TemplateHeinBHYT1__HeinCardFromTime__ValidationRule();
			templateHeinBHYT1__HeinCardFromTime__ValidationRule.txtHeinCardFromTime = txtHeinCardFromTime;
			templateHeinBHYT1__HeinCardFromTime__ValidationRule.PatientTypeId = PatientTypeId;
			templateHeinBHYT1__HeinCardFromTime__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.NguoiDungNhapNgayKhongHopLe);
			templateHeinBHYT1__HeinCardFromTime__ValidationRule.ErrorType = ErrorType.Warning;
			dxValidationProvider1.SetValidationRule(txtHeinCardFromTime, templateHeinBHYT1__HeinCardFromTime__ValidationRule);
		}

		private void ValidRightRouteType(string heinMediOrgCode)
		{
			try
			{
				TemplateHeinBHYT1__RightRouteType__ValidationRule templateHeinBHYT1__RightRouteType__ValidationRule = new TemplateHeinBHYT1__RightRouteType__ValidationRule();
				templateHeinBHYT1__RightRouteType__ValidationRule.txtHeinRightRouteCode = txtHeinRightRouteCode;
				templateHeinBHYT1__RightRouteType__ValidationRule.cboHeinRightRoute = cboHeinRightRoute;
				templateHeinBHYT1__RightRouteType__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__RightRouteType__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, templateHeinBHYT1__RightRouteType__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidRightRouteType()
		{
			try
			{
				lblRightRouteType.AppearanceItemCaption.ForeColor = Color.Maroon;
				TemplateHeinBHYT1__RightRouteType__ValidationRule templateHeinBHYT1__RightRouteType__ValidationRule = new TemplateHeinBHYT1__RightRouteType__ValidationRule();
				templateHeinBHYT1__RightRouteType__ValidationRule.txtHeinRightRouteCode = txtHeinRightRouteCode;
				templateHeinBHYT1__RightRouteType__ValidationRule.cboHeinRightRoute = cboHeinRightRoute;
				templateHeinBHYT1__RightRouteType__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__RightRouteType__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtHeinRightRouteCode, templateHeinBHYT1__RightRouteType__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidPatientTypeCode()
		{
			try
			{
				TemplateHeinBHYT1__PatientTypeCode__ValidationRule templateHeinBHYT1__PatientTypeCode__ValidationRule = new TemplateHeinBHYT1__PatientTypeCode__ValidationRule();
				templateHeinBHYT1__PatientTypeCode__ValidationRule.cboPatientCode = cboPatientCode;
				templateHeinBHYT1__PatientTypeCode__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__PatientTypeCode__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(cboPatientCode, templateHeinBHYT1__PatientTypeCode__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidAddress()
		{
			try
			{
				TemplateHeinBHYT1__Address__ValidationRule templateHeinBHYT1__Address__ValidationRule = new TemplateHeinBHYT1__Address__ValidationRule();
				templateHeinBHYT1__Address__ValidationRule.txtAddress = txtAddress;
				templateHeinBHYT1__Address__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__Address__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtAddress, templateHeinBHYT1__Address__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidNoiChuyenDen()
		{
			try
			{
				TemplateHeinBHYT1__NoiChuyenDen__ValidationRule templateHeinBHYT1__NoiChuyenDen__ValidationRule = new TemplateHeinBHYT1__NoiChuyenDen__ValidationRule();
				templateHeinBHYT1__NoiChuyenDen__ValidationRule.cboNoiChuyenDen = cboNoiChuyenDen;
				templateHeinBHYT1__NoiChuyenDen__ValidationRule.txtMaNoiChuyenDen = txtMaNoiChuyenDen;
				templateHeinBHYT1__NoiChuyenDen__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__NoiChuyenDen__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtMaNoiChuyenDen, templateHeinBHYT1__NoiChuyenDen__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidChuyenTuyen()
		{
			try
			{
				TemplateHeinBHYT1__ChuyenTuyen__ValidationRule templateHeinBHYT1__ChuyenTuyen__ValidationRule = new TemplateHeinBHYT1__ChuyenTuyen__ValidationRule();
				templateHeinBHYT1__ChuyenTuyen__ValidationRule.chkMediRecordNoRouteTransfer = chkMediRecordNoRouteTransfer;
				templateHeinBHYT1__ChuyenTuyen__ValidationRule.chkMediRecordRouteTransfer = chkMediRecordRouteTransfer;
				templateHeinBHYT1__ChuyenTuyen__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__ChuyenTuyen__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(chkMediRecordRouteTransfer, templateHeinBHYT1__ChuyenTuyen__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidTxtSoThe()
		{
			try
			{
				TemplateHeinBHYT1__HeinCardNumber__ValidationRule templateHeinBHYT1__HeinCardNumber__ValidationRule = new TemplateHeinBHYT1__HeinCardNumber__ValidationRule();
				templateHeinBHYT1__HeinCardNumber__ValidationRule.txtSoThe = txtSoThe;
				templateHeinBHYT1__HeinCardNumber__ValidationRule.PatientTypeId = PatientTypeId;
				templateHeinBHYT1__HeinCardNumber__ValidationRule.chkHasDobCertificate = chkHasDobCertificate;
				templateHeinBHYT1__HeinCardNumber__ValidationRule.BhytBlackLists = entity.BhytBlackLists;
				templateHeinBHYT1__HeinCardNumber__ValidationRule.BhytWhiteLists = entity.BhytWhiteLists;
				templateHeinBHYT1__HeinCardNumber__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__HeinCardNumber__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtSoThe, templateHeinBHYT1__HeinCardNumber__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidNoiDKKCBBD()
		{
			try
			{
				TemplateHeinBHYT1__MediOrg__ValidationRule templateHeinBHYT1__MediOrg__ValidationRule = new TemplateHeinBHYT1__MediOrg__ValidationRule();
				templateHeinBHYT1__MediOrg__ValidationRule.txtMaDKKCBBD = txtMaDKKCBBD;
				templateHeinBHYT1__MediOrg__ValidationRule.cboDKKCBBD = cboDKKCBBD;
				templateHeinBHYT1__MediOrg__ValidationRule.PatientTypeId = PatientTypeId;
				templateHeinBHYT1__MediOrg__ValidationRule.chkHasDobCertificate = chkHasDobCertificate;
				templateHeinBHYT1__MediOrg__ValidationRule.ErrorText = His.UC.UCHein.Base.MessageUtil.GetMessage(His.UC.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				templateHeinBHYT1__MediOrg__ValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtMaDKKCBBD, templateHeinBHYT1__MediOrg__ValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		private void ValidIcdByDTGT()
		{
			try
			{
				TemplateHeinBHYT1__Icd__ValidationRule_Is_MediOrg templateHeinBHYT1__Icd__ValidationRule_Is_MediOrg = new TemplateHeinBHYT1__Icd__ValidationRule_Is_MediOrg();
				templateHeinBHYT1__Icd__ValidationRule_Is_MediOrg.txtIcdName = txtDialogText;
				templateHeinBHYT1__Icd__ValidationRule_Is_MediOrg.ErrorText = ResourceMessage.BatBuocNhapTenBenhVoiTruongHopBenhNhanLaDungTuyenGioiThieu;
				templateHeinBHYT1__Icd__ValidationRule_Is_MediOrg.ErrorType = ErrorType.Warning;
				dxValidationProvider1.SetValidationRule(txtMaChanDoanTD, templateHeinBHYT1__Icd__ValidationRule_Is_MediOrg);
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

		private void ResetErrorValidateRule()
		{
			try
			{
				IList<Control> invalidControls = dxValidationProvider1.GetInvalidControls();
				for (int num = invalidControls.Count - 1; num >= 0; num--)
				{
					dxValidationProvider1.RemoveControlError(invalidControls[num]);
				}
				dxErrorProvider1.ClearErrors();
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		protected void ValidateLookupWithTextEdit(LookUpEdit cbo, TextEdit textEdit, DXValidationProvider dxValidationProviderEditor)
		{
			try
			{
				LookupEditWithTextEditValidationRule lookupEditWithTextEditValidationRule = new LookupEditWithTextEditValidationRule();
				lookupEditWithTextEditValidationRule.txtTextEdit = textEdit;
				lookupEditWithTextEditValidationRule.cbo = cbo;
				lookupEditWithTextEditValidationRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				lookupEditWithTextEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(textEdit, lookupEditWithTextEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		protected void ValidateGridLookupWithTextEdit(GridLookUpEdit cbo, TextEdit textEdit, DXValidationProvider dxValidationProviderEditor)
		{
			try
			{
				GridLookupEditWithTextEditValidationRule gridLookupEditWithTextEditValidationRule = new GridLookupEditWithTextEditValidationRule();
				gridLookupEditWithTextEditValidationRule.txtTextEdit = textEdit;
				gridLookupEditWithTextEditValidationRule.cbo = cbo;
				gridLookupEditWithTextEditValidationRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				gridLookupEditWithTextEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(textEdit, gridLookupEditWithTextEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		protected void ValidationSingleControl(BaseEdit control, DXValidationProvider dxValidationProviderEditor)
		{
			try
			{
				ControlEditValidationRule controlEditValidationRule = new ControlEditValidationRule();
				controlEditValidationRule.editor = control;
				controlEditValidationRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				controlEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(control, controlEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}

		protected void ValidationSingleControl(Control control, DXValidationProvider dxValidationProviderEditor, string messageErr, IsValidControl isValidControl)
		{
			try
			{
				ControlEditValidationRule controlEditValidationRule = new ControlEditValidationRule();
				controlEditValidationRule.editor = control;
				if (isValidControl != null)
				{
					controlEditValidationRule.isUseOnlyCustomValidControl = true;
					controlEditValidationRule.isValidControl = isValidControl;
				}
				if (!string.IsNullOrEmpty(messageErr))
				{
					controlEditValidationRule.ErrorText = messageErr;
				}
				else
				{
					controlEditValidationRule.ErrorText = Inventec.Desktop.Common.LibraryMessage.MessageUtil.GetMessage(Inventec.Desktop.Common.LibraryMessage.Message.Enum.TruongDuLieuBatBuoc);
				}
				controlEditValidationRule.ErrorType = ErrorType.Warning;
				dxValidationProviderEditor.SetValidationRule(control, controlEditValidationRule);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
		}
	}
}
