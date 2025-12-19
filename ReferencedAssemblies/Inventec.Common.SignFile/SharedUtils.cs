using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace Inventec.Common.SignFile
{
    public class SharedUtils
    {
        // Methods
        public static string ConvertTVKhongDau(string str)
        {
            string[] strArray = new string[] {
            "\x00e0", "\x00e1", "ạ", "ả", "\x00e3", "\x00e2", "ầ", "ấ", "ậ", "ẩ", "ẫ", "ă", "ằ", "ắ", "ặ", "ẳ",
            "ẵ"
         };
            string[] strArray2 = new string[] {
            "\x00c0", "\x00c1", "Ạ", "Ả", "\x00c3", "\x00c2", "Ầ", "Ấ", "Ậ", "Ẩ", "Ẫ", "Ă", "Ằ", "Ắ", "Ặ", "Ẳ",
            "Ẵ"
         };
            string[] strArray3 = new string[] { "\x00e8", "\x00e9", "ẹ", "ẻ", "ẽ", "\x00ea", "ề", "ế", "ệ", "ể", "ễ" };
            string[] strArray4 = new string[] { "\x00c8", "\x00c9", "Ẹ", "Ẻ", "Ẽ", "\x00ca", "Ề", "Ế", "Ệ", "Ể", "Ễ" };
            string[] strArray5 = new string[] { "\x00ec", "\x00ed", "ị", "ỉ", "ĩ" };
            string[] strArray6 = new string[] { "\x00cc", "\x00cd", "Ị", "Ỉ", "Ĩ" };
            string[] strArray7 = new string[] {
            "\x00f2", "\x00f3", "ọ", "ỏ", "\x00f5", "\x00f4", "ồ", "ố", "ộ", "ổ", "ỗ", "ơ", "ờ", "ớ", "ợ", "ở",
            "ỡ"
         };
            string[] strArray8 = new string[] {
            "\x00d2", "\x00d3", "Ọ", "Ỏ", "\x00d5", "\x00d4", "Ồ", "Ố", "Ộ", "Ổ", "Ỗ", "Ơ", "Ờ", "Ớ", "Ợ", "Ở",
            "Ỡ"
         };
            string[] strArray9 = new string[] { "\x00f9", "\x00fa", "ụ", "ủ", "ũ", "ư", "ừ", "ứ", "ự", "ử", "ữ" };
            string[] strArray10 = new string[] { "\x00d9", "\x00da", "Ụ", "Ủ", "Ũ", "Ư", "Ừ", "Ứ", "Ự", "Ử", "Ữ" };
            string[] strArray11 = new string[] { "ỳ", "\x00fd", "ỵ", "ỷ", "ỹ" };
            string[] strArray12 = new string[] { "Ỳ", "\x00dd", "Ỵ", "Ỷ", "Ỹ" };
            str = str.Replace("đ", "d");
            str = str.Replace("Đ", "D");
            foreach (string str2 in strArray)
            {
                str = str.Replace(str2, "a");
            }
            foreach (string str3 in strArray2)
            {
                str = str.Replace(str3, "A");
            }
            foreach (string str4 in strArray3)
            {
                str = str.Replace(str4, "e");
            }
            foreach (string str5 in strArray4)
            {
                str = str.Replace(str5, "E");
            }
            foreach (string str6 in strArray5)
            {
                str = str.Replace(str6, "i");
            }
            foreach (string str7 in strArray6)
            {
                str = str.Replace(str7, "I");
            }
            foreach (string str8 in strArray7)
            {
                str = str.Replace(str8, "o");
            }
            foreach (string str9 in strArray8)
            {
                str = str.Replace(str9, "O");
            }
            foreach (string str10 in strArray9)
            {
                str = str.Replace(str10, "u");
            }
            foreach (string str11 in strArray10)
            {
                str = str.Replace(str11, "U");
            }
            foreach (string str12 in strArray11)
            {
                str = str.Replace(str12, "y");
            }
            foreach (string str13 in strArray12)
            {
                str = str.Replace(str13, "Y");
            }
            return str;
        }

        public static string GenerateTempFile()
        {
            try
            {
                string pathFolderTemp = GenerateTempFolderWithin();
                return Path.Combine(pathFolderTemp, Guid.NewGuid().ToString() + ".pdf");
            }
            catch (IOException exception)
            {
                Console.WriteLine("Error create temp file: " + exception.Message);
                return "";
            }
        }

        public static string GenerateTempFile(string ext)
        {
            try
            {
                string pathFolderTemp = GenerateTempFolderWithin();
                return Path.Combine(pathFolderTemp, Guid.NewGuid().ToString() + (!String.IsNullOrEmpty(ext) ? ext : ".pdf"));
            }
            catch (IOException exception)
            {
                Console.WriteLine("Error create temp file: " + exception.Message);
                return "";
            }
        }


        internal static string GenerateTempFolderWithin()
        {
            try
            {
                string pathFolderTemp = GenerateTempFolderWithinByDate();
                //if (treatment != null && !String.IsNullOrEmpty(treatment.TREATMENT_CODE))
                //{
                //    pathFolderTemp = Path.Combine(pathFolderTemp, treatment.TREATMENT_CODE);
                //}
                if (!Directory.Exists(pathFolderTemp))
                {
                    Directory.CreateDirectory(pathFolderTemp);
                }
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return "";
            }
        }

        internal static string GenerateTempFolderWithinByDate()
        {
            try
            {
                string pathFolderTemp = Path.Combine(ParentTempFolder(), DateTime.Now.ToString("ddMMyyyy"));
                if (!Directory.Exists(pathFolderTemp))
                {
                    Directory.CreateDirectory(pathFolderTemp);
                }
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return "";
            }
        }

        internal static string ParentTempFolder()
        {
            try
            {
                string pathFolderTemp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp");
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }


        public static void CreatePdf(PdfReader[] readers, ref string outFilePath)
        {
            Document document = new Document();
            outFilePath = GenerateTempFile();
            PdfCopy copy = new PdfCopy(document, File.Open(outFilePath, FileMode.Create));
            copy.SetMergeFields();
            document.Open();
            foreach (PdfReader reader in readers)
            {
                copy.AddDocument(reader);
            }
            document.Close();
            foreach (PdfReader reader in readers)
            {
                //reader.Close();
            }
        }

        public static void CreatePdf(string inFilePath, ref string outFilePath)
        {
            Document document = new Document();
            outFilePath = GenerateTempFile();
            PdfCopy copy = new PdfCopy(document, File.Open(outFilePath, FileMode.Create));
            copy.SetMergeFields();
            document.Open();
            PdfReader reader = new PdfReader(inFilePath);
            copy.AddDocument(reader);
            document.Close();
            reader.Close();
        }

        public static bool SaveNewFileFromReader(PdfReader reader, ref string outFilePath, bool isCloseReader)
        {
            bool flag = false;
            try
            {
                outFilePath = GenerateTempFile();

                using (FileStream fs_ = File.Open(outFilePath, FileMode.Create))
                {
                    PdfReader pdfReader = new PdfReader(reader);

                    var pdfConcat = new PdfConcatenate(fs_);
                    var pages = new List<int>();
                    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);

                    pdfReader.Close();
                    pdfConcat.Close();
                }

                flag = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error insertSignature: " + exception.Message);
                flag = false;
            }
            finally
            {
                if (isCloseReader && reader != null)
                {
                    reader.Close();
                }
            }
            return flag;
        }

        public static bool SaveNewFileFromReader(string filename, ref string outFilePath)
        {
            bool flag = false;
            try
            {
                outFilePath = GenerateTempFile();               
                var pdfReader = new PdfReader(filename);
                var pages = new List<int>();
                for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                {
                    pages.Add(i);
                }
                pdfReader.SelectPages(pages);

                PdfStamper stp = new PdfStamper(pdfReader, new FileStream(outFilePath, FileMode.Create));
                stp.Close();
                pdfReader.Close();

                flag = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error insertSignature: " + exception.Message);
                flag = false;
            }
            finally
            {

            }
            return flag;
        }

        public static bool SaveNewFileFromReader(byte[] bfile, ref string outFilePath, string ext = ".pdf")
        {
            bool flag = false;
            try
            {
                outFilePath = GenerateTempFile(ext);
                if (ext == ".pdf")
                {
                    using (FileStream fs_ = File.Open(outFilePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        var pdfConcat = new PdfConcatenate(fs_);

                        PdfReader pdfReader = new PdfReader(bfile);
                        var pages = new List<int>();
                        for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                        {
                            pages.Add(i);
                        }
                        pdfReader.SelectPages(pages);
                        pdfConcat.AddPages(pdfReader);

                        pdfReader.Close();
                        pdfConcat.Close();
                    }
                }
                else
                {
                    ByteToFile(bfile, outFilePath);
                }

                flag = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error insertSignature: " + exception.Message);
                flag = false;
            }
            finally
            {

            }
            return flag;
        }


        public static bool SaveNewFileFromReader(Stream sfile, ref string outFilePath)
        {
            bool flag = false;
            try
            {
                outFilePath = GenerateTempFile();

                ByteToFile(StreamToByte(sfile), outFilePath);
                //using (FileStream fs_ = File.Open(outFilePath, FileMode.Create))
                //{
                //    sfile.Position = 0;
                //    var pdfConcat = new PdfConcatenate(fs_);

                //    PdfReader pdfReader = new PdfReader(sfile);
                //    var pages = new List<int>();
                //    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                //    {
                //        pages.Add(i);
                //    }
                //    pdfReader.SelectPages(pages);
                //    pdfConcat.AddPages(pdfReader);

                //    pdfReader.Close();
                //    pdfConcat.Close();


                //    //PdfReader readerTmpn = new PdfReader(sfile);
                //    //using (PdfStamper stam = new PdfStamper(readerTmpn, fs_))
                //    //{
                //    //}
                //    //readerTmpn.Close();
                //}

                flag = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error insertSignature: " + exception.Message);
                try
                {
                    File.Delete(outFilePath);
                }
                catch { }
                flag = false;
            }
            finally
            {

            }
            return flag;
        }

        public static byte[] GetBytes(string str)
        {
            try
            {
                byte[] dst = new byte[str.Length * 2];
                Buffer.BlockCopy(str.ToCharArray(), 0, dst, 0, dst.Length);
                return dst;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error get Byte from string: " + exception.Message);
                return null;
            }
        }

        public static string GetCN(X509Certificate cert)
        {
            try
            {
                return GetCNFromDN(GetSubject(cert));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error getCN: " + exception.Message);
                return null;
            }
        }

        public static string GetCNFromDN(string dn)
        {
            try
            {
                char[] separator = new char[] { ',' };
                string[] strArray = dn.Split(separator);
                string str = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].IndexOf("CN=") != -1)
                    {
                        char[] chArray2 = new char[] { '=' };
                        str = strArray[i].Split(chArray2)[1];
                    }
                }
                //if (str != "")
                //{
                //    return ConvertTVKhongDau(str);
                //}
                return str;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error GetCNFromDN: " + exception.Message);
                return "";
            }
        }

        public static double GetCurrentMilli()
        {
            try
            {
                DateTime time = new DateTime(0x7b2, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan span = (TimeSpan)(DateTime.UtcNow - time);
                return (double)((long)span.TotalMilliseconds);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error get current milli: " + exception.Message);
                return 0.0;
            }
        }

        public static string GetLocation(X509Certificate certificate)
        {
            try
            {
                return GetLocationFromDN(GetSubject(certificate));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error getLocation: " + exception.Message);
                return null;
            }
        }

        public static string GetLocationFromDN(string dn)
        {
            try
            {
                char[] separator = new char[] { ',' };
                string[] strArray = dn.Split(separator);
                string str = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].IndexOf("L=") != -1)
                    {
                        char[] chArray2 = new char[] { '=' };
                        str = strArray[i].Split(chArray2)[1];
                    }
                }
                if (str != "")
                {
                    return ConvertTVKhongDau(str);
                }
                return str;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error GetLocationFromDN: " + exception.Message);
                return "";
            }
        }

        public static string getSignName()
        {
            return (GetCurrentMilli().ToString());
        }

        public static string GetSubject(X509Certificate certificate)
        {
            try
            {
                return certificate.SubjectDN.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error getSubject: " + exception.Message);
                return null;
            }
        }

        public static BaseFont GetBaseFont()
        {
            string TAHOMA_TFF = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");

            //Create a base font object making sure to specify IDENTITY-H
            return BaseFont.CreateFont(TAHOMA_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        }

        internal static SecureString GetSecurePin(string PinCode)
        {
            SecureString pwd = new SecureString();
            foreach (var c in PinCode.ToCharArray()) pwd.AppendChar(c);
            return pwd;
        }

        internal static string GetFileContentHash(string fileContent)
        {
            try
            {
                using (HashAlgorithm hash = HashAlgorithm.Create())
                {
                    byte[] hashByte = hash.ComputeHash(Encoding.UTF8.GetBytes(fileContent));//哈希算法根据文本得到哈希码的字节数组                  
                    string str1 = BitConverter.ToString(hashByte);//将字节数组装换为字符串

                    return (str1).Replace("-", String.Empty);//比较哈希码

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return "";
            //HashAlgorithm hash = HashAlgorithm.Create();
            //FileStream file = new FileStream(filePath, FileMode.Open);
            //byte[] hashByte = hash.ComputeHash(file);//哈希算法根据文本得到哈希码的字节数组
            //string str1 = BitConverter.ToString(hashByte);//将字节数组装换为字符串
            //file.Close();
            //return str1;
        }

        internal static byte[] StreamToByte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        internal static byte[] FileToByte(string input)
        {
            return File.ReadAllBytes(input);
        }

        internal static void ByteToFile(byte[] arrInFile, string saveFile)
        {
            File.WriteAllBytes(saveFile, arrInFile);
        }

        internal static void UpdateimageCell(float widthRectangle, float heightRectangle, Image instance, float SignaltureImageWidth, float widthImagePercent, float plusH)
        {

            float imgTotalWidth = 0;
            if (instance != null)
            {
                //float plusH = ProcessHeightPlus(widthImagePercent, displayConfig);
                float heightRecModPlus = heightRectangle - plusH;
                float heighRectangleModPlus1 = (heightRecModPlus > 0 ? heightRecModPlus : heightRectangle);
                if (instance.Width > widthRectangle || instance.Height > heightRectangle || (instance.Height > heightRecModPlus && heightRecModPlus > 0))
                {
                    float weightImgRealPercentTH1 = (instance.Width > widthRectangle ? widthRectangle / instance.Width : 0);
                    float heightImgRealPercentTH1 = (instance.Height > heighRectangleModPlus1 ? heighRectangleModPlus1 / (instance.Height) : 0);
                    if (weightImgRealPercentTH1 > 0 && heightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * (weightImgRealPercentTH1 < heightImgRealPercentTH1 ? weightImgRealPercentTH1 : heightImgRealPercentTH1) / 100);//80
                    }
                    else if (heightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * heightImgRealPercentTH1 / 100);//80
                    }
                    else if (weightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * weightImgRealPercentTH1 / 100);//80
                    }
                    Inventec.Common.Logging.LogSystem.Info("2__"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => weightImgRealPercentTH1), weightImgRealPercentTH1)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightImgRealPercentTH1), heightImgRealPercentTH1)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => plusH), plusH));
                }
                else
                {
                    float newHeightImagePercent = (((heightRecModPlus > 0 ? heightRecModPlus : heightRectangle)) / instance.Height);
                    imgTotalWidth = (float)(instance.Width * widthImagePercent * newHeightImagePercent / 100);//80
                    Inventec.Common.Logging.LogSystem.Info("3__" +
                        Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => newHeightImagePercent), newHeightImagePercent)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => plusH), plusH));
                }
            }
            else
            {
                imgTotalWidth = (float)(widthRectangle * widthImagePercent / 100);//80
                Inventec.Common.Logging.LogSystem.Info("4__" +
                        Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                       + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => widthImagePercent), widthImagePercent));
            }


            //foreach (IElement element in imageCell.CompositeElements)
            //{
            //    // The inserted image is stored in a PdfPTable, so when you find 
            //    // the table element just set the table width with the image width, and lock it.
            //    PdfPTable tblImg = element as PdfPTable;
            //    if (tblImg != null)
            //    {
            //        tblImg.TotalWidth = imgTotalWidth;
            //        tblImg.LockedWidth = true;

            //        float widthPercentageImage = (imgTotalWidth > widthRectangle ? widthRectangle * 100 / imgTotalWidth : (imgTotalWidth < widthRectangle ? imgTotalWidth * 100 / widthRectangle : 100));
            //        float imgHeightNew = (widthPercentageImage / 100) * instance.Height;
            //        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgHeightNew), imgHeightNew));
            //    }
            //}
        }

        public static float CalculateWidthPercent(float widthRectangle, float heightRectangle, Image instance, float SignaltureImageWidth, float widthImagePercent, float plusH)
        {

            //// thay doi kich thuoc anh cho phu hop voi khung ky
            ////tuong ung voi khung hinh
            //// Now find the Image element in the cell and resize it
            //foreach (IElement element in imageCell.CompositeElements)
            //{
            //    // The inserted image is stored in a PdfPTable, so when you find 
            //    // the table element just set the table width with the image width, and lock it.
            //    PdfPTable tblImg = element as PdfPTable;
            //    if (tblImg != null)
            //    {
            //        tblImg.TotalWidth = imgTotalWidth;
            //        tblImg.LockedWidth = true;

            //        float widthPercentageImage = (imgTotalWidth > widthRectangle ? widthRectangle * 100 / imgTotalWidth : (imgTotalWidth < widthRectangle ? imgTotalWidth * 100 / widthRectangle : 100));
            //        float imgHeightNew = (widthPercentageImage / 100) * instance.Height;
            //        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgHeightNew), imgHeightNew));
            //    }
            //}


            float imgTotalWidth = instance.Width;
            if (instance != null)
            {
                float heightRecModPlus = heightRectangle - plusH;
                float heighRectangleModPlus1 = (heightRecModPlus > 0 ? heightRecModPlus : heightRectangle);
                if (instance.Width > widthRectangle || instance.Height > heightRectangle || (instance.Height > heightRecModPlus && heightRecModPlus > 0))
                {
                    float weightImgRealPercentTH1 = (instance.Width > widthRectangle ? widthRectangle / instance.Width : 0);
                    float heightImgRealPercentTH1 = (instance.Height > heighRectangleModPlus1 ? heighRectangleModPlus1 / (instance.Height) : 0);
                    if (weightImgRealPercentTH1 > 0 && heightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * (weightImgRealPercentTH1 < heightImgRealPercentTH1 ? weightImgRealPercentTH1 : heightImgRealPercentTH1) / 100);//80
                    }
                    else if (heightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * heightImgRealPercentTH1 / 100);//80
                    }
                    else if (weightImgRealPercentTH1 > 0)
                    {
                        imgTotalWidth = (float)(instance.Width * widthImagePercent * weightImgRealPercentTH1 / 100);//80
                    }
                    Inventec.Common.Logging.LogSystem.Info("2__"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => weightImgRealPercentTH1), weightImgRealPercentTH1)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightImgRealPercentTH1), heightImgRealPercentTH1)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => widthRectangle), widthRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightRectangle), heightRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightRecModPlus), heightRecModPlus)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Height", instance.Height)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => plusH), plusH));
                }
                else
                {
                    float newHeightImagePercent = (((heightRecModPlus > 0 ? heightRecModPlus : heightRectangle)) / instance.Height);
                    imgTotalWidth = (float)(instance.Width * widthImagePercent * newHeightImagePercent / 100);//80
                    Inventec.Common.Logging.LogSystem.Info("3__" +
                        Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => newHeightImagePercent), newHeightImagePercent)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => widthRectangle), widthRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightRectangle), heightRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightRecModPlus), heightRecModPlus)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Height", instance.Height)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => plusH), plusH));
                }
            }
            else
            {
                imgTotalWidth = (float)(widthRectangle * widthImagePercent / 100);//80
                Inventec.Common.Logging.LogSystem.Info("4__"
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => widthRectangle), widthRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => heightRectangle), heightRectangle)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => imgTotalWidth), imgTotalWidth)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Height", instance.Height)
                        + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                        + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => widthImagePercent), widthImagePercent));
            }


            float fwidth = widthRectangle;
            float fwidthP = (imgTotalWidth > instance.Width ? instance.Width : imgTotalWidth);
            if (SignaltureImageWidth > 0)
            {
                fwidth = SignaltureImageWidth;
            }
            else if (widthRectangle >= 100)
            {
                fwidth = 100;
                if (fwidthP < 140)
                {
                    float ftemp = 0;
                    ftemp = fwidth;
                    fwidth = fwidthP;
                    fwidthP = ftemp * 2;
                }
            }
            else
            {
                fwidth = widthRectangle;

                if (fwidthP < widthRectangle)
                {
                    float ftemp = 0;
                    ftemp = fwidth;
                    fwidth = fwidthP;
                    fwidthP = ftemp;
                }
            }


            float WidthPercentage = 100 * (fwidth / fwidthP);
            return WidthPercentage;
        }

        internal class CertManager
        {
            // note that both *.pfx location and the password are hardcoded!
            // please customize it in a production code
            private static System.Security.Cryptography.X509Certificates.X509Certificate2 _certificate;
            internal static System.Security.Cryptography.X509Certificates.X509Certificate2 Certificate
            {
                get
                {
                    if (_certificate == null)
                    {
                        using (FileStream fs =
                           File.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3dcb620b-e826-436e-a018-6826b0b4ed7f.pfx"), FileMode.Open))
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            _certificate =
                                new System.Security.Cryptography.X509Certificates.X509Certificate2(
                                   br.ReadBytes((int)br.BaseStream.Length), "@123");
                        }
                    }

                    return _certificate;
                }
            }
        }
    }
}
