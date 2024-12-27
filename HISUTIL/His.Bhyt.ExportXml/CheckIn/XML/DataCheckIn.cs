using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CheckIn.XML
{
    [XmlRoot("DATA")]
    public class DataCheckIn
    {
        [XmlElement("HEADER", Order = 1)]
        public Header HEADER { get; set; }

        [XmlElement("BODY", Order = 2)]
        public Body BODY { get; set; }

        [XmlElement("SECURITY", Order = 3)]
        public Security SECURITY { get; set; }
    }
}
