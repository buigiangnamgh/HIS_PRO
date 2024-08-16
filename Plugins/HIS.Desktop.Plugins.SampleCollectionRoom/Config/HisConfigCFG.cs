using HIS.Desktop.LocalStorage.HisConfig;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.SampleCollectionRoom.Config
{
  public  class HisConfigCFG
    {
        private const string MosUser_Base_Uri = "Inventec.Token.ClientSystem.MosUser.Base.Uri";
        internal static string MOS_USER_URI;

        internal static void  LoadConfig()
        {
            MOS_USER_URI = GetValue(MosUser_Base_Uri);
        }

        private static string GetValue(string code)
        {
            string result = null;
            try
            {
                result = HisConfigs.Get<string>(code);
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
