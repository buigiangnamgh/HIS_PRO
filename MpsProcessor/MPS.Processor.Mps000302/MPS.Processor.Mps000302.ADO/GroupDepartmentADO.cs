using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Processor.Mps000302.MPS.Processor.Mps000302.ADO
{
    public class GroupDepartmentADO
    {
        public string KEY_PATY_ALTER { get; set; }

        public long GROUP_DEPARTMENT_ID { get; set; }

        public long GROUP_DEPARTMENT_ID__DepaRoom { get; set; }

        public long GROUP_ROOM_ID__ExeRoom { get; set; }

        public decimal? TOTAL_PRICE_BHYT_HEIN_SERVICE_TYPE { get; set; }

        public decimal? TOTAL_PRICE_HEIN_SERVICE_TYPE { get; set; }

        public decimal? TOTAL_HEIN_PRICE_HEIN_SERVICE_TYPE { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_VIR_HEIN_SERVICE_TYPE { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_HEIN_SERVICE_TYPE { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_SELF_HEIN_SERVICE_TYPE { get; set; }

        public string DEPARTMENT_CODE { get; set; }

        public string DEPARTMENT_NAME { get; set; }

        public string ROOM_CODE { get; set; }

        public string ROOM_NAME { get; set; }

        public short? IS_CLINICAL { get; set; }

        public decimal? OTHER_SOURCE_PRICE { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_LEFT { get; set; }

        public decimal TOTAL_PRICE_VP { get; set; }

        public decimal TOTAL_PATIENT_LEFT { get; set; }

        public string GROUP_ROOM_CODE { get; set; }

        public string GROUP_ROOM_NAME { get; set; }
    }
}
