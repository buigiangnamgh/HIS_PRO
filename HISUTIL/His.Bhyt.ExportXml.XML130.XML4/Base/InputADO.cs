using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace His.Bhyt.ExportXml.XML130.XML4.Base
{
    public class InputADO
    {
        public V_HIS_TREATMENT_12 vTreatment { get; set; }
        public List<V_HIS_SERE_SERV_2> vSereServ { get; set; }
        public List<V_HIS_SERE_SERV_TEIN> vSereServTein { get; set; }
        public List<V_HIS_SERVICE> ListServices { get; set; }
        public List<HIS_EMPLOYEE> Employee { get; set; }
        public List<HIS_CONFIG> Configs { get; set; }
        public List<V_HIS_SERE_SERV_SUIN> vSereServSuin { get; set; }
    }
}
