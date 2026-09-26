using System.Collections.Generic;
using His.UC.UCHein.Data;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt.HeinLiveArea;
using MOS.LibraryHein.Bhyt.HeinRightRouteType;

namespace His.UC.UCHein.Config
{
	internal class DataStore
	{
		internal static List<MediOrgADO> MediOrgs { get; set; }

		internal static List<HIS_MEDI_ORG> MediOrgForHasDobCretidentials { get; set; }

		internal static List<HeinLiveAreaData> LiveAreas { get; set; }

		internal static List<HIS_ICD> Icds { get; set; }

		internal static List<IcdADO> IcdADOs { get; set; }

		internal static List<HIS_TRAN_PATI_FORM> TranPatiForms { get; set; }

		internal static List<HIS_TRAN_PATI_REASON> TranPatiReasons { get; set; }

		internal static List<HIS_GENDER> Genders { get; set; }

		internal static List<HeinRightRouteTypeData> HeinRightRouteTypes { get; set; }

		internal static List<HIS_TREATMENT_TYPE> TreatmentTypes { get; set; }

		internal static List<HIS_PATIENT_TYPE> PatientTypes { get; set; }
	}
}
