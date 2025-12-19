using Inventec.Common.SignFile.XmlProcess.XmlDsig.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventec.Common.SignFile
{
    public class XmlConfig
    {
        public XmlConfig() { }

        public string NodeToSign { get; set; }
        public string Reason { get; set; }
        public string NameIDTimeSignature { get; set; }
        public XmlDsigSignatureFormat SigningType { get; set; }
        public DateTime SigningTime { get; set; }

    }
}
