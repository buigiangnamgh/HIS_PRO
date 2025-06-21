using Inventec.Common.Integrate;
using Inventec.Common.SignLibrary.DTO;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary
{
    public class PdfDocumentProcess
    {
        internal static bool ReplaceTextWithGemBox(List<string> replaceKeys, string sourceFile)
        {
            bool success = false;
            try
            {
                // For complete examples and data files, please go to https://github.com/aspose-pdf/Aspose.PDF-for-.NET
                // The path to the documents directory.
                // Open document
                if (replaceKeys != null && replaceKeys.Count > 0 && System.IO.File.Exists(sourceFile))
                {
                    using (var document = GemBox.Pdf.PdfDocument.Load(sourceFile))
                    {
                        int p = 1;
                        foreach (var page in document.Pages)
                        {
                            foreach (var textElement in page.Content.Elements.All()
                            .Where(element => element.ElementType == GemBox.Pdf.Content.PdfContentElementType.Text)
                            .Cast<GemBox.Pdf.Content.PdfTextContent>())
                            {
                                string text = textElement.ToString();
                                var font = textElement.Format.Text.Font;
                                var color = textElement.Format.Fill.Color;
                                var location = textElement.Location;

                                if (replaceKeys.Contains(text))
                                {

                                    string xy = location.X + ":" + location.Y;
                                    //addTextAnnotationToPDF(templatePath, "$1", p, location.X, location.Y, 5, 5);
                                }

                                // Read the text content element's additional information.
                                //Console.WriteLine("Unicode text: {text}");
                                //Console.WriteLine("Font name: {font.Face.Family.Name}");
                                //Console.WriteLine("Font size: {font.Size}");
                                //Console.WriteLine("Font style: {font.Face.Style}");
                                //Console.WriteLine("Font weight: {font.Face.Weight}");

                                //            if (color.TryGetRgb(out double red, out double green, out double blue)){
                                //                Console.WriteLine("Color: Red={red}, Green={green}, Blue={blue}");
                                //}

                                //Console.WriteLine("Location: X={location.X:0.00}, Y={location.Y:0.00}");
                                //Console.WriteLine();
                            }
                            p++;
                        }

                    }




                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceFile);
                    foreach (var txt in replaceKeys)
                    {
                        // Create TextAbsorber object to find all instances of the input search phrase
                        Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(txt);

                        // Accept the absorber for all the pages
                        pdfDocument.Pages.Accept(textFragmentAbsorber);

                        // Get the extracted text fragments
                        Aspose.Pdf.Text.TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;

                        // Loop through the fragments
                        foreach (Aspose.Pdf.Text.TextFragment textFragment in textFragmentCollection)
                        {
                            // Update text and other properties
                            textFragment.Text = "";
                        }
                    }

                    // Save resulting PDF document.
                    pdfDocument.Save(sourceFile);//outFile
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return success;
        }


        internal static bool ReplaceText(List<string> replaceKeys, string sourceFile)
        {
            bool success = false;
            try
            {
                // For complete examples and data files, please go to https://github.com/aspose-pdf/Aspose.PDF-for-.NET
                // The path to the documents directory.
                // Open document
                if (replaceKeys != null && replaceKeys.Count > 0 && System.IO.File.Exists(sourceFile))
                {
                    Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();
                    Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceFile);
                    foreach (var txt in replaceKeys)
                    {
                        // Create TextAbsorber object to find all instances of the input search phrase
                        Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(txt);

                        // Accept the absorber for all the pages
                        pdfDocument.Pages.Accept(textFragmentAbsorber);

                        // Get the extracted text fragments
                        Aspose.Pdf.Text.TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;

                        // Loop through the fragments
                        foreach (Aspose.Pdf.Text.TextFragment textFragment in textFragmentCollection)
                        {
                            // Update text and other properties
                            textFragment.Text = "";
                        }
                    }

                    // Save resulting PDF document.
                    pdfDocument.Save(sourceFile);//outFile
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return success;
        }

        internal static List<ADO.SignPositionADO> GetPositionBySearchKey(string sourceFile, string keySearch)
        {
            List<ADO.SignPositionADO> signPositionAutos = new List<ADO.SignPositionADO>();
            try
            {
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceFile);

                // Create TextAbsorber object to find all instances of the input search phrase
                Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(keySearch);

                // Accept the absorber for all the pages
                pdfDocument.Pages.Accept(textFragmentAbsorber);

                // Get the extracted text fragments
                Aspose.Pdf.Text.TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;
                //int p = 1;
                // Loop through the fragments
                foreach (Aspose.Pdf.Text.TextFragment textFragment in textFragmentCollection)
                {
                    iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                                               (float)textFragment.Rectangle.LLX,
                                               (float)textFragment.Rectangle.LLY,
                                               (float)textFragment.Rectangle.URX,
                                               (float)textFragment.Rectangle.URY
                                           );

                    signPositionAutos.Add(new ADO.SignPositionADO()
                    {
                        PageNUm = textFragment.Page.Number,
                        Text = textFragment.Text,
                        Reactanle = stickyRectangle
                    });
                    //p++;

                    //Console.WriteLine("Text : {0} ", textFragment.Text);
                    //Console.WriteLine("Position : {0} ", textFragment.Position);
                    //Console.WriteLine("XIndent : {0} ", textFragment.Position.XIndent);
                    //Console.WriteLine("YIndent : {0} ", textFragment.Position.YIndent);
                    //Console.WriteLine("Font - Name : {0}", textFragment.TextState.Font.FontName);
                    //Console.WriteLine("Font - IsAccessible : {0} ", textFragment.TextState.Font.IsAccessible);
                    //Console.WriteLine("Font - IsEmbedded : {0} ", textFragment.TextState.Font.IsEmbedded);
                    //Console.WriteLine("Font - IsSubset : {0} ", textFragment.TextState.Font.IsSubset);
                    //Console.WriteLine("Font Size : {0} ", textFragment.TextState.FontSize);
                    //Console.WriteLine("Foreground Color : {0} ", textFragment.TextState.ForegroundColor);
                }




                //GemBox.Pdf.ComponentInfo.SetLicense(GlobalStore.GemBoxPdf__LicKey);
                //using (var document = GemBox.Pdf.PdfDocument.Load(sourceFile))
                //{                    
                //    foreach (var page in document.Pages)
                //    {
                //        foreach (var textElement in page.Content.Elements.All()
                //        .Where(element => element.ElementType == GemBox.Pdf.Content.PdfContentElementType.Text)
                //        .Cast<GemBox.Pdf.Content.PdfTextContent>())
                //        {
                //            string text = textElement.ToString();
                //            var font = textElement.Format.Text.Font;
                //            var color = textElement.Format.Fill.Color;
                //            var location = textElement.Location;

                //            if (text.Contains(keySearch))//"<SINGLE_KEY__COMMENT_SIGN__"
                //            {
                //                iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                //                               (float)location.X,
                //                               (float)location.Y,
                //                               (float)location.X + 2,
                //                               (float)location.Y + 2
                //                           );

                //                signPositionAutos.Add(new ADO.SignPositionADO()
                //                {
                //                    PageNUm = p,
                //                    Text = text,
                //                    Reactanle = stickyRectangle
                //                });                               
                //            }
                //        }
                //        p++;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return signPositionAutos;
        }

        internal static List<ADO.SignPositionADO> GetPositionBySearchKey(Stream sourceStream, string keySearch)
        {
            List<ADO.SignPositionADO> signPositionAutos = new List<ADO.SignPositionADO>();
            try
            {
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceStream);

                // Create TextAbsorber object to find all instances of the input search phrase
                Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(keySearch);

                // Accept the absorber for all the pages
                pdfDocument.Pages.Accept(textFragmentAbsorber);

                // Get the extracted text fragments
                Aspose.Pdf.Text.TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;
                //int p = 1;
                // Loop through the fragments
                foreach (Aspose.Pdf.Text.TextFragment textFragment in textFragmentCollection)
                {
                    iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                                               (float)textFragment.Rectangle.LLX,
                                               (float)textFragment.Rectangle.LLY,
                                               (float)textFragment.Rectangle.URX,
                                               (float)textFragment.Rectangle.URY
                                           );

                    signPositionAutos.Add(new ADO.SignPositionADO()
                    {
                        PageNUm = textFragment.Page.Number,
                        Text = textFragment.Text,
                        Reactanle = stickyRectangle
                    });
                    //p++;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return signPositionAutos;
        }

        internal static void InsertPages(Stream sourceFile, List<Stream> streamListJoin, string desFileJoined)
        {
            var pages = new List<int>();

            Stream currentStream = File.Open(desFileJoined, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            var pdfConcat = new iTextSharp.text.pdf.PdfConcatenate(currentStream);
            iTextSharp.text.pdf.PdfReader reader1 = null;
            if (sourceFile != null && sourceFile.Length > 0)
            {
                reader1 = new iTextSharp.text.pdf.PdfReader(sourceFile);
                for (int i = 0; i <= reader1.NumberOfPages; i++)
                {
                    pages.Add(i);
                }
                reader1.SelectPages(pages);

                pdfConcat.AddPages(reader1);
            }
            if (streamListJoin != null && streamListJoin.Count > 0)
            {
                foreach (var file in streamListJoin)
                {
                    iTextSharp.text.pdf.PdfReader pdfReader = null;
                    pdfReader = new iTextSharp.text.pdf.PdfReader(file);
                    pages = new List<int>();
                    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();
                }
            }
            try
            {
                if (reader1 != null)
                    reader1.Close();
            }
            catch { }

            try
            {
                sourceFile.Close();
            }
            catch { }

            try
            {
                pdfConcat.Close();
            }
            catch { }
        }

        internal static void InsertPage(Stream sourceFile, List<string> fileListJoin, string desFileJoined)
        {
            List<string> joinStreams = new List<string>();
            if (fileListJoin != null && fileListJoin.Count > 0)
            {
                iTextSharp.text.pdf.PdfReader reader1 = new iTextSharp.text.pdf.PdfReader(sourceFile);
                int pageCount = reader1.NumberOfPages;
                iTextSharp.text.Rectangle pageSize = reader1.GetPageSizeWithRotation(reader1.NumberOfPages);
                iTextSharp.text.Rectangle pageSize1 = new iTextSharp.text.Rectangle(pageSize.Left, pageSize.Bottom, pageSize.Right, (pageSize.Bottom + pageSize.Height), pageSize.Rotation);

                foreach (var item in fileListJoin)
                {
                    int lIndex1 = item.LastIndexOf(".");
                    string EXTENSION = item.Substring(lIndex1 > 0 ? lIndex1 + 1 : lIndex1);
                    if (EXTENSION != "pdf")
                    {
                        var stream = FssFileDownload.GetFile(item);
                        stream.Position = 0;
                        string convertTpPdf = Utils.GenerateTempFileWithin();
                        Stream streamConvert = new FileStream(convertTpPdf, FileMode.Create, FileAccess.Write);
                        iTextSharp.text.Document iTextdocument = new iTextSharp.text.Document(pageSize1, 0, 0, 0, 0);
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(iTextdocument, streamConvert);
                        iTextdocument.Open();
                        writer.Open();

                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(stream);
                        if (img.Height > img.Width)
                        {
                            float percentage = 0.0f;
                            percentage = pageSize.Height / img.Height;
                            img.ScalePercent(percentage * 100);
                        }
                        else
                        {
                            float percentage = 0.0f;
                            percentage = pageSize.Width / img.Width;
                            img.ScalePercent(percentage * 100);
                        }
                        iTextdocument.Add(img);
                        iTextdocument.Close();
                        writer.Close();

                        joinStreams.Add(convertTpPdf);
                    }
                    else
                    {
                        var stream = FssFileDownload.GetFile(item);

                        if (stream != null && stream.Length > 0)
                        {
                            stream.Position = 0;
                            string pdfAddFile = Utils.GenerateTempFileWithin();
                            Utils.ByteToFile(Utils.StreamToByte(stream), pdfAddFile);
                            joinStreams.Add(pdfAddFile);
                        }
                        else
                        {
                            Logging.LogSystem.Error("Loi convert va luu tam file pdf tu server fss ve may tram____item=" + item);
                        }
                    }
                }

                Stream currentStream = File.Open(desFileJoined, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                var pdfConcat = new iTextSharp.text.pdf.PdfConcatenate(currentStream);

                var pages = new List<int>();
                for (int i = 0; i <= reader1.NumberOfPages; i++)
                {
                    pages.Add(i);
                }
                reader1.SelectPages(pages);
                pdfConcat.AddPages(reader1);

                foreach (var file in joinStreams)
                {
                    iTextSharp.text.pdf.PdfReader pdfReader = null;
                    pdfReader = new iTextSharp.text.pdf.PdfReader(file);
                    pages = new List<int>();
                    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();
                }

                try
                {
                    reader1.Close();
                }
                catch { }

                try
                {
                    sourceFile.Close();
                }
                catch { }

                try
                {
                    pdfConcat.Close();
                }
                catch { }

                foreach (var file in joinStreams)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
        }

        public static void InsertPageExt(List<string> fileListJoin, string desFileJoined)
        {
            List<string> joinStreams = new List<string>();
            if (fileListJoin != null && fileListJoin.Count > 0)
            {
                var pages = new List<int>();
                Stream currentStream = File.Open(desFileJoined, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                var pdfConcat = new iTextSharp.text.pdf.PdfConcatenate(currentStream);

                foreach (var file in fileListJoin)
                {
                    iTextSharp.text.pdf.PdfReader pdfReader = null;
                    pdfReader = new iTextSharp.text.pdf.PdfReader(file);
                    pages = new List<int>();
                    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();
                }


                try
                {
                    pdfConcat.Close();
                }
                catch { }

                foreach (var file in joinStreams)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
        }

        public static void InsertPageExt(List<MemoryStream> streamListJoin, string desFileJoined)
        {
            List<string> joinStreams = new List<string>();
            if (streamListJoin != null && streamListJoin.Count > 0)
            {
                var pages = new List<int>();
                Stream currentStream = File.Open(desFileJoined, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                var pdfConcat = new iTextSharp.text.pdf.PdfConcatenate(currentStream);

                foreach (var file in streamListJoin)
                {
                    iTextSharp.text.pdf.PdfReader pdfReader = null;
                    pdfReader = new iTextSharp.text.pdf.PdfReader(file);
                    pages = new List<int>();
                    for (int i = 0; i <= pdfReader.NumberOfPages; i++)
                    {
                        pages.Add(i);
                    }
                    pdfReader.SelectPages(pages);
                    pdfConcat.AddPages(pdfReader);
                    pdfReader.Close();
                }


                try
                {
                    pdfConcat.Close();
                }
                catch { }

                foreach (var file in joinStreams)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Process search key with format <SINGLE_KEY__COMMENT_SIGN__$5__d:4|p:5|f:5|w:50|h:30> and add Annotation
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="outFile"></param>
        /// <param name="keySearch"></param>
        /// <returns></returns>
        internal static List<ADO.SignPositionADO> GetPositionWithAutoAddAnnotationBySearchKey(string sourceFile, string outFile, string keySearch)
        {
            List<ADO.SignPositionADO> signPositionAutos = new List<ADO.SignPositionADO>();
            try
            {

                PDFParser pdfParser = new SignLibrary.PDFParser();
                List<string> listKeys = pdfParser.ReadPdfFile(sourceFile, keySearch);
                if (listKeys != null && listKeys.Count > 0)
                {
                    foreach (var itemKey in listKeys)
                    {
                        Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceFile);
                        // Create TextAbsorber object to find all instances of the input search phrase
                        Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(itemKey);
                        // Accept the absorber for all the pages
                        pdfDocument.Pages.Accept(textFragmentAbsorber);
                        // Get the extracted text fragments
                        Aspose.Pdf.Text.TextFragmentCollection textFragmentCollection = textFragmentAbsorber.TextFragments;
                        int p = 1;
                        // Loop through the fragments
                        foreach (Aspose.Pdf.Text.TextFragment textFragment in textFragmentCollection)
                        {
                            iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                                                       (float)textFragment.Position.XIndent,
                                                       (float)textFragment.Position.YIndent,
                                                       (float)textFragment.Position.XIndent + 2,
                                                       (float)textFragment.Position.YIndent + 2
                                                   );
                            p = textFragment.Page.Number;
                            signPositionAutos.Add(new ADO.SignPositionADO()
                            {
                                PageNUm = p,
                                Text = textFragment.Text,
                                Reactanle = stickyRectangle
                            });
                            var arr = textFragment.Text.Split(new string[] { keySearch }, StringSplitOptions.RemoveEmptyEntries);
                            string txtComment = "";
                            if (arr.Length == 1)
                            {
                                txtComment = arr[0];
                            }
                            else if (arr.Length > 1)
                            {
                                txtComment = arr[arr.Length - 1];
                            }
                            txtComment = txtComment.Replace(">", "").Replace("}", "");

                            Utils.AddTextAnnotation(outFile, txtComment, p, textFragment.Position.XIndent, textFragment.Position.YIndent, 2, 2);
                            p++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return signPositionAutos;
        }

        internal static System.Drawing.Printing.PageSettings GetPaperSize(string filePath)
        {
            System.Drawing.Printing.PageSettings pSettings = new System.Drawing.Printing.PageSettings();
            try
            {
                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(filePath);

                pSettings.PaperSize = new System.Drawing.Printing.PaperSize();
                pSettings.PaperSize.Width = (int)(Math.Round((pdfDocument.PageInfo.Width * 100) / 72, 0, MidpointRounding.AwayFromZero));
                pSettings.PaperSize.Height = (int)(Math.Round((pdfDocument.PageInfo.Height * 100) / 72, 0, MidpointRounding.AwayFromZero));
                pSettings.PaperSize.RawKind = (int)System.Drawing.Printing.PaperKind.Custom;               

                Aspose.Pdf.Facades.PdfPageEditor pageEditor = new Aspose.Pdf.Facades.PdfPageEditor();
                pageEditor.BindPdf(filePath);

                if (pageEditor.GetPageSize(1).IsLandscape)
                {
                    pSettings.Landscape = pageEditor.GetPageSize(1).IsLandscape;
                }

                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("pSettings.Landscape", pSettings.Landscape)
                      + Inventec.Common.Logging.LogUtil.TraceData("pdfDocument.PageInfo.Width", pdfDocument.PageInfo.Width)
                      + Inventec.Common.Logging.LogUtil.TraceData("pdfDocument.PageInfo.Height", pdfDocument.PageInfo.Height)
                      + Inventec.Common.Logging.LogUtil.TraceData("pSettings.PaperSize.Width", pSettings.PaperSize.Width)
                      + Inventec.Common.Logging.LogUtil.TraceData("pSettings.PaperSize.Height", pSettings.PaperSize.Height)
                      );
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
            return pSettings;
        }

        internal static void SplitOnePageToImageAndJoinToNewOnePdf(string sourceTempFilePath, float oginalHeight, ref string joinPdfFilePath, List<ImageOfPageDTO> imageFiles = null)
        {
            try
            {
                if (imageFiles == null || imageFiles.Count == 0)
                    imageFiles = ConvertPdfPageToListImage(sourceTempFilePath);
                iTextSharp.text.pdf.PdfReader readerWorking = new iTextSharp.text.pdf.PdfReader(sourceTempFilePath);
                int pageCount = readerWorking.NumberOfPages;
                iTextSharp.text.Rectangle pageSize = readerWorking.GetPageSizeWithRotation(readerWorking.NumberOfPages);
                iTextSharp.text.Rectangle pageSize1 = new iTextSharp.text.Rectangle(pageSize.Left, pageSize.Bottom, pageSize.Right, (pageSize.Bottom + oginalHeight), pageSize.Rotation);
                pageSize1.BorderColor = pageSize.BorderColor;
                pageSize1.BackgroundColor = pageSize.BackgroundColor;
                pageSize1.Rotation = pageSize.Rotation;
                pageSize1.Border = pageSize.Border;
                pageSize1.BorderWidth = pageSize.BorderWidth;
                pageSize1.BorderColor = pageSize.BorderColor;
                pageSize1.BackgroundColor = pageSize.BackgroundColor;
                pageSize1.BorderColorLeft = pageSize.BorderColorLeft;
                pageSize1.BorderColorRight = pageSize.BorderColorRight;
                pageSize1.BorderColorTop = pageSize.BorderColorTop;
                pageSize1.BorderColorBottom = pageSize.BorderColorBottom;
                pageSize1.BorderWidthLeft = pageSize.BorderWidthLeft;
                pageSize1.BorderWidthRight = pageSize.BorderWidthRight;
                pageSize1.BorderWidthTop = pageSize.BorderWidthTop;
                pageSize1.BorderWidthBottom = pageSize.BorderWidthBottom;
                pageSize1.UseVariableBorders = pageSize.UseVariableBorders;

                if (imageFiles != null && imageFiles.Count > 0)
                {
                    joinPdfFilePath = Utils.GenerateTempFileWithin();
                    PdfReader readerWorkingSource = Utils.GetTempReader(pageSize1);
                    using (FileStream fs_ = File.Open(joinPdfFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (PdfStamper stam = new PdfStamper(readerWorkingSource, fs_))
                        {
                            int pageIndex = 1;
                            float currentPageHeight = 0;

                            foreach (var item in imageFiles)
                            {
                                float pageXPosition = 0, pageYPosition = 0;
                                if (currentPageHeight + item.Height > oginalHeight)
                                {
                                    pageIndex += 1;
                                    stam.InsertPage(
                                        pageIndex,
                                        pageSize1
                                    );
                                    currentPageHeight = item.Height;
                                    pageXPosition = 0;
                                    pageYPosition = oginalHeight - currentPageHeight;
                                }
                                else
                                {
                                    currentPageHeight += item.Height;
                                    pageXPosition = 0;
                                    pageYPosition = oginalHeight - currentPageHeight;
                                }

                                float percentage = 0.0f;
                                iTextSharp.text.Image img = !String.IsNullOrEmpty(item.Path) ? iTextSharp.text.Image.GetInstance(item.Path) : iTextSharp.text.Image.GetInstance(item.ImageContent);
                                img.SetAbsolutePosition(pageXPosition, pageYPosition);

                                percentage = pageSize.Width / img.Width;
                                //percentage = pageSize.Height / img.Height;
                                img.ScalePercent(percentage * 100);

                                stam.GetOverContent(pageIndex)
                                   .AddImage(img);

                            }
                        }
                    }
                }
                readerWorking.Close();

                try
                {
                    //if (File.Exists(joinTempFilePath))
                    //    File.Delete(joinTempFilePath);
                    if (imageFiles != null && imageFiles.Count > 0)
                    {
                        int imgCount = imageFiles.Count;
                        for (int i = imgCount - 1; i >= 0; i--)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(imageFiles[i].Path))
                                    File.Delete(imageFiles[i].Path);
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
            }
        }

        internal static List<ImageOfPageDTO> ConvertPdfPageToListImage(string output_file)
        {
            List<ImageOfPageDTO> imageOfPages = new List<ImageOfPageDTO>();
            try
            {
                Inventec.Common.SignLibrary.License.LicenceProcess.SetLicenseForAspose();

                Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(output_file);
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
