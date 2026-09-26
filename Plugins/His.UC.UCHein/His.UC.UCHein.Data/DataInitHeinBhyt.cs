using System;
using System.Collections.Generic;
using Inventec.Desktop.Common.Modules;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt.HeinLiveArea;
using MOS.LibraryHein.Bhyt.HeinRightRouteType;

namespace His.UC.UCHein.Data
{
	public class DataInitHeinBhyt
	{
		public string HeinPatientCode { get; set; }

		public string Template { get; set; }

		public List<HIS_MEDI_ORG> MediOrgs { get; set; }

		public List<HeinLiveAreaData> LiveAreas { get; set; }

		public List<HIS_ICD> Icds { get; set; }

		public List<HIS_TRAN_PATI_FORM> TranPatiForms { get; set; }

		public List<HIS_TRAN_PATI_REASON> TranPatiReasons { get; set; }

		public List<HeinRightRouteTypeData> HeinRightRouteTypes { get; set; }

		public string MEDI_ORG_CODE__CURRENT { get; set; }

		public List<string> MEDI_ORG_CODES__ACCEPTs { get; set; }

		public string HEIN_LEVEL_CODE__CURRENT { get; set; }

		public long TREATMENT_TYPE_ID__EXAM { get; set; }

		public List<HIS_TREATMENT_TYPE> TreatmentTypes { get; set; }

		public List<HIS_PATIENT_TYPE> PatientTypes { get; set; }

		public long PATIENT_TYPE_ID__BHYT { get; set; }

		public bool IsChild { get; set; }

		public long PatientTypeId { get; set; }

		public long isVisibleControl { get; set; }

		public string IsShowCheckKhongKTHSD { get; set; }

		public string SYS_MEDI_ORG_CODE { get; set; }

		public bool IsNotRequiredRightTypeInCaseOfHavingAreaCode { get; set; }

		public List<HIS_BHYT_WHITELIST> BhytWhiteLists { get; set; }

		public List<HIS_BHYT_BLACKLIST> BhytBlackLists { get; set; }

		public long ExceedDayAllow { get; set; }

		public string AutoCheckIcd { get; set; }

		public bool IsDefaultRightRouteType { get; set; }

		public bool IsEdit { get; set; }

		public bool IsTempQN { get; set; }

		public bool IsObligatoryTranferMediOrg { get; set; }

		public string ObligatoryTranferMediOrg { get; set; }

		public bool IsDungTuyenCapCuuByTime { get; set; }

		public bool IsAutoSelectEmergency { get; set; }

		public List<HIS_GENDER> Genders { get; set; }

		public FillDataPatientSDOToRegisterForm FillDataPatientSDOToRegisterForm { get; set; }

		public SetFocusMoveOut SetFocusMoveOut { get; set; }

		public SetShortcutKeyDown SetShortcutKeyDown { get; set; }

		public ProcessFillDataCareerUnder6AgeByHeinCardNumber ProcessFillDataCareerUnder6AgeByHeinCardNumber { get; set; }

		public DelegateAutoCheckCC AutoCheckCC { get; set; }

		public CheckExamHistoryByHeinCardNumber CheckExamHistory { get; set; }

		public DelegateSetRelativeAddress SetRelativeAddress { get; set; }

		public DeleteTreatmentTypeId DeleteTreatmentTypeId { get; set; }

		public Action ActChangePatientDob { get; set; }

		public bool IsSampleDepartment { get; set; }

		public V_HIS_TREATMENT_4 HisTreatment { get; set; }

		public Module currentModule { get; set; }

		public bool IsInitFromCallPatientTypeAlter { get; set; }

		public long PatientId { get; set; }

		public long treatmentTypeId { get; set; }

		public long ActionType { get; set; }

		public bool IsReset { get; set; }

		public bool IsHideIcdDeathCauseOnly { get; set; }

		public bool IsWarningIcdNotRecommendMainWhenEdit { get; set; }
	}
}
