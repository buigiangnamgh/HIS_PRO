using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML4.QD130.XML
{
    [XmlRoot("CHITIEU_CHITIET_DICHVUCANLAMSANG")]
    public class XML4Data
    {
        [XmlElement("DSACH_CHI_TIET_CLS")]
        public DsChiTietDichVuCLS DSACH_CHI_TIET_CLS { get; set; }
    }
    [XmlRoot("DSACH_CHI_TIET_CLS")]
    public class DsChiTietDichVuCLS
    {
        [XmlElement("CHI_TIET_CLS")]
        public List<XML4DetailData> CHI_TIET_CLS { get; set; }
    }
}
