using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    class HisConfigKey
    {
        internal const string PatientTypeCodeBHYTCFG = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";
        internal const string MAMAYOPTION = "XML.EXPORT.130.XML3.MA_MAY_OPTION";
        internal const string BedTimeOption = "HIS.QD_130_BYT.BED_TIME_OPTION";
        internal const string NguoiThucHienOption = "HIS.QD_130_BYT.NGUOI_THUC_HIEN_OPTION";

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
