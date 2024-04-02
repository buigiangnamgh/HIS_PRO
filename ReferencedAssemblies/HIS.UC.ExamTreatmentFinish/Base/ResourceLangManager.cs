using HIS.UC.ExamTreatmentFinish.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.Base
{
    class ResourceLangManager
    {
        internal static ResourceManager LanguageUCExamTreatmentFinish { get; set; }
        internal static void InitResourceLanguageManager()
        {
            try
            {
                LanguageUCExamTreatmentFinish = new ResourceManager("HIS.UC.ExamTreatmentFinish.Resources.Lang", typeof(UCExamTreatmentFinish).Assembly);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
