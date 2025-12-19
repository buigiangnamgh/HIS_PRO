using iTextSharp.text;
using System;

namespace Inventec.Common.SignFile
{
    /// <summary>
    /// 
    /// </summary>
    public class DisplayConfig
    {
        // Fields
        private int[] alignmentArray;
        private BaseColor backgroundColorTitle = new BaseColor(240, 240, 240);
        private string contact = "";
        private float coorXRectangle = 10f;
        private float coorYRectangle = 10f;

        private string dateFormatstring = Constans.DATE_FORMAT_1;

        private string displayText = "";

        private string fontPath = Constans.FONT_PATH_TAHOMA_WINDOWS;
        private int fontSizeTitlePageSign = 15;
        private string formatRectangleText = Constans.SIGN_TEXT_FORMAT_3_1;
        private int typeDisplay = Constans.DISPLAY_RECTANGLE_TEXT;
        private float widthRectangle = 320f;
        private float heightRectangle = 140f;
        private float heightRowTitlePageSign = 40f;
        private float heightTitle = 30f;
        private bool isDisplaySignature = true;
        private bool isDisplayTitlePageSign = true;
        private string location = "";
        private float marginBottom = 80f;
        private float marginRight = 60f;
        private float marginTop = 80f;
        private int maxPageSign = 10;
        private int numberPageSign = 1;
        private Rectangle pageSize = iTextSharp.text.PageSize.A4;
        private string pathImage;
        private byte[] bImage;
        private string reason = "";

        private DateTime signDate;
        private int signType = Constans.SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD;
        private int sizeFont = 11;
        private string[] textArray;
        private string titlePageSign = "TRANG K\x00dd";
        private string[] titles;
        private int totalPageSign = 1;
        private float[] widthsPercen;

        private Constans.TEXT_POSITON textPosition = Constans.TEXT_POSITON.x100;
        private FontConfig textFormat = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <param name="pathImage"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigImage(int numberPageSign, float coorX, float coorY, float width, float height, string contact, string reason, string location, byte[] bImage)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_IMAGE_STAMP,
                NumberPageSign = numberPageSign,
                CoorXRectangle = coorX,
                CoorYRectangle = coorY,
                WidthRectangle = width,
                HeightRectangle = height,
                Contact = contact,
                Reason = reason,
                Location = location,
                BImage = bImage,
                SignDate = DateTime.Now
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <param name="pathImage"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigImage_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, string contact, string reason, string location, byte[] bImage)
        {
            DisplayConfig config1 = generateDisplayConfigImage(
                numberPageSign,
                coorX,
                coorY,
                width,
                height,
                contact,
                reason,
                location,
                bImage);
            config1.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
            return config1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pathImage"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigImageDefault(int numberPageSign, float coorX, float coorY, float width, float height, byte[] bImage)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_IMAGE_STAMP,
                NumberPageSign = numberPageSign,
                CoorXRectangle = coorX,
                CoorYRectangle = coorY,
                WidthRectangle = width,
                HeightRectangle = height,
                BImage = bImage,
                SignDate = DateTime.Now
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="pathImage"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigImageDefault_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, byte[] bImage)
        {
            DisplayConfig config1 = generateDisplayConfigImageDefault(numberPageSign, coorX, coorY, width, height, bImage);
            config1.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
            return config1;
        }

        public static DisplayConfig generateDisplayConfigRectangleText(int numberPageSign, float coorX, float coorY, float width, float height, string displayText, string formatRectangleText, string contact, string reason, string location, string dateFormatstring)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_RECTANGLE_TEXT,
                NumberPageSign = numberPageSign,
                CoorXRectangle = coorX,
                CoorYRectangle = coorY,
                WidthRectangle = width,
                HeightRectangle = height,
                DisplayText = displayText,
                FormatRectangleText = formatRectangleText,
                Contact = contact,
                Reason = reason,
                Location = location,
                DateFormatstring = dateFormatstring,
                SignDate = DateTime.Now
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="displayText"></param>
        /// <param name="formatRectangleText"></param>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <param name="dateFormatstring"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigRectangleText(int typeDisplay, int sizeFont, Inventec.Common.SignFile.Constans.TEXT_POSITON textPosition, string imagepath, byte[] bImage, int numberPageSign, float coorX, float coorY, float width, float height, string displayText, string formatRectangleText, string contact, string reason, string location, string dateFormatstring)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = (typeDisplay > 0 ? typeDisplay : Constans.DISPLAY_RECTANGLE_TEXT),
                SizeFont = (sizeFont > 0 ? sizeFont : 11),
                TextPosition = (textPosition > 0 ? textPosition : Inventec.Common.SignFile.Constans.TEXT_POSITON.x100),
                PathImage = imagepath,
                BImage = bImage,
                NumberPageSign = numberPageSign,
                CoorXRectangle = coorX,
                CoorYRectangle = coorY,
                WidthRectangle = width,
                HeightRectangle = height,
                DisplayText = displayText,
                FormatRectangleText = (String.IsNullOrEmpty(formatRectangleText) ? Constans.SIGN_TEXT_FORMAT_3_1 : formatRectangleText),
                Contact = contact,
                Reason = reason,
                Location = location,
                DateFormatstring = dateFormatstring,
                SignDate = DateTime.Now,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPageSign"></param>
        /// <param name="coorX"></param>
        /// <param name="coorY"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="displayText"></param>
        /// <param name="formatRectangleText"></param>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <param name="dateFormatString"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigRectangleText_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, string displayText, string formatRectangleText, string contact, string reason, string location, string dateFormatString)
        {
            DisplayConfig config1 = generateDisplayConfigRectangleText(
                numberPageSign,
                coorX,
                coorY,
                width,
                height,
                displayText,
                formatRectangleText,
                contact,
                reason,
                location,
                dateFormatString);
            config1.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
            return config1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigRectangleTextDefault(string contact, string reason, string location)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_RECTANGLE_TEXT,
                Contact = contact,
                Reason = reason,
                Location = location,
                SignDate = DateTime.Now
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <param name="reason"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigRectangleTextDefault_ExistSignatureFieldDefault(string contact, string reason, string location)
        {
            DisplayConfig config1 = generateDisplayConfigRectangleTextDefault(contact, reason, location);
            config1.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
            return config1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalPageSign"></param>
        /// <param name="titles"></param>
        /// <param name="widthsPercen"></param>
        /// <param name="alignmentArray"></param>
        /// <param name="textArray"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigTable(int totalPageSign, string[] titles, float[] widthsPercen, int[] alignmentArray, string[] textArray)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_TABLE,
                TotalPageSign = totalPageSign,
                Titles = titles,
                WidthsPercen = widthsPercen,
                AlignmentArray = alignmentArray,
                TextArray = textArray,
                SignDate = DateTime.Now
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalPageSign"></param>
        /// <param name="textArray"></param>
        /// <returns></returns>
        public static DisplayConfig generateDisplayConfigTableDefault(int totalPageSign, string[] textArray)
        {
            return new DisplayConfig
            {
                IsDisplaySignature = true,
                TypeDisplay = Constans.DISPLAY_TABLE,
                TotalPageSign = totalPageSign,
                titles = new string[] { "STT", "Người K\x00fd", "Đơn vị", "Thời gian k\x00fd", "\x00dd kiến" },
                widthsPercen = new float[] { 0.06f, 0.18f, 0.2f, 0.14f, 0.42f },
                alignmentArray = new int[] { 1, 0, 3, 0, 3 },
                TextArray = textArray,
                SignDate = DateTime.Now
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] AlignmentArray
        {
            get
            {
                return this.alignmentArray;
            }
            set
            {
                this.alignmentArray = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BaseColor BackgroundColorTitle
        {
            get
            {
                return this.backgroundColorTitle;
            }
            set
            {
                this.backgroundColorTitle = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Contact
        {
            get
            {
                return this.contact;
            }
            set
            {
                this.contact = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float CoorXRectangle
        {
            get
            {
                return this.coorXRectangle;
            }
            set
            {
                this.coorXRectangle = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float CoorYRectangle
        {
            get
            {
                return this.coorYRectangle;
            }
            set
            {
                this.coorYRectangle = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DateFormatstring
        {
            get
            {
                return this.dateFormatstring;
            }
            set
            {
                this.dateFormatstring = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DisplayText
        {
            get
            {
                return this.displayText;
            }
            set
            {
                this.displayText = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FontPath
        {
            get
            {
                return this.fontPath;
            }
            set
            {
                this.fontPath = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int FontSizeTitlePageSign
        {
            get
            {
                return this.fontSizeTitlePageSign;
            }
            set
            {
                this.fontSizeTitlePageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FormatRectangleText
        {
            get
            {
                return this.formatRectangleText;
            }
            set
            {
                this.formatRectangleText = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float HeightRectangle
        {
            get
            {
                return this.heightRectangle;
            }
            set
            {
                this.heightRectangle = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float HeightRowTitlePageSign
        {
            get
            {
                return this.heightRowTitlePageSign;
            }
            set
            {
                this.heightRowTitlePageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float HeightTitle
        {
            get
            {
                return this.heightTitle;
            }
            set
            {
                this.heightTitle = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDisplaySignature
        {
            get
            {
                return this.isDisplaySignature;
            }
            set
            {
                this.isDisplaySignature = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDisplayTitlePageSign
        {
            get
            {
                return this.isDisplayTitlePageSign;
            }
            set
            {
                this.isDisplayTitlePageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float MarginBottom
        {
            get
            {
                return this.marginBottom;
            }
            set
            {
                this.marginBottom = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float MarginRight
        {
            get
            {
                return this.marginRight;
            }
            set
            {
                this.marginRight = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float MarginTop
        {
            get
            {
                return this.marginTop;
            }
            set
            {
                this.marginTop = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaxPageSign
        {
            get
            {
                return this.maxPageSign;
            }
            set
            {
                this.maxPageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NumberPageSign
        {
            get
            {
                return this.numberPageSign;
            }
            set
            {
                this.numberPageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Rectangle PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PathImage
        {
            get
            {
                return this.pathImage;
            }
            set
            {
                this.pathImage = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] BImage
        {
            get
            {
                return this.bImage;
            }
            set
            {
                this.bImage = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Reason
        {
            get
            {
                return this.reason;
            }
            set
            {
                this.reason = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime SignDate
        {
            get
            {
                return this.signDate;
            }
            set
            {
                this.signDate = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignType
        {
            get
            {
                return this.signType;
            }
            set
            {
                this.signType = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SizeFont
        {
            get
            {
                return this.sizeFont;
            }
            set
            {
                this.sizeFont = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string[] TextArray
        {
            get
            {
                return this.textArray;
            }
            set
            {
                this.textArray = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TitlePageSign
        {
            get
            {
                return this.titlePageSign;
            }
            set
            {
                this.titlePageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string[] Titles
        {
            get
            {
                return this.titles;
            }
            set
            {
                this.titles = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalPageSign
        {
            get
            {
                return this.totalPageSign;
            }
            set
            {
                this.totalPageSign = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TypeDisplay
        {
            get
            {
                return this.typeDisplay;
            }
            set
            {
                this.typeDisplay = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float WidthRectangle
        {
            get
            {
                return this.widthRectangle;
            }
            set
            {
                this.widthRectangle = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public float[] WidthsPercen
        {
            get
            {
                return this.widthsPercen;
            }
            set
            {
                this.widthsPercen = value;
            }
        }

        /// <summary>
        /// Vi tri hien thi noi dung so voi anh
        /// </summary>
        public Constans.TEXT_POSITON TextPosition
        {
            get
            {
                return this.textPosition;
            }
            set
            {
                this.textPosition = value;
            }
        }

        public float SignaltureImageWidth { get; set; }

        public bool? IsDisplaySignNote { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FontConfig TextFormat
        {
            get
            {
                return this.textFormat;
            }
            set
            {
                this.textFormat = value;
            }
        }
    }
}
