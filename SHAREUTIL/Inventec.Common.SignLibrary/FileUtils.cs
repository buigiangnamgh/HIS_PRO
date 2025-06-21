using Inventec.Common.Integrate;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.ADO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    public class Utils
    {
        public static byte[] SignPadImageData;
        public static PdfReader GetTempReader(iTextSharp.text.Rectangle pageSize = null)
        {
            var stream = new MemoryStream();
            {
                if (pageSize != null)
                {
                    using (Document document = new Document(pageSize))
                    {
                        PdfWriter.GetInstance(document, stream);
                        document.Open();
                        document.Add(new Phrase("123"));
                    }
                }
                else
                {
                    using (Document document = new Document())
                    {
                        PdfWriter.GetInstance(document, stream);
                        document.Open();
                        document.Add(new Phrase("123"));
                    }
                }
                return new PdfReader(stream.ToArray());
            }
        }

        public static void ProcessClearAllFileInTempFolder()
        {
            try
            {
                string tempFolderParent = Utils.ParentTempFolder();
                string tempFolder = Utils.GenerateTempFolderWithinByDate();
                System.IO.DirectoryInfo di = new DirectoryInfo(tempFolderParent);

                if (Directory.Exists(tempFolderParent))
                {
                    foreach (FileInfo file in di.GetFiles())
                    {
                        try
                        {
                            File.SetAttributes(file.FullName, FileAttributes.Normal);
                            file.Delete();
                        }
                        catch (Exception exx1)
                        {
                            Logging.LogSystem.Warn(exx1);
                        }
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        try
                        {
                            if (dir.FullName != tempFolder)
                            {
                                foreach (FileInfo file in dir.GetFiles())
                                {
                                    try
                                    {
                                        File.SetAttributes(file.FullName, FileAttributes.Normal);
                                        file.Delete();
                                    }
                                    catch (Exception exx1)
                                    {
                                        Inventec.Common.Logging.LogSystem.Warn("Xóa file theo đường dẫn thất bại____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => file.FullName), file.FullName));
                                        Logging.LogSystem.Warn(exx1);
                                    }
                                }
                                try
                                {
                                    dir.Delete();
                                }
                                catch (Exception exx1)
                                {
                                    Inventec.Common.Logging.LogSystem.Warn("Xóa cả folder và các file bên trong theo đường dẫn thất bại____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dir), dir));
                                    Inventec.Common.Logging.LogSystem.Warn(exx1);
                                }
                            }
                        }
                        catch (Exception exx1)
                        {
                            Logging.LogSystem.Warn(exx1);
                        }
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug("ProcessClearAllFileInTempFolder: no clear file in folder,  path " + tempFolderParent + " not exists");
                }
            }
            catch (Exception ex1)
            {
                Logging.LogSystem.Warn(ex1);
            }
        }

        public static string GenerateTempFileWithin()
        {
            try
            {
                string pathFolderTemp = GenerateTempFolderWithin();
                return Path.Combine(pathFolderTemp, Guid.NewGuid().ToString() + ".pdf");
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return "";
            }
        }

        public static string GenerateTempFileWithin(string extention)
        {
            try
            {
                string pathFolderTemp = GenerateTempFolderWithin();
                return Path.Combine(pathFolderTemp, Guid.NewGuid().ToString() + extention);
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return "";
            }
        }

        public static string GenerateTempFolderWithin()
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

        public static string GenerateTempFolderWithinByDate()
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

        public static string SignatureFolder()
        {
            try
            {
                string pathFolderTemp = Path.Combine(Path.Combine(Application.StartupPath, "Img"), "Signature");
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return Application.StartupPath;
            }
        }

        public static string ParentTempFolder()
        {
            try
            {
                string pathFolderTemp = Path.Combine(Application.StartupPath, "temp");
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return Application.StartupPath;
            }
        }

        public static string AppFilePathSignService()
        {
            try
            {
                string pathFolderTemp = Path.Combine(Path.Combine(Path.Combine(Application.StartupPath, "Integrate"), "EMR.SignProcessor"), "EMR.SignProcessor.exe");
                return pathFolderTemp;
            }
            catch (IOException exception)
            {
                Inventec.Common.Logging.LogSystem.Warn("Error create temp file: " + exception.Message);
                return "";
            }
        }

        internal static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        internal static string Base64Encode(string dataEncode)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dataEncode));
        }

        public static string GetFullPathFile(string filename)
        {
            return Path.Combine(GenerateTempFolderWithin(), filename);
        }

        internal static BaseFont GetBaseFont()
        {
            string TAHOMA_TFF = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");

            //Create a base font object making sure to specify IDENTITY-H
            return BaseFont.CreateFont(TAHOMA_TFF, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        }

        public static byte[] StreamToByte(Stream input)
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

        public static byte[] FileToByte(string input)
        {
            return File.ReadAllBytes(input);
        }

        public static string FileToBase64String(string input)
        {
            return System.Convert.ToBase64String(File.ReadAllBytes(input));
        }

        public static void ByteToFile(byte[] arrInFile, string saveFile)
        {
            try
            {
                File.WriteAllBytes(saveFile, arrInFile);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn("File gui sang khong phai dinh dang file pdf, he thong core ky phai convert sang pdf, nhung khong convert duoc, co the do khong co quyen do folder dang chay khong duoc gan quyen doc ghi, can kiem tra lai");
                Inventec.Common.Logging.LogSystem.Warn(ex);
                MessageBox.Show("Folder đang chạy không được cấp quyền đọc ghi, vui lòng kiểm tra lại");
            }
        }

        internal static byte[] ImageToByte(System.Drawing.Bitmap img)
        {
            try
            {
                System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
                return (byte[])converter.ConvertTo(img, typeof(byte[]));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return null;
        }

        public static void ProcessFileInput(string inputFile, string ext, ref string inputFileWork, string documentTypeCode = "")
        {
            if (ext == ".pdf" || ext == ".xml" || ext == ".json")
            {
                inputFileWork = inputFile;
            }
            else if (ext == ".xlsx")
            {
                inputFileWork = GenerateTempFileWithin();
                FileConvert.ExcelToPdf(null, inputFile, null, inputFileWork);
            }
            else if (ext == ".xls")
            {
                //FIx với loại văn bản là tờ điều trị thì gọi hàm convert sử dụng excel office(TH này chỉ xảy ra khi tích hợp với HIS1), với các văn bản khác sử dụng thư viện Apose để convert
                if (!String.IsNullOrEmpty(documentTypeCode) && documentTypeCode == "07")
                {
                    inputFileWork = GenerateTempFileWithin();
                    bool convertExcelToPdf__Old = FileConvert.ExcelToPdf__Old(ext, null, inputFile, null, inputFileWork);
                    if (!convertExcelToPdf__Old)
                    {
                        Inventec.Common.Logging.LogSystem.Info("ExcelToPdf__Old fail");
                    }

                    try
                    {
                        if (File.Exists(inputFile))
                        {
                            File.Delete(inputFile);
                        }
                    }
                    catch { }
                }
                else
                {
                    bool convertExcelToPdf__Old = FileConvert.ExportExcelToPdfUsingApose(inputFile, inputFileWork);
                    if (!convertExcelToPdf__Old)
                    {
                        Inventec.Common.Logging.LogSystem.Info("ExportExcelToPdfUsingApose fail");
                    }
                    else
                    {
                        try
                        {
                            if (File.Exists(inputFile))
                            {
                                File.Delete(inputFile);
                            }
                        }
                        catch { }
                    }
                }
            }
            else if (ext == ".rdlc")
            {
                //TODO
                inputFileWork = Utils.ConvertRdlcToPdf(inputFile);
            }
            else
            {
                inputFileWork = GenerateTempFileWithin();
                FileConvert.DocToPdf(null, inputFile, null, inputFileWork);
            }
        }

        public static void ProcessFileInput(byte[] inputByte, string ext, ref string inputFileWork, string documentTypeCode = "")
        {
            inputFileWork = GenerateTempFileWithin();
            if (ext == ".pdf" || ext == ".xml" || ext == ".json")
            {
                File.WriteAllBytes(inputFileWork, inputByte);
            }
            else if (ext == ".xlsx")
            {
                using (MemoryStream ms = new MemoryStream(inputByte))
                {
                    ms.Position = 0;
                    FileConvert.ExcelToPdf(ms, "", null, inputFileWork);
                    DisposeStream(ms);
                }
            }
            else if (ext == ".xls")
            {
                //FIx với loại văn bản là tờ điều trị thì gọi hàm convert sử dụng excel office(TH này chỉ xảy ra khi tích hợp với HIS1), với các văn bản khác sử dụng thư viện Apose để convert
                if (!String.IsNullOrEmpty(documentTypeCode) && documentTypeCode == "07")
                {
                    using (MemoryStream ms = new MemoryStream(inputByte))
                    {
                        ms.Position = 0;
                        bool convertExcelToPdf__Old = FileConvert.ExcelToPdf__Old(ext, ms, "", null, inputFileWork);
                        if (!convertExcelToPdf__Old)
                        {
                            Inventec.Common.Logging.LogSystem.Info("ExcelToPdf__Old fail");
                        }
                        DisposeStream(ms);
                    }
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(inputByte))
                    {
                        ms.Position = 0;
                        bool convertExcelToPdf__Old = FileConvert.ExportExcelToPdfUsingApose(ms, inputFileWork);
                        if (!convertExcelToPdf__Old)
                        {
                            Inventec.Common.Logging.LogSystem.Info("convertExcelToPdf__Old fail");
                        }
                        DisposeStream(ms);
                    }
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(inputByte))
                {
                    ms.Position = 0;
                    FileConvert.DocToPdf(ms, "", null, inputFileWork, ext);
                    DisposeStream(ms);
                }
            }
        }

        public static void DisposeStream(Stream stream)
        {
            try
            {
                if (stream != null)
                {
                    try
                    {
                        //stream.Flush();
                        stream.Close();
                        stream.Dispose();
                    }
                    catch { }
                }
                stream = null;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        public static string GetExtByFileType(FileType fileType)
        {
            string ext = "";

            switch (fileType)
            {
                case FileType.Pdf:
                    ext = ".pdf";
                    break;
                case FileType.Xls:
                    ext = ".xls";
                    break;
                case FileType.Xlsx:
                    ext = ".xlsx";
                    break;
                case FileType.Rdlc:
                    ext = ".rdlc";
                    break;
                case FileType.Doc:
                    ext = ".doc";
                    break;
                case FileType.Docx:
                    ext = ".docx";
                    break;
                case FileType.Html:
                    ext = ".html";
                    break;
                case FileType.Rtf:
                    ext = ".rtf";
                    break;
                case FileType.Xml:
                    ext = ".xml";
                    break;
                case FileType.Json:
                    ext = ".json";
                    break;
                default:
                    ext = ".pdf";
                    break;
            }

            return ext;
        }

        internal static string ConvertRdlcToPdf(string rdlcFile)
        {
            string outPdfFile = GenerateTempFileWithin();
            Microsoft.Reporting.WinForms.Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            Microsoft.Reporting.WinForms.ReportViewer reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();

            string outFP = "";
            reportViewer.LocalReport.ReportPath = rdlcFile;
            byte[] bytes = reportViewer.LocalReport.Render(
               "PDF", null, out mimeType, out encoding, out outFP,
               out streamIds, out warnings);

            using (FileStream fs = new FileStream(outPdfFile, FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            return outPdfFile;
        }

        // Add annotation to PDF using iTextSharp
        internal static void AddTextAnnotation(string filePath, string contents, int pageNum, double x, double y, int width, int height)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamp = null;

            try
            {
                using (var inStream = new FileStream(filePath, FileMode.Open))
                {
                    pdfReader = new PdfReader(inStream);
                }

                using (var outStream = new FileStream(filePath, FileMode.Create))
                {
                    pdfStamp = new PdfStamper(pdfReader, outStream, (char)0, true);

                    var rect = new iTextSharp.text.Rectangle((float)x, (float)y, (float)x + width, (float)y + height);

                    // Generating the annotation's appearance using a TextField
                    TextField textField = new TextField(pdfStamp.Writer, rect, null);
                    textField.Text = contents;
                    textField.FontSize = 8;
                    textField.TextColor = BaseColor.DARK_GRAY;
                    textField.BackgroundColor = new BaseColor(System.Drawing.Color.LightGoldenrodYellow);
                    textField.BorderColor = new BaseColor(System.Drawing.Color.BurlyWood);
                    textField.Options = TextField.MULTILINE;
                    textField.SetExtraMargin(2f, 2f);
                    textField.Alignment = Element.ALIGN_TOP | Element.ALIGN_LEFT;
                    PdfAppearance appearance = textField.GetAppearance();

                    // Create the annotation
                    PdfAnnotation annotation = PdfAnnotation.CreateFreeText(pdfStamp.Writer, rect, null, new PdfContentByte(null));
                    annotation.SetAppearance(PdfName.N, appearance);
                    annotation.Flags = PdfAnnotation.FLAGS_READONLY | PdfAnnotation.FLAGS_LOCKED | PdfAnnotation.FLAGS_PRINT;
                    annotation.Put(PdfName.NM, new PdfString(Guid.NewGuid().ToString()));
                    annotation.Put(PdfName.CONTENTS, new PdfString(contents));
                    // Add annotation to PDF
                    pdfStamp.AddAnnotation(annotation, pageNum);
                    pdfStamp.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add signature image to PDF with error: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm lấy các tọa độ các điểm được đánh dấu ký tại đó thông qua việc lấy các comment tương ứng ở các tọa độ đó, mỗi comment phải gán text theo chuẩn: ${Số thứ tự}. Vd: $1, $2,.... Các comment này được tạo sẵn trong template excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static List<SignPositionADO> GetPdfSignPosition(PdfReader reader)
        {
            List<SignPositionADO> signPositions = new List<SignPositionADO>();
            try
            {
                for (int numbPage = 1; numbPage <= reader.NumberOfPages; numbPage++)
                {
                    PdfDictionary pageDict = reader.GetPageN(numbPage);
                    PdfArray annots = pageDict.GetAsArray(PdfName.ANNOTS);
                    Inventec.Common.Logging.LogSystem.Debug("annots.Size=" + (annots != null ? annots.Size : 0));
                    if (annots == null || annots.Size == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < annots.Size; i++)
                    {
                        PdfDictionary sticky = annots.GetAsDict(i);
                        PdfName name = sticky != null ? sticky.GetAsName(PdfName.SUBTYPE) : null;
                        if (name != null && name.Equals(PdfName.TEXT))// Khi tao stick note tu file pdf -> loai note trong file se la TEXT
                        {
                            //Text String
                            String textString =
                               sticky.GetAsString(PdfName.CONTENTS).ToString();

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = textString
                            };

                            signPositions.Add(signPosition);
                        }
                        else if (name != null && name.Equals(PdfName.SQUARE))//Khi tao note tu file excel sau do convert sang file pdf -> trong file pdf phan note do se la loai SQUARE
                        {
                            //Author:\n1
                            String textString =
                              sticky.GetAsString(PdfName.CONTENTS).ToString();

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            var arr = textString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string txt = "";
                            if (arr.Length == 1)
                            {
                                txt = arr[0];
                            }
                            else if (arr.Length > 1)
                            {
                                txt = arr[arr.Length - 1];
                            }

                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = txt
                            };

                            signPositions.Add(signPosition);
                        }
                        else if (name != null && name.Equals(PdfName.FREETEXT))//{/FreeText}
                        {
                            PdfString s2 = sticky.GetAsString(PdfName.CONTENTS);
                            String textString =
                                s2 != null ? s2.ToUnicodeString() : "";
                            String textString1 =
                                s2 != null ? s2.ToString() : "";

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = !String.IsNullOrEmpty(textString) ? textString : textString1
                            };

                            signPositions.Add(signPosition);
                        }
                    }
                }
                signPositions = signPositions.Where(o => o.Text.StartsWith("$")).OrderBy(o => o.Text).ToList();
                foreach (var sp in signPositions)
                {
                    var arrSps = sp.Text.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
                    if (arrSps != null && arrSps.Count() > 1)
                    {
                        //$1__d:4|p:5|f:7|w:100|h:40
                        var arrSp1s = arrSps[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrSp1s != null && arrSp1s.Count() > 0)
                        {
                            foreach (var itemDT in arrSp1s)
                            {
                                var arrSp2s = itemDT.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                if (arrSp2s != null && arrSp2s.Count() > 1)
                                {
                                    if (arrSp2s[0] == "d")
                                    {
                                        sp.TypeDisplay = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrSp2s[1]);
                                    }
                                    else if (arrSp2s[0] == "p")
                                    {
                                        sp.TextPosition = (Inventec.Common.SignFile.Constans.TEXT_POSITON)(Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrSp2s[1]));
                                    }
                                    else if (arrSp2s[0] == "f")
                                    {
                                        sp.SizeFont = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrSp2s[1]);
                                    }
                                    else if (arrSp2s[0] == "w")
                                    {
                                        sp.WidthRectangle = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrSp2s[1]);
                                    }
                                    else if (arrSp2s[0] == "h")
                                    {
                                        sp.HeightRectangle = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrSp2s[1]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }

            return signPositions;
        }

        /// <summary>
        /// Hàm lấy các tọa độ các điểm được đánh dấu ký của bệnh nhân thông qua việc lấy các comment tương ứng ở các tọa độ đó, mỗi comment phải gán text theo chuẩn: #@!@#PATIENT..... Các comment này được tạo sẵn trong template excel
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        internal static List<SignPositionADO> GetPdfPatientSignPosition(PdfReader reader)
        {
            List<SignPositionADO> signPositions = new List<SignPositionADO>();
            try
            {
                for (int numbPage = 1; numbPage <= reader.NumberOfPages; numbPage++)
                {
                    PdfDictionary pageDict = reader.GetPageN(numbPage);
                    PdfArray annots = pageDict.GetAsArray(PdfName.ANNOTS);
                    Inventec.Common.Logging.LogSystem.Debug("annots.Size=" + (annots != null ? annots.Size : 0));
                    if (annots == null || annots.Size == 0)
                    {
                        continue;
                    }
                    for (int i = 0; i < annots.Size; i++)
                    {
                        PdfDictionary sticky = annots.GetAsDict(i);
                        PdfName name = sticky != null ? sticky.GetAsName(PdfName.SUBTYPE) : null;
                        if (name != null && name.Equals(PdfName.TEXT))// Khi tao stick note tu file pdf -> loai note trong file se la TEXT
                        {
                            //Text String
                            String textString =
                               sticky.GetAsString(PdfName.CONTENTS).ToString();

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = textString
                            };

                            signPositions.Add(signPosition);
                        }
                        else if (name != null && name.Equals(PdfName.SQUARE))//Khi tao note tu file excel sau do convert sang file pdf -> trong file pdf phan note do se la loai SQUARE
                        {
                            //Author:\n1
                            String textString =
                              sticky.GetAsString(PdfName.CONTENTS).ToString();

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            var arr = textString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string txt = "";
                            if (arr.Length == 1)
                            {
                                txt = arr[0];
                            }
                            else if (arr.Length > 1)
                            {
                                txt = arr[arr.Length - 1];
                            }

                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = txt
                            };

                            signPositions.Add(signPosition);
                        }
                        else if (name != null && name.Equals(PdfName.FREETEXT))//{/FreeText}
                        {
                            PdfString s2 = sticky.GetAsString(PdfName.CONTENTS);
                            String textString =
                                s2 != null ? s2.ToUnicodeString() : "";
                            String textString1 =
                                s2 != null ? s2.ToString() : "";

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = !String.IsNullOrEmpty(textString) ? textString : textString1
                            };

                            signPositions.Add(signPosition);
                        }
                        else if (name != null && name.Equals(PdfName.WIDGET))//{/FreeText}
                        {
                            PdfString s2 = sticky.GetAsString(PdfName.CONTENTS);
                            String textString =
                                s2 != null ? s2.ToUnicodeString() : "";
                            String textString1 =
                                s2 != null ? s2.ToString() : "";

                            //Layas toa do
                            PdfArray stickyRect = sticky.GetAsArray(PdfName.RECT);
                            Rectangle stickyRectangle = new Rectangle(
                                stickyRect.GetAsNumber(0).FloatValue, stickyRect.GetAsNumber(1).FloatValue,
                                stickyRect.GetAsNumber(2).FloatValue, stickyRect.GetAsNumber(3).FloatValue
                            );
                            //Xoa annot
                            //annots.Remove(i);
                            SignPositionADO signPosition = new SignPositionADO
                            {
                                PageNUm = numbPage,
                                Reactanle = stickyRectangle,
                                Text = !String.IsNullOrEmpty(textString) ? textString : textString1
                            };

                            signPositions.Add(signPosition);
                        }
                    }
                }
                signPositions = signPositions.Where(o => o.Text.Equals("#@!@#PATIENT")).ToList();             
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }

            return signPositions;
        }
        internal static int GetSignedCount(PdfReader reader)
        {
            AcroFields af = reader.AcroFields;
            List<String> names = af.GetSignatureNames();
            if (names == null || names.Count == 0)
            {
                return 0;
            }

            return names.Count;
        }

        public static string TimeNumberToTimeString(long time)
        {
            string result = null;
            try
            {
                string temp = time.ToString();
                if (temp != null && temp.Length >= 14)
                {
                    result = new StringBuilder().Append(temp.Substring(6, 2)).Append("/").Append(temp.Substring(4, 2)).Append("/").Append(temp.Substring(0, 4)).Append(" ").Append(temp.Substring(8, 2)).Append(":").Append(temp.Substring(10, 2)).Append(":").Append(temp.Substring(12, 2)).ToString();
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public static long? GetTimeNow()
        {
            long? result = null;
            try
            {
                result = Int64.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
