using MOS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.ADO
{
    public class AdviseADO
    {
        public string Advise { get; set; }
        public string AppointmentExamRoomIds { get; set; }
        public List<long> ExamRoomIds { get; set; }
        public long currentRoomId { get; set; }
    }
}
