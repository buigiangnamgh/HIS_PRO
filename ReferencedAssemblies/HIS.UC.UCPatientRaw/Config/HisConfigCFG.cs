using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.UCPatientRaw.Config
{
    internal class HisConfigCFG
    {
        private const string CONFIG_KEY__REQUIRED_CCCD_NUMBER = "HIS.Desktop.Plugins.RegisterV2.Requied_Cccd_Number";
        private static bool IS_REQUIRED_CCCD_NUMBER = false;

        public static bool IsRequired_Cccd_Number
        {
            get
            {
                IS_REQUIRED_CCCD_NUMBER = HIS.Desktop.LocalStorage.HisConfig.HisConfigs.Get<string>(CONFIG_KEY__REQUIRED_CCCD_NUMBER) == "1";
                return IS_REQUIRED_CCCD_NUMBER;
            }
            set
            {
                IS_REQUIRED_CCCD_NUMBER = value;
            }
        }

    }
}
