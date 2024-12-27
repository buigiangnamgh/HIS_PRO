using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CV2076.HoSo
{
    [XmlRoot("DANHSACHHOSO")]
    public class DanhSachHoSo
    {
        [XmlElement("HOSO")]
        public List<HoSo> HOSO { get; set; }
    }
}
