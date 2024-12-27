using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace His.Bhyt.ExportXml.CheckIn.XML
{
    [XmlRoot("CHI_TIEU_TRANG_THAI_KCB")]
    public class DataQd130CheckIn
    {
        [XmlElement("DSACH_TRANG_THAI_KCB", Order = 1)]
        public Qd130CheckInRootList DSACH_TRANG_THAI_KCB { get; set; }

        [XmlElement("CHUKYDONVI", Order = 2)]
        public string CHUKYDONVI { get; set; }
    }

    [XmlRoot("DSACH_TRANG_THAI_KCB")]
    public class Qd130CheckInRootList
    {
        [XmlElement("TRANG_THAI_KCB")]
        public List<Qd130CheckIn> TRANG_THAI_KCB { get; set; }
    }
}
