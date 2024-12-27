using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace His.Bhyt.ExportXml.XML130.XML1.Base
{
    public class InputADO
    {
        public V_HIS_TREATMENT_12 vTreatment { get; set; }
        public List<V_HIS_SERE_SERV_2> vSereServ { get; set; }
        public List<V_HIS_SERE_SERV_PTTT> vSereServPTTT { get; set; }
        public List<HIS_DHST> Dhst { get; set; }
        public List<HIS_CONFIG> Configs { get; set; }
        public List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlter { get;set;}
        public List<HIS_MEDI_ORG> ListHeinMediOrg { get; set; }
        public List<HIS_ICD> TotalIcdData { get; set; }
        public List<V_HIS_BABY> vBaby { get; set; }
        public List<HIS_EMPLOYEE> Employees { get; set; }
        public decimal tienThuoc { get; set; }
        public decimal tienVTYT { get; set; }
        public decimal tongchiBV { get; set; }
        public decimal tongchiBH { get; set; }
        public decimal tongBNTT { get; set; }
        public decimal tongBNCCT { get; set; }
        public decimal tongBHTT { get; set; }
        public decimal tongNguonKhac { get; set; }
        public decimal tongBHTTGDV { get; set; }
        public bool IS_3176 { get; set; }
    }
}
    

