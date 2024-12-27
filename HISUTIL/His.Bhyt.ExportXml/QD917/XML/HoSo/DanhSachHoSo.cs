using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.HoSo
{
    [XmlRoot("DANHSACHHOSO")]
    public class DanhSachHoSo
    {
        [XmlElement("HOSO")]
        public HoSo HOSO { get; set; }
    }
}
