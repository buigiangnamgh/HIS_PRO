using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamServiceAdd.ADO
{
    public class ExamServiceAddInitADO
    {
        public long? ServiceReqId { get; set; }
        public long? roomId { get; set; }
        public bool IsMainExam { get; set; }
        public long? treatmentId { get; set; }
        public long? FinishTime { get; set; }// thoi gian ket thuc kham
        public long? StartTime { get; set; }
        public long? InTime { get; set; }
        public long? OutTime { get; set; } // TG ket thuc dieu tri
        public bool IsNotRequiredFee { get; set; }
        public bool IsBlockNumOrder { get; set; }
        public long? DefaultIdRoom { get; set; }
        public long? NumOrderBlockId { get; set; }
        public string AppointmentDesc { get; set; }
        public long? AppointmentTime { get; set; }
        public string Note { get; set; }
    }
}
