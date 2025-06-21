using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventec.Common.SignLibrary.DTO
{
    public class DocumentSignedUpdateIGSysResultDTO
    {
        // Methods
        public DocumentSignedUpdateIGSysResultDTO() { }

        // Properties    
        public string DocumentCode { get; set; }
        public string token { get; set; }
        public string DocumentTypeCode { get; set; }
        public string DocumentName { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string HisCode { get; set; }
        public string NgayKy { get; set; }
        public string NguoiKy { get; set; }
        /// <summary>
        /// 00: thanh cong, khác: that bai
        /// </summary>
        public string MaLoi { get; set; }
    }
}
