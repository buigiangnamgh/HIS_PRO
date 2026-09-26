using System;
using Inventec.Common.Logging;
using MOS.LibraryHein.Bhyt;

namespace His.UC.UCHein.ControlProcess
{
	public class ServiceRequestProcess
	{
		public string GetDefaultHeinRatio(BhytPatientTypeData patientTypeData, string heinCardNumber, string treatmentTypeCode, string facilityClassCode = null, string formerLevelCode = null, long point = 0L, long ClinicalInTime = 0L)
		{
			string result = "";
			try
			{
				result = new BhytHeinProcessor().GetDefaultHeinRatio(treatmentTypeCode, heinCardNumber, patientTypeData.LEVEL_CODE, patientTypeData.RIGHT_ROUTE_CODE, facilityClassCode, formerLevelCode, point, ClinicalInTime).GetValueOrDefault() * 100m + "%";
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public string GetDefaultHeinRatio(string heinCardNumber, string treatmentTypeCode, string levelCode, string rightRouteCode)
		{
			string result = "";
			try
			{
				result = new BhytHeinProcessor().GetDefaultHeinRatio(treatmentTypeCode, heinCardNumber, levelCode, rightRouteCode).GetValueOrDefault() * 100m + "%";
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}
	}
}
