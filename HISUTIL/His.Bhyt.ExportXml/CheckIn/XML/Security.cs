using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CheckIn.XML
{
    [XmlRoot("SECURITY")]
    public class Security
    {
        [XmlElement("SIGNATURE")]
        public string SIGNATURE { get; set; }
    }
}
