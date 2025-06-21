using EMR.EFMODEL.DataModels;
using EMR.TDO;
using Inventec.Common.SignLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignLibrary.ADO
{
    public class FileADO
    {
        public string Base64FileContent { get; set; }    
        public FileType FileType { get; set; }
        public bool? IsMain { get; set; }
        public FileADO() { }
    }
}
