using HIS.UC.ExamServiceAdd.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamServiceAdd.Base
{
    class ResourceLangManager
    {
        internal static ResourceManager LanguageUCExamServiceAdd { get; set; }
        internal static void InitResourceLanguageManager()
        {
            try
            {
                LanguageUCExamServiceAdd = new ResourceManager("HIS.UC.ExamServiceAdd.Resources.Lang", typeof(UCExamServiceAdd).Assembly);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
