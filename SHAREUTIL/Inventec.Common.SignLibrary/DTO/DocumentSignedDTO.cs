using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventec.Common.SignLibrary.DTO
{
    public class DocumentSignedDTO
    {
        // Methods
        public DocumentSignedDTO() { }

        // Properties    
        public string DocumentTypeCode { get; set; }
        public string HisCode { get; set; }
        public string TreatmentCode { get; set; }
        public bool? IsPrintOnlyContent { get; set; }
    }
}
