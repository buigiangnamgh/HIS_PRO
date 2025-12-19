using com.itextpdf.text.pdf.security;
using Inventec.Common.Logging;
using iTextSharp.text;
using iTextSharp.text.io;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;

namespace Inventec.Common.SignFile
{
    public class SignPdfAsynchronous
    {
        // Fields
        private const string CRYPT_ALG = "RSA";

        private static string hashAlgorithmCfg;
        internal static string HASH_ALG
        {
            get
            {
                if (hashAlgorithmCfg == null)
                {
                    try
                    {
                        hashAlgorithmCfg = System.Configuration.ConfigurationManager.AppSettings["Inventec.Common.SignFile.Hash_Algorithm"];
                        if (String.IsNullOrWhiteSpace(hashAlgorithmCfg))
                        {
                            hashAlgorithmCfg = "SHA1";
                        }
                    }
                    catch (Exception ex)
                    {
                        Inventec.Common.Logging.LogSystem.Error(ex);
                        hashAlgorithmCfg = "SHA1";
                    }
                }

                return hashAlgorithmCfg;
            }
        }

        // Methods
        public List<byte[]> CreateHash(string inFile, string tempFile, string fileName, X509Certificate[] chain, DisplayConfig displayConfig)
        {
            if (!this.EmptySignature(inFile, tempFile, fileName, displayConfig, chain[0]))
            {
                return null;
            }
            return this.PreSign(tempFile, fileName, chain, displayConfig.SignDate);
        }

        internal static void ProcessFontSizeFit(DisplayConfig displayConfig)
        {
            int newSizeFont = 0;
            if (displayConfig.WidthRectangle > 0)
            {
                if (displayConfig.WidthRectangle <= 30 && displayConfig.SizeFont >= 2)
                {
                    newSizeFont = 1;
                }
                if (displayConfig.WidthRectangle <= 40 && displayConfig.SizeFont >= 3)
                {
                    newSizeFont = 2;
                }
                else if (displayConfig.WidthRectangle <= 60 && displayConfig.SizeFont >= 4)
                {
                    newSizeFont = 3;
                }
                else if (displayConfig.WidthRectangle <= 80 && displayConfig.SizeFont >= 5)
                {
                    newSizeFont = 4;
                }
                else if (displayConfig.WidthRectangle <= 120 && displayConfig.SizeFont >= 6)
                {
                    newSizeFont = 5;
                }
                else if (displayConfig.WidthRectangle <= 160 && displayConfig.SizeFont >= 7)
                {
                    newSizeFont = 6;
                }
            }
            if (displayConfig.SizeFont != newSizeFont && newSizeFont > 0)
            {
                displayConfig.SizeFont = newSizeFont;
                Inventec.Common.Logging.LogSystem.Debug("Kiem tra SizeFont cua vung chu ky, neu do rong cua vung ky duoc cau hinh  khong phu hop voi SizeFont thi tu dong dieu chinh cho phu hop____" +
                    Inventec.Common.Logging.LogUtil.TraceData("oldSizeFont", displayConfig.SizeFont) +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => newSizeFont), newSizeFont));
            }
        }

        public static float ProcessHeightPlus(float widthImagePercent, DisplayConfig displayConfig)
        {
            float data = 0;
            if (widthImagePercent == 100)
            {
                if (displayConfig.WidthRectangle <= 30)
                {
                    data = 5;
                }
                if (displayConfig.WidthRectangle <= 40)
                {
                    data = 5;
                }
                else if (displayConfig.WidthRectangle <= 60)
                {
                    data = 10;
                }
                else if (displayConfig.WidthRectangle <= 80)
                {
                    data = 15;
                }
                else if (displayConfig.WidthRectangle <= 100)
                {
                    data = 25;
                }
                else if (displayConfig.WidthRectangle <= 120)
                {
                    data = 30;
                }
                else
                {
                    data = 40;
                }
            }
            return data;
        }

        public static float ProcessHeightPlus(float widthImagePercent, float WidthRectangle)
        {
            float data = 0;
            if (widthImagePercent == 100)
            {
                if (WidthRectangle <= 30)
                {
                    data = 5;
                }
                if (WidthRectangle <= 40)
                {
                    data = 5;
                }
                else if (WidthRectangle <= 60)
                {
                    data = 10;
                }
                else if (WidthRectangle <= 80)
                {
                    data = 15;
                }
                else if (WidthRectangle <= 100)
                {
                    data = 25;
                }
                else if (WidthRectangle <= 120)
                {
                    data = 30;
                }
                else
                {
                    data = 40;
                }
            }
            return data;
        }

        public bool EmptySignature(string inFile, string outFile, string fieldName, DisplayConfig displayConfig, X509Certificate cert)
        {
            PdfReader reader = null;
            FileStream os = null;
            bool success;
            try
            {
                reader = new PdfReader(inFile);
                int numberOfPages = reader.NumberOfPages;
                int numberPageSign = displayConfig.NumberPageSign;
                if ((numberPageSign < 1) || (numberPageSign > numberOfPages))
                {
                    numberPageSign = 1;
                }
                os = new FileStream(outFile, FileMode.Create);

                bool IsRebuilt = reader.IsRebuilt();
                Inventec.Common.Logging.LogSystem.Debug("EmptySignature: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => IsRebuilt), IsRebuilt));

                PdfSignatureAppearance signatureAppearance = null;
                if (IsRebuilt)
                {
                    reader.Catalog.Remove(PdfName.PERMS);//TODO
                    reader.RemoveUsageRights();//TODO

                    signatureAppearance = PdfStamper.CreateSignature(reader, os, '\0', null).SignatureAppearance;
                }
                else
                    signatureAppearance = PdfStamper.CreateSignature(reader, os, '\0', null, true).SignatureAppearance;

                DateTime signDate = displayConfig.SignDate;
                if ("".Equals(displayConfig.Contact))
                {
                    displayConfig.Contact = SharedUtils.GetCN(cert);
                }
                signatureAppearance.Contact = displayConfig.Contact;
                signatureAppearance.SignDate = signDate;
                signatureAppearance.Reason = displayConfig.Reason;
                signatureAppearance.Location = displayConfig.Location;
                signatureAppearance.Certificate = cert;//mới thêm dòng này
                string strDate = string.Format(displayConfig.DateFormatstring, signDate);
                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig));
                if (displayConfig.IsDisplaySignature)
                {
                    float coorXRectangle = displayConfig.CoorXRectangle - (displayConfig.WidthRectangle / 2);
                    float coorYRectangle = displayConfig.CoorYRectangle - (displayConfig.HeightRectangle / 2);
                    float widthRectangle = displayConfig.WidthRectangle;
                    float heightRectangle = displayConfig.HeightRectangle;

                    ProcessFontSizeFit(displayConfig);

                    float wPlus = 0, hPlus = 0;
                    Image instance = null;
                    if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
                    {
                        if (!String.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
                        {
                            instance = Image.GetInstance(displayConfig.PathImage);
                        }
                        else if (displayConfig.BImage != null)
                        {
                            instance = Image.GetInstance(displayConfig.BImage);
                        }
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
                    {
                        if (!String.IsNullOrEmpty(displayConfig.PathImage) && File.Exists(displayConfig.PathImage))
                        {
                            instance = Image.GetInstance(displayConfig.PathImage);
                        }
                        else if (displayConfig.BImage != null)
                        {
                            instance = Image.GetInstance(displayConfig.BImage);
                        }
                    }

                    if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
                    {
                        Rectangle pageRect = new Rectangle(coorXRectangle, coorYRectangle, coorXRectangle + widthRectangle + wPlus, coorYRectangle + heightRectangle + hPlus);
                        signatureAppearance.SetVisibleSignature(pageRect, numberPageSign, fieldName);
                    }
                    else
                    {
                        signatureAppearance.SetVisibleSignature(fieldName);
                    }

                    if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP)
                    {
                        float width = widthRectangle;

                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText column = new ColumnText(layer);
                        PdfPCell imageCell = new PdfPCell();
                        if (instance != null)
                        {
                            instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                            float plusH = SignPdfAsynchronous.ProcessHeightPlus(100, displayConfig);
                            instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, 100, plusH);

                            imageCell.AddElement(instance);
                            imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            imageCell.Border = Rectangle.NO_BORDER;
                            imageCell.MinimumHeight = heightRectangle;
                            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("instance.WidthPercentage", instance.WidthPercentage)
                                + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                                + Inventec.Common.Logging.LogUtil.TraceData("displayConfig.SignaltureImageWidth", displayConfig.SignaltureImageWidth));
                        }

                        PdfPTable newtable = new PdfPTable(1);
                        newtable.TotalWidth = width;
                        newtable.LockedWidth = true;

                        newtable.AddCell(imageCell);

                        column.AddElement(newtable);
                        column.SetSimpleColumn(left, bottom, urx, ury);
                        column.Alignment = Element.ALIGN_CENTER;
                        column.Go();
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_IMAGE_STAMP_WITH_TEXT)
                    {
                        //float width = widthRectangle;

                        string displayText = GetDisplayText(displayConfig, strDate);

                        float widthImagePercent = 0;
                        PdfPTable newtable = null;
                        if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100)
                        {
                            newtable = new PdfPTable(1);
                            widthImagePercent = 100;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75)
                        {
                            newtable = new PdfPTable(new float[] { 25, 75 });
                            widthImagePercent = 25;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70)
                        {
                            newtable = new PdfPTable(new float[] { 30, 70 });
                            widthImagePercent = 30;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60)
                        {
                            newtable = new PdfPTable(new float[] { 40, 60 });
                            widthImagePercent = 40;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50)
                        {
                            newtable = new PdfPTable(new float[] { 50, 50 });
                            widthImagePercent = 50;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x60x40)
                        {
                            newtable = new PdfPTable(new float[] { 60, 40 });
                            widthImagePercent = 40;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x70x30)
                        {
                            newtable = new PdfPTable(new float[] { 70, 30 });
                            widthImagePercent = 30;
                        }
                        else if (displayConfig.TextPosition == Constans.TEXT_POSITON.x75x25)
                        {
                            newtable = new PdfPTable(new float[] { 75, 25 });
                            widthImagePercent = 25;
                        }

                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText column = new ColumnText(layer);
                        PdfPCell imageCell = new PdfPCell();
                        if (instance != null)
                        {
                            instance.Alignment = iTextSharp.text.Image.ALIGN_CENTER;

                            float plusH = SignPdfAsynchronous.ProcessHeightPlus(widthImagePercent, displayConfig);
                            instance.WidthPercentage = SharedUtils.CalculateWidthPercent(widthRectangle, heightRectangle, instance, displayConfig.SignaltureImageWidth, widthImagePercent, plusH);

                            imageCell.AddElement(instance);
                            imageCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            imageCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                            imageCell.Border = Rectangle.NO_BORDER;

                            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("instance.WidthPercentage", instance.WidthPercentage)
                                 + Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance.Width)
                                 + Inventec.Common.Logging.LogUtil.TraceData("displayConfig.SignaltureImageWidth", displayConfig.SignaltureImageWidth));

                        }
                        PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);

                        newtable.TotalWidth = widthRectangle;
                        newtable.LockedWidth = true;

                        if (displayConfig.TextPosition == Constans.TEXT_POSITON.x100
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x25x75
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x30x70
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x40x60
                            || displayConfig.TextPosition == Constans.TEXT_POSITON.x50x50
                            )
                        {
                            newtable.AddCell(imageCell);
                            newtable.AddCell(textCell);
                        }
                        else
                        {
                            newtable.AddCell(textCell);
                            newtable.AddCell(imageCell);
                        }

                        PdfPTable newtableParent = new PdfPTable(1);
                        PdfPCell imageCellParent = new PdfPCell();

                        imageCellParent.AddElement(newtable);
                        imageCellParent.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        imageCellParent.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        imageCellParent.Border = Rectangle.NO_BORDER;
                        imageCellParent.MinimumHeight = heightRectangle;

                        newtableParent.TotalWidth = widthRectangle;
                        newtableParent.LockedWidth = true;
                        newtableParent.AddCell(imageCellParent);

                        column.AddElement(newtableParent);
                        column.SetSimpleColumn(left, bottom, urx, ury);
                        column.Alignment = Element.ALIGN_CENTER;
                        column.Go();

                        Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData("instance.Width", instance != null ? instance.Width : 0)
                            + Inventec.Common.Logging.LogUtil.TraceData("instance.Height", instance != null ? instance.Height : 0)
                            + Inventec.Common.Logging.LogUtil.TraceData("widthRectangle", widthRectangle)
                            + Inventec.Common.Logging.LogUtil.TraceData("heightRectangle", heightRectangle));
                    }
                    else if (displayConfig.TypeDisplay == Constans.DISPLAY_RECTANGLE_TEXT)
                    {
                        PdfTemplate layer = signatureAppearance.GetLayer(2);
                        float left = layer.BoundingBox.Left;
                        float bottom = layer.BoundingBox.Bottom;
                        float urx = layer.BoundingBox.Width;
                        float ury = layer.BoundingBox.Height;
                        ColumnText text = new ColumnText(layer);
                        text.SetSimpleColumn(left, bottom, urx, ury);
                        text.Alignment = Element.ALIGN_MIDDLE;
                        string displayText = GetDisplayText(displayConfig, strDate);
                        PdfPCell textCell = SignPdfFile.GetTextCell(displayText, displayConfig);
                        textCell.MinimumHeight = heightRectangle;

                        PdfPTable newtable = new PdfPTable(1);
                        newtable.TotalWidth = widthRectangle;
                        newtable.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                        newtable.LockedWidth = true;


                        newtable.AddCell(textCell);
                        newtable.CompleteRow();

                        text.AddElement(newtable);
                        text.Go();
                    }


                    Inventec.Common.Logging.LogSystem.Info("EmptySignature____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig.TypeDisplay), displayConfig.TypeDisplay));
                }
                else if (displayConfig.SignType == Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD)
                {
                    signatureAppearance.SetVisibleSignature(new Rectangle(0f, 0f, 0f, 0f), 1, fieldName);
                }
                else
                {
                    signatureAppearance.SetVisibleSignature(fieldName);
                }
                IExternalSignatureContainer externalSignatureContainer = new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                MakeSignature.SignExternalContainer(signatureAppearance, externalSignatureContainer, 0x2000);

                success = true;
            }
            catch (Exception exception)
            {
                Inventec.Common.Logging.LogSystem.Error(exception);
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (os != null)
                {
                    try
                    {
                        os.Close();
                        os.Dispose();
                    }
                    catch (IOException exception2)
                    {
                        LogSystem.Warn("Error emptySignature: " + exception2.Message);
                    }
                }
            }
            Inventec.Common.Logging.LogSystem.Debug("EmptySignature.____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => success), success));
            return success;
        }

        internal static string GetDisplayText(DisplayConfig displayConfig, string strDate)
        {
            string displayText = String.Empty;
            try
            {
                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig), displayConfig)
                    + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => strDate), strDate));
                if ((displayConfig.DisplayText != null) && (displayConfig.DisplayText.Length != 0))
                {
                    displayText = displayConfig.DisplayText;
                }
                else
                {
                    List<object> argss = new List<object>();
                    List<string> argsFormats = new List<string>();
                    string location = "", title = "";

                    if (!String.IsNullOrEmpty(displayConfig.Location) && displayConfig.Location.Contains("|"))
                    {
                        var arrSplits = displayConfig.Location.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arrSplits != null && arrSplits.Length > 0)
                        {
                            location = arrSplits[0];
                        }
                        if (arrSplits != null && arrSplits.Length > 1)
                        {
                            title = arrSplits[1];
                        }
                    }
                    else
                    {
                        location = displayConfig.Location;
                        if (displayConfig.Titles != null && displayConfig.Titles.Length > 0)
                        {
                            title = displayConfig.Titles[0];
                        }
                    }

                    if (displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_3__NO_DATE || displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_USER)
                    {
                        argss.Add(displayConfig.Contact.Replace("\\", ""));
                        argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                        if (!String.IsNullOrEmpty(title))
                        {
                            argss.Add(title);
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", (argsFormats.Count).ToString()));
                        }

                        if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                        {
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                            argss.Add(displayConfig.Reason);
                        }

                        displayConfig.FormatRectangleText = String.Join("\r\n", argsFormats);
                    }
                    else if (displayConfig.FormatRectangleText == Constans.SIGN_TEXT_FORMAT_3__NO_TITLE)
                    {
                        argss.Add(displayConfig.Contact.Replace("\\", ""));
                        argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                        argss.Add(strDate);
                        argsFormats.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", (argsFormats.Count).ToString()));

                        if (!String.IsNullOrEmpty(location))
                        {
                            argss.Add(location);
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", (argsFormats.Count).ToString()));
                        }

                        if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                        {
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                            argss.Add(displayConfig.Reason);
                        }
                        displayConfig.FormatRectangleText = String.Join("\r\n", argsFormats);
                    }
                    else
                    {
                        //--------------EMR.EMR_SIGN.SIGN_INFO_DISPLAY_OPTION----------------------------------
                        //1 - Hiển thị đầy đủ "Tên người ký", "Thời gian ký", "Địa điểm ký", "Chức danh"
                        //2 - Chỉ hiển thị "Tên người ký", "Chức danh"(hiển thị trên 2 dòng)
                        //3 - Chỉ hiển thị "Tên người ký", "Thời gian ký", "Địa điểm ký"
                        //4 - Chỉ hiển thị "Tên người ký", "Thời gian ký", "Chức danh"
                        //5 - chỉ hiển thị "Tên người ký", "Chức danh", "Địa điểm ký".
                        //-------------------------------------------------------------------------------------
                        if (displayConfig.FormatRectangleText == "2")
                        {
                            argss.Add(displayConfig.Contact.Replace("\\", ""));
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                            if (!String.IsNullOrEmpty(title))
                            {
                                argss.Add(title);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                            {
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                                argss.Add(displayConfig.Reason);
                            }
                        }
                        else if (displayConfig.FormatRectangleText == "3")
                        {
                            argss.Add(displayConfig.Contact.Replace("\\", ""));
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                            argss.Add(strDate);
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", (argsFormats.Count).ToString()));

                            if (!String.IsNullOrEmpty(location))
                            {
                                argss.Add(location);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                            {
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                                argss.Add(displayConfig.Reason);
                            }
                        }
                        else if (displayConfig.FormatRectangleText == "4")
                        {
                            argss.Add(displayConfig.Contact.Replace("\\", ""));
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                            argss.Add(strDate);
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", (argsFormats.Count).ToString()));

                            if (!String.IsNullOrEmpty(title))
                            {
                                argss.Add(title);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                            {
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                                argss.Add(displayConfig.Reason);
                            }
                        }
                        else if (displayConfig.FormatRectangleText == "5")
                        {
                            argss.Add(displayConfig.Contact.Replace("\\", ""));
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                            if (!String.IsNullOrEmpty(location))
                            {
                                argss.Add(location);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (!String.IsNullOrEmpty(title))
                            {
                                argss.Add(title);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                            {
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                                argss.Add(displayConfig.Reason);
                            }
                        }
                        else
                        {
                            argss.Add(displayConfig.Contact.Replace("\\", ""));
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_USER.Replace("0", (argsFormats.Count).ToString()));

                            argss.Add(strDate);
                            argsFormats.Add(Constans.SIGN_TEXT_FORMAT_DATE.Replace("0", (argsFormats.Count).ToString()));

                            if (!String.IsNullOrEmpty(location))
                            {
                                argss.Add(location);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_PLACE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (!String.IsNullOrEmpty(title))
                            {
                                argss.Add(title);
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_TITLE.Replace("0", (argsFormats.Count).ToString()));
                            }

                            if (displayConfig.IsDisplaySignNote.HasValue && displayConfig.IsDisplaySignNote.Value && !String.IsNullOrEmpty(displayConfig.Reason))
                            {
                                argsFormats.Add(Constans.SIGN_TEXT_FORMAT_REASON.Replace("0", (argsFormats.Count).ToString()));
                                argss.Add(displayConfig.Reason);
                            }
                        }

                        displayConfig.FormatRectangleText = String.Join("\r\n", argsFormats);
                    }

                    displayText = string.Format(displayConfig.FormatRectangleText, argss.ToArray());
                }
                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayConfig.FormatRectangleText), displayConfig.FormatRectangleText) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => displayText), displayText));
            }
            catch (Exception exception2)
            {
                displayText = Constans.SIGN_TEXT_FORMAT_3_1;
                LogSystem.Warn("Error GetDisplayText: " + exception2.Message);
            }

            return displayText;
        }

        public bool EmptySignatureTable(string inFile, string outFile, string fieldName, DisplayConfig displayConfig, X509Certificate cert)
        {
            PdfReader reader = null;
            FileStream os = null;
            bool flag;
            try
            {
                reader = new PdfReader(inFile);
                AcroFields acroFields = reader.AcroFields;
                int index = 1;
                float[] numArray = new float[displayConfig.MaxPageSign];
                foreach (string str in acroFields.GetSignatureNames())
                {
                    IList<AcroFields.FieldPosition> fieldPositions = acroFields.GetFieldPositions(str);
                    int page = fieldPositions[0].page;
                    if (page > index)
                    {
                        index = page;
                    }
                    float num12 = fieldPositions[0].position.Height;
                    numArray[page] += num12;
                }
                Rectangle pageSize = reader.GetPageSize(index);
                float height = pageSize.Height;
                float marginRight = displayConfig.MarginRight;
                float num4 = pageSize.Width - (displayConfig.MarginRight * 2f);
                os = new FileStream(outFile, FileMode.Create);
                PdfSignatureAppearance signatureAppearance = PdfStamper.CreateSignature(reader, os, '\0', null, true).SignatureAppearance;
                if ("".Equals(displayConfig.Contact))
                {
                    displayConfig.Contact = SharedUtils.GetCN(cert);
                }
                signatureAppearance.Contact = displayConfig.Contact;
                signatureAppearance.Reason = displayConfig.Reason;
                signatureAppearance.Location = displayConfig.Location;
                DateTime signDate = displayConfig.SignDate;
                signatureAppearance.SignDate = signDate;
                PdfPTable element = new PdfPTable(displayConfig.WidthsPercen.Length);
                element.SetWidths(displayConfig.WidthsPercen);
                element.WidthPercentage = 100f;
                element.TotalWidth = num4;
                for (int i = 0; i < displayConfig.TextArray.Length; i++)
                {
                    Paragraph paragraph = new Paragraph(displayConfig.TextArray[i], SignPdfFile.GetFontByConfig(displayConfig))
                    {
                        Alignment = displayConfig.AlignmentArray[i]
                    };
                    PdfPCell cell = new PdfPCell();
                    cell.AddElement(paragraph);
                    element.AddCell(cell);
                }
                float totalHeight = element.TotalHeight;
                float lly = (((height - numArray[index]) - displayConfig.MarginTop) - totalHeight) - displayConfig.HeightTitle;
                Rectangle pageRect = null;
                IExternalSignatureContainer externalSignatureContainer = null;
                if (lly < displayConfig.MarginBottom)
                {
                    if (index >= displayConfig.TotalPageSign)
                    {
                        pageRect = new Rectangle(0f, 0f, 0f, 0f);
                        signatureAppearance.SetVisibleSignature(pageRect, index, fieldName);
                        externalSignatureContainer = new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                        MakeSignature.SignExternalContainer(signatureAppearance, externalSignatureContainer, 0x2000);
                        reader.Close();
                        return true;
                    }
                    index++;
                    lly = ((height - displayConfig.MarginTop) - totalHeight) - displayConfig.HeightTitle;
                }
                pageRect = new Rectangle(marginRight, lly, marginRight + num4, lly + totalHeight);
                signatureAppearance.SetVisibleSignature(pageRect, index, fieldName);
                PdfTemplate layer = signatureAppearance.GetLayer(0);
                float left = layer.BoundingBox.Left;
                float bottom = layer.BoundingBox.Bottom;
                float width = layer.BoundingBox.Width;
                float num10 = layer.BoundingBox.Height;
                ColumnText text1 = new ColumnText(signatureAppearance.GetLayer(2));
                text1.SetSimpleColumn(left, bottom, left + width, bottom + num10);
                text1.AddElement(element);
                text1.Go();
                externalSignatureContainer = new ExternalBlankSignatureContainer(PdfName.ADOBE_PPKLITE, PdfName.ADBE_PKCS7_DETACHED);
                MakeSignature.SignExternalContainer(signatureAppearance, externalSignatureContainer, 0x2000);
                flag = true;
            }
            catch (Exception exception)
            {
                LogSystem.Warn("Error emptySignatureTable: " + exception.Message);
                flag = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (os != null)
                {
                    try
                    {
                        os.Close();
                    }
                    catch (IOException exception2)
                    {
                        LogSystem.Warn("Error emptySignatureTable: " + exception2.Message);
                    }
                }
            }
            return flag;
        }

        public bool InsertSignature(string inFile, string outFile, string fieldName, byte[] hash, byte[] extSignature, X509Certificate[] chain, DateTime signDate, TimestampConfig timestampConfig)
        {
            PdfReader reader = null;
            FileStream outs = null;
            bool flag;
            try
            {
                reader = new PdfReader(inFile);
                outs = new FileStream(outFile, FileMode.Append);
                AcroFields acroFields = reader.AcroFields;
                PdfDictionary signatureDictionary = acroFields.GetSignatureDictionary(fieldName);
                if (signatureDictionary == null)
                {
                    LogSystem.Warn("No field");
                    return false;
                }
                if (!acroFields.SignatureCoversWholeDocument(fieldName))
                {
                    LogSystem.Warn("Not the last signature");
                    return false;
                }
                PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
                long[] numArray = asArray.AsLongArray();
                if ((asArray.Size != 4) || (numArray[0] != 0))
                {
                    LogSystem.Warn("Single exclusion space supported");
                    return false;
                }
                IRandomAccessSource source = reader.SafeFile.CreateSourceView();
                string hashAlgorithm = HASH_ALG;
                PdfPKCS7 fpkcs1 = new PdfPKCS7(null, chain, hashAlgorithm, false);
                fpkcs1.SetExternalDigest(extSignature, null, CRYPT_ALG);
                DateTime signingTime = signDate;
                TSAClientBouncyCastle tsaClient = null;
                if (timestampConfig.UseTimestamp)
                {
                    tsaClient = new TSAClientBouncyCastle(timestampConfig.TsaUrl, timestampConfig.TsaAcc, timestampConfig.TsaPass);
                }
                byte[] buffer = fpkcs1.GetEncodedPKCS7(hash, signingTime, tsaClient, null, null, CryptoStandard.CMS);
                int num = ((int)(numArray[2] - numArray[1])) - 2;
                if ((num & 1) != 0)
                {
                    LogSystem.Warn("Gap is not a multiple of 2");
                    return false;
                }
                num /= 2;
                if (num < buffer.Length)
                {
                    LogSystem.Warn("Not enough space");
                    return false;
                }
                StreamUtil.CopyBytes(source, 0L, numArray[1] + 1L, outs);
                ByteBuffer buffer2 = new ByteBuffer(num * 2);
                foreach (byte num4 in buffer)
                {
                    buffer2.AppendHex(num4);
                }
                int num2 = (num - buffer.Length) * 2;
                for (int i = 0; i < num2; i++)
                {
                    buffer2.Append((byte)0x30);
                }
                buffer2.WriteTo(outs);
                StreamUtil.CopyBytes(source, numArray[2] - 1L, numArray[3] + 1L, outs);
                source.Close();
                buffer2.Close();
                flag = true;
            }
            catch (Exception exception)
            {
                LogSystem.Warn("Error insertSignature: " + exception.Message);
                flag = false;
            }
            finally
            {
                if (outs != null)
                {
                    try
                    {
                        outs.Close();
                        outs.Dispose();
                    }
                    catch (IOException exception2)
                    {
                        LogSystem.Warn("Error insertSignature: " + exception2.Message);
                    }
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return flag;
        }

        public bool InsertSignature(string inFile, Stream outStream, string fieldName, byte[] hash, byte[] extSignature, X509Certificate[] chain, DateTime signDate, TimestampConfig timestampConfig)
        {
            PdfReader reader = null;
            //FileStream outs = null;
            bool success;
            try
            {
                reader = new PdfReader(inFile);
                //outs = new FileStream(outStream, FileMode.Append);
                AcroFields acroFields = reader.AcroFields;
                PdfDictionary signatureDictionary = acroFields.GetSignatureDictionary(fieldName);
                if (signatureDictionary == null)
                {
                    LogSystem.Warn("No field");
                    return false;
                }
                if (!acroFields.SignatureCoversWholeDocument(fieldName))
                {
                    LogSystem.Warn("Not the last signature");
                    return false;
                }
                PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
                long[] numArray = asArray.AsLongArray();
                if ((asArray.Size != 4) || (numArray[0] != 0))
                {
                    LogSystem.Warn("Single exclusion space supported");
                    return false;
                }
                IRandomAccessSource source = reader.SafeFile.CreateSourceView();
                string hashAlgorithm = HASH_ALG;
                PdfPKCS7 fpkcs1 = new PdfPKCS7(null, chain, hashAlgorithm, false);
                fpkcs1.SetExternalDigest(extSignature, null, CRYPT_ALG);
                DateTime signingTime = signDate;
                TSAClientBouncyCastle tsaClient = null;
                if (timestampConfig.UseTimestamp)
                {
                    tsaClient = new TSAClientBouncyCastle(timestampConfig.TsaUrl, timestampConfig.TsaAcc, timestampConfig.TsaPass);
                }
                byte[] buffer = fpkcs1.GetEncodedPKCS7(hash, signingTime, tsaClient, null, null, CryptoStandard.CMS);
                int num = ((int)(numArray[2] - numArray[1])) - 2;
                if ((num & 1) != 0)
                {
                    LogSystem.Warn("Gap is not a multiple of 2");
                    return false;
                }
                num /= 2;
                if (num < buffer.Length)
                {
                    LogSystem.Warn("Not enough space");
                    return false;
                }
                StreamUtil.CopyBytes(source, 0L, numArray[1] + 1L, outStream);
                ByteBuffer buffer2 = new ByteBuffer(num * 2);
                foreach (byte num4 in buffer)
                {
                    buffer2.AppendHex(num4);
                }
                int num2 = (num - buffer.Length) * 2;
                for (int i = 0; i < num2; i++)
                {
                    buffer2.Append((byte)0x30);
                }
                buffer2.WriteTo(outStream);
                StreamUtil.CopyBytes(source, numArray[2] - 1L, numArray[3] + 1L, outStream);
                //outStream.Close();
                source.Close();
                buffer2.Close();
                success = true;
            }
            catch (Exception exception)
            {
                LogSystem.Warn("Error insertSignature: " + exception.Message);
                success = false;
            }
            finally
            {
                //if (outStream != null)
                //{
                //    try
                //    {
                //        outStream.Close();
                //    }
                //    catch (IOException exception2)
                //    {
                //        Console.WriteLine("Error insertSignature: " + exception2.Message);
                //    }
                //}
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return success;
        }

        public List<byte[]> PreSign(string inFile, string fieldName, X509Certificate[] chain, DateTime signDate)
        {
            PdfReader reader = null;
            List<byte[]> list2;
            try
            {
                List<byte[]> list = new List<byte[]>();
                reader = new PdfReader(inFile);
                PdfDictionary signatureDictionary = reader.AcroFields.GetSignatureDictionary(fieldName);
                if (signatureDictionary == null)
                {
                    Console.WriteLine("No field");
                    return null;
                }
                PdfArray asArray = signatureDictionary.GetAsArray(PdfName.BYTERANGE);
                long[] ranges = asArray.AsLongArray();
                if ((asArray.Size != 4) || (ranges[0] != 0))
                {
                    Console.WriteLine("Single exclusion space supported");
                    return null;
                }
                IRandomAccessSource source = reader.SafeFile.CreateSourceView();
                PdfPKCS7 fpkcs = new PdfPKCS7(null, chain, HASH_ALG, false);

                byte[] secondDigest = DigestAlgorithms.Digest(new RASInputStream(new RandomAccessSourceFactory().CreateRanged(source, ranges)), DigestUtilities.GetDigest(HASH_ALG));
                DateTime signingTime = signDate;
                byte[] item = fpkcs.getAuthenticatedAttributeBytes(secondDigest, signingTime, null, null, CryptoStandard.CMS);
                list.Add(item);
                list.Add(secondDigest);
                list2 = list;
            }
            catch (Exception exception)
            {
                LogSystem.Warn("Error create hash: " + exception.Message);
                list2 = null;
            }
            finally
            {
                try
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                catch (Exception exception2)
                {
                    LogSystem.Warn("Error create hash: " + exception2.Message);
                }
            }
            return list2;
        }
    }

}
