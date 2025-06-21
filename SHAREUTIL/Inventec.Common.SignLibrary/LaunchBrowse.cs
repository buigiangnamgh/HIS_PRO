using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    class LaunchBrowse
    {
        internal LaunchBrowse() { }

        internal void Launch(string urlSite)
        {
            try
            {
                string browserPath = GetPathToDefaultBrowser();
                System.Diagnostics.Process.Start(browserPath, urlSite);

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => browserPath), browserPath));
            }
            catch
            {
                System.Diagnostics.Process.Start("cmd", "/C start" + " " + urlSite);
            }
            
        }


        /// <summary>
        /// Returns the path to the current default browser
        /// </summary>
        /// <returns></returns>
        string GetPathToDefaultBrowser()
        {
            const string currentUserSubKey =
            @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(currentUserSubKey, false))
            {
                string progId = (userChoiceKey.GetValue("ProgId").ToString());
                using (RegistryKey kp =
                       Registry.ClassesRoot.OpenSubKey(progId + @"\shell\open\command", false))
                {
                    // Get default value and convert to EXE path.
                    // It's stored as:
                    //    "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" -- "%1"
                    // So we want the first quoted string only
                    string rawValue = (string)kp.GetValue("");
                    Regex reg = new Regex("(?<=\").*?(?=\")");
                    Match m = reg.Match(rawValue);
                    return m.Success ? m.Value : "";
                }
            }
        }

        internal void LaunchChrome(string apiDomain)
        {
            var chromePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            var found = System.IO.File.Exists(chromePath);
            if (!found)
            {
                chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                found = System.IO.File.Exists(chromePath);
            }
            if (!found)
            {
                using (var openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    Filter = "Chrome browser|chrome.exe",
                    CheckPathExists = true,
                    Title = "Hãy chọn trình duyệt chrome"
                })
                {
                    if (openFileDialog.ShowDialog() ==
                    System.Windows.Forms.DialogResult.OK)
                    {
                        chromePath = openFileDialog.FileName;
                        found = true;
                    }
                }
            }
            if (!found)
                return;

            System.Diagnostics.Process.Start(chromePath, apiDomain);
        }

        string GetSecretKey(string maThe)
        {
            // return "abcdef";
            // {time}.salt.{hash({mã thẻ}.{time}.{private})}
            var tick = DateTime.Now.Ticks.ToString();
            var sha256 = SHA256.Create();
            var privateString = "VietSens_Siten_2019";
            var salt = new Random().Next().ToString();
            return tick + "." + salt + "." + Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(maThe + "." + tick + "." + salt + "." + privateString)));
        }
    }
}
