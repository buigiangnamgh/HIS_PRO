using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    class ProcessFile
    {

        internal void SplitPdfFileWithKey(Stream stream, ref Stream splitFileContentStream, ref double oginalHeight, ref double oginalWidth)
        {
            try
            {
                var positionHeaders = PdfDocumentProcess.GetPositionBySearchKey(stream, "{SignLibrary.SplitPdfHeaderKey}");
                stream.Position = 0;
                var positionContents = PdfDocumentProcess.GetPositionBySearchKey(stream, "{SignLibrary.SplitPdfContentKey}");
                stream.Position = 0;
                if ((positionHeaders != null && positionHeaders.Count > 0) || (positionContents != null && positionContents.Count > 0))
                {
                    License.LicenceProcess.SetLicenseForAspose();

                    double h1, w1 = 0;
                    using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(stream))
                    {
                        string fullFilepdfForSplit = Utils.GenerateTempFileWithin();
                        pdfDocument.Save(fullFilepdfForSplit);
                        string tempPdfFile = Utils.GenerateTempFileWithin();

                        Aspose.Pdf.Page pageDoc = pdfDocument.Pages[1];
                        h1 = pageDoc.GetPageRect(false).Height;
                        w1 = pageDoc.GetPageRect(false).Width;
                        oginalHeight = pageDoc.GetPageRect(false).Height;
                        oginalWidth = pageDoc.GetPageRect(false).Width;

                        Aspose.Pdf.Page[] pageForAdds;
                        bool isPageCountThan4 = false;

                        double llx, lly, urx, ury = 0;
                        if (positionContents != null && positionContents.Count > 0)
                        {
                            llx = 0;
                            lly = positionContents[0].Reactanle.Top;
                            urx = w1;
                            ury = positionHeaders[0].Reactanle.Top;

                            var pnums = pdfDocument.Pages.Cast<Aspose.Pdf.Page>().Select(o => o.Number).ToList();                         
                            pageForAdds = pdfDocument.Pages.Cast<Aspose.Pdf.Page>().Where(o => o.Number <= positionContents[0].PageNUm).ToArray();
                        }
                        else
                        {
                            llx = 0;
                            lly = 0;
                            urx = w1;
                            ury = positionHeaders[0].Reactanle.Top;

                            pageForAdds = pdfDocument.Pages.Cast<Aspose.Pdf.Page>().Where(o => o.Number >= positionHeaders[0].PageNUm).ToArray();
                        }
                        bool isCropBoxed = false;
                        int pCount = pageForAdds.Count();
                        while (pCount > 0)
                        {
                            using (Aspose.Pdf.Document document = (isPageCountThan4 && File.Exists(tempPdfFile)) ? new Aspose.Pdf.Document(tempPdfFile) : new Aspose.Pdf.Document())
                            {
                                if (pCount < 4)
                                {
                                    document.Pages.Add(pageForAdds);
                                    pCount = 0;
                                    isPageCountThan4 = false;
                                }
                                else
                                {
                                    document.Pages.Add(pageForAdds.Skip(0).Take(3).ToArray());
                                    pageForAdds = pageForAdds.Skip(3).ToArray();
                                    pCount = pageForAdds.Count();
                                    isPageCountThan4 = true;
                                }

                                if (!isCropBoxed)
                                {
                                    Aspose.Pdf.Page page = document.Pages[1];
                                    page.CropBox = new Aspose.Pdf.Rectangle(llx, lly, urx, ury);
                                    isCropBoxed = true;
                                }

                                document.Save(tempPdfFile);
                            }
                        }
                        using (Aspose.Pdf.Document document = new Aspose.Pdf.Document(tempPdfFile))
                        {
                            // Save updated PDF
                            document.Save(splitFileContentStream);
                            splitFileContentStream.Position = 0;
                        }
                        try
                        {
                            File.Delete(tempPdfFile);
                        }
                        catch { }
                    }
                }
                else
                {
                    using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(stream))
                    {
                        Aspose.Pdf.Page page = pdfDocument.Pages[1];

                        oginalHeight = page.GetPageRect(false).Height;
                        oginalWidth = page.GetPageRect(false).Width;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal static List<ImageOfPageDTO> ConvertPdfToImage(string pdf_file)
        {
            List<ImageOfPageDTO> imageOfPages = new List<ImageOfPageDTO>();
            try
            {
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdf_file);
                for (int pageCount = 1; pageCount <= pdfDocument.Pages.Count; pageCount++)
                {
                    string output_filename = string.Format("splitimage{0:d}{1}.jpg", pageCount, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                    string output_filePath = Utils.GetFullPathFile(output_filename);

                    using (FileStream imageStream = new FileStream(output_filePath, FileMode.Create))
                    {
                        // Create JPEG device with specified attributes
                        // Width, Height, Resolution, Quality
                        // Quality [0-100], 100 is Maximum
                        // Create Resolution object
                        Aspose.Pdf.Devices.Resolution resolution = new Aspose.Pdf.Devices.Resolution(300);

                        // JpegDevice jpegDevice = new JpegDevice(500, 700, resolution, 100);
                        Aspose.Pdf.Devices.JpegDevice jpegDevice = new Aspose.Pdf.Devices.JpegDevice(resolution, 100);

                        // Convert a particular page and save the image to stream
                        jpegDevice.Process(pdfDocument.Pages[pageCount], imageStream);

                        // Close stream
                        imageStream.Close();

                        imageOfPages.Add(new ImageOfPageDTO()
                        {
                            Path = output_filePath,
                            PageNumber = pdfDocument.Pages[pageCount].Number,
                            Width = (float)pdfDocument.Pages[pageCount].Rect.Width,
                            Height = (float)pdfDocument.Pages[pageCount].Rect.Height,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                imageOfPages = new List<ImageOfPageDTO>();
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return imageOfPages;
        }

    }
}
