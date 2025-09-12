using Inventec.Common.Integrate;
using Inventec.Common.Logging;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.Integrate;
using Inventec.Common.SignLibrary.LibraryMessage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary.FingerPrint
{
    class FingerPrintUseBehavior : BusinessBase, IFingerPrint
    {
        InputADO entity;
        byte[] SignPadImageData;
        string deviceSignPadName;
        System.Windows.Forms.Timer timerCheckFileSign;

        internal FingerPrintUseBehavior(CommonParam param, InputADO inputADOWorking)
            : base()
        {
            this.entity = inputADOWorking;
            this.deviceSignPadName = inputADOWorking != null ? inputADOWorking.DeviceSignPadName : null;
        }

        byte[] IFingerPrint.Run()
        {
            try
            {                
                //if (!IsProcessOpen("Inventec.FingerPrintManager"))
                //{
                //    string pathSaveFolder = Path.Combine(Path.Combine(Application.StartupPath, "temp"), DateTime.Now.ToString("ddMMyyyy"), "STFingerPrintFile");
                //    if (!Directory.Exists(pathSaveFolder))
                //    {
                //        Directory.CreateDirectory(pathSaveFolder);
                //    }
                //    DirectoryInfo dicInfo = new DirectoryInfo(pathSaveFolder);

                //    string[] fileImage = Directory.GetFiles(dicInfo.FullName, "*");
                //    if (fileImage != null && fileImage.Length > 0)
                //    {
                //        try
                //        {
                //            dicInfo.Delete(true);
                //        }
                //        catch (Exception exx)
                //        {
                //            LogSystem.Error(exx);
                //        }
                //    }

                //    ProcessStartInfo startInfo = new ProcessStartInfo();
                //    startInfo.FileName = Application.StartupPath + @"\Inventec.FingerPrintManager.exe";
                //    Process.Start(startInfo);                    
                //}

                if (!IsProcessOpen("Inventec.FingerPrintManager"))
                {
                    string pathSaveFolder = Path.Combine(Application.StartupPath, "temp", DateTime.Now.ToString("ddMMyyyy"), "STFingerPrintFile");

                    try
                    {
                        if (Directory.Exists(pathSaveFolder))
                        {
                            DirectoryInfo dicInfo = new DirectoryInfo(pathSaveFolder);
                            if (dicInfo.GetFiles().Length > 0)
                            {
                                dicInfo.Delete(true);
                            }
                        }
                        Directory.CreateDirectory(pathSaveFolder); // Đảm bảo thư mục luôn tồn tại
                    }
                    catch (Exception ex)
                    {
                        Inventec.Common.Logging.LogSystem.Error("Lỗi khi xử lý thư mục vân tay", ex);
                    }

                    string exePath = Path.Combine(Application.StartupPath, "Inventec.FingerPrintManager.exe");
                    if (File.Exists(exePath))
                    {
                        try
                        {
                            Process.Start(exePath);
                        }
                        catch (Exception ex)
                        {
                            Inventec.Common.Logging.LogSystem.Error("Lỗi khởi động FingerPrintManager", ex);
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Error("Không tìm thấy file FingerPrintManager.exe tại: " + exePath);
                    }
                }

                while (true)
                {
                    if (IsProcessOpen("Inventec.FingerPrintManager"))
                    {
                        Inventec.Common.Logging.LogSystem.Debug("Inventec.FingerPrintManager.Run.1");
                        //Nothing...
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("Inventec.FingerPrintManager.Run.2");
                        string pathSaveFolder = Path.Combine(Path.Combine(Application.StartupPath, "temp"), DateTime.Now.ToString("ddMMyyyy"), "STFingerPrintFile");
                        DirectoryInfo dicInfo = new DirectoryInfo(pathSaveFolder);

                        string[] fileImage = Directory.GetFiles(dicInfo.FullName, "*");
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => fileImage), fileImage)
                            + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dicInfo.FullName), dicInfo.FullName));
                        if (fileImage != null && fileImage.Length > 0)
                        {
                            //TODO
                            this.SignPadImageData = Utils.FileToByte(fileImage[0]);
                            Inventec.Common.Logging.LogSystem.Debug("Inventec.FingerPrintManager.Run.3");
                            break;                              
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Debug("Inventec.FingerPrintManager.Run.4");
                            break;
                        }
                    }
                }

                return this.SignPadImageData;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        private void CheckImageFile(object sender, EventArgs e)
        {
            try
            {
                string pathSaveFolder = Path.Combine(Path.Combine(Application.StartupPath, "temp"), DateTime.Now.ToString("ddMMyyyy"), "STFingerPrintFile");
                DirectoryInfo dicInfo = new DirectoryInfo(pathSaveFolder);

                string[] fileImage = Directory.GetFiles(dicInfo.FullName, "*");
                if (fileImage != null && fileImage.Length > 0)
                {
                    //TODO
                    this.SignPadImageData = Utils.FileToByte(fileImage[0]);

                    //picSignImage.Image = Image.FromFile(fileImage[0]);
                    timerCheckFileSign.Enabled = false;
                    timerCheckFileSign.Stop();
                    //try
                    //{
                    //    dicInfo.Delete(true);
                    //}
                    //catch (Exception exx)
                    //{
                    //    LogSystem.Error(exx);
                    //}                    
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        internal bool IsProcessOpen(string name)
        {
            try
            {
                //LogSystem.Debug(String.Format("Ứng dụng {0}.", name));
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName == name || clsProcess.ProcessName == String.Format("{0}.exe", name) || clsProcess.ProcessName == String.Format("{0} (32 bit)", name) || clsProcess.ProcessName == String.Format("{0}.exe (32 bit)", name))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                LogSystem.Debug(String.Format("Xảy ra lỗi khi kiểm tra ứng dụng {0}.", name), ex);
            }

            return false;
        }

        private void ActSelectDevice(string deviceName)
        {
            try
            {
                this.deviceSignPadName = deviceName;
                if (entity != null && entity.ActSelectDevice != null)
                {
                    entity.ActSelectDevice(deviceName);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void GetSignImageFIle(System.Drawing.Bitmap bmpSignImage)
        {
            try
            {
                if (bmpSignImage != null)
                {
                    this.SignPadImageData = Utils.ImageToByte(bmpSignImage);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
