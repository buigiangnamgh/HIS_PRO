using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CV2076.HoSo
{
    [XmlRoot("THONGTINDONVI")]
    public class ThongTinDonVi
    {
        [XmlElement("MACSKCB", Order = 1)]
        public string MACSKCB { get; set; }

        [XmlElement("CHUKYDONVI", Order = 2)]
        public string CHUKYDONVI { get; set; }
    }
}
