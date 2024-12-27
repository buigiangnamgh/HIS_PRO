using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.QD917.XML.HoSo
{
    [XmlRoot("THONGTINHOSO")]
    public class ThongTinHoSo
    {
        [XmlElement("NGAYLAP", Order = 1)]
        public string NGAYLAP { get; set; }

        [XmlElement("SOLUONGHOSO", Order = 2)]
        public int SOLUONGHOSO { get; set; }

        [XmlElement("DANHSACHHOSO", Order = 3)]
        public DanhSachHoSo DANHSACHHOSO { get; set; }
    }
}
