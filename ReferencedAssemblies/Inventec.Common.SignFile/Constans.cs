namespace Inventec.Common.SignFile
{
	public class Constans
	{
		public enum TEXT_POSITON
		{
			x100,
			x50x50,
			x40x60,
			x60x40,
			x30x70,
			x70x30,
			x25x75,
			x75x25
		}

		public enum FileInputType
		{
			Pdf,
			Xml,
			Json
		}

		public const string DATE_FORMAT_1 = "{0:dd/MM/yyyy HH:mm:ss}";

		public const string DATE_FORMAT_2 = "{0:yyyy/MM/dd HH:mm:ss}";

		public const string DATE_FORMAT_3 = "{0:HH:mm:ss dd/MM/yyyy}";

		public const string DATE_FORMAT_4 = "{0:HH:mm:ss yyyy/MM/dd}";

		public static int DISPLAY_IMAGE_STAMP = 2;

		public static int DISPLAY_IMAGE_STAMP_WITH_TEXT = 4;

		public static int DISPLAY_RECTANGLE_TEXT = 1;

		public static int DISPLAY_TABLE = 3;

		public static string FONT_PATH_ARIAL_WINDOWS = "C:/windows/fonts/arial.ttf";

		public static string FONT_PATH_TAHOMA_WINDOWS = "C:/windows/fonts/tahoma.ttf";

		public static string FONT_PATH_TIMESNEWROMAN_WINDOWS = "C:/windows/fonts/times.ttf";

		public static string SIGN_TEXT_FORMAT_0 = "{0}\r\n{1}";

		public static string SIGN_TEXT_FORMAT_1 = "Người Ký: {0}";

		public static string SIGN_TEXT_FORMAT_USER = "Người Ký: {0}";

		public static string SIGN_TEXT_FORMAT_DATE = "Ngày Ký: {0}";

		public static string SIGN_TEXT_FORMAT_PLACE = "Địa Điểm: {0}";

		public static string SIGN_TEXT_FORMAT_TITLE = "Chức danh: {0}";

		public static string SIGN_TEXT_FORMAT_REASON = "Lý Do: {0}";

		public static string SIGN_TEXT_FORMAT_2 = "Người Ký: {0} \r\nNgày Ký: {1}";

		public static string SIGN_TEXT_FORMAT_3 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nLý Do: {2}";

		public static string SIGN_TEXT_FORMAT_3_1 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2}";

		public static string SIGN_TEXT_FORMAT_3__NO_TITLE = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2} ...";

		public static string SIGN_TEXT_FORMAT_3__NO_DATE = "Người Ký: {0} \r\nĐịa Điểm: {1}";

		public static string SIGN_TEXT_FORMAT_3_HAS_REASON__NO_DATE = "Người Ký: {0} \r\nĐịa Điểm: {1} \r\nLý Do: {2}";

		public static string SIGN_TEXT_FORMAT_4 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2} \r\nLý Do: {3}";

		public static string SIGN_TEXT_FORMAT_5 = "Digital signed by: {0}";

		public static string SIGN_TEXT_FORMAT_6 = "Digital signed by: {0} \r\nDate: {1}";

		public static string SIGN_TEXT_FORMAT_7 = "Digital signed by: {0} \r\nDate: {1} \r\nReason: {2}";

		public static string SIGN_TEXT_FORMAT_8 = "Digital signed by: {0} \r\nDate: {1} \r\nReason: {2} \r\nLocation: {3}";

		public static int SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD = 1;

		public static int SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD = 2;
	}
}
