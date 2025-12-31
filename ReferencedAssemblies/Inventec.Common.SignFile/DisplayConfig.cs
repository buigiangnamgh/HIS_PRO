using System;
using iTextSharp.text;

namespace Inventec.Common.SignFile
{
	public class DisplayConfig
	{
		private int[] alignmentArray;

		private BaseColor backgroundColorTitle = new BaseColor(240, 240, 240);

		private string contact = "";

		private float coorXRectangle = 10f;

		private float coorYRectangle = 10f;

		private string dateFormatstring = "{0:dd/MM/yyyy HH:mm:ss}";

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

		private string titlePageSign = "TRANG KÝ";

		private string[] titles;

		private int totalPageSign = 1;

		private float[] widthsPercen;

		private Constans.TEXT_POSITON textPosition = Constans.TEXT_POSITON.x100;

		private FontConfig textFormat = null;

		public string HashAlgorithm { get; set; }

		public int[] AlignmentArray
		{
			get
			{
				return alignmentArray;
			}
			set
			{
				alignmentArray = value;
			}
		}

		public BaseColor BackgroundColorTitle
		{
			get
			{
				return backgroundColorTitle;
			}
			set
			{
				backgroundColorTitle = value;
			}
		}

		public string Contact
		{
			get
			{
				return contact;
			}
			set
			{
				contact = value;
			}
		}

		public float CoorXRectangle
		{
			get
			{
				return coorXRectangle;
			}
			set
			{
				coorXRectangle = value;
			}
		}

		public float CoorYRectangle
		{
			get
			{
				return coorYRectangle;
			}
			set
			{
				coorYRectangle = value;
			}
		}

		public string DateFormatstring
		{
			get
			{
				return dateFormatstring;
			}
			set
			{
				dateFormatstring = value;
			}
		}

		public string DisplayText
		{
			get
			{
				return displayText;
			}
			set
			{
				displayText = value;
			}
		}

		public string FontPath
		{
			get
			{
				return fontPath;
			}
			set
			{
				fontPath = value;
			}
		}

		public int FontSizeTitlePageSign
		{
			get
			{
				return fontSizeTitlePageSign;
			}
			set
			{
				fontSizeTitlePageSign = value;
			}
		}

		public string FormatRectangleText
		{
			get
			{
				return formatRectangleText;
			}
			set
			{
				formatRectangleText = value;
			}
		}

		public float HeightRectangle
		{
			get
			{
				return heightRectangle;
			}
			set
			{
				heightRectangle = value;
			}
		}

		public float HeightRowTitlePageSign
		{
			get
			{
				return heightRowTitlePageSign;
			}
			set
			{
				heightRowTitlePageSign = value;
			}
		}

		public float HeightTitle
		{
			get
			{
				return heightTitle;
			}
			set
			{
				heightTitle = value;
			}
		}

		public bool IsDisplaySignature
		{
			get
			{
				return isDisplaySignature;
			}
			set
			{
				isDisplaySignature = value;
			}
		}

		public bool IsDisplayTitlePageSign
		{
			get
			{
				return isDisplayTitlePageSign;
			}
			set
			{
				isDisplayTitlePageSign = value;
			}
		}

		public string Location
		{
			get
			{
				return location;
			}
			set
			{
				location = value;
			}
		}

		public float MarginBottom
		{
			get
			{
				return marginBottom;
			}
			set
			{
				marginBottom = value;
			}
		}

		public float MarginRight
		{
			get
			{
				return marginRight;
			}
			set
			{
				marginRight = value;
			}
		}

		public float MarginTop
		{
			get
			{
				return marginTop;
			}
			set
			{
				marginTop = value;
			}
		}

		public int MaxPageSign
		{
			get
			{
				return maxPageSign;
			}
			set
			{
				maxPageSign = value;
			}
		}

		public int NumberPageSign
		{
			get
			{
				return numberPageSign;
			}
			set
			{
				numberPageSign = value;
			}
		}

		public Rectangle PageSize
		{
			get
			{
				return pageSize;
			}
			set
			{
				pageSize = value;
			}
		}

		public string PathImage
		{
			get
			{
				return pathImage;
			}
			set
			{
				pathImage = value;
			}
		}

		public byte[] BImage
		{
			get
			{
				return bImage;
			}
			set
			{
				bImage = value;
			}
		}

		public string Reason
		{
			get
			{
				return reason;
			}
			set
			{
				reason = value;
			}
		}

		public DateTime SignDate
		{
			get
			{
				return signDate;
			}
			set
			{
				signDate = value;
			}
		}

		public int SignType
		{
			get
			{
				return signType;
			}
			set
			{
				signType = value;
			}
		}

		public int SizeFont
		{
			get
			{
				return sizeFont;
			}
			set
			{
				sizeFont = value;
			}
		}

		public string[] TextArray
		{
			get
			{
				return textArray;
			}
			set
			{
				textArray = value;
			}
		}

		public string TitlePageSign
		{
			get
			{
				return titlePageSign;
			}
			set
			{
				titlePageSign = value;
			}
		}

		public string[] Titles
		{
			get
			{
				return titles;
			}
			set
			{
				titles = value;
			}
		}

		public int TotalPageSign
		{
			get
			{
				return totalPageSign;
			}
			set
			{
				totalPageSign = value;
			}
		}

		public int TypeDisplay
		{
			get
			{
				return typeDisplay;
			}
			set
			{
				typeDisplay = value;
			}
		}

		public float WidthRectangle
		{
			get
			{
				return widthRectangle;
			}
			set
			{
				widthRectangle = value;
			}
		}

		public float[] WidthsPercen
		{
			get
			{
				return widthsPercen;
			}
			set
			{
				widthsPercen = value;
			}
		}

		public Constans.TEXT_POSITON TextPosition
		{
			get
			{
				return textPosition;
			}
			set
			{
				textPosition = value;
			}
		}

		public float SignaltureImageWidth { get; set; }

		public bool? IsDisplaySignNote { get; set; }

		public FontConfig TextFormat
		{
			get
			{
				return textFormat;
			}
			set
			{
				textFormat = value;
			}
		}

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

		public static DisplayConfig generateDisplayConfigImage_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, string contact, string reason, string location, byte[] bImage)
		{
			DisplayConfig displayConfig = generateDisplayConfigImage(numberPageSign, coorX, coorY, width, height, contact, reason, location, bImage);
			displayConfig.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
			return displayConfig;
		}

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

		public static DisplayConfig generateDisplayConfigImageDefault_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, byte[] bImage)
		{
			DisplayConfig displayConfig = generateDisplayConfigImageDefault(numberPageSign, coorX, coorY, width, height, bImage);
			displayConfig.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
			return displayConfig;
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

		public static DisplayConfig generateDisplayConfigRectangleText(int typeDisplay, int sizeFont, Constans.TEXT_POSITON textPosition, string imagepath, byte[] bImage, int numberPageSign, float coorX, float coorY, float width, float height, string displayText, string formatRectangleText, string contact, string reason, string location, string dateFormatstring)
		{
			return new DisplayConfig
			{
				IsDisplaySignature = true,
				TypeDisplay = ((typeDisplay > 0) ? typeDisplay : Constans.DISPLAY_RECTANGLE_TEXT),
				SizeFont = ((sizeFont > 0) ? sizeFont : 11),
				TextPosition = ((textPosition > Constans.TEXT_POSITON.x100) ? textPosition : Constans.TEXT_POSITON.x100),
				PathImage = imagepath,
				BImage = bImage,
				NumberPageSign = numberPageSign,
				CoorXRectangle = coorX,
				CoorYRectangle = coorY,
				WidthRectangle = width,
				HeightRectangle = height,
				DisplayText = displayText,
				FormatRectangleText = (string.IsNullOrEmpty(formatRectangleText) ? Constans.SIGN_TEXT_FORMAT_3_1 : formatRectangleText),
				Contact = contact,
				Reason = reason,
				Location = location,
				DateFormatstring = dateFormatstring,
				SignDate = DateTime.Now
			};
		}

		public static DisplayConfig generateDisplayConfigRectangleText_ExistedSignatureField(int numberPageSign, float coorX, float coorY, float width, float height, string displayText, string formatRectangleText, string contact, string reason, string location, string dateFormatString)
		{
			DisplayConfig displayConfig = generateDisplayConfigRectangleText(numberPageSign, coorX, coorY, width, height, displayText, formatRectangleText, contact, reason, location, dateFormatString);
			displayConfig.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
			return displayConfig;
		}

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

		public static DisplayConfig generateDisplayConfigRectangleTextDefault_ExistSignatureFieldDefault(string contact, string reason, string location)
		{
			DisplayConfig displayConfig = generateDisplayConfigRectangleTextDefault(contact, reason, location);
			displayConfig.SignType = Constans.SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD;
			return displayConfig;
		}

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

		public static DisplayConfig generateDisplayConfigTableDefault(int totalPageSign, string[] textArray)
		{
			DisplayConfig displayConfig = new DisplayConfig();
			displayConfig.IsDisplaySignature = true;
			displayConfig.TypeDisplay = Constans.DISPLAY_TABLE;
			displayConfig.TotalPageSign = totalPageSign;
			displayConfig.titles = new string[5] { "STT", "Người Ký", "Đơn vị", "Thời gian ký", "Ý kiến" };
			displayConfig.widthsPercen = new float[5] { 0.06f, 0.18f, 0.2f, 0.14f, 0.42f };
			displayConfig.alignmentArray = new int[5] { 1, 0, 3, 0, 3 };
			displayConfig.TextArray = textArray;
			displayConfig.SignDate = DateTime.Now;
			return displayConfig;
		}
	}
}
