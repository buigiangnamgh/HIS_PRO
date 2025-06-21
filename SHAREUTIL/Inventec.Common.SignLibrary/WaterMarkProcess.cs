using EMR.EFMODEL.DataModels;
using Inventec.Common.SignFile;
using Inventec.Common.SignLibrary.ADO;
using Inventec.Common.SignLibrary.LibraryMessage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Inventec.Common.SignLibrary
{
    internal class WaterMarkProcess
    {
        internal static void ProcessInsertWaterMark(PdfReader readerWorking, string outPathFile, V_EMR_DOCUMENT document, List<EMR_SIGN> signAlls, bool hasSignInformationPage, ref DevExpress.XtraRichEdit.RichEditControl txtSignDescriptionList)
        {
            try
            {
                int pageCount = readerWorking.NumberOfPages;
                int defaultFontSize = 18;
                int wkFontSize = 16;
                using (FileStream fs_ = File.Open(outPathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (PdfStamper stam = new PdfStamper(readerWorking, fs_))
                    {
                        PdfLayer layer = new PdfLayer("watermarkPdfLayer", stam.Writer);

                        var signElectronics = (signAlls != null && signAlls.Count > 0) ? signAlls.Where(o => o.IS_SIGN_ELECTRONIC == 1 && o.COOR_X_RECTANGLE > 0 && o.COOR_Y_RECTANGLE > 0).ToList() : null;
                        if (signElectronics != null && signElectronics.Count > 0)
                        {
                            foreach (var itemsignElectronic in signElectronics)
                            {
                                int pageNum = (int)(itemsignElectronic.PAGE_NUMBER ?? 1);
                                PdfContentByte cbo = stam.GetOverContent(pageNum);
                                cbo.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                                cbo.SetFontAndSize(Utils.GetBaseFont(), 12);
                                cbo.BeginText();
                                if (itemsignElectronic.SIGN_IMAGE != null && itemsignElectronic.SIGN_IMAGE.Count() > 0)
                                {
                                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(itemsignElectronic.SIGN_IMAGE);
                                    image.SetAbsolutePosition((float)itemsignElectronic.COOR_X_RECTANGLE, (float)itemsignElectronic.COOR_Y_RECTANGLE);
                                    var configImage = GetConfigImage();
                                    if (configImage != null && configImage.Count() == 2)
                                    {
                                        //image.ScaleAbsoluteWidth((float)(configImage[0] / 2));
                                        //image.ScaleAbsoluteHeight((float)(configImage[1] / 2));
                                        float SignaltureImageWidth = 0;
                                        image.WidthPercentage = SharedUtils.CalculateWidthPercent(configImage[0], configImage[1], image, SignaltureImageWidth, 100, SignPdfAsynchronous.ProcessHeightPlus(100, configImage[0]));
                                    }
                                    var signStr = !string.IsNullOrEmpty(itemsignElectronic.RELATION_PEOPLE_NAME) ? string.Format("{0}({1})", itemsignElectronic.RELATION_PEOPLE_NAME, itemsignElectronic.RELATION_NAME) : itemsignElectronic.VIR_PATIENT_NAME;
                                    switch (document.PATIENT_SIGNATURE_DISPLAY_TYPE)
                                    {
                                        case 0:
                                            break;
                                        case 1:
                                            cbo.ShowTextAligned(PdfContentByte.ALIGN_CENTER, String.Format(MessageUitl.GetMessage(MessageConstan.ChuKyDienTuBenhNhanDaKy), signStr, ""), (float)itemsignElectronic.COOR_X_RECTANGLE + (image.Width / 4), (float)itemsignElectronic.COOR_Y_RECTANGLE - (image.Height / 2), 0f);
                                            break;
                                        case 2:
                                            cbo.AddImage(image);
                                            break;
                                        default:
                                            cbo.AddImage(image);
                                            cbo.ShowTextAligned(PdfContentByte.ALIGN_CENTER, String.Format(MessageUitl.GetMessage(MessageConstan.ChuKyDienTuBenhNhanDaKy), signStr, ""), (float)itemsignElectronic.COOR_X_RECTANGLE + (image.Width / 4), (float)itemsignElectronic.COOR_Y_RECTANGLE - (image.Height / 2), 0f);
                                            break;
                                    }
                                }
                                else if (itemsignElectronic != null && itemsignElectronic.COOR_X_RECTANGLE.HasValue && itemsignElectronic.COOR_Y_RECTANGLE.HasValue && ((itemsignElectronic.IS_SIGN_BOARD ?? 0) != 1) && itemsignElectronic.IS_SIGN_ELECTRONIC + (hasSignInformationPage ? 1 : 0) == pageNum)
                                {
                                    var signStr = !string.IsNullOrEmpty(itemsignElectronic.RELATION_PEOPLE_NAME) ? string.Format("{0}({1})", itemsignElectronic.RELATION_PEOPLE_NAME, itemsignElectronic.RELATION_NAME) : itemsignElectronic.VIR_PATIENT_NAME;
                                    cbo.ShowTextAligned(PdfContentByte.ALIGN_CENTER, String.Format(MessageUitl.GetMessage(MessageConstan.ChuKyDienTuBenhNhanDaKy), signStr, ""), (float)itemsignElectronic.COOR_X_RECTANGLE, (float)itemsignElectronic.COOR_Y_RECTANGLE, 0f);
                                }

                                cbo.EndText();
                            }
                        }
                        for (int i = 1; i <= pageCount; i++)
                        {
                            iTextSharp.text.Rectangle rec = readerWorking.GetPageSize(i);
                            PdfContentByte cb = stam.GetUnderContent(i);
                            ProcessPdfContentByteInsertWaterMark(i, cb, ref layer, ref txtSignDescriptionList, defaultFontSize, wkFontSize, rec, readerWorking, signAlls, document);
                            PdfContentByte cbOver = stam.GetOverContent(i);
                            ProcessPdfContentByteInsertWaterMark(i, cbOver, ref layer, ref txtSignDescriptionList, defaultFontSize, wkFontSize, rec, readerWorking, signAlls, document);
                        }

                    }
                }
                readerWorking.Close();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }
        private static void ProcessPdfContentByteInsertWaterMark(int i,PdfContentByte cb,ref PdfLayer layer, ref DevExpress.XtraRichEdit.RichEditControl txtSignDescriptionList, int defaultFontSize, int wkFontSize, iTextSharp.text.Rectangle rec, PdfReader readerWorking, List<EMR_SIGN> signAlls, V_EMR_DOCUMENT document)
        {
            try
            {
                cb.BeginLayer(layer);
                cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);
                //add watermark                       
                var gstate = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.3f };
                cb.SetGState(gstate);
                iTextSharp.text.Rectangle realPageSize = readerWorking.GetPageSizeWithRotation(i);

                var ps = rec ?? realPageSize; /*dc.PdfDocument.PageSize is not always correct*/
                var x = (ps.Right + ps.Left) / 2;
                var y = (ps.Bottom + ps.Top) / 2;
                var rh = ((ps.Bottom + ps.Top) / 8) + 10;
                string formatWM = "{0}       {1}       {2}       {3}       {4}       {5}       {6}";
                float stepvm = 40;
                int demvm = 1;
                cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                cb.BeginText();

                demvm = 1;
                bool hasSignDescription = false, hasSignReject = false;
                string vlSignReject = "";

                var signHasDescriptions = (signAlls != null && signAlls.Count > 0) ? signAlls.Where(o => !String.IsNullOrEmpty(o.DESCRIPTION) && o.SIGN_TIME != null).OrderBy(o => o.SIGN_TIME).ToList() : null;
                if (signHasDescriptions != null && signHasDescriptions.Count > 0)
                {
                    hasSignDescription = true;
                    string vlvlSignDescription = "";
                    int demCap1 = 0, demCap2 = 0;
                    foreach (var sd in signHasDescriptions)
                    {
                        demCap1 += 1;
                        var arrescription = sd.DESCRIPTION.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrescription != null && arrescription.Length > 1)
                        {
                            demCap2 = 0;
                            foreach (var strItemDest in arrescription)
                            {
                                demCap2 += 1;
                                vlvlSignDescription += String.Format("{0}", strItemDest);
                                vlvlSignDescription += "<br>";
                                vlvlSignDescription += String.Format("<p align=right><b>{0}</b></p>", sd.USERNAME);
                                if (demCap2 < arrescription.Length || (demCap2 == arrescription.Length && demCap1 < signHasDescriptions.Count))
                                {
                                    vlvlSignDescription += "<br>";
                                }
                            }
                        }
                        else
                        {
                            vlvlSignDescription += String.Format("{0}<br><p align=right><b>{1}</b></p>", sd.DESCRIPTION, sd.USERNAME);
                            if (demCap1 < signHasDescriptions.Count)
                                vlvlSignDescription += "<br>";
                        }
                    }

                    txtSignDescriptionList.HtmlText = vlvlSignDescription;
                }

                var signHasRejects = (signAlls != null && signAlls.Count > 0) ? signAlls.Where(o => o.REJECT_TIME != null && !String.IsNullOrEmpty(o.REJECT_REASON)).ToList() : null;
                if (signHasRejects != null && signHasRejects.Count > 0)
                {
                    hasSignReject = true;
                    foreach (var sd in signHasRejects)
                    {
                        vlSignReject += String.Format("{0} đã từ chối ký, {1}, lý do: {2}    ", sd.USERNAME, Inventec.Common.Integrate.DateTimeConvert.TimeNumberToSystemDateTime(sd.REJECT_TIME ?? 0).Value.ToString("dd/MM/yyyy HH:mm:ss"), sd.REJECT_REASON);
                    }
                    vlSignReject = String.Format(formatWM, vlSignReject, vlSignReject, vlSignReject, vlSignReject, vlSignReject, vlSignReject, vlSignReject);

                }

                if (GlobalStore.PrintUsingWaterMark)
                {
                    string vlViewer = String.Format("{0} - {1} - {2}", GlobalStore.LoginName, GlobalStore.UserName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                    string watermarkValue = GetConfigWatermarkValueOption();
                    if (!String.IsNullOrWhiteSpace(watermarkValue) && document != null)
                    {
                        vlViewer = watermarkValue.Replace("<#DOCUMENT_CODE;>", document.DOCUMENT_CODE)
                        .Replace("<#DOCUMENT_NAME;>", document.DOCUMENT_NAME)
                        .Replace("<#TREATMENT_CODE;>", document.TREATMENT_CODE)
                        .Replace("<#LOGINNAME;>", GlobalStore.LoginName)
                        .Replace("<#USER_NAME;>", GlobalStore.UserName)
                        .Replace("<#TIME_NOW;>", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                        .Replace("<#CREATE_TIME_STR;>", document.CREATE_TIME.HasValue ? Utils.TimeNumberToTimeString(document.CREATE_TIME.Value) : "");
                    }
                    string vlLineViewer = String.Format(formatWM, vlViewer, vlViewer, vlViewer, vlViewer, vlViewer, vlViewer, vlViewer);
                    cb.SetFontAndSize(SharedUtils.GetBaseFont(), 18);
                    cb.SetColorFill(iTextSharp.text.BaseColor.RED);

                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, -2 * rh, 45f);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, -rh, 45f);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 0, 45f);

                    //
                    if (hasSignDescription || hasSignReject)
                    {
                        cb.SetFontAndSize(Utils.GetBaseFont(), wkFontSize);
                        if (hasSignReject)
                        {
                            cb.SetColorFill(iTextSharp.text.BaseColor.DARK_GRAY);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlSignReject, (stepvm * (1 - demvm)) + x, 2 * rh, 45f);
                        }
                    }
                    else
                    {
                        cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                        cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);

                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, rh, 45f);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 2 * rh, 45f);
                    }

                    cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);
                    cb.SetColorFill(iTextSharp.text.BaseColor.RED);

                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 3 * rh, 45f);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 4 * rh, 45f);

                    if (hasSignDescription || hasSignReject)
                    {
                        cb.SetFontAndSize(Utils.GetBaseFont(), wkFontSize);
                        if (hasSignReject)
                        {
                            cb.SetColorFill(iTextSharp.text.BaseColor.DARK_GRAY);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlSignReject, (stepvm * (1 - demvm)) + x, 6 * rh, 45f);
                        }
                    }
                    else
                    {
                        cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                        cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);

                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 5 * rh, 45f);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 6 * rh, 45f);
                    }

                    cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);
                    cb.SetColorFill(iTextSharp.text.BaseColor.RED);

                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 7 * rh, 45f);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 8 * rh, 45f);

                    if (hasSignDescription || hasSignReject)
                    {
                        cb.SetFontAndSize(Utils.GetBaseFont(), wkFontSize);
                        if (hasSignReject)
                        {
                            cb.SetColorFill(iTextSharp.text.BaseColor.DARK_GRAY);
                            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlSignReject, (stepvm * (1 - demvm)) + x, 10 * rh, 45f);
                        }
                    }
                    else
                    {
                        cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                        cb.SetFontAndSize(SharedUtils.GetBaseFont(), defaultFontSize);

                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 9 * rh, 45f);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, vlLineViewer, (stepvm * (1 - demvm)) + x, 10 * rh, 45f);
                    }
                }

                demvm += 1;

                cb.EndText();
                cb.EndLayer();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private static List<int> GetConfigImage()
        {
            List<int> rs = new List<int>();
            try
            {
                var configs = GlobalStore.EmrConfigs;
                var cfgAppearanceOptions = configs.Where(o => o.KEY == EmrConfigKeys.SIGNATURE_APPEARANCE_OPTION);
                var cfgAppearanceOption = cfgAppearanceOptions != null ? cfgAppearanceOptions.FirstOrDefault() : null;
                if (cfgAppearanceOption != null)
                {
                    try
                    {
                        string vlAppearanceOption = !String.IsNullOrEmpty(cfgAppearanceOption.VALUE) ? cfgAppearanceOption.VALUE : cfgAppearanceOption.DEFAULT_VALUE;
                        //vd: p:0|f:11|w:320|h:140
                        var arrOT = vlAppearanceOption.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrOT != null && arrOT.Count() > 0)
                        {
                            foreach (string vop in arrOT)
                            {
                                if (!String.IsNullOrEmpty(vop))
                                {
                                    var arrOTDetail = vop.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (arrOTDetail != null && arrOTDetail.Count() > 1)
                                    {
                                        if (!String.IsNullOrEmpty(arrOTDetail[1]))
                                        {
                                            string k = arrOTDetail[0].ToLower();
                                            switch (k)
                                            {
                                                case "w":
                                                    int w = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                    rs.Add(w);
                                                    break;
                                                case "h":
                                                    int h = Inventec.Common.Integrate.TypeConvertParse.ToInt32(arrOTDetail[1]);
                                                    rs.Add(h);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        Inventec.Common.Logging.LogSystem.Warn(ex1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
            return rs;
        }

        internal static void ProcessNoInsertWaterMark(PdfReader readerWorking, string outPathFile)
        {
            try
            {
                int pageCount = readerWorking.NumberOfPages;

                var pages = new List<int>();
                for (int i = 0; i <= readerWorking.NumberOfPages; i++)
                {
                    pages.Add(i);
                }
                if (String.IsNullOrEmpty(outPathFile))
                    outPathFile = Utils.GenerateTempFileWithin();
                var currentStream = File.Open(outPathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                var pdfConcat = new PdfConcatenate(currentStream);

                readerWorking.SelectPages(pages);
                pdfConcat.AddPages(readerWorking);

                try
                {
                    currentStream.Close();
                }
                catch { }

                try
                {
                    pdfConcat.Close();
                }
                catch { }

                try
                {
                    readerWorking.Close();
                }
                catch { }
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        internal static void ProcessInsertWaterMark(string inPathFile, string outPathFile, V_EMR_DOCUMENT document)
        {
            try
            {
                PdfReader readerWorking = new PdfReader(inPathFile);
                int pageCount = readerWorking.NumberOfPages;
                using (FileStream fs_ = File.Open(outPathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (PdfStamper stam = new PdfStamper(readerWorking, fs_))
                    {
                        PdfLayer layer = new PdfLayer("watermarkPdfLayer", stam.Writer);
                        for (int i = 1; i <= pageCount; i++)
                        {
                            iTextSharp.text.Rectangle rec = readerWorking.GetPageSize(i);
                            PdfContentByte cb = stam.GetUnderContent(i);
                            ProcessPdfContentByte(i,cb, ref layer, document, readerWorking, rec);
                            PdfContentByte cbOver = stam.GetOverContent(i);
                            ProcessPdfContentByte(i, cbOver, ref layer, document, readerWorking, rec);
                        }
                    }
                }
                readerWorking.Close();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        private static void ProcessPdfContentByte(int i, PdfContentByte cb, ref PdfLayer layer, V_EMR_DOCUMENT document, PdfReader readerWorking, iTextSharp.text.Rectangle rec)
        {
            try
            {
                cb.BeginLayer(layer);
                cb.SetFontAndSize(SharedUtils.GetBaseFont(), 18);
                //add watermark                       
                var gstate = new PdfGState { FillOpacity = 0.1f, StrokeOpacity = 0.3f };
                cb.SetGState(gstate);

                iTextSharp.text.Rectangle realPageSize = readerWorking.GetPageSizeWithRotation(i);

                var ps = rec ?? realPageSize; /*dc.PdfDocument.PageSize is not always correct*/
                var x = (ps.Right + ps.Left) / 2;
                var y = (ps.Bottom + ps.Top) / 2;
                var rh = ((ps.Bottom + ps.Top) / 8) + 10;

                string formatWM = "{0}       {1}       {2}       {3}       {4}       {5}       {6}";
                float stepvm = 40;
                int demvm = 1;
                cb.SetColorFill(iTextSharp.text.BaseColor.RED);
                cb.BeginText();

                demvm = 1;

                string vlWM = String.Format("{0} - {1} - {2}", GlobalStore.LoginName, GlobalStore.UserName, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                string watermarkValue = GetConfigWatermarkValueOption();
                if (!String.IsNullOrWhiteSpace(watermarkValue) && document != null)
                {
                    vlWM = watermarkValue.Replace("<#DOCUMENT_CODE;>", document.DOCUMENT_CODE)
                    .Replace("<#DOCUMENT_NAME;>", document.DOCUMENT_NAME)
                    .Replace("<#TREATMENT_CODE;>", document.TREATMENT_CODE)
                    .Replace("<#LOGINNAME;>", GlobalStore.LoginName)
                    .Replace("<#USER_NAME;>", GlobalStore.UserName)
                    .Replace("<#TIME_NOW;>", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                    .Replace("<#CREATE_TIME_STR;>", document.CREATE_TIME.HasValue ? Utils.TimeNumberToTimeString(document.CREATE_TIME.Value) : "");
                }
                string wmt2 = String.Format(formatWM, vlWM, vlWM, vlWM, vlWM, vlWM, vlWM, vlWM);

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, -2 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, -rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 0, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 2 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 3 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 4 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 5 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 6 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 7 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 8 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 9 * rh, 45f);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, wmt2, (stepvm * (1 - demvm)) + x, 10 * rh, 45f);
                demvm += 1;

                cb.EndText();
                cb.EndLayer();
            }
            catch (Exception ex)
            {
                Logging.LogSystem.Warn(ex);
            }
        }

        internal static void ProcessInsertWaterMarkOld(string inPathFile, System.Windows.Forms.ImageList imageList1)
        {
            try
            {
                PdfReader readerWorking = new PdfReader(inPathFile);
                int pageCount = readerWorking.NumberOfPages;
                int signedCount = Utils.GetSignedCount(readerWorking);
                List<ADO.SignPositionADO> signPositionADOs = new List<ADO.SignPositionADO>();

                GemBox.Pdf.ComponentInfo.SetLicense(GlobalStore.GemBoxPdf__LicKey);
                using (var document = GemBox.Pdf.PdfDocument.Load(inPathFile))
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

                            if (text.Contains("<SINGLE_KEY__COMMENT_SIGN__"))// thay "<SINGLE_KEY__COMMENT_SIGN__" bằng key cần gán
                            {
                                iTextSharp.text.Rectangle stickyRectangle = new iTextSharp.text.Rectangle(
                                               (float)location.X,
                                               (float)location.Y,
                                               (float)location.X + 2,
                                               (float)location.Y + 2
                                           );

                                signPositionADOs.Add(new ADO.SignPositionADO()
                                {
                                    PageNUm = p,
                                    Text = text,
                                    Reactanle = stickyRectangle
                                });
                            }
                        }
                        p++;
                    }
                }

                if (signPositionADOs != null && signPositionADOs.Count > 0)
                {
                    using (FileStream fs_ = File.Open(inPathFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (PdfStamper stam = new PdfStamper(readerWorking, fs_))
                        {
                            for (int i = 1; i <= pageCount; i++)
                            {
                                iTextSharp.text.Rectangle rec = readerWorking.GetPageSize(i);
                                PdfContentByte cb = stam.GetOverContent(i);

                                var spInserts = signPositionADOs.Skip(signedCount).ToList();
                                foreach (var sp in spInserts)
                                {
                                    iTextSharp.text.Image chartImg = iTextSharp.text.Image.GetInstance(imageList1.Images[1], iTextSharp.text.BaseColor.YELLOW);//thay imageList1.Images[1] bằng ảnh cần hiển thị
                                    chartImg.ScaleToFit(10f, 10f);
                                    chartImg.SetAbsolutePosition(sp.Reactanle.Left, sp.Reactanle.Bottom);

                                    cb.AddImage(chartImg);
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
        }

        private static string GetConfigWatermarkValueOption()
        {
            string result = null;
            try
            {
                var configs = GlobalStore.EmrConfigs;
                var cfgWatermarkValueOption = configs.Where(o => o.KEY == EmrConfigKeys.WARTERMARK__VALUE_OPTION).FirstOrDefault();
                if (cfgWatermarkValueOption != null)
                {
                    result = !String.IsNullOrEmpty(cfgWatermarkValueOption.VALUE) ? cfgWatermarkValueOption.VALUE : (!String.IsNullOrEmpty(cfgWatermarkValueOption.DEFAULT_VALUE) ? cfgWatermarkValueOption.DEFAULT_VALUE : "");
                }
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
