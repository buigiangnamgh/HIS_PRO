using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.CacheClient
{
    public class CacheClientWorker
    {
        private static RegistryKey appFolder = Registry.CurrentUser.CreateSubKey(RegistryConstant.SOFTWARE_FOLDER).CreateSubKey(RegistryConstant.COMPANY_FOLDER).CreateSubKey(RegistryConstant.APP_FOLDER);

        public static void ChangeValue(string value)
        {
            try
            {
                appFolder.SetValue(RegistryConstant.TYPE_DISPLAY_KEY, value);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public static string GetValue()
        {
            string vl = "";
            try
            {
                var f = appFolder.GetValue(RegistryConstant.TYPE_DISPLAY_KEY, "");
                vl = f != null ? f.ToString() : "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return vl;
        }

        public static void ChangeValue(string type, string value)
        {
            try
            {
                appFolder.SetValue(type, value);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public static string GetValue(string type)
        {
            string vl = "";
            try
            {
                var f = appFolder.GetValue(type, "");
                vl = f != null ? f.ToString() : "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return vl;
        }
    }
}
