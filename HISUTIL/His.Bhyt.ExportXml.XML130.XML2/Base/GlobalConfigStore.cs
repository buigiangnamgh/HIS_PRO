using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2.Base
{
    public class GlobalConfigStore
    {
        //Đường dẫn thư mục lưu file xml
        public static string PathSaveXml;

        //Thông tin đơn vị
        public static string Signature;
        public static HIS_BRANCH Branch;
        public static int MAX_LENGTH = 1024;
    }
}
