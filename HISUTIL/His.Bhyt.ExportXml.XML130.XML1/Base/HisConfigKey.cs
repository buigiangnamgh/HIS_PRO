using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML1.Base
{
    class HisConfigKey
    {
        internal const string THOI_GIAN_QT_OPTION = "HIS.QD_130_BYT.THOI_GIAN_QT_OPTION";
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
