using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using libzkfpcsharp;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using Sample;
using Inventec.FingerPrint;

namespace Inventec.FingerPrint
{
    public partial class frmConnectFingerPrint : Form
    {
        IntPtr mDevHandle = IntPtr.Zero;
        IntPtr mDBHandle = IntPtr.Zero;
        IntPtr FormHandle = IntPtr.Zero;
        bool bIsTimeToDie = false;
        bool IsRegister = false;
        bool bIdentify = true;
        byte[] FPBuffer;
        int RegisterCount = 0;
        const int REGISTER_FINGER_COUNT = 3;

        byte[][] RegTmps = new byte[3][];
        byte[] RegTmp = new byte[2048];
        byte[] CapTmp = new byte[2048];
        int cbCapTmp = 2048;
        int cbRegTmp = 0;
        int iFid = 1;
        private Action<Bitmap> actGetSignImageFile;
        private Action<string> actSelectDevice;
        private string deviceDefault = "";

        private int mfpWidth = 0;
        private int mfpHeight = 0;

        const int MESSAGE_CAPTURED_OK = 0x0400 + 6;

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        public frmConnectFingerPrint(SignViewInputADO signViewInputADO)
        {
            if (signViewInputADO != null)
            {
                actGetSignImageFile = signViewInputADO.ActGetSignImageFile;
                actSelectDevice = signViewInputADO.ActSelectDevice;
                deviceDefault = signViewInputADO.DriverName;
            }
            InitializeComponent();
            //int initResult = zkfp2.Init();
            //if (initResult == zkfperrdef.ZKFP_ERR_OK)
            //{
            //    int deviceCount = zkfp2.GetDeviceCount();

            //    if (deviceCount > 0)
            //    {
            //        MessageBox.Show("✅ Có thiết bị vân tay đang kết nối (" + deviceCount + ")");
            //        bnVerify.Enabled = true;
            //    }
            //    else if (deviceCount == 0)
            //        MessageBox.Show("❌ Không tìm thấy thiết bị.");
            //    else
            //        MessageBox.Show("⚠️ Lỗi khi kiểm tra thiết bị.");
            //}
            //else
            //{
            //    MessageBox.Show("❌ Lỗi khởi tạo SDK (Init).");
            //}
            bnInit_Click(null, null);
            bnOpen_Click(null, null);
            bnVerify_Click(null, null);

        }

        private void bnInit_Click(object sender, EventArgs e)
        {
            try
            {
                cmbIdx.Items.Clear();
                int ret = zkfperrdef.ZKFP_ERR_OK;
                if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
                {
                    int nCount = zkfp2.GetDeviceCount();
                    if (nCount > 0)
                    {
                        for (int i = 0; i < nCount; i++)
                        {
                            cmbIdx.Items.Add(i.ToString());
                        }
                        cmbIdx.Text = "0";
                        Inventec.Common.Logging.LogSystem.Debug("frmConnectFingerPrint deviceDefault: " + deviceDefault);
                        bnInit.Enabled = false;
                        bnFree.Enabled = true;
                        bnOpen.Enabled = true;
                    }
                    else
                    {
                        zkfp2.Terminate();
                        MessageBox.Show("Không có thiết bị nào được kết nối!");
                    }
                }
                else
                {
                    MessageBox.Show("Khởi tạo thất bại [ret=" + ret + "] !");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bnFree_Click(object sender, EventArgs e)
        {
            try
            {
                //zkfp2.Terminate();
                cbRegTmp = 0;
                bnInit.Enabled = true;
                bnFree.Enabled = false;
                bnOpen.Enabled = false;
                bnClose.Enabled = false;
                bnEnroll.Enabled = false;
                bnVerify.Enabled = false;
                bnIdentify.Enabled = false;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

        }

        private void bnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                int ret = zkfp.ZKFP_ERR_OK;
                if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(cmbIdx.SelectedIndex)))
                {
                    MessageBox.Show("Kết nối thiết bị thành công");
                    return;
                }
                if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
                {
                    MessageBox.Show("Khởi tạo DB thất bại");
                    //zkfp2.CloseDevice(mDevHandle);
                    mDevHandle = IntPtr.Zero;
                    return;
                }
                bnInit.Enabled = false;
                bnFree.Enabled = true;
                bnOpen.Enabled = false;
                bnClose.Enabled = true;
                bnEnroll.Enabled = true;
                bnVerify.Enabled = true;
                bnIdentify.Enabled = true;
                RegisterCount = 0;
                cbRegTmp = 0;
                iFid = 1;
                for (int i = 0; i < 3; i++)
                {
                    RegTmps[i] = new byte[2048];
                }
                byte[] paramValue = new byte[4];
                int size = 4;
                zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
                zkfp2.ByteArray2Int(paramValue, ref mfpWidth);

                size = 4;
                zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
                zkfp2.ByteArray2Int(paramValue, ref mfpHeight);

                FPBuffer = new byte[mfpWidth * mfpHeight];

                Thread captureThread = new Thread(new ThreadStart(DoCapture));
                captureThread.IsBackground = true;
                captureThread.Start();
                bIsTimeToDie = false;
                textRes.Text = "Open succ";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        private void DoCapture()
        {
            try
            {

                while (!bIsTimeToDie)
                {
                    cbCapTmp = 2048;
                    int ret = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp);
                    if (ret == zkfp.ZKFP_ERR_OK)
                    {
                        SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero);
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        public static byte[] Sharpen(byte[] input, int width, int height)
        {
            try
            {


                if (input.Length != width * height)
                    throw new ArgumentException("Kích thước dữ liệu không hợp lệ.");

                byte[] output = new byte[width * height];

                // Kernel làm nét 3x3
                int[,] kernel = new int[,]
                {
                    { -1, -1, -1 },
                    { -1,  9, -1 },
                    { -1, -1, -1 }
                };

                int kSize = 3;
                int kCenter = kSize / 2;

                for (int y = kCenter; y < height - kCenter; y++)
                {
                    for (int x = kCenter; x < width - kCenter; x++)
                    {
                        int sum = 0;

                        for (int ky = 0; ky < kSize; ky++)
                        {
                            for (int kx = 0; kx < kSize; kx++)
                            {
                                int px = x + kx - kCenter;
                                int py = y + ky - kCenter;
                                int pixel = input[py * width + px];
                                int weight = kernel[ky, kx];

                                sum += pixel * weight;
                            }
                        }

                        sum = Math.Max(0, Math.Min(255, sum)); // clamp 0–255
                        output[y * width + x] = (byte)sum;
                    }
                }

                return output;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }
        Bitmap bmp;
        protected override void DefWndProc(ref Message m)
        {
            try
            {
                switch (m.Msg)
                {
                    case MESSAGE_CAPTURED_OK:
                        {
                            MemoryStream ms = new MemoryStream();
                            BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
                            bmp = new Bitmap(ms);
                            this.picFPImg.Image = bmp;
                          //var bm1 =  Sharpen(ms.ToArray(),256,288);
                            //bm1.Save("MyFile3.Png", System.Drawing.Imaging.ImageFormat.Png);

                            if (IsRegister)
                            {
                                int ret = zkfp.ZKFP_ERR_OK;
                                int fid = 0, score = 0;
                                ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                if (zkfp.ZKFP_ERR_OK == ret)
                                {
                                    textRes.Text = "Dấu vân tay đã được đăng ký bởi " + fid + "!";
                                    return;
                                }
                                if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
                                {
                                    textRes.Text = "Xin mời lấy dấu vân tay (3 lần)!";
                                    return;
                                }
                                Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
                                String strBase64 = zkfp2.BlobToBase64(CapTmp, cbCapTmp);
                                byte[] blob = zkfp2.Base64ToBlob(strBase64);
                                RegisterCount++;
                                if (RegisterCount >= REGISTER_FINGER_COUNT)
                                {
                                    RegisterCount = 0;
                                    if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)) &&
                                           zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBAdd(mDBHandle, iFid, RegTmp)))
                                    {
                                        iFid++;
                                        textRes.Text = "Kết nối thiết bị thành công";
                                    }
                                    else
                                    {
                                        textRes.Text = "Kết nối thiết bị thành công [error code=" + ret + "]";
                                    }
                                    IsRegister = false;
                                    return;
                                }
                                else
                                {
                                    textRes.Text = "You need to press the " + (REGISTER_FINGER_COUNT - RegisterCount) + " times fingerprint";
                                }
                            }
                            else
                            {
                                if (cbRegTmp <= 0)
                                {
                                    textRes.Text = "Xin mời quét vây tay!";
                                    return;
                                }
                                if (bIdentify)
                                {
                                    int ret = zkfp.ZKFP_ERR_OK;
                                    int fid = 0, score = 0;
                                    ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
                                    if (zkfp.ZKFP_ERR_OK == ret)
                                    {
                                        textRes.Text = "Identify succ, fid= " + fid + ",score=" + score + "!";
                                        return;
                                    }
                                    else
                                    {
                                        textRes.Text = "Identify fail, ret= " + ret;
                                        return;
                                    }
                                }
                                else
                                {
                                    int ret = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
                                    if (0 < ret)
                                    {
                                        textRes.Text = "Match finger succ, score=" + ret + "!";
                                        return;
                                    }
                                    else
                                    {
                                        textRes.Text = "Match finger fail, ret= " + ret;
                                        return;
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        base.DefWndProc(ref m);
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormHandle = this.Handle;
        }

        private void bnClose_Click(object sender, EventArgs e)
        {
            try
            {

                bIsTimeToDie = true;
                RegisterCount = 0;
                Thread.Sleep(1000);
                zkfp2.CloseDevice(mDevHandle);
                bnInit.Enabled = false;
                bnFree.Enabled = true;
                bnOpen.Enabled = true;
                bnClose.Enabled = false;
                bnEnroll.Enabled = false;
                bnVerify.Enabled = false;
                bnIdentify.Enabled = false;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bnEnroll_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRegister)
                {
                    IsRegister = true;
                    RegisterCount = 0;
                    cbRegTmp = 0;
                    textRes.Text = "Xin mời quét dấu vân tay! (3 lần)";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bnIdentify_Click(object sender, EventArgs e)
        {
            try
            {
                if (!bIdentify)
                {
                    bIdentify = true;
                    textRes.Text = "Xin mời quét dấu vân tay!";
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                if (bIdentify)
                {
                    bIdentify = false;
                    textRes.Text = "Xin mời quét dấu vân tay!";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        private void bnChoose_Click(object sender, EventArgs e)
        {
            if (actGetSignImageFile != null && bmp != null && bmp.Width > 0)
            {
                actGetSignImageFile(bmp);
            }
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {

        }
    }
}
