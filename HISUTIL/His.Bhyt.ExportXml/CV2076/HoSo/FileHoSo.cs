using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CV2076.HoSo
{
    [XmlRoot("FILEHOSO")]
    public class FileHoSo
    {
        [XmlElement("LOAIHOSO", Order = 1)]
        public string LOAIHOSO { get; set; }

        [XmlElement("NOIDUNGFILE", Order = 2)]
        public string NOIDUNGFILE { get; set; }
    }
}
