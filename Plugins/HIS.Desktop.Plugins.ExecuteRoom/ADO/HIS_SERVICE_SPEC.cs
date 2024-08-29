using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.ExecuteRoom.ADO
{
    class HIS_SERVICE_SPEC
    {

        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public long? CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
        public string GROUP_CODE { get; set; }
        public virtual HIS_SERVICE HIS_SERVICE { get; set; }
        public virtual HIS_SPECIALITY HIS_SPECIALITY { get; set; }
        public long ID { get; set; }
        public short? IS_ACTIVE { get; set; }
        public short? IS_DELETE { get; set; }
        public string MODIFIER { get; set; }
        public long? MODIFY_TIME { get; set; }
        public long SERVICE_ID { get; set; }
        public long SPECIALITY_ID { get; set; }
    }
}
