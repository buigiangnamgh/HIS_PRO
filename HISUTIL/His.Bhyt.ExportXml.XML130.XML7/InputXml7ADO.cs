using MOS.EFMODEL.DataModels;
using System.Collections.Generic;

namespace His.Bhyt.ExportXml.XML130.XML7
{
    public class InputXml7ADO
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
        /// Diện điều trị cuối cùng
        /// </summary>
        public V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter { get; set; }

        /// <summary>
        /// Danh sach nhan vien
        /// </summary>
        public List<HIS_EMPLOYEE> Employees { get; set; }

    }
}
