using Inventec.Common.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.FingerPrintManager.CacheClient
{
    public class CacheClientWorker
    {
        private static RegistryKey appFolder = Registry.CurrentUser.CreateSubKey("SOFTWARE").CreateSubKey("INVENTEC").CreateSubKey(RegistryConstant.APP_FOLDER);

        public static void ChangeValue(string value)
        {
            try
            {
                appFolder.SetValue("SignTypeDisplay", value);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        public static string GetValue()
        {
            string result = "";
            try
            {
                object value = appFolder.GetValue("SignTypeDisplay", "");
                result = ((value != null) ? value.ToString() : "");
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return result;
        }

        public static void ChangeValue(string type, string value)
        {
            try
            {
                appFolder.SetValue(type, value);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        public static string GetValue(string type)
        {
            string result = "";
            try
            {
                object value = appFolder.GetValue(type, "");
                result = ((value != null) ? value.ToString() : "");
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return result;
        }
    }
}
