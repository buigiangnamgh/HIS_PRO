using HIS.Desktop.LocalStorage.BackendData;
using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.CallPatientSample.Config
{
    internal class HisConfigCFG
    {
        private const string MosUserUri = "Inventec.Token.ClientSystem.MosUser.Base.Uri";

        internal static string MOS_USER_URI;

        internal static void LoadConfig()
        {
            try
            {
                LogSystem.Debug("LoadConfig => 1");
                MOS_USER_URI = GetValue(MosUserUri);
                LogSystem.Debug("LoadConfig => 2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static string GetValue(string code)
        {
            string result = null;
            try
            {
                return HisConfigs.Get<string>(code);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
                result = null;
            }
            return result;
        }
    }
}
