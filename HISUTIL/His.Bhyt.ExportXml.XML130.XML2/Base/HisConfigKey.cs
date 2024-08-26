using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2.Base
{
    class HisConfigKey
    {
        internal const string PatientTypeCodeBHYTCFG = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";
        internal const string TutorialFormatCFG = "HIS.Desktop.Plugins.AssignPrescription.TutorialFormat";
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
