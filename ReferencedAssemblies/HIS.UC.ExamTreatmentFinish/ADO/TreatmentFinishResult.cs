using HIS.UC.SecondaryIcd.ADO;
using MOS.EFMODEL.DataModels;
using MOS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.ADO
{
    public class ExamTreatmentFinishResult
    {
        public HisTreatmentFinishSDO TreatmentFinishSDO { get; set; }
        public bool IsSignExam { get; set; }
        public bool IsPrintExam { get; set; }
        public bool IsPrintAppoinment { get; set; }
        public bool IsPrintBordereau { get; set; }
        public bool IsSignAppoinment { get; set; }
        public bool IsSignBordereau { get; set; }
        public bool IsPrintBANT { get; set; }
        public bool IsPrintSurgAppoint { get; set; }
        public bool IsPrintHosTransfer { get; set; }

        public bool IsPrintBHXH { get; set; }
        public bool IsSignBHXH { get; set; }

        public bool IsSignTrichPhuLuc { get; set; }
        public bool IsPrintTrichPhuLuc { get; set; }
        public bool IsPrintPrescription { get; set; }
        public HIS_SEVERE_ILLNESS_INFO SevereIllNessInfo { get; set; }
        public List<HIS_EVENTS_CAUSES_DEATH> ListEventsCausesDeath { get; set; }
        public HIS.UC.Icd.ADO.IcdInputADO icdADOInTreatment { get; set; }
        public HIS.UC.Icd.ADO.IcdInputADO traditionalIcdTreatment { get; set; }
        public SecondaryIcdDataADO icdSubADOInTreatment { get; set; }

        public SecondaryIcdDataADO traditionInIcdSub { get; set; }

        public string Advise { get; set; }
        public string Conclusion { get; set; }
        public string Note { get; set; }
    }
}
