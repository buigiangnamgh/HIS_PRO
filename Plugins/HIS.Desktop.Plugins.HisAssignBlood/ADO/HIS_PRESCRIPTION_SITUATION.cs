using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Desktop.Plugins.HisAssignBlood.ADO
{
    class HIS_PRESCRIPTION_SITUATION
    {
        public long ID { get; set; }
        public Nullable<long> CREATE_TIME { get; set; }
        public Nullable<long> MODIFY_TIME { get; set; }
        public string CREATOR { get; set; }
        public string MODIFIER { get; set; }
        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public Nullable<short> IS_ACTIVE { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public string GROUP_CODE { get; set; }
        public long EXP_MEST_ID { get; set; }
        public long SITUATION_ID { get; set; }

        public virtual HIS_EXP_MEST HIS_EXP_MEST { get; set; }
        public virtual HIS_SITUATION HIS_SITUATION { get; set; }
    }
}
