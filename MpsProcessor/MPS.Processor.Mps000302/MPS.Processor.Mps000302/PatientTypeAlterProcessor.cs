using System;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000302.PDO.Config;
using Newtonsoft.Json;

namespace MPS.Processor.Mps000302
{
	public class PatientTypeAlterProcessor
	{
		public static HIS_PATIENT_TYPE_ALTER GetPatientTypeAlter(HIS_SERE_SERV s, PatientTypeCFG cfg, long treatmentTypeId, ref string key)
		{
			HIS_PATIENT_TYPE_ALTER hIS_PATIENT_TYPE_ALTER = null;
			try
			{
				key = s.JSON_PATIENT_TYPE_ALTER;
				if (!string.IsNullOrWhiteSpace(s.JSON_PATIENT_TYPE_ALTER))
				{
					hIS_PATIENT_TYPE_ALTER = JsonConvert.DeserializeObject<HIS_PATIENT_TYPE_ALTER>(s.JSON_PATIENT_TYPE_ALTER);
					if (hIS_PATIENT_TYPE_ALTER != null)
					{
						key = ToString(hIS_PATIENT_TYPE_ALTER, s, treatmentTypeId);
					}
				}
			}
			catch (Exception ex)
			{
				LogSystem.Warn(ex);
			}
			return hIS_PATIENT_TYPE_ALTER;
		}

		public static string ToString(HIS_PATIENT_TYPE_ALTER patyAlter, HIS_SERE_SERV s, long treatmentTypeId)
		{
			if (patyAlter != null)
			{
				string text = NVL(patyAlter.HEIN_CARD_NUMBER) + "|" + NVL(patyAlter.HEIN_MEDI_ORG_CODE) + "|" + NVL(patyAlter.LEVEL_CODE) + "|" + NVL(patyAlter.RIGHT_ROUTE_CODE) + "|" + NVL(patyAlter.RIGHT_ROUTE_TYPE_CODE) + "|" + NVL(patyAlter.JOIN_5_YEAR) + "|" + NVL(patyAlter.PAID_6_MONTH) + "|" + NVL(patyAlter.LIVE_AREA_CODE) + "|" + NVL(patyAlter.HNCODE) + "|" + NVL(patyAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault().ToString()) + "|" + NVL(patyAlter.HEIN_CARD_TO_TIME.GetValueOrDefault().ToString());
				if (patyAlter.LEVEL_CODE == "2" && patyAlter.RIGHT_ROUTE_CODE == "TT" && treatmentTypeId == 3)
				{
					text = ((s.TDL_INTRUCTION_DATE >= 20210101000000L) ? (text + "|true") : (text + "|false"));
				}
				return text;
			}
			return null;
		}

		public static string ToString(HIS_PATIENT_TYPE_ALTER patyAlter)
		{
			if (patyAlter != null)
			{
				return NVL(patyAlter.HEIN_CARD_NUMBER) + "|" + NVL(patyAlter.HEIN_MEDI_ORG_CODE) + "|" + NVL(patyAlter.LEVEL_CODE) + "|" + NVL(patyAlter.RIGHT_ROUTE_CODE) + "|" + NVL(patyAlter.RIGHT_ROUTE_TYPE_CODE) + "|" + NVL(patyAlter.JOIN_5_YEAR) + "|" + NVL(patyAlter.PAID_6_MONTH) + "|" + NVL(patyAlter.LIVE_AREA_CODE) + "|" + NVL(patyAlter.HNCODE) + "|" + NVL(patyAlter.HEIN_CARD_FROM_TIME.GetValueOrDefault().ToString()) + "|" + NVL(patyAlter.HEIN_CARD_TO_TIME.GetValueOrDefault().ToString());
			}
			return "";
		}

		private static string NVL(string s)
		{
			return (!string.IsNullOrWhiteSpace(s)) ? s : "";
		}
	}
}
