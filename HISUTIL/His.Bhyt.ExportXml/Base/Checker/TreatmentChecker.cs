using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.Base.Checker
{
    class TreatmentChecker
    {
        internal static bool IsLatentTuberculosis(List<HIS_ICD> totalIcdData, string icdCode)
        {
            bool result = false;
            try
            {
                if (String.IsNullOrWhiteSpace(icdCode))
                    return result;
                //icd lao tiềm ẩn
                List<HIS_ICD> icdLatent = null;
                if (totalIcdData != null && totalIcdData.Count > 0)
                {
                    icdLatent = totalIcdData.Where(o => o.IS_LATENT_TUBERCULOSIS == 1).ToList();
                }
                if (icdLatent != null && icdLatent.Count > 0 && icdLatent.Exists(o => o.ICD_CODE == icdCode))
                    result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
