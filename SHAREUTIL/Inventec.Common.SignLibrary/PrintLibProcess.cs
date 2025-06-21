using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Pdf;
using System.Windows.Forms;
using System.Diagnostics;
namespace Inventec.Common.SignLibrary
{
    internal class PrintLibProcess
    {
        static System.Drawing.Printing.PageSettings currentPageSettings;
        static DevExpress.Pdf.PdfPrinterSettings pdfPrinterSettings = null;
        static PrinterSettings printerSettings;
        static int Width_;
        static int Height_;
        static int printNumberCopies;

        internal static bool SimplePrint(string inputFile, int copyCount = 1, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("SimplePrint.1");
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                // Create PdfViewer object
                Aspose.Pdf.Facades.PdfViewer viewer = new Aspose.Pdf.Facades.PdfViewer();

                // Open input PDF file
                viewer.BindPdf(inputFile);

                // Set attributes for printing
                viewer.AutoResize = true;         // Print the file with adjusted size
                viewer.AutoRotate = true;         // Print the file with adjusted rotation
                viewer.PrintPageDialog = true;   // Do not produce the page number dialog when printing

                // Create objects for printer and page settings and PrintDocument
                System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.PageSettings pgs = new System.Drawing.Printing.PageSettings();
                System.Drawing.Printing.PrintDocument prtdoc = new System.Drawing.Printing.PrintDocument();

                // Set printer name
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputFile);

                System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
                //printDialog.AllowPrintToFile = true;
                printDialog.AllowSomePages = true;
                printDialog.PrinterSettings.MinimumPage = 1;
                printDialog.PrinterSettings.MaximumPage = viewer.PageCount;
                printDialog.PrinterSettings.FromPage = 1;
                printDialog.PrinterSettings.ToPage = viewer.PageCount;

                printDialog.PrinterSettings.Copies = copyCount > 0 ? (short)(copyCount) : (short)1;
                if (!String.IsNullOrEmpty(printerName))
                    printDialog.PrinterSettings.PrinterName = printerName;


                Aspose.Pdf.PageCollection pageCollection = pdfDocument.Pages;
                // Get particular page
                Aspose.Pdf.Page pdfPage = pageCollection[1];


                Aspose.Pdf.Facades.PdfPageEditor pageEditor = new Aspose.Pdf.Facades.PdfPageEditor();
                pageEditor.BindPdf(inputFile);

                ////// Set PageSize (if required)
                int iWidth = (int)(Math.Round(((pdfPage.Rect.Width * 100) / 72), 0, MidpointRounding.AwayFromZero));
                int iHeight = (int)(Math.Round(((pdfPage.Rect.Height * 100) / 72), 0, MidpointRounding.AwayFromZero));

                if (paperSize != null)
                {
                    if (String.IsNullOrEmpty(paperSize.PaperName))
                    {
                        paperSize.PaperName = paperSize.Kind.ToString();
                    }
                    printDialog.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
                }
                else
                {
                    //A4	9	827	1169
                    //DEBUG 2021-07-26 20:39:03,775 [1] - ___iWidth:826______iHeight:583______paperSize:null___
                    //___iWidth:583______iHeight:826______paperSize:null___
                    //A5	11	583	827
                    if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                    {
                        System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                        PaperSizeA5.RawKind = 11;

                        printDialog.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                        printDialog.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = PaperSizeA5.RawKind;

                        if (pageEditor.GetPageSize(1).IsLandscape)
                        {
                            pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                            viewer.AutoRotate = false;
                            viewer.AutoResize = false;
                        }
                    }
                    //else
                    //    pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                }

                if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ps = printDialog.PrinterSettings;

                    if (pageEditor.GetPageSize(1).IsLandscape)
                    {
                        pgs.Landscape = true;
                    }

                    if (ps.DefaultPageSettings.PaperSize != null && ps.DefaultPageSettings.PaperSize.RawKind > 0)
                    {
                        if (String.IsNullOrEmpty(ps.DefaultPageSettings.PaperSize.PaperName))
                        {
                            ps.DefaultPageSettings.PaperSize.PaperName = ps.DefaultPageSettings.PaperSize.Kind.ToString();
                        }
                        pgs.PaperSize = ps.DefaultPageSettings.PaperSize;
                    }
                    else
                    {
                        //A4	9	827	1169
                        //DEBUG 2021-07-26 20:39:03,775 [1] - ___iWidth:826______iHeight:583______paperSize:null___
                        //___iWidth:583______iHeight:826______paperSize:null___
                        //A5	11	583	827
                        if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                        {
                            System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                            PaperSizeA5.RawKind = 11;

                            pgs.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                            pgs.PaperSize.RawKind = PaperSizeA5.RawKind;

                            if (pageEditor.GetPageSize(1).IsLandscape)
                            {
                                pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                                viewer.AutoRotate = false;
                                viewer.AutoResize = false;
                            }
                        }
                        else
                            pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                    }

                    Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("pageEditor.GetPageSize(1)", pageEditor.GetPageSize(1)));

                    pgs.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    // Specify the page size of printout
                    ps.DefaultPageSettings.PaperSize = pgs.PaperSize;

                    viewer.PrintDocumentWithSettings(pgs, ps);
                    // Check the print status
                    if (viewer.PrintStatus != null)
                    {
                        // An exception was thrown
                        if (viewer.PrintStatus is Exception)
                        {
                            Exception ex = viewer.PrintStatus as Exception;
                            // Get exception message
                            Inventec.Common.Logging.LogSystem.Warn("In văn bản lỗi.", ex);
                        }
                    }
                    else
                    {
                        // No errors were found. Printing job has completed successfully
                        Console.WriteLine("printing completed without any issue..");
                        Inventec.Common.Logging.LogSystem.Debug("printing completed without any issue..");
                        success = true;
                    }
                }

                // Close the PDF file after priting
                viewer.Close();
                Inventec.Common.Logging.LogSystem.Info("SimplePrint.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal static bool ExecutePrintNowJob(string inputFile, int copyCount, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null, bool isPrintPageDialog = true)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintNowJob.1");
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                // Create PdfViewer object
                Aspose.Pdf.Facades.PdfViewer viewer = new Aspose.Pdf.Facades.PdfViewer();

                // Open input PDF file
                viewer.BindPdf(inputFile);

                // Set attributes for printing
                viewer.AutoResize = true;         // Print the file with adjusted size
                viewer.AutoRotate = true;         // Print the file with adjusted rotation
                viewer.PrintPageDialog = isPrintPageDialog;   // Do not produce the page number dialog when printing

                // Create objects for printer and page settings and PrintDocument
                System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.PageSettings pgs = new System.Drawing.Printing.PageSettings();
                System.Drawing.Printing.PrintDocument prtdoc = new System.Drawing.Printing.PrintDocument();

                // Set printer name
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputFile);

                Aspose.Pdf.PageCollection pageCollection = pdfDocument.Pages;
                // Get particular page
                Aspose.Pdf.Page pdfPage = pageCollection[1];


                Aspose.Pdf.Facades.PdfPageEditor pageEditor = new Aspose.Pdf.Facades.PdfPageEditor();
                pageEditor.BindPdf(inputFile);

                ////// Set PageSize (if required)
                int iWidth = (int)(Math.Round(((pdfPage.Rect.Width * 100) / 72), 0, MidpointRounding.AwayFromZero));
                int iHeight = (int)(Math.Round(((pdfPage.Rect.Height * 100) / 72), 0, MidpointRounding.AwayFromZero));

                if (pageEditor.GetPageSize(1).IsLandscape)
                {
                    pgs.Landscape = true;
                }

                if (paperSize != null && paperSize.RawKind > 0)
                {
                    if (String.IsNullOrEmpty(paperSize.PaperName))
                    {
                        paperSize.PaperName = paperSize.Kind.ToString();
                    }
                    pgs.PaperSize = paperSize;
                }
                else
                {
                    if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                    {
                        System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                        PaperSizeA5.RawKind = 11;

                        pgs.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                        pgs.PaperSize.RawKind = PaperSizeA5.RawKind;

                        if (pageEditor.GetPageSize(1).IsLandscape)
                        {
                            pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                            viewer.AutoRotate = false;
                            viewer.AutoResize = false;
                        }
                    }
                    else
                        pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                }

                pgs.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                // Specify the page size of printout
                ps.DefaultPageSettings.PaperSize = pgs.PaperSize;

                ps.Copies = copyCount > 0 ? (short)(copyCount) : (short)1;

                viewer.PrintDocumentWithSettings(pgs, ps);
                // Check the print status
                if (viewer.PrintStatus != null)
                {
                    // An exception was thrown
                    if (viewer.PrintStatus is Exception)
                    {
                        Exception ex = viewer.PrintStatus as Exception;
                        // Get exception message
                        Inventec.Common.Logging.LogSystem.Warn("In văn bản lỗi.", ex);
                    }
                }
                else
                {
                    // No errors were found. Printing job has completed successfully
                    Console.WriteLine("printing completed without any issue..");
                    Inventec.Common.Logging.LogSystem.Debug("printing completed without any issue..");
                    success = true;
                }

                // Close the PDF file after priting
                viewer.Close();
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintNowJob.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal static bool SimplePrintDevLib(string inputFile, int copyCount = 1, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("SimplePrintDevLib.1");
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                // Create PdfViewer object
                Aspose.Pdf.Facades.PdfViewer viewer = new Aspose.Pdf.Facades.PdfViewer();

                // Open input PDF file
                viewer.BindPdf(inputFile);

                // Set attributes for printing
                viewer.AutoResize = true;         // Print the file with adjusted size
                viewer.AutoRotate = true;         // Print the file with adjusted rotation
                viewer.PrintPageDialog = true;   // Do not produce the page number dialog when printing

                // Create objects for printer and page settings and PrintDocument
                System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.PageSettings pgs = new System.Drawing.Printing.PageSettings();
                System.Drawing.Printing.PrintDocument prtdoc = new System.Drawing.Printing.PrintDocument();

                // Set printer name
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputFile);

                System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog();
                //printDialog.AllowPrintToFile = true;
                printDialog.AllowSomePages = true;
                printDialog.PrinterSettings.MinimumPage = 1;
                printDialog.PrinterSettings.MaximumPage = viewer.PageCount;
                printDialog.PrinterSettings.FromPage = 1;
                printDialog.PrinterSettings.ToPage = viewer.PageCount;

                printDialog.PrinterSettings.Copies = copyCount > 0 ? (short)(copyCount) : (short)1;
                if (!String.IsNullOrEmpty(printerName))
                    printDialog.PrinterSettings.PrinterName = printerName;


                Aspose.Pdf.PageCollection pageCollection = pdfDocument.Pages;
                // Get particular page
                Aspose.Pdf.Page pdfPage = pageCollection[1];


                Aspose.Pdf.Facades.PdfPageEditor pageEditor = new Aspose.Pdf.Facades.PdfPageEditor();
                pageEditor.BindPdf(inputFile);

                ////// Set PageSize (if required)
                int iWidth = (int)(Math.Round(((pdfPage.Rect.Width * 100) / 72), 0, MidpointRounding.AwayFromZero));
                int iHeight = (int)(Math.Round(((pdfPage.Rect.Height * 100) / 72), 0, MidpointRounding.AwayFromZero));


                if (paperSize != null)
                {
                    if (String.IsNullOrEmpty(paperSize.PaperName))
                    {
                        paperSize.PaperName = paperSize.Kind.ToString();
                    }
                    printDialog.PrinterSettings.DefaultPageSettings.PaperSize = paperSize;
                }
                else
                {
                    //A4	9	827	1169
                    //DEBUG 2021-07-26 20:39:03,775 [1] - ___iWidth:826______iHeight:583______paperSize:null___
                    //___iWidth:583______iHeight:826______paperSize:null___
                    //A5	11	583	827
                    if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                    {
                        System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                        PaperSizeA5.RawKind = 11;

                        printDialog.PrinterSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                        printDialog.PrinterSettings.DefaultPageSettings.PaperSize.RawKind = PaperSizeA5.RawKind;

                        if (pageEditor.GetPageSize(1).IsLandscape)
                        {
                            pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                            viewer.AutoRotate = false;
                            viewer.AutoResize = false;
                        }
                    }
                    //else
                    //    pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                }

                if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ps = printDialog.PrinterSettings;

                    if (pageEditor.GetPageSize(1).IsLandscape)
                    {
                        pgs.Landscape = true;
                    }

                    if (ps.DefaultPageSettings.PaperSize != null && ps.DefaultPageSettings.PaperSize.RawKind > 0)
                    {
                        if (String.IsNullOrEmpty(ps.DefaultPageSettings.PaperSize.PaperName))
                        {
                            ps.DefaultPageSettings.PaperSize.PaperName = ps.DefaultPageSettings.PaperSize.Kind.ToString();
                        }
                        pgs.PaperSize = ps.DefaultPageSettings.PaperSize;
                    }
                    else
                    {
                        //A4	9	827	1169
                        //DEBUG 2021-07-26 20:39:03,775 [1] - ___iWidth:826______iHeight:583______paperSize:null___
                        //___iWidth:583______iHeight:826______paperSize:null___
                        //A5	11	583	827
                        if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                        {
                            System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                            PaperSizeA5.RawKind = 11;

                            pgs.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                            pgs.PaperSize.RawKind = PaperSizeA5.RawKind;

                            if (pageEditor.GetPageSize(1).IsLandscape)
                            {
                                pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                                viewer.AutoRotate = false;
                                viewer.AutoResize = false;
                            }
                        }
                        else
                            pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                    }

                    pgs.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                    // Specify the page size of printout
                    ps.DefaultPageSettings.PaperSize = pgs.PaperSize;

                    DevExpress.XtraPdfViewer.PdfViewer pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
                    pdfViewer1.Name = "pdfViewer1";
                    pdfViewer1.DetachStreamAfterLoadComplete = true;
                    pdfViewer1.LoadDocument(inputFile);


                    ps.PrintToFile = true;

                    // Declare the PDF printer settings.
                    // If required, pass the system settings to the PDF printer settings constructor.
                    DevExpress.Pdf.PdfPrinterSettings pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(ps);
                    // Specify the PDF printer settings.
                    //pdfPrinterSettings.PageOrientation = pgs.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                    //pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;

                    // Specify the PDF printer settings.
                    //pdfPrinterSettings.PageOrientation = PdfPrintPageOrientation.Auto;
                    //if (pgs.Landscape)
                    //{
                    //    pdfPrinterSettings.PageOrientation = PdfPrintPageOrientation.Landscape;
                    //}
                    pdfPrinterSettings.ScaleMode = PdfPrintScaleMode.Fit;

                    pdfPrinterSettings.PageOrientation = pgs.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                   // pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;

                    pdfViewer1.QueryPageSettings += OnQueryPageSettings;
                    pdfViewer1.Print(pdfPrinterSettings);
                    pdfViewer1.QueryPageSettings -= OnQueryPageSettings;
                    success = true;
                }

                // Close the PDF file after priting
                viewer.Close();
                Inventec.Common.Logging.LogSystem.Info("SimplePrintDevLib.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal static bool SimplePrintNowDevLib(string inputFile, int copyCount = 1, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("SimplePrintNowDevLib.1");
                currentPageSettings = PdfDocumentProcess.GetPaperSize(inputFile);

                DevExpress.XtraPdfViewer.PdfViewer pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
                pdfViewer1.Name = "pdfViewer1";
                pdfViewer1.PageSetupDialogShowing += new DevExpress.XtraPdfViewer.PdfPageSetupDialogShowingEventHandler(pdfViewer1_PageSetupDialogShowing);
                pdfViewer1.DetachStreamAfterLoadComplete = true;
                pdfViewer1.LoadDocument(inputFile);
                //pdfViewer1.Print();
                var currentPageSettingData = pdfViewer1.GetPageSize(1);
                printerSettings = new System.Drawing.Printing.PrinterSettings();
                printerSettings.Copies = (short)copyCount;
                printNumberCopies = copyCount;

                if (!String.IsNullOrEmpty(printerName))
                    printerSettings.PrinterName = printerName;
                //printerSettings.DefaultPageSettings.PaperSize = this.currentPageSettings.PaperSize;                    
                //  printerSettings.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", currentPageSettingData.PaperSize.Width, currentPageSettingData.PaperSize.Height);

                if (paperSize != null)
                {
                    printerSettings.DefaultPageSettings.PaperSize = paperSize;
                }
                else
                {
                    if ((Width_ == 582 && Height_ == 826) || ((int)(currentPageSettingData.Width * 100) == 582 && (int)(currentPageSettingData.Height * 100) == 826) || (Width_ == 826 && Height_ == 582) || ((int)(currentPageSettingData.Width * 100) == 826 && (int)(currentPageSettingData.Height * 100) == 582))
                    {
                        IEnumerable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
                        PaperSize sizes = paperSizes.FirstOrDefault<PaperSize>(size => size.Kind == PaperKind.A5);
                        printerSettings.DefaultPageSettings.PaperSize = sizes;
                    }
                    else if ((Width_ == 826 && Height_ == 1169) || (Width_ == 1169 && Height_ == 826) || ((int)(currentPageSettingData.Width * 100) == 826 && (int)(currentPageSettingData.Height * 100) == 1169) || ((int)(currentPageSettingData.Width * 100) == 1169 && (int)(currentPageSettingData.Height * 100) == 826))
                    {
                        IEnumerable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
                        PaperSize sizes = paperSizes.FirstOrDefault<PaperSize>(size => size.Kind == PaperKind.A4);
                        printerSettings.DefaultPageSettings.PaperSize = sizes;
                    }
                    else
                    {
                        printerSettings.DefaultPageSettings.PaperSize = currentPageSettings.PaperSize;
                    }
                }


                // Declare the PDF printer settings.
                // If required, pass the system settings to the PDF printer settings constructor.
                pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(printerSettings);

                // Specify the PDF printer settings.
                pdfPrinterSettings.PageOrientation = currentPageSettings.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => inputFile), inputFile));
                pdfViewer1.QueryPageSettings += OnQueryPageSettings;

                pdfViewer1.Print(pdfPrinterSettings);
                pdfViewer1.QueryPageSettings -= OnQueryPageSettings;

                Inventec.Common.Logging.LogSystem.Info("SimplePrintNowDevLib.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }


        static void pdfViewer1_PageSetupDialogShowing(object sender, DevExpress.XtraPdfViewer.PdfPageSetupDialogShowingEventArgs e)
        {
            try
            {
                e.FormStartPosition = FormStartPosition.CenterScreen;
                int w = 600;
                int h = 400;
                if (Screen.PrimaryScreen != null)
                {
                    w = Screen.PrimaryScreen.WorkingArea.Width > 400 ? Screen.PrimaryScreen.WorkingArea.Width - 400 : 100;
                    h = Screen.PrimaryScreen.WorkingArea.Height > 100 ? Screen.PrimaryScreen.WorkingArea.Height - 100 : 50;
                }
                e.FormSize = new System.Drawing.Size(w, h);

                if (printNumberCopies > 1)
                {
                    e.PrinterSettings.Settings.Copies = (short)printNumberCopies;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        static void OnQueryPageSettings(object sender, DevExpress.Pdf.PdfQueryPageSettingsEventArgs e)
        {
            try
            {
                //'set current print page size to defined parameters  

                Width_ = (int)e.PageSize.Width;
                Height_ = (int)e.PageSize.Height;
                currentPageSettings.PaperSize.Width = (int)e.PageSize.Width;
                currentPageSettings.PaperSize.Height = (int)e.PageSize.Height;
                printerSettings.DefaultPageSettings.PaperSize = currentPageSettings.PaperSize;
                pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(printerSettings);

                // Specify the PDF printer settings.
                pdfPrinterSettings.PageOrientation = currentPageSettings.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        internal static bool ExecutePrintNowJobDevLib(string inputFile, int copyCount, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null, bool isPrintPageDialog = true)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintNowJobDevLib.1");
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                // Create PdfViewer object
                Aspose.Pdf.Facades.PdfViewer viewer = new Aspose.Pdf.Facades.PdfViewer();

                // Open input PDF file
                viewer.BindPdf(inputFile);

                // Set attributes for printing
                viewer.AutoResize = true;         // Print the file with adjusted size
                viewer.AutoRotate = true;         // Print the file with adjusted rotation
                viewer.PrintPageDialog = isPrintPageDialog;   // Do not produce the page number dialog when printing

                // Create objects for printer and page settings and PrintDocument
                System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
                System.Drawing.Printing.PageSettings pgs = new System.Drawing.Printing.PageSettings();
                System.Drawing.Printing.PrintDocument prtdoc = new System.Drawing.Printing.PrintDocument();

                // Set printer name
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inputFile);

                Aspose.Pdf.PageCollection pageCollection = pdfDocument.Pages;
                // Get particular page
                Aspose.Pdf.Page pdfPage = pageCollection[1];


                Aspose.Pdf.Facades.PdfPageEditor pageEditor = new Aspose.Pdf.Facades.PdfPageEditor();
                pageEditor.BindPdf(inputFile);

                ////// Set PageSize (if required)
                int iWidth = (int)(Math.Round(((pdfPage.Rect.Width * 100) / 72), 0, MidpointRounding.AwayFromZero));
                int iHeight = (int)(Math.Round(((pdfPage.Rect.Height * 100) / 72), 0, MidpointRounding.AwayFromZero));

                if (pageEditor.GetPageSize(1).IsLandscape)
                {
                    pgs.Landscape = true;
                }

                if (paperSize != null && paperSize.RawKind > 0)
                {
                    if (String.IsNullOrEmpty(paperSize.PaperName))
                    {
                        paperSize.PaperName = paperSize.Kind.ToString();
                    }
                    pgs.PaperSize = paperSize;
                }
                else
                {
                    if ((iWidth < iHeight && iWidth <= 585 && iHeight <= 827) || (iWidth > iHeight && iWidth <= 827 && iHeight <= 585))
                    {
                        System.Drawing.Printing.PaperSize PaperSizeA5 = new PaperSize();
                        PaperSizeA5.RawKind = 11;

                        pgs.PaperSize = new System.Drawing.Printing.PaperSize(PaperSizeA5.Kind.ToString(), iWidth, iHeight);
                        pgs.PaperSize.RawKind = PaperSizeA5.RawKind;

                        if (pageEditor.GetPageSize(1).IsLandscape)
                        {
                            pageEditor.Alignment = Aspose.Pdf.Facades.AlignmentType.Center;
                            viewer.AutoRotate = false;
                            viewer.AutoResize = false;
                        }
                    }
                    else
                        pgs.PaperSize = new System.Drawing.Printing.PaperSize("Custom", iWidth, iHeight);
                }

                //// Set PageMargins (if required)                               
                pgs.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
                // Specify the page size of printout
                ps.DefaultPageSettings.PaperSize = pgs.PaperSize;
                ps.Copies = copyCount > 0 ? (short)(copyCount) : (short)1;
                DevExpress.XtraPdfViewer.PdfViewer pdfViewer1 = new DevExpress.XtraPdfViewer.PdfViewer();
                pdfViewer1.Name = "pdfViewer1";
                pdfViewer1.DetachStreamAfterLoadComplete = true;
                pdfViewer1.LoadDocument(inputFile);

                // Declare the PDF printer settings.
                // If required, pass the system settings to the PDF printer settings constructor.
                DevExpress.Pdf.PdfPrinterSettings pdfPrinterSettings = new DevExpress.Pdf.PdfPrinterSettings(ps);

                // Specify the PDF printer settings.
                //pdfPrinterSettings.PageOrientation = pgs.Landscape ? DevExpress.Pdf.PdfPrintPageOrientation.Landscape : DevExpress.Pdf.PdfPrintPageOrientation.Portrait;
                //pdfPrinterSettings.ScaleMode = DevExpress.Pdf.PdfPrintScaleMode.ActualSize;

                // Specify the PDF printer settings.
                pdfPrinterSettings.PageOrientation = PdfPrintPageOrientation.Auto;
                if (pgs.Landscape)
                {
                    pdfPrinterSettings.PageOrientation = PdfPrintPageOrientation.Landscape;
                }
                pdfPrinterSettings.ScaleMode = PdfPrintScaleMode.Fit;

                pdfViewer1.Print(pdfPrinterSettings);
                success = true;
                // Close the PDF file after priting
                viewer.Close();
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintNowJobDevLib.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

        internal static bool ValidExistsExecutePrintCallExeService()
        {
            bool valid = false;
            try
            {
                string exeServiceFileName = Path.Combine(Application.StartupPath, @"Integrate\PrintService\HPS.ClientLibrary.exe");
                valid = (!String.IsNullOrEmpty(exeServiceFileName) && File.Exists(exeServiceFileName));
                Inventec.Common.Logging.LogSystem.Info("ValidExistsExecutePrintCallExeService" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => valid), valid) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => exeServiceFileName), exeServiceFileName));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return valid;
        }

        internal static bool ExecutePrintCallExeService(string inputFile, int copyCount, string printerName = "", System.Drawing.Printing.PaperSize paperSize = null, bool isPrintPageDialog = true)
        {
            bool success = false;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintCallExeService.1");
                string cmdLn = "";
                cmdLn += "|InputFile|" + inputFile;
                cmdLn += "|ApplicationCode|" + GlobalStore.appCode;
                cmdLn += "|PrinterName|" + printerName;
                cmdLn += "|CopyCount|" + copyCount;

                Inventec.Common.Logging.LogSystem.Info("cmdLn = " + cmdLn + "____FileName = " + Application.StartupPath + @"\Integrate\PrintService\HPS.ClientLibrary.exe");

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = Application.StartupPath + @"\Integrate\PrintService\HPS.ClientLibrary.exe";
                startInfo.Arguments = "\"" + cmdLn + "\"";
                Process.Start(startInfo);
                Inventec.Common.Logging.LogSystem.Info("ExecutePrintCallExeService.2");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }

            return success;
        }

    }
}
