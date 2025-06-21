using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.DTO
{
    public class TreatmentDTO
    {
        // Methods
        public TreatmentDTO() { }

        // Properties     
        public string AVATAR_URL { get; set; }
        public string BRANCH_CODE { get; set; }
        public string CARD_CODE { get; set; }
        public long? CLINICAL_IN_TIME { get; set; } 
        public string CURRENT_DEPARTMENT_CODE { get; set; }
        public string CURRENT_DEPARTMENT_NAME { get; set; }
        public string DATA_STORE_CODE { get; set; }
        public string DATA_STORE_NAME { get; set; }
        public long DOB { get; set; }    
        public string END_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string GENDER_CODE { get; set; }
        public string GENDER_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public long ID { get; set; }
        public string IN_CODE { get; set; }
        public long IN_DATE { get; set; }
        public long IN_TIME { get; set; }      
        public short? IS_EMERGENCY { get; set; }
        public short? IS_HAS_NOT_DAY_DOB { get; set; }
        public string LAST_NAME { get; set; }
        public string OUT_CODE { get; set; }
        public long? OUT_DATE { get; set; }
        public long? OUT_TIME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string STORE_CODE { get; set; }
        public long? STORE_DATE { get; set; }
        public long? STORE_TIME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_RESULT_NAME { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
    }
}
