using Inventec.Common.SignLibrary.DTO;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.SignHandler
{
    public class PdfSplitFileWithKeyProcess
    {
        public PdfSplitFileWithKeyProcess()
        {
        }

        internal void SplitPdfFileWithKey(Stream stream, ref Stream splitFileHeaderStream, ref Stream splitFileContentStream, ref double oginalHeight, bool? isSplitHeaderKey = true, bool? isSplitContentKey = true)
        {
            try
            {
                var positionHeaders = (isSplitHeaderKey == null || isSplitHeaderKey == true) ? PdfDocumentProcess.GetPositionBySearchKey(stream, GlobalStore.SplitPdfHeaderKey) : null;
                stream.Position = 0;
                var positionContents = (isSplitContentKey == null || isSplitContentKey == true) ? PdfDocumentProcess.GetPositionBySearchKey(stream, GlobalStore.SplitPdfContentKey) : null;
                stream.Position = 0;
                if ((positionHeaders != null && positionHeaders.Count > 0) || (positionContents != null && positionContents.Count > 0))
                {
                    Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                    double h1, w1 = 0;
                    using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(stream))
                    {
                        string fullFilepdfForSplit = Utils.GenerateTempFileWithin();
                        pdfDocument.Save(fullFilepdfForSplit);                     
                        if ((isSplitHeaderKey == null || isSplitHeaderKey == true) && positionHeaders != null && positionHeaders.Count > 0)
                        {                          
                            using (Aspose.Pdf.Document document = new Aspose.Pdf.Document())
                            {
                                document.Pages.Add(pdfDocument.Pages.Cast<Aspose.Pdf.Page>().Where(o => o.Number <= positionHeaders[0].PageNUm).ToArray());

                                // Add page
                                Aspose.Pdf.Page page = document.Pages[positionHeaders[0].PageNUm];

                                h1 = page.GetPageRect(false).Height;
                                w1 = page.GetPageRect(false).Width;
                                oginalHeight = page.GetPageRect(false).Height;

                                page.CropBox = new Aspose.Pdf.Rectangle(0, positionHeaders[0].Reactanle.Top, w1, h1);

                                string tempPdfFileHeader = Utils.GenerateTempFileWithin();
                                // Save updated PDF
                                document.Save(splitFileHeaderStream);
                                document.Save(tempPdfFileHeader);
                                splitFileHeaderStream.Position = 0;
                            }
                        }                       

                        if (isSplitContentKey == null || isSplitContentKey == true)
                        {
                            string tempPdfFile = Utils.GenerateTempFileWithin();

                            Aspose.Pdf.Page pageDoc = pdfDocument.Pages[1];
                            h1 = pageDoc.GetPageRect(false).Height;
                            w1 = pageDoc.GetPageRect(false).Width;
                            oginalHeight = pageDoc.GetPageRect(false).Height;

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
                                        if (positionHeaders != null && positionHeaders.Count > 0 && positionHeaders[0].PageNUm < positionContents[0].PageNUm)
                                        {
                                            // Crop page header
                                            Aspose.Pdf.Page pageHeader = document.Pages[positionHeaders[0].PageNUm];
                                            pageHeader.CropBox = new Aspose.Pdf.Rectangle(0, 0, w1, positionHeaders[0].Reactanle.Top);

                                            //Crop page content
                                            Aspose.Pdf.Page page = document.Pages[positionContents[0].PageNUm];
                                            page.CropBox = new Aspose.Pdf.Rectangle(0, positionContents[0].Reactanle.Top, w1, h1);
                                        }
                                        else
                                        {
                                            //Crop page content
                                            Aspose.Pdf.Page page = document.Pages[positionContents[0].PageNUm];
                                            page.CropBox = new Aspose.Pdf.Rectangle(llx, lly, urx, ury);
                                        }

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
                                //File.Delete(tempPdfFile);//TODO                              
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(stream))
                    {
                        Aspose.Pdf.Page page = pdfDocument.Pages[1];

                        oginalHeight = page.GetPageRect(false).Height;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }


        void splitIntoHalfPages(string sourceFile)
        {
            PdfReader reader = new PdfReader(sourceFile);

            try
            {
                MemoryStream targetStream = new MemoryStream();
                iTextSharp.text.Document document = new iTextSharp.text.Document();
                PdfCopy copy = new PdfCopy(document, targetStream);
                document.Open();

                for (int page = 1; page <= reader.NumberOfPages; page++)
                {
                    float margin = 1;
                    PdfDictionary pageN = reader.GetPageN(page);
                    iTextSharp.text.Rectangle cropBox = reader.GetCropBox(page);
                    PdfArray leftBox = new PdfArray(new float[] { cropBox.GetLeft(margin), cropBox.GetBottom(margin), (cropBox.GetLeft(margin) + cropBox.GetRight(margin)) / 2.0f, cropBox.GetTop(margin) });
                    PdfArray rightBox = new PdfArray(new float[] { (cropBox.GetLeft(margin) + cropBox.GetRight(margin)) / 2.0f, cropBox.GetBottom(margin), cropBox.GetRight(margin), cropBox.GetTop(margin) });

                    PdfImportedPage importedPage = copy.GetImportedPage(reader, page);
                    pageN.Put(PdfName.CROPBOX, leftBox);
                    copy.AddPage(importedPage);
                    pageN.Put(PdfName.CROPBOX, rightBox);
                    copy.AddPage(importedPage);
                }

                document.Close();
            }
            finally
            {
                reader.Close();
            }
        }

        public void JoinPartialPdfFile(decimal oginalHeight, FileDataDTO headerData, List<FileDataDTO> contentDatas, ref string outputPdfFile)
        {
            try
            {
                string joinTempFilePath = Utils.GenerateTempFileWithin();
                string joinPdfFilePath = Utils.GenerateTempFileWithin();
                string splitFileHeaderPath = Utils.GenerateTempFileWithin();
                string splitFileContentPath = Utils.GenerateTempFileWithin();
                List<string> imageFiles = new List<string>();
                //int totalPageCount = 0;

                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();              
                Stream splitFileHeaderStream = new MemoryStream();
                Stream splitFileContentStream = new MemoryStream();
                double _oginalHeight = 0;

                if (headerData != null && headerData.Stream != null && headerData.Stream.Length > 0)
                {
                    SplitPdfFileWithKey(headerData.Stream, ref splitFileHeaderStream, ref splitFileContentStream, ref _oginalHeight, true, false);
                }

                List<Stream> streamContens = new List<Stream>();

                using (Aspose.Pdf.Document new_docTempJoin = new Aspose.Pdf.Document())
                {                 
                    for (int i = 0; i < contentDatas.Count; i++)
                    {
                        Stream splitFileHeaderStreamCt = new MemoryStream();
                        Stream splitFileContentStreamCt = new MemoryStream();
                        SplitPdfFileWithKey(contentDatas[i].Stream, ref splitFileHeaderStreamCt, ref splitFileContentStreamCt, ref _oginalHeight);
                        if (splitFileContentStreamCt != null && splitFileContentStreamCt.Length > 0)
                        {
                            splitFileContentStreamCt.Position = 0;

                            streamContens.Add(splitFileContentStreamCt);                          
                        }
                        else
                        {
                            contentDatas[i].Stream.Position = 0;
                            streamContens.Add(contentDatas[i].Stream);
                        }
                    }
                }

                oginalHeight = (decimal)_oginalHeight;

                PdfDocumentProcess.InsertPages(splitFileHeaderStream, streamContens, joinTempFilePath);
                PdfDocumentProcess.SplitOnePageToImageAndJoinToNewOnePdf(joinTempFilePath, (float)oginalHeight, ref joinPdfFilePath);             
                if (!String.IsNullOrEmpty(joinPdfFilePath) && File.Exists(joinPdfFilePath))
                {
                    outputPdfFile = joinPdfFilePath;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }
    }
}
