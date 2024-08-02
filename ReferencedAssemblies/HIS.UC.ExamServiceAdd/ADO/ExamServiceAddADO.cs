using MOS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.HisExamServiceAdd.ADO
{
    public class ExamServiceAddADO : HisServiceReqExamAdditionSDO
    {
        public long? FinishTime { get; set; }
        public bool IsPrintExamAdd { get; set; }
        public bool IsSignExamAdd { get; set; }
        public bool IsAppointment { get; set; }
        public bool IsPrintAppointment { get; set; }
        public long? AppointmentTime { get; set; }
        public string Advise { get; set; }
        public bool IsBlockNumOrder { get; set; }
        public long? DefaultIdRoom { get; set; }
        public long? NumOrderBlockId { get; set; }
        public long? RoomApointmentId { get; set; }
        public long? ServiceApointmentId { get; set; }
        public string Note { get; set; }
    }
}
