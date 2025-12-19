using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignFile
{
    public class Constans
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DATE_FORMAT_1 = "{0:dd/MM/yyyy HH:mm:ss}";
        /// <summary>
        /// 
        /// </summary>
        public const string DATE_FORMAT_2 = "{0:yyyy/MM/dd HH:mm:ss}";
        /// <summary>
        /// 
        /// </summary>
        public const string DATE_FORMAT_3 = "{0:HH:mm:ss dd/MM/yyyy}";
        /// <summary>
        /// 
        /// </summary>
        public const string DATE_FORMAT_4 = "{0:HH:mm:ss yyyy/MM/dd}";

        /// <summary>
        /// 
        /// </summary>
        public static int DISPLAY_IMAGE_STAMP = 2;
        /// <summary>
        /// 
        /// </summary>
        public static int DISPLAY_IMAGE_STAMP_WITH_TEXT = 4;
        /// <summary>
        /// 
        /// </summary>
        public static int DISPLAY_RECTANGLE_TEXT = 1;
        /// <summary>
        /// 
        /// </summary>
        public static int DISPLAY_TABLE = 3;

        /// <summary>
        /// 
        /// </summary>
        public static string FONT_PATH_ARIAL_WINDOWS = "C:/windows/fonts/arial.ttf";
        /// <summary>
        /// 
        /// </summary>
        public static string FONT_PATH_TAHOMA_WINDOWS = "C:/windows/fonts/tahoma.ttf";
        /// <summary>
        /// 
        /// </summary>
        public static string FONT_PATH_TIMESNEWROMAN_WINDOWS = "C:/windows/fonts/times.ttf";

        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_0 = "{0}\r\n{1}";

        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_1 = "Người Ký: {0}";
        public static string SIGN_TEXT_FORMAT_USER = "Người Ký: {0}";
        public static string SIGN_TEXT_FORMAT_DATE = "Ngày Ký: {0}";
        public static string SIGN_TEXT_FORMAT_PLACE = "Địa Điểm: {0}";
        public static string SIGN_TEXT_FORMAT_TITLE = "Chức danh: {0}";
        public static string SIGN_TEXT_FORMAT_REASON = "Lý Do: {0}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_2 = "Người Ký: {0} \r\nNgày Ký: {1}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_3 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nLý Do: {2}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_3_1 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2}";
        public static string SIGN_TEXT_FORMAT_3__NO_TITLE = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2} ...";
        public static string SIGN_TEXT_FORMAT_3__NO_DATE = "Người Ký: {0} \r\nĐịa Điểm: {1}";
        public static string SIGN_TEXT_FORMAT_3_HAS_REASON__NO_DATE = "Người Ký: {0} \r\nĐịa Điểm: {1} \r\nLý Do: {2}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_4 = "Người Ký: {0} \r\nNgày Ký: {1} \r\nĐịa Điểm: {2} \r\nLý Do: {3}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_5 = "Digital signed by: {0}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_6 = "Digital signed by: {0} \r\nDate: {1}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_7 = "Digital signed by: {0} \r\nDate: {1} \r\nReason: {2}";
        /// <summary>
        /// 
        /// </summary>
        public static string SIGN_TEXT_FORMAT_8 = "Digital signed by: {0} \r\nDate: {1} \r\nReason: {2} \r\nLocation: {3}";
        /// <summary>
        /// 
        /// </summary>
        public static int SIGN_TYPE_CREATE_NEW_EMPTY_SIGNATURE_FIELD = 1;
        /// <summary>
        /// 
        /// </summary>
        public static int SIGN_TYPE_EXISTED_EMPTY_SIGNATURE_FIELD = 2;

        public enum TEXT_POSITON
        {
            x100 = 0,
            x50x50 = 1,
            x40x60 = 2,
            x60x40 = 3,
            x30x70 = 4,
            x70x30 = 5,
            x25x75 = 6,
            x75x25 = 7,
        }

        public enum FileInputType
        {
            Pdf = 0,
            Xml = 1,
            Json = 2
        }
    }
}
