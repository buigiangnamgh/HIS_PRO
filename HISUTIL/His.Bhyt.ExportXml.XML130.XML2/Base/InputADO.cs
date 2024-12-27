using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML2.Base
{
    public class InputADO
    {
        public V_HIS_TREATMENT_12 vTreatment12 { get; set; }
        public List<V_HIS_SERE_SERV_2> vSereServ2 { get; set; }
        public List<V_HIS_SERVICE> vHisService { get; set; }
        public List<V_HIS_SERE_SERV_PTTT> vHisSereServPttt { get; set; }
        public List<HIS_EMPLOYEE> HisEmployee { get; set; }
        public List<HIS_CONFIG> HisConfig { get; set; }
        public List<HIS_PATIENT_TYPE> HisPatientTypes { get; set; }
        public bool IS_3176 { get; set; }
    }
}
