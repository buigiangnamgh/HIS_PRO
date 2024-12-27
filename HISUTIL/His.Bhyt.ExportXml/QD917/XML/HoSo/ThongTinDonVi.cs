using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.HoSo
{
    [XmlRoot("THONGTINDONVI")]
    public class ThongTinDonVi
    {
        [XmlElement("MACSKCB")]
        public string MACSKCB { get; set; }
    }
}
