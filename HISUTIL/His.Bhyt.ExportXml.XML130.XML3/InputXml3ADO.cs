using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML3
{
    public class InputXml3ADO
    {
        /// <summary>
        /// Danh sách cấu hình hệ thống
        /// </summary>
        public List<HIS_CONFIG> ConfigData { get; set; }

        /// <summary>
        /// Hồ sơ điều trị
        /// </summary>
        public V_HIS_TREATMENT_12 Treatment { get; set; }

        /// <summary>
        /// các dịch vụ bệnh nhân đã sử dụng
        /// </summary>
        public List<V_HIS_SERE_SERV_2> ListSereServ { get; set; }

        /// <summary>
        /// các dịch vụ PTTT
        /// </summary>
        public List<V_HIS_SERE_SERV_PTTT> SereServPttts { get; set; }

        /// <summary>
        /// Danh sach nhan vien
        /// </summary>
        public List<HIS_EMPLOYEE> Employees { get; set; }

        /// <summary>
        /// Lấy thông tin giường
        /// </summary>
        public List<V_HIS_BED_LOG> BedLogs { get; set; }

        /// <summary>
        /// Danh sách loại vật tư
        /// </summary>
        public List<HIS_MATERIAL_TYPE> MaterialTypes { get; set; }

        /// <summary>
        /// Danh sách dịch vụ
        /// </summary>
        public List<V_HIS_SERVICE> Services { get; set; }

        /// <summary>
        /// Danh sách chuẩn đoán
        /// </summary>
        public List<HIS_ICD> Icds { get; set; }

        /// <summary>
        /// lấy thông tin mã bác sĩ
        /// </summary>
        public List<HIS_EKIP_USER> EkipUsers { get; set; }

        /// <summary>
        /// Danh sách đối tượng bệnh nhân
        /// </summary>
        public List<HIS_PATIENT_TYPE> PatientTypes { get; set; }
        public List<V_HIS_SERE_SERV_TEIN> vHisSereServTeins { get; set; }
        public bool IS_3176 { get; set; }
    }
}
