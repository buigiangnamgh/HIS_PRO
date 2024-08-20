using MOS.EFMODEL.DataModels;
using MPS.Processor.Mps000338.PDO.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000338
{
    public class PatientTypeAlterProcessor
    {
        public static HIS_PATIENT_TYPE_ALTER GetPatientTypeAlter(HIS_SERE_SERV s, PatientTypeCFG cfg, ref string key)
        {
            HIS_PATIENT_TYPE_ALTER result = null;
            try
            {
                key = s.JSON_PATIENT_TYPE_ALTER;
                if (!String.IsNullOrWhiteSpace(s.JSON_PATIENT_TYPE_ALTER))
                {
                    result = JsonConvert.DeserializeObject<HIS_PATIENT_TYPE_ALTER>(s.JSON_PATIENT_TYPE_ALTER);
                    if (result != null) key = ToString(result);
                }
                //if (s.IS_NO_EXECUTE != 1)
                //{
                //    if (s.PATIENT_TYPE_ID == cfg.PATIENT_TYPE__BHYT && s.JSON_PATIENT_TYPE_ALTER != null)
                //    {
                //        if (result != null)
                //        {
                //            key = ToString(result);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return result;
        }

        private static string ToString(HIS_PATIENT_TYPE_ALTER patyAlter)
        {
            if (patyAlter != null)
            {
                return NVL(patyAlter.HEIN_CARD_NUMBER) + "|"
                    + NVL(patyAlter.HEIN_MEDI_ORG_CODE) + "|"
                    + NVL(patyAlter.LEVEL_CODE) + "|"
                    + NVL(patyAlter.RIGHT_ROUTE_CODE) + "|"
                    + NVL(patyAlter.RIGHT_ROUTE_TYPE_CODE) + "|"
                    + NVL(patyAlter.JOIN_5_YEAR) + "|"
                    + NVL(patyAlter.PAID_6_MONTH) + "|"
                    + NVL(patyAlter.LIVE_AREA_CODE) + "|"
                    + NVL(patyAlter.HNCODE)
                    + NVL((patyAlter.HEIN_CARD_FROM_TIME ?? 0).ToString())
                    + NVL((patyAlter.HEIN_CARD_TO_TIME ?? 0).ToString()); ;
            }
            return null;
        }

        private static string NVL(string s)
        {
            return !string.IsNullOrWhiteSpace(s) ? s : "";
        }
    }
}
