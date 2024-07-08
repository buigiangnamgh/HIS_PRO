using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    class HisConfigKey
    {
        internal const string PatientTypeCodeBHYTCFG = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";
        internal const string MA_BAC_SI_HEIN_SERVICE_TYPE = "XML.EXPORT.4210.XML3.MA_BAC_SI_HEIN_SERVICE_TYPE";

        internal const string MaBacSiOption = "XML.EXPORT.4210.XML3.MA_BAC_SI_EXAM_OPTION";

        internal const string NgayYlenhOption = "XML.EXPORT.4210.XML3.NGAY_YL_OPTION ";
        internal const string XML__4210__XML3__NGAY_KQ_OPTION = "XML.EXPORT.4210.XML3.NGAY_KQ_OPTION";

        internal static string GetConfigData(List<HIS_CONFIG> datas, string key)
        {
            string result = "";
            try
            {
                if (datas != null && datas.Count > 0)
                {
                    var cfg = datas.FirstOrDefault(o => o.KEY == key);
                    if (cfg != null)
                    {
                        result = !String.IsNullOrWhiteSpace(cfg.VALUE) ? cfg.VALUE : cfg.DEFAULT_VALUE;
                    }
                }

                if (result != null)
                {
                    result = result.Trim();
                }
            }
            catch (Exception ex)
            {
                result = "";
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
