using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML1.Base
{
    public class GlobalConfigStore
    {
        //Đường dẫn thư mục lưu file xml
        public static string PathSaveXml;

        //Thông tin đơn vị
        public static string Signature;
        public static HIS_BRANCH Branch;
        public static int MAX_LENGTH = 1024;
        private static List<HIS_MEDI_ORG> HisMediOrg;
        internal static List<HIS_MEDI_ORG> HisHeinMediOrg
        {
            get
            {
                if (HisMediOrg == null || HisMediOrg.Count <= 0)
                {
                    HisMediOrg = ConfigHeinMediOrg.GetMediOrgConst();
                }

                return HisMediOrg;
            }
            set
            {
                HisMediOrg = value;
            }
        }
    }
}
