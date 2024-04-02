using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.UC.ExamTreatmentFinish.ADO
{
    public class ShowICDInformationADO
    {
        public string SHOW_ICD_CODE { get; set; }
        public string SHOW_ICD_NAME { get; set; }
        public string SHOW_ICD_SUB_CODE { get; set; }
        public string SHOW_ICD_TEXT { get; set; }

        public ShowICDInformationADO() 
        { 
        }

        public ShowICDInformationADO(HIS_TREATMENT treatment)
        {
            if (treatment != null)
            {
                this.SHOW_ICD_CODE = treatment.SHOW_ICD_CODE;
                this.SHOW_ICD_NAME = treatment.SHOW_ICD_NAME;
                this.SHOW_ICD_SUB_CODE = treatment.SHOW_ICD_SUB_CODE;
                this.SHOW_ICD_TEXT = treatment.SHOW_ICD_TEXT;
            }
        }
    }
}
