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
    public class SignADO
    {
        public DocumentTDO Document { get; set; }    
        public float X { get; set; }
        public float Y { get; set; }
        public int PageNumberCurrent { get; set; }
        public int TotalPageNumber { get; set; }

        public SignADO() { }
    }
}
