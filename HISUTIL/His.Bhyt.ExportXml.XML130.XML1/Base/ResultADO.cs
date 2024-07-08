using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace His.Bhyt.ExportXml.XML130.XML1.Base
{
    public class ResultADO
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public object[] Data { get; set; }

        public ResultADO() { }

        public ResultADO(bool success, string message, object[] data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
    }
}
