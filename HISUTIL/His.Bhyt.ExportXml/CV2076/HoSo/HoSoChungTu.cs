using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CV2076.HoSo
{
    [XmlRoot("HSCHUNGTU")]
    public class HoSoChungTu
    {
        [XmlElement("THONGTINDONVI", Order = 1)]
        public ThongTinDonVi THONGTINDONVI { get; set; }

        [XmlElement("THONGTINHOSO", Order = 2)]
        public ThongTinHoSo THONGTINHOSO { get; set; }
    }
}
