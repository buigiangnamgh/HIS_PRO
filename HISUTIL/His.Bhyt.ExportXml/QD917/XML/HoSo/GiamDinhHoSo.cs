using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.HoSo
{
    [XmlRoot("GIAMDINHHS")]
    public class GiamDinhHoSo
    {
        [XmlElement("THONGTINDONVI", Order = 1)]
        public ThongTinDonVi THONGTINDONVI { get; set; }

        [XmlElement("THONGTINHOSO", Order = 2)]
        public ThongTinHoSo THONGTINHOSO { get; set; }

        [XmlElement("CHUKYDONVI", Order = 3)]
        public string CHUKYDONVI { get; set; }
    }
}
