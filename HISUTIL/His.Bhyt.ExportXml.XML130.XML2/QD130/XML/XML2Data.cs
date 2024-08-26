using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.XML130.XML2.QD130.XML
{
    [XmlRoot("CHITIEU_CHITIET_THUOC")]
    public class XML2Data
    {
        [XmlElement("DSACH_CHI_TIET_THUOC")]
        public DsChiTietThuoc DSACH_CHI_TIET_THUOC { get; set; }
    }

    [XmlRoot("DSACH_CHI_TIET_THUOC")]
    public class DsChiTietThuoc
    {
        [XmlElement("CHI_TIET_THUOC")]
        public List<XML2DetailData> CHI_TIET_THUOC { get; set; }
    }
}
