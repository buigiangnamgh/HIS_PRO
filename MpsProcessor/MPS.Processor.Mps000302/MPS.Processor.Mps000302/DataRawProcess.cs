using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inventec.Common.DateTime;
using Inventec.Common.Logging;
using Inventec.Common.Mapper;
using MOS.EFMODEL.DataModels;
using MOS.LibraryHein.Bhyt;
using MPS.Processor.Mps000302.ADO;

namespace MPS.Processor.Mps000302
{
	public class DataRawProcess
	{
		public static PatientADO PatientRawToADO(V_HIS_TREATMENT treatment)
		{
			PatientADO patientADO = new PatientADO();
			try
			{
				if (treatment != null)
				{
					patientADO.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
					patientADO.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
					patientADO.DOB = treatment.TDL_PATIENT_DOB;
					patientADO.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
					patientADO.AGE = AgeUtil.CalculateFullAge(patientADO.DOB);
					patientADO.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
					if (treatment.TDL_PATIENT_DOB > 0)
					{
						patientADO.DOB_YEAR = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				patientADO = null;
			}
			return patientADO;
		}

		public static PatyAlterBhytADO PatyAlterBHYTRawToADO(HIS_PATIENT_TYPE_ALTER patyAlter, List<HIS_PATIENT_TYPE_ALTER> patyAlterAlls, V_HIS_TREATMENT treatment, HIS_BRANCH branch, List<HIS_TREATMENT_TYPE> treatmentTypes, V_HIS_PATIENT_TYPE_ALTER currentPatyAlter, List<SereServADO> listSereServ)
		{
			PatyAlterBhytADO patyAlterBhytADO = new PatyAlterBhytADO();
			try
			{
				if (patyAlter == null)
				{
					return patyAlterBhytADO;
				}
				DataObjectMapper.Map<PatyAlterBhytADO>(patyAlterBhytADO, patyAlter);
				patyAlterBhytADO.HEIN_CARD_NUMBER_SEPARATE = SetHeinCardNumberDisplayByNumber(patyAlter.HEIN_CARD_NUMBER);
				patyAlterBhytADO.HEIN_MEDI_ORG_CODE = patyAlter.HEIN_MEDI_ORG_CODE;
				patyAlterBhytADO.HEIN_MEDI_ORG_NAME = patyAlter.HEIN_MEDI_ORG_NAME;
				patyAlterBhytADO.IS_HEIN = "X";
				patyAlterBhytADO.IS_VIENPHI = "";
				if (!string.IsNullOrEmpty(patyAlter.HEIN_CARD_NUMBER))
				{
					patyAlterBhytADO.HEIN_CARD_NUMBER_1 = patyAlter.HEIN_CARD_NUMBER.Substring(0, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_2 = patyAlter.HEIN_CARD_NUMBER.Substring(2, 1);
					patyAlterBhytADO.HEIN_CARD_NUMBER_3 = patyAlter.HEIN_CARD_NUMBER.Substring(3, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_4 = patyAlter.HEIN_CARD_NUMBER.Substring(5, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_5 = patyAlter.HEIN_CARD_NUMBER.Substring(7, 3);
					patyAlterBhytADO.HEIN_CARD_NUMBER_6 = patyAlter.HEIN_CARD_NUMBER.Substring(10, 5);
				}
				if (patyAlter.HEIN_CARD_FROM_TIME.HasValue)
				{
					patyAlterBhytADO.STR_HEIN_CARD_FROM_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patyAlter.HEIN_CARD_FROM_TIME.Value);
				}
				if (patyAlter.HEIN_CARD_TO_TIME.HasValue)
				{
					patyAlterBhytADO.STR_HEIN_CARD_TO_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patyAlter.HEIN_CARD_TO_TIME.Value);
				}
				HIS_TREATMENT_TYPE hIS_TREATMENT_TYPE = ((currentPatyAlter != null) ? treatmentTypes.FirstOrDefault((HIS_TREATMENT_TYPE o) => o.ID == currentPatyAlter.TREATMENT_TYPE_ID) : null);
				if (hIS_TREATMENT_TYPE == null)
				{
					LogSystem.Error("Không tìm thấy treatmentType của thẻ " + LogUtil.TraceData(LogUtil.GetMemberName(() => patyAlterBhytADO), patyAlterBhytADO));
					return null;
				}
				patyAlterBhytADO.RATIO_STR = GetDefaultHeinRatioForView(patyAlterBhytADO.HEIN_CARD_NUMBER, hIS_TREATMENT_TYPE.HEIN_TREATMENT_TYPE_CODE, branch.HEIN_LEVEL_CODE, patyAlterBhytADO.RIGHT_ROUTE_CODE);
				if (patyAlterAlls != null && treatment != null)
				{
					patyAlterBhytADO.KBCB_TIME_FROM_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(patyAlterBhytADO.LOG_TIME);
					if (patyAlterBhytADO.LOG_TIME < 20210101000000L && !listSereServ.Exists((SereServADO o) => o.TDL_INTRUCTION_DATE < 20210101000000L) && patyAlterBhytADO.LEVEL_CODE == "2" && patyAlterBhytADO.RIGHT_ROUTE_CODE == "TT" && treatment.TDL_TREATMENT_TYPE_ID == 3)
					{
						patyAlterBhytADO.KBCB_TIME_FROM_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSereServ.Min((SereServADO o) => o.TDL_INTRUCTION_TIME));
					}
					else if (patyAlterBhytADO.LOG_TIME < patyAlterBhytADO.HEIN_CARD_FROM_TIME.GetValueOrDefault())
					{
						patyAlterBhytADO.KBCB_TIME_FROM_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(patyAlterBhytADO.HEIN_CARD_FROM_TIME.GetValueOrDefault());
					}
					HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = patyAlterAlls.FirstOrDefault((HIS_PATIENT_TYPE_ALTER o) => o.LOG_TIME > patyAlterBhytADO.LOG_TIME && PatientTypeAlterProcessor.ToString(o) != PatientTypeAlterProcessor.ToString(patyAlterBhytADO));
					if (hIS_PATIENT_TYPE_ALTER != null)
					{
						if (patyAlterBhytADO.HEIN_CARD_TO_TIME.GetValueOrDefault() > hIS_PATIENT_TYPE_ALTER.LOG_TIME)
						{
							patyAlterBhytADO.KBCB_TIME_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(patyAlterBhytADO.HEIN_CARD_TO_TIME.GetValueOrDefault());
						}
						else
						{
							patyAlterBhytADO.KBCB_TIME_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(hIS_PATIENT_TYPE_ALTER.LOG_TIME);
						}
					}
					else if (patyAlterBhytADO.LOG_TIME < 20210101000000L && !listSereServ.Exists((SereServADO o) => o.TDL_INTRUCTION_DATE > 20210101000000L) && patyAlterBhytADO.LEVEL_CODE == "2" && patyAlterBhytADO.RIGHT_ROUTE_CODE == "TT" && treatment.TDL_TREATMENT_TYPE_ID == 3)
					{
						patyAlterBhytADO.KBCB_TIME_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSereServ.Max((SereServADO o) => o.TDL_INTRUCTION_TIME));
					}
					else if (treatment.OUT_TIME.HasValue)
					{
						patyAlterBhytADO.KBCB_TIME_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME.Value);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				patyAlterBhytADO = null;
			}
			return patyAlterBhytADO;
		}

		public static PatyAlterBhytADO PatyAlterBHYTRawToADO(V_HIS_PATIENT_TYPE_ALTER patyAlter, HIS_BRANCH branch, List<HIS_TREATMENT_TYPE> treatmentTypes)
		{
			PatyAlterBhytADO patyAlterBhytADO = new PatyAlterBhytADO();
			try
			{
				DataObjectMapper.Map<PatyAlterBhytADO>(patyAlterBhytADO, patyAlter);
				patyAlterBhytADO.HEIN_CARD_NUMBER_SEPARATE = SetHeinCardNumberDisplayByNumber(patyAlter.HEIN_CARD_NUMBER);
				patyAlterBhytADO.IS_HEIN = "X";
				patyAlterBhytADO.IS_VIENPHI = "";
				if (!string.IsNullOrEmpty(patyAlter.HEIN_CARD_NUMBER))
				{
					patyAlterBhytADO.HEIN_CARD_NUMBER_1 = patyAlter.HEIN_CARD_NUMBER.Substring(0, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_2 = patyAlter.HEIN_CARD_NUMBER.Substring(2, 1);
					patyAlterBhytADO.HEIN_CARD_NUMBER_3 = patyAlter.HEIN_CARD_NUMBER.Substring(3, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_4 = patyAlter.HEIN_CARD_NUMBER.Substring(5, 2);
					patyAlterBhytADO.HEIN_CARD_NUMBER_5 = patyAlter.HEIN_CARD_NUMBER.Substring(7, 3);
					patyAlterBhytADO.HEIN_CARD_NUMBER_6 = patyAlter.HEIN_CARD_NUMBER.Substring(10, 5);
				}
				if (patyAlter.HEIN_CARD_FROM_TIME.HasValue)
				{
					patyAlterBhytADO.STR_HEIN_CARD_FROM_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patyAlter.HEIN_CARD_FROM_TIME.Value);
				}
				if (patyAlter.HEIN_CARD_TO_TIME.HasValue)
				{
					patyAlterBhytADO.STR_HEIN_CARD_TO_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(patyAlter.HEIN_CARD_TO_TIME.Value);
				}
				patyAlterBhytADO.RATIO_STR = GetDefaultHeinRatioForView(patyAlterBhytADO.HEIN_CARD_NUMBER, patyAlter.HEIN_TREATMENT_TYPE_CODE, branch.HEIN_LEVEL_CODE, patyAlterBhytADO.RIGHT_ROUTE_CODE);
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
				patyAlterBhytADO = null;
			}
			return patyAlterBhytADO;
		}

		public static string GetDefaultHeinRatioForView(string heinCardNumber, string treatmentTypeCode, string levelCode, string rightRouteCode)
		{
			string result = "";
			try
			{
				result = (int)(new BhytHeinProcessor().GetDefaultHeinRatio(treatmentTypeCode, heinCardNumber, levelCode, rightRouteCode).GetValueOrDefault() * 100m) + "%";
				LogSystem.Error(string.Format("treatmentTypeCode {0} , heinCardNumber {1}, levelCode {2}, rightRouteCode {3} ", treatmentTypeCode, heinCardNumber, levelCode, rightRouteCode));
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return result;
		}

		public static string SetHeinCardNumberDisplayByNumber(string heinCardNumber)
		{
			string text = "";
			try
			{
				if (!string.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.Length == 15)
				{
					string value = "-";
					return new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(value).Append(heinCardNumber.Substring(2, 1))
						.Append(value)
						.Append(heinCardNumber.Substring(3, 2))
						.Append(value)
						.Append(heinCardNumber.Substring(5, 2))
						.Append(value)
						.Append(heinCardNumber.Substring(7, 3))
						.Append(value)
						.Append(heinCardNumber.Substring(10, 5))
						.ToString();
				}
				return heinCardNumber;
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				return heinCardNumber;
			}
		}
	}
}
