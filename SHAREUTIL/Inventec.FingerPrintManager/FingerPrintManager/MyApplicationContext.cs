using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Inventec.Common.Logging;
using Inventec.FingerPrintManager.CacheClient;
using Inventec.FingerPrint;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace Inventec.FingerPrintManager
{
  public  class MyApplicationContext : ApplicationContext
    {
        private string deviceName;

        public MyApplicationContext()
        {
            Application.ApplicationExit += OnApplicationExit;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            try
            {
                LogSystem.Info("InitializeComponent.1");
                deviceName = CacheClientWorker.GetValue("FingerPrintDeviceName");
                SignViewInputADO signViewInputADO = new SignViewInputADO();
                signViewInputADO.ActGetSignImageFile = GetSignImageFIle;
                signViewInputADO.ActSelectDevice = ActSelectDevice;
                signViewInputADO.DriverName = deviceName;
                frmConnectFingerPrint mainWindow = new frmConnectFingerPrint(signViewInputADO);
                mainWindow.ShowDialog();
                LogSystem.Info("InitializeComponent.2");
                KillProssApp();
                LogSystem.Info("InitializeComponent.3");
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ActSelectDevice(string deviceName)
        {
            try
            {
                CacheClientWorker.ChangeValue("FingerPrintDeviceName", deviceName);
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private void ByteToFile(byte[] arrInFile, string saveFile)
        {
            try
            {
                File.WriteAllBytes(saveFile, arrInFile);
            }
            catch (Exception ex)
            {
                LogSystem.Warn("File gui sang khong phai dinh dang file pdf, he thong core ky phai convert sang pdf, nhung khong convert duoc, co the do khong co quyen do folder dang chay khong duoc gan quyen doc ghi, can kiem tra lai");
                LogSystem.Warn(ex);
                MessageBox.Show("Folder đang chạy không được cấp quyền đọc ghi, vui lòng kiểm tra lại");
            }
        }

        private byte[] ImageToByte(Bitmap img)
        {
            try
            {
                ImageConverter imageConverter = new ImageConverter();
                return (byte[])imageConverter.ConvertTo(img, typeof(byte[]));
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
            return null;
        }

        private void GetSignImageFIle(Bitmap bmpSignImage)
        {
            try
            {
                LogSystem.Debug("GetSignImageFIle__");
                if (bmpSignImage != null)
                {
                    string text = Path.Combine(Path.Combine(Application.StartupPath, "temp"), DateTime.Now.ToString("ddMMyyyy"), "STFingerPrintFile");
                    if (!Directory.Exists(text))
                    {
                        Directory.CreateDirectory(text);
                    }
                    string saveFile = Path.Combine(text, Guid.NewGuid().ToString() + ".png");
                    Bitmap img = ResizeSignImage(bmpSignImage);
                    ByteToFile(ImageToByte(img), saveFile);
                }
                KillProssApp();
            }
            catch (Exception ex)
            {
                LogSystem.Warn(ex);
            }
        }

        private Bitmap ResizeSignImage(Bitmap bmpSourceImage, string imageFile = "")
        {
            Size size = default(Size);
            if (bmpSourceImage != null)
            {
                LogSystem.Debug("bmpSourceImage.Size.Width:" + bmpSourceImage.Size.Width + "____bmpSourceImage.Size.Height:" + bmpSourceImage.Size.Height + "____imageFile:" + imageFile);
                size = bmpSourceImage.Size;
                if (bmpSourceImage.Size.Width > 600 || bmpSourceImage.Size.Height > 600)
                {
                    int num = (int)(600.0 / (double)bmpSourceImage.Size.Width * (double)bmpSourceImage.Size.Height);
                    LogSystem.Debug("heightD:" + num);
                    size = new Size(600, num);
                    LogSystem.Debug("Anh chu ky qua lon sẽ bi resize lai ve kich thuoc (" + size.Width + ", " + size.Height + ")size:" + size);
                }
            }
            Bitmap bitmap = ((!string.IsNullOrEmpty(imageFile)) ? ((Bitmap)Image.FromFile(imageFile)) : ((Bitmap)bmpSourceImage.Clone()));
            if (!string.IsNullOrEmpty(imageFile) && bitmap != null)
            {
                LogSystem.Debug("imageFile:" + imageFile);
                LogSystem.Debug("b1.Size:" + bitmap.Size);
                int num = bitmap.Size.Height;
                int width = bitmap.Size.Width;
                if (bitmap.Size.Width > 600 || bitmap.Size.Height > 600)
                {
                    width = 600;
                    num = (int)(600.0 / (double)bitmap.Size.Width * (double)bitmap.Size.Height);
                    LogSystem.Debug("heightD:" + num);
                }
                size = new Size(width, num);
                LogSystem.Debug("Anh chu ky qua lon sẽ bi resize lai ve kich thuoc (" + size.Width + ", " + size.Height + ")size:" + size);
            }
            int width2 = size.Width;
            int height = size.Height;
            Bitmap bitmap2 = new Bitmap(width2, height);
            Graphics graphics = Graphics.FromImage(bitmap2);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(bitmap, 0, 0, width2, height);
            graphics.Dispose();
            bitmap2.MakeTransparent();
            return bitmap2;
        }

        private void KillProssApp()
        {
            try
            {
                string exeNameProcess = "Inventec.FingerPrintManager";
                List<Process> list = (from o in Process.GetProcesses()
                                      where o.ProcessName == exeNameProcess || o.ProcessName == $"{exeNameProcess}.exe" || o.ProcessName == $"{exeNameProcess} (32 bit)" || o.ProcessName == $"{exeNameProcess}.exe (32 bit)"
                                      select o).ToList();
                if (list == null || list.Count() <= 0)
                {
                    return;
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    try
                    {
                        LogSystem.Warn("___KILL FingerPrintManager___");
                        list[i].Kill();
                    }
                    catch (Exception ex)
                    {
                        LogSystem.Warn(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Debug(ex);
            }
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            try
            {
                LogSystem.Info("OnApplicationExit. Time=" + DateTime.Now.ToString("yyyyMMddhhmmss"));
            }
            catch (Exception ex)
            {
                LogSystem.Debug(ex);
            }
        }
    }
}
