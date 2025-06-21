using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Spreadsheet;
using DevExpress.Utils;
using DevExpress.XtraRichEdit;
using Inventec.Common.SignLibrary;

namespace Inventec.Common.Integrate
{
    public class FileConvert
    {
        public static bool ExcelToPdf(MemoryStream inputStream, string inputFile, MemoryStream outputStream, string outputFile)
        {
            try
            {
                if (inputStream != null && inputStream.Length > 0)
                {
                    return ExportExcelToPdfUsingApose(inputStream, outputFile);
                }
                else if (!String.IsNullOrEmpty(inputFile))
                {
                    return ExportExcelToPdfUsingApose(inputFile, outputFile);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return true;
        }

        //public static bool ExcelToPdf__Flexcel(MemoryStream inputStream, string inputFile, MemoryStream outputStream, string outputFile)
        //{
        //    bool success = false;
        //    try
        //    {
        //        FlexCel.Render.FlexCelPdfExport flexCelPdfExport1 = new FlexCel.Render.FlexCelPdfExport();
        //        flexCelPdfExport1.FontEmbed = FlexCel.Pdf.TFontEmbed.Embed;
        //        flexCelPdfExport1.PageLayout = FlexCel.Pdf.TPageLayout.None;
        //        flexCelPdfExport1.PageSize = null;
        //        FlexCel.Pdf.TPdfProperties tPdfProperties1 = new FlexCel.Pdf.TPdfProperties();
        //        tPdfProperties1.Author = null;
        //        tPdfProperties1.Creator = null;
        //        tPdfProperties1.Keywords = null;
        //        tPdfProperties1.Subject = null;
        //        tPdfProperties1.Title = null;
        //        flexCelPdfExport1.Properties = tPdfProperties1;
        //        flexCelPdfExport1.Workbook = new FlexCel.XlsAdapter.XlsFile();
        //        if (inputStream != null && inputStream.Length > 0)
        //        {
        //            inputStream.Position = 0;
        //            flexCelPdfExport1.Workbook.Open(inputStream);
        //        }
        //        else
        //        {
        //            flexCelPdfExport1.Workbook.Open(inputFile);
        //        }

        //        if (flexCelPdfExport1.Workbook == null)
        //        {
        //            System.Windows.Forms.MessageBox.Show("You need to open a file first.");
        //            return success;
        //        }

        //        //if (!LoadPreferencesPdf()) return success;
        //        if (outputStream != null)
        //        {
        //            int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
        //            try
        //            {
        //                flexCelPdfExport1.BeginExport(outputStream);

        //                flexCelPdfExport1.PageLayout = FlexCel.Pdf.TPageLayout.None;
        //                flexCelPdfExport1.ExportSheet();

        //                flexCelPdfExport1.EndExport();
        //            }
        //            finally
        //            {
        //                flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
        //            }
        //        }
        //        if (!String.IsNullOrEmpty(outputFile))
        //        {
        //            using (FileStream Pdf = new FileStream(outputFile, FileMode.OpenOrCreate))
        //            {
        //                int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
        //                try
        //                {
        //                    flexCelPdfExport1.BeginExport(Pdf);

        //                    flexCelPdfExport1.PageLayout = FlexCel.Pdf.TPageLayout.None;
        //                    flexCelPdfExport1.ExportSheet();

        //                    flexCelPdfExport1.EndExport();
        //                }
        //                finally
        //                {
        //                    flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Warn(ex);
        //    }

        //    return success;
        //}

        public static bool ExcelToPdf__Old(string ext, MemoryStream inputStream, string inputFile, MemoryStream outputStream, string outputFile)
        {
            bool success = false;
            try
            {
                if ((inputStream == null || inputStream.Length == 0) && String.IsNullOrEmpty(inputFile))
                    throw new ArgumentNullException("inStream & inFile is null");

                if (outputStream == null && String.IsNullOrEmpty(outputFile))
                {
                    throw new ArgumentNullException("outStream & outFile is null");
                }

                Workbook workbook = new Workbook();
                bool valid = false;
                if (inputStream != null && inputStream.Length > 0)
                {
                    if (ext == ".xls")
                    {
                        valid = workbook.LoadDocument(inputStream, DevExpress.Spreadsheet.DocumentFormat.Xls);
                    }
                    else
                    {
                        valid = workbook.LoadDocument(inputStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    }
                }
                else if (!String.IsNullOrEmpty(inputFile))
                {
                    if (ext == ".xls")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xls);
                        Inventec.Common.Logging.LogSystem.Debug("valid:" + valid + "____inputFile:" + inputFile);
                    }
                    else if (ext == ".xlsm")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xlsm);
                    }
                    else if (ext == ".xltx")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xltx);
                    }
                    else if (ext == ".xlt")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xlt);
                    }
                    else if (ext == ".xltm")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xltm);
                    }
                    else
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    }
                }
                if (valid)
                {
                    if (outputStream != null)
                    {
                        workbook.ExportToPdf(outputStream);
                        outputStream.Position = 0;
                    }
                    if (!String.IsNullOrEmpty(outputFile))
                    {
                        workbook.ExportToPdf(outputFile);
                    }
                    workbook.Dispose();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        public static bool ExportWorkbookToPdf(string workbookPath, string outputPath)
        {
            // If either required string is null or empty, stop and bail out
            if (string.IsNullOrEmpty(workbookPath) || string.IsNullOrEmpty(outputPath))
            {
                return false;
            }

            // Create COM Objects
            Microsoft.Office.Interop.Excel.Application excelApplication;
            Microsoft.Office.Interop.Excel.Workbook excelWorkbook;

            // Create new instance of Excel
            excelApplication = new Microsoft.Office.Interop.Excel.Application();

            // Make the process invisible to the user
            excelApplication.ScreenUpdating = false;

            // Make the process silent
            excelApplication.DisplayAlerts = false;

            // Open the workbook that you wish to export to PDF
            excelWorkbook = excelApplication.Workbooks.Open(workbookPath);

            // If the workbook failed to open, stop, clean up, and bail out
            if (excelWorkbook == null)
            {
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;

                return false;
            }

            var exportSuccessful = true;
            try
            {
                // Call Excel's native export function (valid in Office 2007 and Office 2010, AFAIK)
                excelWorkbook.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, outputPath);
            }
            catch (System.Exception ex)
            {
                // Mark the export as failed for the return value...
                exportSuccessful = false;

                // Do something with any exceptions here, if you wish...
                // MessageBox.Show...        
            }
            finally
            {
                // Close the workbook, quit the Excel, and clean up regardless of the results...
                excelWorkbook.Close();
                excelApplication.Quit();

                excelApplication = null;
                excelWorkbook = null;
            }

            // You can use the following method to automatically open the PDF after export if you wish
            // Make sure that the file actually exists first...
            //if (System.IO.File.Exists(outputPath))
            //{
            //    System.Diagnostics.Process.Start(outputPath);
            //}

            return exportSuccessful;
        }

        public static bool ExportExcelToPdfUsingApose(MemoryStream sourceFile, string pdfFile)
        {
            try
            {
                // If either required string is null or empty, stop and bail out
                if (sourceFile == null || sourceFile.Length == 0 || string.IsNullOrEmpty(pdfFile))
                {
                    return false;
                }

                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAsposeCell();

                // Open the template excel file
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(sourceFile);

                string saveExcelFile = Utils.GenerateTempFileWithin(".xlsx");
                wb.Save(saveExcelFile, Aspose.Cells.SaveFormat.Xlsx);
                // Save the pdf file.
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => saveExcelFile), saveExcelFile));
                wb.Save(pdfFile, Aspose.Cells.SaveFormat.Pdf);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return true;
        }

        public static bool ExportExcelToPdfUsingApose(string sourceFile, string pdfFile)
        {
            try
            {
                // If either required string is null or empty, stop and bail out
                if (sourceFile == null || sourceFile.Length == 0 || string.IsNullOrEmpty(pdfFile))
                {
                    return false;
                }

                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAsposeCell();

                // Open the template excel file
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook(sourceFile);

                // Save the pdf file.
                wb.Save(pdfFile, Aspose.Cells.SaveFormat.Pdf);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return true;
        }

        public static bool CombineMultiExcelFile(List<string> sourceFiles, string combineFile)
        {
            try
            {
                // If either required string is null or empty, stop and bail out
                if (sourceFiles == null || sourceFiles.Count == 0 || string.IsNullOrEmpty(combineFile))
                {
                    return false;
                }
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sourceFiles), sourceFiles));
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAsposeCell();
                                

                Aspose.Cells.Workbook sourceBook1 = null;
                Aspose.Cells.Worksheet rawSheet = null;
                Aspose.Cells.Range rawRange = null;
                int i = 0;
                int TotalRowCount = 0;
                string rawPrintAreaRow = "";
                string rawPrintAreaColumn = "";
                foreach (var itemSource in sourceFiles)
                {
                    if (i == 0)
                    {
                        sourceBook1 = new Aspose.Cells.Workbook(itemSource);
                        rawSheet = sourceBook1.Worksheets[0];
                        rawRange = rawSheet.Cells.MaxDisplayRange;
                        TotalRowCount = rawRange.RowCount;

                        Aspose.Cells.PageSetup pageSetup1 = rawSheet.PageSetup;
                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("pageSetup1.PrintArea", pageSetup1.PrintArea));
                        var sps1 = pageSetup1.PrintArea.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (sps1 != null && sps1.Length > 0)
                        {
                            rawPrintAreaRow = sps1[0];
                            rawPrintAreaColumn = sps1[1];
                        }
                    }
                    if (i > 0)
                    {
                        // Open the second excel file.
                        Aspose.Cells.Workbook sourceBook2 = new Aspose.Cells.Workbook(itemSource);                    

                        Aspose.Cells.Worksheet sourceSheet = sourceBook2.Worksheets[0];
                        Aspose.Cells.Range sourceRange = sourceSheet.Cells.MaxDisplayRange;

                        Aspose.Cells.PageSetup pageSetup2 = sourceSheet.PageSetup;
                        var sps2 = pageSetup2.PrintArea.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (sps2 != null && sps2.Length > 0)
                        {
                            int c = string.Compare(sps2[0], rawPrintAreaRow);
                            if (c < 0)
                            {
                                rawPrintAreaRow = sps2[0];
                            }
                            int d = string.Compare(rawPrintAreaColumn, sps2[1]);
                            if (d < 0)
                                rawPrintAreaColumn = sps2[1];
                        }
                        Aspose.Cells.Range destRange = rawSheet.Cells.CreateRange(rawRange.FirstRow + TotalRowCount, sourceRange.FirstColumn,
                              sourceRange.RowCount, sourceRange.ColumnCount);
                        destRange.Copy(sourceRange);
                        TotalRowCount = sourceRange.RowCount + TotalRowCount;
                    }
                    i++;
                }

                // Obtaining the reference of the PageSetup of the worksheet

                Aspose.Cells.PageSetup pageSetup = rawSheet.PageSetup;
                // Specifying the cells range (from A1 cell to F20 cell) of the print area
                Aspose.Cells.Range saveRange = rawSheet.Cells.MaxDisplayRange;
                var sps = saveRange.ToString().Split(new string[] { "!", "]" }, StringSplitOptions.RemoveEmptyEntries);
                if (sps != null && sps.Length > 0)
                {
                    var sps2 = sps[1].Trim().Replace(" ", "").Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (sps2 != null && sps2.Length > 0)
                    {

                        pageSetup.PrintArea = rawPrintAreaRow + ":" + "";// "A1:F20";
                    }                   
                }

                // Save the excel combined file.
                sourceBook1.Save(combineFile);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return true;
        }

        private string GetSheetArea(string str, bool isNumber)
        {
            string result = "";
            if (!String.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {

                }
            }
            return result;
        }

        public static bool CombineMultiExcelFile(List<MemoryStream> sourceStreams, MemoryStream combineFile)
        {
            try
            {
                // If either required string is null or empty, stop and bail out
                if (sourceStreams == null || sourceStreams.Count == 0 || combineFile == null)
                {
                    return false;
                }

                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAsposeCell();
                Aspose.Cells.Workbook sourceBook1 = null;
                int i = 0;
                foreach (var itemSource in sourceStreams)
                {
                    if (i == 0)
                        sourceBook1 = new Aspose.Cells.Workbook(itemSource);
                    if (i > 0)
                    {
                        // Open the second excel file.
                        Aspose.Cells.Workbook sourceBook2 = new Aspose.Cells.Workbook(itemSource);
                        sourceBook1.Combine(sourceBook2);
                    }
                }

                // Save the excel combined file.
                sourceBook1.Save(combineFile, Aspose.Cells.SaveFormat.Auto);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return true;
        }

        public static bool ExcelToPdf__Old(string ext, MemoryStream inputStream, string inputFile, byte[] inputByte, MemoryStream outputStream, string outputFile)
        {
            bool success = false;
            try
            {
                if ((inputStream == null || inputStream.Length == 0) && (inputByte == null || inputByte.Length == 0) && String.IsNullOrEmpty(inputFile))
                    throw new ArgumentNullException("inStream & inFile is null");

                if (outputStream == null && String.IsNullOrEmpty(outputFile))
                {
                    throw new ArgumentNullException("outStream & outFile is null");
                }

                Workbook workbook = new Workbook();
                bool valid = false;
                if (inputStream != null && inputStream.Length > 0)
                {
                    valid = workbook.LoadDocument(inputStream, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                }
                else if (inputByte != null && inputByte.Length > 0)
                {
                    if (ext == ".xls")
                    {
                        string outfileConvert = Utils.GetFullPathFile(Guid.NewGuid().ToString() + ".xls");
                        //File.Copy(inputFile, outfileConvert);
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.Xls);
                        Inventec.Common.Logging.LogSystem.Info("valid:" + valid + "____outfileConvert:" + outfileConvert);
                    }
                    else if (ext == ".xlsm")
                    {
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.Xlsm);
                    }
                    else if (ext == ".xltx")
                    {
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.Xltx);
                    }
                    else if (ext == ".xlt")
                    {
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.Xlt);
                    }
                    else if (ext == ".xltm")
                    {
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.Xltm);
                    }
                    else
                    {
                        valid = workbook.LoadDocument(inputByte, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    }
                }
                else if (!String.IsNullOrEmpty(inputFile))
                {
                    if (ext == ".xls")
                    {
                        string outfileConvert = Utils.GetFullPathFile(Guid.NewGuid().ToString() + ".xls");
                        File.Copy(inputFile, outfileConvert);
                        valid = workbook.LoadDocument(outfileConvert, DevExpress.Spreadsheet.DocumentFormat.Xls);
                        Inventec.Common.Logging.LogSystem.Info("valid:" + valid + "____outfileConvert:" + outfileConvert);
                    }
                    else if (ext == ".xlsm")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xlsm);
                    }
                    else if (ext == ".xltx")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xltx);
                    }
                    else if (ext == ".xlt")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xlt);
                    }
                    else if (ext == ".xltm")
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.Xltm);
                    }
                    else
                    {
                        valid = workbook.LoadDocument(inputFile, DevExpress.Spreadsheet.DocumentFormat.OpenXml);
                    }
                }
                if (valid)
                {
                    if (outputStream != null)
                    {
                        workbook.ExportToPdf(outputStream);
                        outputStream.Position = 0;
                    }
                    if (!String.IsNullOrEmpty(outputFile))
                    {
                        workbook.ExportToPdf(outputFile);
                    }
                    workbook.Dispose();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        public static bool DocToPdf(MemoryStream inputStream, string inputFile, MemoryStream outputStream, string outputFile, string extension = "")
        {
            bool success = false;
            try
            {
                if ((inputStream == null || inputStream.Length == 0) && String.IsNullOrEmpty(inputFile))
                    throw new ArgumentNullException("inStream & inFile is null");

                if (outputStream == null && String.IsNullOrEmpty(outputFile))
                {
                    throw new ArgumentNullException("outStream is null");
                }
                string ext = "";
                RichEditDocumentServer server = new RichEditDocumentServer();
                if (!String.IsNullOrEmpty(inputFile))
                {
                    ext = Path.GetExtension(inputFile);
                    if (ext == ".doc")
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.Doc);
                    }
                    else if (ext == ".html")
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.Html);
                    }
                    else if (ext == ".mht")
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.Mht);
                    }
                    else if (ext == ".txt")
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.PlainText);
                    }
                    else if (ext == ".rtf")
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
                    }
                    else
                    {
                        server.LoadDocument(inputFile, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                    }
                }
                else
                {
                    ext = extension;
                    Inventec.Common.Logging.LogSystem.Debug("DocToPdf____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ext), ext));
                    if (ext == ".doc")
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.Doc);
                    }
                    else if (ext == ".html")
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.Html);
                    }
                    else if (ext == ".mht")
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.Mht);
                    }
                    else if (ext == ".txt")
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.PlainText);
                    }
                    else if (ext == ".rtf")
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
                    }
                    else
                    {
                        server.LoadDocument(inputStream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                    }
                }

                if (outputStream != null)
                {
                    server.ExportToPdf(outputStream);
                    outputStream.Position = 0;
                }
                if (!String.IsNullOrEmpty(outputFile))
                {
                    DevExpress.XtraPrinting.PdfExportOptions options = new DevExpress.XtraPrinting.PdfExportOptions();
                    options.Compressed = false;
                    options.ImageQuality = DevExpress.XtraPrinting.PdfJpegImageQuality.Highest;
                    using (FileStream pdfFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        server.ExportToPdf(pdfFileStream, options);
                    }
                }

                server.Dispose();
                success = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }
    }
}
