using EMR.EFMODEL.DataModels;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    public class DocumentSignedResultDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Base64FileSigned { get; set; }
        public string DocumentCode { get; set; }

        public DocumentSignedResultDTO() { }
    }
}
