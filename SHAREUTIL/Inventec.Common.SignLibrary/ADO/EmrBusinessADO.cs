using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    public class EmrBusinessADO
    {
        public string CONCRETE_ID__IN_SETY { get; set; }
        public string PARENT_ID__IN_SETY { get; set; }
        public bool IS_LEAF { get; set; }
        public string USER_NAME { get; set; }
        public long FLOW_ID { get; set; }
        public string FLOW_CODE { get; set; }
        public string FLOW_NAME { get; set; }
        public long SIGNER_ID { get; set; }
        public string TITLE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long NUM_ORDER { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public string ROOM_TYPE_CODE { get; set; }
    }
}
