using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.TreatmentFinish.Resources
{
    class ResourceLanguageManager
    {
        public static ResourceManager LanguageFormTreatmentFinish = new ResourceManager(
            "HIS.Desktop.Plugins.TreatmentFinish.Resources.Lang",
            typeof(HIS.Desktop.Plugins.TreatmentFinish.FormTreatmentFinish).Assembly);
    }
}
