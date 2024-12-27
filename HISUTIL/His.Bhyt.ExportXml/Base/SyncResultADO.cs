using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.Base
{
    public class SyncResultADO
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public bool Success { get; set; }
        public string CheckCode { get; set; }
    }
}
