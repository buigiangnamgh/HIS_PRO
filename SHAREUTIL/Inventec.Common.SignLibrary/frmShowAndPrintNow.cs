using DevExpress.Pdf;
using Inventec.Common.Logging;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.ADO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventec.Common.SignLibrary
{
    public partial class frmShowAndPrintNow : Form
    {
        string outputFile;
        PdfReader readerWorking;
        List<VerifierADO> verifiers;
        InputADO inputADO;
        Stream currentStream;
        short printNumberCopies;
        string printFilePath = "";
        string outputPdfPathTemp;
        string outputPdfPath = "";

        public frmShowAndPrintNow(string outputFile, InputADO inputADO, short printNumberCopies)
        {
            InitializeComponent();
            this.outputFile = outputFile;
            this.inputADO = inputADO;
            this.printNumberCopies = printNumberCopies;
        }

        private void frmShowAndPrintNow1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(this.outputFile))
                {                   
                    this.readerWorking = new PdfReader(this.outputFile);
                    int pageCount = 1;
                    iTextSharp.text.Rectangle pageSize;
                    pageCount = this.readerWorking.NumberOfPages;
                    pageSize = this.readerWorking.GetPageSizeWithRotation(this.readerWorking.NumberOfPages);
                    int totalPageNumber = this.readerWorking.NumberOfPages;

                    outputPdfPathTemp = Utils.GenerateTempFileWithin();
                    outputPdfPath = "";
                    ProcessInsertSignInformationPage(outputPdfPathTemp, ref outputPdfPath, ref pageCount);

                    printFilePath = "";
                    if (!String.IsNullOrEmpty(outputPdfPath) && File.Exists(outputPdfPath))
                    {
                        printFilePath = outputPdfPath;
                    }
                    else
                    {
                        printFilePath = outputFile;
                    }

                    this.pdfViewer1.DetachStreamAfterLoadComplete = true;
                    this.pdfViewer1.LoadDocument(printFilePath);
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private void ProcessInsertSignInformationPage(string outputPdfPathTemp, ref string outputPdfPath, ref int pageCount)
        {
            try
            {
                if (!this.inputADO.IsPrintOnlyContent && this.verifiers != null && this.verifiers.Count > 0)
                {
                    FileStream fsTemp = File.Open(outputPdfPathTemp, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    Document doc = new Document(this.readerWorking.GetPageSizeWithRotation(this.readerWorking.NumberOfPages));
                    PdfWriter writer = PdfWriter.GetInstance(doc, fsTemp);
                    doc.Open();
                    //adding my table
                    PdfPTable t = AddPdfPTable();
                    doc.Add(t);
                    doc.Close();

                    var pages = new List<int>();
                    for (int i = 0; i <= this.readerWorking.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    outputPdfPath = Utils.GenerateTempFileWithin();
                    currentStream = File.Open(outputPdfPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    var pdfConcat = new PdfConcatenate(currentStream);

                    PdfReader pdfReader = null;
                    if (!String.IsNullOrEmpty(this.outputFile))
                    {
                        pdfReader = new PdfReader(this.outputFile);
                    }

                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();

                    pdfReader = new PdfReader(outputPdfPathTemp);
                    pdfReader.SelectPages(new List<int>() { 0, 1 });
                    pdfConcat.AddPages(pdfReader);
                    //pdfReader.Close();

                    try
                    {
                        fsTemp.Close();
                    }
                    catch { }

                    try
                    {
                        pdfReader.Close();
                    }
                    catch { }

                    try
                    {
                        pdfConcat.Close();
                    }
                    catch { }

                    try
                    {
                        this.readerWorking.Close();
                    }
                    catch { }

                    try
                    {
                        if (File.Exists(outputPdfPathTemp))
                            File.Delete(outputPdfPathTemp);
                    }
                    catch { }

                    this.readerWorking = new PdfReader(outputPdfPath);

                    pageCount += 1;
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private PdfPTable AddPdfPTable()
        {
            PdfPTable t = new PdfPTable(7);
            t.SetTotalWidth(new float[] { 7f, 20f, 20f, 15f, 30f, 30f, 30f });

            iTextSharp.text.Font fContentNormal1 = new iTextSharp.text.Font(Utils.GetBaseFont(), 9, iTextSharp.text.Font.NORMAL);

            iTextSharp.text.Font fContentBold1 = new iTextSharp.text.Font(Utils.GetBaseFont(), 9, iTextSharp.text.Font.BOLD);

            //One row added

            PdfPCell cell21 = new PdfPCell();
            cell21.AddElement(new Paragraph("STT", fContentBold1));
            t.AddCell(cell21);

            PdfPCell cell22 = new PdfPCell();
            cell22.AddElement(new Paragraph("Người ký", fContentBold1));
            t.AddCell(cell22);

            PdfPCell cell23 = new PdfPCell();
            cell23.AddElement(new Paragraph("Thời gian ký", fContentBold1));
            t.AddCell(cell23);

            PdfPCell cell24 = new PdfPCell();
            cell24.AddElement(new Paragraph("Hạn CT", fContentBold1));
            t.AddCell(cell24);

            PdfPCell cell25 = new PdfPCell();
            cell25.AddElement(new Paragraph("Đơn vị", fContentBold1));
            t.AddCell(cell25);

            PdfPCell cell25a = new PdfPCell();
            cell25a.AddElement(new Paragraph("Chức danh", fContentBold1));
            t.AddCell(cell25a);

            PdfPCell cell26 = new PdfPCell();
            cell26.AddElement(new Paragraph("Ý kiến của người ký", fContentBold1));
            t.AddCell(cell26);

            int stt = 1;
            if (this.verifiers != null && this.verifiers.Count > 0)
                foreach (var dr in this.verifiers)
                {
                    PdfPCell c = new PdfPCell();
                    c.AddElement(new Chunk((stt.ToString()), fContentNormal1));
                    t.AddCell(c);

                    PdfPCell c1 = new PdfPCell();
                    c1.AddElement(new Chunk((dr.SignerName), fContentNormal1));
                    t.AddCell(c1);

                    PdfPCell c2 = new PdfPCell();
                    c2.AddElement(new Chunk(dr.Date.ToString("dd/MM/yyyy HH:mm:ss"), fContentNormal1));
                    t.AddCell(c2);

                    PdfPCell c3 = new PdfPCell();
                    c3.AddElement(new Chunk((dr.NotAfter.ToString("dd/MM/yyyy")), fContentNormal1));
                    t.AddCell(c3);

                    string donvi = "", chucdanh = "";
                    if (!String.IsNullOrEmpty(dr.Location))
                    {
                        var larr = dr.Location.Split(new string[] { "|" }, StringSplitOptions.None);
                        if (larr.Length == 2)
                        {
                            donvi = larr[0];
                            chucdanh = larr[1];
                        }
                    }

                    PdfPCell c4 = new PdfPCell();
                    c4.AddElement(new Chunk(donvi, fContentNormal1));
                    t.AddCell(c4);

                    PdfPCell c4a = new PdfPCell();
                    c4a.AddElement(new Chunk(chucdanh, fContentNormal1));
                    t.AddCell(c4a);

                    PdfPCell c5 = new PdfPCell();
                    c5.AddElement(new Chunk(dr.Comment, fContentNormal1));
                    t.AddCell(c5);

                    //Add raw data sign
                    stt += 1;
                }
            return t;
        }

        private bool VerifyPdfInputFile(ref string message)
        {
            VerifyPdfFileHandle verifyPdfFile = new VerifyPdfFileHandle();
            try
            {
                this.verifiers = verifyPdfFile.verify(readerWorking).OrderBy(o => o.Date).ToList();
            }
            catch { this.verifiers = null; }

            if (this.verifiers == null)
            {
                message = "File cần xác thực không hợp lệ";
                return false;
            }
            else if (this.verifiers.Count == 0)
            {
                message = "File đã được chọn không tìm thấy chữ ký số";
                return false;
            }

            return true;
        }

        private void bbtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                PrintLibProcess.SimplePrint(this.printFilePath, this.printNumberCopies, this.inputADO.PrinterDefault, this.inputADO.PaperSizeDefault);
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
    }
}
