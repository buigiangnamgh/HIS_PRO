using Microsoft.Win32;
using System;

namespace Inventec.Common.Integrate
{
    public class RegistryProcessor
    {
        private const string SOFTWARE_FOLDER = "SOFTWARE";
        private const string INVENTEC_FOLDER = "INVENTEC";
        internal static readonly string APP_FOLDER = "EMR.INTERGRATE";

        /// <summary>
        /// Ghi vao registry
        /// </summary>
        /// <param name="key">Ten tham so registry</param>
        /// <param name="value">Gia tri</param>
        /// <param name="subFolders">Mang quy dinh thu muc con can luu registry</param>
        public static void Write(string key, object value, params string[] subFolders)
        {
            try
            {
                RegistryKey register = GetRegister(subFolders);
                register.SetValue(key, value);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }            
        }

        /// <summary>
        /// Doc tu registry
        /// </summary>
        /// <param name="key">Ten tham so registry</param>
        /// <param name="subFolders">Mang quy dinh thu muc con can luu registry</param>
        public static object Read(string key, params string[] subFolders)
        {
            try
            {
                RegistryKey register = GetRegister(subFolders);
                return register.GetValue(key);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return String.Empty;
        }

        /// <summary>
        /// Doc tu registry
        /// </summary>
        /// <param name="key">Ten tham so registry</param>
        /// <param name="subFolders">Mang quy dinh thu muc con can luu registry</param>
        public static void DeleteValue(string key, params string[] subFolders)
        {
            try
            {
                RegistryKey register = GetRegister(subFolders);
                register.DeleteValue(key);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }            
        }

        private static RegistryKey GetRegister(params string[] subFolders)
        {
            RegistryKey register = Registry.CurrentUser.CreateSubKey(SOFTWARE_FOLDER).CreateSubKey(INVENTEC_FOLDER).CreateSubKey(APP_FOLDER);
            if (subFolders != null && subFolders.Length > 0)
            {
                foreach (string subFolder in subFolders)
                {
                    register = register.CreateSubKey(subFolder);
                }
            }
            return register;
        }
    }
}
